using System.Collections.Generic;
using Fred.Entities;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Interface Repo Attachement Pièce Jointe - Réception 
    /// </summary>
    public interface IPieceJointeReceptionRepository : IRepository<PieceJointeReceptionEnt>
    {
        /// <summary>
        /// Récupérer les pièces jointes attachées à une réception
        /// </summary>
        /// <param name="entiteId">L'identifiant de la réception</param>
        /// <returns>Liste des pièces jointes</returns>
        List<PieceJointeEnt> GetPiecesJointes(int entiteId);
    }
}
