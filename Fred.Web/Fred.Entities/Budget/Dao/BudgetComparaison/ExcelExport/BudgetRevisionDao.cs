using System.Diagnostics;

namespace Fred.Entities.Budget.Dao.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Représente une révision de budget.
    /// </summary>
    [DebuggerDisplay("{Revision} - {Etat}")]
    public class BudgetRevisionDao
    {
        /// <summary>
        /// La révision du budget.
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// L'état du budget.
        /// </summary>
        public string Etat { get; set; }
    }
}
