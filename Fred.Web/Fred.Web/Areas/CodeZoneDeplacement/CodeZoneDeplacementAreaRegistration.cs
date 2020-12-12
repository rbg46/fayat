using System.Web.Mvc;

namespace Fred.Web.Areas.CodeZoneDeplacement
{
  public class CodeZoneDeplacementAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "CodeZoneDeplacement";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "CodeZoneDeplacement_default",
          "CodeZoneDeplacement/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}