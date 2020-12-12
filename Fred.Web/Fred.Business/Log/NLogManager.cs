using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Log;
using Fred.Framework.Exceptions;

namespace Fred.Business.Log
{
    /// <summary>
    ///   Gestionnaire des logs
    /// </summary>
    public class NLogManager : Manager<NLogEnt, INLogRepository>, INLogManager
    {
        public NLogManager(IUnitOfWork uow, INLogRepository nLogRepository)
          : base(uow, nLogRepository)
        {
        }

        /// <inheritdoc />
        public List<NLogEnt> Search(string search, string level, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            try
            {
                IQueryable<NLogEnt> logs = this.Repository.Get();

                // Filtre par texte
                if (!string.IsNullOrEmpty(search.Trim()))
                {
                    logs = logs.Where(
                    x => x.Message.Contains(search)
                    || x.Exception.Contains(search)
                    );
                }

                // Filtre par level
                if (!string.IsNullOrEmpty(level.Trim()))
                {
                    logs = logs.Where(x => x.Level == level);
                }

                totalRecord = logs.Count();

                // Ordre
                logs = logs.OrderBy(sort + " " + sortdir);

                // Pagination
                if (pageSize > 0)
                {
                    logs = logs.Skip(skip).Take(pageSize);
                }

                return logs.ToList();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
