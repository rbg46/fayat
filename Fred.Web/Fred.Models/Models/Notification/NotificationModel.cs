using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fred.Business.Notification;

namespace Fred.Web.Models.Notification
{
  public class NotificationModel
  {

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une notification
    /// </summary>
    public int NotificationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'utilisateur de rattachement d'une notification
    /// </summary>
    public int UtilisateurId { get; set; }

    /// <summary>
    /// Obtient ou définit le type de la notification
    /// </summary>
    public int TypeNotificationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'objet correspondant à la notification
    /// </summary>
    public int ObjetId { get; set; }

    /// <summary>
    /// Obtient ou définit la date de création de la notification
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit si la notification a été consultée
    /// </summary>
    public bool EstConsulte { get; set; }
  }
}

