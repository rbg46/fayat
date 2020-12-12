using Fred.Entities.Notification;
using System.Collections.Generic;

namespace Fred.Common.Tests.Data.Notification.Builder
{
  public static class NotificationDataMocks
  {
    public static readonly IEnumerable<NotificationEnt> User1Notifications = new List<NotificationEnt>
    {
      new NotificationEnt
      {
        NotificationId = 1,
        UtilisateurId = 1,
        EstConsulte = true
      },
      new NotificationEnt
      {
        NotificationId = 2,
        UtilisateurId = 1,
        EstConsulte = false
      },
    };

    public static IEnumerable<NotificationEnt> GetNotificationsStored()
    {
      List<NotificationEnt> notifications = new List<NotificationEnt>
      {
        new NotificationEnt
        {
          NotificationId = 3,
          UtilisateurId = 2
        },
        new NotificationEnt
        {
          NotificationId = 4,
          UtilisateurId = 3
        }
      };
      notifications.AddRange(User1Notifications);

      return notifications;
    }
  }
}
