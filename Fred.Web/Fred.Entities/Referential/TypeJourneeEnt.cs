namespace Fred.Entities.Referential
{
    /// <summary>
    /// Représente un type de journée
    /// </summary>
    public class TypeJourneeEnt
    {
        /// <summary>
        /// Clé primaire
        /// </summary>
        public int TypeJourneeId { get; set; }

        /// <summary>
        /// Obtient ou définit le code 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Permettant d’indiquer si le code est actif ou non
        /// </summary>
        public bool IsActif { get; set; }
    }
}
