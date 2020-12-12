using System.Web.Mvc;

namespace Fred.Web.Areas.Nature
{
  /// <summary>
  /// Représente la définition du module MVC Nature.
  /// </summary>
  public class NatureAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "Nature";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module Nature.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Nature_default",
          "Nature/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}