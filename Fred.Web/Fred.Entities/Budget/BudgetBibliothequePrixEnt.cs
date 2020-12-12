using System;
using System.Collections.Generic;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Budget
{
    /// <summary>
    /// Représente une bibliothèque de prix.
    /// </summary>
    public class BudgetBibliothequePrixEnt
    {
        private DateTime dateCreation;

        /// <summary>
        /// Obtient ou définit l'identifiant de la bibliothèque de prix.
        /// </summary>
        public int BudgetBibliothequePrixId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'organisation.
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'organisation.
        /// </summary>
        public OrganisationEnt Organisation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la devise.
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        /// Obtient ou définit la devise.
        /// </summary>
        public DeviseEnt Devise { get; set; }

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
        /// Obtient ou défini la liste des éléments de la bibliothèque des prix.
        /// </summary>
        public ICollection<BudgetBibliothequePrixItemEnt> Items { get; set; }
    }
}
