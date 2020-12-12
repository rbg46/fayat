using System;

namespace Fred.Web.Models.ExternalDirectory
{
  public class ExternalDirectoryModel
  {

    /// <summary>
    /// Obtient ou définit l'identifiant du ticket d'authentification de l'utilisateur Externe
    /// </summary>
    public int FayatAccessDirectoryId { get; set; }

    /// <summary>
    /// Obtient ou définit le mot de passe  du ticket d'authentification de l'utilisateur Externe
    /// </summary>
    public string MotDePasse { get; set; }

    /// <summary>
    /// Obtient ou définit la date d'authentification ticket d'authentification de l'utilisateur Externe
    /// </summary>
    public DateTime? DateExpiration { get; set; }

    /// <summary>
    /// Obtient ou définit le statut ticket d'authentification de l'utilisateur Externe
    /// </summary>
    public bool IsActived { get; set; }

  }
}