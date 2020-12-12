using System;

namespace Fred.DesignPatterns.DI
{
    public interface IDependencyInjectionRegistration
    {
        string Name { get; }
        Type MappedToType { get; }
    }
}