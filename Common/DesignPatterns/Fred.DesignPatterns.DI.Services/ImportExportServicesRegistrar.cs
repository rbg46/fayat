using Fred.ImportExport.Business;
using Fred.ImportExport.Business.CI;
using Fred.ImportExport.Business.CI.AnaelSystem;
using Fred.ImportExport.Business.CI.AnaelSystem.Context;
using Fred.ImportExport.Business.CI.AnaelSystem.Excel;
using Fred.ImportExport.Business.CI.AnaelSystem.Fred;
using Fred.ImportExport.Business.CI.AnaelSystem.Sap;
using Fred.ImportExport.Business.CI.WebApi;
using Fred.ImportExport.Business.CI.WebApi.Context;
using Fred.ImportExport.Business.CI.WebApi.Fred;
using Fred.ImportExport.Business.CI.WebApi.Validator;
using Fred.ImportExport.Business.ContratInterimaire;
using Fred.ImportExport.Business.Email;
using Fred.ImportExport.Business.Email.ActivitySummary;
using Fred.ImportExport.Business.Etablissement;
using Fred.ImportExport.Business.Fournisseur;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Excel;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Sap;
using Fred.ImportExport.Business.Personnel;
using Fred.ImportExport.Business.Personnel.AnaelSystem;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Validator;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Sap;
using Fred.ImportExport.Business.Reception.Services;

namespace Fred.DesignPatterns.DI.Services
{
    public class ImportExportServicesRegistrar : DependencyRegistrar
    {
        public ImportExportServicesRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<ICiFinderInWebApiSystemService, CiFinderInWebApiSystemService>();
            DependencyInjectionService.RegisterType<ICiFluxAnaelSystemService, CiFluxAnaelSystemService>();
            DependencyInjectionService.RegisterType<ICiSapSender, CiSapSender>();
            DependencyInjectionService.RegisterType<ICommonAnaelSystemContextProvider, CommonAnaelSystemContextProvider>();
            DependencyInjectionService.RegisterType<ImportExport.Business.CI.AnaelSystem.Context.Common.ICommonAnaelSystemContextProvider, ImportExport.Business.CI.AnaelSystem.Context.Common.CommonAnaelSystemContextProvider>();
            DependencyInjectionService.RegisterType<ImportExport.Business.Fournisseur.AnaelSystem.Context.Common.ICommonAnaelSystemContextProvider, ImportExport.Business.Fournisseur.AnaelSystem.Context.Common.CommonAnaelSystemContextProvider>();
            DependencyInjectionService.RegisterType<IConsistencyValidator, ConsistencyValidator>();
            DependencyInjectionService.RegisterType<IDateComptableModifierForMigo, DateComptableModifierForMigo>();
            DependencyInjectionService.RegisterType<IEmailActivitySummaryHanfireJob, EmailActivitySummaryHanfireJob>();
            DependencyInjectionService.RegisterType<IEmailScheduler, EmailScheduler>();
            DependencyInjectionService.RegisterType<IEtablissementComptableFluxService, EtablissementComptableFluxService>();
            DependencyInjectionService.RegisterType<IEtablissementValidator, EtablissementValidator>();
            DependencyInjectionService.RegisterType<IExcelCiExtractorService, ExcelCiExtractorService>();
            DependencyInjectionService.RegisterType<IExcelFournisseurExtractorService, ExcelFournisseurExtractorService>();
            DependencyInjectionService.RegisterType<IFournisseurFluxService, FournisseurFluxService>();
            DependencyInjectionService.RegisterType<IFournisseurSapSender, FournisseurSapSender>();
            DependencyInjectionService.RegisterType<IFredCiImportForWebApiService, FredCiImportForWebApiService>();
            DependencyInjectionService.RegisterType<IFredCiImporter, FredCiImporter>();
            DependencyInjectionService.RegisterType<IImportByPersonnelListContextProvider, ImportByPersonnelListContextProvider>();
            DependencyInjectionService.RegisterType<IImportByPersonnelListValidator, ImportByPersonnelListValidator>();
            DependencyInjectionService.RegisterType<IImportCiAnaelSystemManager, ImportCiAnaelSystemManager>();
            DependencyInjectionService.RegisterType<IImportCiByApiContextProvider, ImportCiByApiContextProvider>();
            DependencyInjectionService.RegisterType<IImportCiByCiListAnaelSystemContextProvider, ImportCiByCiListAnaelSystemContextProvider>();
            DependencyInjectionService.RegisterType<IImportCiByExcelAnaelSystemContextProvider, ImportCiByExcelAnaelSystemContextProvider>();
            DependencyInjectionService.RegisterType<IImportCiBySocieteAnaelSystemContextProvider, ImportCiBySocieteAnaelSystemContextProvider>();
            DependencyInjectionService.RegisterType<IImportCiByWebApiValidator, ImportCiByWebApiValidator>();
            DependencyInjectionService.RegisterType<IImportCiWebApiSystemService, ImportCiWebApiSystemService>();
            DependencyInjectionService.RegisterType<IImportFournisseurAnaelSystemManager, ImportFournisseurAnaelSystemManager>();
            DependencyInjectionService.RegisterType<IImportFournisseurByExcelAnaelSystemContextProvider, ImportFournisseurByExcelAnaelSystemContextProvider>();
            DependencyInjectionService.RegisterType<IImportFournisseurByFournisseurListAnaelSystemContextProvider, ImportFournisseurByFournisseurListAnaelSystemContextProvider>();
            DependencyInjectionService.RegisterType<IImportPersonnelAnaelSystemManager, ImportPersonnelAnaelSystemManager>();
            DependencyInjectionService.RegisterType<IImportPersonnelBySocieteContextProvider, ImportPersonnelBySocieteContextProvider>();
            DependencyInjectionService.RegisterType<IImportPersonnelsBySocieteValidator, ImportPersonnelsBySocieteValidator>();
            DependencyInjectionService.RegisterType<ILoggingAdministratorService, LoggingAdministratorService>();
            DependencyInjectionService.RegisterType<IOrganisationManagerOnImportCiService, OrganisationManagerOnImportCiService>();
            DependencyInjectionService.RegisterType<IPersonnelFluxAnaelSystemService, PersonnelFluxAnaelSystemService>();
            DependencyInjectionService.RegisterType<IPersonnelSapSender, PersonnelSapSender>();
            DependencyInjectionService.RegisterType<ISocieteValidator, SocieteValidator>();
            DependencyInjectionService.RegisterType<IContratInterimaireFluxTibcoSystemService, ContratInterimaireFluxTibcoSystemService>();
        }
    }
}
