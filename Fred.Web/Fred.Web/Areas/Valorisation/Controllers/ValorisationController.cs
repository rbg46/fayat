using System.Web.Mvc;

namespace Fred.Web.Areas.Valorisation.Controllers
{
  public class ValorisationController : Controller
  {
    // GET: Valorisation/Valorisation
    public ActionResult Index(int? id)
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Valorisation;
      ViewBag.id = id;
      return View(id);
    }
  }
}