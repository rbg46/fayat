using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Database.ImportExport;

namespace Fred.DesignPatterns.DI.EntityFramework
{
    public class ImportExportUnitOfWorkRegistrar : DependencyRegistrar
    {
        public ImportExportUnitOfWorkRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<ImportExportContext>();

            DependencyInjectionService.RegisterType<IUnitOfWork, UnitOfWork>();
        }
    }
}
