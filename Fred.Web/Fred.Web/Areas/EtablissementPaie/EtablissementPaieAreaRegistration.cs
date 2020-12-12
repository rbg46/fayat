using System.Web.Mvc;

namespace Fred.Web.Areas.EtablissementPaie
{
  /// <summary>
  /// Représente la définition du module MVC EtablissementPaie.
  /// </summary>
  public class EtablissementPaieAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "EtablissementPaie";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module EtablissementPaie.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "EtablissementPaie_default",
          "EtablissementPaie/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}