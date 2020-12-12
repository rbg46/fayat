using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Budget;

namespace Fred.Web.Shared.Models.Budget.BibliothequePrix
{
    /// <summary>
    /// Modèle de chargement de l'historique d'un item de la bibliothèque des prix.
    /// </summary>
    public class BibliothequePrixItemHistoLoadModel
    {
        #region Model

        /// <summary>
        /// Représente l'historique d'un item.
        /// </summary>
        public class Model
        {
            /// <summary>
            /// L'historique de l'item.
            /// </summary>
            public List<ItemModel> Histo { get; set; }
        }

        #endregion
        #region ItemModel

        /// <summary>
        /// Représente un élément de l'historique.
        /// </summary>
        public class ItemModel
        {
            /// <summary>
            /// Selector permettant de constuire ce modèle a partir d'un élément d'une bibliothèque des prix.
            /// </summary>
            public static Expression<Func<BudgetBibliothequePrixItemValuesHistoEnt, ItemModel>> Selector
            {
                get
                {
                    return itemHisto => new ItemModel
                    {
                        Prix = itemHisto.Prix,
                        UniteLibelle = itemHisto.Unite.Libelle,
                        DateInsertion = itemHisto.DateInsertion
                    };
                }
            }

            /// <summary>
            /// Le prix de l'élément de la bibliothèque des prix.
            /// </summary>
            public decimal? Prix { get; set; }

            /// <summary>
            /// L'identifiant de l'unité de l'élément de la bibliothèque des prix.
            /// </summary>
            public string UniteLibelle { get; set; }

            /// <summary>
            /// Date d'insertion de la valeur dans l'historique
            /// </summary>
            public DateTime DateInsertion { get; set; }
        }

        #endregion
    }
}
