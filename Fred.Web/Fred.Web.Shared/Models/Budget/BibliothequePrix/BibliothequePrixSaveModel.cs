using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget.BibliothequePrix
{
    /// <summary>
    /// Modèle d'enregistrement d'une bibliothèque de prix.
    /// </summary>
    public class BibliothequePrixSave
    {
        #region Model

        /// <summary>
        /// Modèle d'enregistrement d'une bibliothèque de prix.
        /// </summary>
        public class Model
        {
            /// <summary>
            /// L'identifiant de l'organisation.
            /// </summary>
            public int OrganisationId { get; set; }

            /// <summary>
            /// L'identifiant de la devise.
            /// </summary>
            public int DeviseId { get; set; }

            /// <summary>
            /// La liste des éléments à enregistrer.
            /// </summary>
            public List<ItemModel> Items { get; set; }
        }

        #endregion
        #region ItemModel

        /// <summary>
        /// Représente les données d'un élément de la bibliothèque des prix.
        /// les valeurs prix et Unite Peuvent etre nullable car l'utilisateur peut décider de retirer d'anciennes valeurs 
        /// Pour cela il suffit de mettre ces deux champs a null, l'historique des items de la bibliotheque des prix sera alors mis à jour
        /// </summary>
        public class ItemModel
        {
            /// <summary>
            /// L'identifiant de la ressource.
            /// </summary>
            public int RessourceId { get; set; }

            /// <summary>
            /// Le prix.
            /// </summary>
            public decimal? Prix { get; set; }

            /// <summary>
            /// L'identifiant de l'unité.
            /// </summary>
            public int? UniteId { get; set; }
        }

        #endregion
        #region ResultModel

        /// <summary>
        /// Représente le résultat de l'enregistrement.
        /// </summary>
        public class ResultModel
        {
            /// <summary>
            /// Instant de l'enregistrement
            /// </summary>
            public DateTime DateSave { get; set; }
        }

        #endregion
    }
}
