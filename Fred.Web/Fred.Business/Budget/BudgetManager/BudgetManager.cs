using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Fred.Business.Budget.Helpers;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Recette;
using Fred.Framework.Exceptions;
using Fred.Web.Models.Budget.Liste;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.Liste;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget.BudgetManager
{
    /// <summary>
    /// Implémentation de l'interface IBudgetManager
    /// </summary>
    public partial class BudgetManager : Manager<BudgetEnt, IBudgetRepository>, IBudgetManager
    {
        private readonly IUnitOfWork uow;
        private readonly IBudgetEtatManager budgetEtatManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IBudgetWorkflowManager budgetWorkflowManager;
        private readonly IBudgetRepository budgetRepository;

        public BudgetManager(
            IUnitOfWork uow,
            IBudgetRepository budgetRepository,
            IBudgetEtatManager budgetEtatManager,
            IBudgetT4Manager budgetT4Manager,
            IUtilisateurManager utilisateurManager,
            IBudgetWorkflowManager budgetWorkflowManager)
            : base(uow, budgetRepository)
        {
            this.uow = uow;
            this.budgetEtatManager = budgetEtatManager;
            this.budgetT4Manager = budgetT4Manager;
            this.utilisateurManager = utilisateurManager;
            this.budgetWorkflowManager = budgetWorkflowManager;
            this.budgetRepository = budgetRepository;
        }

        /// <inheritdoc/>
        public BudgetSuppressionSuccessModel SupprimeBudget(int budgetId)
        {
            var budget = FindById(budgetId);
            var budgetBrouillonCode = budgetEtatManager.GetByCode(EtatBudget.Brouillon).BudgetEtatId;

            if (budget.BudgetEtatId == budgetBrouillonCode)
            {
                budget.DateSuppressionBudget = DateTime.Today;
                Repository.Update(budget);
                Save();

                return new BudgetSuppressionSuccessModel() { DateSuppression = budget.DateSuppressionBudget.Value };
            }

            //Si on arrive ici c'est que le budget n'est pas à l'état brouillon donc ne peut pas être supprimé
            throw new FredBusinessMessageResponseException(FeatureBudgetListe.BudgetSuppressionBudgetNonBrouillon);
        }

        /// <inheritdoc/>
        public bool RestaurerBudget(int budgetId)
        {
            var budget = FindById(budgetId);
            budget.DateSuppressionBudget = null;
            Repository.Update(budget);
            Save();

            return true;
        }

        /// <inheritdoc/>
        public BudgetEnt CreateEmptyBudgetSurCi(int ciId, int utilisateurConnecteId)
        {
            var etatBrouillon = budgetEtatManager.GetByCode(EtatBudget.Brouillon);
            var nouvelleVersion = GetCiMaxVersion(ciId);
            nouvelleVersion = VersionHelper.IncrementVersionMineur(nouvelleVersion);

            //Copie du budget en lui même
            var nouveauBudget = new BudgetEnt
            {
                BudgetEtatId = etatBrouillon.BudgetEtatId,
                CiId = ciId,
                DeviseId = 48,
                Version = nouvelleVersion,
                Partage = false
            };

            //Création d'un nouveau workflow pour le budget ne contenant qu'une entrée à l'état brouillon.
            nouveauBudget.Workflows = new List<BudgetWorkflowEnt>()
            {
                new BudgetWorkflowEnt()
                {
                    AuteurId = utilisateurConnecteId,
                    Budget = nouveauBudget,
                    EtatInitialId = null,
                    EtatCibleId = etatBrouillon.BudgetEtatId,
                    Date = DateTime.Now
                }
            };

            var budgetInsere = Create(nouveauBudget);
            //On rappelle la fonction get pour obtenir les valeurs des propriétés de navigation (comme les informations du personnel dans les workflow)
            budgetInsere = GetBudget(budgetInsere.BudgetId);
            return budgetInsere;
        }

        /// <inheritdoc/>
        public bool PartageOuPrivatiseBudget(int budgetId)
        {
            var budgetAModifier = GetBudget(budgetId);
            budgetAModifier.Partage = !budgetAModifier.Partage;
            Update(budgetAModifier);

            return budgetAModifier.Partage;
        }

        /// <inheritdoc/>
        public BudgetEnt SaveBudgetChangeInListView(ListeBudgetModel budgetListeModel)
        {
            //D'abord on récupère le budget que l'on modifie
            var budgetAModifier = GetBudget(budgetListeModel.BudgetId);

            //Il est possible si le budget est nouveau que ses recettes soient null dans ce cas la on les créé
            if (budgetAModifier.Recette == null)
            {
                budgetAModifier.Recette = new BudgetRecetteEnt();
            }

            //Ensuite on met à jour les recettes du budget
            budgetAModifier.Recette.AutresRecettes = budgetListeModel.Recettes.AutresRecettes;
            budgetAModifier.Recette.MontantAvenants = budgetListeModel.Recettes.MontantAvenants;
            budgetAModifier.Recette.MontantMarche = budgetListeModel.Recettes.MontantMarche;
            budgetAModifier.Recette.PenalitesEtRetenues = budgetListeModel.Recettes.PenalitesEtRetenues;
            budgetAModifier.Recette.Revision = budgetListeModel.Recettes.Revision;
            budgetAModifier.Recette.SommeAValoir = budgetListeModel.Recettes.SommeAValoir;
            budgetAModifier.Recette.TravauxSupplementaires = budgetListeModel.Recettes.TravauxSupplementaires;

            //Enfin on met à jour le commentaire
            var workflowRecent = budgetAModifier.Workflows.OrderByDescending(w => w.Date).First();
            workflowRecent.Commentaire = budgetListeModel.Commentaire;

            return Update(budgetAModifier);
        }

        /// <inheritdoc/>
        public void BudgetChangeEtat(BudgetEnt budget, int etatCibleId, int utilisateurId, string commentaire)
        {
            budgetWorkflowManager.Add(budget, etatCibleId, utilisateurId, commentaire);
            budget.BudgetEtatId = etatCibleId;
            Update(budget);
        }

        /// <summary>
        /// Retourne les révisions de budget d'un CI accessible à l'utilisateur connecté.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI concerné.</param>
        /// <returns>Les révisions de budget du CI.</returns>
        public IEnumerable<BudgetRevisionLoadModel> GetBudgetRevisions(int ciId)
        {
            var utilisateur = utilisateurManager.GetContextUtilisateur();
            return Repository.GetBudgetRevisions(ciId, utilisateur.UtilisateurId);
        }

        /// <summary>
        /// Met à jour la date de suppression de la notification des nouvelles tâches
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        public void UpdateDateDeleteNotificationNewTask(int budgetId)
        {
            var budget = GetBudget(budgetId);
            if (budget != null)
            {
                budget.DateDeleteNotificationNewTask = DateTime.UtcNow;
            }
            Update(budget);
        }

        /// <inheritdoc/>
        public DateTime? GetDateMiseEnApplicationBudgetSurCi(int ciId)
        {
            var budget = GetBudgetEnApplication(ciId);
            PerformEagerLoading(budget, b => b.Workflows);

            var etatEnApplication = budgetEtatManager.GetByCode(EtatBudget.EnApplication);
            var date = budget.Workflows.Where(b => b.EtatCibleId == etatEnApplication.BudgetEtatId).Select(b => b.Date);
            if (date.Any())
            {
                return date.First();
            }

            return null;
        }

        /// <inheritdoc/>
        public IEnumerable<BudgetVersionAuteurModel> GetBudgetBrouillonDuBudgetEnApplication(int ciId, int deviseId)
        {
            var versionMajeure = Repository
                .Get()
                .Where(b => b.CiId == ciId && b.DeviseId == deviseId && b.BudgetEtat.Code == EtatBudget.EnApplication)
                .Select(b => b.Version)
                .FirstOrDefault();

            if (versionMajeure == null)
            {
                //Il est tout a fait possible qu'il n'y ait aucun budget en application, si c'est le cas la version majeure est à mettre a 0
                versionMajeure = "0";
            }
            else
            {
                versionMajeure = versionMajeure.Split('.').First();
            }

            return Repository.Get()
                .Where(b => b.CiId == ciId
                            && b.DeviseId == deviseId
                            && b.BudgetEtat.Code == EtatBudget.Brouillon
                            && b.Version.StartsWith(versionMajeure)
                            && b.DateSuppressionBudget == null)
                .Select(BudgetVersionAuteurModel.Selector);
        }

        /// <inheritdoc/>
        public IEnumerable<BudgetVersionAuteurModel> GetBudgetsBrouillons(int ciId, int deviseId)
        {
            return Repository.Get()
                .Where(b => b.CiId == ciId
                            && b.DeviseId == deviseId
                            && b.BudgetEtat.Code == EtatBudget.Brouillon
                            && b.DateSuppressionBudget == null)
                .Select(BudgetVersionAuteurModel.Selector);
        }
    }
}
