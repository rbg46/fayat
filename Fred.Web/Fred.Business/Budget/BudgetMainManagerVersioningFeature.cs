using Fred.Business.Budget.Helpers;
using Fred.Entities.Budget;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Feature gérant les différentes actions de versioning d'un budget
    /// </summary>
    public class BudgetMainManagerVersioningFeature : ManagersAccess
    {
        private readonly IBudgetEtatManager budgetEtatManager;
        private readonly IBudgetManager budgetManager;

        public BudgetMainManagerVersioningFeature(IBudgetEtatManager budgetEtatManager, IBudgetManager budgetManager)
        {
            this.budgetEtatManager = budgetEtatManager;
            this.budgetManager = budgetManager;
        }
        /// <summary>
        /// Change l'état d'un budget brouillon vers à valider
        /// </summary>
        /// <param name="budget">Le budget</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <param name="commentaire">le commentaire</param>
        public void BudgetBrouillonToAValider(BudgetEnt budget, int utilisateurId, string commentaire)
        {
            var etatCibleId = budgetEtatManager.GetByCode(EtatBudget.AValider).BudgetEtatId;
            budgetManager.BudgetChangeEtat(budget, etatCibleId, utilisateurId, commentaire);
        }

        /// <summary>
        /// Change l'état d'un budget brouillon vers à valider
        /// </summary>
        /// <param name="budget">Le budget</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <param name="commentaire">le commentaire</param>
        public void BudgetAValiderToBrouillon(BudgetEnt budget, int utilisateurId, string commentaire)
        {
            var etatCibleId = budgetEtatManager.GetByCode(EtatBudget.Brouillon).BudgetEtatId;
            budgetManager.BudgetChangeEtat(budget, etatCibleId, utilisateurId, commentaire);
        }

        /// <summary>
        /// Change l'état d'un budget à valider vers en apllication
        /// </summary>
        /// <param name="budget">Le budget</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <param name="commentaire">le commentaire</param>
        public void BudgetAValiderToEnApplication(BudgetEnt budget, int utilisateurId, string commentaire)
        {
            var version = budget.Version;
            var budgetEnApplication = budgetManager.GetBudgetEnApplication(budget.CiId);

            if (budgetEnApplication != null)
            {
                version = budgetEnApplication.Version;
                BudgetEnApplicationToArchive(budgetEnApplication, utilisateurId, "Archivage automatique suite à une monté de version du budget");
            }
            var etatCibleId = budgetEtatManager.GetByCode(EtatBudget.EnApplication).BudgetEtatId;
            budget.Version = VersionHelper.IncrementVersionMajeure(version);
            budget.PeriodeDebut = PeriodeHelper.GetPeriode();
            budgetManager.BudgetChangeEtat(budget, etatCibleId, utilisateurId, commentaire);
        }

        /// <summary>
        /// Change l'état d'un budget brouillon vers Archivé
        /// </summary>
        /// <param name="budget">Le budget</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <param name="commentaire">le commentaire</param>
        public void BudgetEnApplicationToArchive(BudgetEnt budget, int utilisateurId, string commentaire)
        {
            var etatCibleId = budgetEtatManager.GetByCode(EtatBudget.Archive).BudgetEtatId;
            var periode = PeriodeHelper.GetPeriode();
            if (budget.PeriodeDebut == periode)
            {
                budget.PeriodeFin = budget.PeriodeDebut;
            }
            else
            {
                budget.PeriodeFin = PeriodeHelper.GetPreviousPeriod(periode).Value;
            }
            budgetManager.BudgetChangeEtat(budget, etatCibleId, utilisateurId, commentaire);

        }
    }
}
