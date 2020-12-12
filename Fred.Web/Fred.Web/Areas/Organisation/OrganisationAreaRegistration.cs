using System.Web.Mvc;

namespace Fred.Web.Areas.Organisation
{
  public class OrganisationAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Organisation";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Organisation_default",
          "Organisation/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}