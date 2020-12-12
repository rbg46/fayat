using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using Fred.Business.Journal;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.JournauxComptable.Etl.Process;
using Hangfire;

namespace Fred.ImportExport.Business.JournauxComptable
{
    /// <summary>
    /// Gestion de l'import des Journaux Comptables (ANAEL => FRED)
    /// </summary>
    public class JournauxComptableFluxManager : AbstractFluxManager
    {
        private readonly ISocieteManager societeManager;
        private readonly IJournalManager journalComptableManager;
        private readonly IUtilisateurManager utilisateurManager;

        public virtual string FluxCode { get; protected set; }

        public JournauxComptableFluxManager(
            IFluxManager fluxManager,
            ISocieteManager societeManager,
            IJournalManager journalComptableManager,
            IUtilisateurManager utilisateurManager)
            : base(fluxManager)
        {
            this.societeManager = societeManager;
            this.journalComptableManager = journalComptableManager;
            this.utilisateurManager = utilisateurManager;
        }

        public void ExecuteImport()
        {
            BackgroundJob.Enqueue(() => ImportationProcess(FluxCode));
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT] Journaux Comptables (ANAEL => FRED)")]
        public async Task ImportationProcess(string fluxCode)
        {
            await JobRunnerApiRestHelper.PostAsync("ImportationProcessJournauxComptable", Constantes.CodeGroupeRZB, fluxCode);
        }

        public async Task ImportationProcessJobAsync(string fluxCode)
        {
            string anaelConnectionString = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];
            Flux = FluxManager.GetByCode(fluxCode);

            var etl = new JournauxComptableEtlProcess(FluxManager, societeManager, journalComptableManager, utilisateurManager, Flux)
            {
                ChaineConnexionAnael = anaelConnectionString,
                ImportJobId = fluxCode
            };

            // Lancement de l'etl
            etl.Build();
            await etl.ExecuteAsync();
        }
    }
}
