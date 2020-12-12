using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Depense;
using Fred.Web.Models.Depense;

namespace Fred.Business.Depense
{
    /// <summary>
    ///   Gestionnaire des dépenses.
    /// </summary>
    public interface IRemplacementTacheManager : IManager<RemplacementTacheEnt>
    {
        Task AddAsync(RemplacementTacheModel tache);

        Task DeleteByIdAsync(int remplacementTacheId);

        /// <summary>
        ///   Retourne la tache de remplacement l'identifiant unique indiqué.
        /// </summary>
        /// <param name="remplacementTacheId">Identifiant de la tache à retrouver.</param>
        /// <returns>La tache retrouvée, sinon nulle.</returns>
        RemplacementTacheEnt GetById(int remplacementTacheId);

        /// <summary>
        ///   Retourne la liste de taches associées à l'identifiant unique indiqué.
        /// </summary>
        /// <param name="groupId">Identifiant du groupe à retrouver.</param>
        /// <returns>La tache retrouvée, sinon nulle.</returns>
        IEnumerable<RemplacementTacheEnt> GetListByGroupId(int groupId);

        /// <summary>
        /// Retourne la liste de taches de remplacements associées à l'identifiant d'un groupe de remplacements.
        /// </summary>
        /// <param name="groupeTacheId">Identifiant de la tâche</param>
        /// <param name="avecTacheOrigine">Ajout de la tâche d'origine ou non</param>
        /// <returns>Liste de tâches de remplacement</returns>
        Task<IEnumerable<RemplacementTacheEnt>> GetHistoryAsync(int groupeTacheId, bool avecTacheOrigine = true);

        /// <summary>
        ///   Retourne la dernière tâche remplacée à partir de l'identifiant du groupe de remplacement de tâche
        /// </summary>
        /// <param name="groupeRemplacementTacheId">Identifiant du groupe remplacement tâche</param>
        /// <param name="periodeFin">Date fin de période comptable</param>
        /// <returns>Dernier remplacement de Tâche</returns>
        RemplacementTacheEnt GetLast(int groupeRemplacementTacheId, DateTime? periodeFin = null);

        Task<IReadOnlyList<RemplacementTacheEnt>> GetLastAsync(IEnumerable<int> groupeRemplacementTacheIds, DateTime? periodeFin = null);

        /// <summary>
        ///   Mise à jour d'un remplacementTache
        /// </summary>
        /// <param name="rt">Remplacement Tache</param>
        /// <returns>RemplacementTache mis à jour</returns>
        RemplacementTacheEnt Update(RemplacementTacheEnt rt);
    }
}
