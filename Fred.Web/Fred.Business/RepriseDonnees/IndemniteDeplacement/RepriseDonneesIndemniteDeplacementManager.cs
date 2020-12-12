using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.ContextProviders;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Mapper;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Models;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Validators;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Validators.Results;
using Fred.DataAccess.Interfaces;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement
{
    public class RepriseDonneesIndemniteDeplacementManager : IRepriseDonneesIndemniteDeplacementManager
    {
        private readonly IUnitOfWork uow;
        private readonly IIndemniteDeplacementExtractorService indemniteDeplacementExtractorService;
        private readonly IIndemniteDeplacementContextProvider indemniteDeplacementContextProvider;
        private readonly IIndemniteDeplacementValidatorService indemniteDeplacementValidatorService;
        private readonly IIndemniteDeplacementDataMapper indemniteDeplacementDataMapper;
        private readonly IRepriseIndemniteDeplacementRepository repriseIndemniteDeplacementRepository;

        public RepriseDonneesIndemniteDeplacementManager(
            IUnitOfWork uow,
            IIndemniteDeplacementExtractorService indemniteDeplacementExtractorService,
            IIndemniteDeplacementContextProvider indemniteDeplacementContextProvider,
            IIndemniteDeplacementValidatorService indemniteDeplacementValidatorService,
            IIndemniteDeplacementDataMapper indemniteDeplacementDataMapper,
            IRepriseIndemniteDeplacementRepository repriseIndemniteDeplacementRepository)
        {
            this.uow = uow;
            this.indemniteDeplacementExtractorService = indemniteDeplacementExtractorService;
            this.indemniteDeplacementContextProvider = indemniteDeplacementContextProvider;
            this.indemniteDeplacementValidatorService = indemniteDeplacementValidatorService;
            this.indemniteDeplacementDataMapper = indemniteDeplacementDataMapper;
            this.repriseIndemniteDeplacementRepository = repriseIndemniteDeplacementRepository;
        }

        /// <summary>
        /// Importation des Indemnité de Déplacement (avec suppression logique si déjà existante)
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>Le résultat de l'import</returns>
        public ImportIndemniteDeplacementResult CreateIndemniteDeplacement(int groupeId, Stream stream)
        {
            ImportIndemniteDeplacementResult result = new ImportIndemniteDeplacementResult();

            // Recuperation des données de la feuille excel.
            ParseIndemniteDeplacementResult parsageResult = indemniteDeplacementExtractorService.ParseExcelFile(stream);

            // Récuperations de toutes les données qui sont necessaires pour faire un import des Indemnites de Déplacement
            ContextForImportIndemniteDeplacement context = indemniteDeplacementContextProvider.GetContextForImportIndemniteDeplacement(groupeId, parsageResult.IndemniteDeplacements);

            // Verification des règles(RG) de l'import.
            IndemniteDeplacementImportRulesResult importRulesResult = indemniteDeplacementValidatorService.VerifyImportRules(parsageResult.IndemniteDeplacements, context);

            if (importRulesResult.AllLignesAreValid())
            {
                result.IsValid = true;

                // Ici je crée les IndemniteDeplacement (et je supprime de façon logique celles qui existent déjà)
                List<IndemniteDeplacementEnt> indemniteDeplacementEnts = indemniteDeplacementDataMapper.Transform(context, parsageResult.IndemniteDeplacements);

                if (indemniteDeplacementEnts.Count > 0)
                {
                    repriseIndemniteDeplacementRepository.CreateIndemniteDeplacement(indemniteDeplacementEnts);

                    // ATTENTION ici, la resolution du dbcontext et de l'unit of work n'est pas effectuée de la meme maniere
                    // sur Fred.Web et Fred.Ie. sur fred ie, ces classes sont autant de fois instanciée que demandé.
                    // La mise a jour se fait sur un context, il faut donc que je fasse la save sur le meme context.
                    uow.Save();
                }
            }
            else
            {
                result.ErrorMessages = importRulesResult.ImportRuleResults.Where(x => !x.IsValid).Select(x => x.ErrorMessage).ToList();
            }

            return result;
        }
    }
}
