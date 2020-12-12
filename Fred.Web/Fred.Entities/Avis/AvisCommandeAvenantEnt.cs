using Fred.Entities.Commande;

namespace Fred.Entities.Avis
{
    /// <summary>
    /// Entité représentant la relation Avis - CommandeAvenant
    /// </summary>
    public class AvisCommandeAvenantEnt
    {
        /// <summary>
        /// Obtient ou définti l'identifiant unique de la classe
        /// </summary>
        public int? AvisCommandeAvenantId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'avis
        /// </summary>
        public int? AvisId { get; set; }

        /// <summary>
        /// Obtient ou définit l'avis
        /// </summary>
        public AvisEnt Avis { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'avenant d'une commande
        /// </summary>
        public int? CommandeAvenantId { get; set; }

        /// <summary>
        /// Obtient ou définit l'avenant d'une commande
        /// </summary>
        public CommandeAvenantEnt CommandeAvenant { get; set; }
    }
}
