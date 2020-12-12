using System.Web.Mvc;

namespace Fred.Web.Areas.Module
{
  /// <summary>
  /// Représente la définition du module MVC commande.
  /// </summary>
  public class ModuleAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "Module";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module Commande.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Module_Default",
          "Module/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}