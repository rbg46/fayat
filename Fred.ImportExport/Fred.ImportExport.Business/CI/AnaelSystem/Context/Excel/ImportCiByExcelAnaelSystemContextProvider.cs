using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.CI.AnaelSystem.Anael;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des cis a partir d'un fichier excel
    /// </summary>
    public class ImportCiByExcelAnaelSystemContextProvider : IImportCiByExcelAnaelSystemContextProvider
    {
        private readonly ISocieteManager societeManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IEtablissementComptableManager etablissementComptableManager;
        private readonly ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider;

        public ImportCiByExcelAnaelSystemContextProvider(
            ISocieteManager societeManager,
            IOrganisationTreeService organisationTreeService,
            IEtablissementComptableManager etablissementComptableManager,
            ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider)
        {
            this.societeManager = societeManager;
            this.organisationTreeService = organisationTreeService;
            this.etablissementComptableManager = etablissementComptableManager;
            this.commonAnaelSystemContextProvider = commonAnaelSystemContextProvider;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">Logger</param>       
        /// <returns>les données necessaires a l'import des cis</returns>
        public ImportCiContext<ImportCisByExcelInputs> GetContext(ImportCisByExcelInputs input, CiImportExportLogger logger)
        {
            var result = new ImportCiContext<ImportCisByExcelInputs>();

            result.Input = input;

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();

            var societesOfGroupe = result.OrganisationTree.GetAllSocietesForGroupe(input.GroupeId);

            result.SocietesNeeded = GetSocietesUsedInExcel(input.RepriseImportCis, societesOfGroupe);

            var excelGroupsBySocieteCode = input.RepriseImportCis.GroupBy(x => x.CodeSociete);

            foreach (var excelGroupBySocieteCode in excelGroupsBySocieteCode)
            {
                var societeContext = new ImportCiSocieteContext();

                societeContext.Societe = result.SocietesNeeded.FirstOrDefault(x => x.Code == excelGroupBySocieteCode.Key);

                societeContext.TypeSocietes = commonAnaelSystemContextProvider.GetTypeSocietes();

                if (societeContext.Societe != null)
                {
                    societeContext.EtablissementComptables = etablissementComptableManager.GetListBySocieteId(societeContext.Societe.SocieteId).ToList();

                    //log etablissement comptable de la societe
                    logger.LogEtablissementComptablesOfSociete(societeContext.Societe, societeContext.EtablissementComptables);

                    societeContext.AnaelCis = GetAnaelCis(societeContext.Societe.CodeSocieteComptable, excelGroupBySocieteCode.Select(x => x.CodeCi).ToList());

                    //log cis anael
                    logger.LogAnaelModels(societeContext.AnaelCis);
                }
                result.SocietesContexts.Add(societeContext);
            }

            return result;
        }

        private List<SocieteEnt> GetSocietesUsedInExcel(List<RepriseExcelCi> repriseImportCis, List<OrganisationBase> societesOfGroupe)
        {
            var excelSocietes = repriseImportCis.Select(x => x.CodeSociete).Distinct().ToList();

            var societeOragnisations = societesOfGroupe.Where(x => excelSocietes.Contains(x.Code)).ToList();

            var societeIds = societeOragnisations.Select(x => x.Id).Distinct().ToList();

            return societeManager.GetAllSocietesByIds(societeIds);
        }

        private List<CiAnaelModel> GetAnaelCis(string codeSocieteComptable, List<string> codeCisOfSociete)
        {
            var anaelCiProvider = new AnaelCiProvider();

            return anaelCiProvider.GetCisFromAnael(codeSocieteComptable, codeCisOfSociete, applyFilterOnResult: false);
        }

    }
}
