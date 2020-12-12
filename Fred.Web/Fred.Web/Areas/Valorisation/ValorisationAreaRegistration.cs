using System.Web.Mvc;

namespace Fred.Web.Areas.Valorisation
{
  public class ValorisationAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Valorisation";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Valorisation_default",
          "Valorisation/{controller}/{action}/{id}/{type}",
          new { action = "Index", id = UrlParameter.Optional, type = UrlParameter.Optional });

      context.MapRoute(
                "Valorisation_default1",
                "Valorisation/{action}/{id}",
                new { controller = "Externe", action = "Index", id = UrlParameter.Optional });
    }
  }
}