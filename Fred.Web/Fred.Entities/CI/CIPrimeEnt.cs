using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System;

namespace Fred.Entities.CI
{
    /// <summary>
    ///   Représente un CIPrime (association entre un CI et une prime)
    /// </summary>
    public class CIPrimeEnt
    {
        private DateTime? dateSuppression;
        private DateTime? dateCreation;
        private DateTime? dateModification;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'association.
        /// </summary>
        public int CiPrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du CI associé.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la prime associée.
        /// </summary>

        public int PrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le CI associé
        /// </summary>
        public virtual CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit la prime associée
        /// </summary>
        public virtual PrimeEnt Prime { get; set; }

        #region Ajout, Mise à jour et suppression avec l'auteur

        /// <summary>
        ///   Obtient ou définit la date de création
        /// </summary>
        public DateTime? DateCreation
        {
            get
            {
                return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification
        {
            get
            {
                return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression
        {
            get
            {
                return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la création
        /// </summary>
        public virtual UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la modification
        /// </summary>
        public virtual UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la suppression
        /// </summary>
        public virtual UtilisateurEnt AuteurSuppression { get; set; }

        #endregion Ajout, Mise à jour et suppression avec l'auteur
    }
}