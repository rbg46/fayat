using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.IO;
using Fred.Business.Organisation.Tree;
using Fred.Business.RoleFonctionnalite;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.RoleFonctionnalite;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.Security;
using static Fred.Entities.Constantes;
using Fred.Web.Models.Referential;
using System.Configuration;

namespace Fred.Business.Referential
{
    public class EtablissementComptableManager : Manager<EtablissementComptableEnt, IEtablissementComptableRepository>, IEtablissementComptableManager
    {
        private readonly IOrganisationRepository organisationRepository;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IRoleFonctionnaliteManager roleFonctionnaliteManager;
        private readonly ISecurityManager securityManager;
        private readonly ISocieteManager societeManager;
        private readonly IUtilisateurManager utilisateurManager;
        private UtilisateurEnt utilisateurCourant;
        private readonly string cgaFolder = ConfigurationManager.AppSettings["cga:folder"];

        public EtablissementComptableManager(
            IUnitOfWork uow,
            IEtablissementComptableRepository etablissementComptableRepository,
            IUtilisateurManager utilisateurManager,
            ISecurityManager securityManager,
            ISocieteManager societeManager,
            IOrganisationRepository organisationRepository,
            IOrganisationTreeService organisationTreeService,
            IRoleFonctionnaliteManager roleFonctionnaliteManager)
            : base(uow, etablissementComptableRepository)
        {
            this.organisationRepository = organisationRepository;
            this.organisationTreeService = organisationTreeService;
            this.roleFonctionnaliteManager = roleFonctionnaliteManager;
            this.societeManager = societeManager;
            this.organisationTreeService = organisationTreeService;
            this.roleFonctionnaliteManager = roleFonctionnaliteManager;
            this.utilisateurManager = utilisateurManager;
            this.securityManager = securityManager;
        }

        /// <summary>
        /// Utilisateur Courant
        /// </summary>
        private UtilisateurEnt UtilisateurCourant
        {
            get
            {
                if (this.utilisateurCourant == null)
                {
                    this.utilisateurCourant = this.utilisateurManager.GetContextUtilisateur();
                }
                return utilisateurCourant;
            }
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un code absence.
        /// </summary>
        /// <param name="text">Texte recherché dans les codes absences</param>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <returns>Retourne la condition de recherche des codes absences</returns>
        private Expression<Func<EtablissementComptableEnt, bool>> GetPredicate(string text, SearchEtablissementComptableEnt filters)
        {
            return p => (string.IsNullOrEmpty(text)
                         || filters.Code && p.Code.ToLower().Contains(text.ToLower())
                         || filters.Libelle && p.Libelle.ToLower().Contains(text.ToLower()))
                        && (!filters.IsDeleted || p.IsDeleted);
        }

        /// <summary>
        ///   Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        public override void CheckAccessToEntity(EtablissementComptableEnt entity)
        {
            int userId = securityManager.GetUtilisateurId();
            if (entity.Organisation == null)
            {
                this.Repository.PerformEagerLoading(entity, ci => ci.Organisation);
            }
            int idTypeEtabCompt = organisationRepository.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeEtablissement);
            var orgaList = organisationRepository.GetOrganisationsAvailable(null, new List<int> { idTypeEtabCompt }, userId);
            if (orgaList.All(o => o.OrganisationId != entity.Organisation.OrganisationId))
            {
                throw new UnauthorizedAccessException(BusinessResources.PasLeDroitDaccederACetteEntite);
            }
        }

        /// <summary>
        ///   Retourne la liste des établissements comptables.
        /// </summary>
        /// <returns>Liste des établissements comptables.</returns>
        public IEnumerable<EtablissementComptableEnt> GetEtablissementComptableList()
        {
            return this.Repository.GetEtablissementComptableList() ?? new EtablissementComptableEnt[] { };
        }

        /// <summary>
        ///   Retourne l'établissement comptable dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="etablissementComptableId">Identifiant de l'établissement comptable à retrouver.</param>
        /// <returns>L'établissement comptable retrouvé, sinon nulle.</returns>
        public EtablissementComptableEnt GetEtablissementComptableById(int etablissementComptableId)
        {
            return this.Repository.GetEtablissementComptableById(etablissementComptableId);
        }

        /// <summary>
        ///   Retourne l'identifiant de l'établissement comptable portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de l'établissement comptable dont l'identifiant est à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon nulle.</returns>
        public int? GetEtablissementComptableIdByCode(string code)
        {
            return this.Repository.GetEtablissementComptableIdByCode(code);
        }

        /// <summary>
        ///   Vérifie la validité et enregistre un établissement comptable importé depuis ANAËL Finances
        /// </summary>
        /// <param name="etablissements">Liste des entités dont il faut vérifier la validité</param>
        /// <returns>Retourne vrai l'importation a réussi</returns>
        public bool ManageImportedEtablissementComptables(IEnumerable<EtablissementComptableEnt> etablissements)
        {
            bool success = false;

            foreach (EtablissementComptableEnt ets in etablissements.ToList())
            {
                // Vérification des paramètres
                if (ets == null)
                {
                    return success;
                }
                if (!ets.SocieteId.HasValue)
                {
                    return success;
                }

                // Test d'existence gràce au code établissement pour la société
                if (!this.Repository.IsEtablissementComptableExistsByCode(0, ets.Code, ets.SocieteId.Value))
                {
                    //Itération sur toutes les propriétés de type string afin de supprimer d'éventuelles espaces à la fin des chaînes.
                    var properties = typeof(EtablissementComptableEnt).GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.PropertyType == typeof(string) && property.CanWrite)
                        {
                            string value = (string)property.GetValue(ets, null);
                            property.SetValue(ets, value?.TrimEnd());
                        }
                    }

                    // Création de l'établissement comptable
                    AddEtablissementComptable(ets);
                    success = true;
                }
            }
            return success;
        }

        /// <summary>
        ///   Ajoute un nouvel établissement comptable
        /// </summary>
        /// <param name="etablissementComptableEnt">Etablissement comptable à ajouter</param>
        /// <returns>L'identifiant de l'établissement comptable ajouté</returns>
        public int AddEtablissementComptable(EtablissementComptableEnt etablissementComptableEnt)
        {
            if (UtilisateurCourant != null)
            {
                etablissementComptableEnt.AuteurCreationId = UtilisateurCourant.UtilisateurId;
            }
            etablissementComptableEnt.DateCreation = DateTime.UtcNow;

            etablissementComptableEnt.Organisation = organisationRepository.GenerateOrganisation(Constantes.OrganisationType.CodeEtablissement,
                                                                                        societeManager.GetOrganisationIdBySocieteId(etablissementComptableEnt.SocieteId).Value);
            if (etablissementComptableEnt.Organisation != null)
            {
                return this.Repository.AddEtablissementComptable(etablissementComptableEnt);
            }

            return -1;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un établissement comptable
        /// </summary>
        /// <param name="etablissementComptableEnt">Etablissement comptable à modifier</param>
        public void UpdateEtablissementComptable(EtablissementComptableEnt etablissementComptableEnt)
        {
            if (!etablissementComptableEnt.IsDeleted)
            {
                if (UtilisateurCourant != null)
                {
                    etablissementComptableEnt.AuteurModificationId = UtilisateurCourant.UtilisateurId;
                }
                etablissementComptableEnt.DateModification = DateTime.UtcNow;
            }
            else
            {
                if (UtilisateurCourant != null)
                {
                    etablissementComptableEnt.AuteurSuppressionId = UtilisateurCourant.UtilisateurId;
                }
                etablissementComptableEnt.DateSuppression = DateTime.UtcNow;
            }

            this.Repository.UpdateEtablissementComptable(etablissementComptableEnt);
        }

        /// <summary>
        ///   Supprime un établissement comptable
        /// </summary>
        /// <param name="etablissementComptableEnt">L'établissement comptable à supprimer</param>
        public void DeleteEtablissementComptableById(EtablissementComptableEnt etablissementComptableEnt)
        {
            try
            {
                if (this.Repository.IsDeletable(etablissementComptableEnt))
                {
                    var orga = this.Repository.GetOrganisationByEtablissementId(etablissementComptableEnt.EtablissementComptableId);
                    if (orga != null)
                    {
                        etablissementComptableEnt.Organisation = null;
                        this.Repository.Delete(etablissementComptableEnt);
                        organisationRepository.DeleteOrganisationById(orga.OrganisationId);
                        Save();
                    }
                }
                else
                {
                    throw new FredBusinessException("Impossible de supprimer cet élément car il est déjà utilisé.");
                }
            }
            catch (FredRepositoryException repoEx)
            {
                throw new FredBusinessException(repoEx.Message);
            }
        }

        /// <summary>
        ///   Etablissement via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <returns>Renvoie une liste de EtablissementCompteble</returns>
        public IEnumerable<EtablissementComptableEnt> GetListBySocieteId(int societeId)
        {
            return this.Repository.GetListBySocieteId(societeId);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements compltables en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les établissements compltables</param>
        /// <returns>Retourne la liste filtrée de tous les établissements compltables</returns>
        public IEnumerable<EtablissementComptableEnt> SearchListAllWithFilters(Expression<Func<EtablissementComptableEnt, bool>> predicate)
        {
            return this.Repository.SearchListAllWithFilters(predicate);
        }

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance d'établissement comptable.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Nouvelle instance d'établissement de paie intialisée</returns>
        public EtablissementComptableEnt GetNewEtablissementComptable(int societeId)
        {
            return new EtablissementComptableEnt
            {
                Code = string.Empty,
                Libelle = string.Empty,
                Adresse = string.Empty,
                Ville = string.Empty,
                CodePostal = string.Empty,
                PaysId = null,
                ModuleCommandeEnabled = false,
                DateCreation = DateTime.UtcNow,
                DateModification = null,
                DateSuppression = null,
                AuteurCreationId = null,
                AuteurModificationId = null,
                AuteurSuppressionId = null,
                ModuleProductionEnabled = false,
                IsDeleted = false,
                SocieteId = societeId
            };
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements comptables en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="filters">Filtres de recherche sur tous les établissements comptables</param>
        /// <param name="societeId">Id de la societe</param>
        /// <param name="text">text de recherche</param>
        /// <returns>Retourne la liste filtrée de tous les établissements comptables</returns>
        public IEnumerable<EtablissementComptableEnt> SearchEtablissementComptableAllBySocieteIdWithFilters(SearchEtablissementComptableEnt filters, int societeId, string text)
        {
            return this.Repository.SearchEtablissementComptableAllBySocieteIdWithFilters(GetPredicate(text, filters), societeId);
        }

        /// <summary>
        ///   Permet de recupérer l'organisation de l'établissement comptable
        /// </summary>
        /// <param name="etablissementId">L'iD de etablissementId</param>
        /// <returns>l'organisation de l'établissement comptable</returns>
        public OrganisationEnt GetOrganisationByEtablissementId(int etablissementId)
        {
            return this.Repository.GetOrganisationByEtablissementId(etablissementId);
        }

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié à un établissement comptable
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'un établissement comptable</returns>
        public SearchEtablissementComptableEnt GetDefaultFilter()
        {
            var recherche = new SearchEtablissementComptableEnt();
            recherche.Code = true;
            recherche.Libelle = true;
            return recherche;
        }

        /// <summary>
        ///   Permet de connaître l'existence d'une établissement comptable depuis son code.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="codeEtablissementComptable"> code établissement comptable</param>
        /// <param name="societeId">Id de la société</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsEtablissementComptableExistsByCode(int idCourant, string codeEtablissementComptable, int societeId)
        {
            return this.Repository.IsEtablissementComptableExistsByCode(idCourant, codeEtablissementComptable, societeId);
        }

        /// <inheritdoc />
        public IEnumerable<EtablissementComptableEnt> SearchLight(int page = 1, int pageSize = 20, string recherche = "", int? societeId = null, bool showAllOrganisations = false, int? organisationId = null)
        {
            if (organisationId.HasValue && organisationId.Value > 0)
            {
                SocieteEnt societe = societeManager.GetSocieteByOrganisationId(organisationId.Value);
                societeId = societe != null ? societe.SocieteId : societeId;
            }

            OrganisationTree globalOrganisationTree = organisationTreeService.GetOrganisationTree();
            List<int> userEtablissementsComptables = globalOrganisationTree.GetAllEtablissementComptableForUser(this.UtilisateurCourant.UtilisateurId);

            var query = this.Repository
                      .Query()
                      .Include(x => x.Organisation)
                      .Filter(e => !e.IsDeleted)
                      .Filter(e => string.IsNullOrEmpty(recherche) || e.Code.ToLower().Contains(recherche.ToLower())
                                   || e.Libelle.ToLower().Contains(recherche.ToLower()))
                      .Filter(e => !societeId.HasValue || e.SocieteId == societeId)
                      .Filter(e => showAllOrganisations || userEtablissementsComptables.Contains(e.Organisation.OrganisationId))
                      .OrderBy(list => list.OrderBy(e => e.Code))
                      .GetPage(page, pageSize);

            return query.ToList();
        }

        /// <summary>
        /// Retourne l'établissement parent de l'organisation passée en paramètre.
        /// </summary>
        /// <param name="organisationId">L'identifiant de l'organisation.</param>
        /// <returns>L'établissement parent de l'organisation passée en paramètre ou null.</returns>
        public EtablissementComptableEnt GetEtablissementComptableByOrganisationIdEx(int organisationId)
        {
            return Repository.GetEtablissementComptableByOrganisationIdEx(organisationId);
        }

        /// <summary>
        /// Permet d'obtenir la liste des établissements de GFES
        /// </summary>
        /// <returns>liste des étblissements GFES</returns>
        public List<EtablissementComptableEnt> GetEtablissementComptableGFESList()
        {
            return Repository.GetEtablissementComptableGFESList().ToList();
        }

        /// <summary>
        /// Ge current user etab comptable without parent tree
        /// </summary>
        /// <returns>liste des étblissements comptables</returns>
        public async Task<IEnumerable<EtablissementComptableEnt>> GeCurrentUserEtabComptableWithOrganisationPartentId(int page, int pageSize, string text, int? organisationPereId)
        {
            int utilisateurId = UtilisateurCourant.UtilisateurId;
            List<EtablissementComptableEnt> etabComptableList = new List<EtablissementComptableEnt>();
            List<RoleFonctionnaliteEnt> roleFonctionnalites = await roleFonctionnaliteManager.GetRoleFonctionnaliteByUserIdAsync(utilisateurId, Constantes.FonctionnaliteLibelle.ExportAnalytiqueBoutons).ConfigureAwait(false);
            if (roleFonctionnalites.Any())
            {
                IEnumerable<OrganisationEnt> organisationList = roleFonctionnalites.Where(x => x.Role.AffectationSeuilUtilisateurs.Any()).SelectMany(x => x.Role.AffectationSeuilUtilisateurs)
                                                                                   .Where(x => x.UtilisateurId == utilisateurId).Select(x => x.Organisation);
                if (organisationList.Any())
                {
                    IEnumerable<SocieteEnt> listSocietesNv1 = organisationList.Where(x => x.TypeOrganisation.Code.Equals(Constantes.OrganisationType.CodeSociete)).Select(x => x.Societe);
                    IEnumerable<OrganisationEnt> etabComptableOrganisationList = organisationList.Where(x => x.TypeOrganisation.Code.Equals(Constantes.OrganisationType.CodeEtablissement));
                    IEnumerable<int> organisationPereIdList = new List<int>();
                    if (organisationPereId.HasValue)
                    {
                        SocieteEnt societe = listSocietesNv1?.FirstOrDefault(x => x.OrganisationId == organisationPereId.Value);
                        if (societe != null)
                        {
                            organisationPereIdList = new List<int> { organisationPereId.Value };
                            etabComptableList = await Repository.GetEtabComptableListByOrganisationPereIdList(organisationPereIdList).ConfigureAwait(false);
                        }
                        else
                        {
                            IEnumerable<int> etabComptableIdList = etabComptableOrganisationList?.Where(x => x.PereId.HasValue && x.PereId == organisationPereId).Select(x => x.OrganisationId);
                            if (etabComptableIdList.Any())
                            {
                                etabComptableList = await Repository.GetEtabComptableListByOrganisationListId(etabComptableIdList).ConfigureAwait(false);
                            }
                        }
                    }
                    else
                    {
                        organisationPereIdList = listSocietesNv1?.Select(x => x.OrganisationId);
                        if (organisationPereIdList.Any())
                        {
                            etabComptableList = await Repository.GetEtabComptableListByOrganisationPereIdList(organisationPereIdList).ConfigureAwait(false);
                        }

                        IEnumerable<int> etabComptableIdList = etabComptableOrganisationList?.Select(x => x.OrganisationId);
                        if (etabComptableIdList.Any())
                        {
                            IEnumerable<EtablissementComptableEnt> etabComptables = await Repository.GetEtabComptableListByOrganisationListId(etabComptableIdList).ConfigureAwait(false);
                            etabComptableList.AddRange(etabComptables);
                        }
                    }
                }
            }

            return etabComptableList.Distinct().Where(o => FilterText(text, o)).OrderBy(x => x.Code).Skip((page - 1) * pageSize).Take(pageSize);
        }

        private bool FilterText(string text, EtablissementComptableEnt o)
        {
            if (string.IsNullOrEmpty(text) || (o.Libelle != null && ComparatorHelper.ComplexContains(o.Libelle, text)) ||

               (o.Code != null && ComparatorHelper.ComplexContains(o.Code, text)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Permet d'obtenir la liste des établissements par organisation id
        /// </summary>
        /// <param name="organisationIds">Liste des identifiants</param>
        /// <returns>liste des étblissements</returns>
        public IEnumerable<EtablissementComptableEnt> GetEtablissementComptableByOrganisationIds(List<int> organisationIds)
        {
            return Repository.GetEtablissementComptableByOrganisationIds(organisationIds);
        }

        /// <summary>
        ///   Upload CGA Files
        /// </summary>
        /// <param name="file">File and FileName of CGA</param>
        public void UploadCGA(string cgaFournitureFileDoc, string cgaLocationFileDoc, string cgaPrestationFileDoc, string cgaFournitureFileName, string cgaLocationFileName, string cgaPrestationFileName)
        {
            if (cgaFournitureFileDoc != null)
            {
                AppendContentToCGAFile(cgaFournitureFileName, cgaFournitureFileDoc, cgaFolder);
            }
            if (cgaLocationFileDoc != null)
            {
                AppendContentToCGAFile(cgaLocationFileName, cgaLocationFileDoc, cgaFolder);
            }
            if (cgaPrestationFileDoc != null)
            {
                AppendContentToCGAFile(cgaPrestationFileName, cgaPrestationFileDoc, cgaFolder);
            }
        }

        /// <summary>
        /// Append cga file to cga file doc 
        /// </summary>
        /// <param name="cgaFileName"></param>
        /// <param name="cgaFileDoc"></param>
        /// <param name="directoryCGAPath"></param>
        private void AppendContentToCGAFile(string cgaFileName,string cgaFileDoc,string directoryCGAPath)
        {
            string filePath = directoryCGAPath + cgaFileName + BondeCommande.CGAExtension;
            cgaFileDoc = cgaFileDoc.Substring(cgaFileDoc.IndexOf(',') + 1);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllBytes(filePath, Convert.FromBase64String(cgaFileDoc));
        }

        /// <summary>
        ///   Upload CGA Files
        /// </summary>
        /// <param name="file">File and FileName of CGA</param>
        public bool IsCGAFileExist(string cgaFileName)
        {
            bool success = false;
            string filePath = cgaFolder + cgaFileName;

            if (File.Exists(filePath))
            {
                success = true;
            }

            return success;
        }

        public IEnumerable<EtablissementComptableEnt> GetListWithCGA(IEnumerable<EtablissementComptableEnt> ecList)
        {
            foreach (var item in ecList.ToList())
            {
                item.CGAFournitureFileName = $"{item.SocieteId}_{item.Code}{BondeCommande.CGAFournitureSuffixe}{BondeCommande.CGAExtension}";
                item.CGALocationFileName = $"{item.SocieteId}_{item.Code}{BondeCommande.CGALocationSuffixe}{BondeCommande.CGAExtension}";
                item.CGAPrestationFileName = $"{item.SocieteId}_{item.Code}{BondeCommande.CGAPrestationSuffixe}{BondeCommande.CGAExtension}";
                item.CGAFournitureFilePath = $"{cgaFolder}{item.CGAFournitureFileName}";
                item.CGALocationFilePath = $"{cgaFolder}{item.CGALocationFileName}";
                item.CGAPrestationFilePath = $"{cgaFolder}{item.CGAPrestationFileName}";
                if (!IsCGAFileExist(item.CGAFournitureFileName)) { item.CGAFournitureFileName = null; item.CGAFournitureFilePath = null; }
                if (!IsCGAFileExist(item.CGALocationFileName)) { item.CGALocationFileName = null; item.CGALocationFilePath = null; }
                if (!IsCGAFileExist(item.CGAPrestationFileName)) { item.CGAPrestationFileName = null; item.CGAPrestationFilePath = null; }
            }
            return ecList;
        }

        public EtablissementComptableModel GetEtablissementComptableWithCGA(EtablissementComptableModel etabModel)
        {
            if (etabModel != null)
            {
                etabModel.CGAFournitureFileName = $"{etabModel.SocieteId}_{etabModel.Code}{BondeCommande.CGAFournitureSuffixe}{BondeCommande.CGAExtension}";
                etabModel.CGALocationFileName = $"{etabModel.SocieteId}_{etabModel.Code}{BondeCommande.CGALocationSuffixe}{BondeCommande.CGAExtension}";
                etabModel.CGAPrestationFileName = $"{etabModel.SocieteId}_{etabModel.Code}{BondeCommande.CGAPrestationSuffixe}{BondeCommande.CGAExtension}";
                etabModel.CGAFournitureFilePath = $"{cgaFolder}{etabModel.CGAFournitureFileName}";
                etabModel.CGALocationFilePath = $"{cgaFolder}{etabModel.CGALocationFileName}";
                etabModel.CGAPrestationFilePath = $"{cgaFolder}{etabModel.CGAPrestationFileName}";
                if (!IsCGAFileExist(etabModel.CGAFournitureFileName)) { etabModel.CGAFournitureFileName = null; etabModel.CGAFournitureFilePath = null; }
                if (!IsCGAFileExist(etabModel.CGALocationFileName)) { etabModel.CGALocationFileName = null; etabModel.CGALocationFilePath = null; }
                if (!IsCGAFileExist(etabModel.CGAPrestationFileName)) { etabModel.CGAPrestationFileName = null; etabModel.CGAPrestationFilePath = null; }
            }
            return etabModel;
        }
    }
}
