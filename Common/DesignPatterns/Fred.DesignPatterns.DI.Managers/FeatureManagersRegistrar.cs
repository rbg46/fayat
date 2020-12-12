using Fred.Business.Budget.BudgetComparaison;
using Fred.Business.Budget.BudgetComparaison.ExcelExport;
using Fred.Business.IndemniteDeplacement;
using Fred.Business.Rapport;
using Fred.Business.Referential.IndemniteDeplacement.Features;

namespace Fred.DesignPatterns.DI.Managers
{
    public class FeatureManagersRegistrar : DependencyRegistrar
    {
        public FeatureManagersRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<IBudgetComparaisonExcelExporter, BudgetComparaisonExcelExporter>();
            DependencyInjectionService.RegisterType<IBudgetComparer, BudgetComparer>();
            DependencyInjectionService.RegisterType<ICalculFeature, CalculFeature>();
            DependencyInjectionService.RegisterType<Business.IndemniteDeplacement.ICrudFeature, Business.IndemniteDeplacement.CrudFeature>();
            DependencyInjectionService.RegisterType<ICrudWithCalculFeature, CrudWithCalculFeature>();
            DependencyInjectionService.RegisterType<Business.IndemniteDeplacement.ISearchFeature, Business.IndemniteDeplacement.SearchFeature>();
            DependencyInjectionService.RegisterType<Business.Rapport.ICrudFeature, Business.Rapport.CrudFeature>();
            DependencyInjectionService.RegisterType<Business.Rapport.ISearchFeature, Business.Rapport.SearchFeature>();
            DependencyInjectionService.RegisterType<IUtilitiesFeature, UtilitiesFeature>();
            DependencyInjectionService.RegisterType<IExportKlm, ExportKlm>();
        }
    }
}
