using System.Diagnostics;

namespace Fred.Business.Budget.BudgetComparaison.Dto
{
    /// <summary>
    /// Représente une révision de budget.
    /// </summary>
    [DebuggerDisplay("{Revision} - {Etat}")]
    public class BudgetRevisionDto
    {
        /// <summary>
        /// L'identifiant de la révision du budget.
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// La révision du budget.
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// L'état du budget.
        /// </summary>
        public string Etat { get; set; }

        /// <summary>
        /// La période de début du budget à laquelle il prend effet (au format YYYYMM).
        /// </summary>
        public string PeriodeDebut { get; set; }

        /// <summary>
        /// La période de fin du budget à laquelle il cesse de faire effet (au format YYYYMM).
        /// </summary>
        public string PeriodeFin { get; set; }
    }
}
