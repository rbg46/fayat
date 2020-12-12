using System.Web.Mvc;

namespace Fred.Web.Areas.ClassificationSocietes
{
    public class ClassificationSocietesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ClassificationSocietes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ClassificationSocietes_default",
                "ClassificationSocietes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
