using System;
using System.Linq.Expressions;
using Fred.Entities.Budget;

namespace Fred.Business.Budget.BudgetManager.Dtao
{
    /// <summary>
    /// Décrit le model à utiliser lors du chargement des items de la bibliotheque des prix pendant la copie d'un budget
    /// </summary>
    public class BudgetCopieBibliothequePrixItemDao
    {
        /// <summary>
        /// Prix.
        /// </summary>
        public decimal? Prix { get; set; }

        /// <summary>
        /// Identifiant de l'unité.
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        /// Identifiant de la ressource.
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Le type de l'organisation.
        /// </summary>
        public string TypeOrganisation { get; set; }

        /// <summary>
        /// Selector permettant de constuire ce modèle a partir de l'entité.
        /// </summary>
        public Expression<Func<BudgetBibliothequePrixItemEnt, BudgetCopieBibliothequePrixItemDao>> Selector
        {
            get
            {
                return item => new BudgetCopieBibliothequePrixItemDao
                {
                    Prix = item.Prix,
                    UniteId = item.UniteId,
                    RessourceId = item.RessourceId,
                    TypeOrganisation = item.BudgetBibliothequePrix.Organisation.TypeOrganisation.Code
                };
            }
        }
    }
}
