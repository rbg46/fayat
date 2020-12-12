using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Commande.Services;
using Fred.Business.ExternalService.Materiel;
using Fred.Business.ExternalService.Rapport;
using Fred.Business.Organisation;
using Fred.Business.Personnel;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Rapport.RapportHebdo;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.EtatPaie;
using Fred.Web.Shared.Models.Personnel.Light;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;
using Fred.Web.Shared.Models.Referential;

namespace Fred.Web.API
{
    public class RapportController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly IContratAndCommandeInterimaireGeneratorService contratAndCommandeInterimaireGeneratorService;
        private readonly ICommandeManagerExterne commandeManagerExterne;
        private readonly IRapportManager rapportManager;
        private readonly IPointageManager pointageManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IMaterielManagerExterne materielManagerExterne;
        private readonly IRapportManagerExterne rapportManagerExterne;
        private readonly IRapportHebdoService rapportHebdoService;
        private readonly IOrganisationManager organisationManager;

        public RapportController(
            IMapper mapper,
            IContratAndCommandeInterimaireGeneratorService contratAndCommandeInterimaireGeneratorService,
            ICommandeManagerExterne commandeManagerExterne,
            IRapportManager rapportManager,
            IPointageManager pointageManager,
            IUtilisateurManager utilisateurManager,
            IMaterielManagerExterne materielManagerExterne,
            IRapportManagerExterne rapportManagerExterne,
            IRapportHebdoService rapportHebdoService,
            IOrganisationManager organisationManager)
        {
            this.mapper = mapper;
            this.contratAndCommandeInterimaireGeneratorService = contratAndCommandeInterimaireGeneratorService;
            this.commandeManagerExterne = commandeManagerExterne;
            this.rapportManager = rapportManager;
            this.pointageManager = pointageManager;
            this.utilisateurManager = utilisateurManager;
            this.materielManagerExterne = materielManagerExterne;
            this.rapportManagerExterne = rapportManagerExterne;
            this.rapportHebdoService = rapportHebdoService;
            this.organisationManager = organisationManager;
        }

        /// <summary>
        /// Méthode de génération d'un nouveau rapport vide
        /// </summary>
        /// <param name="ciId">Identifiant d'un CI</param>
        /// <returns>Retourne un rapport</returns>
        [HttpGet]
        [Route("api/Rapport/New/{ciId?}")]
        public HttpResponseMessage GetNewRapport(int? ciId = null)
        {
            try
            {
                RapportModel rapport = this.mapper.Map<RapportModel>(rapportManager.GetNewRapport(ciId));
                return Request.CreateResponse(HttpStatusCode.OK, rapport);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération d'un rapport par son id ou d'un rapport dupliqué
        /// </summary>
        /// <param name="id">Identifiant du rapport</param>
        /// <param name="duplicate">Indique si oui ou non on veut dupliquer le rapport</param>
        /// <param name="validate">Passe par validator ou pas</param>
        /// <returns>Un rapport</returns>
        [HttpGet]
        [Route("api/Rapport/Get/{id}/{duplicate?}/{validate?}")]
        public HttpResponseMessage GetById(int id, bool duplicate = false, bool validate = false)
        {
            RapportModel rapport;
            try
            {
                if (!duplicate)
                {
                    rapport = this.mapper.Map<RapportModel>(rapportManager.GetRapportById(id, true));
                }
                else
                {
                    rapport = this.mapper.Map<RapportModel>(rapportManager.DuplicateRapport(rapportManager.GetRapportById(id, true)));
                }

                return Request.CreateResponse(HttpStatusCode.OK, rapport);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode Post duplication d'un rapport pour un autre ci
        /// </summary>
        /// <param name="rapport">rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        [HttpPost]
        [Route("api/Rapport/DuplicateRapportForNewCi")]
        public HttpResponseMessage DuplicateRapportForNewCi(RapportModel rapport)
        {
            return Post(() => this.mapper.Map<RapportModel>(rapportManager.DuplicateRapportForNewCi(this.mapper.Map<RapportEnt>(rapport))));
        }

        /// <summary>
        /// Méthode POST api/controller
        /// </summary>
        /// <param name="rapport">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Rapport/AddOrUpdateRapport")]
        public async Task<IHttpActionResult> AddOrUpdateRapport(RapportModel rapport)
        {
            var rapportEnt = this.mapper.Map<RapportEnt>(rapport);
            List<RapportEnt> listRapport = new List<RapportEnt>();

            if (rapportManager.IsDateChantierInPeriodeCloture(rapportEnt))
            {
                throw new ValidationException(new List<ValidationFailure>()
                {
                    new ValidationFailure("DateChantier",ControllerRessources.Rapport_Controller_PeriodeCloturee)
                });
            }
            if (rapport.RapportId == 0)
            {
                int rapportExistId = rapportManager.CheckRepportsForFES(rapport.CiId, rapport.DateChantier);
                if (rapportExistId != 0)
                {
                    throw new ValidationException(new List<ValidationFailure>()
                    {
                        new ValidationFailure("RapportExist",string.Format(Shared.App_LocalResources.FeatureRapport.Rapport_Detail_Erreur_Doublon, rapport.CiId, rapportExistId))
                    });
                }

                listRapport.Add(rapportManager.AddRapport(rapportEnt));
                rapport.RapportId = listRapport[0].RapportId;
            }
            else
            {
                // Récupération du rapport avant update pour différents contrôles
                var rapportBeforeUpdate = rapportManager.GetRapportByIdWithoutValidation(rapportEnt.RapportId);
                rapportManager.CheckRapportStatutChangedInDb(rapportEnt, rapportBeforeUpdate);
                // Update du rapport
                var updatedRapport = rapportManager.UpdateRapport(rapportEnt);
                listRapport.Add(updatedRapport);
                this.ExportRapportPointageToSap(updatedRapport, rapportBeforeUpdate, utilisateurManager.GetContextUtilisateur());
            }

            rapportHebdoService.CreateOrUpdatePrimeAstreinte(rapportEnt);

            List<CommandeEnt> commandeEntList = this.contratAndCommandeInterimaireGeneratorService.CreateCommandesForPointagesInterimaires(listRapport[0]);

            if (commandeEntList != null && commandeEntList.Count > 0)
            {
                foreach (var commande in commandeEntList)
                {
                    await commandeManagerExterne.ExportCommandeToSapAsync(commande.CommandeId);
                }
            }

            return Ok(mapper.Map<RapportModel>(listRapport[0]));
        }

        [HttpPost]
        [Route("api/Rapport/Duplicate")]
        [ResponseType(typeof(RapportDuplicateResultModel))]
        public async Task<IHttpActionResult> DuplicateAsync(RapportDuplicateModel rapportDuplicateModel)
        {
            DuplicateRapportResult duplicateRapportResult = this.rapportManager.DuplicateRapport(rapportDuplicateModel.RapportId, rapportDuplicateModel.StartDate, rapportDuplicateModel.EndDate);
            if (duplicateRapportResult.HasDatesInClosedMonth)
            {
                return CreateBadRequestWithModelStateCompleted("HasDatesInClosedMonth",
                    Shared.App_LocalResources.FeatureRapport.Pointage_List_Duplicate_Error_Month_closed);
            }
            if (duplicateRapportResult.HasInterimaireWithoutContrat)
            {
                return CreateBadRequestWithModelStateCompleted("HasInterimaireWithoutContrat",
                    Shared.App_LocalResources.FeatureRapport.Pointage_List_Duplicate_Error_Interimaire_Without_Contrat);
            }
            if (duplicateRapportResult.DuplicationOnlyOnWeekend)
            {
                return CreateBadRequestWithModelStateCompleted("DuplicationOnlyOnWeekend",
                    Shared.App_LocalResources.FeatureRapport.Pointage_List_Duplicate_Error_Duplication_Only_OnWeekend);
            }
            if (duplicateRapportResult.HasPersonnelsInactivesOnPeriode)
            {
                return CreateBadRequestWithModelStateCompleted("HasPersonnelsInactivesOnPeriode",
                    Shared.App_LocalResources.FeatureRapport.Pointage_List_Duplicate_Error_Duplication_un_Personnel_Inactif_sur_periode);
            }
            if (duplicateRapportResult.HasAllDuplicationInDifferentZoneDeTravail)
            {
                return CreateBadRequestWithModelStateCompleted("HasAllDuplicationInDifferentZoneDeTravail",
                    Shared.App_LocalResources.FeatureRapport.Pointage_List_Duplicate_Error_no_commun_zones_de_travail);
            }

            await this.contratAndCommandeInterimaireGeneratorService.CreateCommandesForPointagesInterimairesAsync(duplicateRapportResult.Rapports,
                  (commandeId) => commandeManagerExterne.ExportCommandeToSapAsync(commandeId));

            return Ok(mapper.Map<RapportDuplicateResultModel>(duplicateRapportResult));
        }

        private IHttpActionResult CreateBadRequestWithModelStateCompleted(string key, string errorMessage)
        {
            ModelState.AddModelError(key, errorMessage);
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Méthode GET de récupération d'un rapport vide
        /// </summary>
        /// <param name="rapport">Rapport auquel on ajoute une ligne</param>
        /// <returns>Retourne la liste des DatesCalendrierPaies</returns>
        [HttpPost]
        [Route("api/Rapport/AddNewRapportLigneToRapport")]
        public HttpResponseMessage AddNewRapportLigneToRapport(RapportModel rapport)
        {
            try
            {
                rapport = this.mapper.Map<RapportModel>(rapportManager.AddNewPointageReelToRapport(this.mapper.Map<RapportEnt>(rapport)));
                return Request.CreateResponse(HttpStatusCode.OK, rapport);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST d'ajout d'une prime dans un rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="rapportAndPrime">Un rapport et une prime</param>
        /// <returns>Un rapport</returns>
        [HttpPost]
        [Route("api/Rapport/AddPrimeToRapport")]
        public HttpResponseMessage AddPrimeToRapport(RapportAndPrimeModel rapportAndPrime)
        {
            try
            {
                rapportAndPrime.Rapport = this.mapper.Map<RapportModel>(rapportManager.AddPrimeToRapport(this.mapper.Map<RapportEnt>(rapportAndPrime.Rapport), this.mapper.Map<PrimeEnt>(rapportAndPrime.Prime)));
                return Request.CreateResponse(HttpStatusCode.OK, rapportAndPrime.Rapport);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST d'ajout d'une tache dans un rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="rapportAndTache">Un rapport et une tache</param>
        /// <returns>Un rapport</returns>
        [HttpPost]
        [Route("api/Rapport/AddTacheToRapport")]
        public HttpResponseMessage AddTacheToRapport(RapportAndTacheModel rapportAndTache)
        {
            try
            {
                rapportAndTache.Rapport = this.mapper.Map<RapportModel>(rapportManager.AddTacheToRapport(this.mapper.Map<RapportEnt>(rapportAndTache.Rapport), this.mapper.Map<TacheEnt>(rapportAndTache.Tache)));
                return Request.CreateResponse(HttpStatusCode.OK, rapportAndTache.Rapport);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("api/Rapport/CanBeDeleted")]
        public HttpResponseMessage CanBeDeleted(RapportModel rapport)
        {
            try
            {
                bool result = rapportManager.RapportCanBeDeleted(this.mapper.Map<RapportEnt>(rapport));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST de suppression d'un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à supprimer</param>
        /// <param name="fromListeRapport">Indique si la suppression provient de la liste du rapport</param>
        /// <returns>Un booléen/returns>
        [HttpPost]
        [Route("api/Rapport/DeleteRapport/{fromListeRapport?}")]
        public HttpResponseMessage DeleteRapport(RapportModel rapport, bool fromListeRapport = false)
        {
            try
            {
                var currentUser = utilisateurManager.GetContextUtilisateur();
                rapportManager.DeleteRapport(this.mapper.Map<RapportEnt>(rapport), currentUser.UtilisateurId, fromListeRapport);
                try
                {
                    Task.Factory.StartNew(async () =>
                    {
                        // Flux CAT2
                        await rapportManagerExterne.ExportPointagePersonnelToSapAsync(rapport.RapportId, currentUser);
                        // Flux J3G$
                        await materielManagerExterne.ExportPointageMaterielToStormAsync(rapport.RapportId, currentUser);
                    });
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST de suppression d'un rapport
        /// </summary>
        /// <param name="lstRapport">Les rapports à supprimer</param>
        /// <param name="fromListeRapport">Indique si la suppression provient de la liste du rapport</param>
        /// <returns>Un booléen/returns>
        [HttpPost]
        [Route("api/Rapport/DeleteRapports")]
        public HttpResponseMessage DeleteRapports(List<RapportModel> lstRapport, bool fromListeRapport = false)
        {
            try
            {
                var currentUser = utilisateurManager.GetContextUtilisateur();
                lstRapport.ForEach(r => rapportManager.DeleteRapport(this.mapper.Map<RapportEnt>(r), utilisateurManager.GetContextUtilisateurId(), fromListeRapport));

                Task.Factory.StartNew(async () =>
                {
                    foreach (var rapport in lstRapport)
                    {
                        try
                        {
                            // Flux CAT2
                            await rapportManagerExterne.ExportPointagePersonnelToSapAsync(rapport.RapportId, currentUser);
                            // Flux J3G$
                            await materielManagerExterne.ExportPointageMaterielToStormAsync(rapport.RapportId, currentUser);
                        }
                        catch (Exception exception)
                        {
                            logger.Error(exception);
                        }
                    }
                });
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Récupère la liste des rapports correspondant aux critères de recherche fourni
        /// </summary>
        /// <param name="filter">Filtres de recherche sur Rapport</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>IEnumerable contenant les rapports correspondant aux critères de recherche</returns>
        [HttpPost]
        [Route("api/Rapport/SearchRapportWithFilters/{page?}/{pageSize?}")]
        public HttpResponseMessage SearchRapportWithFilters(SearchRapportModel filter, int? page = 1, int? pageSize = 20)
        {
            return Post(() =>
            {
                filter.SortFields["Ci.Code"] = filter.CiCodeAsc;
                filter.SortFields["RapportStatut.Libelle"] = filter.StatutAsc;
                filter.SortFields["RapportId"] = filter.NumeroRapportAsc;
                filter.SortFields["DateChantier"] = filter.DateChantierAsc;

                return mapper.Map<SearchRapportListWithFilterResultModel>(rapportManager.SearchRapportWithFilter(this.mapper.Map<SearchRapportEnt>(filter), page, pageSize));
            });
        }

        /// <summary>
        /// Récupère la liste des rapports correspondant aux critères de recherche fourni
        /// </summary>
        /// <param name="filters">Filtres de recherche sur Rapport</param>
        /// <returns>IEnumerable contenant les rapports correspondant aux critères de recherche</returns>
        [HttpPost]
        [Route("api/Rapport/SearchRapportLigneWithFilters/")]
        public HttpResponseMessage SearchRapportLigneWithFilters(SearchRapportLigneModel filters)
        {
            try
            {
                List<RapportLigneModel> pointagesReel = new List<RapportLigneModel>();
                if (filters.IsReel)
                {
                    pointagesReel.AddRange(this.mapper.Map<IEnumerable<RapportLigneModel>>(pointageManager.SearchPointageReelWithFilter(this.mapper.Map<SearchRapportLigneEnt>(filters), true)));
                }
                return Request.CreateResponse(HttpStatusCode.OK, pointagesReel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération des filtres de recherche sur Rapport
        /// </summary>
        /// <returns>Retourne la liste des filtres de recherche sur Rapport</returns>
        [HttpGet]
        [Route("api/Rapport/Filter/")]
        public SearchRapportModel Filters()
        {
            int demandeurId = utilisateurManager.GetContextUtilisateurId();
            SearchRapportEnt filters = rapportManager.GetFiltersList(demandeurId);
            return this.mapper.Map<SearchRapportModel>(filters);
        }

        /// <summary>
        /// Méthode POST test d'intégrité d'un rapport
        /// </summary>
        /// <param name="rapport">Rapport à vérifier</param>
        /// <returns>Retourne un rapport avec les erreurs</returns>
        [HttpPost]
        [Route("api/Rapport/CheckRapport/")]
        public HttpResponseMessage CheckRapport(RapportModel rapport)
        {
            try
            {
                rapport = (this.mapper.Map<RapportModel>(rapportManager.CheckRapport(this.mapper.Map<RapportEnt>(rapport))));
                return Request.CreateResponse(HttpStatusCode.OK, rapport);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST Liste les identifiants de rapport en erreur
        /// </summary>
        /// <param name="rapportsId">Liste des identifiants des rapports à vérifier</param>
        /// <returns>Retourne la liste des identifiants de rapport en erreur</returns>
        [HttpPost]
        [Route("api/Rapport/GetListRapportIdWithError/")]
        public HttpResponseMessage GetListRapportIdWithError(List<int> rapportsId)
        {
            try
            {
                var listRapportId = rapportManager.GetListRapportIdWithError(rapportsId);
                return Request.CreateResponse(HttpStatusCode.OK, listRapportId);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération d'une nouvelle instance code absence intialisée.
        /// </summary>
        /// <returns>Retourne une nouvelle instance code absence intialisée</returns>
        [HttpGet]
        [Route("api/Rapport/NewPointageMensuel")]
        public HttpResponseMessage NewPointageMensuel()
        {
            return Get(() => this.mapper.Map<PointageMensuelModel>(pointageManager.GetNewPointageMensuel()));
        }

        /// <summary>
        /// Méthode POST d'application des règles de gestion sur un rapport
        /// </summary>
        /// <param name="rapport">Un rapport</param>
        /// <returns>Un pointage</returns>
        [HttpPost]
        [Route("api/Rapport/ApplyRGCIOnRapport")]
        public HttpResponseMessage ApplyRGCIOnRapport(RapportModel rapport)
        {
            try
            {
                // TFS 4538 : les lignes de rapport supprimées dans la vue ne doivent pas être prise en compte ici
                // On ajoute une ligne que si le rapport n'en contient pas ou si toutes sont dans l'état "supprimé"
                if (rapport.ListLignes.All(l => l.IsDeleted))
                {
                    rapport = this.mapper.Map<RapportModel>(rapportManager.AddNewPointageReelToRapport(this.mapper.Map<RapportEnt>(rapport)));
                }
                rapport = this.mapper.Map<RapportModel>(rapportManager.ApplyValuesRgRapport(this.mapper.Map<RapportEnt>(rapport), Entities.Constantes.EntityType.CI));
                return Request.CreateResponse(HttpStatusCode.OK, rapport);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST d'application des règles de gestion sur une ligne de pointage
        /// </summary>
        /// <param name="pointage">Un pointage</param>
        /// <returns>Un pointage auquel on apllique les RG du code absence</returns>
        [HttpPost]
        [Route("api/Rapport/ApplyRGCodeAbsenceOnPointage")]
        public HttpResponseMessage ApplyRGCodeAbsenceOnPointage(RapportLigneModel pointage)
        {
            try
            {
                pointage = this.mapper.Map<RapportLigneModel>(pointageManager.ApplyValuesRGPointageReel(this.mapper.Map<RapportLigneEnt>(pointage), Entities.Constantes.EntityType.Absence));
                pointage = this.mapper.Map<RapportLigneModel>(pointageManager.ApplyReadOnlyRGPointageReel(this.mapper.Map<RapportLigneEnt>(pointage)));
                return Request.CreateResponse(HttpStatusCode.OK, pointage);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST d'application des règles de gestion sur une ligne de pointage
        /// </summary>
        /// <param name="pointage">Un pointage</param>
        /// <returns>Un pointage auquel on apllique les RG du code déplacement</returns>
        [HttpPost]
        [Route("api/Rapport/ApplyRGCodeDeplacementOnPointage")]
        public HttpResponseMessage ApplyRGCodeDeplacementOnPointage(RapportLigneModel pointage)
        {
            try
            {
                pointage = this.mapper.Map<RapportLigneModel>(pointageManager.ApplyValuesRGPointageReel(this.mapper.Map<RapportLigneEnt>(pointage), Entities.Constantes.EntityType.Deplacement));
                pointage = this.mapper.Map<RapportLigneModel>(pointageManager.ApplyReadOnlyRGPointageReel(this.mapper.Map<RapportLigneEnt>(pointage)));
                return Request.CreateResponse(HttpStatusCode.OK, pointage);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST d'application Pour le verrou d'un rapport
        /// </summary>
        /// <param name="rapport">Rapport à verrouiller</param>
        /// <returns>le retour de la requête</returns>
        [HttpPost]
        [Route("api/Rapport/VerrouillerRapport/")]
        public HttpResponseMessage VerrouillerRapport(RapportModel rapport)
        {
            try
            {
                var currentUser = utilisateurManager.GetContextUtilisateur();
                int demandeurId = currentUser.UtilisateurId;
                var rapportEnt = this.mapper.Map<RapportEnt>(rapport);
                var rapportBeforeUpdate = rapportManager.GetRapportByIdWithoutValidation(rapport.RapportId);
                rapportManager.CheckRapportStatutChangedInDb(rapportEnt, rapportBeforeUpdate);
                var rapportVerrouille = rapportManager.VerrouillerRapport(rapportEnt, demandeurId);
                this.ExportRapportPointageToSap(rapportVerrouille, rapportBeforeUpdate, utilisateurManager.GetContextUtilisateur());
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST d'application Pour le déverrouillage d'un rapport
        /// </summary>
        /// <param name="rapport">Rapport à déverrouiller</param>
        /// <returns>le retour de la requête</returns>
        [HttpPost]
        [Route("api/Rapport/DeverrouillerRapport/")]
        public HttpResponseMessage DeverrouillerRapport(RapportModel rapport)
        {
            try
            {
                var currentUser = utilisateurManager.GetContextUtilisateur();
                int demandeurId = currentUser.UtilisateurId;
                var rapportEnt = this.mapper.Map<RapportEnt>(rapport);
                var rapportBeforeUpdate = rapportManager.GetRapportByIdWithoutValidation(rapport.RapportId);
                rapportManager.CheckRapportStatutChangedInDb(rapportEnt, rapportBeforeUpdate);
                rapportManager.DeverrouillerRapport(rapportEnt, demandeurId);
                this.ExportRapportPointageToSap(rapportEnt, rapportBeforeUpdate, currentUser);
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        ///   Verrouille une liste de rapports en fonction d'un filtre
        ///   /!\ Risque de pb de perf dans FRED IE 
        /// </summary>
        /// <param name="filter">Filtre permettant de récupérer l'ensemble des rapports à verrouiler</param>
        /// <returns>Réponse HTTP</returns>
        [HttpPost]
        [Route("api/Rapport/Verrouiller/All")]
        public async Task<IHttpActionResult> LockRapportAllAsync([FromBody] SearchRapportModel filter)
        {
            SearchRapportEnt filterEnt = this.mapper.Map<SearchRapportEnt>(filter);
            var searchResult = rapportManager.SearchRapportWithFilter(filterEnt, null, null);
            List<int> rapportToExportForMateriel = new List<int>();
            List<RapportEnt> rapportsToLock = searchResult.Rapports;
            var currentUser = utilisateurManager.GetContextUtilisateur();
            List<int> rapportsEnCoursBeforeLock = rapportsToLock
                .Where(r => r.RapportStatutId == RapportStatutEnt.RapportStatutEnCours.Key)
                .Select(x => x.RapportId).ToList();

            List<int> rapportsId = rapportsToLock.Select(x => x.RapportId).ToList();
            List<int> notLockableRaportsIds = new List<int>();
            if (currentUser.Personnel.Societe.Groupe.Code != Constantes.CodeGroupeFES)
            {
                notLockableRaportsIds = rapportManager.GetListRapportIdWithError(rapportsId);
            }

            // Verrouillage de la liste
            LockRapportResponse response = rapportManager.VerrouillerListeRapport(rapportsId,
                                                                                     currentUser.UtilisateurId,
                                                                                     notLockableRaportsIds,
                                                                                     filterEnt,
                                                                                     currentUser.Personnel.Societe.Groupe.Code);

            // liste des rapports contenant des lignes avec materiel pour export J3G$
            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeRZB)
            {
                rapportToExportForMateriel = response.LockedRapports.Where(r => rapportsEnCoursBeforeLock.Contains(r.RapportId)
                                                                       && r.ListLignes.Any(l => l.Materiel != null && !l.DateSuppression.HasValue && !l.Materiel.MaterielLocation))
                                                          .Select(x => x.RapportId).ToList();
            }
            else
            {
                rapportToExportForMateriel = response.LockedRapports.Where(r => rapportsEnCoursBeforeLock.Contains(r.RapportId))
                                                           .Select(x => x.RapportId).ToList();
            }


            //// Les deux fonctions suivantes ont des instructions try catch (le catch ne renvoi pas d'exception, juste un log
            //// NB : On n'interrompt pas le verrouillage des rapports si les exports vers SAP plantent
            Task.Factory.StartNew(async () =>
            {
                // Flux J3G$ 
                await materielManagerExterne.ExportPointageMaterielToStormAsync(rapportToExportForMateriel, currentUser);
                // Flux CAT2 /!\ FAYAT TP UNIQUEMENT
                await rapportManagerExterne.ExportPointagePersonnelToSapAsync(response.LockedRapports.Select(x => x.RapportId).ToList(), currentUser);
            });

            return Ok(new VerrouillageRapportResponse { NotLockableRaportsIds = notLockableRaportsIds, PartialLockedReportIds = response.PartialLockedReport.ToList() });
        }

        /// <summary>
        /// Déverrouillage d'une liste de rapports en fonction d'un filtre
        /// </summary>
        /// <param name="filter">Filtre permettant de récupérer l'ensemble des rapports à déverrouiler</param>
        /// <returns>Réponse HTTP</returns>
        [HttpPost]
        [Route("api/Rapport/Deverrouiller/All")]
        public HttpResponseMessage UnlockRapportAll([FromBody] SearchRapportModel filter)
        {
            return Post(() =>
            {
                int demandeurId = utilisateurManager.GetContextUtilisateurId();
                var searchResult = rapportManager.SearchRapportWithFilter(this.mapper.Map<SearchRapportEnt>(filter), null, null);
                List<RapportEnt> rapportsToUnlock = searchResult.Rapports;
                // Déverrouillage de la liste
                rapportManager.DeverrouillerListeRapport(rapportsToUnlock.Select(x => x.RapportId).ToList(), demandeurId);
                return HttpStatusCode.OK;
            });
        }

        /// <summary>
        ///   Verrouille une liste de rapports en fonction d'un filtre
        ///   /!\ Risque de pb de perf dans FRED IE 
        /// </summary>
        /// <param name="filter">Filtre permettant de récupérer l'ensemble des rapports à verrouiler</param>
        /// <returns>Réponse HTTP</returns>
        [HttpPost]
        [Route("api/Rapport/Verrouiller/List")]
        public async Task<IHttpActionResult> LockRapportListAsync([FromBody] VerrouillageRapportModel verrouillageRapportModel)
        {
            var currentUser = utilisateurManager.GetContextUtilisateur();
            List<int> rapportToExportForMateriel = new List<int>();
            List<int> rapportsEnCoursBeforeLock = verrouillageRapportModel.RapportList.Where(r => r.RapportStatutId == RapportStatutEnt.RapportStatutEnCours.Key).Select(x => x.RapportId).ToList();
            List<int> rapportsId = verrouillageRapportModel.RapportList.Select(x => x.RapportId).ToList();
            List<int> notLockableRaportsIds = new List<int>();
            IEnumerable<int> partialLockedReport = new List<int>();
            SearchRapportEnt filter = this.mapper.Map<SearchRapportEnt>(verrouillageRapportModel.Filter);
            if (currentUser.Personnel.Societe.Groupe.Code != Constantes.CodeGroupeFES)
            {
                notLockableRaportsIds = rapportManager.GetListRapportIdWithError(rapportsId);
            }

            // Verrouillage de la liste
            LockRapportResponse response = rapportManager.VerrouillerListeRapport(rapportsId,
                                                                                     currentUser.UtilisateurId,
                                                                                     notLockableRaportsIds,
                                                                                     filter,
                                                                                     currentUser.Personnel.Societe.Groupe.Code);

            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeRZB)
            {
                rapportToExportForMateriel = response.LockedRapports.Where(r => rapportsEnCoursBeforeLock.Contains(r.RapportId)
                                                                        && r.ListLignes.Any(l => l.Materiel != null && !l.DateSuppression.HasValue && !l.Materiel.MaterielLocation))
                                                          .Select(x => x.RapportId).ToList();
            }
            else
            {
                rapportToExportForMateriel = response.LockedRapports.Where(r => rapportsEnCoursBeforeLock.Contains(r.RapportId))
                                                           .Select(x => x.RapportId).ToList();
            }
            //// Les deux fonctions suivantes ont des instructions try catch (le catch ne renvoi pas d'exception, juste un log
            //// NB : On n'interrompt pas le verrouillage des rapports si les exports vers SAP plantent

            Task.Factory.StartNew(async () =>
            {
                // Flux J3G$ 
                await materielManagerExterne.ExportPointageMaterielToStormAsync(rapportToExportForMateriel, currentUser);
                // Flux CAT2 /!\ SOMOPA UNIQUEMENT
                await rapportManagerExterne.ExportPointagePersonnelToSapAsync(response.LockedRapports.Select(x => x.RapportId).ToList(), currentUser);
            });

            return Ok(new VerrouillageRapportResponse { NotLockableRaportsIds = notLockableRaportsIds, PartialLockedReportIds = response.PartialLockedReport.ToList() });
        }

        /// <summary>
        /// Déverrouillage d'une liste de rapports en fonction d'un filtre
        /// </summary>
        /// <param name="rapports">Filtre permettant de récupérer l'ensemble des rapports à déverrouiler</param>
        /// <returns>Réponse HTTP</returns>
        [HttpPost]
        [Route("api/Rapport/Deverrouiller/List")]
        public HttpResponseMessage UnlockRapportList([FromBody] List<RapportModel> rapports)
        {
            return Post(() =>
            {
                int demandeurId = utilisateurManager.GetContextUtilisateurId();
                // Déverrouillage de la liste
                rapportManager.DeverrouillerListeRapport(rapports.Select(x => x.RapportId).ToList(), demandeurId);
                return HttpStatusCode.OK;
            });
        }

        /// <summary>
        /// Méthode POST d'application Pour la validation des rapports
        /// </summary>
        /// <param name="rapport">Un rapport</param>
        /// <returns>le retour de la requête</returns>
        [HttpPost]
        [Route("api/Rapport/ValidationRapport")]
        public HttpResponseMessage ValidationRapport(RapportModel rapport)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
                }

                var currentUser = utilisateurManager.GetContextUtilisateur();
                int demandeurId = currentUser.UtilisateurId;
                var rapportEnt = this.mapper.Map<RapportEnt>(rapport);
                var rapportBeforeUpdate = rapportManager.GetRapportByIdWithoutValidation(rapport.RapportId);
                rapportManager.CheckRapportStatutChangedInDb(rapportEnt, rapportBeforeUpdate);
                bool success = rapportManager.ValidationRapport(rapportEnt, demandeurId);

                if (!success)
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden);
                }

                this.ExportRapportPointageToSap(rapportEnt, rapportBeforeUpdate, utilisateurManager.GetContextUtilisateur());

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (ValidationException ex)
            {
                return GetValidationErrorResponse(ex);
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
        [Route("api/Rapport/GetOrCreateIndemniteDeplacement")]
        public HttpResponseMessage GetOrCreateIndemniteDeplacement(RapportLigneModel rapportLigne)
        {
            try
            {
                List<string> warnings;
                rapportLigne = this.mapper.Map<RapportLigneModel>(pointageManager.GetOrCreateIndemniteDeplacementForRapportLigne(this.mapper.Map<RapportLigneEnt>(rapportLigne), out warnings, false));
                rapportLigne.Warnings = warnings;
                return Request.CreateResponse(HttpStatusCode.OK, rapportLigne);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST de rafraichissement des indémnités de déplacement.
        /// </summary>
        /// <param name="rapportLigne">Une ligne de rapport</param>
        /// <returns>le retour de la requête</returns>
        [HttpPost]
        [Route("api/Rapport/RefreshIndemniteDeplacement")]
        public HttpResponseMessage RefreshIndemniteDeplacement(RapportLigneModel rapportLigne)
        {
            try
            {
                List<string> warnings;
                rapportLigne = this.mapper.Map<RapportLigneModel>(pointageManager.GetOrCreateIndemniteDeplacementForRapportLigne(this.mapper.Map<RapportLigneEnt>(rapportLigne), out warnings, true));
                rapportLigne.Warnings = warnings;
                return Request.CreateResponse(HttpStatusCode.OK, rapportLigne);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode pour initialiser les informations des sorties astreintes pour un rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <returns>le retour de la requête</returns>
        [HttpPost]
        [Route("api/Rapport/InitializeAstreintesInformations")]
        public HttpResponseMessage InitializeAstreintesInformations(RapportModel rapport)
        {
            try
            {
                rapportManager.InitializeAstreintesInformations(this.mapper.Map<RapportEnt>(rapport));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode pour de remplir les informations des sorties astreintes pour un rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <returns>le retour de la requête</returns>
        [HttpPost]
        [Route("api/Rapport/FulfillAstreintesInformations")]
        public HttpResponseMessage FulfillAstreintesInformations(RapportModel rapport)
        {
            try
            {
                var result = this.mapper.Map<RapportModel>(rapportManager.FulfillAstreintesInformations(this.mapper.Map<RapportEnt>(rapport)));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        ///   POST récupère une liste de pointage vérouiller par rapport à un personnelId
        /// </summary>
        /// <param name="personnelId">Identifiant unique d'un personnel</param>
        /// <returns>retouner une liste de pointage vérouiller</returns>
        [HttpGet]
        [Route("api/Rapport/Pointage/Personnel/Verrouiller/{personnelId}")]
        public HttpResponseMessage GetPointageVerrouillerByPersonnelId(int personnelId)
        {
            return Get(() => this.mapper.Map<IEnumerable<RapportLigneModel>>(pointageManager.GetPointageVerrouillerByPersonnelId(personnelId)));
        }

        private void ExportRapportPointageToSap(RapportEnt rapport, RapportEnt rapportBeforeUpdate, UtilisateurEnt currentUser)
        {
            // Envoi des pointages personnels si un verrouillage a eu lieu ou si il y a eu des modifications de personnel
            // pas d'envoi si le rapport comporte des erreurs
            var sendPersonnel = rapport.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key
                                    && !rapport.ListErreurs.Any()
                                    && (rapport.RapportStatutId != rapportBeforeUpdate.RapportStatutId
                                        || rapportManager.CheckRapportLignesPersonnelChanged(rapport, rapportBeforeUpdate));
            // Envoi des pointages matériels si le statut est passé de en cours à validé
            // ou si il y a eu des modifications de personnel 
            // pas d'envoi pour le statut en cours
            // pas d'envoi si le rapport comporte des erreurs
            // pas d'envoi si le rapport ne contient pas de materiel interne pour la cas RZB
            bool sendMateriel;
            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeRZB)
            {
                sendMateriel = rapport.RapportStatutId != RapportStatutEnt.RapportStatutEnCours.Key
                             && !rapport.ListErreurs.Any()
                             && (rapportBeforeUpdate.RapportStatutId == RapportStatutEnt.RapportStatutEnCours.Key
                                 || rapportManager.CheckRapportLignesMaterielChanged(rapport, rapportBeforeUpdate))
                             && (rapportBeforeUpdate.ListLignes.Any(l => l.Materiel != null && !l.DateSuppression.HasValue && !l.Materiel.MaterielLocation)
                                || rapport.ListLignes.Any(l => l.Materiel != null && !l.DateSuppression.HasValue && !l.Materiel.MaterielLocation));
            }
            else
            {
                sendMateriel = rapport.RapportStatutId != RapportStatutEnt.RapportStatutEnCours.Key
                                && !rapport.ListErreurs.Any()
                                && (rapportBeforeUpdate.RapportStatutId == RapportStatutEnt.RapportStatutEnCours.Key && rapport.ListLignes.Any(l => l.MaterielId.HasValue)
                                    || rapportManager.CheckRapportLignesMaterielChanged(rapport, rapportBeforeUpdate));
            }

            Task.Factory.StartNew(async () =>
            {
                // Flux CAT2
                if (sendPersonnel)
                {
                    await rapportManagerExterne.ExportPointagePersonnelToSapAsync(rapport.RapportId, currentUser);
                }

                // Flux J3G$
                if (sendMateriel)
                {
                    await materielManagerExterne.ExportPointageMaterielToStormAsync(rapport.RapportId, currentUser);
                }
            });
        }

        /// <summary>
        /// Get list des majorations affected to list of personnel
        /// </summary>
        /// <param name="majorationPersonnelModel">majoration Personnel get model</param>
        /// <returns>List des primes affected</returns>
        [HttpGet]
        [Route("api/Rapport/MajorationPersonnelAffected")]
        public HttpResponseMessage MajorationPersonnelAffected([FromUri] MajorationPersonnelGetModel majorationPersonnelModel)
        {
            return this.Get(() => this.mapper.Map<IEnumerable<MajorationPersonnelAffectationModel>>(pointageManager.MajorationPersonnelAffected(this.mapper.Map<MajorationPersonnelsGetEnt>(majorationPersonnelModel))));
        }

        /// <summary>
        ///   Méthode de génération
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/Rapport/IsExcelControlePointagesNotEmpty")]
        public HttpResponseMessage IsExcelControlePointagesNotEmpty(EtatPaieExportModel etatPaieExportModel)
        {
            try
            {
                return this.Post(() => pointageManager.IsExcelControlePointagesNotEmpty(etatPaieExportModel));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Rechercher les auteurs de rapport
        /// {page?}/{pageSize?}/{recherche?}/{societeId?}/{ciId?}/{statut?}/{onlyUtilisateur?}
        /// </summary>
        /// <param name="search">Object de recherche</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Rapport/SearchAuthor")]
        public HttpResponseMessage SearchAuthor([FromUri] SearchLightPersonnelModel search)
        {
            var user = utilisateurManager.GetContextUtilisateur();
            int? userId = user?.UtilisateurId;
            int? currentUserGroupeId = user?.Personnel?.Societe?.GroupeId;
            var listOrga = organisationManager.GetOrganisationsAvailable(null, new List<int> { Entities.OrganisationType.Ci.ToIntValue(), Entities.OrganisationType.SousCi.ToIntValue() }, userId, null).Select(o => o.OrganisationId);
            return Get(() => mapper.Map<IEnumerable<PersonnelLightForPickListModel>>(rapportManager.SearchRapportAuthor(search, currentUserGroupeId, listOrga)));
        }

        /// <summary>
        /// Permet de controler les saisies pour Tibco
        /// </summary>
        /// <param name="filter">Object de recherche</param>
        /// <returns>liste de model erreur dans le content de la réponse http</returns>
        [HttpPost]
        [Route("api/Rapport/ControlerSaisiesForTibco")]
        public HttpResponseMessage ControlerSaisiesForTibco(ExportAnalytiqueFilterModel filter)
        {
            return Post(() => pointageManager.ControleSaisiesForTibco(mapper.Map<ExportPointagePersonnelFilterModel>(filter)));
        }

        /// <summary>
        /// Permet d'envoyer les paramètres de selection à Tibco pour qu'il puisse appeler api Fred ie 
        /// </summary>
        /// <param name="filter">Object de recherche</param>
        /// <returns>réponse ok</returns>
        [HttpPost]
        [Route("api/Rapport/ExporterAnalytiqueForTibco")]
        public async Task<IHttpActionResult> ExporterAnalytiqueForTibcoAsync(ExportAnalytiqueFilterModel filter)
        {
            var modelTibcoInput = mapper.Map<ExportPointagePersonnelFilterModel>(filter);
            string errorMessage = pointageManager.CheckExportAnalytiqueErrors(modelTibcoInput);
            if (string.IsNullOrEmpty(errorMessage))
            {
                await rapportManagerExterne.ExportPointagePersonnelToTibcoAsync(modelTibcoInput);
            }

            return Ok(errorMessage);
        }
    }
}
