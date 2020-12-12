using System;

namespace Fred.Entities.EntityBase
{
    /// <summary>
    /// Une IAuditableEntity correspond a une entité ui peux stocké en base l'id de l'utilisateur qui
    /// créé et met a jour l'entite ansi que les dates correspondantes.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// AuteurCreationId
        /// </summary>
        int? AuteurCreationId { get; set; }

        /// <summary>
        /// DateCreation
        /// </summary>
        DateTime? DateCreation { get; set; }

        /// <summary>
        /// AuteurModificationId
        /// </summary>
        int? AuteurModificationId { get; set; }

        /// <summary>
        /// DateModification
        /// </summary>
        DateTime? DateModification { get; set; }
    }
}