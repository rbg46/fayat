using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Web.Models.Commande;

namespace Fred.Web.Shared.Models.Avis
{
    /// <summary>
    /// Modèle d'une relation Avis - Commande
    /// </summary>
    public class AvisCommandeModel
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
        public AvisModel Avis { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la commande
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit la commande
        /// </summary>
        public CommandeModel Commande { get; set; }
    }
}
