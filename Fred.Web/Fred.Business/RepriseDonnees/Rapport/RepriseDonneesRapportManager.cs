using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Business.RepriseDonnees.Rapport.ContextProviders;
using Fred.Business.RepriseDonnees.Rapport.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.Rapport.Mapper;
using Fred.Business.RepriseDonnees.Rapport.Validators;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Groupe;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.Rapport
{
    public class RepriseDonneesRapportManager : IRepriseDonneesRapportManager
    {
        private readonly IRapportDataMapper rapportDataMapper;
        private readonly IRapportContextProvider rapportContextProvider;
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IRepriseRapportRepository repriseRapportRepository;
        private readonly IRapportExtractorService rapportExtractorService;
        private readonly IRepriseRapportValidatorService repriseRapportValidatorService;

        public RepriseDonneesRapportManager(
            IRapportDataMapper rapportDataMapper,
            IRapportContextProvider rapportContextProvider,
            IRepriseDonneesRepository repriseDonneesRepository,
            IRepriseRapportRepository repriseRapportRepository,
            IRapportExtractorService rapportExtractorService,
            IRepriseRapportValidatorService repriseRapportValidatorService)
        {
            this.rapportDataMapper = rapportDataMapper;
            this.rapportContextProvider = rapportContextProvider;
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.repriseRapportRepository = repriseRapportRepository;
            this.rapportExtractorService = rapportExtractorService;
            this.repriseRapportValidatorService = repriseRapportValidatorService;
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
        /// Importation des Rapports 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportRapportResult ImportRapports(int groupeId, Stream stream)
        {
            var result = new ImportRapportResult();

            // Recuperation des données de la feuille excel.
            var parsageResult = rapportExtractorService.ParseExcelFile(stream);

            // Récuperations de toutes les données qui sont necessaires pour faire un import Rapport(creation).
            var context = rapportContextProvider.GetContextForImportRapports(groupeId, parsageResult.Rapports);

            // verification des règles(RG) de l'import des rapports.
            var importRapportsRulesResult = repriseRapportValidatorService.VerifyImportRules(parsageResult.Rapports, context);

            if (importRapportsRulesResult.AllLignesAreValid())
            {
                result.IsValid = true;

                // Ici je mets les valeurs recu du fichier excel dans l'entité de la base fred (RapportEnt).
                var transformRapportResult = rapportDataMapper.Transform(context, parsageResult.Rapports);
                var rapportEnts = transformRapportResult.Rapports;
                var rapportLignes = transformRapportResult.RapportLignes;

                if (rapportEnts.Any())
                {
                    repriseRapportRepository.SaveEntities(rapportEnts, rapportLignes);
                }
            }
            else
            {
                result.ErrorMessages = importRapportsRulesResult.ImportRuleResults.Where(x => !x.IsValid).Select(x => x.ErrorMessage).ToList();
            }
            return result;
        }
    }
}
