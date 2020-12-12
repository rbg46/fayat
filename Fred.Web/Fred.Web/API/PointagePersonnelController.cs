using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Affectation;
using Fred.Business.Commande.Services;
using Fred.Business.Common.ExportDocument;
using Fred.Business.DatesClotureComptable;
using Fred.Business.ExternalService.Materiel;
using Fred.Business.ExternalService.Rapport;
using Fred.Business.Personnel;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Rapport.RapportHebdo;
using Fred.Business.Referential;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Permission;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.DateTimeExtend;
using Fred.Web.Models.CI;
using Fred.Web.Models.PointagePersonnel;
using Fred.Web.Models.Rapport;
using Fred.Web.Modules.Authorization.Api;
using Fred.Web.Shared.Models.PointagePersonnel;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.RapportHebdo;

namespace Fred.Web.API
{
    public class PointagePersonnelController : ApiControllerBase
    {
        private readonly string templateFolderPath = HttpContext.Current.Server.MapPath("/Templates/");

        private readonly IMapper mapper;
        private readonly IExportDocumentService exportDocumentService;
        private readonly IContratAndCommandeInterimaireGeneratorService contratAndCommandeInterimaireGeneratorService;
        private readonly ICommandeManagerExterne commandeManagerExterne;
        private readonly IPointageManager pointageManager;
        private readonly IRapportManager rapportManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IDateTimeExtendManager dateTimeExtendManager;
        private readonly IAffectationManager affectationManager;
        private readonly IPrimeManager primeManager;
        private readonly IMaterielManagerExterne materielManagerExterne;
        private readonly IRapportManagerExterne rapportManagerExterne;
        private readonly IPersonnelManager personnelManager;
        private readonly IRapportHebdoManager rapportHebdoManager;

        public PointagePersonnelController(
            IMapper mapper,
            IExportDocumentService exportDocumentService,
            IContratAndCommandeInterimaireGeneratorService contratAndCommandeInterimaireGeneratorService,
            ICommandeManagerExterne commandeManagerExterne,
            IPointageManager pointageManager,
            IRapportManager rapportManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            IUtilisateurManager utilisateurManager,
            IDateTimeExtendManager dateTimeExtendManager,
            IAffectationManager affectationManager,
            IPrimeManager primeManager,
            IMaterielManagerExterne materielManagerExterne,
            IRapportManagerExterne rapportManagerExterne,
            IPersonnelManager personnelManager,
            IRapportHebdoManager rapportHebdoManager)
        {
            this.mapper = mapper;
            this.exportDocumentService = exportDocumentService;
            this.contratAndCommandeInterimaireGeneratorService = contratAndCommandeInterimaireGeneratorService;
            this.commandeManagerExterne = commandeManagerExterne;
            this.pointageManager = pointageManager;
            this.rapportManager = rapportManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.utilisateurManager = utilisateurManager;
            this.dateTimeExtendManager = dateTimeExtendManager;
            this.affectationManager = affectationManager;
            this.primeManager = primeManager;
            this.materielManagerExterne = materielManagerExterne;
            this.rapportManagerExterne = rapportManagerExterne;
            this.personnelManager = personnelManager;
            this.rapportHebdoManager = rapportHebdoManager;
        }

        /// <summary>
        /// Méthode POST d'application de vérification des erreurs d'un pointage
        /// </summary>
        /// <param name="pointage">Un pointage</param>
        /// <returns>le retour de la requête</returns>
        [HttpPost]
        [Route("api/PointagePersonnel/CheckPointage")]
        public HttpResponseMessage CheckPointage(PointagePersonnelModel<CIModel> pointage)
        {
            try
            {
                return Get(() => this.mapper.Map<PointagePersonnelModel<CIModel>>(pointageManager.CheckPointage(this.mapper.Map<RapportLigneEnt>(pointage))));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get list des astreintes par personnel et ci
        /// </summary>
        /// <param name="personnelId">personnel identifier</param>
        /// <param name="ciId">ci identifier</param>
        /// <param name="year">Année de la période en cours</param>
        /// <param name="month">Mois de la période en cours</param>
        /// <returns>List des astreintes</returns>
        [HttpPost]
        [Route("api/PointagePersonnel/GetAstreintesByPersonnelIdAndCiId")]
        public HttpResponseMessage GetAstreintesByPersonnelIdAndCiId(int personnelId, int ciId, int year, int month)
        {
            DateTime dateDebut = new DateTime(year, month, 1);
            return Get(() => affectationManager.GetAstreintesByPersonnelIdAndCiId(personnelId, ciId, dateDebut, dateDebut.AddMonths(1)));
        }

        /// <summary>
        /// Recupere l'id d'un statut de rapport verrouille
        /// </summary>
        /// <returns>Id du statut verrouille</returns>
        [HttpGet]
        [Route("api/PointagePersonnel/GetRapportStatutVerrouille")]
        public HttpResponseMessage GetRapportStatutVerrouille()
        {
            return Get(() => rapportHebdoManager.GetRapportStatutVerrouille());
        }

        /// <summary>
        /// Get empty object de class PointagePersonnelModel
        /// </summary>
        /// <returns>List des astreintes</returns>
        [HttpGet]
        [Route("api/PointagePersonnel/GetNewObjectPointagePersonnelModel")]
        public HttpResponseMessage GetNewObjectPointagePersonnelModel()
        {
            return Get(() => new PointagePersonnelModel<CIModel>());
        }

        /// <summary>
        /// Méthode POST d'application de vérification des erreurs d'un pointage
        /// </summary>
        /// <param name="listPointages">Un pointage</param>
        /// <returns>le retour de la requête</returns>
        [HttpPost]
        [Route("api/PointagePersonnel/CheckListePointages")]
        public HttpResponseMessage CheckListePointages(List<PointagePersonnelModel<CIModel>> listPointages)
        {
            try
            {
                List<RapportLigneEnt> pointageListToCheck = this.mapper.Map<List<RapportLigneEnt>>(listPointages);
                pointageManager.CheckListPointages(pointageListToCheck);
                return Get(() => this.mapper.Map<List<PointagePersonnelModel<CIModel>>>(pointageListToCheck));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// GET La liste des pointages pour un personnel et une période comptable
        /// </summary>
        /// <param name="periode">Période comptable</param>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Lot de pointage signé</returns>
        [HttpGet]
        [FredWebApiAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPointagePersonnelIndex, mode: FonctionnaliteTypeMode.Read)]
        [Route("api/PointagePersonnel")]
        public async Task<IHttpActionResult> GetPointagesByPersonnelIdAndPeriodeAsync(DateTime periode, int personnelId)
        {
            PointagePersonnelInfo pointagePersonnelInfo = await pointageManager.GetListPointagesByPersonnelIdAndPeriodeAsync(personnelId, periode).ConfigureAwait(false);
            UtilisateurEnt currentUser = utilisateurManager.GetContextUtilisateur();
            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFES)
            {
                return Ok(InitializePointagePersonnelResponse<CIFullLibelleModel>(pointagePersonnelInfo));
            }
            else
            {
                return Ok(InitializePointagePersonnelResponse<CIModel>(pointagePersonnelInfo));
            }
        }

        private PointagePersonnelLoadModel<CI> InitializePointagePersonnelResponse<CI>(PointagePersonnelInfo pointagePersonnelInfo) where CI : CIModel
        {
            return new PointagePersonnelLoadModel<CI>
            {
                CodeDeplacementReadonly = pointagePersonnelInfo.CodeDeplacementReadonly,
                ShowSaisieManuelle = pointagePersonnelInfo.ShowSaisieManuelle,
                ShowDeplacement = pointagePersonnelInfo.ShowDeplacement,
                Pointages = mapper.Map<List<PointagePersonnelModel<CI>>>(pointagePersonnelInfo.Pointages)
            };
        }

        /// <summary>
        /// GET La liste des pointages pour un personnel et une période comptable
        /// </summary>
        /// <param name="periode">Période comptable</param>
        /// <param name="isWeekPeriode">isWeekPeriode</param>
        /// <returns>Lot de pointage signé</returns>
        [HttpGet]
        [Route("api/PointagePersonnel/GetDaysInMonth")]
        public HttpResponseMessage GetDaysInMonth(DateTime periode, bool isWeekPeriode = false)
        {
            return Get(() => dateTimeExtendManager.TestHoliday(periode, new CultureInfo("fr-FR"), isWeekPeriode));
        }

        /// <summary>
        /// POST Apport du visa sur un lot de pointage
        /// </summary>
        /// <param name="listPointages">Liste des pointages</param>    
        /// <returns>Lot de pointage signé</returns>
        [HttpPost]
        [FredWebApiAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPointagePersonnelIndex, mode: FonctionnaliteTypeMode.Write)]
        [Route("api/PointagePersonnel/Save")]
        public async Task<HttpResponseMessage> SaveAsync(List<PointagePersonnelModel<CIModel>> listPointages)
        {
            try
            {

                List<RapportLigneEnt> rapportLignes = this.mapper.Map<List<RapportLigneEnt>>(listPointages);

                var allRapportId = rapportLignes.Select(r => r.RapportId).Distinct();
                List<RapportEnt> allRapportBeforeUpdate = rapportManager.GetRapportListWithRapportLignesNoTracking(allRapportId).ToList();
                List<RapportLigneEnt> rapportLignesLatest = rapportLignes.Where(r => r.AffectationMoyenId == null).ToList();
                if (rapportLignesLatest.Any())
                {
                    SearchCorrespondtRapportLigneAndUpdated(allRapportBeforeUpdate, rapportLignesLatest);
                }

                List<RapportEnt> rapportAdded;
                List<RapportEnt> rapportUpdated;
                PointagePersonnelSaveResultModel ret = rapportManager.SaveListPointagesPersonnel(rapportLignes, out rapportAdded, out rapportUpdated);

                if (ret.Errors.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, ret);
                }
                var rapportAEnvoyer = rapportUpdated.Concat(rapportAdded).ToList();

                if (listPointages.FirstOrDefault().Personnel.IsInterimaire && rapportAEnvoyer.Any())
                {
                    await contratAndCommandeInterimaireGeneratorService.CreateCommandesForPointagesInterimairesAsync(rapportAEnvoyer,
                           (commandeId) => commandeManagerExterne.ExportCommandeToSapAsync(commandeId));
                }


                SendPointageMaterielAndPersonnelToExternalSystem(rapportAEnvoyer, allRapportBeforeUpdate);

                return Request.CreateResponse(HttpStatusCode.Accepted, ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private void SearchCorrespondtRapportLigneAndUpdated(List<RapportEnt> allRapportBeforeUpdate, List<RapportLigneEnt> rapportLignesLatest)
        {
            List<RapportLigneEnt> listLignesFromDatabase = new List<RapportLigneEnt>();
            allRapportBeforeUpdate.ForEach(x => listLignesFromDatabase.AddRange(x.ListLignes));

            foreach (var rapportligne in listLignesFromDatabase)
            {
                RapportLigneEnt rapport = rapportLignesLatest.Find(e => e.PointageId == rapportligne.PointageId);
                if (rapport != null)
                {
                    UpdateNewCreatedLineWithAffectationMoyenIdAndMachineHour(rapportligne, rapport);
                }
            }
        }

        private void UpdateNewCreatedLineWithAffectationMoyenIdAndMachineHour(RapportLigneEnt ligneRapoortFromDataBase, RapportLigneEnt ligneRapportOnFront)
        {
            ligneRapportOnFront.AffectationMoyenId = ligneRapoortFromDataBase.AffectationMoyenId;
            ligneRapportOnFront.HeuresMachine = ligneRapoortFromDataBase.HeuresMachine;
        }

        [HttpPut]
        [FredWebApiAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPointagePersonnelIndex, mode: FonctionnaliteTypeMode.Write)]
        [Route("api/PointagePersonnel/Duplicate")]
        public async Task<IHttpActionResult> DuplicateAsync(PointageDuplicateModel pointageDuplicateModel)
        {

            var duplicatedResult = pointageManager.DuplicatePointage(pointageDuplicateModel.RapportLigneId, pointageDuplicateModel.CiId, pointageDuplicateModel.StartDate, pointageDuplicateModel.EndDate);

            if (!duplicatedResult.HasError())
            {
                List<RapportEnt> rapportAdded;
                List<RapportEnt> rapportUpdated;
                rapportManager.SaveListDuplicatedPointagesPersonnel(duplicatedResult.DuplicatedRapportLignes, duplicatedResult.PointageToDuplicate.PointageId, out rapportAdded, out rapportUpdated);
                if (duplicatedResult.PointageToDuplicate.Personnel.IsInterimaire && rapportAdded.Any())
                {
                    await this.contratAndCommandeInterimaireGeneratorService.CreateCommandesForPointagesInterimairesAsync(rapportAdded,
                          (commandeId) => commandeManagerExterne.ExportCommandeToSapAsync(commandeId));
                }

                SendPointageMaterielAndPersonnelToExternalSystem(rapportAdded, null);
            }

            var result = this.mapper.Map<PointageDuplicateResultModel>(duplicatedResult);

            return Ok(result);
        }

        private void SendPointageMaterielAndPersonnelToExternalSystem(List<RapportEnt> rapportsAEnvoyer, List<RapportEnt> allRapportBeforeUpdate)
        {
            var currentUser = utilisateurManager.GetContextUtilisateur();
            var listRapportIdFluxSAPWithMateriel = GetListRapportIdFluxSapWithMateriel(rapportsAEnvoyer, allRapportBeforeUpdate).ToList();

            Task.Factory.StartNew(async () =>
            {
                // pas d'envoi à STORM pour FES 
                if (currentUser.Personnel.Societe.Groupe.Code != Constantes.CodeGroupeFES && listRapportIdFluxSAPWithMateriel.Any())
                {
                    await materielManagerExterne.ExportPointageMaterielToStormAsync(listRapportIdFluxSAPWithMateriel);
                }

                foreach (var rapport in rapportsAEnvoyer)
                {
                    // FAYAT TP UNIQUEMENT
                    if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFTP)
                    {
                        // Flux CAT2
                        await rapportManagerExterne.ExportPointagePersonnelToSapAsync(rapport.RapportId);
                    }
                    // SOMOPA UNIQUEMENT
                }

            });
        }

        private IEnumerable<int> GetListRapportIdFluxSapWithMateriel(List<RapportEnt> rapportsAEnvoyer, List<RapportEnt> allRapportBeforeUpdate)
        {
            IEnumerable<int> listRapportIdFluxSAPWithMateriel = null;
            if (allRapportBeforeUpdate != null)
            {
                //On récupère tous les rapports ayant 
                // 1. Une ligne contenant un materiel
                // 1. Ou une ligne qui a connu des changements de materiel
                listRapportIdFluxSAPWithMateriel = rapportsAEnvoyer
                .Where(r => r.ListLignes.Any(rl => rl.MaterielId != null ||
                                (
                                    //Aucun des paramètres de la fonciton CheckRapportLignesMaterielChanged ne peuvent être null
                                    //Donc comme r (rapport courant) ne peut pas être null on ne le vérifie pas
                                    //Mais comme il est possible que le rapport courant soit nouveau (et donc pas dans la liste allRapportBeforeUpdate)
                                    //On vérifie qu'il est présent dans la liste afin de pouvoir faire un single sans risquer un crash
                                    allRapportBeforeUpdate.Any(rb => rb.RapportId == r.RapportId) &&
                                    rapportManager.CheckRapportLignesMaterielChanged(r, allRapportBeforeUpdate.Single(rb => rb.RapportId == r.RapportId))
                                )))
                .Select(r => r.RapportId);
            }
            else
            {
                listRapportIdFluxSAPWithMateriel = rapportsAEnvoyer
                .Where(r => r.ListLignes.Any(rl => rl.MaterielId != null))
                .Select(r => r.RapportId);
            }

            return listRapportIdFluxSAPWithMateriel;
        }

        /// <summary>
        /// Méthode POST d'ajout d'une prime dans un pointage de personnel
        /// </summary>
        /// <param name="rapportLigneAndPrime">Un rapport et une prime</param>
        /// <returns>Un rapport</returns>
        [HttpPost]
        [FredWebApiAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPointagePersonnelIndex, mode: FonctionnaliteTypeMode.Write)]
        [Route("api/PointagePersonnel/AddPrimeToPointagePersonnel")]
        public HttpResponseMessage AddPrimeToPointagePersonnel(PointagePersonnelAndPrimeModel rapportLigneAndPrime)
        {
            try
            {
                if (!rapportLigneAndPrime.RapportLigne.IsAnticipe)
                {
                    rapportLigneAndPrime.RapportLigne = this.mapper.Map<PointagePersonnelModel<CIModel>>(pointageManager.AddPrimeToPointage(this.mapper.Map<RapportLigneEnt>(rapportLigneAndPrime.RapportLigne), this.mapper.Map<PrimeEnt>(rapportLigneAndPrime.Prime)));
                }
                return Request.CreateResponse(HttpStatusCode.OK, rapportLigneAndPrime.RapportLigne);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST d'ajout d'une tache dans un pointage de personnel
        /// </summary>
        /// <param name="rapportLigneAndTache">Un rapport et une tache</param>
        /// <returns>Un rapport</returns>
        [HttpPost]
        [FredWebApiAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPointagePersonnelIndex, mode: FonctionnaliteTypeMode.Write)]
        [Route("api/PointagePersonnel/AddTacheToPointagePersonnel")]
        public HttpResponseMessage AddTacheToPointagePersonnel(PointagePersonnelAndTacheModel rapportLigneAndTache)
        {
            try
            {
                if (!rapportLigneAndTache.RapportLigne.IsAnticipe)
                {
                    rapportLigneAndTache.RapportLigne = this.mapper.Map<PointagePersonnelModel<CIModel>>(pointageManager.AddTacheToPointageReel(this.mapper.Map<RapportLigneEnt>(rapportLigneAndTache.RapportLigne), this.mapper.Map<TacheEnt>(rapportLigneAndTache.Tache)));
                    return Request.CreateResponse(HttpStatusCode.OK, rapportLigneAndTache.RapportLigne);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST d'application de la récupération ou création d'indémnités de déplacements
        /// </summary>
        /// <param name="rapportLigne">Une ligne de rapport</param>
        /// <returns>le retour de la requête</returns>
        [HttpPost]
        [FredWebApiAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPointagePersonnelIndex, mode: FonctionnaliteTypeMode.Write)]
        [Route("api/PointagePersonnel/GetOrCreateIndemniteDeplacement")]
        public HttpResponseMessage GetOrCreateIndemniteDeplacement(PointagePersonnelModel<CIModel> rapportLigne)
        {
            try
            {
                List<string> warnings;
                rapportLigne = this.mapper.Map<PointagePersonnelModel<CIModel>>(pointageManager.GetOrCreateIndemniteDeplacementForRapportLigne(this.mapper.Map<RapportLigneEnt>(rapportLigne), out warnings, false));
                rapportLigne.Warnings = warnings;

                return Request.CreateResponse(HttpStatusCode.OK, rapportLigne);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// GET La liste des pointages pour un personnel et une période comptable
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="year">Année de la période en cours</param>
        /// <param name="month">Mois de la période en cours</param>
        /// <param name="day">jour du pointage en cours</param>
        /// <param name="personnelId">l'identifaint du personnel</param>
        /// <param name="statut">statut du personnel</param>
        /// <returns>Lot de pointage signé</returns>
        [HttpPost]
        [Route("api/PointagePersonnel/InitPointagePersonnel")]
        public HttpResponseMessage InitPointagePersonnel(int ciId, int year, int month, int? day = null, int personnelId = 0, string statut = "")
        {
            try
            {
                RapportLigneEnt pointage = null;
                if (ciId == 0)
                {
                    CIEnt defaultCi = affectationManager.GetDefaultCi(personnelId);
                    pointage = pointageManager.InitTacheParDefautPointagePersonnel(defaultCi.CiId, personnelId);
                    if (statut == "1")
                    {
                        var primeIpd = primeManager.GetPrimesList().FirstOrDefault(p => p.Code == "IPD5");
                        pointageManager.AddPrimeToPointage(pointage, primeIpd);
                        var prime = pointage.ListRapportLignePrimes.FirstOrDefault();
                        prime.HeurePrime = 7;
                        prime.IsChecked = true;
                    }
                }
                else
                {
                    pointage = pointageManager.InitTacheParDefautPointagePersonnel(ciId, personnelId);
                }
                if (personnelId != 0)
                {
                    MaterielEnt materielParDefault = personnelManager.GetMaterielDefault(personnelId);
                    if (materielParDefault != null)
                    {
                        pointage.Materiel = materielParDefault;
                        pointage.MaterielId = materielParDefault.MaterielId;
                        pointage.MaterielNomTemporaire = materielParDefault.LibelleLong;
                    }

                    if (day.Value != 0)
                    {
                        var astreinte = affectationManager.GetAstreinte(ciId, personnelId, new DateTime(year, month, day.Value));
                        if (astreinte != null)
                        {
                            pointage.HasAstreinte = true;
                            pointage.AstreinteId = astreinte.AstreintId;
                        }
                    }
                }
                var pointageModel = mapper.Map<PointagePersonnelModel<CIModel>>(pointage);
                pointageModel.Cloture = datesClotureComptableManager.IsPeriodClosed(ciId, year, month);
                pointageModel.MonPerimetre = utilisateurManager.IsInMyPerimetre(ciId);
                return Get(() => pointageModel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// GET Créé et récupère le fichier excel ou pdf des pointages d'un personnel pour une période donnée
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="typeExport">Type d'export</param>
        /// <returns>Retourne le fichier crée</returns>
        [HttpGet]
        [FredWebApiAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPointagePersonnelIndex, mode: FonctionnaliteTypeMode.Read)]
        [Route("api/PointagePersonnel/Export/{personnelId}/{periode}/{typeExport}")]
        public async Task<HttpResponseMessage> GetPointagePersonnelExportAsync(int personnelId, DateTime periode, int typeExport)
        {
            var isFes = utilisateurManager.IsUtilisateurOfGroupe(Constantes.CodeGroupeFES);
            var exportFilename = exportDocumentService.GetDocumentFileName(pointageManager.GetPointagePersonnelExportFilename(personnelId, periode), typeExport);
            byte[] pointagePersonnelExport = await pointageManager.GetPointagePersonnelExportAsync(personnelId, periode, typeExport, isFes, templateFolderPath).ConfigureAwait(false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportFilename, pointagePersonnelExport);
        }

        /// <summary>
        /// Créé le fichier excel ou pdf des pointages d'un intérimaire pour une période donnée
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>L'id du fichier mis en cache</returns>
        [HttpPost]
        [Route("api/PointagePersonnel/Export/Interimaire")]
        public object PostPointageInterimaireExport(PointagePersonnelExportModel pointagePersonnelExportModel)
        {
            var excelBytes = pointageManager.GetPointageInterimaireExport(pointagePersonnelExportModel);
            if (excelBytes != null && excelBytes.Length > 0)
            {
                string typeCache = "excelBytes_";
                string cacheId = Guid.NewGuid().ToString();

                CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
                MemoryCache.Default.Add(typeCache + cacheId, excelBytes, policy);

                return new { id = cacheId };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Récupère le fichier excel ou pdf des pointages d'un intérimaire pour une période donnée
        /// </summary>
        /// <param name="id">id du fichier mis en cache</param>
        /// <param name="typeExport">Type de l'export</param>
        /// <param name="dateDebut">Date début</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>Retourne le fichier crée</returns>
        [HttpGet]
        [Route("api/PointagePersonnel/Export/Interimaire/{id}/{typeExport}/{dateDebut}/{dateFin}")]
        public HttpResponseMessage GetPointageInterimaireExport(string id, int typeExport, DateTime dateDebut, DateTime dateFin)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }

            var exportFilename = exportDocumentService.GetDocumentFileName(pointageManager.GetPointageInterimaireExportFilename(dateDebut, dateFin), typeExport);
            return exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);
        }

        /// <summary>
        ///  Créé le fichier excel des pointages d'un personnel hebdomadaire pour une période donnée
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>L'id du fichier mis en cache</returns>
        [HttpPost]
        [Route("api/PointagePersonnel/Export/Hebdomadaire")]
        public object PostPointagePersonnelHebdomadaireExport(PointagePersonnelExportModel pointagePersonnelExportModel)
        {
            var excelBytes = pointageManager.GetPointagePersonnelHebdomadaireExport(pointagePersonnelExportModel);
            if (excelBytes != null && excelBytes.Length > 0)
            {
                string typeCache = "excelBytes_";
                string cacheId = Guid.NewGuid().ToString();

                CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
                MemoryCache.Default.Add(typeCache + cacheId, excelBytes, policy);

                return new { id = cacheId };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Rrécupère le fichier excel des pointages d'un personnel hebdomadaire pour une période donnée
        /// </summary>
        /// <param name="id">id du fichier mis en cache</param>
        /// <param name="typeExport">Type de l'export</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="dateDebut">Date début</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>Retourne le fichier crée</returns>
        [HttpGet]
        [Route("api/PointagePersonnel/Export/Hebdomadaire/{id}/{typeExport}/{dateComptable}/{dateDebut}/{dateFin}")]
        public HttpResponseMessage GetPointagePersonnelHebdomadaireExport(string id, int typeExport, DateTime? dateComptable, DateTime? dateDebut, DateTime? dateFin)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }

            if (dateDebut == null || dateFin == null)
            {
                dateDebut = default(DateTime);
                dateFin = default(DateTime);
            }
            else if (dateComptable == null)
            {
                dateComptable = default(DateTime);
            }

            var exportFilename = exportDocumentService.GetDocumentFileName(pointageManager.GetPointagePersonnelHebdomadaireExportFilename((DateTime)dateComptable, (DateTime)dateDebut, (DateTime)dateFin), typeExport);
            return exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);

        }

        /// <summary>
        /// Créé le fichier excel ou pdf des pointages d'un Challenge Securite pour une période donnée
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>L'id du fichier mis en cache</returns>
        [HttpPost]
        [Route("api/PointagePersonnel/Export/ChallengeSecurite")]
        public object PostPointageChallengeSecuriteExport(PointagePersonnelExportModel pointagePersonnelExportModel)
        {
            var excelBytes = pointageManager.GetPointageChallengeSecuriteExport(pointagePersonnelExportModel, templateFolderPath);
            if (excelBytes != null)
            {
                string typeCache = "excelBytes_";
                string cacheId = Guid.NewGuid().ToString();

                CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
                MemoryCache.Default.Add(typeCache + cacheId, excelBytes, policy);

                return new { id = cacheId };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Rrécupère le fichier excel des pointages d'un Challenge Securite pour une période donnée
        /// </summary>
        /// <param name="id">id du fichier mis en cache</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="dateDebut">Date début</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>Retourne le fichier crée</returns>
        [HttpGet]
        [Route("api/PointagePersonnel/Export/ChallengeSecurite/{id}/{dateComptable}/{dateDebut}/{dateFin}")]
        public HttpResponseMessage GetPointageChallengeSecuriteExport(string id, DateTime? dateComptable, DateTime? dateDebut, DateTime? dateFin)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }

            if (dateDebut == null || dateFin == null)
            {
                dateDebut = default(DateTime);
                dateFin = default(DateTime);
            }
            else if (dateComptable == null)
            {
                dateComptable = default(DateTime);
            }

            var exportFilename = exportDocumentService.GetDocumentFileName(pointageManager.GetPointageChallengeSecuriteExportFilename((DateTime)dateComptable, (DateTime)dateDebut, (DateTime)dateFin), false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);

        }

        /// <summary>
        /// Récupere les heures normales des personnels passé en paramétre selon une période
        /// </summary>
        /// <param name="rapportHebdoPersonnelWithAllCiModel">model contenant une liste de personnel avec la période</param>
        /// <returns>Http response message</returns>
        [HttpGet]
        [Route("api/PointagePersonnel/GetPointageByPersonnelIDAndInterval")]
        public HttpResponseMessage GetPointageByPersonnelIDAndInterval([FromUri] RapportHebdoPersonnelWithAllCiModel rapportHebdoPersonnelWithAllCiModel)
        {
            return this.Get(() => this.mapper.Map<IEnumerable<RapportHebdoPersonnelWithTotalHourModel>>(Managers.Pointage.GetPointageByPersonnelIDAndInterval(this.mapper.Map<RapportHebdoPersonnelWithAllCiEnt>(rapportHebdoPersonnelWithAllCiModel))));
        }


    }
}
