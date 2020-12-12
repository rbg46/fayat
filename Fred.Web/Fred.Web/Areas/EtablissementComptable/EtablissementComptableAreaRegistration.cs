using System.Web.Mvc;

namespace Fred.Web.Areas.EtablissementComptable
{
    public class EtablissementComptableAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EtablissementComptable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EtablissementComptable_default",
                "EtablissementComptable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}