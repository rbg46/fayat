using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.Groupe;
using Fred.Business.Referential;
using Fred.Business.Referential.Service;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Fournisseur;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Framework.Log;
using Fred.ImportExport.Models;
using Hangfire;

namespace Fred.ImportExport.Business
{
    public class FournisseurFluxManager : AbstractFluxManager
    {
        private readonly IMapper mapper;
        private readonly IImportExportLoggingService loggingService;
        private readonly IFournisseurFluxService fournisseurFluxService;
        private readonly IFournisseursImportService fournisseursImportService;
        private readonly IPaysManager paysManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IGroupeManager groupeManager;
        private readonly IFluxRepository fluxRepository;

        public string ImportJobId => ConfigurationManager.AppSettings["flux:fournisseur"];

        public FournisseurFluxManager(
            IFluxManager fluxManager,
            IMapper mapper,
            IImportExportLoggingService loggingService,
            IFournisseurFluxService fournisseurFluxService,
            IFournisseursImportService fournisseursImportService,
            IPaysManager paysManager,
            IUtilisateurManager utilisateurManager,
            IGroupeManager groupeManager,
            IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            this.mapper = mapper;
            this.loggingService = loggingService;
            this.fournisseurFluxService = fournisseurFluxService;
            this.fournisseursImportService = fournisseursImportService;
            this.paysManager = paysManager;
            this.utilisateurManager = utilisateurManager;
            this.groupeManager = groupeManager;
            this.fluxRepository = fluxRepository;
        }

        /// <summary>
        ///   Exécution du l'import des Fournisseurs d'Anael vers FRED
        /// </summary>
        /// <param name="bypassDate">Booléen permettant d'ignorer la condition de date de modif</param>
        /// <param name="codeSocieteComptable">Code société comptable ANAEL</param>
        /// <param name="typeSequences">Type de séquences TIERS, TIERS2, GROUPE, MAT</param>
        /// <param name="regleGestions">Regle de gestion (F,C,C1,I)</param>
        /// <param name="isStormOutputDesactivated">Détermine si on active l'export vers STORM ou pas</param>
        public void ExecuteImport(bool bypassDate, string codeSocieteComptable, string typeSequences, string regleGestions, bool isStormOutputDesactivated = false)
        {
            try
            {
                BackgroundJob.Enqueue(() => ImportationProcess(bypassDate, codeSocieteComptable, typeSequences, regleGestions, isStormOutputDesactivated));
            }
            catch (FredBusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                var exception = new FredBusinessException(e.Message, e);
                NLog.LogManager.GetCurrentClassLogger().Error(exception);
                throw exception;
            }
        }

        /// <summary>
        ///   Planifie l'exécution du job selon un CRON
        /// </summary>
        /// <param name="cron">Command On Run</param>
        /// <param name="codeSocieteComptable">Code société comptable ANAEL</param>
        /// <param name="typeSequences">Type de séquences TIERS, TIERS2, GROUPE, MAT</param>
        /// <param name="regleGestions">Regle de gestion (F,C,C1,I)</param>
        /// <param name="isStormOutputDesactivated">Détermine si on active l'export vers STORM ou pas</param>
        public void ScheduleImport(string cron, string codeSocieteComptable, string typeSequences, string regleGestions, bool isStormOutputDesactivated = false)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(ImportJobId, () => ImportationProcess(false, codeSocieteComptable, typeSequences, regleGestions, isStormOutputDesactivated), cron);
            }
            else
            {
                string msg = string.Format(FredImportExportBusinessResources.CronExpressionPasParametre, ImportJobId);
                var exception = new FredBusinessException(msg);
                loggingService.LogError(msg, exception);
                throw exception;
            }
        }

        /// <summary>
        ///   Job d'import des fournisseurs d'ANAEL vers FRED
        /// </summary>
        /// <param name="bypassDate">Ignorer la date</param>
        /// <param name="codeSocieteComptable">Code société comptable ANAEL</param>
        /// <param name="typeSequences">Type de séquences TIERS, TIERS2, GROUPE, MAT</param>
        /// <param name="regleGestions">Regle de gestion (F,C,C1,I)</param>
        /// <param name="isStormOutputDesactivated">Détermine si on active l'export vers STORM ou pas</param>
        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT] Fournisseurs/Tiers (ANAEL => FRED)")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task ImportationProcess(bool bypassDate, string codeSocieteComptable, string typeSequences, string regleGestions, bool isStormOutputDesactivated = false)
        {
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(ImportJobId);
            var parameter = new ImportationFournisseurParameter { BypassDate = bypassDate, CodeSocieteComptable = codeSocieteComptable, TypeSequences = typeSequences, RegleGestions = regleGestions, IsStormOutputDesactivated = isStormOutputDesactivated };

            await JobRunnerApiRestHelper.PostAsync("ImportationProcessFournisseur", groupCode, parameter);
        }

        public async Task ImportationProcessJobAsync(ImportationFournisseurParameter parameter)
        {
            bool bypassDate = parameter.BypassDate;
            string codeSocieteComptable = parameter.CodeSocieteComptable;
            string typeSequences = parameter.TypeSequences;
            string regleGestions = parameter.RegleGestions;
            bool isStormOutputDesactivated = parameter.IsStormOutputDesactivated;

            await fournisseurFluxService.ImportationProcessAsync(bypassDate, codeSocieteComptable, typeSequences, regleGestions, isStormOutputDesactivated);
        }

        /// <summary>
        /// Permet d'importer une liste de fournisseur dans FRED.
        /// </summary>
        /// <param name="fournisseurs">Une liste de fournisseur.</param>
        public async Task ImportFournisseursAsync(List<FournisseurModel> fournisseurs)
        {
            await fournisseurFluxService.ImportFournisseursAsync(fournisseurs);
        }

        /// <summary>
        /// Import un fournisseur ou agence depuis un modele envoye de l'exterieur
        /// </summary>
        /// <param name="fournisseurModel">mon modele envoye</param>
        public void AddOrUpdateFournisseur(ImportFournisseurModel fournisseurModel)
        {
            int userId = utilisateurManager.GetByLogin("fred_ie").UtilisateurId;
            List<PaysEnt> pays = paysManager.GetList().ToList();

            fournisseurModel.PaysId = pays.FirstOrDefault(p => p.Code == fournisseurModel.CodePays)?.PaysId;
            fournisseurModel.Agences.ForEach(a => a.PaysId = pays.FirstOrDefault(p => p.Code == a.CodePays)?.PaysId);

            // AC = Fournisseur type Agence
            if (fournisseurModel.RolePartenaire == "AC")
            {
                var agenceEnt = mapper.Map<AgenceEnt>(fournisseurModel);
                fournisseursImportService.UpdateAgence(agenceEnt, userId);
            }
            else
            {
                // Pour l'US 3916, Fayat TP en parfaite synchro avec SAP nous a demande de ne plus gerer le groupe code et l'utilisateur de l'API et de le mettre en dur.
                // Si vous voulez reutiliser cette methode, il faudra se baser sur l'utilisateur dans les parametres de l'API.
                fournisseurModel.GroupeId = groupeManager.GetGroupeByCode(Constantes.CodeGroupeFTP).GroupeId;

                var fournisseurFromSap = mapper.Map<FournisseurEnt>(fournisseurModel);
                fournisseursImportService.AddOrUpdateFournisseurs(fournisseurFromSap, userId);
            }
        }
    }
}
