using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Notification;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Notification
{
    public class NotificationRepository : FredRepository<NotificationEnt>, INotificationRepository
    {
        public NotificationRepository(FredDbContext context)
          : base(context)
        {
        }

        public async Task<IEnumerable<NotificationEnt>> FindUserNotificationsAsync(int userId, Expression<Func<NotificationEnt, bool>> filter = null)
        {
            IQueryable<NotificationEnt> notifications = Context.Notifications.Where(c => c.UtilisateurId == userId);
            if (filter != null)
            {
                notifications = notifications.Where(filter);
            }

            return await notifications.OrderByDescending(x => x.DateCreation).ToListAsync();
        }

        public async Task<IEnumerable<NotificationEnt>> FindUserNotificationsAsync(int userId, string searchText)
        {
            return await Context.Notifications.Where(x => x.UtilisateurId == userId && x.Message.Contains(searchText)).OrderByDescending(x => x.DateCreation).ToListAsync();
        }

        public async Task<int> GetUserUnreadNotificationCountAsync(int userId)
        {
            return await Context.Notifications.CountAsync(x => x.UtilisateurId == userId && !x.EstConsulte);
        }

        public async Task<Dictionary<int, int>> GetUsersUnreadNotificationCountAsync(IEnumerable<int> userIds)
        {
            return await Context.Notifications.Where(n => !n.EstConsulte && userIds.Contains(n.UtilisateurId)).GroupBy(n => n.UtilisateurId).ToDictionaryAsync(n => n.Key, n => n.Count());
        }

        public async Task AddAsync(NotificationEnt notification)
        {
            await Context.Notifications.AddAsync(notification);
        }

        public async Task DeleteAsync(IEnumerable<NotificationEnt> notifications)
        {
            IEnumerable<int> notificationsIds = notifications.Select(n => n.NotificationId);
            List<NotificationEnt> notificationsToRemove = await Context.Notifications.Where(n => notificationsIds.Contains(n.NotificationId)).ToListAsync();

            Remove(notificationsToRemove);
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            IEnumerable<NotificationEnt> notifications = await FindUserNotificationsAsync(userId);
            Remove(notifications);
        }

        private void Remove(IEnumerable<NotificationEnt> notifications)
        {
            Context.Notifications.RemoveRange(notifications);
        }
    }
}