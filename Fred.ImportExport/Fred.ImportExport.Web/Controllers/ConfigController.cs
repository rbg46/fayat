using Fred.ImportExport.Web.Models;
using System.Configuration;
using System.Web.Mvc;

namespace Fred.ImportExport.Web.Controllers
{
    /// <summary>
    /// Représente la controller de Config
    /// </summary>
    public class ConfigController : Controller
    {
        /// <summary>
        ///     Get view Index Config
        /// </summary>
        /// <returns>ActionResult index</returns>
        public ActionResult Index()
        {
            var configVm = new ConfigViewModel
            {
                StormApiUrl = ConfigurationManager.AppSettings["Storm:WebApiUrl"],
                StormApiUrlGroupeRazelBec = ConfigurationManager.AppSettings["Storm:WebApiUrl:Groupe:GRZB"],
                StormApiUrlGroupeFayatTp = ConfigurationManager.AppSettings["Storm:WebApiUrl:Groupe:GFTP"],
                BridgeApiUrl = ConfigurationManager.AppSettings["Bridge:WebApiUrl"]
            };

            return View(configVm);
        }
    }
}