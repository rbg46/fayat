using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Définition d'une ligne de rapport sur une prime
    /// </summary>
    public interface IRapportTacheRepository : IRepository<RapportTacheEnt>
    {
        /// <summary>
        ///   Récupère l'entité RapportTache correspondant au identifiants passés en paramètre
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <param name="tacheId">Identifiant de la tache</param>
        /// <returns>Retourne l'entité RapportTache correspondant au identifiants passés en paramètre</returns>
        RapportTacheEnt GetByRapportIdAndTacheId(int rapportId, int tacheId);

        /// <summary>
        /// Retourne les rapports taches a partir d'une liste de rapportsId
        /// </summary>
        /// <param name="rapportIds">Liste de ></param>
        /// <returns>Retourne les rapports taches</returns>
        IEnumerable<RapportTacheEnt> GetRapportTachesByRapportIds(List<int> rapportIds);

        /// <summary>
        /// Retourne les commentaires des rapports taches passé en parametre et du rapportid et de la tacheId
        /// </summary>
        /// <param name="rapportTaches">rapportTaches</param>
        /// <param name="rapportId">rapportId</param>
        /// <param name="tacheId">tacheId</param>
        /// <returns>Retourne les commentaires des rapports taches</returns>
        string GetCommentairesByRapportIdAndTacheId(IEnumerable<RapportTacheEnt> rapportTaches, int rapportId, int tacheId);
    }
}
