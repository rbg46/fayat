using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RapportPrime;
using Fred.EntityFramework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.RapportPrime
{
    /// <summary>
    ///   Référentiel de données pour les primes d'une ligne de rapport de prime.
    /// </summary>
    public class RapportPrimeLignePrimeRepository : FredRepository<RapportPrimeLignePrimeEnt>, IRapportPrimeLignePrimeRepository
    {
        public RapportPrimeLignePrimeRepository(FredDbContext context)
            : base(context)
        {
        }

        private enum ActionType
        {
            Insert = 1,
            Delete = 2,
            Update = 3
        }

        /// <summary>
        /// Supprime une liste de ligne de prime
        /// </summary>
        /// <param name="list">Liste à insérer</param>
        public async Task DeleteRangeAsync(List<RapportPrimeLignePrimeEnt> list)
        {
            await ActionOnRangeAsync(list, ActionType.Delete);
        }

        /// <summary>
        /// Permet l'insertion d'une liste de ligne de prime
        /// </summary>
        /// <param name="list">Liste à insérer</param>
        public async Task InsertRangeAsync(List<RapportPrimeLignePrimeEnt> list)
        {
            await ActionOnRangeAsync(list, ActionType.Insert);
        }

        /// <summary>
        /// Mise à jour par liste de ligne de prime
        /// </summary>
        /// <param name="list">Lite à mettre à jour</param>
        public async Task UpdateRangeAsync(List<RapportPrimeLignePrimeEnt> list)
        {
            await ActionOnRangeAsync(list, ActionType.Update);
        }

        private async Task ActionOnRangeAsync(List<RapportPrimeLignePrimeEnt> list, ActionType action)
        {
            if (list?.Any() != true)
            {
                return;
            }

            using (var dbcontext = new FredDbContext())
            {
                using (var dbContextTransaction = dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        // disable detection of changes
                        dbcontext.ChangeTracker.AutoDetectChangesEnabled = false;

                        if (action == ActionType.Insert)
                        {
                            // Ajout des primes d'une ligne de rapport de prime (donc une liste de RapportPrimeLignePrime)
                            dbcontext.RapportPrimeLignePrime.AddRange(list);
                        }
                        else if (action == ActionType.Delete)
                        {
                            // Vu qu'on est pas dans le meme context que celui qui a récupéré les items, on doit les Attacher à ce context pour pouvoir les supprimer
                            // Contrairement à l'InsertRange qui lui n'a pas besoin de connaitre l'entité (attach) pour pouvoir l'ajouter
                            foreach (RapportPrimeLignePrimeEnt lignePrime in list)
                            {
                                dbcontext.RapportPrimeLignePrime.Attach(lignePrime);
                            }

                            // Ajout des primes d'une ligne de rapport de prime (donc une liste de RapportPrimeLignePrime)
                            dbcontext.RapportPrimeLignePrime.RemoveRange(list);
                        }
                        else
                        { // action == ActionType.Update
                            // Ajout des primes d'une ligne de rapport de prime (donc une liste de RapportPrimeLignePrime)
                            foreach (RapportPrimeLignePrimeEnt lignePrime in list)
                            {
                                dbcontext.RapportPrimeLignePrime.Attach(lignePrime);
                                dbcontext.Entry(lignePrime).State = EntityState.Modified;
                            }
                        }

                        await dbcontext.SaveChangesAsync();
                        dbContextTransaction.Commit();

                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new FredRepositoryException(e.Message, e);
                    }
                    finally
                    {
                        // re-enable detection of changes
                        dbcontext.ChangeTracker.AutoDetectChangesEnabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Renvoie la liste des lignes de rapports de ligne de primes donné sur une periode
        /// </summary>
        /// <param name="periode">Periode choisie</param>
        /// <returns>Liste de primes de lignes de rapport de primes</returns>
        public List<RapportPrimeLignePrimeEnt> GetRapportPrimeLignePrime(DateTime periode)
        {
            DateTime startDate = periode.GetLimitsOfMonth().StartDate;
            DateTime endDate = periode.GetLimitsOfMonth().EndDate;
            return Context.RapportPrimeLignePrime
                          .Include(x => x.RapportPrimeLigne.RapportPrime)
                          .Include(x => x.RapportPrimeLigne.Personnel)
                          .Include(x => x.RapportPrimeLigne.Ci)
                          .Include(x => x.Prime)
                          .Where(q => !q.RapportPrimeLigne.DateSuppression.HasValue &&
                                      q.RapportPrimeLigne.DateValidation >= startDate &&
                                      q.RapportPrimeLigne.DateValidation <= endDate)
                          .ToList();
        }

        /// <inheritdoc />
        public async Task<int> GetRapportPrimeLignePrimeIdAsync(int primeId)
        {
            return await Context.RapportPrimeLignePrime
                .Where(s => s.PrimeId == primeId)
                .Select(s => s.RapportPrimeLignePrimeId)
                .FirstOrDefaultAsync();
        }
    }
}
