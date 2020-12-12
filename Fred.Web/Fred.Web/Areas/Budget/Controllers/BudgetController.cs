using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.Budget.Controllers
{
    [Authorize]
    public class BudgetController : Controller
    {
        // GET: Budget/Budget
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuBudgetIndex)]
        public ActionResult Index()
        {
            ViewBag.titre = ViewBag.titre = Shared.App_LocalResources.Page_Title.Title_App + Shared.App_LocalResources.Page_Title.Title_budget;
            return View();
        }

        // GET: Budget/Budget
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuBudgetIndex)]
        public ActionResult Liste(int? id, int? favoriId = 0)
        {
            ViewBag.id = id;
            ViewBag.favoriId = favoriId;
            return View();
        }

        // GET: Budget/Budget/Detail/budgetId
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuBudgetIndex)]
        public ActionResult Detail(int? id = 0, int? favoriId = 0)
        {
            return Liste(id, favoriId);
        }

        // GET: Budget/Budget
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuBudgetIndex)]
        public ActionResult SousDetail()
        {
            return View();
        }

        // GET: Budget/Budget
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAvancement)]
        public ActionResult Avancement(int? favoriId = 0)
        {
            ViewBag.favoriId = favoriId;
            return View("Avancement");
        }

        // GET: Budget/Budget
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAvancementRecette)]
        public ActionResult AvancementRecette()
        {
            return View();
        }

        // GET: Budget/Budget
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuControleBudgetaire)]
        public ActionResult ControleBudgetaire(int? favoriId = 0)
        {
            ViewBag.favoriId = favoriId;
            return View("ControleBudgetaire");
        }

        // Bibliothèque des prix
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuBibliothequePrix)]
        public ActionResult BibliothequePrix(int? organisationId, int? favoriId, int? deviseId)
        {
            ViewBag.organisationId = organisationId;
            ViewBag.favoriId = favoriId;
            ViewBag.deviseId = deviseId;
            return View();
        }

        // Comparaison de budget
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuBudgetComparaison)]
        public ActionResult BudgetComparaison()
        {
            return View();
        }
    }
}
