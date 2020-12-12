using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.EntityFramework;

namespace Fred.DesignPatterns.DI.EntityFramework
{
    public class UnitOfWorkRegistrar : DependencyRegistrar
    {
        public UnitOfWorkRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<FredDbContext>();

            DependencyInjectionService.RegisterType<IUnitOfWork, UnitOfWork>();
        }
    }
}
