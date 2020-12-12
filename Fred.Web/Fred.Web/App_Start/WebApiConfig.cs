using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Fred.Web.App_Start;
using Fred.Web.Middlewares;

namespace Fred.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new PassthroughExceptionHandler());
            config.Filters.Add(new ValidateModelAttribute());

            // Web API configuration and services
            GlobalConfiguration.Configuration.BindParameter(typeof(string), new EmptyStringOrNullModelBinder());

            //Mise en place sur tous les controlleurs d'un filtre qui demande d'etre connecter pour acceder aux web api.
            config.Filters.Add(new AuthorizeAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
              name: "DefaultApi",
              routeTemplate: "api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional });
        }
    }
}
