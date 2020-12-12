using System.Collections.Generic;
using Fred.Entities;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Interface Repo Attachement Pièce Jointe - Commande 
    /// </summary>
    public interface IPieceJointeCommandeRepository : IRepository<PieceJointeCommandeEnt>
    {
        /// <summary>
        /// Récupérer les pièces jointes attachées à une commande
        /// </summary>
        /// <param name="entiteId">L'identifiant de la commande</param>
        /// <returns>Liste des pièces jointes</returns>
        List<PieceJointeEnt> GetPiecesJointes(int entiteId);
    }
}
