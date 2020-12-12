using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Rapport.Pointage.FredIe;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.Pointage.PointageEtl;
using Fred.ImportExport.Business.Pointage.PointageEtl.Process;
using Fred.ImportExport.Business.WorkflowLogicielTiers;
using Fred.Web.Shared.Models.Rapport;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.Pointage.Personnel.PointagePersonnelEtl
{
    /// <summary>
    /// Ce mananger export les pointage personnel vers SAP
    /// </summary>
    public class PointageFluxManager : IPointageFluxManager
    {
        private readonly IFredIePointageFluxService fredIePointageFluxService;
        private readonly ICIManager ciManager;
        private readonly IMatriculeExterneManager matriculeExterneManager;
        private readonly IPersonnelManager personnelManager;
        private readonly ISocieteManager societeManager;
        private readonly IApplicationsSapManager applicationsSapManager;
        private readonly IWorkflowPointageManager workflowPointageManager;
        private readonly ILogicielTiersManager logicielTiersManager;
        private readonly IGroupeRepository groupRepository;
        private readonly IMapper mapper;
        private readonly IPointageManager pointageManager;

        public PointageFluxManager(
            IFredIePointageFluxService fredIePointageFluxService,
            ICIManager ciManager,
            IMatriculeExterneManager matriculeExterneManager,
            IPersonnelManager personnelManager,
            ISocieteManager societeManager,
            IApplicationsSapManager applicationsSapManager,
            IWorkflowPointageManager workflowPointageManager,
            ILogicielTiersManager logicielTiersManager,
            IGroupeRepository groupRepository,
            IMapper mapper,
            IPointageManager pointageManager)
        {
            this.fredIePointageFluxService = fredIePointageFluxService;
            this.ciManager = ciManager;
            this.matriculeExterneManager = matriculeExterneManager;
            this.personnelManager = personnelManager;
            this.societeManager = societeManager;
            this.applicationsSapManager = applicationsSapManager;
            this.workflowPointageManager = workflowPointageManager;
            this.logicielTiersManager = logicielTiersManager;
            this.groupRepository = groupRepository;
            this.mapper = mapper;
            this.pointageManager = pointageManager;
        }

        /// <summary>
        /// Methode qui export les pointage personnel vers SAP
        /// Il créer un etl d'envoie des pointages personnel que si le fichier de mapping contient
        /// une entree avec un codeSocieStorm correspondant au codeSocieStorm de la societe du rapport
        /// </summary>
        /// <param name="rapportId">l'id FRED  du rapport</param>   
        /// <param name="context">Contexte Hangfire</param>
        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[PERSONNEL][POINTAGE][FLUX_CAT2] Export pointage personnel vers SAP du rapportId : {0}")]
        public async Task ExportPointagePersonnelToSap(int rapportId, PerformContext context)
        {
            var parameter = new ExportByRapportParameter { RapportId = rapportId, BackgroundJobId = context.BackgroundJob.Id };
            string groupCode = await groupRepository.GetGroupCodeByReportIdAsync(rapportId);

            await JobRunnerApiRestHelper.PostAsync("ExportPointagePersonnelToSap", groupCode, parameter);
        }

        public async Task ExportPointagePersonnelToSapJobAsync(ExportByRapportParameter parameter)
        {
            int rapportId = parameter.RapportId;
            string backgroundJobId = parameter.BackgroundJobId;

            var factory = new PointageEtlFactory(GreateDependenciesWrapper());
            IPointageProcess etl = factory.GetEtl(rapportId, backgroundJobId);
            if (etl != null)
            {
                etl.Build();
                await etl.ExecuteAsync();
            }
        }

        /// <summary>
        /// Methode qui export les pointage personnel vers SAP d'une liste de rapports
        /// Il créer un etl d'envoie des pointages personnel que si le fichier de mapping contient
        /// une entree avec un codeSocieStorm correspondant au codeSocieStorm de la societe du rapport
        /// </summary>
        /// <param name="rapportIds">liste d'id FRED de rapports</param>
        /// <param name="context">Contexte Hangfire</param>
        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[PERSONNEL][POINTAGE][FLUX_CAT2] Export pointage personnel vers SAP (Liste de rapports)")]
        public async Task ExportPointagePersonnelToSap(List<int> rapportIds, PerformContext context)
        {
            var parameter = new ExportByRapportsParameter { RapportIds = rapportIds, BackgroundJobId = context.BackgroundJob.Id };
            string groupCode = await groupRepository.GetGroupCodeByReportIdsAsync(rapportIds);

            await JobRunnerApiRestHelper.PostAsync("ExportPointagePersonnelListToSap", groupCode, parameter);
        }

        public async Task ExportPointagePersonnelToSapJobAsync(ExportByRapportsParameter parameter)
        {
            List<int> rapportIds = parameter.RapportIds;
            string backgroundJobId = parameter.BackgroundJobId;

            var factory = new PointageEtlFactory(GreateDependenciesWrapper());

            foreach (IPointageProcess etl in factory.GetEtl(rapportIds, backgroundJobId))
            {
                try
                {

                    if (etl != null)
                    {
                        etl.Build();
                        await etl.ExecuteAsync();
                    }
                }
                catch
                {
                    //Si pour une raison ou pour une autre, le traitement de cet ETL echoue, on recevra une exception qu'il faut catcher 
                    //pour continuer l'exécution et traiter les ETL suivants. Nous n'avons pas besoin de traiter ici la ou les exceptions car elles sont
                    //déjà gérées dans la classe EtlProcessBase (c'est d'ailleurs elle qui nous les rethrows)
                }

            }
        }

        public PointageEtlDependenciesWrapper GreateDependenciesWrapper()
        {
            return new PointageEtlDependenciesWrapper()
            {
                FredIePointageFluxService = this.fredIePointageFluxService,
                SocieteManager = this.societeManager,
                ApplicationsSapManager = this.applicationsSapManager,
                WorkflowPointageManager = this.workflowPointageManager,
                LogicielTiersManager = this.logicielTiersManager,
                MatriculeExterneManager = this.matriculeExterneManager,
                PersonnelManager = this.personnelManager,
                CIManager = ciManager
            };
        }

        /// <summary>
        /// Export des pointages personnel vers Tibco
        /// </summary>
        /// <param name="user">identifiant de l'utilisateur</param>
        /// <param name="simulation">flag, si oui ne pas tenir compte du vérrouillage</param>
        /// <param name="periode">date période</param>
        /// <param name="type_periode">semaine ou mois</param>
        /// <param name="societe">code société</param>
        /// <param name="etabs">liste des codes établissements comptable</param>
        /// <returns>Model des lignes des rapports au format tibco</returns>
        public ExportPointagePersonnelTibcoModel ExportPointagePersonnelToTibco(int user, bool simulation, DateTime periode, string type_periode, string societe, string etabs)
        {
            List<string> etablissements = etabs.Split(',').ToList();
            var filter = new ExportPointagePersonnelFilterModel
            {
                UserId = user,
                Simulation = simulation,
                DateDebut = periode,
                Hebdo = (type_periode == "semaine"),
                SocieteCode = societe,
                EtablissementsComptablesCodes = etablissements
            };
            return pointageManager.GetPointagePersonnelForTibco(mapper.Map<ExportPointagePersonnelFilterModel>(filter));
        }
    }
}
