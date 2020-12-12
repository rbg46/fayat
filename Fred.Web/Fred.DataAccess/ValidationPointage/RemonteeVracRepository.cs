using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ValidationPointage;
using Fred.EntityFramework;

namespace Fred.DataAccess.ValidationPointage
{
    /// <summary>
    ///   Classe remonteeVracRepository
    /// </summary>
    public class RemonteeVracRepository : FredRepository<RemonteeVracEnt>, IRemonteeVracRepository
    {
        /// <summary>
        ///   Constructeur <seealso cref="RemonteeVracRepository"/>
        /// </summary>
        /// <param name="logMgr">Gestionnaire des logs</param>
        /// <param name="uow">Unit of work</param>
        public RemonteeVracRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc />
        public RemonteeVracEnt AddRemonteeVrac(RemonteeVracEnt remonteeVrac)
        {
            Insert(remonteeVrac);

            return remonteeVrac;
        }

        /// <inheritdoc />
        public void DeleteRemonteeVrac(int remonteeVracId)
        {
            DeleteById(remonteeVracId);
        }

        /// <inheritdoc />
        public RemonteeVracEnt Get(int remonteeVracId)
        {
            return GetDefaultQuery()
                   .FirstOrDefault(x => x.RemonteeVracId == remonteeVracId);

        }

        /// <inheritdoc />
        public RemonteeVracEnt GetLatest(int utilisateurId, DateTime periode)
        {
            return GetDefaultQuery()
                   .Where(x => x.AuteurCreationId == utilisateurId && x.Periode.Month == periode.Month && x.Periode.Year == periode.Year)
                   .OrderByDescending(x => x.DateDebut)
                   .FirstOrDefault();
        }

        /// <inheritdoc />
        public IEnumerable<RemonteeVracEnt> GetAll()
        {
            return GetDefaultQuery();
        }

        /// <summary>
        ///   Requête par défaut de récupération des Remontées Vrac
        /// </summary>
        /// <returns>Requête par défaut</returns>
        private IQueryable<RemonteeVracEnt> GetDefaultQuery()
        {
            return Query()
                   .Include(x => x.AuteurCreation.Personnel)
                   .Include(x => x.Erreurs)
                   .Get();
        }

        /// <inheritdoc />
        public IEnumerable<RemonteeVracEnt> GetList(int auteurCreationId)
        {
            return GetDefaultQuery()
                   .Where(x => x.AuteurCreationId == auteurCreationId);
        }

        /// <inheritdoc />
        public IEnumerable<RemonteeVracEnt> GetList(DateTime periode)
        {
            return GetDefaultQuery()
                   .Where(x => x.Periode.Month == periode.Month && x.Periode.Year == periode.Year);
        }

        /// <inheritdoc />
        public RemonteeVracEnt UpdateRemonteeVrac(RemonteeVracEnt remonteeVrac)
        {
            Update(remonteeVrac);

            return remonteeVrac;
        }
    }
}
