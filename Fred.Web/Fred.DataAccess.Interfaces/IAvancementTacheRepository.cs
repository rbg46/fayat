using System.Collections.Generic;
using Fred.Entities.Budget.Avancement;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données pour les taches d'avancements.
    /// </summary>
    public interface IAvancementTacheRepository : IFredRepository<AvancementTacheEnt>
    {
        /// <summary>
        ///   Obtient la liste des taches d'avancement par budget et periode
        /// </summary>
        /// <param name="budgetId">ientifiant de budget</param>
        /// <param name="periode">période</param>
        /// <returns>Liste des taches d'avancement</returns>
        IEnumerable<AvancementTacheEnt> GetAvancementTaches(int budgetId, int periode);

        /// <summary>
        ///   Met a jour la liste des taches d'avancement pour budget et une periode donnée
        /// </summary>
        /// <param name="budgetId">ientifiant de budget</param>
        /// <param name="periode">période</param>
        /// <param name="avancementTacheEnts">liste des taches</param>
        void UpdateListe(int budgetId, int periode, IEnumerable<AvancementTacheEnt> avancementTacheEnts);
    }
}
