using System.Web.Mvc;

namespace Fred.Web.Areas.ValidationPointage
{
    public class ValidationPointageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ValidationPointage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ValidationPointage_default",
                "ValidationPointage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}