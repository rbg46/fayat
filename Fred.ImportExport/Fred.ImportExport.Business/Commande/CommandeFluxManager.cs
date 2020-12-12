using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.Commande.Services;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Framework.Models;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Commande;
using Hangfire;

namespace Fred.ImportExport.Business.Commande
{
    public class CommandeFluxManager : AbstractFluxManager
    {
        private readonly IMapper mapper;
        private readonly ISocieteManager societeManager;
        private readonly IApplicationsSapManager applicationsSapManager;
        private readonly ICommandeFluxService commandeFluxService;
        private readonly ICommandeStormImporter commandeStormImporter;
        private readonly ICommandeFluxHelper commandeFluxHelper;
        private readonly IGroupeRepository groupRepository;
        private readonly ISepService sepService;

        public CommandeFluxManager(
            IFluxManager fluxManager,
            IMapper mapper,
            ISocieteManager societeManager,
            IApplicationsSapManager applicationsSapManager,
            ICommandeFluxService commandeFluxService,
            ISepService sepService,
            ICommandeStormImporter commandeStormImporter,
            ICommandeFluxHelper commandeFluxHelper,
            IGroupeRepository groupRepository)
            : base(fluxManager)
        {
            this.mapper = mapper;
            this.societeManager = societeManager;
            this.applicationsSapManager = applicationsSapManager;
            this.commandeFluxService = commandeFluxService;
            this.sepService = sepService;
            this.commandeStormImporter = commandeStormImporter;
            this.commandeFluxHelper = commandeFluxHelper;
            this.groupRepository = groupRepository;
        }

        /// <summary>
        /// Execute le job si on trouve une conf SAP correspondant la société de la commande
        /// </summary>
        /// <param name="commandeId">commandeId</param>
        /// <returns>ResultModel avec Success si une conf a été trouvée</returns>
        public async Task<Result<string>> EnqueueExportCommandeJobAsync(int commandeId)
        {
            Result<bool> canExportResult = await CanExportCommandeJobAsync(commandeId, FredImportExportBusinessResources.CommandeFluxMe21Error);

            Result<string> exportResult = canExportResult.Success
                ? Result<string>.CreateSuccess(commandeFluxHelper.EnqueueJob(() => ExportCommandeToSap(commandeId)))
                : Result<string>.CreateFailure(canExportResult.Error);

            return exportResult;
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[COMMANDE][FLUX_ME21] Export commande vers SAP")]
        public async Task<CommandeSapModel> ExportCommandeToSap(int commandeId)
        {
            string groupCode = await groupRepository.GetGroupCodeByOrderIdAsync(commandeId);

            return await JobRunnerApiRestHelper.PostAsync<CommandeSapModel>("ExportCommandetoSap", groupCode, commandeId);
        }

        public async Task<CommandeSapModel> ExportCommandeToSapJobAsync(int commandeId)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("[COMMANDE][FLUX_ME21] Export commande vers SAP");
            NLog.LogManager.GetCurrentClassLogger().Info($"[COMMANDE][FLUX_ME21][PARAMETRES] commandeId {commandeId}");

            try
            {
                CommandeEnt cmd = await commandeFluxService.GetCommandeSAPAsync(commandeId, FredImportExportBusinessResources.CommandeFluxMe21Error);
                cmd.Lignes = cmd.Lignes.Where(l => l.AvenantLigneId == null).ToList();

                var cmdSap = mapper.Map<CommandeSapModel>(cmd);

                SetAgenceAndFournisseurAdresse(cmd, cmdSap);

                // On recherche la société du CI
                SocieteEnt societe = societeManager.GetSocieteParentByOrgaId(cmd.CI.Organisation.OrganisationId);
                cmdSap.SocieteCode = societe.Code;
                cmdSap.SocieteComptableCode = societe.CodeSocieteComptable;

                int societeId = await sepService.IsSepAsync(societe)
                    ? await sepService.GetSocieteAssocieIdGerantAsync(societe)
                    : societe.SocieteId;

                List<CommandeLigneEnt> lignes = cmd.Lignes.ToList();
                for (var i = 0; i < lignes.Count; i++)
                {
                    cmdSap.Lignes[i].NatureCode = lignes[i]?.Ressource?.ReferentielEtendus.FirstOrDefault(x => x.SocieteId == societeId)?.Nature.Code;
                }

                await SendJobAsync(cmdSap, societe.SocieteId, "ME21");

                return cmdSap;
            }
            catch (Exception exception)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
                NLog.LogManager.GetCurrentClassLogger().Error($"[COMMANDE][FLUX_ME21] commandeId {commandeId},erreur: {exception.Message}");
                throw new FredIeBusinessException(exception.Message, exception);
            }
        }

        /// <summary>
        /// Execute le job si on trouve une conf SAP correspondant la société de la commande
        /// </summary>
        /// <param name="commandeId">commandeId</param>
        /// <param name="numeroAvenant">numeroAvenant</param>
        /// <returns>ResultModel avec Success si une conf a été trouvée</returns>
        public async Task<Result<string>> EnqueueExportCommandeAvenantJobAsync(int commandeId, int numeroAvenant)
        {
            Result<bool> canExportResult = await CanExportCommandeJobAsync(commandeId, FredImportExportBusinessResources.CommandeFluxMe22Error);

            Result<string> exportResult = canExportResult.Success
                ? Result<string>.CreateSuccess(commandeFluxHelper.EnqueueJob(() => ExportCommandeAvenantToSap(commandeId, numeroAvenant)))
                : Result<string>.CreateFailure(canExportResult.Error);

            return exportResult;
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[COMMANDE][FLUX_ME22] Export avenant de commande vers SAP")]
        public async Task<CommandeAvenantSapModel> ExportCommandeAvenantToSap(int commandeId, int numeroAvenant)
        {
            var parameter = new ExportCommandeAvenantToSapParameter { CommandeId = commandeId, NumeroAvenant = numeroAvenant };
            string groupCode = await groupRepository.GetGroupCodeByOrderIdAsync(commandeId);

            return await JobRunnerApiRestHelper.PostAsync<CommandeAvenantSapModel>("ExportCommandeAvenantToSap", groupCode, parameter);
        }

        public async Task<CommandeAvenantSapModel> ExportCommandeAvenantToSapJobAsync(ExportCommandeAvenantToSapParameter parameter)
        {
            int commandeId = parameter.CommandeId;
            int numeroAvenant = parameter.NumeroAvenant;

            NLog.LogManager.GetCurrentClassLogger().Info("[COMMANDE][FLUX_ME22] Export avenant de commande vers SAP");
            NLog.LogManager.GetCurrentClassLogger().Info($"[COMMANDE][FLUX_ME22][PARAMETRES] commandeId {commandeId} numeroAvenant {numeroAvenant}");

            try
            {
                CommandeEnt cmd = await commandeFluxService.GetCommandeAvenantSAPAsync(commandeId, numeroAvenant, FredImportExportBusinessResources.CommandeFluxMe22Error);
                var cmdSap = mapper.Map<CommandeAvenantSapModel>(cmd);

                SetAgenceAndFournisseurAdresse(cmd, cmdSap);

                // On recherche la société du CI
                SocieteEnt societe = societeManager.GetSocieteParentByOrgaId(cmd.CI.Organisation.OrganisationId);
                cmdSap.SocieteCode = societe.Code;
                cmdSap.SocieteComptableCode = societe.CodeSocieteComptable;

                int societeId = await sepService.IsSepAsync(societe)
                  ? await sepService.GetSocieteAssocieIdGerantAsync(societe)
                  : societe.SocieteId;

                List<CommandeLigneEnt> lignes = cmd.Lignes.ToList();
                for (var i = 0; i < lignes.Count; i++)
                {
                    cmdSap.Lignes[i].NatureCode = lignes[i]?.Ressource?.ReferentielEtendus.FirstOrDefault(x => x.SocieteId == societeId)?.Nature.Code;
                }

                await SendJobAsync(cmdSap, societe.SocieteId, "ME22");

                return cmdSap;
            }
            catch (Exception exception)
            {
                NLog.LogManager.GetCurrentClassLogger().Error("[COMMANDE][FLUX_ME22] Export avenant de commande vers SAP");
                NLog.LogManager.GetCurrentClassLogger().Error($"[COMMANDE][FLUX_ME22][PARAMETRES] commandeId {commandeId} numeroAvenant {numeroAvenant}");
                throw new FredIeBusinessException(exception.Message, exception);
            }
        }

        [DisplayName("[COMMANDE][FLUX_ME23] Import des commandes STORM dans FRED")]
        public async Task ImportCommandeStorm(CommandeSapModel commandeStorm)
        {
            string groupCode = await groupRepository.GetGroupCodeByAccountingCompanyCodeAsync(commandeStorm.SocieteComptableCode);

            await JobRunnerApiRestHelper.PostAsync("ImportCommandeStorm", groupCode, commandeStorm);
        }

        public void ImportCommandeStormJob(CommandeSapModel commandeSap)
        {
            commandeStormImporter.Import(commandeSap);
        }

        private void SetAgenceAndFournisseurAdresse(CommandeEnt cmd, CommandeAdresseModel cmdSap)
        {
            if (cmd.AgenceId == null)
                return;

            if (cmd.FournisseurAdresse == cmd.Agence.Adresse?.Ligne
                && cmd.FournisseurCPostal == cmd.Agence.Adresse?.CodePostal
                && cmd.FournisseurVille == cmd.Agence.Adresse?.Ville)
            {
                cmdSap.AgenceAdresse = cmd.Agence.Adresse?.Ligne;
                cmdSap.AgenceCPostal = cmd.Agence.Adresse?.CodePostal;
                cmdSap.AgenceVille = cmd.Agence.Adresse?.Ville;
            }
            else
            {
                cmdSap.AgenceAdresse = cmd.FournisseurAdresse;
                cmdSap.AgenceCPostal = cmd.FournisseurCPostal;
                cmdSap.AgenceVille = cmd.FournisseurVille;
            }

            cmdSap.AgenceCode = cmd.Agence.Code;
            cmdSap.AgencePaysCode = cmd.Agence?.Adresse?.Pays?.Code;

            cmdSap.FournisseurCode = cmd.Agence?.Fournisseur?.Code;
            cmdSap.FournisseurAdresse = cmd.Agence?.Fournisseur?.Adresse;
            cmdSap.FournisseurCPostal = cmd.Agence?.Fournisseur?.CodePostal;
            cmdSap.FournisseurVille = cmd.Agence?.Fournisseur?.Ville;
            cmdSap.FournisseurPaysCode = cmd.Agence?.Fournisseur?.Pays?.Code;
        }

        private async Task SendJobAsync(object cmdSap, int societeId, string fluxCode)
        {
            ApplicationSapParameter applicationSapParameter = applicationsSapManager.GetParametersForSociete(societeId);
            if (!applicationSapParameter.IsFound)
            {
                throw new FredBusinessException(FredImportExportBusinessResources.CanExportError);
            }

            HttpResponseMessage result = await commandeFluxHelper.SendJobAsync(cmdSap, fluxCode, applicationSapParameter.Login, applicationSapParameter.Password, applicationSapParameter.Url);
            await LogResponseAsync(result, fluxCode);
        }

        private async Task LogResponseAsync(HttpResponseMessage response, string fluxCode)
        {
            if (response != null)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"[COMMANDE][FLUX_{fluxCode}] Réponse de Sap : {(int)response.StatusCode} : {response.StatusCode.ToString()}");
                NLog.LogManager.GetCurrentClassLogger().Info($"[COMMANDE][FLUX_{fluxCode}] Réponse de Sap détaillée : {response}");
                NLog.LogManager.GetCurrentClassLogger().Info($"[COMMANDE][FLUX_{fluxCode}] Réponse de Sap Content : {await response.Content.ReadAsStringAsync()}");
            }
            else
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"[COMMANDE][FLUX_{fluxCode}] Pas de réponse de Sap.");
            }
        }

        private async Task<Result<bool>> CanExportCommandeJobAsync(int commandeId, string errorMessage)
        {
            int organisationId = await commandeFluxService.GetOrganisationIdByCommandeIdAsync(commandeId);

            if (organisationId == 0)
            {
                return Result<bool>.CreateFailure($"{errorMessage} {commandeId.ToString()}");
            }

            SocieteEnt societe = societeManager.GetSocieteParentByOrgaId(organisationId);
            ApplicationSapParameter applicationSapParameter = applicationsSapManager.GetParametersForSociete(societe.SocieteId);

            return applicationSapParameter.IsFound
                ? Result<bool>.CreateSuccess(true)
                : Result<bool>.CreateFailure($"{errorMessage} {commandeId.ToString()}");
        }
    }
}
