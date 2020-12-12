using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Notification;
using Fred.Notifications.Interfaces;

namespace Fred.Business.Notification
{
    public class NotificationManager : Manager<NotificationEnt, INotificationRepository>, INotificationManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly INotificationService notificationService;

        public NotificationManager(
            IUnitOfWork uow,
            INotificationRepository notificationRepository,
            IUtilisateurManager userManager,
            INotificationService notificationService)
            : base(uow, notificationRepository)
        {
            this.userManager = userManager;
            this.notificationService = notificationService;
        }

        public async Task<IEnumerable<NotificationEnt>> FindUserNotificationsAsync(int userId, string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return await Repository.FindUserNotificationsAsync(userId);

            return await Repository.FindUserNotificationsAsync(userId, searchText);
        }

        public async Task<IEnumerable<NotificationEnt>> GetUserUnreadNotificationsAsync()
        {
            int userId = userManager.GetContextUtilisateurId();

            return await Repository.FindUserNotificationsAsync(userId);
        }

        public async Task<int> GetUserUnreadNotificationCountAsync(int userId)
        {
            return await Repository.GetUserUnreadNotificationCountAsync(userId);
        }

        public async Task<int> CreateNotificationAsync(int userId, TypeNotification typeNotification, string message)
        {
            var notification = new NotificationEnt
            {
                UtilisateurId = userId,
                TypeNotification = typeNotification,
                Message = message,
                EstConsulte = false,
                DateCreation = DateTime.Now
            };

            await Repository.AddAsync(notification);
            await SaveAsync();
            await NotificationCountChangedAsync(userId);

            return notification.NotificationId;
        }

        public async Task MarkUserNotificationsAsReadAsync(int userId)
        {
            IEnumerable<NotificationEnt> notificationsToUpdate = await Repository.FindUserNotificationsAsync(userId, n => !n.EstConsulte);
            foreach (NotificationEnt notification in notificationsToUpdate)
            {
                notification.EstConsulte = true;
                Repository.Update(notification);
            }

            await SaveAsync();
            await NotificationCountChangedAsync(userId);
        }

        public async Task DeleteUserNotificationsAsync(int userId)
        {
            await Repository.DeleteByUserIdAsync(userId);
            await SaveAsync();
            await NotificationCountChangedAsync(userId);
        }

        public async Task DeleteNotificationsAsync(IEnumerable<NotificationEnt> notifications)
        {
            await Repository.DeleteAsync(notifications);
            await SaveAsync();

            IEnumerable<int> userIds = notifications.Select(t => t.UtilisateurId).Distinct();
            await NotificationCountChangedAsync(userIds);
        }

        private async Task NotificationCountChangedAsync(int userId)
        {
            int notificationCount = await GetUserUnreadNotificationCountAsync(userId);

            notificationService.SendUserUnreadNotificationCount(userId, notificationCount);
        }

        private async Task NotificationCountChangedAsync(IEnumerable<int> userIds)
        {
            Dictionary<int, int> notificationCountByUser = await Repository.GetUsersUnreadNotificationCountAsync(userIds);
            foreach (KeyValuePair<int, int> userNotificationCount in notificationCountByUser)
            {
                int userId = userNotificationCount.Key;
                int notificationCount = userNotificationCount.Value;

                notificationService.SendUserUnreadNotificationCount(userId, notificationCount);
            }
        }
    }
}