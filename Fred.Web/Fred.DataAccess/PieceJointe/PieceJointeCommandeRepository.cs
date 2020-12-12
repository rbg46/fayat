using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.PieceJointe
{
    /// <summary>
    /// Repository Attachement Piece Jointe - Commande
    /// </summary>
    public class PieceJointeCommandeRepository : FredRepository<PieceJointeCommandeEnt>, IPieceJointeCommandeRepository
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="PieceJointeCommandeRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public PieceJointeCommandeRepository(FredDbContext context)
          : base(context)
        {

        }

        /// <summary>
        /// Récupérer les pièces jointes attachées à une commande
        /// </summary>
        /// <param name="entiteId">L'identifiant de la commande</param>
        /// <returns>Liste des pièces jointes</returns>
        public List<PieceJointeEnt> GetPiecesJointes(int entiteId)
        {
            return Context.PieceJointeCommandes
                 .Where(a => a.CommandeId == entiteId)
                 .Select(b => b.PieceJointe)
                 .AsNoTracking()
                 .ToList();
        }
    }
}
