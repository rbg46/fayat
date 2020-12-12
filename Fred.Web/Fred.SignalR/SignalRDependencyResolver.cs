using System;
using System.Collections.Generic;
using Fred.DesignPatterns.DI;
using Microsoft.AspNet.SignalR;

namespace Fred.SignalR
{
    public class SignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly IDependencyInjectionService dependencyInjectionService;

        public SignalRDependencyResolver(IDependencyInjectionService dependencyInjectionService)
        {
            this.dependencyInjectionService = dependencyInjectionService;
        }

        public override object GetService(Type serviceType)
        {
            if (dependencyInjectionService.IsRegistered(serviceType))
                return dependencyInjectionService.Resolve(serviceType);

            return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            if (dependencyInjectionService.IsRegistered(serviceType))
                return dependencyInjectionService.ResolveAll(serviceType);

            return base.GetServices(serviceType);
        }
    }
}
