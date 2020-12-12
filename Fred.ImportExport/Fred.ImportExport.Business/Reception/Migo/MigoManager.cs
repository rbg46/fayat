using AutoMapper;
using Fred.Business.Action.CommandeLigne;
using Fred.Business.Action.CommandeLigne.Models;
using Fred.Business.Action.Models;
using Fred.Business.Commande;
using Fred.Business.Reception.FredIe;
using Fred.Business.Reception.Services;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Models;
using Fred.Entities.Models.Flux.Depense;
using Fred.Entities.Societe;
using Fred.Framework.Models;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.Reception.Migo.Service;
using Fred.ImportExport.Business.Reception.Services;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Depense;
using Hangfire;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fred.ImportExport.Business.Reception.Migo
{
    public class MigoManager : IMigoManager
    {
        private const string FluxName = "MIGO";
        private readonly string filteredReceptionsReason = "QuantiteZeroJobId";
        private readonly string depenseFluxError = "Plusieurs réceptions de la sélection n'ont pas pu être visées, veuillez contacter le support.";

        private readonly IVisableReceptionProviderService visableReceptionProviderService;
        private readonly IDateComptableModifierForMigo dateComptableModifierForMigo;
        private readonly IActionCommandeLigneService actionCommandeLigneService;
        private readonly IGroupeRepository groupRepository;
        private readonly ISocieteManager societeManager;
        private readonly IApplicationsSapManager applicationsSapManager;
        private readonly IMapper mapper;
        private readonly IReceptionSapProviderService fredIeReceptionService;
        private readonly IMigoService migoService;

        public MigoManager(IReceptionSapProviderService fredIeReceptionService,
            IApplicationsSapManager applicationsSapManager,
            IMapper mapper,
            ISocieteManager societeManager,
            IVisableReceptionProviderService visableReceptionProviderService,
            IDateComptableModifierForMigo dateComptableModifierForMigo,
             IActionCommandeLigneService actionCommandeLigneService,
            IGroupeRepository groupRepository,
            IMigoService migoService)
        {
            this.visableReceptionProviderService = visableReceptionProviderService;
            this.dateComptableModifierForMigo = dateComptableModifierForMigo;
            this.groupRepository = groupRepository;
            this.actionCommandeLigneService = actionCommandeLigneService;
            this.societeManager = societeManager;
            this.applicationsSapManager = applicationsSapManager;
            this.mapper = mapper;
            this.fredIeReceptionService = fredIeReceptionService;
            this.migoService = migoService;
        }

        /// <summary>
        /// Execute l'envoie des Migo vers Sap, en splittant par societe
        /// </summary>
        /// <param name="receptionIds">Liste des receptions a envoyer</param>
        /// <returns>Un job par societe, donc un jobid avec les receptions de la societe</returns>
        public List<ResultModel<DepenseFluxResponseModel>> ManageExportReceptionToSap(List<int> receptionIds)
        {
            ReceptionFilterForSap receptionFilterForSap = FilterReceptionForSap(receptionIds);

            List<ApplicationSapRequestParameter<List<int>>> requests = GenerateSapRequests(receptionFilterForSap.ValidReceptions);

            List<Result<DepenseFluxResponseModel>> responses = ExecuteJobs(requests);

            // On créé les réponses pour les receptions qui n'ont pas été envoyés à SAP volontairement 
            responses.AddRange(CreateResponseForIgnoredReceptions(receptionFilterForSap.FilteredReceptions));

            return mapper.Map<List<ResultModel<DepenseFluxResponseModel>>>(responses);
        }

        /// <summary>
        /// Filtre les réceptions qui seront ou ne seront pas envoyées à SAP
        /// </summary>
        /// <param name="receptionIds">Ids de toute les réceptions</param>
        /// <returns>ReceptionFilterForSap qui contient les réceptions envoyés et les réceptions filtrés</returns>
        private ReceptionFilterForSap FilterReceptionForSap(List<int> receptionIds)
        {
            ReceptionFilterForSap receptionFilterForSap = new ReceptionFilterForSap();

            IEnumerable<DepenseAchatEnt> receptions = fredIeReceptionService.GetReceptions(receptionIds).Where(visableReceptionProviderService.IsVisable);

            receptionFilterForSap.FilteredReceptions = receptions.Where(r => r.Quantite == 0).ToList();

            receptionFilterForSap.ValidReceptions = receptions.Where(r => !receptionFilterForSap.FilteredReceptions.Contains(r)).ToList();

            return receptionFilterForSap;
        }

        /// <summary>
        /// Créé une reponse pour les receptions qui ne sont pas envoyées à SAP
        /// </summary>
        /// <param name="filteredReceptions">Les réceptions qui ne sont pas envoyées à SAP</param>
        /// <returns>Une réponse contenant toutes les réceptions non envoyées, avec un JobId contenant la raison du filtre</returns>
        private List<Result<DepenseFluxResponseModel>> CreateResponseForIgnoredReceptions(List<DepenseAchatEnt> filteredReceptions)
        {
            List<Result<DepenseFluxResponseModel>> responses = new List<Result<DepenseFluxResponseModel>>();

            if (filteredReceptions.Count > 0)
            {
                DepenseFluxResponseModel successResponse = new DepenseFluxResponseModel()
                {
                    JobId = filteredReceptionsReason,
                    ReceptionsIds = filteredReceptions.Select(x => x.DepenseId).ToList()
                };
                responses.Add(Result<DepenseFluxResponseModel>.CreateSuccess(successResponse));
            }

            return responses;
        }

        /// <summary>
        /// Genère les requêtes pour l'envoi à SAP
        /// </summary>
        /// <param name="receptions">Les réceptions valides pour SAP</param>
        /// <returns>La liste des requêtes</returns>
        private List<ApplicationSapRequestParameter<List<int>>> GenerateSapRequests(List<DepenseAchatEnt> receptions)
        {
            List<ApplicationSapRequestParameter<List<int>>> requests = new List<ApplicationSapRequestParameter<List<int>>>();

            Dictionary<int, List<int>> receptionIdsBySocietes = SplitReceptionsBySociete(receptions);
            foreach (KeyValuePair<int, List<int>> receptionIdsBySociete in receptionIdsBySocietes)
            {
                int societeId = receptionIdsBySociete.Key;
                List<int> receptionsOnSociete = receptionIdsBySociete.Value;
                ApplicationSapParameter applicationSapParameter = applicationsSapManager.GetParametersForSociete(societeId);
                ApplicationSapRequestParameter<List<int>> request = new ApplicationSapRequestParameter<List<int>>(societeId, applicationSapParameter, receptionsOnSociete);
                requests.Add(request);
            }

            return requests;
        }

        private List<Result<DepenseFluxResponseModel>> ExecuteJobs(List<ApplicationSapRequestParameter<List<int>>> requests)
        {
            List<Result<DepenseFluxResponseModel>> result = new List<Result<DepenseFluxResponseModel>>();
            foreach (ApplicationSapRequestParameter<List<int>> request in requests)
            {
                if (request.ApplicationSapParameter.IsFound)
                {
                    string jobId = BackgroundJob.Enqueue(() => ExportReceptionToSapForSociete(request.SocieteId, request.Data, null));
                    DepenseFluxResponseModel successResponse = new DepenseFluxResponseModel()
                    {
                        JobId = jobId,
                        ReceptionsIds = request.Data
                    };
                    Result<DepenseFluxResponseModel> successResultForSociete = Result<DepenseFluxResponseModel>.CreateSuccess(successResponse);
                    result.Add(successResultForSociete);
                }
                else
                {
                    DepenseFluxResponseModel errorResponse = new DepenseFluxResponseModel()
                    {
                        JobId = string.Empty,//Pas de jobId si pas de config
                        ReceptionsIds = request.Data
                    };
                    Result<DepenseFluxResponseModel> echecResultForSociete = Result<DepenseFluxResponseModel>.CreateFailureWithData(depenseFluxError, errorResponse);
                    result.Add(echecResultForSociete);
                }
            }
            return result;
        }

        private Dictionary<int, List<int>> SplitReceptionsBySociete(List<DepenseAchatEnt> receptions)
        {
            Dictionary<int, List<int>> depensesByCiId = receptions
                                               .Where(d => d.CommandeLigne.Commande?.CI?.Organisation != null)
                                               .GroupBy(d => d.CommandeLigne.Commande.CI.Organisation.OrganisationId)
                                               .ToDictionary(x => x.Key, x => x.Select(r => r.DepenseId).ToList());

            Dictionary<int, List<int>> depensesBySociete = new Dictionary<int, List<int>>();

            foreach (KeyValuePair<int, List<int>> ciOrganisationId in depensesByCiId)
            {
                SocieteEnt societe = societeManager.GetSocieteByOrganisationId(ciOrganisationId.Key);
                if (depensesBySociete.ContainsKey(societe.SocieteId))
                {
                    depensesBySociete[societe.SocieteId].AddRange(ciOrganisationId.Value);
                }
                else
                {
                    depensesBySociete.Add(societe.SocieteId, ciOrganisationId.Value);
                }
            }

            return depensesBySociete;
        }


        /// <summary>
        /// WARNING !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// Cette methode ne sert que pour la retrocompatibilité.
        /// La modification de la signature de la methode ExportReceptionToSapForSociete ci-dessous
        /// nous oblige a garder aussi l'ancienne methode car sinon on ne pourra pas relancer les jobs hanfire
        /// Cette methode pourra etre supprimer dans 6 mois : nous somme le 28/05/2020 donc le 28/11/2020.
        /// </summary>
        /// <param name="societeId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[RECEPTION][2019][FLUX_MIGO] Export réception vers SAP {0}")]
        public async Task<List<ReceptionSapModel>> ExportReceptionToSapForSociete(int societeId, List<int> ids)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_MIGO][OLD METHOD] Demarrage du job { DateTime.Now }");
            ExportReceptionToSapForSocieteParameter parameter = new ExportReceptionToSapForSocieteParameter { SocieteId = societeId, ReceptionIds = ids, JobId = null };

            string groupCode = await groupRepository.GetGroupCodeByCompanyIdAsync(societeId);

            return await JobRunnerApiRestHelper.PostAsync<List<ReceptionSapModel>>("ExportMigoReceptionToSapForSociete", groupCode, parameter);
        }


        /// <summary>
        /// Exporte les receptions vers Sap (Flux MIGO)
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ids">La liste des identifants à récupérer</param>
        /// <returns>Une liste de dépenses.</returns>
        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[RECEPTION][2019][FLUX_MIGO] Export réception vers SAP {0}")]
        public async Task<List<ReceptionSapModel>> ExportReceptionToSapForSociete(int societeId, List<int> ids, PerformContext context)
        {
            ExportReceptionToSapForSocieteParameter parameter = new ExportReceptionToSapForSocieteParameter { SocieteId = societeId, ReceptionIds = ids, JobId = context.BackgroundJob.Id };

            string groupCode = await groupRepository.GetGroupCodeByCompanyIdAsync(societeId);

            return await JobRunnerApiRestHelper.PostAsync<List<ReceptionSapModel>>("ExportMigoReceptionToSapForSociete", groupCode, parameter);
        }

        public async Task<List<ReceptionSapModel>> ExportReceptionToSapForSocieteJobAsync(ExportReceptionToSapForSocieteParameter parameter)
        {
            int societeId = parameter.SocieteId;
            List<int> ids = parameter.ReceptionIds;

            //action commande ligne
            Dictionary<int, bool> listCommandeLigneIdsAdded = new Dictionary<int, bool>();
            bool withActionCommandeLigne = false;

            try
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_MIGO] Demarrage du job { DateTime.Now }");

                IEnumerable<DepenseAchatEnt> receptions = migoService.GetReceptionsFilteredForSap(ids);

                List<ReceptionSapModel> receptionsSap = new List<ReceptionSapModel>();

                foreach (DepenseAchatEnt reception in receptions)
                {
                    ReceptionSapModel receptionSap = mapper.Map<ReceptionSapModel>(reception);
                    if (reception.CommandeLigne.IsVerrou)
                    {
                        if (reception.CommandeLigneId != null && !listCommandeLigneIdsAdded.Any(x => x.Key == reception.CommandeLigneId.Value))
                        {
                            withActionCommandeLigne = true;

                            CreateActionCommandeLigne(parameter, reception.CommandeLigneId.Value, reception.CommandeLigne.IsVerrou, ActionStatus.Pending, string.Empty);

                            listCommandeLigneIdsAdded.Add(reception.CommandeLigneId.Value, reception.CommandeLigne.IsVerrou);
                        }

                        receptionSap.DateVerouillage = DateTime.UtcNow.Date;
                    }

                    migoService.UpdateQuantityAndDiminutionField(receptionSap);
                    receptionsSap.Add(receptionSap);
                }

                if (receptionsSap?.Count > 0)
                {
                    await ExportToSapAsync(societeId, receptionsSap);
                }
                else
                {
                    NLog.LogManager.GetCurrentClassLogger()
                      .Warn("[RECEPTION][FLUX_MIGO] Export réception vers SAP : Il n'y a aucune répcetion éligible à envoyer vers STORM.");
                }
                NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_MIGO] Fin du job { DateTime.Now }");

                if (withActionCommandeLigne)
                {
                    CreateActionCommandeLigne(parameter, listCommandeLigneIdsAdded, ActionStatus.Success, string.Empty);
                }

                return receptionsSap;
            }
            catch (Exception exception)
            {
                if (withActionCommandeLigne)
                {
                    CreateActionCommandeLigne(parameter, listCommandeLigneIdsAdded, ActionStatus.Failed, exception.Message);
                }

                LogResponseError(exception, societeId, ids);
                throw new FredIeBusinessException(exception.Message, exception);
            }
        }

        private async Task ExportToSapAsync(int societeId, List<ReceptionSapModel> receptionsSap)
        {
            SocieteEnt societe = societeManager.FindById(societeId);

            foreach (ReceptionSapModel receptionSap in receptionsSap)
            {
                receptionSap.SocieteComptableCode = societe?.CodeSocieteComptable;
                // BUG 8713 
                // La règle sur la date comptable est bien respectée par FRED mais l'heure utilisée (00h00m00s) est interprétée comme minuit dans SAP 
                // et donc prend en compte le jour précédent
                receptionSap.DateComptable = dateComptableModifierForMigo.AddOneMinuteForTheFirstDayOfMonth(receptionSap.DateComptable);
            }

            ApplicationSapParameter applicationSapParameter = applicationsSapManager.GetParametersForSociete(societeId);
            if (applicationSapParameter.IsFound)
            {
                // On envoie à SAP
                NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_MIGO] POST : {applicationSapParameter.Url}&ACTION=MIGO");

                RestClient restClient = new RestClient(applicationSapParameter.Login, applicationSapParameter.Password);

                HttpResponseMessage response = await restClient.PostAndEnsureSuccessAsync($"{applicationSapParameter.Url}&ACTION=MIGO", receptionsSap);

                LogResponse(response);

            }
            else
            {
                NLog.LogManager.GetCurrentClassLogger()
                  .Warn($"[RECEPTION][FLUX_MIGO] Export réception vers SAP : Il n'y a pas de configuration correspondant à cette société({societeId}).");
            }
        }

        private void CreateActionCommandeLigne(ExportReceptionToSapForSocieteParameter parameter, int commandeLigneId, bool isLocked, ActionStatus actionStatus, string addedMessage)
        {
            ActionCommandeLigneInputModel actionInputModel = BuildActionLockUnlockInputModel(
                commandeLigneId,
                isLocked,
                parameter.JobId,
                FluxName,
                actionStatus,
                0,
                addedMessage);
            actionCommandeLigneService.CreateActionCommandeLigne(actionInputModel);
        }

        private void CreateActionCommandeLigne(ExportReceptionToSapForSocieteParameter parameter, Dictionary<int, bool> listCommandeLigneIdsAdded, ActionStatus actionStatus, string addedMessage)
        {
            if (listCommandeLigneIdsAdded.Any())
            {
                foreach (KeyValuePair<int, bool> commandeLigneTemp in listCommandeLigneIdsAdded)
                {
                    ActionCommandeLigneInputModel actionInputModel = BuildActionLockUnlockInputModel(
                        commandeLigneTemp.Key,
                        commandeLigneTemp.Value,
                        parameter.JobId,
                        FluxName,
                        actionStatus,
                        0,
                        addedMessage);
                    actionCommandeLigneService.CreateActionCommandeLigne(actionInputModel);
                }
            }
        }


        /// <summary>
        /// Log l'erreur
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="societeId">societeId</param>
        /// <param name="ids">ids</param>
        private void LogResponseError(Exception exception, int societeId, List<int> ids)
        {
            try
            {
                string error = exception?.ToString();
                NLog.LogManager.GetCurrentClassLogger().Error($"[RECEPTION][FLUX_MIGO] Echec du job { DateTime.Now }");
                NLog.LogManager.GetCurrentClassLogger().Error($"[RECEPTION][FLUX_MIGO] societeId={societeId}, ids={string.Join(", ", ids)}");
                NLog.LogManager.GetCurrentClassLogger().Error($"[RECEPTION][FLUX_MIGO] Message={error}");
            }
            catch
            {
                // ici je fait rien car un log ne doit pas déclanché d'exception
            }
        }

        /// <summary>
        /// Log un message détaillé de la réponse Http
        /// </summary>
        /// <param name="response">réponse http</param>
        private void LogResponse(HttpResponseMessage response)
        {
            try
            {
                if (response != null)
                {
                    NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_MIGO] Réponse de Sap : {(int)response.StatusCode} : {response.StatusCode.ToString()}");
                    NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_MIGO] Réponse de Sap détaillée : {response}");
                    NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_MIGO] Réponse de Sap Content : {response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult()}");
                }
                else
                {
                    NLog.LogManager.GetCurrentClassLogger().Error($"[RECEPTION][FLUX_MIGO] Pas de réponse de Sap.");
                }
            }
            catch
            {
                // ici je fait rien car un log ne doit pas déclanché d'exception
            }
        }

        private ActionCommandeLigneInputModel BuildActionLockUnlockInputModel(int commandeLigneId, bool toLock, string jobId, string jobName, ActionStatus? actionStatus, int auteurId, string addedMessage)
        {
            ActionCommandeLigneInputModel actionInputModel = new ActionCommandeLigneInputModel
            {
                ActionInput = new ActionInputModel
                {
                    ActionStatus = actionStatus,
                    ActionType = toLock ? ActionType.Verrouillage : ActionType.Deverrouillage,
                    Message = CommandeResources.SendLockActionCommandeLigneToSap,
                    AuteurId = auteurId,
                    JobActionInput = new JobActionInputModel
                    {
                        ExternalJobId = jobId,
                        ExternalJobName = jobName
                    }
                },
                CommandeLigneId = commandeLigneId
            };

            if (addedMessage != string.Empty)
            {
                actionInputModel.ActionInput.Message += actionInputModel.ActionInput.Options.ConcatSeparator + addedMessage;
            }

            return actionInputModel;
        }
    }
}
