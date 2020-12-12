using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.PlanTaches.ExcelDataExtractor
{
    /// <summary>
    /// Resultat du parsage des Plan de tâches a partir du fichier excel
    /// </summary>
    public class ParsePlanTachesResult
    {
        /// <summary>
        /// Liste des ci du fichier excel
        /// </summary>
        public List<RepriseExcelPlanTaches> PlanTaches { get; set; } = new List<RepriseExcelPlanTaches>();
    }
}
