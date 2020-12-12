using System;
using Fred.Entities.Referential;

namespace Fred.Entities.Budget
{
    /// <summary>
    /// Historique des valeurs d'un item de la bibliotheque des prix
    /// </summary>
    public class BudgetBibliothequePrixItemValuesHistoEnt
    {
        /// <summary>
        /// Id unique et primarey key de la donnée
        /// </summary>
        public int BudgetBibliothequePrixItemValuesHistoId { get; set; }

        /// <summary>
        /// Date d'insertion de la ligne dans l'historique.
        /// </summary>
        public DateTime DateInsertion { get; set; }

        /// <summary>
        /// Id de l'item rattachée à cette ligne de l'historique
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Item rattachée à cette ligne de l'historique
        /// </summary>
        public BudgetBibliothequePrixItemEnt Item { get; set; }

        /// <summary>
        /// Obtient ou définit le prix.
        /// </summary>
        public decimal? Prix { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité.
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité.
        /// </summary>
        public UniteEnt Unite { get; set; }

    }
}
