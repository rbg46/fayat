using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.PieceJointe
{
    /// <summary>
    /// Repository Piece Jointe
    /// </summary>
    public class PieceJointeRepository : FredRepository<PieceJointeEnt>, IPieceJointeRepository
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="PieceJointeRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public PieceJointeRepository(FredDbContext context)
          : base(context)
        {

        }
    }
}
