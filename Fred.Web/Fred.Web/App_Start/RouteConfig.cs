using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fred.Web
{
  public static class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      

      routes.MapRoute(
        name: "Default",
        url: "{controller}/{action}/{id}",
        defaults: new { controller = "Authentication", action = "Connect", id = UrlParameter.Optional });

      routes.MapRoute(
        name: "CommandeDetail",
        url: "{controller}/{action}/{id}",
        defaults: new { controller = "Commande", action = "Detail", id = UrlParameter.Optional });

      routes.MapRoute(
        name: "ListeCommandes",
        url: "{controller}/{action}/{id}",
        defaults: new { controller = "Commande", action = "Commande", id = UrlParameter.Optional });
    }
  }
}
