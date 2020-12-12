using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Fred.ImportExport.DataAccess.Log;
using Fred.ImportExport.Entities;
using Fred.ImportExport.Framework.Exceptions;

namespace Fred.ImportExport.Business.Log
{
    public class LogManager
    {
        private readonly LogRepository logRepository;

        public LogManager(LogRepository logRepository)
        {
            this.logRepository = logRepository;
        }

        public List<NLogFredIeEnt> GetAll()
        {
            try
            {
                return logRepository.Get().ToList();
            }
            catch (Exception e)
            {
                throw new FredIeBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de rechercher les logs.
        /// </summary>
        /// <param name="search">Le texte pour filtrer.</param>
        /// <param name="startDate">La date de début de la periode</param>
        /// <param name="endDate">La date de fin de la periode</param>
        /// <param name="level">Le niveau de log.</param>
        /// <param name="fluxCode">Le code du flux.</param>
        /// <param name="sort">La colonne pour ordonner la liste.</param>
        /// <param name="sortdir">Le sens du tri.</param>
        /// <param name="totalRecord">Le nombre d'élément totale pour la pagination.</param>
        /// <param name="page">L'index pour la pagination.</param>
        /// <param name="pageSize">Le nombre d'élément par page pour la pagination.</param>    
        /// <returns>Une liste de logs.</returns>
        public List<NLogFredIeEnt> Search(string search, DateTime? startDate, DateTime? endDate, string level, string fluxCode, string sort, string sortdir, out int totalRecord, int page = 1, int pageSize = 10)
        {
            try
            {
                IQueryable<NLogFredIeEnt> logs = logRepository.Get();
                var filters = new List<Expression<Func<NLogFredIeEnt, bool>>>();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    filters.Add(x => x.Message.Contains(search.Trim()) || x.Exception.Contains(search.Trim()));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    sortdir = "asc";
                    DateTime startDateUtc = startDate.Value.ToUniversalTime();
                    DateTime endDateUtc = endDate.Value.ToUniversalTime();
                    filters.Add(x => DateTime.Compare(x.Logged, startDateUtc) >= 0
                                && DateTime.Compare(x.Logged, endDateUtc) <= 0);
                }
                else if (startDate.HasValue && !endDate.HasValue)
                {
                    sortdir = "asc";
                    DateTime startDateUtc = startDate.Value.ToUniversalTime();
                    filters.Add(x => DateTime.Compare(x.Logged, startDateUtc) >= 0);
                }
                else if (!startDate.HasValue && endDate.HasValue)
                {
                    DateTime endDateUtc = endDate.Value.ToUniversalTime();
                    filters.Add(x => DateTime.Compare(x.Logged, endDateUtc) <= 0);
                }

                if (!string.IsNullOrWhiteSpace(level))
                {
                    filters.Add(x => x.Level == level);
                }

                if (!string.IsNullOrWhiteSpace(fluxCode))
                {
                    filters.Add(x => x.Message.Contains(fluxCode.Trim()));
                }

                filters.ForEach(f => { logs = logs.Where(f); });

                totalRecord = logs.Count();
                if (totalRecord == 0)
                {
                    return new List<NLogFredIeEnt>();
                }

                logs = logs.OrderBy(sort + " " + sortdir);
                return logs.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception e)
            {
                throw new FredIeBusinessException(e.Message, e);
            }
        }
    }
}
