using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.Business.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Factory;
using Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces;

namespace Fred.DesignPatterns.DI.Transversal
{
    public class ImportExportBusinessFactoriesRegistrar : DependencyRegistrar
    {
        public ImportExportBusinessFactoriesRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<PersonnelEtlFactory>();

            DependencyInjectionService.RegisterType<IValidationPointageFluxFactory, ValidationPointageFluxFactory>();
            DependencyInjectionService.RegisterType<IValidationPointageSettingsProvider, ValidationPointageSettingsProvider>();
        }
    }
}
