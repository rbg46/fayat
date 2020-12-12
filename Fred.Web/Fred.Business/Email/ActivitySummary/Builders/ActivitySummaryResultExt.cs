using System.Linq;
using Fred.Entities.ActivitySummary;

namespace Fred.Business.Email.ActivitySummary.Builders
{
    /// <summary>
    /// Extension de ActivitySummaryResult
    /// </summary>
    public static class ActivitySummaryResultExt
    {
        /// <summary>
        /// Calcule les totaux de l'ActivitySummaryResult, et la couleur de fond associée à chaque total
        /// </summary>
        /// <param name="activitySummaryResult">L'ActivitySummaryResult pour laquelle on va calculer les totaux</param>
        public static void ComputeTotalsAndTotalsColors(this ActivitySummaryResult activitySummaryResult)
        {
            // Calcul des totaux

            activitySummaryResult.TotalCommandeAValider = activitySummaryResult.UsersActivities
                .Where(u => u.NombreCommandeAValider.HasValue).Sum(u => u.NombreCommandeAValider.Value);

            activitySummaryResult.TotalRapportsAvalide1 = activitySummaryResult.UsersActivities
                .Where(u => u.NombreRapportsAvalide1.HasValue).Sum(u => u.NombreRapportsAvalide1.Value);

            activitySummaryResult.TotalReceptionsAviser = activitySummaryResult.UsersActivities
                .Where(u => u.NombreReceptionsAviser.HasValue).Sum(u => u.NombreReceptionsAviser.Value);

            activitySummaryResult.TotalBudgetAvalider = activitySummaryResult.UsersActivities
                .Where(u => u.NombreBudgetAvalider.HasValue).Sum(u => u.NombreBudgetAvalider.Value);

            activitySummaryResult.TotalAvancementAvalider = activitySummaryResult.UsersActivities
                .Where(u => u.NombreAvancementAvalider.HasValue).Sum(u => u.NombreAvancementAvalider.Value);

            activitySummaryResult.TotalControleBudgetaireAvalider = activitySummaryResult.UsersActivities
                .Where(u => u.NombreControleBudgetaireAvalider.HasValue).Sum(u => u.NombreControleBudgetaireAvalider.Value);

            // Calcul des couleurs des totaux

            UserActivityColorProvider userActivityColorProvider = new UserActivityColorProvider();
            activitySummaryResult.ColorTotalCommandeAValider = userActivityColorProvider.GetActivityColor(activitySummaryResult.TotalCommandeAValider);
            activitySummaryResult.ColorTotalRapportsAvalide1 = userActivityColorProvider.GetActivityColor(activitySummaryResult.TotalRapportsAvalide1);
            activitySummaryResult.ColorTotalReceptionsAviser = userActivityColorProvider.GetActivityColor(activitySummaryResult.TotalReceptionsAviser);
            activitySummaryResult.ColorTotalBudgetAvalider = userActivityColorProvider.GetActivityColor(activitySummaryResult.TotalBudgetAvalider);
            activitySummaryResult.ColorTotalAvancementAvalider = userActivityColorProvider.GetActivityColor(activitySummaryResult.TotalAvancementAvalider);
            activitySummaryResult.ColorTotalControleBudgetaireAvalider = userActivityColorProvider.GetActivityColor(activitySummaryResult.TotalControleBudgetaireAvalider);
        }
    }
}
