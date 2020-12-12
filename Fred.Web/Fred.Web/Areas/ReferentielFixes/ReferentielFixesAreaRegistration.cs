using System.Web.Mvc;

namespace Fred.Web.Areas.ReferentielFixes
{
    public class ReferentielFixesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ReferentielFixes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ReferentielFixes_default",
                "ReferentielFixes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}