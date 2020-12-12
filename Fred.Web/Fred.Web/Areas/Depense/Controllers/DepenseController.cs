using System;
using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.Depense.Controllers
{
  /// <summary>
  /// Contrôleur MVC des dépenses
  /// </summary>
  [Authorize]
  public class DepenseController : Controller
  {
        /// <summary>
        /// Get explorateur de depense
        /// </summary>
        /// <param name="id">Id du CI sur lequel on va travailler. Cette valeur sera injectée dans le ViewBag</param>
        /// <param name="favoriId">Id du favori. Cette valeur sera injectée dans le ViewBag</param>
        /// <param name="dateDebut">Date de début de la période</param>
        /// <param name="dateFin">Date de fin de la période</param>
        /// <param name="codeFamille">Code de la famille opération diverse</param>
        /// <returns>la page</returns>
        public ActionResult Explorateur(int? id, int? favoriId, string dateDebut = null, string dateFin = null, string codeFamille = null)
        {
            ViewBag.ciId = id;
            ViewBag.favoriId = favoriId;
            ViewBag.dateDebut = dateDebut;
            ViewBag.dateFin = dateFin;
            ViewBag.codeFamille = codeFamille;
            return View();
        }
    }
}