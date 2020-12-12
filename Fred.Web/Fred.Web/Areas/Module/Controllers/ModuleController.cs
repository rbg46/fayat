using System.Web.Mvc;

namespace Fred.Web.Areas.Module.Controllers
{
    /// <summary>
    /// Contrôleur MVC des modules
    /// </summary>
    [Authorize]
    public class ModuleController : Controller
    {
        /// <summary>
        /// GET: Module/Module
        /// </summary>
        /// <returns>Retourne une action MVC</returns>    
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {
            ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Module;
            return View();
        }
    }
}
