using System.Web.Mvc;

namespace Fred.Web.Areas.CloturesPeriodes
{
    public class CloturesPeriodesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CloturesPeriodes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CloturesPeriodes_default",
                "CloturesPeriodes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}