namespace Fred.DesignPatterns.DI
{
    public abstract class DependencyRegistrar
    {
        protected IDependencyInjectionService DependencyInjectionService { get; }

        protected DependencyRegistrar(IDependencyInjectionService dependencyInjectionService)
        {
            DependencyInjectionService = dependencyInjectionService;
        }

        public abstract void RegisterTypes();
    }
}
