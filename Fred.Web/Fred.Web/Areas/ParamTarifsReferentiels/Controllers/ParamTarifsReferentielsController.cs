using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.ParamTarifsReferentiels.Controllers
{
  [Authorize]
  public class ParamTarifsReferentielsController : Controller
  {
    // GET: ParamTarifsReferentiels/ParamTarifsReferentiels
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuParamTarifsReferentielsIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Param_Tarif;
      return View();
    }
  }
}