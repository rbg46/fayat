using System;
using System.Linq.Expressions;
using Fred.Entities.Budget;

namespace Fred.Web.Shared.Models.Budget.Details.SousDetail
{
    /// <summary>
    /// Model utilisé pour charger les éléments de la bibliotheque des prix dans le sous détail d'un budget
    /// </summary>
    public class BibliothequePrixItemModel
    {
        /// <summary>
        /// Prix référencé dans la bibliotheque des prix pour cette ressource
        /// </summary>
        public decimal? Prix { get; set; }

        /// <summary>
        /// Unite Id référencée pour cette ressource
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        /// La Ressource
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Selector permettant d'obtenir un objet Model depuis un objet entité
        /// </summary>
        public static Expression<Func<BudgetBibliothequePrixItemEnt, BibliothequePrixItemModel>> Selector
        {
            get
            {
                return i => new BibliothequePrixItemModel
                {
                    Prix = i.Prix,
                    UniteId = i.UniteId,
                    RessourceId = i.RessourceId
                };
            }
        }
    }
}
