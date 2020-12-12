using Fred.Business.ExternalService;
using Fred.Business.ExternalService.Ci;
using Fred.Business.ExternalService.CommandeLigne;
using Fred.Business.ExternalService.EcritureComptable;
using Fred.Business.ExternalService.Materiel;
using Fred.Business.ExternalService.Notification;
using Fred.Business.ExternalService.Personnel;
using Fred.Business.ExternalService.Rapport;
using Fred.Business.ExternalService.Reception;
using Fred.Business.ExternalService.ValidationPointage;

namespace Fred.DesignPatterns.DI.Managers
{
    public class ExternalManagersRegistrar : DependencyRegistrar
    {
        public ExternalManagersRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<ICiManagerExterne, CiManagerExterne>();
            DependencyInjectionService.RegisterType<ICommandeEnergieManagerExterne, CommandeEnergieManagerExterne>();
            DependencyInjectionService.RegisterType<ICommandeLigneManagerExterne, CommandeLigneManagerExterne>();
            DependencyInjectionService.RegisterType<ICommandeManagerExterne, CommandeManagerExterne>();
            DependencyInjectionService.RegisterType<IEcritureComptableManagerExterne, EcritureComptableManagerExterne>();
            DependencyInjectionService.RegisterType<IFournisseurManagerExterne, FournisseurManagerExterne>();
            DependencyInjectionService.RegisterType<IMaterielManagerExterne, MaterielManagerExterne>();
            DependencyInjectionService.RegisterType<IMoyenManagerExterne, MoyenManagerExterne>();
            DependencyInjectionService.RegisterType<INotificationManagerExterne, NotificationManagerExterne>();
            DependencyInjectionService.RegisterType<IPersonnelManagerExterne, PersonnelManagerExterne>();
            DependencyInjectionService.RegisterType<IRapportManagerExterne, RapportManagerExterne>();
            DependencyInjectionService.RegisterType<IReceptionManagerExterne, ReceptionManagerExterne>();
            DependencyInjectionService.RegisterType<IValidationPointageManagerExterne, ValidationPointageManagerExterne>();
        }
    }
}
