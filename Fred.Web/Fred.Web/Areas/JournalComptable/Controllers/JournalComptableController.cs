using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.JournalComptable.Controllers
{
  [Authorize]
  public class JournalComptableController : Controller
  {
    /// <summary>
    /// GET: JournalComptable/JournalComptable
    /// </summary>
    /// <returns>Action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuJournalComptableIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Shared.App_LocalResources.Page_Title.Title_App + Shared.App_LocalResources.Page_Title.Title_Journal_Comptable;
      return View();
    }
  }
}