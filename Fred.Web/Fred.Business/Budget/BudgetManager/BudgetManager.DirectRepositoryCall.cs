using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget.BudgetManager
{
    public partial class BudgetManager : Manager<BudgetEnt, IBudgetRepository>, IBudgetManager
    {
        public int GetCiIdAssociatedToBudgetId(int budgetId)
        {
            return Repository.FindById(budgetId).CiId;
        }

        public BudgetEnt GetBudget(int budgetId, bool avecT4etRessource = false)
        {
            return Repository.GetBudget(budgetId, avecT4etRessource);
        }

        public IEnumerable<BudgetEnt> GetBudgetVisiblePourUserSurCi(int utilisateurId, int ciId)
        {
            return Repository.GetBudgetVisiblePourUserSurCi(utilisateurId, ciId);
        }

        public string GetBudgetMaxVersion(int budgetId)
        {
            return Repository.GetBudgetMaxVersion(budgetId);
        }

        public string GetCiMaxVersion(int ciId)
        {
            return Repository.GetCiMaxVersion(ciId);
        }

        public BudgetEnt GetBudgetEnApplication(int ciId)
        {
            return Repository.GetBudgetEnApplication(ciId);
        }

        public BudgetEnt GetBudgetEnApplicationIncludeDevise(int ciId)
        {
            return Repository.GetBudgetEnApplicationIncludeDevise(ciId);
        }

        public int GetIdBudgetEnApplication(int ciId)
        {
            return Repository.SelectOneColumn(b => b.BudgetId,
                    b => b.CiId == ciId
                    && b.BudgetEtat.Code == EtatBudget.EnApplication
                ).FirstOrDefault();
        }

        public List<int> GetListIdBudgetEnApplication(int ciId)
        {
            return Repository.SelectOneColumn(b => b.BudgetId,
                  b => (b.CiId == ciId)
                  && b.BudgetEtat.Code == EtatBudget.EnApplication
              ).ToList();
        }   

        public int GetVersionMajeureEnApplication(int ciId)
        {
            return Repository.GetVersionMajeureEnApplication(ciId);
        }

        public BudgetEnt Update(BudgetEnt budget)
        {
            Repository.Update(budget);
            Save();
            return budget;
        }

        public BudgetEnt Create(BudgetEnt budget)
        {
            var budgetInsere = Repository.Insert(budget);
            Save();
            return budgetInsere;
        }

        public void DeleteById(int budgetId)
        {
            Repository.DeleteById(budgetId);
            Save();
        }


        public int? GetPeriodeDebutBudget(int budgetId)
        {
            return Repository.SelectOneColumn(b => b.PeriodeDebut, b => b.BudgetId == budgetId).FirstOrDefault();
        }
    }
}
