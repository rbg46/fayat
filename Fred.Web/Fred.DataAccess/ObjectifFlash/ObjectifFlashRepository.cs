using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ObjectifFlash;
using Fred.Entities.ObjectifFlash.Search;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Fred.DataAccess.ObjectifFlash
{
    /// <summary>
    ///   Représente un référentiel de données pour les Objectifs flash.
    /// </summary>
    public class ObjectifFlashRepository : FredRepository<ObjectifFlashEnt>, IObjectifFlashRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ObjectifFlashRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public ObjectifFlashRepository(FredDbContext context)
          : base(context)
        {
        }
        #region ObjectifFlash

        /// <inheritdoc />
        public ICollection<ObjectifFlashEnt> GetObjectifFlashListNotClosedForRapport(int ciId, DateTime dateChantier)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                Context.Filter<ObjectifFlashEnt>(d => d.Where(c => !c.DateCloture.HasValue));
                Context.Filter<ObjectifFlashEnt>(d => d.Where(c => c.CiId == ciId));
                Context.Filter<ObjectifFlashEnt>(d => d.Where(c => !c.DateSuppression.HasValue));
                Context.Filter<ObjectifFlashEnt>(d => d.Where(c => c.DateDebut <= dateChantier && c.DateFin >= dateChantier));

                return Context.ObjectifFlash
                  .IncludeFilter(c => c.Taches.Select(l => l.Tache))
                  .IncludeFilter(c => c.Taches.Select(u => u.Unite))
                  .AsNoTracking()
                  .ToList();
            }
        }

        /// <summary>
        /// Get Objectif flash by Id sans les tache
        /// </summary>
        /// <param name="objectifFlashId">Id d'objectif flash</param>
        /// <returns>ObjectifFlashEnt</returns>
        public ObjectifFlashEnt GetObjectifFlashById(int objectifFlashId)
        {
            return Query().Get().SingleOrDefault(x => x.ObjectifFlashId == objectifFlashId);
        }

        /// <summary>
        /// Get Objectif flash by Id avec les tache/ressources/journalisations
        /// </summary>
        /// <param name="objectifFlashId">Id d'objectif flash</param>
        /// <returns>ObjectifFlashEnt</returns>
        public ObjectifFlashEnt GetObjectifFlashWithTachesById(int objectifFlashId)
        {
            return this.GetObjectifFlashWithTachesQuery()
                    .FirstOrDefault(c => c.ObjectifFlashId.Equals(objectifFlashId));
        }

        /// <summary>
        /// Get Objectif flash by Id avec les tache/ressources/journalisations sans le tracking
        /// </summary>
        /// <param name="objectifFlashId">Id d'objectif flash</param>
        /// <returns>ObjectifFlashEnt</returns>
        public ObjectifFlashEnt GetObjectifFlashWithTachesByIdNoTracking(int objectifFlashId)
        {
            return this.GetObjectifFlashWithTachesQuery()
                     .AsNoTracking()
                    .FirstOrDefault(c => c.ObjectifFlashId.Equals(objectifFlashId));
        }

        /// <summary>
        /// Retourne la requete de selection des objectifs flash avec tache/ressources/journalisations
        /// </summary>
        /// <returns>Queryable ObjectifFlashEnt</returns>
        private IQueryable<ObjectifFlashEnt> GetObjectifFlashWithTachesQuery()
        {
            return this.Query()
                     .Include(o => o.Ci)
                     .Include(o => o.Ci.Organisation)
                     .Include(o => o.Taches.Select(t => t.Tache))
                     .Include(o => o.Taches.Select(t => t.Unite))
                     .Include(o => o.Taches.Select(t => t.Ressources))
                     .Include(o => o.Taches.Select(t => t.TacheJournalisations))
                     .Include(o => o.Taches.Select(t => t.Ressources.Select(r => r.Ressource.SousChapitre.Chapitre)))
                     .Include(o => o.Taches.Select(t => t.Ressources.Select(r => r.Unite)))
                     .Include(o => o.Taches.Select(t => t.Ressources.Select(r => r.TacheRessourceJournalisations)))
                     .Get();
        }

        /// <summary>
        /// Retourne l'objectif flash portant l'identifiant unique indiqué avec taches et realisations.
        /// </summary>
        /// <param name="objectifFlashId">Identifiant de l'objectif flash à retrouver.</param>
        /// <returns>L'objectif flash retrouvée, sinon null.</returns>
        public ObjectifFlashEnt GetObjectifFlashWithRealisationsById(int objectifFlashId)
        {
            return this.Query()
                    .Include(o => o.Ci)
                    .Include(o => o.Ci.Organisation)
                    .Include(o => o.Taches.Select(t => t.Tache))
                    .Include(o => o.Taches.Select(t => t.TacheRealisations))
                    .Include(o => o.Taches.Select(t => t.Unite))
                    .Include(o => o.Taches.Select(t => t.Ressources))
                    .Include(o => o.Taches.Select(t => t.TacheJournalisations))
                    .Include(o => o.Taches.Select(t => t.Ressources.Select(r => r.Ressource)))
                    .Include(o => o.Taches.Select(t => t.Ressources.Select(r => r.Unite)))
                    .Include(o => o.Taches.Select(t => t.Ressources.Select(r => r.TacheRessourceJournalisations)))
                    .Get()
                    .FirstOrDefault(c => c.ObjectifFlashId.Equals(objectifFlashId));
        }


        /// <summary>
        /// Retourne une liste d'objectif flash paginée
        /// </summary>
        /// <param name="filter">filtre</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">nombre d'items par page</param>
        /// <returns>Liste d'ObjectifFlashEnt</returns>
        public SearchObjectifFlashListWithFilterResult SearchObjectifFlashListWithFilter(SearchObjectifFlashEnt filter, int page = 1, int pageSize = 20)
        {
            var result = new SearchObjectifFlashListWithFilterResult();
            int totalCount;
            var query = Query()
                        .Include(x => x.Ci)
                        .Include(x => x.Taches)
                        .Filter(c => !c.DateSuppression.HasValue);

            if (filter.CI != null)
            {
                query.Filter(c => c.CiId == filter.CI.CiId);
            }
            else
            {
                query.Filter(c => filter.UserCiIds.Contains(c.CiId ?? 0));
            }
            if (filter.DateDebut.HasValue)
            {
                filter.DateDebut = filter.DateDebut.Value.Date;
                query.Filter(c => c.DateDebut >= filter.DateDebut.Value);
            }
            if (filter.DateFin.HasValue)
            {
                filter.DateFin = filter.DateFin.Value.Date;
                query.Filter(c => c.DateFin <= filter.DateFin.Value);
            }
            if (!filter.DisplayClosed)
            {
                query.Filter(c => !c.DateCloture.HasValue);
            }
            var items = query.OrderBy(q => q.OrderBy(of => of.Ci.Code)
                                            .ThenByDescending(of => of.DateDebut))
                             .GetPage(page, pageSize, out totalCount)
                             .ToList();

            items.ForEach(x => x.IsClosed = x.DateCloture.HasValue);

            result.Items = items;
            result.TotalCount = totalCount;
            return result;
        }

        /// <summary>
        /// Ajout d'un Objectif flash
        /// </summary>
        /// <param name="objectifFlash">objectifFlash</param>
        public void AddObjectifFlash(ObjectifFlashEnt objectifFlash)
        {
            objectifFlash.CleanProperties();
            Insert(objectifFlash);
        }

        /// <summary>
        /// Update d'un Objectif flash
        /// </summary>
        /// <param name="objectifFlash">objectifFlash</param>
        /// <param name="objectifFlashInDb">objectifFlash en base</param>
        public void UpdateObjectifFlash(ObjectifFlashEnt objectifFlash, ObjectifFlashEnt objectifFlashInDb)
        {
            // clean des navigation properties avant update
            objectifFlash.CleanProperties();

            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    objectifFlash.DateDebut = objectifFlash.DateDebut.Date;
                    objectifFlash.DateFin = objectifFlash.DateFin.Date;
                    objectifFlash.TotalMontantObjectif = objectifFlash.Taches.SelectMany(t => t.Ressources).Sum(r => (r.QuantiteObjectif ?? 0) * (r.PuHT ?? 0));
                    Context.Entry(objectifFlashInDb).CurrentValues.SetValues(objectifFlash);
                    // pas d'update des taches pour un objectif flash actif ou cloturé
                    if (objectifFlash.IsActif || objectifFlash.IsClosed)
                    {
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }

                    // mise à jour des taches/ressources/journalisations
                    objectifFlash.Taches.ToList().ForEach(
                        x =>
                        {
                            x.ObjectifFlashId = objectifFlashInDb.ObjectifFlashId;
                            x.UniteId = x.UniteId != 0 ? x.UniteId : null;
                        });
                    this.UpdateObjectifFlashTacheList(objectifFlashInDb.Taches, objectifFlash.Taches);
                    Context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region methodes privées


        private void UpdateObjectifFlashTacheList(IEnumerable<ObjectifFlashTacheEnt> objectifFlashTachesInDb, IEnumerable<ObjectifFlashTacheEnt> objectifFlashTaches)
        {
            var tachesToAdd = objectifFlashTaches.Where(x => x.ObjectifFlashTacheId == 0).ToList();
            var tachesToDelete = objectifFlashTachesInDb.Where(x => !objectifFlashTaches.Any(y => x.ObjectifFlashTacheId == y.ObjectifFlashTacheId)).ToList();
            var tachesToUpdate = objectifFlashTachesInDb.Where(x => objectifFlashTaches.Any(y => x.ObjectifFlashTacheId == y.ObjectifFlashTacheId) && x.ObjectifFlashTacheId != 0).ToList();

            // Suppression des taches d'objectif flash existantes
            foreach (var tacheToDelete in tachesToDelete)
            {
                if (tacheToDelete.Ressources != null && tacheToDelete.Ressources.Any())
                {
                    Context.ObjectifFlashTacheRessourceJournalisation.RemoveRange(tacheToDelete.Ressources.SelectMany(x => x.TacheRessourceJournalisations));
                    Context.ObjectifFlashTacheRessource.RemoveRange(tacheToDelete.Ressources);
                    Context.ObjectifFlashTacheJournalisation.RemoveRange(tacheToDelete.TacheJournalisations);
                }
            }
            Context.ObjectifFlashTache.RemoveRange(tachesToDelete);

            // Ajout des taches d'objectif flash
            Context.ObjectifFlashTache.AddRange(tachesToAdd);

            // Update des taches d'objectif flash
            foreach (var tacheToUpdate in tachesToUpdate)
            {
                var tache = objectifFlashTaches.SingleOrDefault(x => x.ObjectifFlashTacheId == tacheToUpdate.ObjectifFlashTacheId);
                tacheToUpdate.UniteId = tache.UniteId == 0 ? null : tache.UniteId;
                Context.Entry(tacheToUpdate).CurrentValues.SetValues(tache);
                UpdateObjectifFlashTacheRessourceList(tacheToUpdate, tache);
                UpdateObjectifFlashTacheJournalisationList(tacheToUpdate, tache);
            }
        }

        private void UpdateObjectifFlashTacheRessourceList(ObjectifFlashTacheEnt objectifFlashTacheInDb, ObjectifFlashTacheEnt objectifFlashTache)
        {
            var objectifFlashTacheRessources = objectifFlashTache.Ressources ?? new List<ObjectifFlashTacheRessourceEnt>();
            var ressourcesToAdd = objectifFlashTache.Ressources.Where(x => x.ObjectifFlashTacheRessourceId == 0).ToList();
            var ressourcesToDelete = objectifFlashTacheInDb.Ressources.Where(x => !objectifFlashTacheRessources.Any(y => x.ObjectifFlashTacheRessourceId == y.ObjectifFlashTacheRessourceId)).ToList();
            var ressourcesToUpdate = objectifFlashTacheInDb.Ressources.Where(x => objectifFlashTacheRessources.Any(y => x.ObjectifFlashTacheRessourceId == y.ObjectifFlashTacheRessourceId) && x.ObjectifFlashTacheRessourceId != 0).ToList();

            // Suppression des ressources de taches d'objectif flash 
            Context.ObjectifFlashTacheRessource.RemoveRange(ressourcesToDelete);

            // Ajout des ressources de taches d'objectif flash
            ressourcesToAdd.ToList().ForEach(x => x.ObjectifFlashTacheId = objectifFlashTacheInDb.ObjectifFlashTacheId);
            Context.ObjectifFlashTacheRessource.AddRange(ressourcesToAdd);

            // Update des taches d'objectif flash
            foreach (var ressourceToUpdate in ressourcesToUpdate)
            {
                var objectifFlashTacheRessource = objectifFlashTacheRessources.SingleOrDefault(x => x.ObjectifFlashTacheRessourceId == ressourceToUpdate.ObjectifFlashTacheRessourceId);
                ressourceToUpdate.UniteId = objectifFlashTacheRessource.UniteId == 0 ? null : objectifFlashTacheRessource.UniteId;
                Context.Entry(ressourceToUpdate).CurrentValues.SetValues(objectifFlashTacheRessource);
                UpdateObjectifFlashTacheRessourceJournalisationList(ressourceToUpdate, objectifFlashTacheRessource);
            }
        }

        private void UpdateObjectifFlashTacheJournalisationList(ObjectifFlashTacheEnt objectifFlashTacheInDb, ObjectifFlashTacheEnt objectifFlashTache)
        {
            var objectifFlashTacheJournalisations = objectifFlashTache.TacheJournalisations ?? new List<ObjectifFlashTacheJournalisationEnt>();
            var objectifFlashTacheJournalisationsInDb = objectifFlashTacheInDb.TacheJournalisations ?? new List<ObjectifFlashTacheJournalisationEnt>();
            var tacheJournalisationsToAdd = objectifFlashTacheJournalisations.Where(x => x.ObjectifFlashTacheJournalisationId == 0).ToList();
            var tacheJournalisationsToDelete = objectifFlashTacheJournalisationsInDb.Where(x => !objectifFlashTacheJournalisations.Any(y => x.ObjectifFlashTacheJournalisationId == y.ObjectifFlashTacheJournalisationId)).ToList();
            var tacheJournalisationsToUpdate = objectifFlashTacheJournalisationsInDb.Where(x => objectifFlashTacheJournalisations.Any(y => x.ObjectifFlashTacheJournalisationId == y.ObjectifFlashTacheJournalisationId) && x.ObjectifFlashTacheJournalisationId != 0).ToList();

            // Suppression des ressources de taches d'objectif flash 
            Context.ObjectifFlashTacheJournalisation.RemoveRange(tacheJournalisationsToDelete);

            // Ajout des ressources de taches d'objectif flash
            tacheJournalisationsToAdd.ToList().ForEach(x => x.ObjectifFlashTacheId = objectifFlashTacheInDb.ObjectifFlashTacheId);
            Context.ObjectifFlashTacheJournalisation.AddRange(tacheJournalisationsToAdd);

            // Update des journalisations de tache d'objectif flash
            foreach (var tacheJournalisationToUpdate in tacheJournalisationsToUpdate)
            {
                var objectifFlashTacheRessource = objectifFlashTacheJournalisations.SingleOrDefault(x => x.ObjectifFlashTacheJournalisationId == tacheJournalisationToUpdate.ObjectifFlashTacheJournalisationId);
                if (tacheJournalisationToUpdate.QuantiteObjectif != objectifFlashTacheRessource.QuantiteObjectif)
                {
                    Context.Entry(tacheJournalisationToUpdate).CurrentValues.SetValues(objectifFlashTacheRessource);
                }
            }
        }

        private void UpdateObjectifFlashTacheRessourceJournalisationList(ObjectifFlashTacheRessourceEnt objectifFlashTacheRessourceInDb, ObjectifFlashTacheRessourceEnt objectifFlashTacheRessource)
        {
            var objectifFlashTacheRessourceJournalisations = objectifFlashTacheRessource.TacheRessourceJournalisations ?? new List<ObjectifFlashTacheRessourceJournalisationEnt>();
            var objectifFlashTacheRessourceJournalisationsInDb = objectifFlashTacheRessourceInDb.TacheRessourceJournalisations ?? new List<ObjectifFlashTacheRessourceJournalisationEnt>();
            var tacheRessourceJournalisationsToAdd = objectifFlashTacheRessourceJournalisations.Where(x => x.ObjectifFlashTacheRessourceJournalisationId == 0);
            var tacheRessourceJournalisationsToDelete = objectifFlashTacheRessourceJournalisationsInDb.Where(x => !objectifFlashTacheRessourceJournalisations.Any(y => x.ObjectifFlashTacheRessourceJournalisationId == y.ObjectifFlashTacheRessourceJournalisationId));
            var tacheRessourceJournalisationsToUpdate = objectifFlashTacheRessourceJournalisationsInDb.Where(x => objectifFlashTacheRessourceJournalisations.Any(y => x.ObjectifFlashTacheRessourceJournalisationId == y.ObjectifFlashTacheRessourceJournalisationId) && x.ObjectifFlashTacheRessourceJournalisationId != 0);

            // Suppression des ressources de taches d'objectif flash 
            Context.ObjectifFlashTacheRessourceJournalisation.RemoveRange(tacheRessourceJournalisationsToDelete);

            // Ajout des ressources de taches d'objectif flash
            Context.ObjectifFlashTacheRessourceJournalisation.AddRange(tacheRessourceJournalisationsToAdd);
            // Update des journalisations de ressource d'objectif flash
            foreach (var tacheRessourceJournalisationToUpdate in tacheRessourceJournalisationsToUpdate)
            {
                var journalisation = objectifFlashTacheRessourceJournalisations.SingleOrDefault(x => x.ObjectifFlashTacheRessourceJournalisationId == tacheRessourceJournalisationToUpdate.ObjectifFlashTacheRessourceJournalisationId);
                if (journalisation.QuantiteObjectif != tacheRessourceJournalisationToUpdate.QuantiteObjectif)
                {
                    Context.Entry(tacheRessourceJournalisationToUpdate).CurrentValues.SetValues(journalisation);
                }
            }
        }
        #endregion methodes privées
    }
}
