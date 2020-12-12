using System.Web.Mvc;

namespace Fred.Web.Areas.Commande
{
  /// <summary>
  /// Représente la définition du module MVC commande.
  /// </summary>
  public class CommandeAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "Commande";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module Commande.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Commande_default",
          "Commande/{controller}/{action}/{id}/{duplicate}",
          new { action = "Index", id = UrlParameter.Optional, duplicate = UrlParameter.Optional });
    }
  }
}