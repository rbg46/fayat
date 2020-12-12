using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Organisation.Tree;
using Fred.Business.RepriseDonnees.Ci;
using Fred.Business.RepriseDonnees.Ci.ContextProviders;
using Fred.Business.RepriseDonnees.Ci.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.Ci.Validators;
using Fred.Entities.Organisation.Tree;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.CI.AnaelSystem.Context;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Input;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Validator;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Societe.Input;
using Fred.ImportExport.Business.CI.AnaelSystem.Converter;
using Fred.ImportExport.Business.CI.AnaelSystem.Etablissement;
using Fred.ImportExport.Business.CI.AnaelSystem.Fred;
using Fred.ImportExport.Business.CI.AnaelSystem.Sap;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.AnaelSystem
{
    /// <summary>
    /// Gere l'import et la mis a jours des ci dans un system couplé a l'A400 et a SAP
    /// </summary>
    public class ImportCiAnaelSystemManager : IImportCiAnaelSystemManager
    {
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IImportCiByExcelAnaelSystemContextProvider importCiByExcelAnaelSystemContextProvider;
        private readonly IImportCiByCiListAnaelSystemContextProvider importCiByCiListAnaelSystemContextProvider;
        private readonly IImportCiBySocieteAnaelSystemContextProvider importCiBySocieteAnaelSystemContextProvider;
        private readonly ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider;
        private readonly ICiSapSender ciSapSender;
        private readonly IFredCiImporter fredCiImporter;
        private readonly ICiContextProvider ciContextProvider;
        private readonly ICiExtractorService ciExtractorService;
        private readonly IRepriseCiValidatorService repriseCiValidatorService;
        private readonly IRepriseDonneesCiManager repriseDonneeCiManager;

        public ImportCiAnaelSystemManager(
            IOrganisationTreeService organisationTreeService,
            IImportCiByExcelAnaelSystemContextProvider importCiByExcelAnaelSystemContextProvider,
            IImportCiByCiListAnaelSystemContextProvider importCiByCiListAnaelSystemContextProvider,
            IImportCiBySocieteAnaelSystemContextProvider importCiBySocieteAnaelSystemContextProvider,
            ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider,
            ICiSapSender ciSapSender,
            IFredCiImporter fredCiImporter,
            ICiContextProvider ciContextProvider,
            ICiExtractorService ciExtractorService,
            IRepriseCiValidatorService repriseCiValidatorService,
            IRepriseDonneesCiManager repriseDonneeCiManager)
        {
            this.organisationTreeService = organisationTreeService;
            this.importCiByExcelAnaelSystemContextProvider = importCiByExcelAnaelSystemContextProvider;
            this.importCiByCiListAnaelSystemContextProvider = importCiByCiListAnaelSystemContextProvider;
            this.importCiBySocieteAnaelSystemContextProvider = importCiBySocieteAnaelSystemContextProvider;
            this.commonAnaelSystemContextProvider = commonAnaelSystemContextProvider;
            this.ciSapSender = ciSapSender;
            this.fredCiImporter = fredCiImporter;
            this.ciContextProvider = ciContextProvider;
            this.ciExtractorService = ciExtractorService;
            this.repriseCiValidatorService = repriseCiValidatorService;
            this.repriseDonneeCiManager = repriseDonneeCiManager;
        }

        /// <summary>
        /// Importation des cis par societe
        /// </summary>
        /// <param name="input">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        public async Task<ImportResult> ImportCiBySocieteAsync(ImportCisBySocieteInputs input)
        {

            var logger = new CiImportExportLogger("IMPORT-EXPORT", "IMPORT-CI-BY-SOCIETE");

            ImportCiContext<ImportCisBySocieteInputs> context = importCiBySocieteAnaelSystemContextProvider.GetContext(input, logger);

            context.Logger = logger;

            var validator = new ImportCiBySocieteValidator();

            //verification si la societe Existe => non nulle
            var rulesResult = validator.VerifyRules(context);

            if (!rulesResult.IsValid)
            {
                return rulesResult;
            }

            var societesContexts = context.SocietesContexts.Where(x => x.Societe != null).ToList();

            societesContexts = GetSocietesWithEtablissementComptableManagables(societesContexts, logger);

            MapAndImportInFred(context.OrganisationTree, logger, societesContexts);

            await SelectSocieteAndSendToSapAsync(logger, context, societesContexts);

            return rulesResult;

        }

        /// <summary>
        /// Importation des cis par liste de cis
        /// </summary>
        /// <param name="importCisByCiListInputs">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        public async Task<ImportResult> ImportCiByCiIdsAsync(ImportCisByCiListInputs importCisByCiListInputs)
        {
            var logger = new CiImportExportLogger("IMPORT-EXPORT", "IMPORT-CI-BY-CIS-LIST");

            ImportCiContext<ImportCisByCiListInputs> context = importCiByCiListAnaelSystemContextProvider.GetContext(importCisByCiListInputs, logger);

            context.Logger = logger;

            var validator = new ImportCiValidator();

            var rulesResult = validator.Verify(context);

            if (!rulesResult.IsValid)
            {
                logger.WarnValidationErrors(rulesResult);

                throw new FredBusinessException(rulesResult.ErrorMessages.Aggregate((c, a) => c + " " + a));
            }

            var societesContexts = context.SocietesContexts.Where(x => x.Societe != null).ToList();

            societesContexts = GetSocietesWithEtablissementComptableManagables(societesContexts, logger);

            MapAndImportInFred(context.OrganisationTree, logger, societesContexts);

            await SelectSocieteAndSendToSapAsync(logger, context, societesContexts);

            return rulesResult;
        }

        /// <summary>
        /// Importation des cis par fichier excel
        /// </summary>
        /// <param name="importCisByExcelInput">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        public async Task<ImportResult> ImportCiByExcelAsync(ImportCisByExcelInputs importCisByExcelInput)
        {
            var logger = new CiImportExportLogger("IMPORT-EXPORT", "IMPORT-CI-BY-EXCEL");

            // Recuperation des données de la feuille excel.
            var parsageResult = ciExtractorService.ParseExcelFile(importCisByExcelInput.ExcelStream);

            // Récuperations de toutes les données qui sont necessaires pour faire la validation
            var contextForValidation = ciContextProvider.GetContextForImportCis(importCisByExcelInput.GroupeId, parsageResult.Cis);

            // verification des règles(RG) de l'import.
            var importRulesResult = repriseCiValidatorService.VerifyImportRules(parsageResult.Cis, contextForValidation);

            var isValid = importRulesResult.AllLignesAreValid();

            if (isValid)
            {
                importCisByExcelInput.RepriseImportCis = parsageResult.Cis;

                ImportCiContext<ImportCisByExcelInputs> context = importCiByExcelAnaelSystemContextProvider.GetContext(importCisByExcelInput, logger);

                var societesContexts = context.SocietesContexts.Where(x => x.Societe != null).ToList();

                societesContexts = GetSocietesWithEtablissementComptableManagables(societesContexts, logger);

                MapAndImportInFred(context.OrganisationTree, logger, societesContexts);

                repriseDonneeCiManager.UpdateCis(parsageResult, contextForValidation);

                await SelectSocieteAndSendToSapAsync(logger, context, societesContexts);
            }

            return new ImportResult { IsValid = isValid, ErrorMessages = importRulesResult.ImportRuleResults.Where(i => !string.IsNullOrEmpty(i.ErrorMessage)).Select(i => i.ErrorMessage).ToList() };

        }

        private List<ImportCiSocieteContext> GetSocietesWithEtablissementComptableManagables(List<ImportCiSocieteContext> societesContexts, CiImportExportLogger logger)
        {
            var result = new List<ImportCiSocieteContext>();

            var defaultEtablissementProvider = new DefaultEtablissementProvider();

            foreach (var societeContext in societesContexts)
            {
                if (defaultEtablissementProvider.CanOverrideEtablissementComptableIfNecessary(societeContext.Societe, societeContext.EtablissementComptables))
                {
                    if (societeContext.Societe.EtablissementParDefaut)
                    {
                        logger.LogEtablissementComptableWillBeDefault(societeContext.Societe);
                    }

                    result.Add(societeContext);
                }
                else
                {
                    logger.WarnEtablissementComptableCantBeOverrided(societeContext.Societe);
                }
            }
            return result;
        }


        private void MapAndImportInFred(OrganisationTree organisationTree, CiImportExportLogger logger, List<ImportCiSocieteContext> societesContexts)
        {

            if (societesContexts.Count == 0)
            {
                logger.WarnNoSocieteWillBeImportedInFred();
                return;
            }
            var mapper = new AnaelModelToCiEntConverter(logger);

            var defaultEtablissementProvider = new DefaultEtablissementProvider();

            foreach (var societeContext in societesContexts)
            {
                List<CiAnaelModel> ciAnaelModels = societeContext.AnaelCis;

                var anaelCisConvertedToCiEnts = mapper.ConvertAnaelModelToCiEnts(societeContext.Societe.SocieteId, societeContext.EtablissementComptables, ciAnaelModels);

                defaultEtablissementProvider.MapDefaultEtablissementIfNecessary(societeContext.Societe, societeContext.EtablissementComptables, anaelCisConvertedToCiEnts);

                societeContext.FredCis = this.fredCiImporter.ImportCIsFromAnael(organisationTree, societeContext.Societe, anaelCisConvertedToCiEnts);

                // log des cis fred
                logger.LogFredEnts(societeContext.FredCis);
            }

        }

        private async Task SelectSocieteAndSendToSapAsync<T>(CiImportExportLogger logger, ImportCiContext<T> context, List<ImportCiSocieteContext> societesContexts) where T : class
        {
            if (societesContexts.Count == 0)
            {
                logger.WarnNoSocieteWillBeExportedInSap();
                return;
            }
            var sapFilter = new SapFilter();

            var societesMustSendCisToSap = societesContexts.Where(x => sapFilter.CanSendToSap(x.Societe, x.TypeSocietes)).ToList();

            if (societesMustSendCisToSap.Any())
            {
                LoadSapMandatoryData(context, societesMustSendCisToSap);

                await ciSapSender.MapAndSendToSapAsync(logger, context, societesMustSendCisToSap);
            }
        }



        private void LoadSapMandatoryData<T>(ImportCiContext<T> context, List<ImportCiSocieteContext> societesMustSendCisToSap) where T : class
        {
            //Rechargement de l'arbre car il a pu changer puisque l'on a pu inserer de nouveaux cis
            context.OrganisationTree = organisationTreeService.GetOrganisationTree();

            context.Responsables = commonAnaelSystemContextProvider.GetResponsables(societesMustSendCisToSap);

            context.SocietesOfResponsables = commonAnaelSystemContextProvider.GetSocietesOfResponsables(context.Responsables);

            context.CiPays = commonAnaelSystemContextProvider.GetPaysOfCis(societesMustSendCisToSap);

        }

    }


}
