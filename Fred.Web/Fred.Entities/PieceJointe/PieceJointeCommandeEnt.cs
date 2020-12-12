using Fred.Entities.Commande;
using Fred.Entities.EntityBase;
using System.Diagnostics;

namespace Fred.Entities
{
    /// <summary>
    /// Entité représentant un attachement d'une pièce jointe à une commande
    /// </summary>
    [DebuggerDisplay("{PieceJointeCommandeId}")]
    public class PieceJointeCommandeEnt : AuditableEntity
    {
        /// <summary>
        /// Obtient ou définti l'identifiant de l'attachement
        /// </summary>
        public int PieceJointeCommandeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la pièce jointe
        /// </summary>
        public int PieceJointeId { get; set; }

        /// <summary>
        ///   Obtient ou définit la pièce jointe de la commande.        
        /// </summary>
        public PieceJointeEnt PieceJointe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la commande
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        ///   Obtient ou définit la commande
        /// </summary>
        public CommandeEnt Commande { get; set; }

        /// <summary>
        ///   Supprimer les propriétés de l'objet
        /// </summary>
        public void CleanProperties()
        {
            PieceJointe = null;
            Commande = null;
        }
    }
}
