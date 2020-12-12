using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Fournisseur.Etl.Process;
using Fred.ImportExport.Framework.Log;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur
{
    /// <summary>
    /// Classe de gestion des flux import fournisseurs
    /// </summary>
    public class FournisseurFluxService : IFournisseurFluxService
    {
        private readonly IImportExportLoggingService loggingService;
        private readonly ISocieteManager societeManager;
        private readonly IFournisseurManager fournisseurManager;
        private readonly IPaysManager paysManager;
        private readonly IFluxManager fluxManager;

        public FournisseurFluxService(
            IImportExportLoggingService loggingService,
            ISocieteManager societeManager,
            IFournisseurManager fournisseurManager,
            IPaysManager paysManager,
            IFluxManager fluxManager)
        {
            this.loggingService = loggingService;
            this.societeManager = societeManager;
            this.fournisseurManager = fournisseurManager;
            this.paysManager = paysManager;
            this.fluxManager = fluxManager;
        }

        /// <inheritdoc />
        public async Task ImportationProcessAsync(bool bypassDate, string codeSocieteComptable, string typeSequences, string regleGestions, bool isStormOutputDesactivated = false)
        {
            var societe = societeManager.GetSocieteByCodeSocieteComptable(codeSocieteComptable);
            if (societe != null)
            {
                var etl = new FournisseurAnaelProcess(fluxManager, paysManager, fournisseurManager, societeManager, isStormOutputDesactivated);
                etl.SetParameter(bypassDate, codeSocieteComptable, typeSequences, regleGestions, societe.SocieteId);

                etl.Build();

                await etl.ExecuteAsync();
            }
            else
            {
                var msg = $"[IMPORT] Fournisseurs/Tiers (ANAEL => FRED) - Il n' y a pas de société correspondant à ce codeSocieteComptable ({codeSocieteComptable}).";
                var exception = new FredBusinessException(msg);
                loggingService.LogError(msg, exception);
                throw exception;
            }
        }

        /// <inheritdoc />
        public async Task ImportFournisseursAsync(List<FournisseurModel> fournisseurs)
        {
            var etl = new ImportFournisseurProcess(fluxManager, paysManager, fournisseurManager, societeManager);
            etl.Build();
            etl.SetParameter(fournisseurs);
            await etl.ExecuteAsync();
        }
    }
}
