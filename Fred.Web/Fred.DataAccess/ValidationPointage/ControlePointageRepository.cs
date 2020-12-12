using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ValidationPointage;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.ValidationPointage
{
    /// <summary>
    ///   Classe ControlePointageRepository
    /// </summary>
    public class ControlePointageRepository : FredRepository<ControlePointageEnt>, IControlePointageRepository
    {
        /// <summary>
        ///   Constructeur <seealso cref="ControlePointageRepository"/>
        /// </summary>
        /// <param name="logMgr">Gestionnaire des logs</param>
        /// <param name="uow">Unit of work</param>
        public ControlePointageRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc />
        public ControlePointageEnt Get(int controlePointageId)
        {
            return GetDefaultQuery()
                   .FirstOrDefault(x => x.ControlePointageId == controlePointageId);
        }

        /// <inheritdoc />
        public ControlePointageEnt GetLatest(int lotPointageId, int typeControle)
        {
            return GetDefaultQuery()
                   .Where(x => x.LotPointageId == lotPointageId && x.TypeControle == typeControle)
                   .OrderByDescending(x => x.DateDebut)
                   .FirstOrDefault();
        }

        /// <inheritdoc />
        public IEnumerable<ControlePointageEnt> GetAll()
        {
            return GetDefaultQuery();
        }

        /// <summary>
        ///   Requête par défaut de récupération des Validations de pointage
        /// </summary>
        /// <returns>Requête par défaut</returns>
        private IQueryable<ControlePointageEnt> GetDefaultQuery()
        {
            return Query()
                   .Include(x => x.AuteurCreation.Personnel)
                   .Include(x => x.AuteurCreation.AffectationsRole)
                   .Include(x => x.LotPointage)
                   .Get()
                   .AsNoTracking();
        }

        /// <inheritdoc />
        public IEnumerable<ControlePointageEnt> GetList(int lotPointageId)
        {
            return GetDefaultQuery()
                  .Where(x => x.LotPointageId == lotPointageId);
        }

        /// <inheritdoc />
        public IEnumerable<ControlePointageEnt> GetLatestList(List<int> listLotsPointagesIds)
        {
            return Query()
                   .Get()
                   .Where(x => listLotsPointagesIds.Contains(x.LotPointageId))
                   .GroupBy(x => x.TypeControle)
                   .Select(s => s.OrderByDescending(x => x.DateDebut).FirstOrDefault())
                   .Include(x => x.AuteurCreation.Personnel)
                   .Include(x => x.LotPointage);
        }

        /// <inheritdoc />
        public IEnumerable<ControlePointageEnt> GetList(int lotPointageId, int typeControle)
        {
            return GetDefaultQuery()
                   .Where(x => x.LotPointageId == lotPointageId && x.TypeControle == typeControle);
        }

        /// <inheritdoc />
        public ControlePointageEnt AddControlePointage(ControlePointageEnt controlePointage)
        {
            Insert(controlePointage);

            return controlePointage;
        }

        /// <inheritdoc />
        public ControlePointageEnt UpdateControlePointage(ControlePointageEnt controlePointage)
        {
            Update(controlePointage);

            return controlePointage;
        }

        /// <inheritdoc />
        public void DeleteControlePointage(int controlePointageId)
        {
            DeleteById(controlePointageId);
        }
    }
}
