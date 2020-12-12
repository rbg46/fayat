using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Business.RepriseDonnees.Ci.ContextProviders;
using Fred.Business.RepriseDonnees.Ci.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.Ci.Mapper;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Business.RepriseDonnees.Ci.Validators;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Groupe;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.Ci
{
    public class RepriseDonneesCiManager : IRepriseDonneesCiManager
    {
        private readonly IUnitOfWork uow;
        private readonly ICiDataMapper ciDataMapper;
        private readonly ICiContextProvider ciContextProvider;
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly ICiExtractorService ciExtractorService;
        private readonly IRepriseCiValidatorService repriseCiValidatorService;
        private readonly IRepriseCiRepository repriseCiRepository;

        public RepriseDonneesCiManager(
            IUnitOfWork uow,
            ICiDataMapper ciDataMapper,
            ICiContextProvider ciContextProvider,
            IRepriseDonneesRepository repriseDonneesRepository,
            ICiExtractorService ciExtractorService,
            IRepriseCiValidatorService repriseCiValidatorService,
            IRepriseCiRepository repriseCiRepository)
        {
            this.uow = uow;
            this.ciDataMapper = ciDataMapper;
            this.ciContextProvider = ciContextProvider;
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.ciExtractorService = ciExtractorService;
            this.repriseCiValidatorService = repriseCiValidatorService;
            this.repriseCiRepository = repriseCiRepository;
        }

        /// <summary>
        /// Retourne tous les groupes
        /// </summary>
        /// <returns>Liste de groupes</returns>
        public List<GroupeEnt> GetAllGroupes()
        {
            return repriseDonneesRepository.GetAllGroupes();
        }

        /// <summary>
        /// Importation des Cis 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportCiResult ImportCis(int groupeId, Stream stream)
        {
            var result = new ImportCiResult();

            // Recuperation des données de la feuille excel.
            var parsageResult = ciExtractorService.ParseExcelFile(stream);

            // Récuperations de toutes les données qui sont necessaires pour faire un import CI(mise a jour).
            var context = ciContextProvider.GetContextForImportCis(groupeId, parsageResult.Cis);

            // verification des règles(RG) de l'import.
            var importRulesResult = repriseCiValidatorService.VerifyImportRules(parsageResult.Cis, context);

            if (importRulesResult.AllLignesAreValid())
            {
                result.IsValid = true;

                UpdateCis(parsageResult, context);
            }
            else
            {
                result.ErrorMessages = importRulesResult.ImportRuleResults.Where(x => !x.IsValid).Select(x => x.ErrorMessage).ToList();
            }

            return result;
        }

        /// <summary>
        /// Permet de mettre à jour dans la base de donnée les données ci Excel
        /// </summary>
        /// <param name="parsageResult"> donnée excel</param>
        /// <param name="context">ensembe de donnée nécessaire à l'import</param>
        public void UpdateCis(ParseCisResult parsageResult, ContextForImportCi context)
        {
            // Ici je mets les valeurs recu du fichier excel dans l'entité de la base fred.
            var ciEnts = ciDataMapper.Map(context, parsageResult.Cis);

            if (ciEnts.Any())
            {
                // ici, je signale au dbcontext les champs qui sont modifié, pour eviter la mise a jours d'autre champs que ceux explicitement demandé 
                repriseCiRepository.UpdateCis(ciEnts);

                uow.Save();
            }
        }
    }
}
