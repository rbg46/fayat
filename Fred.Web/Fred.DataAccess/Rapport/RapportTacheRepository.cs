using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Rapport.Pointage;
using Fred.Entities.Rapport;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Rapport
{
    /// <summary>
    ///   Référentiel de données pour les lignes de prime.
    /// </summary>
    public class RapportTacheRepository : FredRepository<RapportTacheEnt>, IRapportTacheRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="RapportLignePrimeRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public RapportTacheRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        ///   Récupère l'entité RapportTache correspondant au identifiants passés en paramètre
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <param name="tacheId">Identifiant de la tache</param>
        /// <returns>Retourne l'entité RapportTache correspondant au identifiants passés en paramètre</returns>
        public RapportTacheEnt GetByRapportIdAndTacheId(int rapportId, int tacheId)
        {
            return Context.RapportTache.AsNoTracking().FirstOrDefault(rt => rt.RapportId == rapportId && rt.TacheId == tacheId);
        }

        /// <summary>
        /// Retourne les rapports taches a partir d'une liste de rapportsId
        /// </summary>
        /// <param name="rapportIds">Liste de &gt;</param>
        /// <returns>Retourne les rapports taches</returns>
        public IEnumerable<RapportTacheEnt> GetRapportTachesByRapportIds(List<int> rapportIds)
        {
            return Context.RapportTache.Where(rt => rapportIds.Contains(rt.RapportId)).AsNoTracking().ToList();
        }

        /// <summary>
        /// Retourne les commentaires des rapports taches passé en parametre et du rapportid et de la tacheId
        /// </summary>
        /// <param name="rapportTaches">rapportTaches</param>
        /// <param name="rapportId">rapportId</param>
        /// <param name="tacheId">tacheId</param>
        /// <returns>Retourne les commentaires des rapports taches</returns>
        public string GetCommentairesByRapportIdAndTacheId(IEnumerable<RapportTacheEnt> rapportTaches, int rapportId, int tacheId)
        {
            return rapportTaches.Where(rt => rt.RapportId == rapportId && rt.TacheId == tacheId).Select(rt => rt.Commentaire).FirstOrDefault() ?? string.Empty;
        }

    }
}
