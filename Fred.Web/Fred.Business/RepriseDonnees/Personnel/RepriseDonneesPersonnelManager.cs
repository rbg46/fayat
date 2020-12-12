using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Business.RepriseDonnees.Personnel.ContextProdivers;
using Fred.Business.RepriseDonnees.Personnel.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.Personnel.Mapper;
using Fred.Business.RepriseDonnees.Personnel.Models;
using Fred.Business.RepriseDonnees.Personnel.Validators;
using Fred.Business.RepriseDonnees.Personnel.Validators.Results;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.Personnel
{
    public class RepriseDonneesPersonnelManager : IRepriseDonneesPersonnelManager
    {
        private readonly IUnitOfWork uow;
        private readonly IPersonnelExtractorService personnelExtractorService;
        private readonly IPersonnelContextProvider personnelContextProvider;
        private readonly IPersonnelValidatorService personnelValidatorService;
        private readonly IPersonnelDataMapper personnelDataMapper;
        private readonly IReprisePersonnelRepository reprisePersonnelRepository;

        public RepriseDonneesPersonnelManager(
            IUnitOfWork uow,
            IPersonnelExtractorService personnelExtractorService,
            IPersonnelContextProvider personnelContextProvider,
            IPersonnelValidatorService personnelValidatorService,
            IPersonnelDataMapper personnelDataMapper,
            IReprisePersonnelRepository reprisePersonnelRepository)
        {
            this.uow = uow;
            this.personnelExtractorService = personnelExtractorService;
            this.personnelContextProvider = personnelContextProvider;
            this.personnelValidatorService = personnelValidatorService;
            this.personnelDataMapper = personnelDataMapper;
            this.reprisePersonnelRepository = reprisePersonnelRepository;
        }

        /// <summary>
        /// Importation des Personnels
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>Le resultat de l'import</returns>
        public ImportPersonnelResult CreatePersonnel(int groupeId, Stream stream)
        {
            ImportPersonnelResult result = new ImportPersonnelResult();

            // Recuperation des données de la feuille excel.
            ParsePersonnelResult parsageResult = personnelExtractorService.ParseExcelFile(stream);

            // Récuperations de toutes les données qui sont necessaires pour faire un import des Personnels
            ContextForImportPersonnel context = personnelContextProvider.GetContextForImportPersonnel(groupeId, parsageResult.Personnels);

            // Verification des règles(RG) de l'import.
            PersonnelImportRulesResult importRulesResult = personnelValidatorService.VerifyImportRules(parsageResult.Personnels, context);

            if (importRulesResult.AllLignesAreValid())
            {
                result.IsValid = true;

                // Ici je crée les Personnels
                List<PersonnelEnt> personnelEnts = personnelDataMapper.Transform(context, parsageResult.Personnels);

                if (personnelEnts.Count > 0)
                {
                    reprisePersonnelRepository.CreatePersonnel(personnelEnts);

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
