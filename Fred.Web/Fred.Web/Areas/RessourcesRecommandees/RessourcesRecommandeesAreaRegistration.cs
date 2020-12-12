using System.Web.Mvc;

namespace Fred.Web.Areas.RessourcesRecommandees
{
    public class RessourcesRecommandeesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RessourcesRecommandees";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RessourcesRecommandees_default",
                "RessourcesRecommandees/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
