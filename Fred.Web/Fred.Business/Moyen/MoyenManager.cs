using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Fred.Business.AffectationMoyen;
using Fred.Business.Moyen.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Reporting;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Shared.Comparer;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.Moyen;
using Syncfusion.XlsIO;
namespace Fred.Business.Moyen
{
    /// <summary>
    /// Gestionnaire des moyens
    /// </summary>
    public class MoyenManager : Manager<MaterielEnt, IMaterielRepository>, IMoyenManager
    {
        private readonly MoyenPointageHelper moyenPointageHelper;
        private readonly IAffectationMoyenManager affectationMoyenManager;
        private readonly IMapper mapper;
        private readonly IMaterielLocationManager materielLocationManager;
        private readonly IRapportMoyenExtractionService rapportMoyenExtractionService;
        private readonly IUpdatePointageMoyenInputBuilder updateMoyenRequestBuilder;
        private readonly IRapportMoyenService rapportMoyenService;
        private readonly IDateTimeExtendManager dateTimeExtendManager;
        private readonly IAffectationMoyenTypeRepository affectationMoyenTypeRepository;
        private readonly IChapitreRepository chapitreRepository;
        private readonly ISousChapitreRepository sousChapitreRepository;
        private readonly IRessourceRepository ressourceRepository;

        public MoyenManager(
            IUnitOfWork uow,
            IMaterielRepository materielRepository,
            IAffectationMoyenManager affectationMoyenManager,
            IMapper mapper,
            IMaterielLocationManager materielLocationManager,
            IRapportMoyenExtractionService rapportMoyenExtractionService,
            IUpdatePointageMoyenInputBuilder updateMoyenRequestBuilder,
            IRapportMoyenService rapportMoyenService,
            IDateTimeExtendManager dateTimeExtendManager,
            IAffectationMoyenTypeRepository affectationMoyenTypeRepository,
            IChapitreRepository chapitreRepository,
            ISousChapitreRepository sousChapitreRepository,
            IRessourceRepository ressourceRepository)
          : base(uow, materielRepository)
        {
            moyenPointageHelper = new MoyenPointageHelper();

            this.affectationMoyenManager = affectationMoyenManager;
            this.mapper = mapper;
            this.materielLocationManager = materielLocationManager;
            this.rapportMoyenExtractionService = rapportMoyenExtractionService;
            this.updateMoyenRequestBuilder = updateMoyenRequestBuilder;
            this.rapportMoyenService = rapportMoyenService;
            this.dateTimeExtendManager = dateTimeExtendManager;
            this.affectationMoyenTypeRepository = affectationMoyenTypeRepository;
            this.chapitreRepository = chapitreRepository;
            this.sousChapitreRepository = sousChapitreRepository;
            this.ressourceRepository = ressourceRepository;
        }

        /// <inheritdoc />
        public MaterielEnt GetMoyen(string code)
        {
            try
            {
                return Repository.GetMoyen(code);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Récupération du moyen en fonction de son code et l'id de la société
        /// </summary>
        /// <param name="code">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <returns>Un objet materiel Ent qui corresponds au code et l'id envoyés</returns>
        public MaterielEnt GetMoyen(string code, int societeId)
        {
            try
            {
                return Repository.GetMoyen(code, societeId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Récupération du moyen en fonction de son code et l'id de la société
        /// </summary>
        /// <param name="code">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <param name="etablissementComptableId">Id de l'établiessement comptable</param>
        /// <returns>Un objet materiel Ent qui corresponds au code et l'id envoyés</returns>
        public MaterielEnt GetMoyen(string code, int societeId, int? etablissementComptableId)
        {
            try
            {
                return Repository.GetMoyen(code, societeId, etablissementComptableId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public MaterielEnt AddOrUpdateMoyen(MaterielEnt materiel)
        {
            try
            {
                return Repository.AddOrUpdateMoyen(materiel);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        ///  Chercher des moyens en fonction des critéres fournies en paramètres
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>List des moyens</returns>
        public IEnumerable<MaterielEnt> SearchLightForMoyen(SearchMoyenEnt filters, int page = 1, int pageSize = 20)
        {
            try
            {
                IEnumerable<int> chapitresIds = Managers.ReferentielFixe.GetFesChapitreListMoyen();
                return Repository.SearchLightForMoyen(filters, page, pageSize, chapitresIds);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        ///  Chercher des immatriculation des moyens en fonction des critéres fournies en paramètres
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>List des immatriculations des moyens</returns>
        public IEnumerable<MoyenImmatriculationModel> SearchLightForImmatriculation(SearchImmatriculationMoyenEnt filters, int page = 1, int pageSize = 30)
        {
            try
            {
                IEnumerable<int> chapitresIds = this.Managers.ReferentielFixe.GetFesChapitreListMoyen();
                List<MoyenImmatriculationModel> materialLocations = materielLocationManager.SearchLightForImmatriculation(filters, chapitresIds, page, pageSize).ToList();
                List<MoyenImmatriculationModel> materials = Repository.SearchLightForImmatriculation(filters, chapitresIds, page, pageSize)
                  .Select(m => new MoyenImmatriculationModel
                  {
                      Immatriculation = m.Immatriculation,
                      Libelle = m.Libelle
                  }).ToList();

                materialLocations.AddRange(materials);
                return materialLocations.Distinct(new MoyenImmatriculationComparer());
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Chercher les types d'affectations des moyens en fonction des critéres fournies en paramètres 
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de la recherche</param>
        /// <returns>Liste des types des affectations</returns>
        public IEnumerable<AffectationMoyenTypeEnt> SearchLightForAffectationMoyenType(int page = 1, int pageSize = 20, string text = null)
        {
            try
            {
                return affectationMoyenTypeRepository
                .Query()
                .Filter(a => string.IsNullOrEmpty(text) || (a.Code != null && a.Code.ToUpper().Contains(text.ToUpper())) || (a.Libelle != null && a.Libelle.ToUpper().Contains(text.ToUpper())))
                .OrderBy(a => a.OrderBy(t => t.Code))
                .GetPage(page, pageSize)
                .AsEnumerable();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Chercher les types des moyens 
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de la recherche</param>
        /// <returns>Liste des types des moyens</returns>
        public IEnumerable<ChapitreEnt> SearchLightForTypeMoyen(int page = 1, int pageSize = 20, string text = null)
        {
            try
            {
                IEnumerable<int> chapitresIds = this.Managers.ReferentielFixe.GetFesChapitreListMoyen();

                return chapitreRepository
                  .Query()
                  .Filter(c => chapitresIds.Any(x => x == c.ChapitreId))
                  .Filter(c => string.IsNullOrEmpty(text) || (c.Code != null && c.Code.ToUpper().Contains(text.ToUpper())) || (c.Libelle != null && c.Libelle.ToUpper().Contains(text.ToUpper())))
                  .OrderBy(a => a.OrderBy(c => c.Code))
                  .GetPage(page, pageSize)
                  .AsEnumerable();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Chercher les sous types des moyens 
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de la recherche</param>
        /// <param name="typeMoyen">Type d'un moyen</param>
        /// <returns>Liste des sous types des moyens</returns>
        public IEnumerable<SousChapitreEnt> SearchLightForSousTypeMoyen(int page = 1, int pageSize = 20, string text = null, string typeMoyen = null)
        {
            try
            {
                IEnumerable<int> chapitresIds = this.Managers.ReferentielFixe.GetFesChapitreListMoyen();
                return sousChapitreRepository
                  .Query()
                  .Include(r => r.Chapitre)
                  .Filter(moyenPointageHelper.GetSousChapitreGroupeFilter(chapitresIds))
                  .Filter(s => string.IsNullOrEmpty(typeMoyen) || (s.Chapitre != null && s.Chapitre.Code != null && s.Chapitre.Code.Equals(typeMoyen)))
                  .Filter(s => string.IsNullOrEmpty(text) || (s.Code != null && s.Code.ToUpper().Contains(text.ToUpper())) || (s.Libelle != null && s.Libelle.ToUpper().Contains(text.ToUpper())))
                  .OrderBy(a => a.OrderBy(c => c.Code))
                  .GetPage(page, pageSize)
                  .AsEnumerable();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Chercher les modèles des moyens 
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de la recherche</param>
        /// <param name="typeMoyen">Type d'un moyen</param>
        /// <param name="sousTypeMoyen">Sous type d'un moyen</param>
        /// <returns>Liste des modèles des moyens</returns>
        public IEnumerable<RessourceEnt> SearchLightForModelMoyen(int page = 1, int pageSize = 20, string text = null, string typeMoyen = null, string sousTypeMoyen = null)
        {
            try
            {
                IEnumerable<int> chapitresIds = this.Managers.ReferentielFixe.GetFesChapitreListMoyen();
                return ressourceRepository
                  .Query()
                  .Include(s => s.SousChapitre.Chapitre)
                  .Filter(moyenPointageHelper.GetRessourceGroupeFilter(chapitresIds))
                  .Filter(s => string.IsNullOrEmpty(sousTypeMoyen) || (s.SousChapitre != null && s.SousChapitre.Code != null && s.SousChapitre.Code.Equals(sousTypeMoyen)))
                  .Filter(s => string.IsNullOrEmpty(typeMoyen) || (s.SousChapitre != null && s.SousChapitre.Chapitre != null && s.SousChapitre.Chapitre.Code != null && s.SousChapitre.Chapitre.Code.Equals(typeMoyen)))
                  .Filter(s => string.IsNullOrEmpty(text) || (s.Code != null && s.Code.ToUpper().Contains(text.ToUpper())) || (s.Libelle != null && s.Libelle.ToUpper().Contains(text.ToUpper())))
                  .OrderBy(a => a.OrderBy(c => c.Code))
                  .GetPage(page, pageSize)
                  .AsEnumerable();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Search light for fiche generique
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="text">text to use in search</param>
        /// <returns>IEnumerable fo FicheGeneriqueModel</returns>
        public IEnumerable<FicheGeneriqueModel> SearchLightForFicheGenerique(int page = 1, int pageSize = 30, string text = null)
        {
            try
            {
                List<MaterielEnt> materielList = this.Repository.GetMoyenForFicheGenerique(page, pageSize, text).ToList();
                if (materielList.IsNullOrEmpty())
                {
                    return new List<FicheGeneriqueModel>();
                }

                List<MaterielEnt> itemList = new List<MaterielEnt>();

                // Ce check est nécessaire pour éviter des éléments en double
                if (page >= 2)
                {
                    IEnumerable<int> previousMoyenList = Repository.GetMoyenForFicheGenerique(page - 1, pageSize, text).Select(v => v.MaterielId).ToList();

                    if (!previousMoyenList.IsNullOrEmpty())
                    {
                        List<MaterielEnt> items = materielList.FindAll(m => !previousMoyenList.Contains(m.MaterielId));
                        itemList.AddRange(items);
                    }
                }
                else
                {
                    itemList = materielList;
                }

                return itemList.GroupBy(a => a.RessourceId).Select(g => new FicheGeneriqueModel
                {
                    Ressource = mapper.Map<RessourceModel>(g.FirstOrDefault().Ressource),
                });
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Create moyen en location
        /// </summary>
        /// <param name="materiel">Moyen</param>
        public void CreateMoyenEnLocation(MaterielEnt materiel)
        {
            try
            {
                if (materiel == null)
                {
                    throw new FredBusinessException("Problème de création de moyen en location");
                }

                MaterielLocationEnt materielLocation = new MaterielLocationEnt
                {
                    Immatriculation = materiel.Immatriculation,
                    Libelle = materiel.Libelle,
                    MaterielId = materiel.MaterielId,
                    DateCreation = DateTime.UtcNow
                };

                int materialLocationId = materielLocationManager.AddMaterielLocation(materielLocation);
                AffectationMoyenEnt affectationMoyen = new AffectationMoyenEnt
                {
                    MaterielLocationId = materialLocationId,
                    MaterielId = materiel.MaterielId,
                    DateDebut = DateTime.UtcNow,
                    AffectationMoyenTypeId = (int)AffectationMoyenTypeCode.NoAffectation,
                    IsActive = true
                };

                affectationMoyenManager.AddAffectationMoyen(affectationMoyen);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Mettre à jour un materiel de type location
        /// </summary>
        /// <param name="materielLocation">Materiel a mettre a jour</param>
        /// <returns>Le matriel location</returns>
        public int UpdateMaterielLocation(MaterielLocationEnt materielLocation)
        {
            try
            {
                int materielLocationId = materielLocation.MaterielLocationId;
                var allLocationAssocieToMaterielLocation = affectationMoyenManager.GetAllAffectationByMaterielLocationId(materielLocationId);
                foreach (AffectationMoyenEnt item in allLocationAssocieToMaterielLocation)
                {
                    item.MaterielId = materielLocation.MaterielId;
                }
                materielLocationManager.UpdateMaterielLocation(materielLocation);
                affectationMoyenManager.AddOrUpdateRangeAffectationList(allLocationAssocieToMaterielLocation, isAdd: false);

                return materielLocationId;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Supprimer un materiel de type location en fonction d'un ID 
        /// </summary>
        /// <param name="materielLocationId">L'id materiel a mettre a jour </param>
        public void DeleteMaterielLocation(int materielLocationId)
        {
            try
            {
                affectationMoyenManager.DeleteAffectationMoyen(materielLocationId);
                materielLocationManager.DeleteMaterielLocation(materielLocationId);
                Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        ///  Chercher des sociétés de moyen en fonction des critéres fournis en paramètres
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>List des moyens</returns>
        public IEnumerable<SocieteEnt> SearchLightForSociete(SearchSocieteMoyenEnt filters, int page = 1, int pageSize = 30)
        {
            IEnumerable<int> chapitresIds = this.Managers.ReferentielFixe.GetFesChapitreListMoyen();
            try
            {
                try
                {
                    return Repository.Query()
                            .Filter(filters.GetPredicateWhere())
                            .Filter(moyenPointageHelper.GetMaterielGroupeFilter(chapitresIds))
                            .Get()
                            .Select(x => x.Societe)
                            .Distinct()
                            .OrderBy(e => e.Code)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .AsEnumerable();
                }
                catch (Exception e)
                {
                    throw new FredRepositoryException(e.Message, e);
                }
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        ///  Chercher des établissements comptables de moyen en fonction des critéres fournis en paramètres
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>List des moyens</returns>
        public IEnumerable<EtablissementComptableEnt> SearchLightForEtablissementComptable(SearchEtablissementMoyenEnt filters, int page = 1, int pageSize = 30)
        {
            IEnumerable<int> chapitresIds = Managers.ReferentielFixe.GetFesChapitreListMoyen();
            try
            {
                try
                {
                    return Repository.Query()
                            .Filter(filters.GetPredicateWhere())
                            .Filter(moyenPointageHelper.GetMaterielGroupeFilter(chapitresIds))
                            .Get()
                            .Select(x => x.EtablissementComptable)
                            .Distinct()
                            .OrderBy(e => e.Code)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .AsEnumerable();
                }
                catch (Exception e)
                {
                    throw new FredRepositoryException(e.Message, e);
                }
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Generate Excel Moyen
        /// </summary>
        /// <param name="listAffectationMoyen">list Affectation Moyen</param>
        /// <param name="userName">user Name</param>
        /// <param name="periode">periode</param>
        /// <returns>MemoryStream</returns>
        public MemoryStream GenerateExcelMoyen(List<RapportLigneEnt> listAffectationMoyen, string userName, string periode, string templateFolderPath)
        {
            string pathName = Path.Combine(templateFolderPath, Commun.Constantes.PathExcelFile);
            ExcelFormat excelFormat = new ExcelFormat();
            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
            List<RapportMoyenLigneExcelModel> lignes = rapportMoyenExtractionService.GetRapportMoyenLigneExcel(listAffectationMoyen);
            var entete = new
            {
                userName,
                dateEdition = DateTime.Now,
                periode
            };

            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable(Commun.Constantes.EnteteVariableExcel, entete);
            excelFormat.AddVariable(Commun.Constantes.LignesVariableExcel, lignes);
            excelFormat.ApplyVariables();

            return excelFormat.GeneratePdfOrExcel(false, workbook);
        }

        /// <summary>
        /// Mise à jour des pointages matériel 
        /// </summary>
        /// <param name="startDate">Date de début de la mise à jour</param>
        /// <param name="endDate">Date de fin de la mise à jour</param>
        /// <returns>Pointage moyen response</returns>
        public PointageMoyenResponse UpdatePointageMoyen(DateTime startDate, DateTime endDate)
        {
            PointageMoyenResponse response = new PointageMoyenResponse { Error = string.Empty };
            try
            {
                // Préparation de la requéte pour générer les moyens 
                // cette requéte est nécessaire pour des raisons de performance . Elle contient tout ce qui est nécessaire 
                // pour la génération et évite les allers-retours à la base de données pouvant ralentir le traitement .
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
                UpdateMoyenPointageInput request = BuildUpdateMoyenInput(moyenPointageHelper, dateTimeExtendManager, startDate, endDate);

                // Absence d'affectation dans l'intervalle choisi 
                if (request?.AffectationMoyenList.IsNullOrEmpty() != false)
                {
                    response.Error = "Aucune affectation à traiter dans l'intervalle des dates .";
                    return response;
                }

                // Traitement de la requéte  et application des régles de déversmement
                ProccesPointageMoyenResult result = ProcessUpdatePointageRequest(request);

                // Ajout des erreurs d'absence de pointage pour des personnels donnés .
                result.PersonnelPointageErrors.ToList().ForEach(r => response.AddPersonnelErrors(r));

                // Traitement des lignes de rapports aprés application des régles de génération de pointage .
                // Création ou modification des lignes de rapports .
                rapportMoyenService.UpdatePointageMoyen(result.RapportLigneEntsToCreate, result.RapportLigneEntsToUpdate);

                if (response?.PersonnelListNotRegistred.IsNullOrEmpty() == false)
                {
                    response.PersonnelListNotRegistred = response.PersonnelListNotRegistred.OrderBy(x => x.Personnel.PersonnelRef).ToList();
                }
            }
            catch (FredBusinessException ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Process un update pointage request
        /// </summary>
        /// <param name="request">Update moyen pointage request</param>
        /// <returns>Process pointage moyen result</returns>
        private ProccesPointageMoyenResult ProcessUpdatePointageRequest(UpdateMoyenPointageInput request)
        {
            List<RapportLigneEnt> newLines = new List<RapportLigneEnt>();
            List<RapportLigneEnt> updatedLines = new List<RapportLigneEnt>();
            List<PersonnelPointageError> errorPersonnelList = new List<PersonnelPointageError>();
            List<AffectationMoyenRapportModel> affectationMoyenIdList = new List<AffectationMoyenRapportModel>();

            foreach (AffectationMoyenEnt affectationMoyen in request.AffectationMoyenList)
            {
                ProcessAffectationResponse processResponse = rapportMoyenService.Process(request, affectationMoyen, dateTimeExtendManager, moyenPointageHelper, affectationMoyenIdList);

                ProcessAffectationPersonnelResponse personnelResponse = processResponse as ProcessAffectationPersonnelResponse;
                if (personnelResponse != null)
                {
                    errorPersonnelList.Add(personnelResponse.PersonnelPointageError);
                }

                if (!processResponse.RapportLigneEntsToCreate.IsNullOrEmpty())
                {
                    newLines.AddRange(processResponse.RapportLigneEntsToCreate);
                }

                if (!processResponse.RapportLigneEntsToUpdate.IsNullOrEmpty())
                {
                    processResponse.RapportLigneEntsToUpdate.ForEach(v =>
                    {
                        if (!updatedLines.Any(o => o.RapportLigneId == v.RapportLigneId))
                        {
                            updatedLines.Add(v);
                        }
                    });
                }
            }
            return new ProccesPointageMoyenResult(newLines, updatedLines, errorPersonnelList);
        }

        /// <summary>
        /// Prépare toutes les entrées nécessaires pour commençer la génération des moyens
        /// </summary>
        /// <param name="moyenPointageHelper">Moyen pointage helper</param>
        /// <param name="dateTimeExtendManager">Date time extend manager</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Update moyen pointage input</returns>
        private UpdateMoyenPointageInput BuildUpdateMoyenInput(MoyenPointageHelper moyenPointageHelper, IDateTimeExtendManager dateTimeExtendManager, DateTime startDate, DateTime endDate)
        {
            Expression<Func<AffectationMoyenEnt, bool>> datesPredicate = moyenPointageHelper.GetAffectationMoyenDatesPredicate(startDate, endDate);
            Expression<Func<AffectationMoyenEnt, bool>> typePredicate = moyenPointageHelper.GetPointageMoyenAffectationTypePredicate();
            IEnumerable<AffectationMoyenEnt> affectationMoyenEnts = affectationMoyenManager.GetPointageMoyenAffectations(datesPredicate, typePredicate);

            if (affectationMoyenEnts.IsNullOrEmpty())
            {
                return null;
            }

            UpdateMoyenPointageInput request = updateMoyenRequestBuilder.GetPointageMoyenRequest(startDate, endDate, affectationMoyenEnts, dateTimeExtendManager, moyenPointageHelper);

            request.AffectationMoyenList = affectationMoyenEnts;

            IEnumerable<int> personnelIds = affectationMoyenEnts.Where(a => a.PersonnelId.HasValue).Select(v => v.PersonnelId.Value).Distinct();
            IEnumerable<int> ciIds = affectationMoyenEnts.Where(a => a.CiId.HasValue).Select(v => v.CiId.Value).Distinct();

            request.AffectationsPersonnelsAndCisRapportLignes = rapportMoyenService.GetPointageByCisPersonnelsAndDates(ciIds, personnelIds, startDate, endDate);
            request.PersonnelPointageList = rapportMoyenService.GetPersonnelPointageSummary(request.AffectationsPersonnelsAndCisRapportLignes, moyenPointageHelper);
            return request;
        }
    }
}
