using System.Web.Mvc;

namespace Fred.Web.Areas.Role
{
  /// <summary>
  /// Représente la définition du module MVC commande.
  /// </summary>
  public class RoleAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "Role";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module Commande.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Role_Default",
          "Role/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}