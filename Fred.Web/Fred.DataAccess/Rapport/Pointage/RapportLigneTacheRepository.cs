using System.Collections.Generic;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.EntityFramework;

namespace Fred.DataAccess.Rapport.Pointage
{
    /// <summary>
    ///   Référentiel de données pour les lignes de tâche.
    /// </summary>
    public class RapportLigneTacheRepository : FredRepository<RapportLigneTacheEnt>, IRapportLigneTacheRepository
    {
        public RapportLigneTacheRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Sauvegarde les modifications apportée à un rapport ligne Tache
        /// </summary>
        /// <param name="rapportLigneTache">List Rapport Ligne Tache à modifier</param>
        public void UpdateRangeRapportLigneTache(IEnumerable<RapportLigneTacheEnt> rapportLigneTache)
        {
            Context.RapportLigneTaches.UpdateRange(rapportLigneTache);
        }

    }
}
