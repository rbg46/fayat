
using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les lignes des rapports de tâches.
    /// </summary>
    public interface IRapportLigneTacheRepository : IRepository<RapportLigneTacheEnt>
    {
        /// <summary>
        /// Sauvegarde les modifications apportée à un rapport ligne Tache
        /// </summary>
        /// <param name="rapportLigneTache">List Rapport Ligne Tache à modifier</param>
        void UpdateRangeRapportLigneTache(IEnumerable<RapportLigneTacheEnt> rapportLigneTache);
    }
}
