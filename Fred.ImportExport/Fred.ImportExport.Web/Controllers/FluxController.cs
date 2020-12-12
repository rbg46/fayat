using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Resources.Resources;

namespace Fred.ImportExport.Web.Controllers
{
    public class FluxController : ControllerBase
    {
        private readonly IFluxManager fluxManager;

        public FluxController(IFluxManager fluxManager)
        {
            this.fluxManager = fluxManager;
        }

        /// <summary>
        /// Permet d'afficher une liste des flux
        /// </summary>
        /// <param name="page">La page à afficher</param>
        /// <param name="sort">La colonne pour ordonner la liste.</param>
        /// <param name="sortdir">Le sens du tri.</param>
        /// <returns>Le résultat</returns>
        public ActionResult Index(int page = 1, string sort = "Id", string sortdir = "asc")
        {
            List<FluxEnt> data = new List<FluxEnt>();
            int totalRecord = 0;

            try
            {
                data = fluxManager.GetAll();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            ViewBag.TotalRows = totalRecord;
            return View(data);
        }

        /// <summary>
        /// Permet de mettre à jour l'état d'un flux
        /// </summary>
        /// <param name="id">L'identifiant du flux</param>
        /// <param name="isActif">L'état du flux (Actif ou Inactif)</param>
        /// <returns>Le résultat</returns>
        [HttpPost]
        public ActionResult UpdateActif(int id, bool isActif)
        {
            try
            {
                fluxManager.UpdateActive(id, isActif);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return null;
        }
    }
}
