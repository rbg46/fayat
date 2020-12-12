using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.RepriseDonnees.PlanTaches.Models;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.PlanTaches.ContextProviders
{
    public class PlanTachesContextProvider : IPlanTachesContextProvider
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IUtilisateurManager utilisateurManager;

        public PlanTachesContextProvider(
            IRepriseDonneesRepository repriseDonneesRepository,
            IOrganisationTreeService organisationTreeService,
            IUtilisateurManager utilisateurManager)
        {
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.organisationTreeService = organisationTreeService;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des Plan de taches
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelPlanTaches">repriseExcelPlanTaches</param>
        /// <returns>les données necessaires a l'import des Plans de taches</returns>
        public ContextForImportPlanTaches GetContextForImportPlanTaches(int groupeId, List<RepriseExcelPlanTaches> repriseExcelPlanTaches)
        {
            ContextForImportPlanTaches result = new ContextForImportPlanTaches();

            result.GroupeId = groupeId;

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();

            result.FredIeUser = utilisateurManager.GetByLogin("fred_ie");

            List<string> ciCodes = repriseExcelPlanTaches.Select(x => x.CodeCi).Distinct().ToList();
            result.CisUsedInExcel = repriseDonneesRepository.GetCisByCodes(ciCodes);

            List<string> tacheCodes = repriseExcelPlanTaches.Select(x => x.CodeTache).Distinct().ToList();
            result.TachesUsedInExcel = repriseDonneesRepository.GetTachesByCodes(tacheCodes);

            List<string> tacheParentCodes = repriseExcelPlanTaches.Select(x => x.CodeTacheParent).Distinct().ToList();
            result.TachesParentsUsedInExcel = repriseDonneesRepository.GetTachesByCodes(tacheParentCodes);

            return result;
        }
    }
}
