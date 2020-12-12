using System.Diagnostics;

namespace Fred.Entities.Budget.Dao.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Représente les révisions des budgets comparés.
    /// </summary>
    [DebuggerDisplay("{Revision} - {Etat}")]
    public class BudgetsRevisionsDao
    {
        /// <summary>
        /// La révision du budget 1.
        /// </summary>
        public BudgetRevisionDao Budget1 { get; set; }

        /// <summary>
        /// La révision du budget 2.
        /// </summary>
        public BudgetRevisionDao Budget2 { get; set; }
    }
}
