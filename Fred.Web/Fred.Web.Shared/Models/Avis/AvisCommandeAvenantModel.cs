using Fred.Web.Shared.Models.Commande;

namespace Fred.Web.Shared.Models.Avis
{
    /// <summary>
    /// Modèle d'une relation 
    /// </summary>
    public class AvisCommandeAvenantModel
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
        public AvisModel Avis { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'avenant d'une commande
        /// </summary>
        public int? CommandeAvenantId { get; set; }

        /// <summary>
        /// Obtient ou définit l'avenant d'une commande
        /// </summary>
        public CommandeAvenantModel CommandeAvenant { get; set; }
    }
}
