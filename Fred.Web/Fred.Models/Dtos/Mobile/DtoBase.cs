namespace Fred.Web.Dtos.Mobile
{
    /// <summary>
    /// Dto de base
    /// </summary>
    public abstract class DtoBase
    {
        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant créé la réception.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant modifié la réception.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant supprimer la réception.
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si supprimé
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}