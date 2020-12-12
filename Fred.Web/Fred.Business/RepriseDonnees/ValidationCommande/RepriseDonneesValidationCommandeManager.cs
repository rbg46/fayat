using System;
using System.IO;
using System.Linq;
using Fred.Business.RepriseDonnees.ValidationCommande.ContextProviders;
using Fred.Business.RepriseDonnees.ValidationCommande.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.ValidationCommande.Mapper;
using Fred.Business.RepriseDonnees.ValidationCommande.Validators;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.ValidationCommande
{
    public class RepriseDonneesValidationCommandeManager : IRepriseDonneesValidationCommandeManager
    {
        private readonly IValidationCommandeDataMapper validationCommandeDataMapper;
        private readonly IValidationCommandeContextProvider alidationCommandeContextProvider;
        private readonly IRepriseValidationCommandeRepository repriseValidationCommandeRepository;
        private readonly IValidationCommandeExtractorService validationCommandeExtractorService;
        private readonly IValidationCommandeValidatorService validationCommandeValidatorService;

        public RepriseDonneesValidationCommandeManager(
            IValidationCommandeDataMapper validationCommandeDataMapper,
            IValidationCommandeContextProvider alidationCommandeContextProvider,
            IRepriseValidationCommandeRepository repriseValidationCommandeRepository,
            IValidationCommandeExtractorService validationCommandeExtractorService,
            IValidationCommandeValidatorService validationCommandeValidatorService)
        {
            this.validationCommandeDataMapper = validationCommandeDataMapper;
            this.alidationCommandeContextProvider = alidationCommandeContextProvider;
            this.repriseValidationCommandeRepository = repriseValidationCommandeRepository;
            this.validationCommandeExtractorService = validationCommandeExtractorService;
            this.validationCommandeValidatorService = validationCommandeValidatorService;
        }

        /// <summary>
        /// Valide une liste de commandes
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <param name="backgroundJobFunc">Fonction qui execute le jog d'envoie de la commande a SAP </param>
        /// <returns>le resultat de l'import</returns>
        public ImportValidationCommandeResult ValidateCommandes(int groupeId, Stream stream, Func<int, string> backgroundJobFunc)
        {
            ImportValidationCommandeResult result = new ImportValidationCommandeResult();

            // Recuperation des données de la feuille excel.
            ParseValidationCommandesResult parsageResult = validationCommandeExtractorService.ParseExcelFile(stream);

            // Récuperations de toutes les données qui sont necessaires pour faire la validation des commandes.
            ContextForValidationCommande context = alidationCommandeContextProvider.GetContextForValidationCommandes(groupeId, parsageResult.Commandes);

            // verification des règles(RG) de l'import.
            Validators.Results.ValidationCommandeRulesResult importRulesResult = validationCommandeValidatorService.VerifyImportRules(parsageResult.Commandes, context);

            if (importRulesResult.AllLignesAreValid())
            {
                result.IsValid = true;

                var commandes = context.CommandesUsedInExcel;

                foreach (CommandeEnt commande in commandes)
                {
                    validationCommandeDataMapper.SetCommandeAsValidate(commande, context.StatutCommandeValidee.StatutCommandeId);
                }

                repriseValidationCommandeRepository.SetCommandesAsValidatedAndSaveCommandes(commandes);

                foreach (CommandeEnt commande in commandes)
                {
                    string jobId = backgroundJobFunc(commande.CommandeId);

                    validationCommandeDataMapper.SetHangfireJobId(commande, jobId);
                }

                repriseValidationCommandeRepository.SetHangfireIdsAndSaveCommandes(commandes);

            }
            else
            {
                result.ErrorMessages = importRulesResult.ImportRuleResults.Where(x => !x.IsValid).Select(x => x.ErrorMessage).ToList();
            }


            return result;
        }

    }
}
