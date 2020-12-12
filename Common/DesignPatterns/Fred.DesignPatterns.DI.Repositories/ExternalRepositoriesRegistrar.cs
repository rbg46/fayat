using Fred.DataAccess.ExternalService.FredImportExport.Ci;
using Fred.DataAccess.ExternalService.FredImportExport.Commande;
using Fred.DataAccess.ExternalService.FredImportExport.CommandeLigne;
using Fred.DataAccess.ExternalService.FredImportExport.EcritureComptable;
using Fred.DataAccess.ExternalService.FredImportExport.Fournisseur;
using Fred.DataAccess.ExternalService.FredImportExport.Materiel;
using Fred.DataAccess.ExternalService.FredImportExport.Moyen;
using Fred.DataAccess.ExternalService.FredImportExport.Notification;
using Fred.DataAccess.ExternalService.FredImportExport.Personnel;
using Fred.DataAccess.ExternalService.FredImportExport.Rapport;
using Fred.DataAccess.ExternalService.FredImportExport.Reception;
using Fred.DataAccess.ExternalService.FredImportExport.ValidationPointage;

namespace Fred.DesignPatterns.DI.Repositories
{
    public class ExternalRepositoriesRegistrar : DependencyRegistrar
    {
        public ExternalRepositoriesRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<ICIRepositoryExterne, CIRepositoryExterne>();
            DependencyInjectionService.RegisterType<ICommandeRepositoryExterne, CommandeRepositoryExterne>();
            DependencyInjectionService.RegisterType<ICommandeLigneRepositoryExterne, CommandeLigneRepositoryExterne>();
            DependencyInjectionService.RegisterType<IEcritureComptableRepositoryExterne, EcritureComptableRepositoryExterne>();
            DependencyInjectionService.RegisterType<IFournisseurRepositoryExterne, FournisseurRepositoryExterne>();
            DependencyInjectionService.RegisterType<IMaterielRepositoryExterne, MaterielRepositoryExterne>();
            DependencyInjectionService.RegisterType<IMoyenRepositoryExterne, MoyenRepositoryExterne>();
            DependencyInjectionService.RegisterType<INotificationRepositoryExterne, NotificationRepositoryExterne>();
            DependencyInjectionService.RegisterType<IPersonnelRepositoryExterne, PersonnelRepositoryExterne>();
            DependencyInjectionService.RegisterType<IRapportRepositoryExterne, RapportRepositoryExterne>();
            DependencyInjectionService.RegisterType<IReceptionRepositoryExterne, ReceptionRepositoryExterne>();
            DependencyInjectionService.RegisterType<IValidationPointageRepositoryExterne, ValidationPointageRepositoryExterne>();
        }
    }
}
