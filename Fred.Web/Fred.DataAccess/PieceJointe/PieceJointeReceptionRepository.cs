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
    /// Repository Attachement Piece Jointe - Réception
    /// </summary>
    public class PieceJointeReceptionRepository : FredRepository<PieceJointeReceptionEnt>, IPieceJointeReceptionRepository
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="PieceJointeReceptionRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public PieceJointeReceptionRepository(FredDbContext context)
          : base(context)
        {

        }

        /// <summary>
        /// Récupérer les pièces jointes attachées à une réception
        /// </summary>
        /// <param name="entiteId">L'identifiant de la réception</param>
        /// <returns>Liste des pièces jointes</returns>
        public List<PieceJointeEnt> GetPiecesJointes(int entiteId)
        {
            return Query()
                .Filter(a => a.ReceptionId == entiteId)
                .Include(b => b.PieceJointe)
                .Get()
                .Select(b => b.PieceJointe)
                .AsNoTracking()
                .ToList();
        }
    }
}
