using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Budget
{
    /// <summary>
    /// Référentiel de données pour budget sous détail.
    /// </summary>
    public class BudgetSousDetailRepository : FredRepository<BudgetSousDetailEnt>, IBudgetSousDetailRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="logMgr">Gestionnaire des logs.</param>
        /// <param name="context">Le contexte.</param>
        public BudgetSousDetailRepository(FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant de la tâche de niveau 4.</param>
        /// <returns>Le sous-détail de la tâche de niveau 4 pour le budget.</returns>
        public IEnumerable<BudgetSousDetailEnt> GetByBudgetT4Id(int budgetT4Id)
        {
            return Context.BudgetSousDetails
              .Include(b => b.Unite)
              .AsNoTracking()
              .Where(b => b.BudgetT4Id == budgetT4Id).ToList();
        }

        /// <summary>
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant de la tâche de niveau 4.</param>
        /// <returns>Le sous-détail de la tâche de niveau 4 pour le budget.</returns>
        public IEnumerable<BudgetSousDetailEnt> GetByBudgetT4IdIncludeRessource(int budgetT4Id)
        {
            return Context.BudgetSousDetails
              .Include(b => b.Ressource)
              .Include(b => b.Unite)
              .AsNoTracking()
              .Where(b => b.BudgetT4Id == budgetT4Id).ToList();
        }

        /// <summary>
        /// Retourne un sous-détail en fonction de son identifiant.
        /// </summary>
        /// <param name="budgetSousDetailId">Identifiant du sous-détail.</param>
        /// <returns>Le sous-détail ou null s'il n'existe pas.</returns>
        public BudgetSousDetailEnt GetById(int budgetSousDetailId)
        {
            return Context.BudgetSousDetails.FirstOrDefault(sd => sd.BudgetSousDetailId == budgetSousDetailId);
        }

        /// <summary>
        /// Récupère les sous-détails pour la comparaison de budget.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget concerné.</param>
        /// <returns>Les sous-détails.</returns>
        public List<SousDetailDao> GetSousDetailPourBudgetComparaison(int budgetId)
        {
            return context.BudgetSousDetails
                .Where(sd => sd.BudgetT4.BudgetId == budgetId)
                .Select(sd => new SousDetailDao
                {
                    BudgetId = budgetId,
                    SousDetailId = sd.BudgetSousDetailId,
                    Tache1Id = sd.BudgetT4.T3 != null ? sd.BudgetT4.T3.Parent.Parent.TacheId : sd.BudgetT4.T4.Parent.Parent.Parent.TacheId,
                    Tache2Id = sd.BudgetT4.T3 != null ? sd.BudgetT4.T3.Parent.TacheId : sd.BudgetT4.T4.Parent.Parent.TacheId,
                    Tache3Id = sd.BudgetT4.T3 != null ? sd.BudgetT4.T3.TacheId : sd.BudgetT4.T4.Parent.TacheId,
                    Tache4Id = sd.BudgetT4.T4Id,
                    ChapitreId = sd.Ressource.SousChapitre.Chapitre.ChapitreId,
                    SousChapitreId = sd.Ressource.SousChapitre.SousChapitreId,
                    RessourceId = sd.Ressource.RessourceId,
                    Quantite = sd.Quantite,
                    PrixUnitaire = sd.PU,
                    Montant = (sd.Quantite ?? 0) * (sd.PU ?? 0),
                    UniteId = sd.UniteId,
                })
                .ToList();
        }

        public void UpdateSousDetail(BudgetSousDetailEnt sousDetail)
        {
            context.BudgetSousDetails.Update(sousDetail);
        }
    }
}
