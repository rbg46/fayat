using System.Web.Http;
using System.Web.Http.Dependencies;
using CommonServiceLocator;

namespace Fred.DesignPatterns.DI.WebApi
{
    public static class WebApiActivator
    {
        public static void Start(IDependencyResolver dependencyResolver, IServiceLocator serviceLocator)
        {
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }
    }
}