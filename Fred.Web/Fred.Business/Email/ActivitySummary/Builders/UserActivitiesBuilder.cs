using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Entities.ActivitySummary;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.Email.ActivitySummary.Builders
{
    /// <summary>
    /// Construit les activité en cours 
    /// </summary>
    public class UserActivitiesBuilder
    {
        /// <summary>
        ///  Construit les activité en cours
        /// </summary>
        /// <param name="mainOrganisationTree">L'abre de fred</param>
        /// <param name="subscribersIds">Les ids des personnels qui ont souscrit au mail recap</param>
        /// <param name="countData">Les données de type Nombre</param>
        /// <returns>Liste de UserActivitySummary</returns>
        public List<UserActivitySummary> BuildUserActivitySummaries(OrganisationTree mainOrganisationTree, List<int> subscribersIds, List<ActivityRequestWithCountResult> countData)
        {
            var result = new List<UserActivitySummary>();

            foreach (var userId in subscribersIds)
            {
                List<OrganisationBase> ciOrganisationBases = mainOrganisationTree.GetAllCiOrganisationBaseForUser(userId);

                result.AddRange(BuildUserActivity(userId, countData, ciOrganisationBases));

            }
            UpdateActivitiesColors(result);

            return result;
        }

        private List<UserActivitySummary> BuildUserActivity(int userId, List<ActivityRequestWithCountResult> countData, List<OrganisationBase> ciOrganisationBases)
        {

            var result = new List<UserActivitySummary>();

            foreach (var ciOrganisationBase in ciOrganisationBases)
            {
                var infoTypeCountForCiAndUser = countData.Where(asrr => asrr.CiId == ciOrganisationBase.Id).ToList();

                var userActivity = new UserActivitySummary()
                {
                    UserId = userId,
                    CiId = ciOrganisationBase.Id,
                    Labelle = ciOrganisationBase.Code + " - " + ciOrganisationBase.Libelle,
                    NombreCommandeAValider = infoTypeCountForCiAndUser.FirstOrDefault(i => i.RequestName == TypeActivity.CommandeAvalider)?.Count,
                    NombreRapportsAvalide1 = infoTypeCountForCiAndUser.FirstOrDefault(i => i.RequestName == TypeActivity.RapportsAvalide1)?.Count,
                    NombreReceptionsAviser = infoTypeCountForCiAndUser.FirstOrDefault(i => i.RequestName == TypeActivity.ReceptionsAviser)?.Count,
                    NombreBudgetAvalider = infoTypeCountForCiAndUser.FirstOrDefault(i => i.RequestName == TypeActivity.BudgetAvalider)?.Count,
                    NombreAvancementAvalider = infoTypeCountForCiAndUser.FirstOrDefault(i => i.RequestName == TypeActivity.AvancementAvalider)?.Count,
                    NombreControleBudgetaireAvalider = infoTypeCountForCiAndUser.FirstOrDefault(i => i.RequestName == TypeActivity.ControleBudgetAvalider)?.Count,
                };
                if (FilterActivityDetected(userActivity))
                {
                    result.Add(userActivity);
                }

            }
            return result;
        }


        private bool FilterActivityDetected(UserActivitySummary userActivitySummary)
        {
            return userActivitySummary?.NombreBudgetAvalider > 0
                || userActivitySummary?.NombreCommandeAValider > 0
                || userActivitySummary?.NombreRapportsAvalide1 > 0
                || userActivitySummary?.NombreReceptionsAviser > 0
                || userActivitySummary?.NombreAvancementAvalider > 0;
        }



        private void UpdateActivitiesColors(List<UserActivitySummary> userActivitySummaries)
        {
            var userActivityColorProvider = new UserActivityColorProvider();

            foreach (var userActivitySummary in userActivitySummaries)
            {
                userActivitySummary.ColorAvancementAvalider = userActivityColorProvider.GetActivityColor(userActivitySummary.NombreAvancementAvalider);
                userActivitySummary.ColorBudgetAvalider = userActivityColorProvider.GetActivityColor(userActivitySummary.NombreBudgetAvalider);
                userActivitySummary.ColorCommandeAValider = userActivityColorProvider.GetActivityColor(userActivitySummary.NombreCommandeAValider);
                userActivitySummary.ColorRapportsAvalide1 = userActivityColorProvider.GetActivityColor(userActivitySummary.NombreRapportsAvalide1);
                userActivitySummary.ColorReceptionsAviser = userActivityColorProvider.GetActivityColor(userActivitySummary.NombreReceptionsAviser);
                userActivitySummary.ColorControleBudgetaireAvalider = userActivityColorProvider.GetActivityColor(userActivitySummary.NombreControleBudgetaireAvalider);

            }
        }
    }
}
