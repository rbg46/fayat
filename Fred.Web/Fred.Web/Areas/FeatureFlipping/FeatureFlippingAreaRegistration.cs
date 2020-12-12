using System.Web.Mvc;

namespace Fred.Web.Areas.FeatureFlipping
{
  /// <summary>
  /// Représente la définition du module MVC FeatureFlipping
  /// </summary>
  public class FeatureFlippingAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "FeatureFlipping";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module FeatureFlipping.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "FeatureFlipping_default",
          "FeatureFlipping/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}
