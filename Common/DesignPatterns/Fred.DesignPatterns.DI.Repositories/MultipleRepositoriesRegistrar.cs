using Fred.DataAccess.ActivitySummary;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Organisation.Tree.Repository;
using Fred.DataAccess.PieceJointe;
using Fred.DataAccess.Rapport.Pointage.FredIe;
using Fred.DataAccess.RepriseDonnees;
using Fred.DataAccess.RepriseDonnees.Ci;
using Fred.DataAccess.RepriseDonnees.Commande;
using Fred.DataAccess.RepriseDonnees.IndemniteDeplacement;
using Fred.DataAccess.RepriseDonnees.Materiel;
using Fred.DataAccess.RepriseDonnees.OperationDiverse;
using Fred.DataAccess.RepriseDonnees.Personnel;
using Fred.DataAccess.RepriseDonnees.PlanTaches;
using Fred.DataAccess.RepriseDonnees.Rapport;
using Fred.DataAccess.RepriseDonnees.ValidationCommande;
using Fred.DataAccess.VerificationPointage;

namespace Fred.DesignPatterns.DI.Repositories
{
    public class MultipleRepositoriesRegistrar : DependencyRegistrar
    {
        public MultipleRepositoriesRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<IActivitySummaryRepository, ActivitySummaryRepository>();
            DependencyInjectionService.RegisterType<IChekingPointingReposity, ChekingPointingReposity>();
            DependencyInjectionService.RegisterType<IFredIePointageFluxRepository, FredIePointageFluxRepository>();
            DependencyInjectionService.RegisterType<IOrganisationTreeRepository, OrganisationTreeRepository>();
            DependencyInjectionService.RegisterType<IPieceJointeStockagePhysiqueRepository, PieceJointeStockagePhysiqueRepository>();
            DependencyInjectionService.RegisterType<IRepriseCiRepository, RepriseCiRepository>();
            DependencyInjectionService.RegisterType<IRepriseCommandeRepository, RepriseCommandeRepository>();
            DependencyInjectionService.RegisterType<IRepriseDonneesRepository, RepriseDonneesRepository>();
            DependencyInjectionService.RegisterType<IRepriseIndemniteDeplacementRepository, RepriseIndemniteDeplacementRepository>();
            DependencyInjectionService.RegisterType<IRepriseMaterielRepository, RepriseMaterielRepository>();
            DependencyInjectionService.RegisterType<IRepriseODRepository, RepriseODRepository>();
            DependencyInjectionService.RegisterType<IReprisePersonnelRepository, ReprisePersonnelRepository>();
            DependencyInjectionService.RegisterType<IReprisePlanTachesRepository, ReprisePlanTachesRepository>();
            DependencyInjectionService.RegisterType<IRepriseRapportRepository, RepriseRapportRepository>();
            DependencyInjectionService.RegisterType<IRepriseValidationCommandeRepository, RepriseValidationCommandeRepository>();
        }
    }
}
