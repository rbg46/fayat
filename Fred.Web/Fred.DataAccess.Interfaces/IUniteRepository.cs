using System.Collections.Generic;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///  Représente un référentiel de données pour les devises.
    /// </summary>
    public interface IUniteRepository : IRepository<UniteEnt>
    {
        /// <summary>
        /// Retourne une liste d'unité à partir d'une liste d'identifiant
        /// </summary>
        /// <param name="unitesIds">Liste d'identifiant d'unités</param>
        /// <returns>Liste d'unite</returns>
        IReadOnlyList<UniteEnt> Get(List<int> unitesIds);

        /// <summary>
        /// Retourne une liste d'unité à partir d'une liste d'identifiant
        /// </summary>
        /// <param name="unitesCodes">Liste des codes d'unités</param>
        /// <returns>Liste d'unite</returns>
        IReadOnlyList<UniteEnt> Get(List<string> unitesCodes);

        /// <summary>
        /// Retourne les unités pour la comparaison de budget.
        /// </summary>
        /// <param name="uniteIds">Les identifiants des unités concernées.</param>
        /// <returns>Les unités.</returns>
        List<UniteDao> GetUnitesPourBudgetComparaison(IEnumerable<int> uniteIds);

    }
}
