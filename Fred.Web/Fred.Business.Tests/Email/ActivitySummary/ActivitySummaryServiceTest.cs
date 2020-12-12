using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Email.ActivitySummary;
using Fred.Business.Email.Subscription;
using Fred.Business.Organisation.Tree;
using Fred.Common.Tests.Data.Email.ActivitySummary.Mock;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ActivitySummary;
using Fred.Entities.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Email.ActivitySummary
{

    [TestClass]
    public class ActivitySummaryServiceTestServiceTest
    {

        ActivitySummaryService activitySummaryService;

        private Mock<IActivitySummaryRepository> activitySummaryRepositoryMock;

        private Mock<IOrganisationTreeService> organisationTreeServiceMock;

        private Mock<IEmailSubscriptionManager> emailSubscriptionManagerMock;
        private Entities.Organisation.Tree.OrganisationTree organisationTree;

        private List<int> subscribers = new List<int>();

        private int userIdOne = 1;

        [TestInitialize]
        public void TestInitialize()
        {
            this.organisationTreeServiceMock = new Mock<IOrganisationTreeService>();

            this.emailSubscriptionManagerMock = new Mock<IEmailSubscriptionManager>();

            this.activitySummaryRepositoryMock = new Mock<IActivitySummaryRepository>();

            activitySummaryService = new ActivitySummaryService(
                activitySummaryRepositoryMock.Object,
                organisationTreeServiceMock.Object,
                emailSubscriptionManagerMock.Object);

            organisationTree = OrganisationTreeServiceMocks.GetMinimalOrganisationTree();

            subscribers.Add(userIdOne);

            this.organisationTreeServiceMock.Setup(otm => otm.GetOrganisationTree(It.IsAny<bool>()))
                                         .Returns(organisationTree);

            this.emailSubscriptionManagerMock.Setup(otm => otm.GetSubscribersToMaillingList(It.IsAny<EmailSouscriptionKey>(), It.IsAny<DateTime>()))
                                        .Returns(subscribers);

            this.activitySummaryRepositoryMock.Setup(m => m.GetDataForCalculateWorkInProgress())
                                    .Returns(new List<ActivityRequestWithCountResult>());

            this.activitySummaryRepositoryMock.Setup(m => m.GetCiActifs())
                                    .Returns(new List<int>() { 1, 2 });
            this.activitySummaryRepositoryMock.Setup(m => m.GetCiActifs(It.IsAny<bool>()))
                                    .Returns(new List<int>() { 1, 2 });

            this.activitySummaryRepositoryMock.Setup(m => m.GetJalons())
                                   .Returns(new List<ActivityRequestWithDateResult>());
        }



        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("ActivitySummaryService")]
        public void Verifie_Que_il_y_a_une_UserActivitySummary()
        {

            this.activitySummaryRepositoryMock.Setup(m => m.GetDataForCalculateWorkInProgress())
                                      .Returns(new List<ActivityRequestWithCountResult>()
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

                                                    }
                                                 });



            var activitySummaryResult = activitySummaryService.CalculateActivitiesAndJalons(subscribers, organisationTree).FirstOrDefault();

            Assert.IsTrue(activitySummaryResult.UsersActivities.Count == 1, "Il faut 1 lignes pour l'utilsateur 1 et pour le c1 1.");

        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("ActivitySummaryService")]
        public void Verifie_Que_il_y_a_deux_UserActivitySummary_pour_2_donnes_de_type_count()
        {

            this.activitySummaryRepositoryMock.Setup(m => m.GetDataForCalculateWorkInProgress())
                                      .Returns(new List<ActivityRequestWithCountResult>()
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
                                                            CiId = 2,
                                                            Count = 11

                                                    }
                                                 });


            var activitySummaryResult = activitySummaryService.CalculateActivitiesAndJalons(subscribers, organisationTree).FirstOrDefault();

            Assert.IsTrue(activitySummaryResult.UsersActivities.Count == 2, "Il faut 2 activites pour l'utilsateur 1 et pour le ci(1) et le ci(2).");

        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("ActivitySummaryService")]
        public void Verifie_Que_il_y_a_2_UserJalonSummary_car_2_ci()
        {
            this.activitySummaryRepositoryMock.Setup(m => m.GetJalons())
                                  .Returns(new List<ActivityRequestWithDateResult>()
            {
                new ActivityRequestWithDateResult()
                {
                     RequestName = TypeJalon.JalonClotureDepense,
                     CiId = 1,
                     Date = new System.DateTime(2018,02,05)

                }
              });

            var activitySummaryResult = activitySummaryService.CalculateActivitiesAndJalons(subscribers, organisationTree).FirstOrDefault();

            Assert.IsTrue(activitySummaryResult.UsersCiJalons.Count == 2, "Il faut 2 jalon pour l'utilsateur 1 car il y a 2 ci.");

        }


        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("ActivitySummaryService")]
        public void Verifie_Que_il_y_a_deux_UserJalonSummar_car_l_utilisateur_a_2_cis_dans_son_perimetres_d_habilitations()
        {

            this.activitySummaryRepositoryMock.Setup(m => m.GetJalons())
                                  .Returns(new List<ActivityRequestWithDateResult>()
            {
                new ActivityRequestWithDateResult()
                {
                     RequestName = TypeJalon.JalonAvancementValider,
                     CiId = 1,
                     Date = new System.DateTime(2018,02,05)

                },
                 new ActivityRequestWithDateResult()
                {
                     RequestName = TypeJalon.JalonClotureDepense,
                     CiId = 1,
                     Date = new System.DateTime(2018,02,05)

                }
              });

            var activitySummaryResult = activitySummaryService.CalculateActivitiesAndJalons(subscribers, organisationTree).FirstOrDefault();

            Assert.IsTrue(activitySummaryResult.UsersCiJalons.Count == 2, "Il faut 2 jalon pour l'utilsateur 1 et pour le ci(1) et le ci(2).");

        }


        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("ActivitySummaryService")]
        public void Verifie_Que_il_y_a_deux_jalons_et_que_le_jalon_cicloturer_a_le_plus_anciens_des_jalons_sur_un_meme_ci()
        {
            var dateJalonAvancementValider = new System.DateTime(2018, 02, 05);

            var dateJalonTransfertFar = new System.DateTime(2017, 02, 05);

            this.activitySummaryRepositoryMock.Setup(m => m.GetJalons())
                                  .Returns(new List<ActivityRequestWithDateResult>()
            {
                new ActivityRequestWithDateResult()
                {
                     RequestName = TypeJalon.JalonAvancementValider,
                     CiId = 1,
                     Date = dateJalonAvancementValider

                },
                 new ActivityRequestWithDateResult()
                {
                     RequestName = TypeJalon.JalonTransfertFar,
                     CiId = 1,
                     Date = dateJalonTransfertFar

                }
              });

            var activitySummaryResult = activitySummaryService.CalculateActivitiesAndJalons(subscribers, organisationTree).FirstOrDefault();

            var usersCiJalons = activitySummaryResult.UsersCiJalons;
            var firstUserCiJalon = usersCiJalons.FirstOrDefault();
            var flastUserCiJalon = usersCiJalons.LastOrDefault();
            Assert.AreEqual(dateJalonTransfertFar, firstUserCiJalon.JalonCiCloturer, "La date la plus ancienne devrait etre la date du jalon TransfertFar");
            Assert.AreEqual(2, activitySummaryResult.UsersCiJalons.Count, "Il faut 2 jalons pour l'utilsateur 1 et pour le ci(1) et le ci(2).");
        }

    }
}
