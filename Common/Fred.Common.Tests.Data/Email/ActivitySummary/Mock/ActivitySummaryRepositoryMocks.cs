using System.Collections.Generic;
using Fred.Entities.ActivitySummary;

namespace Fred.Common.Tests.Data.Email.ActivitySummary.Mock
{
    public static class ActivitySummaryRepositoryMocks
    {

        public static List<ActivityRequestWithCountResult> GetDataForCalculateWorkInProgress()
        {
            return new List<ActivityRequestWithCountResult>()
            {
                new ActivityRequestWithCountResult()
                {
                     RequestName = TypeActivity.AvancementAvalider,
                     CiId = 1,
                     Count = 10

                },
                 new ActivityRequestWithCountResult()
                {
                     RequestName = TypeActivity.BudgetAvalider,
                     CiId = 1,
                     Count = 11

                },
                new ActivityRequestWithCountResult()
                {
                     RequestName = TypeActivity.CommandeAvalider,
                     CiId = 1,
                     Count = 12

                },
                new ActivityRequestWithCountResult()
                {
                     RequestName = TypeActivity.ControleBudgetAvalider,
                     CiId = 1,
                     Count = 13

                },
                new ActivityRequestWithCountResult()
                {
                     RequestName = TypeActivity.RapportsAvalide1,
                     CiId = 1,
                     Count = 14

                }
            };
        }


        public static List<ActivityRequestWithDateResult> GetJalons()
        {
            return new List<ActivityRequestWithDateResult>()
            {
                new ActivityRequestWithDateResult()
                {
                     RequestName = TypeJalon.JalonAvancementValider,
                     CiId = 1,
                     Date = new System.DateTime(2018,02,05)

                },
                 new ActivityRequestWithDateResult()
                {
                     RequestName = TypeJalon.JalonValidationControleBudgetaire,
                     CiId = 1,
                     Date = new System.DateTime(2018,02,05)

                },
                new ActivityRequestWithDateResult()
                {
                     RequestName = TypeJalon.JalonClotureDepense,
                     CiId = 1,
                     Date = new System.DateTime(2018,02,05)

                },
                new ActivityRequestWithDateResult()
                {
                     RequestName = TypeJalon.JalonTransfertFar,
                     CiId = 1,
                     Date = new System.DateTime(2018,02,05)

                }
            };
        }
    }
}
