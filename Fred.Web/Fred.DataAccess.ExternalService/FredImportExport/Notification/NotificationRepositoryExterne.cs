using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.Notifications.Interfaces;
using Fred.SignalR;
using Microsoft.AspNet.SignalR.Client;

namespace Fred.DataAccess.ExternalService.FredImportExport.Notification
{
    public class NotificationRepositoryExterne : INotificationRepositoryExterne
    {
        private readonly INotificationService notificationService;
        private readonly IImportExportServiceDescriptor importExportServiceDescriptor;

        public NotificationRepositoryExterne(INotificationService notificationService, IImportExportServiceDescriptor importExportServiceDescriptor)
        {
            this.notificationService = notificationService;
            this.importExportServiceDescriptor = importExportServiceDescriptor;
        }

        public async Task SubscribeToUserNotificationsAsync(int userId)
        {
            HubConnection connection = GetHubConnection();
            SubscribeToHubNotificationEvent(connection, userId);

            //await connection.Start();

            HubConnection GetHubConnection()
            {
                string baseUrl = importExportServiceDescriptor.GetRestEndpoint().BaseUrl;
                var querystringData = new Dictionary<string, string> { { "UserId", userId.ToString() } };

                return new HubConnection(baseUrl, querystringData);
            }
        }

        private void SubscribeToHubNotificationEvent(HubConnection connection, int userId)
        {
            IHubProxy stockTickerHubProxy = GetHubProxy();
            stockTickerHubProxy.On<int>("UpdateNotificationCount", OnUpdateNotificationCount);

            IHubProxy GetHubProxy() => connection.CreateHubProxy(nameof(FredHub));
            void OnUpdateNotificationCount(int notificationCount) => notificationService.SendUserUnreadNotificationCount(userId, notificationCount);
        }
    }
}
