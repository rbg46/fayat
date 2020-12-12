using System.Web.Mvc;

namespace Fred.Web.Areas.Rapport
{
  /// <summary>
  /// Représente la définition du module MVC Rapport.
  /// </summary>
  public class PointageAreaRegistration : AreaRegistration
  {
    /// <summary>
    /// Permet d'obtenir le nom du module.
    /// </summary>
    public override string AreaName
    {
      get
      {
        return "Pointage";
      }
    }

    /// <summary>
    /// Permet l'initialisation des routes du module Rapport.
    /// </summary>
    /// <param name="context">Contexte MVC</param>
    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
         "Pointage_new",
         "Pointage/{controller}/New",
         new { action = "New"}
     );

      context.MapRoute(
          "Pointage_default",
          "Pointage/{controller}/{action}/{id}/{duplicate}",
          new { action = "Detail",  duplicate = UrlParameter.Optional }, new {id = @"\d+" }
      );

      context.MapRoute(
          "Pointage_default2",
          "Pointage/{controller}/{action}/{msg}",
          new { action = "Index", msg = UrlParameter.Optional }
      );

      context.MapRoute(
          "PointagePersonnel_default",
          "PointagePersonnel/{controller}/{action}/{personnelId}/{period}",
          new { action = "Index", personnelId = UrlParameter.Optional, period = UrlParameter.Optional }
      );

      context.MapRoute(
          "ExportPointagePersonnel_default",
          "PointagePersonnel/{controller}/{action}",
          new { action = "Index"}
      );
    }
  }
}
