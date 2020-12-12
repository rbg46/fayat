using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Bootstrapper.Migration;
using Fred.Framework.Web.AppInsights;
using Fred.Web.Controllers;

namespace Fred.Web
{
#pragma warning disable SA1649 // File name must match first type name , C'est le fichier global.asax
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(System.Web.Configuration.WebConfigurationManager.AppSettings["SyncFusion:LicenseKey"]);

            MigrationInitializer.Initialize();
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppInsightsConfig.RegisterAppInsights();
            BundlesRazorEngine.Initialize();
        }

        protected void Application_Error()
        {
            Exception lastException = Server.GetLastError();
            Log(lastException);
            RedirectOnError(lastException);
        }

        private void RedirectOnError(Exception lastException)
        {
            try
            {
                if (lastException != null)
                {
                    var isHttpAntiForgeryException = lastException.GetType() == typeof(HttpAntiForgeryException);
                    HttpException httpException = lastException as HttpException;
                    if (httpException != null)
                    {
                        switch (httpException.GetHttpCode())
                        {
                            // page not found
                            case 404:
                                ManageNotFound();
                                break;

                            // server error
                            case 500:
                                if (isHttpAntiForgeryException)
                                {
                                    ExecuteController("HttpAntiForgery", lastException);
                                }
                                else
                                {
                                    ExecuteController("InternalServer", lastException);
                                }
                                break;

                            default:
                                Response.Redirect("~/Error/General");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        private void ManageNotFound()
        {
            if (string.IsNullOrEmpty(Request.CurrentExecutionFilePathExtension))
            {
                Response.Redirect("~/Error/NotFound");
            }
        }

        /// <summary>
        /// Créer le controlleur a la volé, et execute l'action passé en parametre.
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="exception">exception</param>
        private void ExecuteController(string action, Exception exception)
        {
            Response.Clear();
            Server.ClearError();

            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;
            routeData.Values["exception"] = exception;

            IController controller = new ErrorController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            controller.Execute(rc);
        }

        private void Log(Exception exception)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error(exception);
        }
    }
#pragma warning restore SA1649 // File name must match first type name
}
