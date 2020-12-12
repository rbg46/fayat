using System.Web.Mvc;

namespace Fred.Web.Areas.Bareme
{
  public class BaremeAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Bareme";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
        "Bareme",
        "Bareme/{controller}/{action}"
      );
    }
  }
}
