using System.Collections.Generic;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;

namespace Fred.DataAccess.RepriseDonnees.PlanTaches
{
    /// <summary>
    /// Repository des Reprises des données d'un Plan de tâches
    /// </summary>
    public class ReprisePlanTachesRepository : IReprisePlanTachesRepository
    {
        private readonly FredDbContext fredDbContext;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>       
        public ReprisePlanTachesRepository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            this.fredDbContext = (UnitOfWork as UnitOfWork).Context;
            //// this.fredDbContext.Database.Log = s => Debug.WriteLine(s) 

        }

        /// <summary>
        /// UnitOfWork
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Creation du Plan de tâches
        /// </summary>
        /// <param name="planTachesEnts">Le Plan de tâches a créer</param>
        public void CreatePlanTaches(List<TacheEnt> planTachesEnts)
        {
            if (planTachesEnts.Count > 0)
            {
                this.fredDbContext.Taches.AddRange(planTachesEnts);
            }
        }

        /// <summary>
        /// Mise a jour des taches par défaut en mettant uniquement à jour cette propriété (ParDefaut)
        /// </summary>
        /// <param name="tachesParDefaut">Taches à mettre a jours</param>
        public void UpdateTachesParDefaut(List<TacheEnt> tachesParDefaut)
        {
            foreach (TacheEnt tache in tachesParDefaut)
            {
                this.fredDbContext.Taches.Attach(tache);
                this.fredDbContext.Entry(tache).Property(x => x.TacheParDefaut).IsModified = true;
                this.fredDbContext.Entry(tache).Property(x => x.DateModification).IsModified = true;
            }
        }
    }
}
