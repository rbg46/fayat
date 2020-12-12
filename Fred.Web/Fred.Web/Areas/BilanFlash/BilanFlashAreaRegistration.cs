using System.Web.Mvc;

namespace Fred.Web.Areas.BilanFlash
{
    public class BilanFlashAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BilanFlash";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BilanFlash_default",
                "BilanFlash/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}