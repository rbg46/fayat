namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente un type d'énergie
    /// </summary>
    public class TypeEnergieEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un type de dépense.
        /// </summary>
        public int TypeEnergieId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un type de dépense.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un type de dépense.
        /// </summary>
        public string Libelle { get; set; }
    }
}
