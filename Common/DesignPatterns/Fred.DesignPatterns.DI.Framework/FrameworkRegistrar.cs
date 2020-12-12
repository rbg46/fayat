using Fred.Framework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Security;
using Fred.Framework.Services.Google;
using Fred.Framework.Tool;

namespace Fred.DesignPatterns.DI.Framework
{
    public class FrameworkRegistrar : DependencyRegistrar
    {
        public FrameworkRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<ICacheManager, CacheManager>();
            DependencyInjectionService.RegisterType<IDateTimeExtendManager, DateTimeExtendManager>();
            DependencyInjectionService.RegisterType<IGeocodeService, GeocodeService>();
            DependencyInjectionService.RegisterType<ILogManager, LogManager>();
            DependencyInjectionService.RegisterType<ISecurityManager, SecurityManager>();
            DependencyInjectionService.RegisterType<IToolManager, ToolManager>();
        }
    }
}
