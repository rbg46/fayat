using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Budget
{
    /// <summary>
    /// Référentiel de données pour budget T4.
    /// </summary>
    public class BudgetT4Repository : FredRepository<BudgetT4Ent>, IBudgetT4Repository
    {
        public BudgetT4Repository(FredDbContext context)
          : base(context)
        { }

        /// <summary>
        /// Récupère les budgets T4 d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="loadSousDetails">Indique si on charge les sous-détail d'un budget T4</param>
        /// <returns>Les budgets T4 du budget.</returns>
        public IEnumerable<BudgetT4Ent> GetByBudgetId(int budgetId, bool loadSousDetails = false)
        {
            IQueryable<BudgetT4Ent> query = null;

            if (!loadSousDetails)
            {
                query = Context.BudgetT4
                .Include(b => b.Unite)
                .Include(b => b.T4.Parent)
                .Include(b => b.T3.Parent.Parent)
                .AsNoTracking()
                .Where(b => b.BudgetId == budgetId);
            }
            else
            {
                query = Context.BudgetT4
                .Include(b => b.Unite)
                .Include(b => b.BudgetSousDetails)
                .Include(b => b.T4.Parent.Parent.Parent)
                .Include(b => b.BudgetSousDetails).ThenInclude(sd => sd.Ressource.SousChapitre.Chapitre)
                .Include(b => b.BudgetSousDetails).ThenInclude(sd => sd.Ressource.ReferentielEtendus).ThenInclude(r => r.ParametrageReferentielEtendus).ThenInclude(p => p.Unite)
                .Include(b => b.BudgetSousDetails).ThenInclude(sd => sd.Unite)
                .Include(b => b.T3.Parent.Parent)
                .AsNoTracking()
                .Where(b => b.BudgetId == budgetId);
            }

            foreach (var budgetT4 in query.ToList())
            {
                if (budgetT4.T3 == null)
                {
                    budgetT4.T3 = budgetT4.T4.Parent;
                    budgetT4.T3Id = budgetT4.T3.TacheId;
                }
                yield return budgetT4;
            }
        }

        /// <summary>
        /// Récupère les budgets T4 d'un budget créé avant .
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="date">Date</param>
        /// <param name="loadSousDetails">Indique si on veut charger les sous-détails liés aux budget T4</param>
        /// <returns>Les budgets T4 du budget.</returns>
        public IEnumerable<BudgetT4Ent> GetByBudgetIdAndCreationDate(int budgetId, DateTime? date, bool loadSousDetails = false)
        {
            IQueryable<BudgetT4Ent> query = null;

            if (!loadSousDetails)
            {
                query = Context.BudgetT4
                .Include(b => b.Unite)
                .Include(b => b.T4.Parent)
                .Include(b => b.T3.Parent.Parent)
                .AsNoTracking()
                .Where(b => b.BudgetId == budgetId && (date == null || b.T4.DateCreation < date));
            }
            else
            {
                query = Context.BudgetT4
                .Include(b => b.Unite)
                .Include(b => b.BudgetSousDetails)
                .Include(b => b.T4.Parent.Parent.Parent)
                .Include(b => b.BudgetSousDetails).ThenInclude(sd => sd.Ressource.SousChapitre.Chapitre)
                .Include(b => b.BudgetSousDetails).ThenInclude(sd => sd.Ressource.ReferentielEtendus).ThenInclude(r => r.ParametrageReferentielEtendus).ThenInclude(p => p.Unite)
                .Include(b => b.BudgetSousDetails).ThenInclude(sd => sd.Unite)
                .Include(b => b.T3.Parent.Parent)
                .AsNoTracking()
                .Where(b => b.BudgetId == budgetId && (date == null || b.T4.DateCreation < date));
            }

            foreach (var budgetT4 in query.ToList())
            {
                if (budgetT4.T3 == null)
                {
                    budgetT4.T3 = budgetT4.T4.Parent;
                    budgetT4.T3Id = budgetT4.T3.TacheId;
                }
                yield return budgetT4;
            }
        }

        /// <summary>
        /// Récupère un budget T4.
        /// </summary>
        /// <param name="tacheId">Identifiant de la tâche.</param>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <returns>Un budget T4.</returns>
        public BudgetT4Ent GetByTacheIdAndBudgetId(int tacheId, int budgetId)
        {
            return Context.BudgetT4
              .AsNoTracking()
              .FirstOrDefault(b => b.T4Id == tacheId && b.BudgetId == budgetId);
        }

        /// <summary>
        /// Récupère les budgets T4 d'un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant du budget.</param>
        /// <returns>Les budgets T4 du budget.</returns>
        public BudgetT4Ent GetByIdWithSousDetailAndAvancement(int budgetT4Id)
        {
            return Context.BudgetT4
                  .Include(b => b.T4)
                  .Include(b => b.BudgetSousDetails)
                  .AsNoTracking()
                  .FirstOrDefault(b => b.BudgetT4Id == budgetT4Id);
        }

        /// <summary>
        /// Récupère un T4 d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tacheId">Identifiant de la tâche</param>
        /// <returns>Le T4 du budget ou null s'il n'existe pas.</returns>
        public BudgetT4Ent Get(int budgetId, int tacheId)
        {
            return Context.BudgetT4.FirstOrDefault(t => t.BudgetId == budgetId && t.T4Id == tacheId);
        }

        public BudgetT4Ent GetByBudgetIdAndTacheIdWithSousDetails(int budgetId, int tacheId)
        {
            return Context.BudgetT4.Include(t => t.BudgetSousDetails).FirstOrDefault(t => t.BudgetId == budgetId && t.T4Id == tacheId);
        }

        public List<BudgetT4Ent> GetByBudgetIdAndTacheIdsWithSousDetails(int budgetId, IEnumerable<int> tacheIds)
        {
            return Context.BudgetT4
                .Include(t => t.BudgetSousDetails)
                .Where(t => t.BudgetId == budgetId && tacheIds.Contains(t.T4Id))
                .ToList();
        }

        /// <summary>
        /// Récupère la liste des budgets T4 enfant d'un T3 pour une version de budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tache3Id">Identifiant de la tâche</param>
        /// <returns>Le T4 du budget ou null s'il n'existe pas.</returns>
        public List<BudgetT4Ent> GetByBudgetIdAndTache3Id(int budgetId, int tache3Id)
        {
            return Context.BudgetT4.Where(t => t.BudgetId == budgetId && ((t.T3 != null && t.T3.TacheId == tache3Id) || (t.T3 == null && t.T4.Parent.TacheId == tache3Id))).ToList();
        }

        /// <summary>
        /// Retourne une liste des T4s groupé par T3 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget dont on veut les T4s</param>
        /// <returns>Les budgets T4 regrouper par tache de niveau 3</returns>
        public List<IGrouping<TacheEnt, BudgetT4Ent>> GetT4GroupByT3ByBudgetId(int budgetId)
        {
            return Context.BudgetT4.Include(t => t.T4.Parent).Include(t => t.T3).Where(t => t.BudgetId == budgetId).GroupBy(t => t.T3 ?? t.T4.Parent).ToList();
        }

        /// <summary>
        /// Retourne une liste des T4s groupé par T3 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget dont on veut les T4s</param>
        /// <returns>Les budgets T4 regrouper par tache de niveau 3</returns>
        public List<BudgetT4Ent> GetT4ByBudgetId(int budgetId)
        {
            return Context.BudgetT4.Include(t => t.T4.Parent).Include(t => t.BudgetSousDetails).Where(t => t.BudgetId == budgetId).ToList();
        }

        /// <summary>
        /// Indique si un budget est révisé.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <returns>True si le budget est révisé, sinon false.</returns>
        public bool IsBudgetRevise(int budgetId)
        {
            return Context.BudgetT4
            .AsNoTracking()
            .Where(b => b.BudgetId == budgetId)
            .Any(b => b.IsReadOnly);
        }

        public async Task UpdateVueSDToZero(List<BudgetT4Ent> budgetT4WithVueT4)
        {
            if (budgetT4WithVueT4 != null)
            {
                foreach (var budgetT4Ent in budgetT4WithVueT4)
                {
                    budgetT4Ent.VueSD = 0;
                }

                Context.UpdateRange(budgetT4WithVueT4);
                await Context.SaveChangesAsync();
            }
        }

        public void UpdateT4(BudgetT4Ent budgetT4)
        {
            Context.BudgetT4.Update(budgetT4);
        }
    }
}
