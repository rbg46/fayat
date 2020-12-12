using Fred.Notifications.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace Fred.SignalR
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly IConnectionManager connectionManager;

        public SignalRNotificationService(IConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public void SendUserUnreadNotificationCount(int userId, int notificationCount)
        {
            IHubContext context = connectionManager.GetHubContext<FredHub>();
            dynamic user = context.Clients.User(userId.ToString());

            user.UpdateNotificationCount(notificationCount);
        }
    }
}
