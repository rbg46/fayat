using System.Web.Mvc;

namespace Fred.Web.Areas.CommandeEnergies
{
    public class CommandeEnergieAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CommandeEnergie";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CommandeEnergie_default",
                "CommandeEnergie/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
