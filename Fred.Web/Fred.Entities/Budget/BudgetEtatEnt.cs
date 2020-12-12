namespace Fred.Entities.Budget
{
    /// <summary>
    ///   Représente un budget
    /// </summary>
    public class BudgetEtatEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un état de budget.
        /// </summary>
        public int BudgetEtatId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un état de budget.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un état de budget.
        /// </summary>
        public string Libelle { get; set; }
    }
}
