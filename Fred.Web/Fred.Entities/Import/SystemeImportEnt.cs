namespace Fred.Entities.Import
{
    /// <summary>
    /// Représente le systèmes d’import de données
    /// </summary>
    public class SystemeImportEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant.
        /// </summary>
        public int SystemeImportId { get; set; }

        /// <summary>
        /// Obtient ou définit le code interne.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la description.
        /// </summary>
        public string Description { get; set; }
    }
}
