using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Entities.ActivitySummary;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.Email.ActivitySummary.Builders
{
    /// <summary>
    /// Construit les jalons
    /// </summary>
    public class JalonsBuilder
    {

        /// <summary>
        /// Retourne une liste d'UserJalonSummary, classe qui contient les dates de jalons
        /// </summary>
        /// <param name="mainOrganisationTree">Organisation tree de fred</param>
        /// <param name="subscribersIds">ids des personnel pour lequel on demande la contrustion des jalons</param>
        /// <param name="jalonsData">les jalons</param>
        /// <returns>liste UserJalonSummary</returns>
        public List<UserJalonSummary> BuildJalons(OrganisationTree mainOrganisationTree, List<int> subscribersIds, List<ActivityRequestWithDateResult> jalonsData)
        {
            var result = new List<UserJalonSummary>();

            foreach (var userId in subscribersIds)
            {
                List<OrganisationBase> ciIdsOfUser = mainOrganisationTree.GetAllCiOrganisationBaseForUser(userId);

                result.AddRange(BuildUserJalons(userId, jalonsData, ciIdsOfUser));
            }

            UpdateJalonsColors(result);

            return result;
        }

        private List<UserJalonSummary> BuildUserJalons(int userId, List<ActivityRequestWithDateResult> jalonsData, List<OrganisationBase> ciOrganisationBasesOfUser)
        {

            var result = new List<UserJalonSummary>();

            foreach (var ciOrganisationBaseOfUser in ciOrganisationBasesOfUser)
            {
                var infoTypeDateForCiAndUser = jalonsData.Where(j => j.CiId == ciOrganisationBaseOfUser.Id).ToList();

                var jalon = new UserJalonSummary()
                {
                    UserId = userId,
                    CiId = ciOrganisationBaseOfUser.Id,
                    Labelle = ciOrganisationBaseOfUser.Code + " " + ciOrganisationBaseOfUser.Libelle,
                    JalonTransfertFar = infoTypeDateForCiAndUser.FirstOrDefault(j => j.RequestName == TypeJalon.JalonTransfertFar)?.Date,
                    JalonAvancementvalider = infoTypeDateForCiAndUser.FirstOrDefault(j => j.RequestName == TypeJalon.JalonAvancementValider)?.Date,
                    JalonClotureDepense = infoTypeDateForCiAndUser.FirstOrDefault(j => j.RequestName == TypeJalon.JalonClotureDepense)?.Date,
                    JalonValidationControleBudgetaire = infoTypeDateForCiAndUser.FirstOrDefault(j => j.RequestName == TypeJalon.JalonValidationControleBudgetaire)?.Date
                };

                jalon.JalonCiCloturer = GetTheOldestDate(jalon);

                result.Add(jalon);
            }
            return result;
        }


        private DateTime? GetTheOldestDate(UserJalonSummary userJalonSummary)
        {
            var allDates = new List<DateTime?>()
            {
                userJalonSummary.JalonAvancementvalider,
                userJalonSummary.JalonClotureDepense,
                userJalonSummary.JalonTransfertFar,
                userJalonSummary.JalonValidationControleBudgetaire
            };

            var oldestDate = allDates.Where(d => d.HasValue).OrderBy(d => d).FirstOrDefault();

            return oldestDate;
        }


        private void UpdateJalonsColors(List<UserJalonSummary> userJalonSummaries)
        {
            var jalonColorProvider = new JalonColorProvider();

            var now = DateTime.UtcNow;

            foreach (var userJalonSummary in userJalonSummaries)
            {
                userJalonSummary.ColorJalonAvancementvalider = jalonColorProvider.GetJalonColor(now, userJalonSummary.JalonAvancementvalider);
                userJalonSummary.ColorJalonCiCloturer = jalonColorProvider.GetJalonColor(now, userJalonSummary.JalonCiCloturer);
                userJalonSummary.ColorJalonClotureDepense = jalonColorProvider.GetJalonColor(now, userJalonSummary.JalonClotureDepense);
                userJalonSummary.ColorJalonValidationControleBudgetaire = jalonColorProvider.GetJalonColor(now, userJalonSummary.JalonValidationControleBudgetaire);
                userJalonSummary.ColorJalonTransfertFar = jalonColorProvider.GetJalonColor(now, userJalonSummary.JalonTransfertFar);
            }
        }
    }
}
