using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Rapport.Controllers
{
  [Authorize]
  public class PointagePersonnelController : Controller
  {
    // GET: PointagePersonnel/PointagePersonnel
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPointagePersonnelIndex)]
    public ActionResult Index(int? personnelId, int? favoriId, string period = null)
    {
      ViewBag.PersonnelId = personnelId;
      ViewBag.Period = period;
      ViewBag.favoriId = favoriId;
      return View();
    }

    /// <summary>
    /// GET: PointagePersonnel/Export
    /// </summary>
    /// <returns>Current view</returns> 
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuExportPointagePersonnelIndex)]
    public ActionResult Export()
    {
        return View();
    }
  }
}
