using Fred.Entities.Commande;

namespace Fred.Entities.Avis
{
    /// <summary>
    /// Entité représentant la relation Avis - Commande
    /// </summary>
    public class AvisCommandeEnt
    {
        /// <summary>
        /// Obtient ou définti l'identifiant unique de la classe
        /// </summary>
        public int? AvisCommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'avis
        /// </summary>
        public int? AvisId { get; set; }

        /// <summary>
        /// Obtient ou définit l'avis
        /// </summary>
        public AvisEnt Avis { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la commande
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit la commande
        /// </summary>
        public CommandeEnt Commande { get; set; }
    }
}
