using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.PieceJointe
{
    /// <summary>
    /// Représente une relation Reception - Pièce jointe
    /// </summary>
    public class PieceJointeReceptionModel
    {
        /// <summary>
        ///   Obtient ou définit l'ID de la pièce jointe
        /// </summary>
        public int PieceJointeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la réception
        /// </summary>
        public int ReceptionId { get; set; }
    }
}
