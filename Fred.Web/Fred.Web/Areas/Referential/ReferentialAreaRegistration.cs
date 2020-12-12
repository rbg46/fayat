using System.Web.Mvc;

namespace Fred.Web.Areas.Referential
{
  public class ReferentialAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Referential";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Referential_PickList",
          "Referential/{controller}/PickListReferential/{id}",
          new { action= "PickListReferential",  id = UrlParameter.Optional }
      );

      context.MapRoute(
        "Referential_PickListCaller",
        "Referential/{controller}/PickListReferentialCaller/{isintegrated}",
        new { action = "PickListReferentialCaller", isintegrated = false }
      );

      context.MapRoute(
          "Referential_default",
          "Referential/{controller}/{Action}/{id}",
          new { action = "PickListReferential", id = UrlParameter.Optional }
      );
    }
  }
}