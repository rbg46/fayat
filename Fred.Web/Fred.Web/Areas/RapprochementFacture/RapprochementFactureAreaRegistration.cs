using System.Web.Mvc;

namespace Fred.Web.Areas.RapprochementFacture
{
    public class RapprochementFactureAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RapprochementFacture";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RapprochementFacture_default",
                "RapprochementFacture/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}