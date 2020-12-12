using System.Web.Mvc;

namespace Fred.Web.Areas.CodeAbsence
{
    public class CodeAbsenceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CodeAbsence";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CodeAbsence_default",
                "CodeAbsence/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}