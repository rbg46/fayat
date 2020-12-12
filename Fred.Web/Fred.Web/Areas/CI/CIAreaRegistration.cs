using System.Web.Mvc;

namespace Fred.Web.Areas.CI
{
  /// <summary>
  /// Représente la définition du module MVC CI.
  /// </summary>
  public class CIAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "CI";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module CI.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "CI_default",
          "CI/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}