using System.Web.Mvc;

namespace Fred.Web.Areas.AuthentificationLog
{
    public class AuthentificationLogAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AuthentificationLog";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AuthentificationLog_default",
                "AuthentificationLog/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}