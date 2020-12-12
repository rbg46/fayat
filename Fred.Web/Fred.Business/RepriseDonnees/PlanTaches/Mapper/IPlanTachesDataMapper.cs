using System.Collections.Generic;
using Fred.Business.RepriseDonnees.PlanTaches.Models;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.PlanTaches.Mapper
{
    /// <summary>
    /// Transform les données du fichier excel en plan de taches
    /// </summary>
    public interface IPlanTachesDataMapper : IService
    {
        /// <summary>
        /// Creer un Plan de tâches d'une liste de RepriseExcelPlanTaches
        /// </summary>
        /// <param name="context">Context contenant les data necessaire a l'import</param>
        /// <param name="listRepriseExcelPlanTaches">Le Plan de tâches sous la forme excel</param>
        /// <returns>Liste de Taches</returns>
        List<TacheEnt> Transform(ContextForImportPlanTaches context, List<RepriseExcelPlanTaches> listRepriseExcelPlanTaches);
    }
}
