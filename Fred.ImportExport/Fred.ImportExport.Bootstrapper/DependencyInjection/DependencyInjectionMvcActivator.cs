using Fred.DesignPatterns.DI.Mvc;
using Unity;
using Unity.AspNet.Mvc;
using Unity.ServiceLocation;

namespace Fred.ImportExport.Bootstrapper.DependencyInjection
{
    public static class DependencyInjectionMvcActivator
    {
        private static IUnityContainer container;

        public static void Start()
        {
            container = DependencyInjectionConfig.Container;

            var unityFilterAttributeFilterProvider = new UnityFilterAttributeFilterProvider(container);
            var unityDependencyResolver = new UnityDependencyResolver(container);
            var unityServiceLocator = new UnityServiceLocator(container);

            MvcActivator.Start(unityFilterAttributeFilterProvider, unityDependencyResolver, unityServiceLocator);
        }

        public static void Shutdown()
        {
            container.Dispose();
        }
    }
}
