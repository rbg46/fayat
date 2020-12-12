using Fred.Entities;
using Fred.GroupSpecific.Ftp.Societe;
using Fred.GroupSpecific.Rzb.Societe;
using Fred.ImportExport.Business.EcritureComptable.Interfaces;
using Fred.ImportExport.Business.Facturation.Validators;
using Fred.ImportExport.Business.Facturation.Validators.GFTP;
using Fred.ImportExport.Business.Reception.Migo.Service;

namespace Fred.DesignPatterns.DI.Managers
{
    public class ImportExportGroupSpecificServicesRegistrar : DependencyRegistrar
    {
        public ImportExportGroupSpecificServicesRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<IEcritureComptableFluxManager, MoulinsEcritureComptableFluxManager>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IEcritureComptableFluxManager, RzbEcritureComptableFluxManager>(Constantes.CodeGroupeRZB);
            DependencyInjectionService.RegisterType<IEcritureComptableFluxManager, FtpEcritureComptableFluxManager>(Constantes.CodeGroupeFTP);
            DependencyInjectionService.RegisterFactory<IEcritureComptableFluxManager>();

            DependencyInjectionService.RegisterType<IFluxFB60ImporterValidator, FluxFB60ImporterValidator>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IFluxFB60ImporterValidator, FluxFB60ImporterValidatorFtp>(Constantes.CodeGroupeFTP);
            DependencyInjectionService.RegisterFactory<IFluxFB60ImporterValidator>();

            DependencyInjectionService.RegisterType<IMigoService, MigoService>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IMigoService, MigoServiceFtp>(Constantes.CodeGroupeFTP);
            DependencyInjectionService.RegisterFactory<IMigoService>();
        }
    }
}
