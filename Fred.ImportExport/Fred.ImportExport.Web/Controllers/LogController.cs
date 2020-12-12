using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fred.ImportExport.Business;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Log;
using Fred.ImportExport.Entities;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Log;
using Fred.ImportExport.Models.Log;
using Fred.ImportExport.Resources.Resources;

namespace Fred.ImportExport.Web.Controllers
{
    /// <summary>
    /// Represente la controller des logs
    /// </summary>
    public class LogController : ControllerBase
    {
        private readonly LogManager logManager;
        private readonly IImportExportLoggingService loggingService;
        private readonly ILoggingAdministratorService loggingAdminService;
        private readonly IFluxManager fluxManager;

        public LogController(LogManager logManager,
            IImportExportLoggingService loggingService,
            ILoggingAdministratorService loggingAdminService,
            IFluxManager fluxManager)
        {
            this.logManager = logManager;
            this.loggingService = loggingService;
            this.loggingAdminService = loggingAdminService;
            this.fluxManager = fluxManager;
        }


        /// <summary>
        /// Permet d'afficher une liste des logs
        /// </summary>
        /// <param name="page">La page à afficher</param>
        /// <param name="sort">La colonne pour ordonner la liste.</param>
        /// <param name="sortdir">Le sens du tri.</param>
        /// <param name="search">Le texte pour filtrer.</param>
        /// <param name="level">Le niveau de log.</param>
        /// <param name="fluxCode">Le code flux</param>
        /// <param name="startDate">La date de début de la periode</param>
        /// <param name="endDate">La date de fin de la periode</param>
        /// <returns>Le résultat</returns>
        public async Task<ActionResult> Index(int page = 1, string sort = "Id", string sortdir = "desc", string search = "", string level = "", string fluxCode = "", string startDate = "", string endDate = "")
        {
            List<NLogFredIeEnt> data = new List<NLogFredIeEnt>();
            const int pageSize = 20;
            int totalRecord = 0;

            try
            {
                // Initialisation des filtres
                ViewBag.levels = GetLevels();
                ViewBag.minloglevels = GetMinLogLevels();
                ViewBag.search = search;
                ViewBag.MinLogLevel = loggingService.GetMinLogLevelFor(Resource.LogRules_Database_Regex);
                ViewBag.ApiMinLogLevel = await loggingAdminService.GetApiLogLevelAsync(Resource.LogRules_Database_Regex);
                ViewBag.FluxCodes = GetFluxCodes();


                var (startDateTime, endDateTime) = ValidateDate(startDate, endDate);
                ViewBag.startDate = startDateTime?.ToString("dd/MM/yyyy HH:mm:ss");
                ViewBag.endDate = endDateTime?.ToString("dd/MM/yyyy HH:mm:ss");

                if (page <= 0) { page = 1; }

                data = logManager.Search(search, startDateTime, endDateTime, level, fluxCode, sort, sortdir, out totalRecord, page, pageSize);

                ViewBag.TotalRows = totalRecord;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View(data);
        }

        /// <summary>
        /// Change le nivea de log minimum
        /// </summary>
        /// <param name="niveauLog">niveau de log min en string</param>
        /// <returns>return la vue</returns>
        public ActionResult ChangeLogLevel(string niveauLog)
        {
            try
            {
                if (ViewBag.MinLogLevel != niveauLog)
                {
                    //Changer le niveau des logs dans nlog
                    loggingService.SetLogLevel(niveauLog, Resource.LogRules_Database_Regex);
                }

                if (ViewBag.ApiMinLogLevel != niveauLog)
                {
                    var logConfigurationModel = new LogConfigurationModel()
                    {
                        LogLevel = niveauLog,
                        RegexRules = Resource.LogRules_Database_Regex
                    };

                    //Change aussi le niveau de log au niveau de l'API Fred IE
                    loggingAdminService.UpdateApiLogLevel(logConfigurationModel);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Permet de récupérer la liste des niveaux de logs
        /// </summary>
        /// <returns>Une liste des niveaux de log</returns>
        private List<SelectListItem> GetLevels()
        {
            List<SelectListItem> levels = new List<SelectListItem>
            {
                new SelectListItem { Text = Resource.Log_Info, Value = "Info" },
                new SelectListItem { Text = Resource.Log_Debug, Value = "Debug" },
                new SelectListItem { Text = Resource.Log_Fatal, Value = "Fatal" },
                new SelectListItem { Text = Resource.Log_Error, Value = "Error" },
                new SelectListItem { Text = Resource.Log_Warn, Value = "Warn" }
            };

            levels = levels.OrderBy(x => x.Text).ToList();
            levels.Insert(0, new SelectListItem { Text = "Tous les levels", Value = string.Empty });

            return levels;
        }

        /// <summary>
        /// Permet de récupérer la liste des niveaux de logs
        /// </summary>
        /// <returns>Une liste des niveaux de log</returns>
        private List<SelectListItem> GetMinLogLevels()
        {
            List<SelectListItem> levels = new List<SelectListItem>
            {
                new SelectListItem { Text = Resource.Log_Info, Value = "Info" },
                new SelectListItem { Text = Resource.Log_Debug, Value = "Debug" }
            };

            levels = levels.OrderBy(x => x.Text).ToList();

            return levels;
        }

        private List<SelectListItem> GetFluxCodes()
        {
            List<SelectListItem> fluxCodes = new List<SelectListItem>();
            List<FluxEnt> allFlux = fluxManager.GetAll();

            foreach (FluxEnt flux in allFlux)
            {
                fluxCodes.Add(new SelectListItem { Text = flux.Code, Value = flux.Code });
            }

            fluxCodes = fluxCodes.OrderBy(x => x.Text).ToList();
            fluxCodes.Insert(0, new SelectListItem { Text = "Tous les codes flux", Value = string.Empty });

            return fluxCodes;
        }

        private (DateTime? StartDateTime, DateTime? EndDateTime) ValidateDate(string startDate, string endDate)
        {
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                return (null, null);
            }

            var isValidStartDate = DateTime.TryParse(startDate, out var startDateTime);
            var isValidEndDate = DateTime.TryParse(endDate, out var endDateTime);

            if (isValidStartDate && isValidEndDate)
            {
                if (DateTime.Compare(endDateTime, startDateTime) <= 0)
                {
                    return (null, null);
                }
                return (startDateTime, endDateTime);
            }

            // Si on n'arrive pas à parser une des 2 dates => reset les 2 dates.
            if (!string.IsNullOrEmpty(startDate) && !isValidStartDate
                || !string.IsNullOrEmpty(endDate) && !isValidEndDate)
            {
                return (null, null);
            }

            // Une des 2 dates est null et l'autre valide.
            return isValidStartDate
            ? (startDateTime as DateTime?, default(DateTime?))
            : (default(DateTime?), endDateTime as DateTime?);
        }
    }
}
