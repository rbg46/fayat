namespace Fred.Entities.Referential
{
    /// <summary>
    /// Représente un statut d'absence
    /// </summary>
    public class StatutAbsenceEnt
    {
        /// <summary>
        /// Clé primaire
        /// </summary>
        public int StatutAbsenceId { get; set; }

        /// <summary>
        /// Obtient ou définit le code 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Niveau du statut permettant de hiérarchiser les différents statuts
        /// </summary>
        public int Niveau { get; set; }

        /// <summary>
        /// Permettant d’indiquer si le code est actif ou non
        /// </summary>
        public bool IsActif { get; set; }
    }
}
