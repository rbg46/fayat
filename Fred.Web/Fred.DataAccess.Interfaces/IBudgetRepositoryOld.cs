
using System.Collections.Generic;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données pour les Budgets.
    /// </summary>
    public interface IBudgetRepositoryOld : IFredRepository<BudgetEnt>
    {
        /// <summary>
        /// Permet de récupérer la liste des tâches pour une révision d'un budget
        /// </summary>
        /// <param name="revisionId">Identifiant de la révision</param>
        /// <returns>Liste de tâche de niveau 1 avec leurs enfants</returns>
        ICollection<TacheEnt> GetBudgetRevisionTaches(int revisionId);


        /// <summary>
        /// Permet de récupérer la liste des tâches de niveau 4 pour une révision d'un budget sans include
        /// </summary>
        /// <param name="revisionId">Identifiant de la révision</param>
        /// <returns>Liste de tâche de niveau 4 avec leurs enfants sans les ressources</returns>
        ICollection<TacheEnt> GetBudgetRevisionTachesLevel4(int revisionId);

        /// <summary>
        /// Permet de récupérer une tâche avec la liste des ressources associés jusqu'aux devises
        /// </summary>
        /// <param name="tacheId">Identifiant de la tâche</param>
        /// <returns>tâche</returns>
        TacheEnt GetTacheWithRessourceTaches(int tacheId);

        /// <summary>
        /// Permet de récupoérer un budget avec ses révision pour un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>BudgetEnt</returns>
        BudgetEnt GetBudget(int ciId);

        /// <summary>
        /// Permet de sauvegarder un détail de ressource
        /// </summary>
        /// <param name="ressource">ressource à insérer</param>
        /// <returns>ressource insérée</returns>
        RessourceEnt AddRessource(RessourceEnt ressource);
    }
}