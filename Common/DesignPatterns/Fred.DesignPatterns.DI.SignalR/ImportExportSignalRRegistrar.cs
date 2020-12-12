using Fred.Notifications.Interfaces;
using Fred.SignalR;
using Microsoft.AspNet.SignalR;

namespace Fred.DesignPatterns.DI.SignalR
{
    public class ImportExportSignalRRegistrar : DependencyRegistrar
    {
        public ImportExportSignalRRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterInstance<IUserIdProvider>(new UserQueryStringIdProvider());

            DependencyInjectionService.RegisterType<INotificationService, SignalRNotificationService>();
        }
    }
}
