using System.Web.Mvc;

namespace Fred.Web.Areas.FamilleOperationDiverse
{
    public class FamilleOperationDiverseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FamilleOperationDiverse";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FamilleOperationDiverse_default",
                "FamilleOperationDiverse/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
