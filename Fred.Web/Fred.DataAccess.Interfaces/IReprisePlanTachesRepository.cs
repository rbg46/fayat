using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Repository des Reprises des données d'un Plan de tâches
    /// </summary>
    public interface IReprisePlanTachesRepository : IMultipleRepository
    {/// <summary>
     /// UnitOfWork
     /// </summary>
        IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Creation du Plan de tâches
        /// </summary>
        /// <param name="planTachesEnts">Le Plan de tâches a créer</param>
        void CreatePlanTaches(List<TacheEnt> planTachesEnts);

        /// <summary>
        /// Mise a jour des taches par défaut en mettant uniquement à jour cette propriété (ParDefaut)
        /// </summary>
        /// <param name="tachesParDefaut">Taches à mettre a jours</param>
        void UpdateTachesParDefaut(List<TacheEnt> tachesParDefaut);
    }
}
