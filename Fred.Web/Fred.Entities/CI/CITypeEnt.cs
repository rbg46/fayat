namespace Fred.Entities.CI
{
    /// <summary>
    /// Représente un type de <see cref="CIEnt"/>.
    /// </summary>
    public class CITypeEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un type de <see cref="CIEnt"/>.
        /// </summary>
        public int CITypeId { get; set; }

        /// <summary>
        /// Obtient ou définit la designation du type.
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Obtient ou définit la clé ressource pour récupérer le nom du type.
        /// </summary>
        public string RessourceKey { get; set; }

        /// <summary>
        /// Obtient ou definit le code du type
        /// </summary>
        public string Code { get; set; }
    }
}
