using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.PlanTaches.Mapper
{
    /// <summary>
    /// Classe de mapping entre une tache et une RepriseExcelPlanTache
    /// </summary>
    public class TacheExcelMapResult
    {
        /// <summary>
        /// Tache crée a partie du RepriseExcelPlanTache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        /// RepriseExcelPlanTache representant une ligne excel
        /// </summary>
        public RepriseExcelPlanTaches RepriseExcelPlanTache { get; set; }
    }
}
