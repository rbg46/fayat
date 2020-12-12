using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Avancement;

namespace Fred.Business.Budget.Avancement
{
    /// <summary>
    /// Gestionnaire des taches d'avancement
    /// </summary>
    public class AvancementTacheManager : Manager<AvancementTacheEnt, IAvancementTacheRepository>, IAvancementTacheManager
    {
        public AvancementTacheManager(IUnitOfWork uow, IAvancementTacheRepository avancementTacheRepository)
         : base(uow, avancementTacheRepository)
        {
        }

        /// <summary>
        ///   Obtient la liste des taches d'avancement par budget et periode
        /// </summary>
        /// <param name="budgetId">identifiant de budget</param>
        /// <param name="periode">période</param>
        /// <returns>Liste des taches d'avancement</returns>
        public IEnumerable<AvancementTacheEnt> GetAvancementTaches(int budgetId, int periode)
        {
            return Repository.GetAvancementTaches(budgetId, periode);
        }

        /// <summary>
        /// Update une liste de tache pour un budget et une période donnée
        /// </summary>
        /// <param name="budgetId">Identifiant de budget</param>
        /// <param name="periode">Identifiant de période</param>
        /// <param name="avancementTacheEnts">liste des taches</param>
        public void UpdateListe(int budgetId, int periode, IEnumerable<AvancementTacheEnt> avancementTacheEnts)
        {
            Repository.UpdateListe(budgetId, periode, avancementTacheEnts);
        }
    }
}
