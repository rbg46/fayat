using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;

namespace Fred.Business.RapportTache
{
    /// <summary>
    /// Manager des rapportTacheEnt
    /// </summary>
    public class RapportTacheManager : Manager<RapportTacheEnt, IRapportTacheRepository>, IRapportTacheManager
    {
        public RapportTacheManager(IUnitOfWork uow, IRapportTacheRepository rapportTacheRepository)
            : base(uow, rapportTacheRepository)
        {
        }

        /// <summary>
        /// Retourne les rapports taches a partir d'une liste de rapportsId
        /// </summary>
        /// <param name="rapportIds">Liste de &gt;</param>
        /// <returns>Retourne les rapports taches</returns>
        public IEnumerable<RapportTacheEnt> GetRapportTachesByRapportIds(List<int> rapportIds)
        {
            return this.Repository.GetRapportTachesByRapportIds(rapportIds);
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
            return this.Repository.GetCommentairesByRapportIdAndTacheId(rapportTaches, rapportId, tacheId);
        }
    }
}
