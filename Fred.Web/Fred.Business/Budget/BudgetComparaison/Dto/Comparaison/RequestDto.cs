using System.Collections.Generic;

namespace Fred.Business.Budget.BudgetComparaison.Dto.Comparaison
{
    /// <summary>
    /// Représente la requête de la comparaison de budget.
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
    }
}
