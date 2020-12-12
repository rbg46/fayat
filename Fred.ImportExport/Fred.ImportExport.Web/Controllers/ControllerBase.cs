using NLog;
using System.Web.Mvc;

namespace Fred.ImportExport.Web.Controllers
{
  /// <summary>
  /// Classe de base des controllers
  /// </summary>
  public class ControllerBase : Controller
  {
    /// <summary>
    /// Initialise une nouvelle instance.
    /// </summary>
    public ControllerBase()
    {
      Logger = LogManager.GetCurrentClassLogger();
    }

    protected Logger Logger { get; set; }

    /// <summary>
    /// Permet de capture l'exception d'une action et de l'afficher
    /// </summary>
    /// <param name="filterContext">Le context de l'exception</param>
    protected override void OnException(ExceptionContext filterContext)
    {
      filterContext.ExceptionHandled = true;

      filterContext.Result = this.View("Error", new HandleErrorInfo(filterContext.Exception,
       filterContext.RouteData.Values["controller"].ToString(),
       filterContext.RouteData.Values["action"].ToString()));

      // On log des errors avec NLog
      Logger.Error(filterContext.Exception);
    }
  }
}