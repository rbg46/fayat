namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Représente des informations de révision de budget.
    /// </summary>
    public class BudgetRevisionLoadModel
    {
        /// <summary>
        /// L'identifiant de la révision du budget.
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// La révision du budget.
        /// </summary>
        public string Revision { get; set; }
    }
}
