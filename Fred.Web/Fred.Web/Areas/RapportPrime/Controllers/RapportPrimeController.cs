using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.RapportPrime.Controllers
{
  /// <summary>
  /// Contrôleur MVC des Rapports prime
  /// </summary>
  [Authorize()]
  public class RapportPrimeController : Controller
    {
        // GET: RapportPrime/RapportPrime
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuRapportPrimesIndex)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
