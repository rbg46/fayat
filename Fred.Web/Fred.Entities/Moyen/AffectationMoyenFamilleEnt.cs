namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente un regroupement des types des affectations des moyens
    /// </summary>
    public class AffectationMoyenFamilleEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de la famille d'affectation d'un moyen
        /// </summary>
        public int AffectationMoyenFamilleId { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la famille d'affectation
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de la famille d'affectation
        /// </summary>
        public string Libelle { get; set; }
    }
}
