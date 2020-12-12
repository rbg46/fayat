namespace Fred.Notifications.Interfaces
{
    public interface INotificationService
    {
        void SendUserUnreadNotificationCount(int userId, int notificationCount);
    }
}
