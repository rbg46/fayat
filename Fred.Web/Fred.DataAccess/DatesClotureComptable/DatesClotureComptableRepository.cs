using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.DatesClotureComptable;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Fred.Framework.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.DatesClotureComptable
{
    /// <summary>
    ///   Référentiel de données pour les sociétés.
    /// </summary>
    public class DatesClotureComptableRepository : FredRepository<DatesClotureComptableEnt>, IDatesClotureComptableRepository
    {
        private readonly ILogManager logManager;

        public DatesClotureComptableRepository(FredDbContext context, ILogManager logManager)
           : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Retourne une liste des clotures comptables par ciId et année
        /// </summary>
        /// <param name="ciId">Un CI</param>
        /// <param name="year">Une année</param>
        /// <returns>La liste des clotures comptables</returns>
        public IEnumerable<DatesClotureComptableEnt> GetCIListDatesClotureComptableByIdAndYear(int ciId, int year)
        {
            return Context.DatesCloturesComptables.Where(o => o.CiId == ciId && o.Annee == year && !o.Historique)
               .OrderBy(o => o.Mois)
               .AsNoTracking()
               .ToList();
        }

        /// <summary>
        ///  Retourne une liste de DatesClotureComptableEnt par ci postérieur à une période donnée
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="periodeDebut">Une periode de début</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        public IEnumerable<DatesClotureComptableEnt> GetListDatesClotureComptableByCiGreaterThanPeriode(int ciId, int periodeDebut)
        {
            int year = periodeDebut / 100;
            int month = periodeDebut % 100;
            return Context.DatesCloturesComptables.Where(o => o.CiId == ciId && !o.Historique && ((o.Annee == year && o.Mois >= month) || o.Annee > year)).OrderBy(o => o.Annee).ThenBy(o => o.Mois).ToList();
        }

        /// <summary>
        /// Retourne une liste de DatesClotureComptableEnt par ci postérieur à une période donnée
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="month">Mois de début</param>
        /// <param name="year">Année de début</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        public IEnumerable<DatesClotureComptableEnt> GetListDatesClotureComptableByCiGreaterThanPeriode(int ciId, int month, int year)
        {
            return Context.DatesCloturesComptables.Where(o => o.CiId == ciId && !o.Historique && (o.Annee >= year || (o.Annee == year && o.Mois >= month))).OrderBy(o => o.Annee).ThenBy(o => o.Mois).ToList();
        }

        /// <summary>
        /// Retourne une date cloture comptable par CI, année, mois
        /// </summary>
        /// <param name="ciId">Un CI</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Une cloture comptable</returns>
        public DatesClotureComptableEnt Get(int ciId, int year, int month)
        {
            return Query()
              .Filter(dc => dc.CiId == ciId && dc.Annee == year && dc.Mois == month)
              .Filter(dcc => !dcc.Historique)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }

        /// <summary>
        ///  Retourne une date cloture comptable pour une période comprise entre deux date
        /// </summary>
        /// <param name="ciId">Un CI</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Une cloture comptable</returns>
        public List<DatesClotureComptableEnt> Get(int ciId, DateTime startDate, DateTime endDate)
        {
            return Query()
              .Filter(dc => dc.CiId == ciId)
              .Filter(dcc => !dcc.Historique)
              .Filter(dc => ((100 * dc.Annee) + dc.Mois >= (100 * startDate.Year) + startDate.Month)
                          && ((100 * dc.Annee) + dc.Mois <= (100 * endDate.Year) + endDate.Month))
              .Get()
              .AsNoTracking().ToList();
        }

        /// <summary>
        ///   Rerourne une liste de DatesClotureComptableEnt pour une liste de CI,une année et un mois
        /// </summary>
        /// <param name="ciIds">liste de Ci</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        public IEnumerable<DatesClotureComptableEnt> Get(List<int> ciIds, int year, int month)
        {
            return Query()
              .Filter(dc => ciIds.Contains(dc.CiId) && dc.Annee == year && dc.Mois == month)
              .Filter(dcc => !dcc.Historique)
              .Get()
              .AsNoTracking()
              .ToList();
        }

        /// <summary>
        ///  Retourne une date cloture comptable par CI, année, mois.
        ///  S'io n'y a pas de data, alors une valeur par defaut est retournée.
        /// </summary>
        /// <param name="ciId">Un CI</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Une cloture comptable</returns>
        public DatesClotureComptableEnt GetCIDatesClotureComptableByIdAndYearAndMonthOrDefault(int ciId, int year, int month)
        {
            DatesClotureComptableEnt result = Query()
              .Filter(dc => dc.CiId == ciId && dc.Annee == year && dc.Mois == month)
              .Filter(dcc => !dcc.Historique)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();

            if (result == null)
            {
                result = new DatesClotureComptableEnt()
                {
                    CiId = ciId,
                    Annee = year,
                    Mois = month
                };
            }
            return result;
        }

        /// <summary>
        /// Recupere l'annee et le mois recedant et suivant
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="year">year</param>
        /// <returns>List de DatesClotureComptableEnt</returns>
        public IEnumerable<DatesClotureComptableEnt> GetYearAndPreviousNextMonths(int ciId, int year)
        {
            List<DatesClotureComptableEnt> currentYear = Context.DatesCloturesComptables.Where(o => o.CiId == ciId && o.Annee == year && !o.Historique)
                                                             .OrderBy(o => o.Mois)
                                                             .AsNoTracking()
                                                             .ToList();

            List<DatesClotureComptableEnt> previousMonth = Context.DatesCloturesComptables.Where(o => o.CiId == ciId && o.Annee == year - 1 && o.Mois == 12 && !o.Historique)
                                                               .OrderBy(o => o.Mois)
                                                               .AsNoTracking()
                                                               .ToList();

            List<DatesClotureComptableEnt> nextMonth = Context.DatesCloturesComptables.Where(o => o.CiId == ciId && o.Annee == year + 1 && o.Mois == 1 && !o.Historique)
                                                           .OrderBy(o => o.Mois)
                                                           .AsNoTracking()
                                                           .ToList();
            currentYear.AddRange(previousMonth);
            currentYear.AddRange(nextMonth);

            return currentYear;
        }

        /// <summary>
        ///  Créer une cloture comptable
        /// </summary>
        /// <param name="dcc">DatesClotureComptableEnt</param>
        /// <param name="currentUserId">L'ID de l'utilisateur courrant.</param>
        /// <returns>La DatesClotureComptableEnt créé</returns>
        public DatesClotureComptableEnt CreateDatesClotureComptable(DatesClotureComptableEnt dcc, int currentUserId)
        {
            try
            {
                dcc.Historique = false;
                Context.DatesCloturesComptables.Add(dcc);
                Context.SaveChangesWithAudit(currentUserId);
                CloneAndSave(dcc);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                logManager.TraceException(exception.Message, exception);
                throw;
            }

            return dcc;
        }

        /// <summary>
        ///  Met à jour une cloture comptable
        /// </summary>
        /// <param name="dcc">Une DatesClotureComptableEnt</param>
        /// <param name="currentUserId">L'ID de l'utilisateur courrant.</param>
        /// <returns>Une DatesClotureComptableEnt modifié</returns>
        public DatesClotureComptableEnt ModifyDatesClotureComptable(DatesClotureComptableEnt dcc, int currentUserId)
        {
            try
            {
                Context.Entry(dcc).State = EntityState.Modified;
                dcc.Historique = false;
                Context.SaveChangesWithAudit(currentUserId);
                CloneAndSave(dcc);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
            return dcc;
        }

        /// <summary>
        /// Ajoute un historique dans la base. Copie de la DatesClotureComptableEnt, sauf l'id.
        /// </summary>
        /// <param name="dcc">Une DatesClotureComptableEnt a copié.</param>
        private void CloneAndSave(DatesClotureComptableEnt dcc)
        {
            DatesClotureComptableEnt entity = Context.DatesCloturesComptables
                          .AsNoTracking()
                          .FirstOrDefault(x => x.DatesClotureComptableId == dcc.DatesClotureComptableId);

            entity.DatesClotureComptableId = 0;

            entity.Historique = true;

            Context.DatesCloturesComptables.Add(entity);

            Context.SaveChanges();
        }

        /// <summary>
        /// Permet d'ajouter des dates de clôture comptable en masse
        /// </summary>
        /// <param name="datesClotureComptables">Liste des dates de clôture comptable</param>
        public void InsertInMass(List<DatesClotureComptableEnt> datesClotureComptables)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (DatesClotureComptableEnt datesClotureComptable in datesClotureComptables)
                    {
                        // On récupère une date de clôture comptable en fonction du CI, de l'année et du mois.
                        DatesClotureComptableEnt datesClotureComptableExisted = GetDatesClotureComptable(datesClotureComptable.CiId, datesClotureComptable.Annee, datesClotureComptable.Mois);

                        // Si la date de clôture comptable existe, on l'historise.
                        if (datesClotureComptableExisted != null)
                        {
                            datesClotureComptableExisted.Historique = true;
                            datesClotureComptableExisted.DateModification = DateTime.UtcNow;
                            Context.Entry(datesClotureComptableExisted).State = EntityState.Modified;
                        }

                        // On ajoute une date de clôture comptable.
                        datesClotureComptable.DateModification = datesClotureComptable.DateCreation = DateTime.UtcNow;
                        Context.DatesCloturesComptables.Add(datesClotureComptable);
                    }

                    Context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Permet de récupéré une date cloture comptable.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <param name="annee">L'année.</param>
        /// <param name="mois">Le mois.</param>
        /// <returns>Une date cloture comptable.</returns>
        private DatesClotureComptableEnt GetDatesClotureComptable(int ciId, int annee, int mois)
        {
            return Query()
                    .Filter(dc => dc.CiId == ciId && dc.Annee == annee && dc.Mois == mois)
                    .Filter(dcc => !dcc.Historique)
                    .Get()
                    .FirstOrDefault();
        }

        /// <summary>
        /// Retourne l'ensemble des dates de clôture d'un CI.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <returns>L'ensemble des dates de clôture du CI.</returns>
        public IEnumerable<DatesClotureComptableEnt> Get(int ciId)
        {
            return Context.DatesCloturesComptables.Where(dc => dc.CiId == ciId);
        }

        /// <summary>
        /// Retourne l'ensemble des dates de clôture d'un CI pour la synchronisation mobile.
        /// </summary>
        /// <returns>L'ensemble des dates de clôture du CI.</returns>
        public IEnumerable<DatesClotureComptableEnt> GetAllSync()
        {
            return Context.DatesCloturesComptables.AsNoTracking();
        }

        /// <summary>
        /// Retourne la dernière date de cloture pour un CI
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <returns>La dernière date de cloture pour un ci</returns>
        public DateTime GetLastDateClotureByCiID(int ciId)
        {
            DatesClotureComptableEnt dateExist = Query().Filter(d => d.CiId == ciId && !d.Historique && d.DateCloture != null).Get().FirstOrDefault();

            if (dateExist == null)
            {
                return new DateTime(1900, 1, 1);
            }

            int annee = Query()
                  .Filter(d => d.CiId == ciId && !d.Historique && d.DateCloture != null)
                  .Get()
                  .Max(d => d.Annee);

            int mois = Query()
                        .Filter(d => d.CiId == ciId && d.Annee == annee && !d.Historique && d.DateCloture != null)
                        .Get()
                        .Max(d => d.Mois);

            if (mois == 12)
            {
                mois = 0;
                annee++;
            }

            DateTime date = new DateTime(annee, mois + 1, 1);

            return date;
        }


        public List<DatesClotureComptableEnt> GetLastDateClotures(List<int> ciIds)
        {
            IQueryable<IGrouping<int, DatesClotureComptableEnt>> ciGroup = this.Context.DatesCloturesComptables
                                                             .Where(x => ciIds.Contains(x.CiId))
                                                             .Where(x => !x.Historique && x.DateCloture.HasValue)
                                                             .GroupBy(x => x.CiId);
            return ciGroup.Select(x => x.OrderByDescending(y => y.Annee * 100 + y.Mois).FirstOrDefault())
                                             .AsNoTracking()
                                             .ToList();
        }


        /// <summary>
        /// Permet d'executer un lot de requette OR avec Ef.       
        /// </summary>
        /// <param name="queries">Les requettes</param>
        /// <param name="batchSize">Permet de faire des bache de requette OR (default 50 OR)</param>
        /// <returns>les DatesClotureComptableEnt</returns>
        public List<DatesClotureComptableEnt> ExecuteOrQueries(List<Expression<Func<DatesClotureComptableEnt, bool>>> queries, int batchSize = 50)
        {
            ExpressionStarter<DatesClotureComptableEnt> predicate = PredicateBuilder.New<DatesClotureComptableEnt>();

            IQueryable<DatesClotureComptableEnt> expandable = Context.DatesCloturesComptables.AsExpandable();

            List<DatesClotureComptableEnt> datesClotureComptables = new List<DatesClotureComptableEnt>();

            foreach (IEnumerable<Expression<Func<DatesClotureComptableEnt, bool>>> batch in queries.Batch(batchSize))
            {
                foreach (Expression<Func<DatesClotureComptableEnt, bool>> q in batch)
                {
                    predicate = predicate.Or(q);
                }
                List<DatesClotureComptableEnt> batchRequest = expandable.Where(predicate).AsNoTracking().ToList();

                datesClotureComptables.AddRange(batchRequest);
            }

            return datesClotureComptables;
        }

        public void AddRange(IEnumerable<DatesClotureComptableEnt> datesComptableAndCis)
        {
            Context.DatesCloturesComptables.AddRange(datesComptableAndCis);
        }
    }
}
