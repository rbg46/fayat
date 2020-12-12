using Fred.DesignPatterns.DI;
using Fred.Entities;

namespace Fred.GroupSpecific.GroupResolution
{
    public abstract class GroupAwareServiceResolver : IDependencyInjectionFactoryResolver
    {
        private readonly IDependencyInjectionService dependencyInjectionService;

        protected GroupAwareServiceResolver(IDependencyInjectionService dependencyInjectionService)
        {
            this.dependencyInjectionService = dependencyInjectionService;
        }

        public T Resolve<T>()
        {
            string currentGroupCode = GetCurrentGroupCode();

            bool isRegistered = dependencyInjectionService.IsRegistered<T>(currentGroupCode);
            if (!isRegistered)
            {
                currentGroupCode = Constantes.CodeGroupeDefault;
            }

            return dependencyInjectionService.Resolve<T>(currentGroupCode);
        }

        public abstract string GetCurrentGroupCode();
    }
}
