using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Gestionnaire du budget Tache.
    /// </summary>
    public class BudgetTacheManager : Manager<BudgetTacheEnt, IBudgetTacheRepository>, IBudgetTacheManager
    {
        public BudgetTacheManager(IUnitOfWork uow, IBudgetTacheRepository budgetTacheRepository)
          : base(uow, budgetTacheRepository)
        { }

        /// <inheritdoc />
        public void AddBudgetTache(BudgetTacheEnt budgetTache)
        {
            this.Repository.Insert(budgetTache);
            this.Save();
        }

        /// <inheritdoc />
        public void DeleteBudgetTache(int budgetTacheId)
        {
            this.Repository.DeleteById(budgetTacheId);
            this.Save();
        }

        /// <inheritdoc />
        public BudgetTacheEnt GetByBudgetIdAndTacheId(int budgetId, int tacheId)
        {
            return this.Repository.GetByBudgetIdAndTacheId(budgetId, tacheId);
        }

        /// <inheritdoc />
        public BudgetTacheEnt UpdateBudgetTache(BudgetTacheEnt budgetTache)
        {
            this.Repository.Update(budgetTache);
            this.Save();
            return budgetTache;
        }

        /// <summary>
        /// Récupère les informations sur les tâches pour un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <returns>Les informations sur les tâches.</returns>
        public IEnumerable<BudgetTacheEnt> Get(int budgetId)
        {
            return Repository.Get(budgetId);
        }



        /// <summary>
        /// Enregistre.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tache1To3sModel">Les tâches de niveau 1 à 3 à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public List<BudgetDetailSave.BudgetTacheCreatedModel> Save(int budgetId, List<BudgetDetailSave.Tache1To3Model> tache1To3sModel)
        {
            var ret = new List<BudgetDetailSave.BudgetTacheCreatedModel>();
            var budgetTachesCreated = new List<BudgetTacheEnt>();
            if (tache1To3sModel.Any())
            {
                foreach (var tache in tache1To3sModel)
                {
                    var budgetTacheEnt = Repository.GetByBudgetIdAndTacheId(budgetId, tache.TacheId);
                    if (budgetTacheEnt == null)
                    {
                        budgetTacheEnt = new BudgetTacheEnt()
                        {
                            BudgetId = budgetId,
                            TacheId = tache.TacheId,
                            Commentaire = tache.Commentaire
                        };

                        Repository.Insert(budgetTacheEnt);
                        budgetTachesCreated.Add(budgetTacheEnt);
                    }
                    else
                    {
                        budgetTacheEnt.Commentaire = tache.Commentaire;
                        Repository.Update(budgetTacheEnt);
                    }
                }

                Save();

                foreach (var budgetTacheCreated in budgetTachesCreated)
                {
                    ret.Add(new BudgetDetailSave.BudgetTacheCreatedModel(budgetTacheCreated));
                }
            }

            return ret;
        }
    }
}
