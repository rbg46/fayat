using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Depense
{
    /// <summary>
    ///   Référentiel de données pour les lot de FARs.
    /// </summary>
    public class LotFarRepository : FredRepository<LotFarEnt>, ILotFarRepository
    {

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="LotFarRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="uow">The unit of work.</param>
        public LotFarRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc />
        public IEnumerable<LotFarEnt> GetAll()
        {
            return this.Query().Include(x => x.AuteurCreation).Get().AsNoTracking();
        }

        /// <inheritdoc />
        public LotFarEnt GetById(int lotFarId)
        {
            return Query().Include(x => x.AuteurCreation).Get().AsNoTracking().FirstOrDefault(x => x.LotFarId.Equals(lotFarId));
        }

        /// <inheritdoc />
        public LotFarEnt GetByNumeroLot(int numLot)
        {
            return Query().Include(x => x.AuteurCreation).Get().AsNoTracking().FirstOrDefault(x => x.NumeroLot.Equals(numLot));
        }

        /// <inheritdoc />
        public LotFarEnt AddLotFar(LotFarEnt lotFar)
        {
            Insert(lotFar);

            return lotFar;
        }
    }
}