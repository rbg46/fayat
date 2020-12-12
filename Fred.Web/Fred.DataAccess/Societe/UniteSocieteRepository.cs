using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Societe
{
    /// <summary>
    ///   Référentiel de données pour les sociétés.
    /// </summary>
    public class UniteSocieteRepository : FredRepository<UniteSocieteEnt>, IUniteSocieteRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="UniteSocieteRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public UniteSocieteRepository(FredDbContext context)
          : base(context)
        {

        }

        /// <inheritdoc/>
        public void AddSocieteUnite(UniteSocieteEnt uniteSocieteEnt)
        {
            Insert(uniteSocieteEnt);
        }

        /// <inheritdoc/>
        public void UpdateSocieteUnite(UniteSocieteEnt uniteSocieteEnt)
        {
            Update(uniteSocieteEnt);
        }

        /// <inheritdoc/>
        public void DeleteSocieteUnite(int uniteSocieteId)
        {
            DeleteById(uniteSocieteId);
        }

        /// <inheritdoc/>
        public List<UniteSocieteEnt> GetListSocieteUnite(int societeId)
        {
            try
            {
                return Get().AsNoTracking().Where(us => us.SocieteId == societeId).Include(us => us.Unite).ToList();
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }
    }
}