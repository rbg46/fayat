using System.Web.Mvc;

namespace Fred.Web.Areas.Personnel
{
  public class PersonnelAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Personnel";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Personnel_default",
          "Personnel/{controller}/{action}/{id}/{type}",
          new { action = "Index", id = UrlParameter.Optional, type = UrlParameter.Optional });

      context.MapRoute(
                "Personnel_default1",
                "Personnel/{action}/{id}",
                new { controller = "Externe", action = "Index", id = UrlParameter.Optional });
    }
  }
}