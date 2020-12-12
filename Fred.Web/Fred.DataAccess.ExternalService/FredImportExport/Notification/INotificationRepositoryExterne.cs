using System.Threading.Tasks;

namespace Fred.DataAccess.ExternalService.FredImportExport.Notification
{
    public interface INotificationRepositoryExterne
    {
        Task SubscribeToUserNotificationsAsync(int userId);
    }
}