using System.Web.Mvc;

namespace Fred.Web.Areas.CodeDeplacement
{
  /// <summary>
  /// Représente la définition du module MVC CodeDeplacement.
  /// </summary>
  public class CodeDeplacementAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "CodeDeplacement";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module CodeDeplacement.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "CodeDeplacement_default",
          "CodeDeplacement/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}