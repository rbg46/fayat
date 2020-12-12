using System.Web.Mvc;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.ClassificationSocietes.Controllers
{
    [Authorize()]
    public class ClassificationSocietesController : Controller
    {
        // GET: ClassificationSocietes/ClassificationSocietes
        [FredAspAuthorize(globalPermissionKey: Entities.Permission.PermissionKeys.AffichageMenuClassificationSocietesIndex)]
        public ActionResult Index()
        {
            return View();
        }

  
    }
}
