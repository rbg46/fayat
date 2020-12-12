using System.Web.Mvc;

namespace Fred.Web.Areas.Depense
{
  /// <summary>
  /// Représente la définition du module MVC dépense.
  /// </summary>
  public class DepenseAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom de la dépense.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "Depense";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module dépense.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Depense_Default",
          "Depense/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}