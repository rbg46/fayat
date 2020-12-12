using Fred.DesignPatterns.DI.WebApi;
using Unity;
using Unity.ServiceLocation;
using Unity.WebApi;

namespace Fred.Web.Bootstrapper.DependencyInjection
{
    public static class DependencyInjectionWebApiActivator
    {
        private static IUnityContainer container;

        public static void Start()
        {
            container = DependencyInjectionConfig.Container;

            var unityDependencyResolver = new UnityDependencyResolver(container);
            var unityServiceLocator = new UnityServiceLocator(container);

            WebApiActivator.Start(unityDependencyResolver, unityServiceLocator);
        }

        public static void Shutdown()
        {
            container.Dispose();
        }
    }
}