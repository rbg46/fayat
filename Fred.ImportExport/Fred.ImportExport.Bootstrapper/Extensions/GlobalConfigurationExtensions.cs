using System;
using Fred.DesignPatterns.DI;
using Hangfire;
using Hangfire.Annotations;

namespace Fred.ImportExport.Bootstrapper.Extensions
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration UseDependencyActivator(
            [NotNull] this IGlobalConfiguration configuration,
            IDependencyInjectionService dependencyInjectionService)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (dependencyInjectionService == null)
                throw new ArgumentNullException(nameof(dependencyInjectionService));

            // https://stackoverflow.com/a/28034217/2277311
            return configuration.UseActivator(new DependencyJobActivator(dependencyInjectionService));
        }

        public class DependencyJobActivator : JobActivator
        {
            private readonly IDependencyInjectionService dependencyInjectionService;

            public DependencyJobActivator(IDependencyInjectionService dependencyInjectionService)
            {
                this.dependencyInjectionService = dependencyInjectionService
                    ?? throw new ArgumentNullException(nameof(dependencyInjectionService));
            }

            public override object ActivateJob(Type jobType)
            {
                return dependencyInjectionService.Resolve(jobType);
            }
        }
    }
}