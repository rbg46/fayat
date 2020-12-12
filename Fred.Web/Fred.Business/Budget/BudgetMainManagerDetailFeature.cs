using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Budget.Details;
using Fred.Business.Budget.Extensions;
using Fred.Business.Params;
using Fred.Business.Referential;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielEtendu;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Common;
using Fred.Entities.Budget;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.Details;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget
{
    public class BudgetMainManagerDetailFeature : ManagersAccess
    {
        private readonly BudgetMainManagerSousDetailFeature sousDetailFeature;
        private readonly BudgetMainManagerVersioningFeature versioningFeature;
        private readonly IBudgetT4Repository budgetT4Repository;
        private readonly IBudgetEtatManager budgetEtatManager;
        private readonly IBudgetTacheManager budgetTacheManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IBudgetManager budgetManager;
        private readonly IBudgetWorkflowManager budgetWorkflowManager;
        private readonly IParamsManager paramsManager;
        private readonly ITacheManager tacheManager;
        private readonly IDeviseManager deviseManager;
        private readonly IUnitOfWork uow;
        private readonly ITacheRepository tacheRepository;
        private readonly ITacheSearchHelper taskSearchHelper;
        private readonly ISocieteManager societeManager;

        public BudgetMainManagerDetailFeature(
            IUnitOfWork uow,
            ITacheRepository tacheRepository,
            ITacheSearchHelper taskSearchHelper,
            IRessourceRepository ressourceRepository,
            IBudgetBibliothequePrixItemRepository budgetBibliothequePrixItemRepository,
            ICIRepository ciRepository,
            IBudgetT4Repository budgetT4Repository,
            IBudgetEtatManager budgetEtatManager,
            IBudgetTacheManager budgetTacheManager,
            IBudgetSousDetailManager budgetSousDetailManager,
            IBudgetT4Manager budgetT4Manager,
            IBudgetManager budgetManager,
            IBudgetWorkflowManager budgetWorkflowManager,
            IParamsManager paramsManager,
            IReferentielEtenduManager referentielEtenduManager,
            ITacheManager tacheManager,
            IDeviseManager deviseManager,
            IRessourceValidator ressourceValidator,
            ISocieteManager societeManager)
        {
            this.uow = uow;
            this.tacheRepository = tacheRepository;

            sousDetailFeature = new BudgetMainManagerSousDetailFeature(
                uow,
                ressourceRepository,
                budgetBibliothequePrixItemRepository,
                ciRepository,
                budgetT4Repository,
                budgetSousDetailManager,
                budgetT4Manager,
                budgetManager,
                referentielEtenduManager,
                ressourceValidator,
                societeManager);

            versioningFeature = new BudgetMainManagerVersioningFeature(budgetEtatManager, budgetManager);
            this.budgetT4Repository = budgetT4Repository;
            this.budgetEtatManager = budgetEtatManager;
            this.budgetTacheManager = budgetTacheManager;
            this.budgetT4Manager = budgetT4Manager;
            this.budgetManager = budgetManager;
            this.budgetWorkflowManager = budgetWorkflowManager;
            this.paramsManager = paramsManager;
            this.tacheManager = tacheManager;
            this.deviseManager = deviseManager;
            this.societeManager = societeManager;
            this.taskSearchHelper = taskSearchHelper;
        }

        /// <summary>
        /// Renvoi un export excel en utilisant la classe BudgetDetailExportExcelFeature
        /// </summary>
        /// <param name="detailBudgetExportExcel">Type de feature</param> 
        /// <param name="model">BudgetDetailsExportExcelLoadModel</param>
        /// <returns>les données de l'excel sous forme de tableau d'octets</returns>
        public byte[] GetExportExcel(IBudgetDetailsExportExcelFeature detailBudgetExportExcel, BudgetDetailsExportExcelLoadModel model)
        {
            return detailBudgetExportExcel.GetBudgetDetailExportExcel(model);
        }

        /// <summary>
        /// Retourne le détail d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <returns>Le détail du budget.</returns>
        public BudgetDetailLoad.Model Get(int budgetId)
        {
            return new BudgetT4Loader(
                budgetId,
                taskSearchHelper,
                budgetEtatManager,
                budgetTacheManager,
                budgetT4Manager,
                budgetManager,
                budgetWorkflowManager,
                paramsManager,
                tacheManager,
                deviseManager,
                societeManager).Load();
        }

        /// <summary>
        /// Enregistre le détail d'un budget.
        /// </summary>
        /// <param name="model">Données à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public BudgetDetailSave.ResultModel Save(BudgetDetailSave.Model model)
        {
            var ret = new BudgetDetailSave.ResultModel();
            try
            {
                // NPI : transaction
                ret.BudgetTachesCreated = budgetTacheManager.Save(model.BudgetId, model.Tache1To3s);
                ret.BudgetT4sCreated = budgetT4Manager.Save(model.BudgetId, model.Taches4, model.BudgetT4sDeleted);
                foreach (var tache4Model in model.Taches4)
                {
                    var tacheEnt = tacheRepository.GetTacheById(tache4Model.TacheId);
                    if (tacheEnt.ParentId == null) continue;
                    var tacheParentEnt = tacheRepository.GetTacheById(tache4Model.Tache3Id);
                    tacheEnt.Active = tacheParentEnt.Active;
                    tacheRepository.UpdateTache(tacheEnt);
                    uow.Save();
                }
            }
            catch (Exception ex)
            {
                ret.Erreur = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// Valide le détail d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="commentaire">Commentaire</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public string Validate(int budgetId, string commentaire = "")
        {
            var userId = Managers.Utilisateur.GetContextUtilisateurId();
            var budget = budgetManager.GetBudget(budgetId, true);

            var societeIdOfCi = Managers.CI.GetSocieteByCIId(budget.CiId).SocieteId;
            var societeOfCi = societeManager.GetSocieteByIdWithParameters(societeIdOfCi);
            var erreur = string.Empty;
            if (societeOfCi.IsBudgetSaisieRecette)
            {
                erreur = budget.CheckBudgetRecette();
            }
            erreur = string.Concat(erreur, budget.BudgetT4s.CheckBudgetT4OfBudget(societeOfCi.IsBudgetTypeAvancementDynamique));

            if (string.IsNullOrEmpty(erreur))
            {
                if (budget.BudgetEtat.Code == EtatBudget.Brouillon)
                {
                    versioningFeature.BudgetBrouillonToAValider(budget, userId, commentaire);
                }
                else if (budget.BudgetEtat.Code == EtatBudget.AValider)
                {
                    versioningFeature.BudgetAValiderToEnApplication(budget, userId, commentaire);
                }
            }

            return erreur;
        }

        /// <summary>
        /// Passe l'état d'un budget 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <param name="commentaire">Commentaire</param>
        /// <returns>vide</returns>
        public string RetourBrouillon(int budgetId, string commentaire = "")
        {
            var userId = Managers.Utilisateur.GetContextUtilisateurId();
            var budget = budgetManager.GetBudget(budgetId);
            if (budget.BudgetEtat.Code == EtatBudget.AValider)
            {
                versioningFeature.BudgetAValiderToBrouillon(budget, userId, commentaire);
            }
            return string.Empty;
        }

        /// <summary>
        /// Copie des sous-détails dans des budgets T4.
        /// </summary>
        /// <param name="model">Données à copier.</param>
        /// <returns>Le résultat de la copie.</returns>
        public BudgetSousDetailCopier.ResultModel CopySousDetails(BudgetSousDetailCopier.Model model)
        {
            var ret = new BudgetSousDetailCopier.ResultModel();
            var budgetT4CibleEnts = new List<BudgetT4Ent>();
            foreach (var itemToCopyModel in model.Items)
            {
                var budgetT4CibleModel = itemToCopyModel.BudgetT4Cible;
                var budgetT4CibleEnt = new BudgetT4Ent()
                {
                    BudgetId = model.BudgetCibleId,
                    T4Id = budgetT4CibleModel.TacheId,
                    MontantT4 = budgetT4CibleModel.MontantT4,
                    PU = budgetT4CibleModel.PU,
                    QuantiteDeBase = budgetT4CibleModel.QuantiteDeBase,
                    QuantiteARealiser = budgetT4CibleModel.QuantiteARealiser,
                    UniteId = budgetT4CibleModel.UniteId,
                    VueSD = budgetT4CibleModel.VueSD,
                    TypeAvancement = budgetT4CibleModel.TypeAvancement
                };
                budgetT4CibleEnts.Add(budgetT4CibleEnt);

                var sousDetailSourceModel = sousDetailFeature.Get(model.CiSourceId, itemToCopyModel.BudgetT4SourceId);
                if (budgetT4CibleEnt.BudgetSousDetails == null)
                {
                    budgetT4CibleEnt.BudgetSousDetails = new List<BudgetSousDetailEnt>();
                }
                foreach (var itemSourceModel in sousDetailSourceModel.Items)
                {
                    budgetT4CibleEnt.BudgetSousDetails.Add(new BudgetSousDetailEnt()
                    {
                        RessourceId = itemSourceModel.Ressource.RessourceId,
                        QuantiteSD = itemSourceModel.QuantiteSD,
                        QuantiteSDFormule = itemSourceModel.QuantiteSDFormule,
                        Quantite = itemSourceModel.Quantite,
                        QuantiteFormule = itemSourceModel.QuantiteFormule,
                        PU = itemSourceModel.PrixUnitaire,
                        Montant = itemSourceModel.Montant,
                        Commentaire = itemSourceModel.Commentaire,
                        UniteId = itemSourceModel.Unite?.UniteId
                    });
                }

                budgetT4Repository.Insert(budgetT4CibleEnt);
            }

            uow.Save();

            foreach (var budgetT4CibleEnt in budgetT4CibleEnts)
            {
                ret.BudgetT4sIdCreated.Add(budgetT4CibleEnt.BudgetT4Id);
            }

            return ret;
        }

        /// <summary>
        /// Retourne les tâches de niveau 4 qui ne sont pas utilisées dans une révision de budget.
        /// </summary>
        /// <param name="model">Model de chargement des tâches 4 non utilisées dans une révision de budget.</param>
        /// <returns>Le résultat du chargement.</returns>
        public Tache4Inutilisees.ResultModel GetTache4Inutilisees(Tache4Inutilisees.Model model)
        {
            var ret = new Tache4Inutilisees.ResultModel();

            // Récupères toutes les tâches de niveau 4 qui concernent le CI
            var allTaches = tacheManager.GetTacheListByCiIdAndNiveau(model.CiId, 4)
                .OrderBy(t => t.Code)
                .Where(t => t.Active)
                .ToList();

            // Récupère toutes les tâches en lecture seule, comme les T4 rev, afin de les ignorer
            // L'information est stockée au niveau du budget T4, il faut donc faire le lien
            var budgetT4Readonlys = budgetT4Manager.GetByBudgetId(model.BudgetId, false).Where(bt4 => bt4.IsReadOnly).ToList();

            // Parcours toutes les tâches du CI
            foreach (var tache in allTaches)
            {
                // Si au moins un budget T4 lié à la tâche courante est en lecture seule, on l'ignore
                if (budgetT4Readonlys.FirstOrDefault(bt4 => bt4.T4Id == tache.TacheId) != null)
                {
                    continue;
                }

                // Ingore les tâches actuellement dans la révision et pas forcément encore enregistrées
                if (model.Tache4Ids.Any(t4Id => t4Id == tache.TacheId))
                {
                    continue;
                }

                ret.Taches4.Add(new Tache4Inutilisees.Tache4Model(tache));
            }
            return ret;
        }
    }
}
