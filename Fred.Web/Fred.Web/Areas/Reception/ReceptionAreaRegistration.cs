using System.Web.Mvc;

namespace Fred.Web.Areas.Reception
{
  public class ReceptionAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Reception";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Reception_default",
          "Reception/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional }
            );    
    }
  }
}