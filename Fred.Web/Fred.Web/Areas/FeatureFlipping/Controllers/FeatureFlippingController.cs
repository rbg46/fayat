using System.Web.Mvc;

namespace Fred.Web.Areas.FeatureFlipping.Controllers
{
    [Authorize()]
    public class FeatureFlippingController : Controller
    {
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {
            ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_FeatureFlipping;
            return View();
        }
    }
}
