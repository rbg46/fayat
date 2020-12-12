namespace Fred.Entities.Fonctionnalite
{
    /// <summary>
    /// Resultat d'une requette pour savoir quel fonctionnalite correspond  une permission
    /// </summary>
    public class FonctionnaliteForPermissionResponse
    {
        /// <summary>
        /// SocieteId
        /// </summary>
        public int SocieteId { get; set; }
        /// <summary>
        /// FonctionnaliteId
        /// </summary>
        public int FonctionnaliteId { get; set; }
        /// <summary>
        /// RoleId
        /// </summary>
        public int RoleId { get; set; }
    }
}
