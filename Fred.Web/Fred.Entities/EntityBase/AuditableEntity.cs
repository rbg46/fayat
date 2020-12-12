using Fred.Entities.Utilisateur;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.EntityBase
{
    /// <summary>
    /// Une AuditableEntity correspond a une entité ui peux stocké en base l'id de l'utilisateur qui
    /// créé et met a jour l'entite ainsi que les dates correspondantes.
    /// </summary>
    public abstract class AuditableEntity : IAuditableEntity
    {

        private DateTime? dateCreation;
        private DateTime? dateModification;
        /// <summary>
        ///   Obtient ou définit l'ID du saisisseur de l'entité.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant saisi l'entité
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de saisie de l'entité.
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
        ///   Obtient ou définit l'ID de la personne ayant modifier l'entité.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant modifier l'entité
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la dernière date de modification de l'entité.
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
    }
}
