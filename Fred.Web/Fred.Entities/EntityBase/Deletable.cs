using Fred.Entities.Utilisateur;
using System;

namespace Fred.Entities.EntityBase
{
    /// <summary>
    ///  Represente une entité ou il y aura un auteur et une date de création, auteur et date modification, auteur et date de suppression
    /// </summary>
    public abstract class Deletable : AuditableEntity, IDeletable
    {
        private DateTime? dateSuppression;

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
        ///   Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la suppression
        /// </summary>
        public virtual UtilisateurEnt AuteurSuppression { get; set; }
    }
}
