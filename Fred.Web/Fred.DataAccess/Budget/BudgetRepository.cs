using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Dao.BudgetComparaison.ExcelExport;
using Fred.EntityFramework;
using Fred.Framework.Extensions;
using Fred.Web.Shared.Models.Budget;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.Budget
{
    public class BudgetRepository : FredRepository<BudgetEnt>, IBudgetRepository
    {
        public BudgetRepository(FredDbContext context)
          : base(context)
        {
        }

        public BudgetEnt GetBudget(int budgetId, bool avecT4etRessource = false)
        {
            BudgetEnt budget;
            if (avecT4etRessource)
            {
                budget = Context.Budgets.Where(b => b.BudgetId == budgetId).AvecT4etRessource().SingleOrDefault();
            }
            else
            {
                budget = Context.Budgets.Where(b => b.BudgetId == budgetId).WithIncludes().SingleOrDefault();
            }
            return budget;
        }

        public IEnumerable<BudgetEnt> GetBudgetForCi(int ciId)
        {
            var budgets = Context.Budgets.Where(b => b.CiId == ciId).WithIncludes();
            return budgets;
        }

        public IEnumerable<BudgetEnt> GetBudgetVisiblePourUserSurCi(int utilisateurId, int ciId)
        {
            return GetBudgetVisiblePourUserSurCiQuery(utilisateurId, ciId)
              .Include(b => b.Ci)
              .Include(b => b.Devise)
              .Include(b => b.BudgetEtat)
              .Include(b => b.Workflows)
              .Include(b => b.Workflows).ThenInclude(w => w.EtatInitial)
              .Include(b => b.Workflows).ThenInclude(w => w.EtatCible)
              .Include(b => b.Workflows).ThenInclude(w => w.Auteur)
              .Include(b => b.Workflows).ThenInclude(w => w.Auteur.Personnel)
              .Include(b => b.BudgetT4s)
              .Include(b => b.Recette)
              .Include(b => b.CopyHistos)
              .Include(b => b.CopyHistos).ThenInclude(h => h.BudgetSourceCI)
              .Include(b => b.CopyHistos).ThenInclude(h => h.BibliothequePrixSourceCI);
        }

        public string GetBudgetMaxVersion(int budgetId)
        {
            var budget = GetBudget(budgetId);
            var versionMajeur = budget.Version.Split('.')[0];

            var budgetAvecMemeVersionMajeure = Context.Budgets.Where(b => b.CiId == budget.CiId && b.Version.StartsWith(versionMajeur));
            int max = 0;
            string versionMineureMax = versionMajeur + ".0";
            foreach (var budgetVersion in budgetAvecMemeVersionMajeure)
            {
                var versionMineure = Convert.ToInt32((budgetVersion.Version.Split('.')[1]));
                if (max < versionMineure)
                {
                    versionMineureMax = budgetVersion.Version;
                    max = versionMineure;
                }
            }
            return versionMineureMax;
        }

        public string GetCiMaxVersion(int ciId)
        {
            var listVersion = Context.Budgets.Where(b => b.CiId == ciId).Select(b => b.Version);
            var maxVersionMineure = 0;
            var maxVersionMajeure = 0;
            var versionMineure = 0;
            var versionMajeure = 0;
            if (listVersion.ToList().Count == 0)
            {
                return "0.0";
            }
            foreach (var version in listVersion)
            {
                versionMajeure = Convert.ToInt32(version.Split('.')[0]);
                if (maxVersionMajeure < versionMajeure)
                {
                    maxVersionMajeure = versionMajeure;
                }
            }
            foreach (var version in listVersion)
            {
                versionMajeure = Convert.ToInt32(version.Split('.')[0]);
                if (maxVersionMajeure == versionMajeure)
                {
                    versionMineure = Convert.ToInt32(version.Split('.')[1]);
                    if (maxVersionMineure < versionMineure)
                    {
                        maxVersionMineure = versionMineure;
                    }
                }
            }
            return string.Concat(maxVersionMajeure, '.', maxVersionMineure);
        }

        public BudgetEnt GetBudgetEnApplication(int ciId)
        {
            return Context.Budgets.FirstOrDefault(b => b.CiId == ciId && b.BudgetEtat != null && b.BudgetEtat.Code == EtatBudget.EnApplication);
        }

        public BudgetEnt GetBudgetEnApplicationIncludeDevise(int ciId)
        {
            return Query()
                .Include(x => x.Devise)
                .Get()
                .SingleOrDefault(b => b.CiId == ciId && b.BudgetEtat.Code == EtatBudget.EnApplication);
        }

        public int GetVersionMajeureEnApplication(int ciId)
        {
            var version = Context.Budgets.Where(b => b.CiId == ciId && b.BudgetEtat.Code == EtatBudget.EnApplication).Select(b => b.Version).ToString();
            return Convert.ToInt32(version.Split('.')[0]);
        }

        private IQueryable<BudgetEnt> GetBudgetVisiblePourUserSurCiQuery(int utilisateurId, int ciId)
        {
            return Context.Budgets
                .Where(b => b.CiId == ciId &&
                            (b.Workflows
                                 .OrderBy(w => w.Date)
                                 .Select(w => w.AuteurId)
                                 .FirstOrDefault() == utilisateurId
                            || (b.Partage ||
                                b.BudgetEtat.Code == EtatBudget.EnApplication ||
                                b.BudgetEtat.Code == EtatBudget.Archive ||
                                b.BudgetEtat.Code == EtatBudget.AValider)));
        }

        public TBudget GetBudget<TBudget>(int budgetId, Expression<Func<BudgetEnt, TBudget>> selector)
        {
            return Context.Budgets
                .Where(b => b.BudgetId == budgetId)
                .Select(selector)
                .FirstOrDefault();
        }

        public async Task<BudgetEnt> GetTargetBudgetForCopyAsync(int budgetId)
        {
            return await Context.Budgets
                .Include(b => b.BudgetT4s)
                .ThenInclude(bt => bt.BudgetSousDetails)
                .ThenInclude(bsd => bsd.Ressource)
                .Include(b => b.BudgetT4s)
                .ThenInclude(bt => bt.T4)
                .FirstOrDefaultAsync(b => b.BudgetId == budgetId);
        }

        public TBudget GetBudgetVisiblePourUserSurCi<TBudget>(int utilisateurId, int ciId, int deviseId, string revision, Expression<Func<BudgetEnt, TBudget>> selector)
        {
            return GetBudgetVisiblePourUserSurCiQuery(utilisateurId, ciId)
                .Where(b => b.DeviseId == deviseId && b.Version == revision)
                .Select(selector)
                .FirstOrDefault();
        }

        public List<BudgetRevisionLoadModel> GetBudgetRevisions(int ciId, int utilisateurId)
        {
            return GetBudgetRevisions(ciId, utilisateurId, b => new BudgetRevisionLoadModel
            {
                BudgetId = b.BudgetId,
                Revision = b.Version
            });
        }

        public List<Entities.Budget.Dao.BudgetComparaison.Comparaison.BudgetRevisionDao> GetBudgetRevisionsPourBudgetComparaison(int ciId, int utilisateurId)
        {
            return GetBudgetRevisions(ciId, utilisateurId, b => new Entities.Budget.Dao.BudgetComparaison.Comparaison.BudgetRevisionDao
            {
                BudgetId = b.BudgetId,
                Revision = b.Version,
                Etat = b.BudgetEtat.Libelle,
                PeriodeDebut = b.PeriodeDebut,
                PeriodeFin = b.PeriodeFin
            });
        }

        public BudgetsRevisionsDao GetBudgetRevisionsPourBudgetComparaisonExportExcel(int budgetId1, int budgetId2)
        {
            var revisions = Context.Budgets
                .Where(b => b.BudgetId == budgetId1 || b.BudgetId == budgetId2)
                .Select(b => new
                {
                    b.BudgetId,
                    Revision = new BudgetRevisionDao
                    {
                        Revision = b.Version,
                        Etat = b.BudgetEtat.Libelle
                    }
                })
                .ToList();

            return new BudgetsRevisionsDao
            {
                Budget1 = revisions.FirstOrDefault(r => r.BudgetId == budgetId1)?.Revision,
                Budget2 = revisions.FirstOrDefault(r => r.BudgetId == budgetId2)?.Revision,
            };
        }

        private List<TRevision> GetBudgetRevisions<TRevision>(int ciId, int utilisateurId, Expression<Func<BudgetEnt, TRevision>> selector)
        {
            return GetBudgetVisiblePourUserSurCiQuery(utilisateurId, ciId)
                .Where(b => b.DateSuppressionBudget == null)
                .OrderBy(b => b.Workflows.FirstOrDefault().Date)
                .Select(selector)
                .ToList();
        }
    }
}
