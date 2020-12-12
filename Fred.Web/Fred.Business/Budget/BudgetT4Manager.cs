using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Gestionnaire du budget T4.
    /// </summary>
    public class BudgetT4Manager : Manager<BudgetT4Ent, IBudgetT4Repository>, IBudgetT4Manager
    {
        public BudgetT4Manager(IUnitOfWork uow, IBudgetT4Repository budgetT4Repository)
          : base(uow, budgetT4Repository)
        { }

        /// <summary>
        /// Récupère les budgets T4 d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="loadSousDetails">Indique si on veut charger les sous-détails liés aux budget T4</param>
        /// <returns>Les budgets T4 du budget.</returns>
        public IEnumerable<BudgetT4Ent> GetByBudgetId(int budgetId, bool loadSousDetails = false)
        {
            return Repository.GetByBudgetId(budgetId, loadSousDetails);
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
            return Repository.GetByBudgetIdAndCreationDate(budgetId, date, loadSousDetails);
        }

        /// <summary>
        /// Récupère la liste des budgets T4 enfant d'un T3 pour une version de budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tache3Id">Identifiant de la tâche</param>
        /// <returns>Le T4 du budget ou null s'il n'existe pas.</returns>
        public List<BudgetT4Ent> GetByBudgetIdAndTache3Id(int budgetId, int tache3Id)
        {
            return Repository.GetByBudgetIdAndTache3Id(budgetId, tache3Id);
        }

        /// <summary>
        /// Récupère un budget T4.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <param name="tacheId">Identifiant de la tâche.</param>
        /// <returns>Un budget T4.</returns>
        public BudgetT4Ent GetByTacheIdAndBudgetId(int budgetId, int tacheId)
        {
            return Repository.GetByBudgetIdAndTacheIdWithSousDetails(budgetId, tacheId);
        }

        public List<BudgetT4Ent> GetByTacheIdsAndBudgetId(int budgetId, IEnumerable<int> tacheIds)
        {
            return Repository.GetByBudgetIdAndTacheIdsWithSousDetails(budgetId, tacheIds);
        }

        /// <summary>
        /// Ajoute un budgetT4 au contexte
        /// </summary>
        /// <param name="budgetT4">Budget T4.</param>
        public void Add(BudgetT4Ent budgetT4)
        {
            Repository.Insert(budgetT4);
        }

        /// <summary>
        /// Met à jour un budgetT4 au contexte
        /// </summary>
        /// <param name="budgetT4">Budget T4.</param>
        public void Update(BudgetT4Ent budgetT4)
        {
            Repository.UpdateT4(budgetT4);
        }

        /// <summary>
        /// Récupère les budgets T4 d'un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant du budget.</param>
        /// <returns>Les budgets T4 du budget.</returns>
        public BudgetT4Ent GetByIdWithSousDetailAndAvancement(int budgetT4Id)
        {
            return Repository.GetByIdWithSousDetailAndAvancement(budgetT4Id);
        }

        /// <summary>
        /// Retourne une liste des T4s groupé par T3 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget dont on veut les T4s</param>
        /// <returns>Les budgets T4 regrouper par tache de niveau 3</returns>
        public List<IGrouping<TacheEnt, BudgetT4Ent>> GetT4GroupByT3ByBudgetId(int budgetId)
        {
            return Repository.GetT4GroupByT3ByBudgetId(budgetId);
        }

        /// <summary>
        /// Retourne une liste des T4s 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget dont on veut les T4s</param>
        /// <returns>Les budgets T4 </returns>
        public List<BudgetT4Ent> GetT4ByBudgetId(int budgetId)
        {
            return Repository.GetT4ByBudgetId(budgetId);
        }

        /// <summary>
        /// Enregistre.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="taches4Model">Les tâches de niveau 4 à enregistrer.</param>
        /// <param name="budgetT4sDeleted">Liste des identifiants des budget T4 à supprimer</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public List<BudgetDetailSave.BudgetT4CreatedModel> Save(int budgetId, List<BudgetDetailSave.Tache4Model> taches4Model, List<int> budgetT4sDeleted)
        {
            var ret = new List<BudgetDetailSave.BudgetT4CreatedModel>();
            var budgetT4EntsCreated = new List<BudgetT4Ent>();
            if (taches4Model.Any() || budgetT4sDeleted.Any())
            {
                foreach (var budgetT4Deleted in budgetT4sDeleted)
                {
                    Repository.DeleteById(budgetT4Deleted);
                }

                foreach (var tache in taches4Model)
                {
                    var budgetT4Ent = Repository.Get(budgetId, tache.TacheId);
                    if (budgetT4Ent == null)
                    {
                        budgetT4Ent = new BudgetT4Ent()
                        {
                            BudgetId = budgetId,
                            T4Id = tache.TacheId,
                            Commentaire = tache.Commentaire,
                            TypeAvancement = tache.TypeAvancement,
                            T3Id = tache.Tache3Id
                        };

                        Repository.Insert(budgetT4Ent);
                        budgetT4EntsCreated.Add(budgetT4Ent);
                    }
                    else
                    {
                        budgetT4Ent.Commentaire = tache.Commentaire;
                        budgetT4Ent.TypeAvancement = tache.TypeAvancement;
                        budgetT4Ent.T3Id = tache.Tache3Id;
                        Repository.Update(budgetT4Ent);
                    }
                }

                Save();

                foreach (var budgetT4EntCreated in budgetT4EntsCreated)
                {
                    ret.Add(new BudgetDetailSave.BudgetT4CreatedModel(budgetT4EntCreated));
                }
            }

            return ret;
        }

        /// <summary>
        /// Enregistre.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="tache4Model">La tâche de niveau 4 à enregistrer.</param>
        /// <param name="created">Indique si le budget T4 a été créé.</param>
        /// <returns>Le budget T4.</returns>
        public BudgetT4Ent Save(int budgetId, BudgetSousDetailSave.Tache4Model tache4Model, out bool created)
        {
            created = false;
            var budgetT4Ent = Repository.Get(budgetId, tache4Model.TacheId);
            if (budgetT4Ent == null)
            {
                budgetT4Ent = new BudgetT4Ent()
                {
                    BudgetId = budgetId,
                    T4Id = tache4Model.TacheId,
                    MontantT4 = tache4Model.MontantT4,
                    PU = tache4Model.PU,
                    QuantiteDeBase = tache4Model.QuantiteDeBase,
                    QuantiteARealiser = tache4Model.QuantiteARealiser,
                    UniteId = tache4Model.UniteId,
                    VueSD = tache4Model.VueSD
                };

                Repository.Insert(budgetT4Ent);
                created = true;
            }
            else
            {
                budgetT4Ent.MontantT4 = tache4Model.MontantT4;
                budgetT4Ent.PU = tache4Model.PU;
                budgetT4Ent.QuantiteDeBase = tache4Model.QuantiteDeBase;
                budgetT4Ent.QuantiteARealiser = tache4Model.QuantiteARealiser;
                budgetT4Ent.UniteId = tache4Model.UniteId;
                budgetT4Ent.VueSD = tache4Model.VueSD;
                Repository.Update(budgetT4Ent);
            }

            Save();
            return budgetT4Ent;
        }

        /// <summary>
        /// Indique si un budget est révisé.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <returns>True si le budget est révisé, sinon false.</returns>
        public bool IsBudgetRevise(int budgetId)
        {
            return Repository.IsBudgetRevise(budgetId);
        }
    }
}
