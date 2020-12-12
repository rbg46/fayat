using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models
{
  public class AuthentificationModel
  {

    /// <summary>
    /// Message qui sera affiché à l'utilisateur
    /// </summary>
    public string message { get; set; }


    /// <summary>
    /// Code du message d'erreur nécessaire pour extraire l'information référencée en Ressource
    /// </summary>
    public int idErrorCode { get; set; }

    /// <summary>
    /// Persistance de l'url de retour (à voir si doit être utilisé)
    /// </summary>
    public string returnUrl { get; set; }

  }
}