using System.Web.Mvc;

namespace Fred.Web.Areas.ReferentielEtendu
{
    public class ReferentielEtenduAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ReferentielEtendu";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ReferentielEtendu_default",
                "ReferentielEtendu/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}