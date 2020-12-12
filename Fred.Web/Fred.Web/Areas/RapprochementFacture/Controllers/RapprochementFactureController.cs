using System.Web.Mvc;

namespace Fred.Web.Areas.RapprochementFacture.Controllers
{
  [Authorize()]
  public class RapprochementFactureController : Controller
  {
    // GET: RapprochementFacture/RapprochementFacture    
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Facture;
      return View();
    }
  }
}