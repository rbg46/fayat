using System.Web.Mvc;

namespace Fred.Web.Areas.AuthentificationLog.Controllers
{
    [Authorize]
    public class AuthentificationLogController : Controller
    {
        // GET: AuthentificationLog/AuthentificationLog
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {
            ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Athentification;
            return View();
        }
    }
}
