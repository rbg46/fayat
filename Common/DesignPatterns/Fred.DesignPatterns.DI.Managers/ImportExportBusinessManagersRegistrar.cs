using Fred.Business.Budget;
using Fred.Business.Budget.Details;
using Fred.Business.CI;
using Fred.Business.Groupe;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.ImportExport.Business;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Commande;
using Fred.ImportExport.Business.CommandeLigne.VME22;
using Fred.ImportExport.Business.Depense;
using Fred.ImportExport.Business.Etablissement.Etl.Process;
using Fred.ImportExport.Business.Facturation.Validators;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Groupe;
using Fred.ImportExport.Business.JournauxComptable;
using Fred.ImportExport.Business.Kilometre;
using Fred.ImportExport.Business.Log;
using Fred.ImportExport.Business.Materiel;
using Fred.ImportExport.Business.Materiel.ExportMaterielToSap;
using Fred.ImportExport.Business.Materiel.SocieteCodeImportMateriel;
using Fred.ImportExport.Business.Pointage.Personnel.PointagePersonnelEtl;
using Fred.ImportExport.Business.Reception.Migo;
using Fred.ImportExport.Business.Reception.RMigo;
using Fred.ImportExport.Business.ReceptionInterimaire;
using Fred.ImportExport.Business.RepriseDonnees;
using Fred.ImportExport.Business.Utilisateur;
using Fred.ImportExport.Business.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Fes;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Fes.Logs;
using Fred.ImportExport.Business.ValidationPointage.Fon;
using Fred.ImportExport.Business.ValidationPointage.Fon.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Fon.Logs;
using Fred.ImportExport.Business.ValidationPointage.Ftp;
using Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac;
using Fred.ImportExport.Business.ValidationPointage.Rzb;
using Fred.ImportExport.Business.WorkflowLogicielTiers;

namespace Fred.DesignPatterns.DI.Managers
{
    public class ImportExportBusinessManagersRegistrar : DependencyRegistrar
    {
        public ImportExportBusinessManagersRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<CIFluxManager>();
            DependencyInjectionService.RegisterType<CleaningOutgoingUsersFluxManager>();
            DependencyInjectionService.RegisterType<ControleHelper>();
            DependencyInjectionService.RegisterType<ControleVrac>();
            DependencyInjectionService.RegisterType<ControleVracFes>();
            DependencyInjectionService.RegisterType<ControleVracFtp>();
            DependencyInjectionService.RegisterType<ControleVracFon>();
            DependencyInjectionService.RegisterType<DepenseFluxManager>();
            DependencyInjectionService.RegisterType<EtablissementComptableFluxManager>();
            DependencyInjectionService.RegisterType<FluxFb60Importer>();
            DependencyInjectionService.RegisterType<FluxMiroImporter>();
            DependencyInjectionService.RegisterType<FluxMr11Importer>();
            DependencyInjectionService.RegisterType<FournisseurFluxManager>();
            DependencyInjectionService.RegisterType<JournauxComptableFluxManager>();
            DependencyInjectionService.RegisterType<JournauxComptableFluxManagerMoulins>();
            DependencyInjectionService.RegisterType<JournauxComptableFluxManagerRzb>();
            DependencyInjectionService.RegisterType<KlmFluxManager>();
            DependencyInjectionService.RegisterType<LogManager>();
            DependencyInjectionService.RegisterType<PersonnelFluxManager>();
            DependencyInjectionService.RegisterType<ReceptionInterimaireFluxManager>();

            DependencyInjectionService.RegisterType<RemonteeVrac>();
            DependencyInjectionService.RegisterType<RemonteeVracFes>();
            DependencyInjectionService.RegisterType<RemonteeVracFtp>();
            DependencyInjectionService.RegisterType<RemonteeVracFon>();
            DependencyInjectionService.RegisterType<RvgControleHelper>();

            DependencyInjectionService.RegisterType<IApplicationsSapManager, ApplicationsSapManager>();
            DependencyInjectionService.RegisterType<IBudgetDetailsExportExcelFeature, BudgetDetailsExportExcelFeature>();
            DependencyInjectionService.RegisterType<IBudgetMainManager, BudgetMainManager>();
            DependencyInjectionService.RegisterType<ICIManager, CIManager>();
            DependencyInjectionService.RegisterType<ICommandeFluxHelper, CommandeFluxHelper>();
            DependencyInjectionService.RegisterType<ICommandeStormImporter, CommandeStormImporter>();
            DependencyInjectionService.RegisterType<IContratInterimaireImportManager, ContratInterimaireImportManager>();
            DependencyInjectionService.RegisterType<IContratInterimaireManager, ContratInterimaireManager>();
            DependencyInjectionService.RegisterType<IEtablissementComptableEtlProcess, EtablissementComptableEtlProcess>();
            DependencyInjectionService.RegisterType<IEtatContratInterimaireManager, EtatContratInterimaireManager>();
            DependencyInjectionService.RegisterType<IExportMaterielToSapManager, ExportMaterielToSapManager>();
            DependencyInjectionService.RegisterType<IFluxManager, FluxManager>();
            DependencyInjectionService.RegisterType<IFournisseurManager, FournisseurManager>();
            DependencyInjectionService.RegisterType<IGroupeInterimaireManager, GroupeInterimaireManager>();
            DependencyInjectionService.RegisterType<IGroupeManager, GroupeManager>();
            DependencyInjectionService.RegisterType<ILogicielTiersManager, LogicielTiersManager>();
            DependencyInjectionService.RegisterType<IMigoManager, MigoManager>();
            DependencyInjectionService.RegisterType<IPaysManager, PaysManager>();
            DependencyInjectionService.RegisterType<IPersonnelManager, PersonnelManager>();
            DependencyInjectionService.RegisterType<IPointageFluxManager, PointageFluxManager>();
            DependencyInjectionService.RegisterType<IRMigoManager, RMigoManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneesViewManager, RepriseDonneesViewManager>();
            DependencyInjectionService.RegisterType<IServiceImportMateriel, ServiceImportMateriel>();
            DependencyInjectionService.RegisterType<ISocieteCodeImportMaterielManager, SocieteCodeImportMaterielManager>();
            DependencyInjectionService.RegisterType<ISocieteManager, SocieteManager>();
            DependencyInjectionService.RegisterType<IUniteManager, UniteManager>();
            DependencyInjectionService.RegisterType<IUtilisateurManager, UtilisateurManager>();
            DependencyInjectionService.RegisterType<IValidationPointageFluxManagerFes, ValidationPointageFluxManagerFes>();
            DependencyInjectionService.RegisterType<IValidationPointageFluxManagerFon, ValidationPointageFluxManagerFon>();
            DependencyInjectionService.RegisterType<IValidationPointageFluxManagerFtp, ValidationPointageFluxManagerFtp>();
            DependencyInjectionService.RegisterType<IValidationPointageFluxManagerRzb, ValidationPointageFluxManager>();
            DependencyInjectionService.RegisterType<IValidationPointageFesLogger, ValidationPointageFesLogger>();
            DependencyInjectionService.RegisterType<IValidationPointageFonLogger, ValidationPointageFonLogger>();
            DependencyInjectionService.RegisterType<IVme22FluxManager, Vme22FluxManager>();
            DependencyInjectionService.RegisterType<IWorkflowLogicielTiersManager, WorkflowLogicielTiersManager>();
            DependencyInjectionService.RegisterType<IWorkflowPointageManager, WorkflowPointageManager>();
        }
    }
}
