using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.Business.Budget.BudgetComparaison.Dto.Comparaison.Result
{
    /// <summary>
    /// Représente un groupe.
    /// </summary>
    [DebuggerDisplay("Montant = {Montant}")]
    public class GroupDto
    {
        /// <summary>
        /// La quantité.
        /// </summary>
        public decimal? Quantite { get; set; }

        /// <summary>
        /// Les unités.
        /// </summary>
        public List<int?> UniteIds { get; set; }

        /// <summary>
        /// Le prix unitaire.
        /// </summary>
        public decimal? PrixUnitaire { get; set; }

        /// <summary>
        /// Le montant.
        /// </summary>
        public decimal? Montant { get; set; }
    }
}
