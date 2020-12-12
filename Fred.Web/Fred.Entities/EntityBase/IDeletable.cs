using System;

namespace Fred.Entities.EntityBase
{
    /// <summary>
    ///  Represente une entité ou il y aura un auteur et une date de création, auteur et date modification, auteur et date de suppression
    /// </summary>
    public interface IDeletable : IAuditableEntity
    {
        /// <summary>
        /// AuteurSuppressionId
        /// </summary>
        int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// DateSuppression
        /// </summary>
        DateTime? DateSuppression { get; set; }
    }
}
