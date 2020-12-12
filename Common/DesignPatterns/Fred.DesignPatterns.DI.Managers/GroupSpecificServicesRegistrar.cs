using Fred.Business.CI;
using Fred.Business.GroupeFeature.Services;
using Fred.Business.OperationDiverse;
using Fred.Business.Rapport;
using Fred.Business.Reception.Validators;
using Fred.Business.Reception.Validators.GFTP;
using Fred.Business.Referential.Materiel;
using Fred.Business.RepartitionEcart;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.GroupSpecific.Default;
using Fred.GroupSpecific.Default.ExternalServices;
using Fred.GroupSpecific.Default.Societe;
using Fred.GroupSpecific.Fes;
using Fred.GroupSpecific.Fes.ExternalServices;
using Fred.GroupSpecific.Fes.Societe;
using Fred.GroupSpecific.Ftp.ExternalServices;
using Fred.GroupSpecific.Ftp.Societe;
using Fred.GroupSpecific.Rzb.ExternalServices;
using Fred.GroupSpecific.Rzb.Societe;
using Fred.ImportExport.Business.Reception.Migo.Service;

namespace Fred.DesignPatterns.DI.Managers
{
    public class GroupSpecificServicesRegistrar : DependencyRegistrar
    {
        public GroupSpecificServicesRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<ISocieteManager, DefaultSocieteManager>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<ISocieteManager, FesSocieteManager>(Constantes.CodeGroupeFES);
            DependencyInjectionService.RegisterType<ISocieteManager, FtpSocieteManager>(Constantes.CodeGroupeFTP);
            DependencyInjectionService.RegisterType<ISocieteManager, RzbSocieteManager>(Constantes.CodeGroupeRZB);
            DependencyInjectionService.RegisterFactory<ISocieteManager>();

            DependencyInjectionService.RegisterType<ICIManager, DefaultCIManager>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<ICIManager, FesCIManager>(Constantes.CodeGroupeFES);
            DependencyInjectionService.RegisterFactory<ICIManager>();

            DependencyInjectionService.RegisterType<IImportExportServiceDescriptor, DefaultImportExportServiceDescriptor>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IImportExportServiceDescriptor, FesImportExportServiceDescriptor>(Constantes.CodeGroupeFES);
            DependencyInjectionService.RegisterType<IImportExportServiceDescriptor, FtpImportExportServiceDescriptor>(Constantes.CodeGroupeFTP);
            DependencyInjectionService.RegisterType<IImportExportServiceDescriptor, RzbImportExportServiceDescriptor>(Constantes.CodeGroupeRZB);
            DependencyInjectionService.RegisterFactory<IImportExportServiceDescriptor>();

            DependencyInjectionService.RegisterType<IReceptionQuantityRulesValidator, ReceptionQuantityRulesValidator>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IReceptionQuantityRulesValidator, ReceptionQuantityRulesValidatorFtp>(Constantes.CodeGroupeFTP);
            DependencyInjectionService.RegisterFactory<IReceptionQuantityRulesValidator>();

            DependencyInjectionService.RegisterType<IGroupeFeatureService, GroupeFeatureService>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IGroupeFeatureService, GroupeFeatureServiceFtp>(Constantes.CodeGroupeFTP);
            DependencyInjectionService.RegisterFactory<IGroupeFeatureService>();

            DependencyInjectionService.RegisterType<IMigoService, MigoService>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IMigoService, MigoServiceFtp>(Constantes.CodeGroupeFTP);
            DependencyInjectionService.RegisterFactory<IMigoService>();

            DependencyInjectionService.RegisterType<IMaterielManager, DefaultMaterielManager>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IMaterielManager, MaterielManagerFES>(Constantes.CodeGroupeFES);
            DependencyInjectionService.RegisterFactory<IMaterielManager>();

            DependencyInjectionService.RegisterType<IOperationDiverseManager, DefaultOperationDiverseManager>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IOperationDiverseManager, RzbOperationDiverseManager>(Constantes.CodeGroupeRZB);
            DependencyInjectionService.RegisterFactory<IOperationDiverseManager>();

            DependencyInjectionService.RegisterType<ICrudFeature, CrudFeature>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<ICrudFeature, CrudFeatureFes>(Constantes.CodeGroupeFES);
            DependencyInjectionService.RegisterFactory<ICrudFeature>();

            DependencyInjectionService.RegisterType<ISearchFeature, SearchFeature>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<ISearchFeature, SearchFeatureFes>(Constantes.CodeGroupeFES);
            DependencyInjectionService.RegisterFactory<ISearchFeature>();

            DependencyInjectionService.RegisterType<IConsolidationManager, DefaultConsolidationManager>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IConsolidationManager, RzbConsolidationManager>(Constantes.CodeGroupeRZB);
            DependencyInjectionService.RegisterFactory<IConsolidationManager>();

            DependencyInjectionService.RegisterType<IRepartitionEcartManager, DefaultRepartitionEcartManager>(Constantes.CodeGroupeDefault);
            DependencyInjectionService.RegisterType<IRepartitionEcartManager, RzbRepartitionEcartManager>(Constantes.CodeGroupeRZB);
            DependencyInjectionService.RegisterFactory<IRepartitionEcartManager>();
        }
    }
}
