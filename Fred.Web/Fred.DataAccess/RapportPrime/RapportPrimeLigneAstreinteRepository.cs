using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RapportPrime;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fred.DataAccess.RapportPrime
{
    /// <summary>
    ///   Référentiel de données pour les astreintes d'une ligne de rapport de prime.
    /// </summary>
    public class RapportPrimeLigneAstreinteRepository : FredRepository<RapportPrimeLigneAstreinteEnt>, IRapportPrimeLigneAstreinteRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="RapportPrimeLigneAstreinteRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public RapportPrimeLigneAstreinteRepository(FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        private enum ActionType
        {
            Insert = 1,
            Delete = 2
        }

        /// <summary>
        /// Supprime une liste de ligne d'astreinte
        /// </summary>
        /// <param name="list">Liste à insérer</param>
        public async Task DeleteRangeAsync(List<RapportPrimeLigneAstreinteEnt> list)
        {
            await ActionOnRange(list, ActionType.Delete);
        }

        /// <summary>
        /// Retourne la liste (non trackée par le contexte) des lignes d'astreinte pour une ligne de rapport de prime
        /// </summary>
        /// <param name="rapportPrimeLigne">Ligne de rapport de prime</param>
        /// <returns>List de RapportPrimeLigneAstreinteEnt </returns>
        public async Task<List<RapportPrimeLigneAstreinteEnt>> GetRapportPrimeLigneAstreintesAsync(RapportPrimeLigneEnt rapportPrimeLigne)
        {
            return await context.RapportPrimeLigneAstreinte
                .Where(x => x.RapportPrimeLigneId == rapportPrimeLigne.RapportPrimeLigneId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Retourne le RapportPrimeLigneAstreinte pour l'Astreinte et le RapportPrimeLigne données en paramètre, null sinon
        /// </summary>
        /// <param name="astreinteId">Id de l'astreinte</param>
        /// <param name="rapportPrimeLigneId">Id de la ligne de rapport de prime</param>
        /// <returns>RapportPrimeLigneAstreinteEnt ou null</returns>
        public async Task<RapportPrimeLigneAstreinteEnt> GetRapportPrimeLigneAstreinteAsync(int astreinteId, int rapportPrimeLigneId)
        {
            return await context.RapportPrimeLigneAstreinte
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AstreinteId == astreinteId && x.RapportPrimeLigneId == rapportPrimeLigneId);
        }

        public void RemoveRange(IEnumerable<RapportPrimeLigneAstreinteEnt> listAstreinteToDelete)
        {
            context.RapportPrimeLigneAstreinte.RemoveRange(listAstreinteToDelete);
        }

        /// <summary>
        /// Permet l'insertion d'une liste de ligne de prime d'astreinte
        /// </summary>
        /// <param name="list">Liste à insérer</param>
        public async Task InsertRangeAsync(List<RapportPrimeLigneAstreinteEnt> list)
        {
            await ActionOnRange(list, ActionType.Insert);
        }

        private static async Task ActionOnRange(List<RapportPrimeLigneAstreinteEnt> list, ActionType action)
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
                            // Ajout des astreintes dans la ligne de rapport de prime
                            dbcontext.RapportPrimeLigneAstreinte.AddRange(list);
                        }
                        else
                        { // action == ActionType.Delete

                            // Vu qu'on est pas dans le meme context que celui qui a récupéré les items, on doit les Attacher à ce context pour pouvoir les supprimer
                            // Contrairement à l'InsertRange qui lui n'a pas besoin de connaitre l'entité (attach) pour pouvoir l'ajouter
                            foreach (RapportPrimeLigneAstreinteEnt ligneAstreinte in list)
                            {
                                dbcontext.RapportPrimeLigneAstreinte.Attach(ligneAstreinte);
                            }

                            // Suppression des astreintes dans la ligne de rapport de prime
                            dbcontext.RapportPrimeLigneAstreinte.RemoveRange(list);
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
    }
}
