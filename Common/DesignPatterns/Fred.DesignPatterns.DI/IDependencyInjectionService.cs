using System;
using System.Collections.Generic;

namespace Fred.DesignPatterns.DI
{
    public interface IDependencyInjectionService : IDependencyInjectionRegistrar, IDependencyInjectionResolver
    {
        bool IsRegistered<T>(string name);
        bool IsRegistered(Type type);
        IEnumerable<IDependencyInjectionRegistration> GetRegistrations();
    }
}
