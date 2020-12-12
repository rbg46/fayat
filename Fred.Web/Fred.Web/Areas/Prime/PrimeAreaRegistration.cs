using System.Web.Mvc;

namespace Fred.Web.Areas.Prime
{
  /// <summary>
  /// Représente la définition du module MVC Prime.
  /// </summary>
  public class PrimeAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "Prime";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module Prime.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Prime_default",
          "Prime/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}