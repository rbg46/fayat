using System.Web.Mvc;

namespace Fred.Web.Areas.DatesClotureComptable
{
  /// <summary>
  /// Représente la définition du module MVC DatesClotureComptable.
  /// </summary>
  public class DatesClotureComptableAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "DatesClotureComptable";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module DatesClotureComptable.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
      "DatesClotureComptable_default",
      "DatesClotureComptable/{controller}/{action}/{id}",
      new { action = "Index", id = UrlParameter.Optional });
    }
  }
}