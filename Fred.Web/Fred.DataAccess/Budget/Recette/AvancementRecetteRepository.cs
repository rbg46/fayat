using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Recette;
using Fred.Framework;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget.Recette
{
    /// <summary>
    /// Implementation de l'interface IAvancementRecetteRepository
    /// </summary>
    public class AvancementRecetteRepository : FredRepository<AvancementRecetteEnt>, IAvancementRecetteRepository
    {
        /// <summary>
        /// Constructeur par défault
        /// </summary>
        /// <param name="logMgr">manager de log</param>
        public AvancementRecetteRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Retourne l'avancement correspondant à une recette et à la période donnée
        /// </summary>
        /// <param name="budgetRecetteId">Identifiant de la rectte</param>
        /// <param name="periode">Période YYYYMM</param>
        /// <returns>L'avancement correspondant une recette et à la période donnée</returns>
        public AvancementRecetteEnt GetByBudgetRecetteIdAndPeriod(int budgetRecetteId, int periode)
        {
            return Context.AvancementRecettes.FirstOrDefault(c => c.BudgetRecetteId == budgetRecetteId && c.Periode == periode);
        }

        /// <summary>
        /// Retourne l'avancement précédent correspondant à une recette et à la période donnée
        /// </summary>
        /// <param name="budgetRecetteId">Identifiant de la recette</param>
        /// <param name="periode">Période YYYYMM</param>
        /// <returns>L'avancement précédent correspondant à une recette et à la période donnée</returns>
        public AvancementRecetteEnt GetPreviousByBudgetRecetteIdAndPeriod(int budgetRecetteId, int periode)
        {
            return Context.AvancementRecettes.Where(c => c.BudgetRecetteId == budgetRecetteId && c.Periode <= periode).OrderByDescending(a => a.Periode).FirstOrDefault();
        }

        /// <summary>
        /// Retourne une liste d'avancement correspondant à une liste de recette et une fouchette de période
        /// </summary>
        /// <param name="budgetRecetteIds">Liste des identifiants de recette</param>
        /// <param name="fromperiode">Début Période YYYYMM</param>
        /// <param name="toperiode">Fin Période YYYYMM</param>
        /// <returns>Une liste d'avancement correspondant à une liste de recette et une fouchette de période</returns>
        public IEnumerable<AvancementRecetteEnt> GetByBudgetRecetteIdsAndToPeriod(List<int> budgetRecetteIds, int fromperiode , int toperiode)
        {
            return Context.AvancementRecettes.Where(c => (budgetRecetteIds.Contains(c.BudgetRecetteId) ) && (c.Periode>=fromperiode && c.Periode <= toperiode)).OrderBy(c=>c.Periode).ToList();
        }
    }
}
