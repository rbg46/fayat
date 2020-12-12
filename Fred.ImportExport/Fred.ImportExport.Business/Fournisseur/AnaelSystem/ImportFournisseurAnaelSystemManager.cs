using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.Referential;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Ci.Validator;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Fournisseur.Input;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Converter;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Excel;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Excel.Validator;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Sap;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem
{
    /// <summary>
    /// Gere l'import et la mis a jours des ci dans un system couplé a l'A400 et a SAP
    /// </summary>
    public class ImportFournisseurAnaelSystemManager : IImportFournisseurAnaelSystemManager
    {
        private readonly IExcelFournisseurExtractorService excelFournisseurExtractorService;
        private readonly IFournisseurManager fournisseurManager;
        private readonly IImportFournisseurByExcelAnaelSystemContextProvider importFournisseurByExcelAnaelSystemContextProvider;
        private readonly IImportFournisseurByFournisseurListAnaelSystemContextProvider importFournisseurByFournisseurListAnaelSystemContextProvider;
        private readonly IFournisseurSapSender fournisseurSapSender;
        private readonly IMapper mapper;

        public ImportFournisseurAnaelSystemManager(
            IExcelFournisseurExtractorService excelFournisseurExtractorService,
            IFournisseurManager fournisseurManager,
            IImportFournisseurByExcelAnaelSystemContextProvider importFournisseurByExcelAnaelSystemContextProvider,
            IImportFournisseurByFournisseurListAnaelSystemContextProvider importFournisseurByFournisseurListAnaelSystemContextProvider,
            IFournisseurSapSender fournisseurSapSender,
            IMapper mapper)
        {
            this.excelFournisseurExtractorService = excelFournisseurExtractorService;
            this.fournisseurManager = fournisseurManager;
            this.importFournisseurByExcelAnaelSystemContextProvider = importFournisseurByExcelAnaelSystemContextProvider;
            this.importFournisseurByFournisseurListAnaelSystemContextProvider = importFournisseurByFournisseurListAnaelSystemContextProvider;
            this.fournisseurSapSender = fournisseurSapSender;
            this.mapper = mapper;
        }

        /// <summary>
        /// Importation des fournisseurs par liste de fournisseurs
        /// </summary>
        /// <param name="importFournisseurByIdsListInputs">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        public async Task<ImportResult> ImportFournisseurByFournisseurIdsAsync(ImportFournisseurByIdsListInputs importFournisseurByIdsListInputs)
        {
            var logger = new FournisseurImportExportLogger("IMPORT-EXPORT", "IMPORT-FOURNISSEUR-BY-CIS-LIST");

            ImportFournisseurContext<ImportFournisseurByIdsListInputs> context = importFournisseurByFournisseurListAnaelSystemContextProvider.GetContext(importFournisseurByIdsListInputs, logger);

            context.Logger = logger;

            var validator = new ImportFournisseurValidator();

            ImportResult rulesResult = validator.Verify(context);

            if (!rulesResult.IsValid)
            {
                logger.WarnValidationErrors(rulesResult);

                throw new FredBusinessException(rulesResult.ErrorMessages.Aggregate((c, a) => c + " " + a));
            }

            List<ImportFournisseurSocieteContext> societesContexts = context.SocietesContexts.Where(x => x.Societe != null).ToList();

            MapAndImportInFred(logger, societesContexts);

            await SelectSocieteAndSendToSapAsync(logger, context, societesContexts);

            return rulesResult;
        }

        /// <summary>
        /// Importation des fournisseurs par fichier excel
        /// </summary>
        /// <param name="importFournisseursByExcelInput">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        public async Task<ImportResult> ImportFournisseurByExcelAsync(ImportFournisseursByExcelInputs importFournisseursByExcelInput)
        {

            var logger = new FournisseurImportExportLogger("IMPORT-EXPORT", "IMPORT-FOURNISSEUR-BY-EXCEL");

            ParseImportFournisseursResult parseImportFournisseursResult = excelFournisseurExtractorService.ParseExcelFile(importFournisseursByExcelInput.ExcelStream);

            logger.ParsageDuFichierSucess();

            importFournisseursByExcelInput.RepriseImportFournisseurs = parseImportFournisseursResult.Fournisseurs;

            ImportFournisseurContext<ImportFournisseursByExcelInputs> context = importFournisseurByExcelAnaelSystemContextProvider.GetContext(importFournisseursByExcelInput, logger);

            var validator = new ImportFournisseurExcelValidator();

            ImportResult rulesResult = validator.Verify(context);

            if (!rulesResult.IsValid)
            {
                logger.WarnValidationErrors(rulesResult);

                return rulesResult;
            }

            List<ImportFournisseurSocieteContext> societesContexts = context.SocietesContexts.Where(x => x.Societe != null).ToList();

            MapAndImportInFred(logger, societesContexts);

            await SelectSocieteAndSendToSapAsync(logger, context, societesContexts);

            return rulesResult;

        }


        private void MapAndImportInFred(FournisseurImportExportLogger logger, List<ImportFournisseurSocieteContext> societesContexts)
        {

            if (societesContexts.Count == 0)
            {
                logger.WarnNoSocieteWillBeImportedInFred();
                return;
            }
            var converter = new FournisseurFredModelToFournisseurEntConverter(mapper);

            foreach (ImportFournisseurSocieteContext societeContext in societesContexts)
            {
                int groupeId = societeContext.Societe.GroupeId;
                List<FournisseurEnt> fournisseurModelToFournisseurEnts = converter.ConvertFournisseurFredModelToFournisseurEnts(groupeId, societeContext.AnaelFournisseurs);

                // Import vers FRED
                societeContext.FredFournisseurs = fournisseurManager.ManageImportedFournisseurs(
                    fournisseurModelToFournisseurEnts,
                    societeContext.Societe.CodeSocieteComptable,
                    groupeId).ToList();

                // log des cis fred
                logger.LogFredEnts(societeContext.FredFournisseurs);
            }

        }

        private async Task SelectSocieteAndSendToSapAsync<T>(FournisseurImportExportLogger logger, ImportFournisseurContext<T> context, List<ImportFournisseurSocieteContext> societesContexts) where T : class
        {
            if (societesContexts.Count == 0)
            {
                logger.WarnNoSocieteWillBeExportedInSap();
                return;
            }
            var sapFilter = new SapFilter();

            List<ImportFournisseurSocieteContext> societesCanSendToSap = societesContexts.Where(x => sapFilter.CanSendToSap(x.Societe, context.TypeSocietes)).ToList();

            if (societesCanSendToSap.Any())
            {
                await fournisseurSapSender.MapAndSendToSapAsync(logger, context, societesCanSendToSap);
            }
        }
    }


}
