using System.Web.Mvc;

namespace Fred.Web.Areas.Facture
{
  public class FactureAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Facture";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Facture_default",
          "Facture/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}