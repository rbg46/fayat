using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Gestionnaire du budget sous détail.
    /// </summary>
    public class BudgetSousDetailManager : Manager<BudgetSousDetailEnt, IBudgetSousDetailRepository>, IBudgetSousDetailManager
    {
        public BudgetSousDetailManager(IUnitOfWork uow, IBudgetSousDetailRepository budgetSousDetailRepository)
          : base(uow, budgetSousDetailRepository)
        { }

        /// <summary>
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant de la tâche de niveau 4.</param>
        /// <returns>Le sous-détail de la tâche de niveau 4 pour le budget.</returns>
        public IEnumerable<BudgetSousDetailEnt> GetByBudgetT4Id(int budgetT4Id)
        {
            return Repository.GetByBudgetT4Id(budgetT4Id);
        }

        /// <summary>
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant de la tâche de niveau 4.</param>
        /// <returns>Le sous-détail de la tâche de niveau 4 pour le budget.</returns>
        public IEnumerable<BudgetSousDetailEnt> GetByBudgetT4IdIncludeRessource(int budgetT4Id)
        {
            return Repository.GetByBudgetT4IdIncludeRessource(budgetT4Id);
        }

        /// <summary>
        /// Insère un sous-détail dans le contexte
        /// </summary>
        /// <param name="sousDetail">Le sous-détail à insérer</param>
        public void InsereSousDetail(BudgetSousDetailEnt sousDetail)
        {
            Repository.Insert(sousDetail);
        }

        /// <summary>
        /// Met à jour un sous-détail dans le contexte
        /// </summary>
        /// <param name="sousDetail">Le sous-détail à insérer</param>
        public void UpdateSousDetail(BudgetSousDetailEnt sousDetail)
        {
            Repository.UpdateSousDetail(sousDetail);
        }

        /// <summary>
        /// Enregistre.
        /// </summary>
        /// <param name="budgetT4Id">Identifiant du détail.</param>
        /// <param name="itemsChanged">Elements du sous-détail ajoutés ou modifiés.</param>
        /// <param name="itemsDeletedId">Identifiants des élements du sous-détail supprimés.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public List<BudgetSousDetailSave.ItemCreatedModel> Save(int budgetT4Id, List<BudgetSousDetailSave.ItemModel> itemsChanged, List<int> itemsDeletedId)
        {
            var ret = new List<BudgetSousDetailSave.ItemCreatedModel>();
            var budgetSousDetailEntsCreated = new Dictionary<int, BudgetSousDetailEnt>();
            if (itemsChanged.Any() || itemsDeletedId.Any())
            {
                // Elements supprimé
                foreach (var itemDeletedId in itemsDeletedId)
                {
                    Repository.DeleteById(itemDeletedId);
                }

                // Eléments ajoutés ou modifiés
                foreach (var itemChanged in itemsChanged)
                {
                    itemChanged.Montant = CalculateMontant(itemChanged.PrixUnitaire, itemChanged.Quantite);
                    var budgetSousDetailEnt = Repository.GetById(itemChanged.BudgetSousDetailId);
                    if (budgetSousDetailEnt == null)
                    {
                        budgetSousDetailEnt = new BudgetSousDetailEnt()
                        {
                            BudgetT4Id = budgetT4Id,
                            RessourceId = itemChanged.RessourceId,
                            QuantiteSD = itemChanged.QuantiteSD,
                            QuantiteSDFormule = itemChanged.QuantiteSDFormule,
                            Quantite = itemChanged.Quantite,
                            QuantiteFormule = itemChanged.QuantiteFormule,
                            PU = itemChanged.PrixUnitaire,
                            Montant = itemChanged.Montant,
                            Commentaire = itemChanged.Commentaire,
                            UniteId = itemChanged.UniteId
                        };

                        Repository.Insert(budgetSousDetailEnt);
                        budgetSousDetailEntsCreated.Add(itemChanged.ViewId, budgetSousDetailEnt);
                    }
                    else
                    {
                        budgetSousDetailEnt.QuantiteSD = itemChanged.QuantiteSD;
                        budgetSousDetailEnt.QuantiteSDFormule = itemChanged.QuantiteSDFormule;
                        budgetSousDetailEnt.Quantite = itemChanged.Quantite;
                        budgetSousDetailEnt.QuantiteFormule = itemChanged.QuantiteFormule;
                        budgetSousDetailEnt.PU = itemChanged.PrixUnitaire;
                        budgetSousDetailEnt.Montant = itemChanged.Montant;
                        budgetSousDetailEnt.Commentaire = itemChanged.Commentaire;
                        budgetSousDetailEnt.UniteId = itemChanged.UniteId;
                        Repository.Update(budgetSousDetailEnt);
                    }
                }

                // Enregistre
                Save();

                // Mets à jour les identifiants des éléments ajoutés pour la vue.
                foreach (var kvp in budgetSousDetailEntsCreated)
                {
                    ret.Add(new BudgetSousDetailSave.ItemCreatedModel(kvp.Key, kvp.Value.BudgetSousDetailId));
                }
            }

            return ret;
        }

        /// <summary>
        /// Calcul du montant à partir du prix unitaire et de la quantité
        /// </summary>
        /// <param name="prixUnitaire">Prix unitaire</param>
        /// <param name="quantite">Quantité</param>
        /// <returns>Montant calculé</returns>
        private decimal? CalculateMontant(decimal? prixUnitaire, decimal? quantite)
        {
            if (!prixUnitaire.HasValue || !quantite.HasValue)
            {
                return null;
            }
            return prixUnitaire.Value * quantite.Value;
        }
    }
}
