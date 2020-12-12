namespace Fred.DesignPatterns.DI
{
    public interface IDependencyInjectionFactoryResolver
    {
        T Resolve<T>();
    }
}
