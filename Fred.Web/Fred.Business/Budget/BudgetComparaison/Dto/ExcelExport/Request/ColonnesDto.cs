namespace Fred.Business.Budget.BudgetComparaison.Dto.ExcelExport.Request
{
    /// <summary>
    /// Représente les colonnes.
    /// </summary>
    public class ColonnesDto
    {
        /// <summary>
        /// Les colonnes du budget 1.
        /// </summary>
        public GroupColonnesDto Budget1 { get; set; }

        /// <summary>
        /// Les colonnes du budget 2.
        /// </summary>
        public GroupColonnesDto Budget2 { get; set; }

        /// <summary>
        /// Les colonnes de l'écart.
        /// </summary>
        public GroupColonnesDto Ecart { get; set; }
    }
}
