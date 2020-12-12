using Fred.Notifications.Interfaces;
using Fred.SignalR;
using Microsoft.AspNet.SignalR;

namespace Fred.DesignPatterns.DI.SignalR
{
    public class SignalRRegistrar : DependencyRegistrar
    {
        public SignalRRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterInstance<IUserIdProvider>(new UserLoginIdProvider());
            DependencyInjectionService.RegisterType<INotificationService, SignalRNotificationService>();
        }
    }
}
