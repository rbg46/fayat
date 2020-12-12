using System;

namespace Fred.Web.Shared.Models
{
  public class AuthentificationLogListModel
  {
    /// <summary>
    /// Id
    /// </summary>   
    public int AuthentificationLogId { get; set; }

    /// <summary>
    /// Login renseigné
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de création.
    /// </summary>
    public DateTime DateCreation { get; set; }

    /// <summary>
    /// type d'erreur si l'anomalie vient d'une règle de gestion métier
    /// </summary>
    public int ErrorType { get; set; }

    /// <summary>
    /// IP du poste
    /// </summary>
    public string AdressIp { get; set; }

    /// <summary>
    /// URI initialement demandée.
    /// </summary>
    public string RequestedUrl { get; set; }

    /// <summary>
    /// Message d'erreur retourné a l'utilisateur
    /// </summary>  
    public string Error { get; set; }

    /// <summary>
    /// Origin de l'erreur affiché a l'utilisateur
    /// </summary>  
    public string OriginText { get; set; }
  }
}