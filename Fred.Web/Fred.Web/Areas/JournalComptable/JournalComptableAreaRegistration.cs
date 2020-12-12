using System.Web.Mvc;

namespace Fred.Web.Areas.JournalComptable
{
    public class JournalComptableAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "JournalComptable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "JournalComptable_default",
                "JournalComptable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}