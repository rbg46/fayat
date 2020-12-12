using System;

namespace Fred.DesignPatterns.DI
{
    public interface IDependencyInjectionRegistrar
    {
        void RegisterFactory<T>();
        void RegisterInstance<TInterface>(TInterface instance);
        void RegisterInstance(Type t, object instance);
        void RegisterType<T>();
        void RegisterType<TFrom, TTo>() where TTo : TFrom;
        void RegisterType<TFrom, TTo>(string name) where TTo : TFrom;
        void RegisterType(Type fromType, Type toType);
    }
}