using System.Threading.Tasks;

namespace Fred.Business.ExternalService.Notification
{
    public interface INotificationManagerExterne
    {
        Task SubscribeToUserNotificationsAsync(int userId);
    }
}