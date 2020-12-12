using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Notification;

namespace Fred.Business.Notification
{
    public interface INotificationManager : IManager<NotificationEnt>
    {
        Task<IEnumerable<NotificationEnt>> FindUserNotificationsAsync(int userId, string searchText);

        Task<IEnumerable<NotificationEnt>> GetUserUnreadNotificationsAsync();

        Task<int> GetUserUnreadNotificationCountAsync(int userId);

        Task<int> CreateNotificationAsync(int userId, TypeNotification typeNotification, string message);

        Task MarkUserNotificationsAsReadAsync(int userId);

        Task DeleteUserNotificationsAsync(int userId);

        Task DeleteNotificationsAsync(IEnumerable<NotificationEnt> notifications);
    }
}