using System;
using System.Collections.Generic;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Budget
{
    /// <summary>
    /// Représente un élément d'une bibliothèque de prix.
    /// </summary>
    public class BudgetBibliothequePrixItemEnt
    {
        private DateTime dateCreation;
        private DateTime? dateModification;
        private DateTime? dateSuppression;

        /// <summary>
        /// Obtient ou définit l'identifiant de l'élément de la bibliothèque des prix.
        /// </summary>
        public int BudgetBibliothequePrixItemId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la bibliothèque des prix.
        /// </summary>
        public int BudgetBibliothequePrixId { get; set; }

        /// <summary>
        /// Obtient ou définit la bibliothèque des prix.
        /// </summary>
        public BudgetBibliothequePrixEnt BudgetBibliothequePrix { get; set; }

        /// <summary>
        /// Représente l'historique des valeurs associées à cet item
        /// </summary>
        public ICollection<BudgetBibliothequePrixItemValuesHistoEnt> ItemValuesHisto { get; set; }

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

        /// <summary>
        /// Obtient ou définit l'identifiant de la ressource.
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit la ressource.
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du créateur.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit le créateur.
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création.
        /// </summary>
        public DateTime DateCreation
        {
            get { return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc); }
            set { dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        /// Obtient ou définit l'identifiant du modificateur.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit le modificateur.
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de modification.
        /// </summary>
        public DateTime? DateModification
        {
            get { return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit l'identifiant du suppresseur.
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit le suppresseur.
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit la date de suppression.
        /// </summary>
        public DateTime? DateSuppression
        {
            get { return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

    }
}
