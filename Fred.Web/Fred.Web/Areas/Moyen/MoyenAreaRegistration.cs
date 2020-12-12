using System.Web.Mvc;

namespace Fred.Web.Areas.Moyen
{
    public class MoyenAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Moyen";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Moyen_default",
                "Moyen/{controller}/{action}/{id}",
                new { action = "GestionMoyen", id = UrlParameter.Optional }
            );
        }
    }
}
