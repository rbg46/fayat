using System.Web.Mvc;

namespace Fred.Web.Areas.CodeMajoration
{
  /// <summary>
  /// Représente la définition du module MVC CodeMajoration.
  /// </summary>
  public class CodeMajorationAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "CodeMajoration";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module CodeMajoration.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "CodeMajoration_default",
          "CodeMajoration/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}