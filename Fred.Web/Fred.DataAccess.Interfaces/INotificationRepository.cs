using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Notification;

namespace Fred.DataAccess.Interfaces
{
    public interface INotificationRepository : IRepository<NotificationEnt>
    {
        Task<IEnumerable<NotificationEnt>> FindUserNotificationsAsync(int userId, Expression<Func<NotificationEnt, bool>> filter = null);

        Task<IEnumerable<NotificationEnt>> FindUserNotificationsAsync(int userId, string searchText);

        Task<int> GetUserUnreadNotificationCountAsync(int userId);

        Task<Dictionary<int, int>> GetUsersUnreadNotificationCountAsync(IEnumerable<int> userIds);

        Task AddAsync(NotificationEnt notification);

        Task DeleteAsync(IEnumerable<NotificationEnt> notifications);

        Task DeleteByUserIdAsync(int userId);
    }
}