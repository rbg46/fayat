using System.Collections.Generic;
using Fred.Business.RepriseDonnees.PlanTaches.Models;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.PlanTaches.ContextProviders
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des Plans de taches
    /// </summary>
    public interface IPlanTachesContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des Plan de taches
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelPlanTaches">repriseExcelPlanTaches</param>
        /// <returns>les données necessaires a l'import des Plans de taches</returns>
        ContextForImportPlanTaches GetContextForImportPlanTaches(int groupeId, List<RepriseExcelPlanTaches> repriseExcelPlanTaches);
    }
}
