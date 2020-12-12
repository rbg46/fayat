using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Reception;
using Fred.Business.Reception.FredIe;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Groupe;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.Reception.Services;
using Fred.ImportExport.Models.Depense;
using Fred.ImportExport.Models.Groupe;
using Hangfire;
using Hangfire.Server;
using NLog;

namespace Fred.ImportExport.Business.ReceptionInterimaire
{
    /// <summary>
    ///   Gestion de l'export des réception des intérimaires
    /// </summary>
    public class ReceptionInterimaireFluxManager : AbstractFluxManager
    {
        private readonly IReceptionManager receptionManager;
        private readonly IMapper mapper;
        private readonly IMatriculeExterneManager matriculeExterneManager;
        private readonly ISocieteManager societeManager;
        private readonly ICommandeContratInterimaireManager commandeContratInterimaireManager;
        private readonly IApplicationsSapManager applicationsSapManager;
        private readonly IGroupeInterimaireManager groupeInterimaireManager;
        private readonly ICIManager cIManager;
        private readonly IDateComptableModifierForMigo dateComptableModifierForMigo;
        private readonly IReceptionSapProviderService receptionSapProviderService;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IGroupeRepository groupRepository;
        private readonly ISepService sepService;

        public ReceptionInterimaireFluxManager(
            IFluxManager fluxManager,
            IReceptionManager receptionManager,
            IMapper mapper,
            IMatriculeExterneManager matriculeExterneManager,
            ISocieteManager societeManager,
            ICommandeContratInterimaireManager commandeContratInterimaireManager,
            IApplicationsSapManager applicationsSapManager,
            IGroupeInterimaireManager groupeInterimaireManager,
            ICIManager cIManager,
            IDateComptableModifierForMigo dateComptableModifierForMigo,
            IReceptionSapProviderService receptionSapProviderService,
            IUtilisateurManager utilisateurManager,
            IGroupeRepository groupRepository,
            ISepService sepService)
            : base(fluxManager)
        {
            this.receptionManager = receptionManager;
            this.mapper = mapper;
            this.matriculeExterneManager = matriculeExterneManager;
            this.societeManager = societeManager;
            this.commandeContratInterimaireManager = commandeContratInterimaireManager;
            this.applicationsSapManager = applicationsSapManager;
            this.groupeInterimaireManager = groupeInterimaireManager;
            this.cIManager = cIManager;
            this.dateComptableModifierForMigo = dateComptableModifierForMigo;
            this.receptionSapProviderService = receptionSapProviderService;
            this.utilisateurManager = utilisateurManager;
            this.groupRepository = groupRepository;
            this.sepService = sepService;
            Flux = FluxManager.GetByCode(ExportJobId);
        }

        /// <summary>
        /// Obtient l'identifiant du job d'exprot (Code du flux d'export).
        /// </summary>
        public string ExportJobId { get; } = ConfigurationManager.AppSettings["flux:reception:interimaire"];

        /// <summary>
        /// Exécution du l'export une fois.
        /// </summary>
        /// <param name="listSocieteId">Liste d'identifiant unique de societe</param>
        public void ExecuteExport(List<int> listSocieteId)
        {
            ExecuteExport(listSocieteId, utilisateurManager.GetByLogin("fred_ie").UtilisateurId);
        }

        /// <summary>
        /// Exécution du l'export une fois.
        /// </summary>
        /// <param name="listSocieteId">Liste d'identifiant unique de societe</param>
        /// <param name="userId">Flag pour savoir si fred web</param>
        public void ExecuteExport(List<int> listSocieteId, int userId)
        {
            ControlerExistanceFlux();

            foreach (int societeId in listSocieteId)
            {
                BackgroundJob.Enqueue(() => Export(societeId, null, userId));
            }
        }

        /// <summary>
        /// Exécution du l'export une fois.
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant unique de ci</param>
        /// <param name="userId">Flag pour savoir si fred web</param>
        public void ExecuteExportByCis(List<int> ciIds, int userId)
        {
            ControlerExistanceFlux();
            if (ciIds.Any())
            {
                BackgroundJob.Enqueue(() => Export(ciIds, null, userId));
            }
            else
            {
                LogManager.GetCurrentClassLogger().Warn(string.Format(IEBusiness.WarnNoDataFound, ExportJobId));
            }
        }

        /// <summary>
        /// Planifie l'exécution du job selon un CRON.
        /// </summary>
        /// <param name="cron">Le CRON à utiliser.</param>        
        public void ScheduleExport(string cron)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(ExportJobId, () => ExportRecurring(), cron);
            }
            else
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {ExportJobId}";
                var exception = new FredBusinessException(msg);
                LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }
        }

        public void ExportRecurring()
        {
            var listSocieteId = new List<int>();

            List<GroupeInterimaireModel> groupeInterimaires = groupeInterimaireManager.GetGroupeInterimaire();
            foreach (GroupeInterimaireModel groupe in groupeInterimaires)
            {
                listSocieteId.AddRange(groupe.SocieteNotInterimaires.Select(s => s.SocieteId));
            }

            ExecuteExport(listSocieteId);
        }

        /// <summary>
        /// Exporte.
        /// </summary>
        /// <param name="societeId">identifiant de la societe</param>
        /// <param name="context">permet de récupérer les propriétés du job hangfire et ainsi de récupérer le job id</param>
        /// <param name="utilisateurId">utilisateur qui a lancer le job</param>
        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[EXPORT] Réception intérimaire vers SAP")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task Export(int societeId, PerformContext context, int utilisateurId)
        {
            List<int> ciIds = cIManager.GetCiIdListBySocieteId(societeId);

            ciIds.AddRange(cIManager.GetCiIdsBySocieteIds(sepService.GetSepSocieteParticipantWhenSocieteIsGerante(societeId)));

            if (ciIds.Any())
                await Export(ciIds, context, utilisateurId);
        }

        /// <summary>
        /// Exporte.
        /// </summary>
        /// <param name="listCiId">identifiant unique des cis</param>
        /// <param name="context">permet de récupérer les propriétés du job hangfire et ainsi de récupérer le job id</param>
        /// <param name="utilisateurId">uilisateur qui a lancer le job</param>
        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[EXPORT] Réception intérimaire vers SAP")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task Export(List<int> listCiId, PerformContext context, int utilisateurId)
        {
            if (listCiId.Any())
            {
                var parameter = new ExportByCIParameter { ListCiId = listCiId, BackgroundJobId = context.BackgroundJob.Id, UtilisateurId = utilisateurId };
                string groupCode = await groupRepository.GetGroupCodeByCiIdsAsync(listCiId);

                await JobRunnerApiRestHelper.PostAsync("ExportReceptionInterimaire", groupCode, parameter);
            }
        }

        public async Task ExportReceptionInterimaireJob(ExportByCIParameter parameter)
        {
            List<int> listCiId = parameter.ListCiId;
            string backgroundJobId = parameter.BackgroundJobId;
            int utilisateurId = parameter.UtilisateurId;

            try
            {
                var ids = new List<int>();
                receptionManager.ReceptionInterimaire(listCiId, utilisateurId);

                ids.AddRange(receptionManager.GetReceptionInterimaireToSend(listCiId));

                List<DepenseAchatEnt> receptions = receptionSapProviderService.GetReceptionsFilteredForSap(ids).ToList();

                if (receptions.Any())
                {
                    var receptionsSap = new List<ReceptionSapModel>();

                    foreach (DepenseAchatEnt reception in receptions)
                    {
                        reception.CommandeLigne.Commande.CommandeContratInterimaire = commandeContratInterimaireManager.GetCommandeContratInterimaireByCommandeId(reception.CommandeLigne.CommandeId);
                        reception.CommandeLigne.Commande.CommandeContratInterimaire.Interimaire.MatriculeExterne = matriculeExterneManager.GetMatriculeExterneByPersonnelId(reception.CommandeLigne.Commande.CommandeContratInterimaire.InterimaireId);
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
                            LogManager.GetCurrentClassLogger().Info($"[RECEPTION_INTÉRIMAIRE][FLUX_ML81N] POST : {applicationSapParameter.Url}&ACTION=ML81N]");
                            var restClient = new RestClient(applicationSapParameter.Login, applicationSapParameter.Password);
                            await restClient.PostAndEnsureSuccessAsync($"{applicationSapParameter.Url}&ACTION=ML81N", receptionsSap);
                        }
                        catch (Exception e)
                        {
                            while (e.InnerException != null)
                            {
                                e = e.InnerException;
                            }

                            throw new FredBusinessException("[ERREUR SAP] - " + e.Message);
                        }
                        finally
                        {
                            receptionManager.UpdateHangfireJobId(ids, backgroundJobId);
                        }
                    }
                    else
                    {
                        LogManager.GetCurrentClassLogger()
                            .Warn($"[RECEPTION_INTÉRIMAIRE][FLUX_ML81N] Export réception intérimaire vers SAP : Il n'y a pas de configuration correspondant à cette société({societeIdForSend}).");
                    }
                }
                else
                {
                    LogManager.GetCurrentClassLogger()
                      .Warn("[RECEPTION_INTÉRIMAIRE][FLUX_ML81N] Export réception intérimaire vers SAP : Il n'y a aucune réception éligible à envoyer vers SAP.");
                }
            }
            catch (Exception e)
            {
                string msg = string.Format(IEBusiness.FluxErreurExport, ExportJobId);
                var exception = new FredBusinessException(msg, e);
                LogManager.GetCurrentClassLogger().Error(exception, exception.Message);
                throw exception;
            }
        }

        private void ControlerExistanceFlux()
        {
            if (Flux == null)
            {
                string msg = string.Format(IEBusiness.FluxInconnu, ExportJobId);
                var exception = new FredBusinessException(msg);
                LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }

            if (!Flux.IsActif)
            {
                string msg = string.Format(IEBusiness.FluxInactif, ExportJobId);
                var exception = new FredBusinessException(msg);
                LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }
        }
    }
}
