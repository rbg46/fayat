using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Personnel.Interimaire;
using Fred.ImportExport.DataAccess.ContratInterimaire;
using Fred.ImportExport.DataAccess.Flux;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.DataAccess.SocieteCodeImportMateriel;
using Fred.ImportExport.DataAccess.Transposition;
using Fred.ImportExport.DataAccess.WorkflowLogicielTiers;

namespace Fred.DesignPatterns.DI.Repositories
{
    public class ImportExportRepositoriesRegistrar : DependencyRegistrar
    {
        public ImportExportRepositoriesRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<IContratInterimaireImportRepository, ContratInterimaireImportRepository>();
            DependencyInjectionService.RegisterType<IEtatContratInterimaireRepository, EtatContratInterimaireRepository>();
            DependencyInjectionService.RegisterType<IFluxRepository, FluxRepository>();
            DependencyInjectionService.RegisterType<IInterfaceTransfertDonneeRepository, InterfaceTransfertDonneeRepository>();
            DependencyInjectionService.RegisterType<ILogicielTiersRepository, LogicielTiersRepository>();
            DependencyInjectionService.RegisterType<ISocieteCodeImportMaterielRepository, SocieteCodeImportMaterielRepository>();
            DependencyInjectionService.RegisterType<ITranspoCodeEmploiToRessourceRepository, TranspoCodeEmploiToRessourceRepository>();
            DependencyInjectionService.RegisterType<IWorkflowLogicielTiersRepository, WorkflowLogicielTiersRepository>();
            DependencyInjectionService.RegisterType<IWorkflowPointageRepository, WorkflowPointageRepository>();
        }
    }
}
