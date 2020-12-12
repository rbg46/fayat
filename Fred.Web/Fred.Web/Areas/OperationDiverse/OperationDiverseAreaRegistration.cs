using System.Web.Mvc;

namespace Fred.Web.Areas.OperationDiverse
{
    public class OperationDiverseAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OperationDiverse";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OperationDiverse_default",
                "OperationDiverse/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}