using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Fred.Business.Organisation.Tree;
using Fred.Business.Personnel.Extensions;
using Fred.Business.Personnel.Mapper;
using Fred.Business.Personnel.Models;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;
using Fred.Framework.Reporting;
using Fred.Web.Shared.Models.Personnel.Excel;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;

namespace Fred.Business.Personnel.Excel
{
    /// <summary>
    /// Implémentation de l'IPersonnelExcelManager
    /// </summary>
    public class PersonnelExcelManager : Manager<PersonnelEnt, IPersonnelRepository>, IPersonnelExcelManager
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IAffectationToExcelModelMapper affectationToExcelModelMapper;

        private UtilisateurEnt currentUser;

        public PersonnelExcelManager(
            IUnitOfWork uow,
            IPersonnelRepository personnelRepository,
            IUtilisateurManager utilisateurManager,
            IOrganisationTreeService organisationTreeService,
            IAffectationToExcelModelMapper affectationToExcelModelMapper)
            : base(uow, personnelRepository)
        {
            this.utilisateurManager = utilisateurManager;
            this.organisationTreeService = organisationTreeService;
            this.affectationToExcelModelMapper = affectationToExcelModelMapper;
        }

        /// <summary>
        /// Utilisateur Courant
        /// </summary>
        private UtilisateurEnt CurrentUser
        {
            get
            {
                return this.currentUser ?? (this.currentUser = this.utilisateurManager.GetContextUtilisateur());
            }
        }

        /// <inheritdoc/>
        public byte[] GetExportExcel(SearchPersonnelEnt filter, bool haveHabilitation)
        {
            string ExcelTemplate = "Templates/Personnel/TemplatePersonnel.xlsx";
            string ExcelTemplateWithHabilitation = "Templates/Personnel/TemplatePersonnelWithHabilitation.xlsx";
            string selectedTemplate = ExcelTemplate;
            //Filtre additionnel aussi ajouté par le personnel Manager, mais pas géré par le PredicateWhere
            int? currentUserGroupeId = this.CurrentUser?.Personnel?.Societe?.GroupeId;
            string codeGroupe = this.CurrentUser?.Personnel?.Societe?.Groupe?.Code;
            var allPersonnel = Repository.GetPersonnellListForExportExcel(filter, this.CurrentUser.SuperAdmin, currentUserGroupeId).ToList();

            var excelModel = allPersonnel.Select(p => new PersonnelExcelModel()
            {
                Societe = p.Societe?.Libelle,
                Matricule = p.Matricule,
                Nom = p.Nom,
                Prenom = p.Prenom,
                EtablissementPaie = p.EtablissementPaie?.Libelle,
                IsActif = GetOorNStringFromBool(p.GetPersonnelIsActive()),
                IsInterne = GetOorNStringFromBool(p.IsInterne),
                IsInterimaire = GetOorNStringFromBool(p.IsInterimaire),
                IsUtilisateur = GetOorNStringFromBool(p.IsUtilisateur()),
                DateEntree = p?.DateEntree != null ? p?.DateEntree?.ToString("dd/MM/yyyy") : string.Empty,
                DateSortie = p?.DateSortie != null ? p?.DateSortie?.ToString("dd/MM/yyyy") : string.Empty,
                Statut = p.GetPersonnelStatutString(),
                Ressource = p.Ressource?.Libelle,
                Adresse1 = p.Adresse1,
                Adresse2 = p.Adresse2,
                Adresse3 = p.Adresse3,
                CodePostal = p.CodePostal,
                Ville = p.Ville,
                Pays = p.Pays?.Libelle,
                Email = p.Email
            }).ToList();

            if (haveHabilitation)
            {
                List<PersonnelExcelModel> personnelHabExcelModel = new List<PersonnelExcelModel>();
                UtilisateurEnt user = this.currentUser;
                bool isGRZB = user.Personnel?.Societe?.Groupe.Code == Constantes.CodeGroupeRZB;
                OrganisationTree userOrganisations = null;

                if (isGRZB)
                {
                    userOrganisations = GetOrganisationTreeByUserId(this.currentUser.UtilisateurId);
                }
                else
                {
                    userOrganisations = GetOrganisationsTree(allPersonnel);
                }

                var affectations = GetAffectationsOfOrganisationTree(userOrganisations);
                personnelHabExcelModel.AddRange(affectationToExcelModelMapper.TransformPersonnelWithHabilitation(affectations));
                var personnelHabExcelModelIds = personnelHabExcelModel.Select(x => x.Matricule).Distinct().ToList();
                if (personnelHabExcelModel.Any() && personnelHabExcelModelIds.Any())
                {
                    excelModel.RemoveAll(x => personnelHabExcelModelIds.Contains(x.Matricule));
                    excelModel.AddRange(personnelHabExcelModel);
                }

                excelModel = excelModel.OrderBy(x => x.Nom).ToList();
                selectedTemplate = ExcelTemplateWithHabilitation;
            }

            using (var excelFormat = new ExcelFormat())
            {
                var excelByte = excelFormat.GenerateExcel(AppDomain.CurrentDomain.BaseDirectory + selectedTemplate, excelModel, null, null, true);
                var memoryStream = new MemoryStream(excelByte);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// ajouter l'excel généré dans le cache et retourner la clé de cache
        /// </summary>
        /// <param name="utilisateurID">id d'utilsateur</param>
        /// <returns>return la clé de cache</returns>
        public string AddGeneratedExcelStreamToCache(int utilisateurID, string templateFolderPath)
        {
            MemoryStream stream = GenerateExcelStream(utilisateurID, templateFolderPath);
            return AddToCacheThenGetCacheId(stream);
        }

        private string GetOorNStringFromBool(bool value)
        {
            return value ? "O" : "N";
        }

        private string AddToCacheThenGetCacheId(MemoryStream stream)
        {
            const string cacheType = "excelBytes_";
            stream.Position = 0;
            var excelBytes = new byte[stream.Length];
            stream.Read(excelBytes, 0, (int)stream.Length);

            string cacheId = Guid.NewGuid().ToString();
            string cacheKey = cacheType + cacheId;

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
            MemoryCache.Default.Add(cacheKey, excelBytes, policy);

            return cacheId;
        }

        /// <summary>
        /// Genere Excel stream 
        /// </summary>
        /// <param name="utilisateurID">id d'utilisateur</param>
        /// <returns>MemoryStream</returns>
        private MemoryStream GenerateExcelStream(int utilisateurID, string templateFolderPath)
        {
            UtilisateurEnt selectedUser = utilisateurManager.GetUtilisateurById(utilisateurID);
            string pathName = Path.Combine(templateFolderPath, "Personnel/TemplateHabilitationsUtilisateurs.xls");
            ExcelFormat excelFormat = new ExcelFormat();
            var entete = new
            {
                Utilisateur = string.Format("{0} - {1}", selectedUser.Personnel.Societe?.Code, selectedUser.Personnel.CodeNomPrenom),
                DateEdition = DateTime.UtcNow.ToString("dd/MM/yyyy")
            };
            var listeUtilisateursExcelModel = new List<HabilitationsUtilisateursExcelModel>();

            // RG_5279_003 : périmètre d’habilitation de l’utilisateur sélectionné
            OrganisationTree userOrganisations = GetOrganisationTreeByUserId(utilisateurID);

            var affectations = GetAffectationsOfOrganisationTree(userOrganisations);

            listeUtilisateursExcelModel.AddRange(affectationToExcelModelMapper.Transform(affectations));

            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("Entete", entete);
            excelFormat.AddVariable("lignes", listeUtilisateursExcelModel);
            excelFormat.ApplyVariables();
            MemoryStream stream = GeneratePdfOrExcel(false, excelFormat, workbook);

            return stream;
        }

        /// <summary>
        /// Genere un PDF ou Excel à partir d'un MemoryStream
        /// </summary>
        /// <param name="pdf">True si on doit générer un PDF</param>
        /// <param name="excelFormat">Formattage Excel</param>
        /// <param name="workbook">Objet Excel Workbook</param>
        /// <returns>MemoryStream</returns>
        private MemoryStream GeneratePdfOrExcel(bool pdf, ExcelFormat excelFormat, IWorkbook workbook)
        {
            MemoryStream stream = new MemoryStream();
            if (pdf)
            {
                PdfDocument pdfDoc = excelFormat.PrintExcelToPdf(workbook);
                string cacheId = Guid.NewGuid().ToString();
                string tempFilename = Path.Combine(Path.GetTempPath(), cacheId + ".pdf");
                using (var fileStream = new FileStream(tempFilename, FileMode.Create))
                {
                    pdfDoc.Save(fileStream);
                }
                stream = excelFormat.ChargerFichier(tempFilename);
                File.Delete(tempFilename);
                pdfDoc.Close();
            }
            else
            {
                workbook.SaveAs(stream);
            }

            workbook.Close();

            return stream;
        }

        private OrganisationTree GetOrganisationTreeByUserId(int utilisateurID)
        {
            OrganisationTree globalOrganisationTree = organisationTreeService.GetOrganisationTree();
            OrganisationTree userOrganisationTree = globalOrganisationTree.GetOrganisationTreeForUser(utilisateurID);
            userOrganisationTree.Nodes = userOrganisationTree.Nodes.Where(n => n.Data.Affectations.Count > 0).Distinct().ToList();
            return userOrganisationTree;
        }

        private OrganisationTree GetOrganisationsTree(List<PersonnelEnt> allPersonnels)
        {
            OrganisationTree globalOrganisationTree = organisationTreeService.GetOrganisationTree();
            globalOrganisationTree.Nodes = globalOrganisationTree.Nodes.Where(n => n.Data.Affectations.Count > 0 &&
                                           n.Data.Affectations.Any(aff => allPersonnels.Select(a => a.UtilisateurId)
                                           .Contains(aff.UtilisateurId))).Distinct().ToList();
            return globalOrganisationTree;
        }

        private UtilOrgaRoleLists GetAffectationsOfOrganisationTree(OrganisationTree organisation)
        {
            UtilOrgaRoleLists utilOrgaRoleLists = new UtilOrgaRoleLists
            {
                UtilisateursIds = new List<int>(),
                RolesIds = new List<int>(),
                OrganisationsIds = new List<int>(),
                OrganisationsSimples = new List<OrganisationSimple>()
            };
            foreach (var node in organisation.Nodes)
            {
                var organisationBase = node.Data;
                utilOrgaRoleLists.OrganisationsSimples.Add(
                      new OrganisationSimple
                      {
                          OrganisationId = organisationBase.OrganisationId,
                          Id = organisationBase.Id,
                          Code = organisationBase.Code,
                          Libelle = organisationBase.Libelle
                      }
                      );
                foreach (var affectation in node.Data.Affectations)
                {
                    AddIfNotExist(utilOrgaRoleLists.UtilisateursIds, affectation.UtilisateurId);
                    AddIfNotExist(utilOrgaRoleLists.OrganisationsIds, affectation.OrganisationId);
                    AddIfNotExist(utilOrgaRoleLists.RolesIds, affectation.RoleId);
                }
            }
            return utilOrgaRoleLists;
        }

        private void AddIfNotExist(List<int> list, int item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
    }
}
