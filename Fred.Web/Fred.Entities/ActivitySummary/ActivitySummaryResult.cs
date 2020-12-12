using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fred.Entities.ActivitySummary
{
    /// <summary>
    /// Liste les activité en cours pour un utilisateur
    /// </summary>
    [DebuggerDisplay("UsersActivities = {UsersActivities.Count}")]
    public class ActivitySummaryResult
    {
        /// <summary>
        /// Activites en cours pour des utilisateurs
        /// </summary>
        public List<UserActivitySummary> UsersActivities { get; set; } = new List<UserActivitySummary>();

        /// <summary>
        /// Liste les jalons pour tous des utilisateurs
        /// </summary>
        public List<UserJalonSummary> UsersCiJalons { get; set; } = new List<UserJalonSummary>();

        /// <summary>
        /// Id de l'utilisateur
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Permet de savoir si l'utilisateur a des activitées en cours
        /// </summary>
        public bool HasActivitiesInProgress
        {
            get
            {
                return UsersActivities.Any();
            }
        }


        /// <summary>
        /// TotalCommandeAValider
        /// </summary>
        public int TotalCommandeAValider { get; set; }

        /// <summary>
        /// Couleur de fond de l'affichage du total des commandes à valider
        /// </summary>
        public ColorActivity ColorTotalCommandeAValider { get; set; }

        /// <summary>
        /// TotalRapportsAvalide1
        /// </summary>
        public int TotalRapportsAvalide1 { get; set; }

        /// <summary>
        /// Couleur de fond de l'affichage du total des rapports à valider
        /// </summary>
        public ColorActivity ColorTotalRapportsAvalide1 { get; set; }

        /// <summary>
        /// TotalReceptionsAviser
        /// </summary>
        public int TotalReceptionsAviser { get; set; }

        /// <summary>
        /// Couleur de fond de l'affichage du total des réceptions à viser
        /// </summary>
        public ColorActivity ColorTotalReceptionsAviser { get; set; }

        /// <summary>
        /// TotalBudgetAvalider
        /// </summary>
        public int TotalBudgetAvalider { get; set; }

        /// <summary>
        /// Couleur de fond de l'affichage du total des budgets à valider
        /// </summary>
        public ColorActivity ColorTotalBudgetAvalider { get; set; }

        /// <summary>
        /// TotalAvancementAvalider
        /// </summary>
        public int TotalAvancementAvalider { get; set; }

        /// <summary>
        /// Couleur de fond de l'affichage du total des avancements à valider
        /// </summary>
        public ColorActivity ColorTotalAvancementAvalider { get; set; }

        /// <summary>
        /// TotalControleBudgetaireAvalider
        /// </summary>
        public int TotalControleBudgetaireAvalider { get; set; }

        /// <summary>
        /// Couleur de fond de l'affichage du total des contrôles budgétaires à valider
        /// </summary>
        public ColorActivity ColorTotalControleBudgetaireAvalider { get; set; }
    }
}
