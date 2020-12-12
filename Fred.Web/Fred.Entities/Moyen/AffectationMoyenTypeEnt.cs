namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente le type d'affectation d'un moyen
    /// </summary>
    public class AffectationMoyenTypeEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant du type d'affectation d'un moyen
        /// </summary>
        public int AffectationMoyenTypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le code du type d'affectation
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé du type d'affectation
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le code du CI
        /// </summary>
        // A revoir pour le prochain refacto des code Cis => [System.Obsolete("Abhération conceptuelle. Ne utiliser.", false)]
        // Il faut créer une interface pour gérer les Cis à utiliser pour la maintenance et la restitution
        public string CiCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la famille d'affectation
        /// </summary>
        public int AffectationMoyenFamilleId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité famille d'affectation
        /// </summary>
        public AffectationMoyenFamilleEnt AffectationMoyenFamille { get; set; }
    }
}
