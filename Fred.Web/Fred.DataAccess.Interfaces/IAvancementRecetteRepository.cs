using System.Collections.Generic;
using Fred.Entities.Budget.Recette;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Acces aux données des recettes
    /// </summary>
    public interface IAvancementRecetteRepository : IFredRepository<AvancementRecetteEnt>
    {
        /// <summary>
        /// Retourne l'avancement correspondant à une recette et à la période donnée
        /// </summary>
        /// <param name="budgetRecetteId">Identifiant de la rectte</param>
        /// <param name="periode">Période YYYYMM</param>
        /// <returns>L'avancement correspondant une recette et à la période donnée</returns>
        AvancementRecetteEnt GetByBudgetRecetteIdAndPeriod(int budgetRecetteId, int periode);

        /// <summary>
        /// Retourne l'avancement précédent correspondant à une recette et à la période donnée
        /// </summary>
        /// <param name="budgetRecetteId">Identifiant de la recette</param>
        /// <param name="periode">Période YYYYMM</param>
        /// <returns>L'avancement précédent correspondant à une recette et à la période donnée</returns>
        AvancementRecetteEnt GetPreviousByBudgetRecetteIdAndPeriod(int budgetRecetteId, int periode);

        /// <summary>
        /// Retourne une liste d'avancement correspondant à une liste de recette et une fouchette de période
        /// </summary>
        /// <param name="budgetRecetteIds">Liste des identifiants de recette</param>
        /// <param name="fromperiode">Début Période YYYYMM</param>
        /// <param name="toperiode">Fin Période YYYYMM</param>
        /// <returns>Une liste d'avancement correspondant à une liste de recette et une fouchette de période</returns>
        IEnumerable<AvancementRecetteEnt> GetByBudgetRecetteIdsAndToPeriod(List<int> budgetRecetteIds, int fromperiode, int toperiode);
    }
}
