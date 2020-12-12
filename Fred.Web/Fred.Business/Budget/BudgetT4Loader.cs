using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Budget.Extensions;
using Fred.Business.Params;
using Fred.Business.Referential;
using Fred.Business.Referential.Tache;
using Fred.Business.Societe;
using Fred.DataAccess.Referential.Common;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget
{
    public class BudgetT4Loader : ManagersAccess
    {
        private readonly int budgetId;
        private List<TacheEnt> tacheEnts;
        private List<BudgetTacheEnt> budgetTacheEnts;
        private IEnumerable<BudgetT4Ent> budgetT4Ents;
        private BudgetDetailLoad.Model loadModel;
        private DateTime? dateDeleteNotificationNewTask = null;
        private readonly ITacheSearchHelper taskSearchHelper;
        private readonly IBudgetEtatManager budgetEtatManager;
        private readonly IBudgetTacheManager budgetTacheManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IBudgetManager budgetManager;
        private readonly IBudgetWorkflowManager budgetWorkflowManager;
        private readonly IParamsManager paramsManager;
        private readonly ITacheManager tacheManager;
        private readonly IDeviseManager deviseManager;
        private readonly ISocieteManager societeManager;

        public BudgetT4Loader(
            int budgetId,
            ITacheSearchHelper taskSearchHelper,
            IBudgetEtatManager budgetEtatManager,
            IBudgetTacheManager budgetTacheManager,
            IBudgetT4Manager budgetT4Manager,
            IBudgetManager budgetManager,
            IBudgetWorkflowManager budgetWorkflowManager,
            IParamsManager paramsManager,
            ITacheManager tacheManager,
            IDeviseManager deviseManager,
            ISocieteManager societeManager)
        {
            this.budgetId = budgetId;
            this.taskSearchHelper = taskSearchHelper;
            this.budgetEtatManager = budgetEtatManager;
            this.budgetTacheManager = budgetTacheManager;
            this.budgetT4Manager = budgetT4Manager;
            this.budgetManager = budgetManager;
            this.budgetWorkflowManager = budgetWorkflowManager;
            this.paramsManager = paramsManager;
            this.tacheManager = tacheManager;
            this.deviseManager = deviseManager;
            this.societeManager = societeManager;
        }

        /// <summary>
        /// Charge le détail d'un budget.
        /// </summary>
        /// <returns>Le détail du budget.</returns>
        public BudgetDetailLoad.Model Load()
        {
            tacheEnts = null;
            budgetTacheEnts = null;
            budgetT4Ents = null;
            loadModel = new BudgetDetailLoad.Model();

            var budgetEnt = budgetManager.FindById(budgetId);
            if (budgetEnt == null)
            {
                loadModel.Erreur = FeatureBudget._Budget_Erreur_Chargement_BudgetInexistant;
                return loadModel;
            }

            dateDeleteNotificationNewTask = budgetEnt.DateDeleteNotificationNewTask;
            var dateMiseEnApplicationBudget = budgetWorkflowManager.GetLastLockWorkflowDate(budgetEnt.BudgetId);
            var budgetEtat = budgetEtatManager.FindById(budgetEnt.BudgetEtatId);
            budgetTacheEnts = budgetTacheManager.Get(budgetId).ToList();

            var ciDeviseEnts = Managers.CI.GetCIDevise(budgetEnt.CiId).ToList();
            var budgetDeviseEnt = deviseManager.FindById(budgetEnt.DeviseId);
            if (ciDeviseEnts.FirstOrDefault(cd => cd.Devise != null && cd.Devise.DeviseId == budgetDeviseEnt.DeviseId) == null)
            {
                // Ici la devise du budget n'est pas une devise possible du CI
                // Ajoute cette devise à la liste
                budgetDeviseEnt.IsDeleted = false;
                budgetDeviseEnt.Active = true;
                var ciDeviseEnt = new CIDeviseEnt()
                {
                    CiDeviseId = 0,
                    CiId = budgetEnt.CiId,
                    DeviseId = budgetDeviseEnt.DeviseId,
                    Devise = budgetDeviseEnt
                };
                ciDeviseEnts.Insert(0, ciDeviseEnt);

                loadModel.Avertissement = FeatureBudgetDetail.Budget_Detail_Erreur_Chargement_DeviseInvalide;
            }

            var ciEnt = Managers.CI.GetCI(budgetEnt.CiId);

            // Récupère l'identifiant de la société
            var societe = ciEnt.Societe;
            if (societe == null)
            {
                societe = Managers.CI.GetSocieteByCIId(ciEnt.CiId);
            }
            var organisationId = societeManager.GetOrganisationIdBySocieteId(societe.SocieteId);
            var paramValues = paramsManager.GetOrganisationParamValues(organisationId.Value);

            loadModel.Set(budgetEnt, budgetEtat, societe, ciEnt, budgetDeviseEnt, ciDeviseEnts, paramValues.GetBooleanValueFromKey(ParamsKeys.BudgetTypeAvancementDynamique));

            budgetT4Ents = budgetT4Manager.GetByBudgetId(budgetId).ToList();
            tacheEnts = tacheManager.GetAllT1ByCiId(budgetEnt.CiId, dateMiseEnApplicationBudget, true).ToList();

            RemplirTaches1To3();

            AssocierTache4();

            var tacheEcart = loadModel.Taches1.FirstOrDefault(t => t.TacheType == (int)TacheType.EcartNiveau1);

            if (tacheEcart != null)
            {
                foreach (var t2 in tacheEcart?.Taches2)
                {
                    t2.Taches3.RemoveAll(t3 => t3.Taches4 == null || t3.Taches4.Count == 0);
                }

                tacheEcart.Taches2.RemoveAll(t2 => t2.Taches3 == null || t2.Taches3.Count == 0);

                loadModel.Taches1.RemoveAll(t1 => taskSearchHelper.IsTacheEcart(new TacheEnt() { TacheType = t1.TacheType }) && t1.Taches2 == null || t1.Taches2.Count == 0);
            }

            return loadModel;
        }

        /// <summary>
        /// Remplit les tâches de niveau 1 à 3 dans le détail d'un budget.
        /// </summary>
        private void RemplirTaches1To3()
        {
            foreach (var tache1Ent in tacheEnts)
            {
                var budgetTacheEnt = GetAndRemoveBudgetTache(tache1Ent);
                var tache1Model = new BudgetDetailLoad.Tache1Model(tache1Ent, budgetTacheEnt);
                if (dateDeleteNotificationNewTask != null && tache1Ent.DateCreation > dateDeleteNotificationNewTask)
                {
                    tache1Model.Warnings = FeatureBudgetDetail.BudgetDetail_NewTaskWarningTitle;
                    loadModel.IsNewTaskNotification = true;
                }
                loadModel.Taches1.Add(tache1Model);
                RemplirTaches2To3(tache1Ent, tache1Model);
            }
        }

        /// <summary>
        /// Remplit les tâches de niveau 2 à 3 dans le détail d'un budget.
        /// </summary>
        /// <param name="tache1Ent">L'entité de la tâche parente de niveau 1.</param>
        /// <param name="tache1Model">Le modèle de la tâche parente de niveau 1.</param>
        private void RemplirTaches2To3(TacheEnt tache1Ent, BudgetDetailLoad.Tache1Model tache1Model)
        {
            if (tache1Ent.TachesEnfants != null)
            {
                foreach (var tache2Ent in tache1Ent.TachesEnfants)
                {
                    var budgetTacheEnt = GetAndRemoveBudgetTache(tache2Ent);
                    var tache2Model = new BudgetDetailLoad.Tache2Model(tache2Ent, budgetTacheEnt);
                    if (dateDeleteNotificationNewTask != null && tache2Ent.DateCreation > dateDeleteNotificationNewTask)
                    {
                        tache2Model.Warnings = FeatureBudgetDetail.BudgetDetail_NewTaskWarningTitle;
                        loadModel.IsNewTaskNotification = true;
                    }
                    tache1Model.Taches2.Add(tache2Model);
                    RemplirTaches3(tache2Ent, tache2Model);
                }
            }
        }

        /// <summary>
        /// Remplit les tâches de niveau 4 dans le détail d'un budget.
        /// </summary>
        /// <param name="tache2Ent">L'entité de la tâche parente de niveau 2.</param>
        /// <param name="tache2Model">Le modèle de la tâche parente de niveau 2.</param>
        private void RemplirTaches3(TacheEnt tache2Ent, BudgetDetailLoad.Tache2Model tache2Model)
        {
            if (tache2Ent.TachesEnfants != null)
            {
                foreach (var tache3Ent in tache2Ent.TachesEnfants)
                {
                    var budgetTacheEnt = GetAndRemoveBudgetTache(tache3Ent);
                    var tache3Model = new BudgetDetailLoad.Tache3Model(tache3Ent, budgetTacheEnt);
                    if (dateDeleteNotificationNewTask != null && tache3Ent.DateCreation > dateDeleteNotificationNewTask)
                    {
                        tache3Model.Warnings = FeatureBudgetDetail.BudgetDetail_NewTaskWarningTitle;
                        loadModel.IsNewTaskNotification = true;
                    }
                    tache2Model.Taches3.Add(tache3Model);
                }
            }
        }

        private void AssocierTache4()
        {
            foreach (var budgetT4Ent in budgetT4Ents)
            {
                var tache4Ent = GetTache4Ent(budgetT4Ent.T4Id);
                var tache4Model = new BudgetDetailLoad.Tache4Model(tache4Ent, budgetT4Ent);
                var tache3Model = GetTache3Model(budgetT4Ent.T3Id.Value);
                tache3Model.Taches4.Add(tache4Model);
            }
        }

        private TacheEnt GetTache4Ent(int tache4Id)
        {
            return tacheEnts
                .Where(t1 => t1.TachesEnfants != null)
                .SelectMany(t1 => t1.TachesEnfants)
                .Where(t2 => t2.TachesEnfants != null)
                .SelectMany(t2 => t2.TachesEnfants)
                .Where(t3 => t3.TachesEnfants != null)
                .SelectMany(t3 => t3.TachesEnfants)
                .FirstOrDefault(t4 => t4.TacheId == tache4Id);
        }

        private BudgetDetailLoad.Tache3Model GetTache3Model(int tache3Id)
        {
            return loadModel.Taches1
                .SelectMany(t1 => t1.Taches2)
                .SelectMany(t2 => t2.Taches3)
                .FirstOrDefault(t3 => t3.TacheId == tache3Id);
        }


        /// <summary>
        /// Récupère un budget tâche pour un tâche spécifiée.
        /// Le supprime également de la liste.
        /// </summary>
        /// <param name="tacheEnt">L'entité concernée.</param>
        /// <returns>Le budget tâche correspondant.</returns>
        private BudgetTacheEnt GetAndRemoveBudgetTache(TacheEnt tacheEnt)
        {
            var ret = budgetTacheEnts.FirstOrDefault(bt => bt.TacheId == tacheEnt.TacheId);
            if (ret != null)
            {
                budgetTacheEnts.Remove(ret);
            }
            return ret;
        }
    }
}
