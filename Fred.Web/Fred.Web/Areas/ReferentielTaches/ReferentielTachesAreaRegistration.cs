using System.Web.Mvc;

namespace Fred.Web.Areas.ReferentielTaches
{
    public class ReferentielTachesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ReferentielTaches";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
      context.MapRoute(
                "ReferentielTaches_default",
                "ReferentielTaches/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}