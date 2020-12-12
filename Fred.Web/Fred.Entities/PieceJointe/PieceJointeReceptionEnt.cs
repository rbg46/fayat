using Fred.Entities.Depense;
using Fred.Entities.EntityBase;
using System.Diagnostics;

namespace Fred.Entities
{
    /// <summary>
    /// Entité représentant un attachement d'une pièce jointe à une réception
    /// </summary>
    [DebuggerDisplay("{PieceJointeReceptionId}")]
    public class PieceJointeReceptionEnt : AuditableEntity
    {
        /// <summary>
        /// Obtient ou définti l'identifiant de l'attachement
        /// </summary>
        public int PieceJointeReceptionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la pièce jointe
        /// </summary>
        public int PieceJointeId { get; set; }

        /// <summary>
        ///   Obtient ou définit la pièce jointe de la réception.        
        /// </summary>
        public PieceJointeEnt PieceJointe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la réception
        /// </summary>
        public int ReceptionId { get; set; }

        /// <summary>
        ///   Obtient ou définit la réception
        /// </summary>
        public DepenseAchatEnt Reception { get; set; }
    }
}
