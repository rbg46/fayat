using System.Web.Mvc;

namespace Fred.Web.Areas.DatesCalendrierPaie
{
  /// <summary>
  /// Représente la définition du module MVC DatesCalendrierPaie.
  /// </summary>
  public class DatesCalendrierPaieAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "DatesCalendrierPaie";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module DatesCalendrierPaie.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
      "DatesCalendrierPaie_default",
      "DatesCalendrierPaie/{controller}/{action}/{id}",
      new { action = "Index", id = UrlParameter.Optional });
    }
  }
}