using System;
using System.Collections.Generic;

namespace Fred.DesignPatterns.DI
{
    public interface IDependencyInjectionResolver
    {
        T Resolve<T>();
        T Resolve<T>(string name);
        object Resolve(Type type);
        IEnumerable<object> ResolveAll(Type type);
    }
}
