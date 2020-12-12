using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.PieceJointe
{
    /// <summary>
    /// Représente une relation Commande - Pièce jointe
    /// </summary>
    public class PieceJointeCommandeModel
    {
        /// <summary>
        ///   Obtient ou définit l'ID de la pièce jointe
        /// </summary>
        public int PieceJointeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la commande
        /// </summary>
        public int CommandeId { get; set; }
    }
}
