using System.Web.Mvc;

namespace Fred.Web.Areas.ParamTarifsReferentiels
{
    public class ParamTarifsReferentielsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ParamTarifsReferentiels";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ParamTarifsReferentiels_default",
                "ParamTarifsReferentiels/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}