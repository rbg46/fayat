using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Fred.Business.Action.CommandeLigne;
using Fred.Business.Action.CommandeLigne.Models;
using Fred.Business.Action.Models;
using Fred.Business.Commande;
using Fred.Business.Organisation.Tree;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Commande;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Commande;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.CommandeLigne.VME22
{
    public class Vme22FluxManager : IVme22FluxManager
    {
        private const string FluxName = "VME22";
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICommandeLigneManager commandeLigneManager;
        private readonly ICommandeFluxHelper commandeFluxHelper;
        private readonly IApplicationsSapManager applicationsSapManager;
        private readonly ICommandeManager commandeManager;

        private readonly ISocieteManager societeManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IActionCommandeLigneService actionCommandeLigneService;

        public Vme22FluxManager(
            IUtilisateurManager utilisateurManager,
            ICommandeLigneManager commandeLigneManager,
            ICommandeFluxHelper commandeFluxHelper,
            IApplicationsSapManager applicationsSapManager,
            ICommandeManager commandeManager,
            ISocieteManager societeManager,
            IOrganisationTreeService organisationTreeService,
            IActionCommandeLigneService actionCommandeLigneService)
        {
            this.utilisateurManager = utilisateurManager;
            this.commandeLigneManager = commandeLigneManager;
            this.commandeFluxHelper = commandeFluxHelper;
            this.applicationsSapManager = applicationsSapManager;
            this.commandeManager = commandeManager;
            this.societeManager = societeManager;
            this.organisationTreeService = organisationTreeService;
            this.actionCommandeLigneService = actionCommandeLigneService;
        }
        /// <summary>
        /// Créé le job ExportManualUnlockLigneDeCommandeToSap
        /// </summary>
        /// <param name="parameters">paramètres</param>       
        public void EnqueueExportManualLockUnlockLigneDeCommande(ExportManualLockUnlockLigneDeCommandeParameters parameters)
        {
            commandeFluxHelper.EnqueueJob(() => ExportManualLockUnlockLigneDeCommandeToSap(parameters, null));
        }

        /// <summary>
        /// Permet d'exporter un vérouillage / déverouillage de ligne de commande vers SAP
        /// </summary>
        /// <param name="parameters">paramètres</param>
        /// <param name="context">context hangfire</param>
        /// <returns>La commande exportée.</returns>
        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[COMMANDE][FLUX_VME22] Export déverouillage manuel vers SAP")]
        public async Task<CommandeLigneLockSapModel> ExportManualLockUnlockLigneDeCommandeToSap(ExportManualLockUnlockLigneDeCommandeParameters parameters, PerformContext context)
        {
            parameters.JobId = context.BackgroundJob.Id;

            return await JobRunnerApiRestHelper.PostAsync<CommandeLigneLockSapModel>("ExportManualUnlockLigneDeComandeToSap", parameters.AuteurModificationGroupeCode, parameters);
        }

        public async Task<CommandeLigneLockSapModel> ExportManualLockUnlockLigneDeCommandeToSapJob(ExportManualLockUnlockLigneDeCommandeParameters parameters)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("[COMMANDE][FLUX_VME22] Export déverouillage manuel vers SAP");
            NLog.LogManager.GetCurrentClassLogger().Info($"[COMMANDE][FLUX_VME22][PARAMETRES] CommandeLigneId {parameters.CommandeLigneId} AuteurModificationId {parameters.AuteurModificationId} DateVerouillage {parameters.DateVerouillage}");

            ActionCommandeLigneInputModel inputModel = BuildActionLockUnlockInputModel(
                parameters.CommandeLigneId,
                parameters.IsLocked,
                parameters.JobId,
                FluxName,
                ActionStatus.Pending,
                parameters.AuteurModificationId);

            try
            {
                actionCommandeLigneService.CreateActionCommandeLigne(inputModel);

                UtilisateurEnt auteurModification = utilisateurManager.GetById(parameters.AuteurModificationId);
                CommandeLigneEnt commandeLigne = await commandeLigneManager.FindByIdAsync(parameters.CommandeLigneId);
                CommandeEnt commande = await commandeManager.FindByIdAsync(commandeLigne.CommandeId);

                OrganisationTree organisationTree = organisationTreeService.GetOrganisationTree();
                OrganisationBase organisationBase = organisationTree.GetSocieteParentOfCi(commande.CiId.Value);
                OrganisationBase ciOrganisationBase = organisationTree.GetCi(commande.CiId.Value);
                SocieteEnt societe = await societeManager.FindByIdAsync(organisationBase.Id);

                var commandeLigneUnlockSapModel = new CommandeLigneLockSapModel
                {
                    CommandeLigneId = parameters.CommandeLigneId,
                    DateVerouillage = parameters.IsLocked ? parameters.DateVerouillage : default(DateTime?),
                    AuteurModification = auteurModification.PrenomNom,
                    Numero = commande.Numero,
                    SocieteComptableCode = societe.CodeSocieteComptable,
                    CiCode = ciOrganisationBase.Code,
                    NumeroCommandeExterne = commandeLigne.NumeroCommandeLigneExterne,
                    CommandeLigneSap = commande.NumeroCommandeExterne
                };

                var request = new List<CommandeLigneLockSapModel>()
                {
                    commandeLigneUnlockSapModel
                };

                await SendJobAsync(request, societe.SocieteId, "VME22");

                inputModel.ActionInput.ActionStatus = ActionStatus.Success;
                actionCommandeLigneService.CreateActionCommandeLigne(inputModel);

                return commandeLigneUnlockSapModel;
            }
            catch (Exception exception)
            {
                inputModel.ActionInput.ActionStatus = ActionStatus.Failed;
                inputModel.ActionInput.Message += " - " + exception.Message;
                actionCommandeLigneService.CreateActionCommandeLigne(inputModel);
                NLog.LogManager.GetCurrentClassLogger().Error("[COMMANDE][FLUX_VME22] Export déverouillage manuel vers SAP");
                NLog.LogManager.GetCurrentClassLogger().Error($"[COMMANDE][FLUX_VME22][PARAMETRES] CommandeLigneId {parameters.CommandeLigneId} AuteurModificationId {parameters.AuteurModificationId} DateVerouillage {parameters.DateVerouillage}");
                throw new FredIeBusinessException(exception.Message, exception);
            }
        }


        private async Task SendJobAsync(object cmdSap, int societeId, string fluxCode)
        {
            var applicationSapParameter = applicationsSapManager.GetParametersForSociete(societeId);
            if (!applicationSapParameter.IsFound)
            {
                throw new FredBusinessException(FredImportExportBusinessResources.CanExportError);
            }

            var result = await commandeFluxHelper.SendJobAsync(cmdSap, fluxCode, applicationSapParameter.Login, applicationSapParameter.Password, applicationSapParameter.Url);
            await LogResponseAsync(result, fluxCode);
        }

        private async Task LogResponseAsync(HttpResponseMessage response, string fluxCode)
        {
            if (response != null)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"[COMMANDE][FLUX_{fluxCode}] Réponse de Sap : {(int)response.StatusCode} : {response.StatusCode.ToString()}");
                NLog.LogManager.GetCurrentClassLogger().Info($"[COMMANDE][FLUX_{fluxCode}] Réponse de Sap détaillée : {response.ToString()}");
                NLog.LogManager.GetCurrentClassLogger().Info($"[COMMANDE][FLUX_{fluxCode}] Réponse de Sap Content : {await response.Content.ReadAsStringAsync()}");
            }
            else
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"[COMMANDE][FLUX_{fluxCode}] Pas de réponse de Sap.");
            }
        }

        private ActionCommandeLigneInputModel BuildActionLockUnlockInputModel(int commandeLigneId, bool toLock, string jobId, string jobName, ActionStatus? actionStatus, int auteurId)
        {
            return new ActionCommandeLigneInputModel
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
        }
    }
}
