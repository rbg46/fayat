
using Fred.Entities.Depense;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Représente un référentiel de données pour les lot de remplacements de taches
    /// </summary>
    public interface IRemplacementTacheRepository : IRepository<RemplacementTacheEnt>
    {
        /// <summary>
        /// Ajout d'une tâche de remplacement
        /// </summary>
        /// <param name="remplacementT">La tache de remplacement</param>
        /// <returns>La liste des Taches de remplacement.</returns>
        RemplacementTacheEnt AddRemplacementTache(RemplacementTacheEnt remplacementT);

        /// <summary>
        ///   Supprime une dépense logiquement en base (DateSuppression renseignée)
        /// </summary>
        /// <param name="remplacementTacheId">L'identifiant du dépense à supprimer</param>
        /// <returns>L'id du groupe à supprimer si besoin</returns>
        int DeleteRemplacementTacheById(int remplacementTacheId);

        /// <summary>
        /// Retourne la taches.
        /// </summary>
        /// <param name="remplacementTacheId">Identifiant de la tâche</param>
        /// <returns>La tache de remplacement</returns>
        RemplacementTacheEnt GetRemplacementTacheById(int remplacementTacheId);

        /// <summary>
        ///   Retourne la liste des taches.
        /// </summary>
        /// <returns>La liste des dépenses.</returns>
        IEnumerable<RemplacementTacheEnt> GetRemplacementTacheList();

        /// <summary>
        /// Renvoie la liste des tâches remplacées associées à un groupe de remplacements
        /// </summary>
        /// <param name="groupId">Identifiant du groupe de tâches</param>
        /// <returns>Liste des tâches associées au groupe</returns>
        IEnumerable<RemplacementTacheEnt> GetRemplacementTachesListByGroupId(int groupId);

        /// <summary>
        /// Mise à jour d'une tâche de remplacement
        /// </summary>
        /// <param name="remplacementT">La tache de remplacement</param>
        /// <returns>La liste des Taches de remplacement.</returns>
        RemplacementTacheEnt UpdateRemplacementTache(RemplacementTacheEnt remplacementT);

        RemplacementTacheEnt GetLast(int groupeRemplacementTacheId, DateTime? periodeFin);

        Task<IReadOnlyList<RemplacementTacheEnt>> GetLastAsync(IEnumerable<int> groupeRemplacementTacheIds, DateTime? periodeFin);
        List<RemplacementTacheEnt> GetRemplacementTachesListByGroupeRemplacementTacheIds(List<int> groupRemplacementIds);
        Task<List<RemplacementTacheOriginInformationModel>> GetRemplacementTacheOrigineAsync(List<int> groupRemplacementIds);
    }
}
