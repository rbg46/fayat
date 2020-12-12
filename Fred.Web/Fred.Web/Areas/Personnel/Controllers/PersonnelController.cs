using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.Personnel.Controllers
{
    [Authorize]
    public class PersonnelController : Controller
    {
        // GET: Personnel/Personnel
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPersonnelIndex)]
        public ActionResult Index(int? id, int? favoriId)
        {
            ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Personnel;
            ViewBag.id = id;
            ViewBag.favoriId = favoriId;
            return View();
        }

        // GET: Personnel/Personnel/Equipe
        public ActionResult Equipe(int? id)
        {
            ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Personnel;
            ViewBag.id = id;
            return View(id);
        }

        /// <summary>
        ///  Edition d'un personnel
        /// </summary>
        /// <param name="id">id du personnel</param>
        /// <param name="type">Type personnel = intérimaire ou externe</param>
        /// <returns>View Edit</returns>
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPersonnelEdit)]
        public ActionResult Edit(int? id = 0, string type = "")
        {
            ViewBag.id = id;
            ViewBag.typePersonnel = type;
            return View();
        }
    }
}
