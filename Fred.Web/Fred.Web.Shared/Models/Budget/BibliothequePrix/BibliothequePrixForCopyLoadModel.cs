using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Fred.Entities.Budget;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.Budget.BibliothequePrix
{
    /// <summary>
    /// Modèle de chargement d'une bibliothèque de prix pour la copie.
    /// </summary>
    public class BibliothequePrixForCopyLoad
    {
        #region ResultModel

        /// <summary>
        /// Résultat du chargement d'une bibliothèque de prix.
        /// </summary>
        public class ResultModel : ErreurResultModel
        {
            /// <summary>
            /// Identifiant de l'organisation chargée.
            /// </summary>
            public int OrganisationId { get; set; }

            /// <summary>
            /// Identifiant de la devise chargée.
            /// </summary>
            public int DeviseId { get; set; }

            /// <summary>
            /// Liste des unités disponibles.
            /// </summary>
            public List<UniteModel> Unites { get; set; }

            /// <summary>
            /// Liste des éléments de la bibliothèque des prix.
            /// </summary>
            public List<ItemModel> Items { get; set; }
        }

        #endregion
        #region UniteModel

        /// <summary>
        /// Représente une unité.
        /// </summary>
        [DebuggerDisplay("Unité {Code} - {Libelle}")]
        public class UniteModel
        {
            /// <summary>
            /// Selector permettant de constuire ce modèle a partir d'une unité.
            /// </summary>
            public static Expression<Func<UniteEnt, UniteModel>> Selector
            {
                get
                {
                    return (unite) => new UniteModel
                    {
                        UniteId = unite.UniteId,
                        Code = unite.Code,
                        Libelle = unite.Libelle
                    };
                }
            }

            /// <summary>
            /// L'identifiant de l'unité.
            /// </summary>
            public int UniteId { get; set; }

            /// <summary>
            /// Le code de l'unité.
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Le libellé de l'unité.
            /// </summary>
            public string Libelle { get; set; }
        }

        #endregion
        #region ItemModel

        /// <summary>
        /// Représente les données d'un élément de la bibliothèque des prix.
        /// </summary>
        public class ItemModel
        {
            /// <summary>
            /// Selector permettant de constuire ce modèle a partir d'un élément d'une bibliothèque des prix.
            /// </summary>
            public static Expression<Func<BudgetBibliothequePrixItemEnt, ItemModel>> Selector
            {
                get
                {
                    return item => new ItemModel
                    {
                        RessourceId = item.RessourceId,
                        Prix = item.Prix,
                        UniteId = item.UniteId,
                    };
                }
            }

            /// <summary>
            /// L'identifiant de la ressource.
            /// </summary>
            public int RessourceId { get; set; }

            /// <summary>
            /// Le prix de l'élément de la bibliothèque des prix.
            /// </summary>
            public decimal? Prix { get; set; }

            /// <summary>
            /// L'identifiant de l'unité de l'élément de la bibliothèque des prix.
            /// </summary>
            public int? UniteId { get; set; }
        }

        #endregion
    }
}
