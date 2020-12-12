using System;

namespace Fred.Web.Models.Notification
{
  /// <summary>
  /// Modèle d'affichage d'une notification
  /// </summary>
  public class NotificationModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une notification
    /// </summary>
    public int NotificationId { get; set; }

    /// <summary>
    /// Obtient ou définit le type de la notification
    /// </summary>
    public int TypeNotification { get; set; }

    /// <summary>
    /// Obtient ou définit le message correspondant à la notification
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de création d'une Notification.
    /// </summary>
    public DateTime DateCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si la notification a été consultée.
    /// </summary>
    public bool EstConsulte { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la notification est supprimé
    /// </summary>
    public bool IsDeleted { get; set; }
  }
}

