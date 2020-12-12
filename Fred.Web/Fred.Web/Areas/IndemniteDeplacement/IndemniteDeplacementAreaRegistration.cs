using System.Web.Mvc;

namespace Fred.Web.Areas.IndemniteDeplacement
{
  public class IndemniteDeplacementAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "IndemniteDeplacement";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "IndemniteDeplacement_default",
          "IndemniteDeplacement/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}