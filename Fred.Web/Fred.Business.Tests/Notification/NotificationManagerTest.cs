using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.Notification;
using Fred.Business.Utilisateur;
using Fred.Common.Tests.Data.Notification.Builder;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Notification;
using Fred.Notifications.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Notification
{
    [TestClass]
    public class NotificationManagerTest
    {
        private static NotificationManager manager;

        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<INotificationRepository> notificationRepository;
        private static Mock<IUtilisateurManager> userManager;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            notificationRepository = new Mock<INotificationRepository>();
            userManager = new Mock<IUtilisateurManager>();
            var notificationService = new Mock<INotificationService>();
            unitOfWork = new Mock<IUnitOfWork>();

            manager = new NotificationManager(unitOfWork.Object, notificationRepository.Object, userManager.Object, notificationService.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            notificationRepository.Reset();
        }

        /// <summary>
        /// Teste la récupération des nouvelles notifications d'un utilisateur
        /// </summary>
        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("NotificationManager")]
        public async Task GetCurrentUserUnreadNotifications_Returns_Notifications_For_The_Current_User()
        {
            IEnumerable<NotificationEnt> unreadNotifications = NotificationDataMocks.User1Notifications.Where(n => !n.EstConsulte);
            userManager.Setup(u => u.GetContextUtilisateurId()).Returns(1);
            //HACK: remplacer le It.IsAny par la vraie fonction
            notificationRepository.Setup(notif => notif.FindUserNotificationsAsync(1, It.IsAny<Expression<Func<NotificationEnt, bool>>>()))
                                                  .Returns(Task.FromResult(unreadNotifications));

            IEnumerable<NotificationEnt> unreadUser1Notifications = await manager.GetUserUnreadNotificationsAsync();

            Assert.AreEqual(unreadNotifications, unreadUser1Notifications);
        }
    }
}