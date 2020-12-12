using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Business.RepriseDonnees.Materiel.ContextProviders;
using Fred.Business.RepriseDonnees.Materiel.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.Materiel.Mapper;
using Fred.Business.RepriseDonnees.Materiel.Models;
using Fred.Business.RepriseDonnees.Materiel.Validators;
using Fred.Business.RepriseDonnees.Materiel.Validators.Results;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.Materiel
{
    public class RepriseDonneesMaterielManager : IRepriseDonneesMaterielManager
    {
        private readonly IUnitOfWork uow;
        private readonly IMaterielExtractorService materielExtractorService;
        private readonly IMaterielContextProvider materielContextProvider;
        private readonly IMaterielValidatorService materielValidatorService;
        private readonly IMaterielDataMapper materielDataMapper;
        private readonly IRepriseMaterielRepository repriseMaterielRepository;

        public RepriseDonneesMaterielManager(
            IUnitOfWork uow,
            IMaterielExtractorService materielExtractorService,
            IMaterielContextProvider materielContextProvider,
            IMaterielValidatorService materielValidatorService,
            IMaterielDataMapper materielDataMapper,
            IRepriseMaterielRepository repriseMaterielRepository)
        {
            this.uow = uow;
            this.materielExtractorService = materielExtractorService;
            this.materielContextProvider = materielContextProvider;
            this.materielValidatorService = materielValidatorService;
            this.materielDataMapper = materielDataMapper;
            this.repriseMaterielRepository = repriseMaterielRepository;
        }

        /// <summary>
        /// Importation des Materiels
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>Le resultat de l'import</returns>
        public ImportMaterielResult CreateMateriel(int groupeId, Stream stream)
        {
            ImportMaterielResult result = new ImportMaterielResult();

            // Recuperation des données de la feuille excel.
            ParseMaterielResult parsageResult = materielExtractorService.ParseExcelFile(stream);

            // Récuperations de toutes les données qui sont necessaires pour faire un import des Materiels
            ContextForImportMateriel context = materielContextProvider.GetContextForImportMateriel(groupeId, parsageResult.Materiels);

            // Verification des règles(RG) de l'import.
            MaterielImportRulesResult importRulesResult = materielValidatorService.VerifyImportRules(parsageResult.Materiels, context);

            if (importRulesResult.AllLignesAreValid())
            {
                result.IsValid = true;

                // Ici je crée les Materiels
                List<MaterielEnt> materielEnts = materielDataMapper.Transform(context, parsageResult.Materiels);

                if (materielEnts.Count > 0)
                {
                    repriseMaterielRepository.CreateMateriel(materielEnts);

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
