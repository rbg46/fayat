using System.Web.Mvc;

namespace Fred.Web.Areas.Materiel
{
  public class MaterielAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Materiel";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Materiel_default",
          "Materiel/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional });
    }
  }
}