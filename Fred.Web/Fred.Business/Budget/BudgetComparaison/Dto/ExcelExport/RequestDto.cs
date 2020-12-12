using System.Collections.Generic;
using Fred.Business.Budget.BudgetComparaison.Dto.ExcelExport.Request;

namespace Fred.Business.Budget.BudgetComparaison.Dto.ExcelExport
{
    /// <summary>
    /// Représente la requête de l'export Excel.
    /// </summary>
    public class RequestDto
    {
        /// <summary>
        /// L'identifiant du premier budget.
        /// </summary>
        public int BudgetId1 { get; set; }

        /// <summary>
        /// L'identifiant du second budget.
        /// </summary>
        public int BudgetId2 { get; set; }

        /// <summary>
        /// les axes choisis.
        /// </summary>
        public List<AxeType> Axes { get; set; }

        /// <summary>
        /// L'axe principal.
        /// </summary>
        public AxePrincipalType AxePrincipal { get; set; }

        /// <summary>
        /// Les colonnes à exporter.
        /// </summary>
        public ColonnesDto Colonnes { get; set; }
    }
}
