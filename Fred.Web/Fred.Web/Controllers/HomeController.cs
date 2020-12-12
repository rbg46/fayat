using System.Web.Mvc;

namespace Fred.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = Fred.Web.Shared.App_LocalResources.Page_Title.Title_Home;

            return View();
        }

        public ActionResult Monitoring()
        {
            return View();
        }
    }
}
