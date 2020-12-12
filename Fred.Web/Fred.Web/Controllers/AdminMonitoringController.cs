using System.Web.Mvc;

namespace Fred.Web.Controllers
{
  [Authorize]
  public class AdminMonitoringController : Controller
  {
    public ActionResult Index(int? id = 0)
    {
      return View();
    }

    public ActionResult ModelUiFonts(int? id = 0)
    {
      return View();
    }

    public ActionResult ModelUiBootstrapControls(int? id = 0)
    {
      return View();
    }

    [AllowAnonymous]
    public ActionResult Security()
    {
      return View();
    }
  }
}