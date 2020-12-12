using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.Groupe;
using Fred.Business.Reception;
using Fred.Business.Reception.FredIe;
using Fred.Business.Societe;
using Fred.Entities.Depense;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.Reception.Services;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Models.Depense;
using Hangfire;
using Hangfire.Server;
using NLog;

namespace Fred.ImportExport.Business.Depense
{
    /// <summary>
    ///   Classe DepenseFluxManager <see cref="DepenseFluxManager"/>
    /// </summary>
    public class DepenseFluxManager : AbstractFluxManager
    {
        private readonly IReceptionManager receptionManager;
        private readonly IApplicationsSapManager applicationsSapManager;
        private readonly IMapper mapper;
        private readonly ISocieteManager societeManager;
        private readonly IReceptionSapProviderService receptionSapProviderService;
        private readonly IDateComptableModifierForMigo dateComptableModifierForMigo;
        private readonly IGroupeManager groupeManager;
        private readonly IFluxRepository fluxRepository;

        public string Code { get; } = ConfigurationManager.AppSettings["flux:far"];
        public string CodeMaterielExterne { get; } = ConfigurationManager.AppSettings["flux:reception:materiel:externe"];

        public DepenseFluxManager(
            IFluxManager fluxManager,
            IMapper mapper,
            ISocieteManager societeManager,
            IReceptionManager receptionManager,
            IApplicationsSapManager applicationsSapManager,
            IReceptionSapProviderService receptionSapProviderService,
            IDateComptableModifierForMigo dateComptableModifierForMigo,
            IGroupeManager groupeManager,
            IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            this.receptionManager = receptionManager;
            this.applicationsSapManager = applicationsSapManager;
            this.mapper = mapper;
            this.societeManager = societeManager;
            this.receptionSapProviderService = receptionSapProviderService;
            this.dateComptableModifierForMigo = dateComptableModifierForMigo;
            this.groupeManager = groupeManager;
            this.fluxRepository = fluxRepository;
        }

        public void ScheduleExport(string cron)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(CodeMaterielExterne, () => ExecuteExport(), cron);
            }
            else
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {CodeMaterielExterne}";
                var exception = new FredBusinessException(msg);
                LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }
        }

        public void ExecuteExport()
        {
            BackgroundJob.Enqueue(() => ExportMaterielExterne(null));
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[RECEPTION_MATERIEL_EXTERNE][FLUX_MIGO] Export réception materiel externe vers SAP")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task ExportMaterielExterne(PerformContext context)
        {
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(CodeMaterielExterne);
            var parameter = new ExportBySocieteParameter { CodeGroupe = groupCode, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("ExportMaterielExterne", groupCode, parameter);
        }

        public async Task ExportMaterielExterneJobAsync(ExportBySocieteParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            ValidateFlux();

            string backgroundJobId = parameter.BackgroundJobId;
            int? groupeId = await groupeManager.GetGroupeIdByCodeAsync(parameter.CodeGroupe);

            if (!groupeId.HasValue || groupeId.Value == 0)
            {
                throw new FredBusinessException(string.Format(FredImportExportBusinessResources.CodeGroupeDoesNotExist, CodeMaterielExterne));
            }

            IReadOnlyList<int> societeIds = societeManager.GetSocieteIdsByGroupeId(groupeId.Value);

            if (societeIds == null || !societeIds.Any())
            {
                throw new FredBusinessException(string.Format(FredImportExportBusinessResources.CompanyNotFoundForThisGroupe, groupeId.Value));
            }

            var receptionTasks = new List<Task>();
            foreach (int societeId in societeIds)
            {
                receptionTasks.Add(AddUpdateReceptionAsync(societeId, backgroundJobId));
            }

            await Task.WhenAll(receptionTasks);
        }

        private void ValidateFlux()
        {
            FluxEnt materielExterneFlux = FluxManager.GetByCode(CodeMaterielExterne);
            if (materielExterneFlux == null)
            {
                throw CreateAndLogFredBusinessException(IEBusiness.FluxInconnu);
            }

            if (!materielExterneFlux.IsActif)
            {
                throw CreateAndLogFredBusinessException(IEBusiness.FluxInactif);
            }

            FredBusinessException CreateAndLogFredBusinessException(string erreurMessage)
            {
                var msg = string.Format(erreurMessage, CodeMaterielExterne);
                var exception = new FredBusinessException(msg);
                LogManager.GetCurrentClassLogger().Error(exception, msg);
                return exception;
            }
        }

        private async Task AddUpdateReceptionAsync(int societeId, string backgroundJobId)
        {
            try
            {
                var ids = new List<int>();
                receptionManager.ReceptionMaterielExterne(societeId);

                ids.AddRange(receptionManager.GetReceptionMaterielExterneToSend());

                IEnumerable<DepenseAchatEnt> receptions = receptionSapProviderService.GetReceptionsFilteredForSap(ids);

                if (receptions.Any())
                {
                    var receptionsSap = new List<ReceptionSapModel>();

                    foreach (DepenseAchatEnt reception in receptions)
                    {
                        var receptionSap = mapper.Map<ReceptionSapModel>(reception);
                        SocieteEnt societe = societeManager.GetSocieteParentByOrgaId(reception.CI.Organisation.OrganisationId);
                        receptionSap.SocieteComptableCode = societe?.CodeSocieteComptable;
                        // BUG 8713 
                        // La règle sur la date comptable est bien respectée par FRED mais l'heure utilisée (00h00m00s) est interprétée comme minuit dans SAP 
                        // et donc prend en compte le jour précédent
                        receptionSap.DateComptable = dateComptableModifierForMigo.AddOneMinuteForTheFirstDayOfMonth(receptionSap.DateComptable);
                        receptionsSap.Add(receptionSap);
                    }

                    var societeIdForSend = (int)societeManager.GetSocieteIdByCodeSocieteComptable(receptionsSap.FirstOrDefault().SocieteComptableCode);
                    ApplicationSapParameter applicationSapParameter = applicationsSapManager.GetParametersForSociete(societeIdForSend);
                    if (applicationSapParameter.IsFound)
                    {
                        try
                        {
                            // On envoie à SAP
                            LogManager.GetCurrentClassLogger().Info($"[RECEPTION_MATERIEL_EXTERNE][FLUX_MIGO] POST : {applicationSapParameter.Url}&ACTION=MIGO]");
                            var restClient = new RestClient(applicationSapParameter.Login, applicationSapParameter.Password);
                            await restClient.PostAndEnsureSuccessAsync($"{applicationSapParameter.Url}&ACTION=MIGO", receptionsSap);
                            receptionManager.UpdateHangfireJobId(ids, backgroundJobId);
                        }
                        catch (Exception)
                        {
                            receptionManager.UpdateHangfireJobId(ids, backgroundJobId);
                        }
                    }
                    else
                    {
                        LogManager.GetCurrentClassLogger().Warn($"[RECEPTION_MATERIEL_EXTERNE][FLUX_MIGO] Export réception materiel externe vers SAP : Il n'y a pas de configuration correspondant à cette société({societeIdForSend}).");
                    }
                }
                else
                {
                    LogManager.GetCurrentClassLogger().Warn("[RECEPTION_MATERIEL_EXTERNE][FLUX_MIGO] Export réception materiel externe vers SAP : Il n'y a aucune répcetion éligible à envoyer vers SAP.");
                }
            }
            catch (Exception e)
            {
                IEBusinessLogger(IEBusiness.FluxErreurExport, e);
            }
        }

        private void IEBusinessLogger(string messageformat, Exception ex)
        {
            string msg = string.Format(messageformat, CodeMaterielExterne);
            var exception = new FredBusinessException(msg, ex);
            LogManager.GetCurrentClassLogger().Error(exception, exception.Message ?? msg);
        }
    }
}
