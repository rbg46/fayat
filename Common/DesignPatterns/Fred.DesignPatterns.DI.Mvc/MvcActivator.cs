using System.Linq;
using System.Web.Mvc;
using CommonServiceLocator;

namespace Fred.DesignPatterns.DI.Mvc
{
    public static class MvcActivator
    {
        public static void Start(IFilterProvider filterAttributeFilterProvider, IDependencyResolver dependencyResolver, IServiceLocator serviceLocator)
        {
            SetDefaultFilterProvider(filterAttributeFilterProvider);
            DependencyResolver.SetResolver(dependencyResolver);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        private static void SetDefaultFilterProvider(IFilterProvider filterAttributeFilterProvider)
        {
            RemoveDefaultFilterProvider();
            AddFilterProvider();

            void RemoveDefaultFilterProvider()
            {
                FilterAttributeFilterProvider defaultFilterProvider = FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First();

                FilterProviders.Providers.Remove(defaultFilterProvider);
            }

            void AddFilterProvider() => FilterProviders.Providers.Add(filterAttributeFilterProvider);
        }
    }
}