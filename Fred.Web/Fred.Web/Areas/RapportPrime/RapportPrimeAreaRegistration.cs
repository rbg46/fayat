using System.Web.Mvc;

namespace Fred.Web.Areas.RapportPrime
{
    public class RapportPrimeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RapportPrime";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RapportPrime_default",
                "RapportPrime/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}