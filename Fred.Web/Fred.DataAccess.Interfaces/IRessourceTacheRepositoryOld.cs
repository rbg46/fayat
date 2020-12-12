
using System.Collections.Generic;
using Fred.Entities.Budget;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Gestion du ressources tâches
    /// </summary>
    public interface IRessourceTacheRepositoryOld : IFredRepository<RessourceTacheEnt>
    {
        /// <summary>
        /// Permet de mettre à jour les ressources tâches d'une tâche.
        /// </summary>
        /// <param name="tacheId">L'identifiant de la tâche.</param>
        /// <param name="ressourceTaches">La liste des ressources tâches.</param>
        /// <returns>La liste des ressources tâches à jour.</returns>
        ICollection<RessourceTacheEnt> UpdateRessourceTaches(int tacheId, List<RessourceTacheEnt> ressourceTaches);
    }
}
