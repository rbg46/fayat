using System.Threading.Tasks;
using Fred.DataAccess.ExternalService.FredImportExport.Notification;

namespace Fred.Business.ExternalService.Notification
{
    public class NotificationManagerExterne : INotificationManagerExterne
    {
        private readonly INotificationRepositoryExterne notificationRepositoryExterne;

        public NotificationManagerExterne(INotificationRepositoryExterne notificationRepositoryExterne)
        {
            this.notificationRepositoryExterne = notificationRepositoryExterne;
        }

        public async Task SubscribeToUserNotificationsAsync(int userId)
        {
            await notificationRepositoryExterne.SubscribeToUserNotificationsAsync(userId);
        }
    }
}
