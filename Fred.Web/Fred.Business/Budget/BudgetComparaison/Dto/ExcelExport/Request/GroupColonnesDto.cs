namespace Fred.Business.Budget.BudgetComparaison.Dto.ExcelExport.Request
{
    /// <summary>
    /// Représente les colonnes d'un groupe.
    /// </summary>
    public class GroupColonnesDto
    {
        /// <summary>
        /// La colonne quantité.
        /// </summary>
        public bool HasQuantite { get; set; }

        /// <summary>
        /// La colonne unité.
        /// </summary>
        public bool HasUnite { get; set; }

        /// <summary>
        /// La colonne prix unitaire.
        /// </summary>
        public bool HasPrixUnitaire { get; set; }
    }
}
