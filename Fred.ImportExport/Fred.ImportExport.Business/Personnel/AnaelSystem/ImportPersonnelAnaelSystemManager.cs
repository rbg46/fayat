using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Personnel.Input;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Input;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Validator;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Converter;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Logger;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Sap;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem
{
    public class ImportPersonnelAnaelSystemManager : IImportPersonnelAnaelSystemManager
    {
        private readonly IEmailGeneratorService emailGeneratorService;
        private readonly IPersonnelManager personnelManager;
        private readonly IPersonnelSapSender personnelSapSender;
        private readonly IImportPersonnelBySocieteContextProvider importPersonnelBySocieteContextProvider;
        private readonly IImportByPersonnelListContextProvider importByPersonnelListContextProvider;
        private readonly ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider;
        private readonly IImportByPersonnelListValidator importByPersonnelListValidator;
        private readonly IImportPersonnelsBySocieteValidator importPersonnelBySocieteValidator;
        private readonly IMatriculeExterneManager matriculeExterneManager;
        private readonly IPaysManager paysManager;

        public ImportPersonnelAnaelSystemManager(IEmailGeneratorService emailGeneratorService,
            IPersonnelManager personnelManager,
            IPersonnelSapSender personnelSapSender,
            IImportPersonnelBySocieteContextProvider importPersonnelBySocieteContextProvider,
            IImportByPersonnelListContextProvider importByPersonnelListContextProvider,
            ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider,
            IImportByPersonnelListValidator importByPersonnelListValidator,
            IImportPersonnelsBySocieteValidator importPersonnelBySocieteValidator,
            IMatriculeExterneManager matriculeExterneManager,
            IPaysManager paysManager)
        {
            this.emailGeneratorService = emailGeneratorService;
            this.personnelManager = personnelManager;
            this.personnelSapSender = personnelSapSender;
            this.importPersonnelBySocieteContextProvider = importPersonnelBySocieteContextProvider;
            this.importByPersonnelListContextProvider = importByPersonnelListContextProvider;
            this.commonAnaelSystemContextProvider = commonAnaelSystemContextProvider;
            this.importByPersonnelListValidator = importByPersonnelListValidator;
            this.importPersonnelBySocieteValidator = importPersonnelBySocieteValidator;
            this.matriculeExterneManager = matriculeExterneManager;
            this.paysManager = paysManager;
        }


        /// <summary>
        /// Importation des personnels par societe
        /// </summary>
        /// <param name="input">Parametre d'entrée</param>
        /// <param name="allPays">List des pays</param>
        /// <returns>Le resultat de l'import</returns>
        public async Task<ImportResult> ImportPersonnelsBySocieteAsync(ImportPersonnelsBySocieteInput input, IEnumerable<PaysEnt> allPays)
        {
            var logger = new PersonnelImportExportLogger("IMPORT-EXPORT", "IMPORT-PERSONNEL-BY-SOCIETE");

            ImportPersonnelContext<ImportPersonnelsBySocieteInput> context = importPersonnelBySocieteContextProvider.GetContext(input, logger);
            context.PersonnelPays = allPays?.ToList();
            context.Logger = logger;

            ImportResult rulesResult = importPersonnelBySocieteValidator.Verify(context);

            if (!rulesResult.IsValid)
            {
                logger.WarnValidationErrors(rulesResult);
                return rulesResult;
            }

            List<ImportPersonnelSocieteContext> societesContexts = context.SocietesContexts.Where(x => x.Societe != null).ToList();

            MapAndImportInFred(logger, context, societesContexts);

            await SelectSocieteAndSendToSapAsync(logger, context, societesContexts);

            return rulesResult;

        }

        /// <summary>
        /// Importation des personnels par personnelId
        /// </summary>
        /// <param name="input">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        public async Task<ImportResult> ImportPersonnelByPersonnelIdsAsync(ImportByPersonnelListInputs input)
        {

            var logger = new PersonnelImportExportLogger("IMPORT-EXPORT", "IMPORT-PERSONNEL-BY-PERSONNEL-LIST");

            ImportPersonnelContext<ImportByPersonnelListInputs> context = importByPersonnelListContextProvider.GetContext(input, logger);

            context.Logger = logger;

            ImportResult rulesResult = importByPersonnelListValidator.Verify(context);

            if (!rulesResult.IsValid)
            {
                logger.WarnValidationErrors(rulesResult);
                return rulesResult;
            }

            List<ImportPersonnelSocieteContext> societesContexts = context.SocietesContexts.Where(x => x.Societe != null).ToList();

            await SelectSocieteAndSendToSapAsync(logger, context, societesContexts);

            return rulesResult;

        }

        private void MapAndImportInFred(PersonnelImportExportLogger logger, ImportPersonnelContext<ImportPersonnelsBySocieteInput> context, List<ImportPersonnelSocieteContext> societesContexts)
        {
            if (societesContexts.Count == 0)
            {
                logger.WarnNoSocieteWillBeImportedInFred();
                return;
            }
            var mapper = new AnaelModelToPersonnelEntConverter(emailGeneratorService);

            foreach (ImportPersonnelSocieteContext societeContext in societesContexts)
            {
                List<PersonnelEnt> anaelPersonnelsConvertedToPersonnelEnts = mapper.ConvertToEnts(context?.Ressoures, societeContext, context?.PersonnelPays);

                //ManageImportedPersonnels retourne les personnelEnt ajoutes en base dans FRED
                societeContext.FredPersonnels = personnelManager.ManageImportedPersonnels(anaelPersonnelsConvertedToPersonnelEnts, societeContext.Societe.SocieteId);
                matriculeExterneManager.AddOrUpdateMatriculeSapListFTP(societeContext.FredPersonnels, societeContext.Societe);
                logger.LogFredEnts(societeContext.FredPersonnels);
            }

        }


        private async Task SelectSocieteAndSendToSapAsync<T>(PersonnelImportExportLogger logger, ImportPersonnelContext<T> context, List<ImportPersonnelSocieteContext> societesContexts) where T : class
        {
            if (societesContexts.Count == 0)
            {
                logger.WarnNoSocieteWillBeExportedInSap();
                return;
            }
            var sapFilter = new SapFilter();

            List<ImportPersonnelSocieteContext> societesMustSendPersonnelsToSap = societesContexts.Where(x => sapFilter.CanSendToSap(x.SocieteGroupeParent?.Code, x.Societe, context?.TypeSocietes)).ToList();

            if (societesMustSendPersonnelsToSap.Any())
            {
                LoadNecessaryDataForSendPersonnelsToSap(context, societesMustSendPersonnelsToSap);

                await personnelSapSender.MapAndSendToSapAsync(logger, context, societesMustSendPersonnelsToSap);
            }
        }


        private void LoadNecessaryDataForSendPersonnelsToSap<T>(ImportPersonnelContext<T> context, List<ImportPersonnelSocieteContext> societesMustSendPersonnelsToSap) where T : class
        {
            context.PersonnelPays = commonAnaelSystemContextProvider.GetPaysOfPersonnels(societesMustSendPersonnelsToSap);
        }
    }
}
