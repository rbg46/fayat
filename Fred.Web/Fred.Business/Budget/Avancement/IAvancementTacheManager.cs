using System.Collections.Generic;
using Fred.Entities.Budget.Avancement;

namespace Fred.Business.Budget.Avancement
{
    /// <summary>
    /// Interface des taches d'avancements.
    /// </summary>
    public interface IAvancementTacheManager
    {
        /// <summary>
        /// Retourne la liste des taches d'avancement pour un budget et une période
        /// </summary>
        /// <param name="budgetId">identifiant de budget</param>
        /// <param name="periode">période</param>
        /// <returns>liste des taches d'avancement</returns>
        IEnumerable<AvancementTacheEnt> GetAvancementTaches(int budgetId, int periode);

        /// <summary>
        /// Mise à jour d'une liste de taches d'avancements pour un budget et une période
        /// </summary>
        /// <param name="budgetId">Identifiant de budget</param>
        /// <param name="periode">Identifiant de période</param>
        /// <param name="avancementTacheEnts">liste des taches</param>
        void UpdateListe(int budgetId, int periode, IEnumerable<AvancementTacheEnt> avancementTacheEnts);
    }
}
