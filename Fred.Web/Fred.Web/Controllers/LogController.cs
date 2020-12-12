using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fred.Business.Log;
using Fred.Entities.Log;
using Fred.Entities.Role;
using Fred.Web.Shared.App_LocalResources;
using NLog;

namespace Fred.Web.Controllers
{
    [Authorize(Roles = ApplicationRole.SuperAdmin)]
    public class LogController : Controller
    {
        private readonly INLogManager logManager;
        protected Logger logger;

        public LogController(INLogManager logManager)
        {
            this.logManager = logManager;
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Permet d'afficher une liste des logs
        /// </summary>
        /// <param name="page">La page à afficher</param>
        /// <param name="sort">La colonne pour ordonner la liste.</param>
        /// <param name="sortdir">Le sens du tri.</param>
        /// <param name="search">Le texte pour filtrer.</param>
        /// <param name="level">Le niveau de log.</param>
        /// <returns>Le résultat</returns>
        public ActionResult Index(int page = 1, string sort = "Id", string sortdir = "asc", string search = "", string level = "")
        {
            List<NLogEnt> data = new List<NLogEnt>();
            int pageSize = 10;
            int totalRecord = 0;

            try
            {
                // Initialisation des filtres
                ViewBag.levels = GetLevels();
                ViewBag.search = search;

                // Initialisation de la liste  
                if (page < 1)
                {
                    page = 1;
                }

                int skip = (page * pageSize) - pageSize;
                data = logManager.Search(search, level, sort, sortdir, skip, pageSize, out totalRecord);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                ViewBag.Error = LogRessource.Log_Exception;
            }

            ViewBag.TotalRows = totalRecord;
            return View(data);
        }

        /// <summary>
        /// Permet de récupérer la liste des niveaux de logs
        /// </summary>
        /// <returns>Une liste des niveaux de log</returns>
        private List<SelectListItem> GetLevels()
        {
            List<SelectListItem> levels = new List<SelectListItem>();
            levels.Add(new SelectListItem { Text = LogRessource.Log_Fatal, Value = "Fatal" });
            levels.Add(new SelectListItem { Text = LogRessource.Log_Error, Value = "Error" });
            levels.Add(new SelectListItem { Text = LogRessource.Log_Warn, Value = "Warn" });
            levels.Add(new SelectListItem { Text = LogRessource.Log_Info, Value = "Info" });
            levels.Add(new SelectListItem { Text = LogRessource.Log_Debug, Value = "Debug" });
            levels.Add(new SelectListItem { Text = LogRessource.Log_Trace, Value = "Trace" });
            levels = levels.OrderBy(x => x.Text).ToList();

            levels.Insert(0, new SelectListItem { Text = LogRessource.Log_TousLevels, Value = string.Empty });
            return levels;
        }
    }
}