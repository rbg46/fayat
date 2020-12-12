using System.IO;
using System.Linq;
using Fred.Business.RepriseDonnees.Commande.ContextProviders;
using Fred.Business.RepriseDonnees.Commande.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.Commande.Mapper;
using Fred.Business.RepriseDonnees.Commande.Validators;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.Commande
{
    public class RepriseDonneesCommandeManager : IRepriseDonneesCommandeManager
    {
        private readonly ICommandeDataMapper commandeDataMapper;
        private readonly ICommandeContextProvider commandeContextProvider;
        private readonly IRepriseCommandeRepository repriseCommandeRepository;
        private readonly ICommandeExtractorService commandeExtractorService;
        private readonly ICommandeValidatorService commandeValidatorService;

        public RepriseDonneesCommandeManager(
            ICommandeDataMapper commandeDataMapper,
            ICommandeContextProvider commandeContextProvider,
            IRepriseCommandeRepository repriseCommandeRepository,
            ICommandeExtractorService commandeExtractorService,
            ICommandeValidatorService commandeValidatorService)
        {
            this.commandeDataMapper = commandeDataMapper;
            this.commandeContextProvider = commandeContextProvider;
            this.repriseCommandeRepository = repriseCommandeRepository;
            this.commandeExtractorService = commandeExtractorService;
            this.commandeValidatorService = commandeValidatorService;
        }

        /// <summary>
        /// Importation des Commandes 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportCommandeResult CreateCommandeAndReceptions(int groupeId, Stream stream)
        {
            var result = new ImportCommandeResult();

            // Recuperation des données de la feuille excel.
            var parsageResult = commandeExtractorService.ParseExcelFile(stream);

            // Récuperations de toutes les données qui sont necessaires pour faire un import Commande(creation).
            var context = commandeContextProvider.GetContextForImportCommandes(groupeId, parsageResult.Commandes);

            // verification des règles(RG) de l'import.
            var importRulesResult = commandeValidatorService.VerifyImportRules(parsageResult.Commandes, context);

            if (importRulesResult.AllLignesAreValid())
            {
                result.IsValid = true;

                // Ici je créer les commandes, commandes lignes et receptions
                var commandeTransformResult = commandeDataMapper.Transform(context, parsageResult.Commandes);

                if (commandeTransformResult.Commandes.Any())
                {
                    repriseCommandeRepository.SaveEntities(commandeTransformResult.Commandes,
                                                           commandeTransformResult.CommandeLignes,
                                                           commandeTransformResult.Receptions,
                                                           (commandes) => commandeDataMapper.UpdateNumeroDeCommandes(commandes));
                }
            }
            else
            {
                result.ErrorMessages = importRulesResult.ImportRuleResults.Where(x => !x.IsValid).OrderBy(x => x.NumeroDeLigne).Select(x => x.ErrorMessage).ToList();
            }


            return result;
        }

    }
}
