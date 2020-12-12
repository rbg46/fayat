using System.Diagnostics;

namespace Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison
{
    /// <summary>
    /// Représente une devise.
    /// </summary>
    [DebuggerDisplay("{Libelle}")]
    public class DeviseDao
    {
        /// <summary>
        /// L'identifiant de la devise.
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        /// Le symbole de la devise.
        /// </summary>
        public string Symbole { get; set; }

        /// <summary>
        /// Le code ISO de la devise.
        /// </summary>
        public string IsoCode { get; set; }

        /// <summary>
        /// Le libellé de la devise.
        /// </summary>
        public string Libelle { get; set; }
    }
}
