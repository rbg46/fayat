using System.Web.Mvc;

namespace Fred.Web.Areas.RessourcesSpecifiquesCI
{
    public class RessourcesSpecifiquesCIAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RessourcesSpecifiquesCI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RessourcesSpecifiquesCI_default",
                "RessourcesSpecifiquesCI/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}