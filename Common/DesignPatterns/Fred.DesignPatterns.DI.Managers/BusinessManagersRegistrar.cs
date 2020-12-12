﻿using Fred.Business;
using Fred.Business.Achat.Calculation;
using Fred.Business.Achat.Calculation.Interfaces;
using Fred.Business.Affectation;
using Fred.Business.AffectationMoyen;
using Fred.Business.AffectationSeuilOrga;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.Astreinte;
using Fred.Business.Authentification;
using Fred.Business.Avenant;
using Fred.Business.Avis;
using Fred.Business.BaremeExploitation;
using Fred.Business.BaremeExploitation.Edition;
using Fred.Business.Budget;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.Avancement.Excel;
using Fred.Business.Budget.BibliothequePrix;
using Fred.Business.Budget.BudgetComparaison;
using Fred.Business.Budget.BudgetManager;
using Fred.Business.Budget.ControleBudgetaire;
using Fred.Business.Budget.Recette;
using Fred.Business.Carburant;
using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.Commande.Services;
using Fred.Business.CommandeEnergies;
using Fred.Business.CompteExploitation.BilanTechnique;
using Fred.Business.CompteExploitation.Excel13C4C.Managers;
using Fred.Business.CompteExploitation.Excel13C4C.Managers.Interfaces;
using Fred.Business.CompteExploitation.TableauSynthese;
using Fred.Business.CompteExploitation.TableauSynthese.Interfaces;
using Fred.Business.CompteExploitation.TableauSynthese.Services;
using Fred.Business.CompteExploitation.TableauSynthese.Services.Rows;
using Fred.Business.DatesCalendrierPaie;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Delegation;
using Fred.Business.Depense;
using Fred.Business.DepenseGlobale;
using Fred.Business.Directory;
using Fred.Business.EcritureComptable;
using Fred.Business.EcritureComptable.Import;
using Fred.Business.Email.Subscription;
using Fred.Business.Equipe;
using Fred.Business.EtatPaie;
using Fred.Business.EtatPaie.ControlePointage;
using Fred.Business.ExplorateurDepense;
using Fred.Business.Facturation;
using Fred.Business.Facture;
using Fred.Business.Favori;
using Fred.Business.FeatureFlipping;
using Fred.Business.Fonctionnalite;
using Fred.Business.FonctionnaliteDesactive;
using Fred.Business.Groupe;
using Fred.Business.Habilitation;
using Fred.Business.Habilitation.Core;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.Holding;
using Fred.Business.Images;
using Fred.Business.Import;
using Fred.Business.IndemniteDeplacement;
using Fred.Business.Journal;
using Fred.Business.Log;
using Fred.Business.Module;
using Fred.Business.ModuleDesactive;
using Fred.Business.Moyen;
using Fred.Business.Notification;
using Fred.Business.ObjectifFlash;
using Fred.Business.ObjectifFlash.Reporting;
using Fred.Business.OperationDiverse;
using Fred.Business.Organisation;
using Fred.Business.Parametre;
using Fred.Business.Params;
using Fred.Business.PermissionFonctionnalite;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Excel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Rapport.Pointage.PointagePersonnel;
using Fred.Business.Rapport.Pointage.PointagePersonnel.Interfaces;
using Fred.Business.Rapport.Pointage.Validation;
using Fred.Business.Rapport.Pointage.Validation.Interfaces;
using Fred.Business.Rapport.RapportHebdo;
using Fred.Business.RapportPrime;
using Fred.Business.RapportStatut;
using Fred.Business.RapportTache;
using Fred.Business.Reception;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeAbsence;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Business.Referential.CodeZoneDeplacement;
using Fred.Business.Referential.IndemniteDeplacement.Features;
using Fred.Business.Referential.Materiel;
using Fred.Business.Referential.Nature;
using Fred.Business.Referential.StatutAbsence;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielEtendu;
using Fred.Business.ReferentielFixe;
using Fred.Business.RepartitionEcart;
using Fred.Business.RepriseDonnees;
using Fred.Business.RepriseDonnees.Ci;
using Fred.Business.RepriseDonnees.Commande;
using Fred.Business.RepriseDonnees.IndemniteDeplacement;
using Fred.Business.RepriseDonnees.Materiel;
using Fred.Business.RepriseDonnees.Personnel;
using Fred.Business.RepriseDonnees.PlanTaches;
using Fred.Business.RepriseDonnees.Rapport;
using Fred.Business.RepriseDonnees.ValidationCommande;
using Fred.Business.RessourceRecommandee;
using Fred.Business.RessourcesSpecifiquesCI;
using Fred.Business.Role;
using Fred.Business.RoleFonctionnalite;
using Fred.Business.SeuilValidation.Manager;
using Fred.Business.Site;
using Fred.Business.Societe;
using Fred.Business.Societe.Classification;
using Fred.Business.Societe.Interfaces;
using Fred.Business.TypeOrganisation;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage;
using Fred.Business.ValidationPointage.Controles;
using Fred.Business.Valorisation;
using Fred.Business.VerificationPointage;

namespace Fred.DesignPatterns.DI.Managers
{
    public class BusinessManagersRegistrar : DependencyRegistrar
    {
        public BusinessManagersRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<AvancementLoader>();
            DependencyInjectionService.RegisterType<ControleChantier>();
            DependencyInjectionService.RegisterType<IndemniteDeplacementCalculator>();
            DependencyInjectionService.RegisterType<PlanTacheCopier>();
            DependencyInjectionService.RegisterType<RapportHebdoSaver>();

            DependencyInjectionService.RegisterType<IAffectationManager, AffectationManager>();
            DependencyInjectionService.RegisterType<IAffectationMoyenManager, AffectationMoyenManager>();
            DependencyInjectionService.RegisterType<IAffectationSeuilOrgaManager, AffectationSeuilOrgaManager>();
            DependencyInjectionService.RegisterType<IAffectationSeuilUtilisateurManager, AffectationSeuilUtilisateurManager>();
            DependencyInjectionService.RegisterType<IAssocieSepManager, AssocieSepManager>();
            DependencyInjectionService.RegisterType<IAstreinteManager, AstreinteManager>();
            DependencyInjectionService.RegisterType<IAuthentificationLogManager, AuthentificationLogManager>();
            DependencyInjectionService.RegisterType<IAuthentificationManager, AuthentificationManager>();
            DependencyInjectionService.RegisterType<IAvancementEtatManager, AvancementEtatManager>();
            DependencyInjectionService.RegisterType<IAvancementExportExcelManager, AvancementExportExcelManager>();
            DependencyInjectionService.RegisterType<IAvancementManager, AvancementManager>();
            DependencyInjectionService.RegisterType<IAvancementRecetteManager, AvancementRecetteManager>();
            DependencyInjectionService.RegisterType<IAvancementTacheManager, AvancementTacheManager>();
            DependencyInjectionService.RegisterType<IAvancementWorkflowManager, AvancementWorkflowManager>();
            DependencyInjectionService.RegisterType<IAvenantManager, AvenantManager>();
            DependencyInjectionService.RegisterType<IAvisManager, AvisManager>();
            DependencyInjectionService.RegisterType<IBaremeCiExcelHandler, BaremeCiExcelHandler>();
            DependencyInjectionService.RegisterType<IBaremeExploitationCIManager, BaremeExploitationCIManager>();
            DependencyInjectionService.RegisterType<IBaremeExploitationCISurchargeManager, BaremeExploitationCISurchargeManager>();
            DependencyInjectionService.RegisterType<IBaremeExploitationOrganisationManager, BaremeExploitationOrganisationManager>();
            DependencyInjectionService.RegisterType<IBaremeExportHelper, BaremeExportHelper>();
            DependencyInjectionService.RegisterType<IBaremeOrganisationExcelHelper, BaremeOrganisationExcelHelper>();
            DependencyInjectionService.RegisterType<IBilanFlashExportManager, BilanFlashExportManager>();
            DependencyInjectionService.RegisterType<IBilanFlashSyntheseExportManager, BilanFlashSyntheseExportManager>();
            DependencyInjectionService.RegisterType<IBilanTechniqueExportDataManager, BilanTechniqueExportDataManager>();
            DependencyInjectionService.RegisterType<IBilanTechniqueExportManager, BilanTechniqueExportManager>();
            DependencyInjectionService.RegisterType<IBudgetBibliothequePrixManager, BudgetBibliothequePrixManager>();
            DependencyInjectionService.RegisterType<IBudgetComparaisonManager, BudgetComparaisonManager>();
            DependencyInjectionService.RegisterType<IBudgetCopieManager, BudgetCopieManager>();
            DependencyInjectionService.RegisterType<IBudgetEtatManager, BudgetEtatManager>();
            DependencyInjectionService.RegisterType<IBudgetMainManager, BudgetMainManager>();
            DependencyInjectionService.RegisterType<IBudgetManager, BudgetManager>();
            DependencyInjectionService.RegisterType<IBudgetRecetteManager, BudgetRecetteManager>();
            DependencyInjectionService.RegisterType<IBudgetSousDetailManager, BudgetSousDetailManager>();
            DependencyInjectionService.RegisterType<IBudgetT4Manager, BudgetT4Manager>();
            DependencyInjectionService.RegisterType<IBudgetTacheManager, BudgetTacheManager>();
            DependencyInjectionService.RegisterType<IBudgetWorkflowManager, BudgetWorkflowManager>();
            DependencyInjectionService.RegisterType<ICIManager, CIManager>();
            DependencyInjectionService.RegisterType<ICIPrimeManager, CIPrimeManager>();
            DependencyInjectionService.RegisterType<IChekingPointingManager, ChekingPointingManager>();
            DependencyInjectionService.RegisterType<ICloturesPeriodesManager, CloturesPeriodesManager>();
            DependencyInjectionService.RegisterType<ICodeAbsenceManager, CodeAbsenceManager>();
            DependencyInjectionService.RegisterType<ICodeDeplacementManager, CodeDeplacementManager>();
            DependencyInjectionService.RegisterType<ICodeMajorationManager, CodeMajorationManager>();
            DependencyInjectionService.RegisterType<ICodeZoneDeplacementManager, CodeZoneDeplacementManager>();
            DependencyInjectionService.RegisterType<ICommandeContratInterimaireManager, CommandeContratInterimaireManager>();
            DependencyInjectionService.RegisterType<ICommandeEnergieLigneManager, CommandeEnergieLigneManager>();
            DependencyInjectionService.RegisterType<ICommandeEnergieManager, CommandeEnergieManager>();
            DependencyInjectionService.RegisterType<ICommandeLigneManager, CommandeLigneManager>();
            DependencyInjectionService.RegisterType<ICommandeManager, CommandeManager>();
            DependencyInjectionService.RegisterType<ICommandeTypeManager, CommandeTypeManager>();
            DependencyInjectionService.RegisterType<ICompteExploitation4CManager, CompteExploitation4CManager>();
            DependencyInjectionService.RegisterType<ICompteExploitation13CManager, CompteExploitation13CManager>();
            DependencyInjectionService.RegisterType<ICompteExploitation4CTotalsManager, CompteExploitation4CTotalsManager>();
            DependencyInjectionService.RegisterType<ICompteExploitation13CTotalsManager, CompteExploitation13CTotalsManager>();
            DependencyInjectionService.RegisterType<ICompteExploitation4CExcelManager, CompteExploitation4CExcelManager>();
            DependencyInjectionService.RegisterType<ICompteExploitation13CExcelManager, CompteExploitation13CExcelManager>();
            DependencyInjectionService.RegisterType<ICompteExploitation4CRowsManager, CompteExploitation4CRowsManager>();
            DependencyInjectionService.RegisterType<ICompteExploitation13CRowsManager, CompteExploitation13CRowsManager>();
            DependencyInjectionService.RegisterType<IConsolidationManager, ConsolidationManager>();
            DependencyInjectionService.RegisterType<IContratAndCommandeInterimaireGeneratorService, ContratAndCommandeInterimaireGeneratorService>();
            DependencyInjectionService.RegisterType<IContratInterimaireManager, ContratInterimaireManager>();
            DependencyInjectionService.RegisterType<IControleBudgetaireExcelManager, ControleBudgetaireExcelManager>();
            DependencyInjectionService.RegisterType<IControleBudgetaireManager, ControleBudgetaireManager>();
            DependencyInjectionService.RegisterType<IControlePointageManager, ControlePointageManager>();
            DependencyInjectionService.RegisterType<IControlePointagesFesManager, ControlePointagesFesManager>();
            DependencyInjectionService.RegisterType<IControlePointagesHebdomadaireManager, ControlePointagesHebdomadaireManager>();
            DependencyInjectionService.RegisterType<IControlePointagesManager, ControlePointagesManager>();
            DependencyInjectionService.RegisterType<ICopierBudgetSourceToCible, CopierBudgetSourceToCible>();
            DependencyInjectionService.RegisterType<IDatesCalendrierPaieManager, DatesCalendrierPaieManager>();
            DependencyInjectionService.RegisterType<IDatesClotureComptableManager, DatesClotureComptableManager>();
            DependencyInjectionService.RegisterType<IDelegationManager, DelegationManager>();
            DependencyInjectionService.RegisterType<IDepenseGlobaleManager, DepenseGlobaleManager>();
            DependencyInjectionService.RegisterType<IDepenseManager, DepenseManager>();
            DependencyInjectionService.RegisterType<IDepenseTypeManager, DepenseTypeManager>();
            DependencyInjectionService.RegisterType<IDeviseManager, DeviseManager>();
            DependencyInjectionService.RegisterType<IEcritureComptableCumulManager, EcritureComptableCumulManager>();
            DependencyInjectionService.RegisterType<IEcritureComptableImportManager, EcritureComptableImportManager>();
            DependencyInjectionService.RegisterType<IEcritureComptableManager, EcritureComptableManager>();
            DependencyInjectionService.RegisterType<IEcritureComptableRejetManager, EcritureComptableRejetManager>();
            DependencyInjectionService.RegisterType<IEmailSubscriptionManager, EmailSubscriptionManager>();
            DependencyInjectionService.RegisterType<IEquipeManager, EquipeManager>();
            DependencyInjectionService.RegisterType<IEtablissementComptableManager, EtablissementComptableManager>();
            DependencyInjectionService.RegisterType<IEtablissementPaieManager, EtablissementPaieManager>();
            DependencyInjectionService.RegisterType<IEtatPaieManager, EtatPaieManager>();
            DependencyInjectionService.RegisterType<IExplorateurDepenseManager, ExplorateurDepenseManager>();
            DependencyInjectionService.RegisterType<IExplorateurDepenseManagerFayatTP, ExplorateurDepenseManagerFayatTP>();
            DependencyInjectionService.RegisterType<IExplorateurDepenseManagerRzb, ExplorateurDepenseManagerRzb>();
            DependencyInjectionService.RegisterType<IExternalDirectoryManager, ExternalDirectoryManager>();
            DependencyInjectionService.RegisterType<IFacturationManager, FacturationManager>();
            DependencyInjectionService.RegisterType<IFacturationTypeManager, FacturationTypeManager>();
            DependencyInjectionService.RegisterType<IFactureManager, FactureManager>();
            DependencyInjectionService.RegisterType<IFamilleOperationDiverseExportExcelManager, FamilleOperationDiverseExportExcelManager>();
            DependencyInjectionService.RegisterType<IFamilleOperationDiverseManager, FamilleOperationDiverseManager>();
            DependencyInjectionService.RegisterType<IFavoriManager, FavoriManager>();
            DependencyInjectionService.RegisterType<IFeatureFlippingManager, FeatureFlippingManager>();
            DependencyInjectionService.RegisterType<IFonctionnaliteDesactiveManager, FonctionnaliteDesactiveManager>();
            DependencyInjectionService.RegisterType<IFonctionnaliteManager, FonctionnaliteManager>();
            DependencyInjectionService.RegisterType<IFournisseurManager, FournisseurManager>();
            DependencyInjectionService.RegisterType<IGroupeManager, GroupeManager>();
            DependencyInjectionService.RegisterType<IGroupeRemplacementTacheManager, GroupeRemplacementTacheManager>();
            DependencyInjectionService.RegisterType<IHabilitationCoreManager, HabilitationCoreManager>();
            DependencyInjectionService.RegisterType<IHabilitationForCiManager, HabilitationForCiManager>();
            DependencyInjectionService.RegisterType<IHabilitationForCommandeManager, HabilitationForCommandeManager>();
            DependencyInjectionService.RegisterType<IHabilitationForRapportManager, HabilitationForRapportManager>();
            DependencyInjectionService.RegisterType<IHabilitationManager, HabilitationManager>();
            DependencyInjectionService.RegisterType<IHoldingManager, HoldingManager>();
            DependencyInjectionService.RegisterType<IImageManager, ImageManager>();
            DependencyInjectionService.RegisterType<IImageSocietePathManager, ImageSocietePathManager>();
            DependencyInjectionService.RegisterType<IImportPersonnelManager, ImportPersonnelManager>();
            DependencyInjectionService.RegisterType<IIndemniteDeplacementManager, IndemniteDeplacementManager>();
            DependencyInjectionService.RegisterType<IJournalManager, JournalManager>();
            DependencyInjectionService.RegisterType<IListeAbsencesMensuellesManager, ListeAbsencesMensuellesManager>();
            DependencyInjectionService.RegisterType<IListeHeuresSpecifiquesManager, ListeHeuresSpecifiquesManager>();
            DependencyInjectionService.RegisterType<IListeIndemniteDeplacementManager, ListeIndemniteDeplacementManager>();
            DependencyInjectionService.RegisterType<IListePrimesManager, ListePrimesManager>();
            DependencyInjectionService.RegisterType<ILotFarManager, LotFarManager>();
            DependencyInjectionService.RegisterType<ILotPointageManager, LotPointageManager>();
            DependencyInjectionService.RegisterType<IMaintenanceManager, MaintenanceManager>();
            DependencyInjectionService.RegisterType<IMaterielExterneManager, MaterielExterneManager>();
            DependencyInjectionService.RegisterType<IMaterielLocationManager, MaterielLocationManager>();
            DependencyInjectionService.RegisterType<IMatriculeExterneManager, MatriculeExterneManager>();
            DependencyInjectionService.RegisterType<IModuleDesactiveManager, ModuleDesactiveManager>();
            DependencyInjectionService.RegisterType<IModuleManager, ModuleManager>();
            DependencyInjectionService.RegisterType<IMontantHtReceptionneService, MontantHtReceptionneService>();
            DependencyInjectionService.RegisterType<IMontantFactureService, MontantFactureService>();
            DependencyInjectionService.RegisterType<IMontantHtService, MontantHtService>();
            DependencyInjectionService.RegisterType<IMoyenManager, MoyenManager>();
            DependencyInjectionService.RegisterType<INLogManager, NLogManager>();
            DependencyInjectionService.RegisterType<INatureManager, NatureManager>();
            DependencyInjectionService.RegisterType<INotificationManager, NotificationManager>();
            DependencyInjectionService.RegisterType<IObjectifFlashBudgetManager, ObjectifFlashBudgetManager>();
            DependencyInjectionService.RegisterType<IObjectifFlashManager, ObjectifFlashManager>();
            DependencyInjectionService.RegisterType<IObjectifFlashTacheManager, ObjectifFlashTacheManager>();
            DependencyInjectionService.RegisterType<IOperationDiverseAbonnementManager, OperationDiverseAbonnementManager>();
            DependencyInjectionService.RegisterType<IOperationDiverseManager, OperationDiverseManager>();
            DependencyInjectionService.RegisterType<IOrganisationManager, OrganisationManager>();
            DependencyInjectionService.RegisterType<IParametrageCarburantManager, ParametrageCarburantManager>();
            DependencyInjectionService.RegisterType<IParametrageReferentielEtenduManager, ParametrageReferentielEtenduManager>();
            DependencyInjectionService.RegisterType<IParametreManager, ParametreManager>();
            DependencyInjectionService.RegisterType<IParamsManager, ParamsManager>();
            DependencyInjectionService.RegisterType<IPaysManager, PaysManager>();
            DependencyInjectionService.RegisterType<IPermissionFonctionnaliteManager, PermissionFonctionnaliteManager>();
            DependencyInjectionService.RegisterType<IPermissionManager, PermissionManager>();
            DependencyInjectionService.RegisterType<IPersonnelExcelManager, PersonnelExcelManager>();
            DependencyInjectionService.RegisterType<IPersonnelImageManager, PersonnelImageManager>();
            DependencyInjectionService.RegisterType<IPersonnelManager, PersonnelManager>();
            DependencyInjectionService.RegisterType<IPointageAnticipeManager, PointageAnticipeManager>();
            DependencyInjectionService.RegisterType<IPointageManager, PointageManager>();
            DependencyInjectionService.RegisterType<IPointagePersonnelGlobalDataProvider, PointagePersonnelGlobalDataProvider>();
            DependencyInjectionService.RegisterType<IPointagePersonnelTransformerForViewService, PointagePersonnelTransformerForViewService>();
            DependencyInjectionService.RegisterType<IPrimeManager, PrimeManager>();
            DependencyInjectionService.RegisterType<IRapportHebdoManager, RapportHebdoManager>();
            DependencyInjectionService.RegisterType<IRapportHebdoService, RapportHebdoService>();
            DependencyInjectionService.RegisterType<IRapportLigneErrorBuilder, RapportLigneErrorBuilder>();
            DependencyInjectionService.RegisterType<IRapportLignesValidationDataProvider, RapportLignesValidationDataProvider>();
            DependencyInjectionService.RegisterType<IRapportManager, RapportManager>();
            DependencyInjectionService.RegisterType<IRapportPrimeLigneManager, RapportPrimeLigneManager>();
            DependencyInjectionService.RegisterType<IRapportPrimeLignePrimeManager, RapportPrimeLignePrimeManager>();
            DependencyInjectionService.RegisterType<IRapportPrimeManager, RapportPrimeManager>();
            DependencyInjectionService.RegisterType<IRapportStatutManager, RapportStatutManager>();
            DependencyInjectionService.RegisterType<IRapportTacheManager, RapportTacheManager>();
            DependencyInjectionService.RegisterType<IReceptionManager, ReceptionManager>();
            DependencyInjectionService.RegisterType<IReferentielEtenduManager, ReferentielEtenduManager>();
            DependencyInjectionService.RegisterType<IReferentielFixeManager, ReferentielFixeManager>();
            DependencyInjectionService.RegisterType<IRemonteeVracManager, RemonteeVracManager>();
            DependencyInjectionService.RegisterType<IRemplacementTacheManager, RemplacementTacheManager>();
            DependencyInjectionService.RegisterType<IRepartitionEcartManager, RepartitionEcartManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneeManager, RepriseDonneeManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneesCiManager, RepriseDonneesCiManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneesCommandeManager, RepriseDonneesCommandeManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneesIndemniteDeplacementManager, RepriseDonneesIndemniteDeplacementManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneesMaterielManager, RepriseDonneesMaterielManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneesPersonnelManager, RepriseDonneesPersonnelManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneesPlanTachesManager, RepriseDonneesPlanTachesManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneesRapportManager, RepriseDonneesRapportManager>();
            DependencyInjectionService.RegisterType<IRepriseDonneesValidationCommandeManager, RepriseDonneesValidationCommandeManager>();
            DependencyInjectionService.RegisterType<IRessourceRecommandeeManager, RessourceRecommandeeManager>();
            DependencyInjectionService.RegisterType<IRessourcesSpecifiquesCiManager, RessourcesSpecifiquesCiManager>();
            DependencyInjectionService.RegisterType<IRoleFonctionnaliteManager, RoleFonctionnaliteManager>();
            DependencyInjectionService.RegisterType<IRoleManager, RoleManager>();
            DependencyInjectionService.RegisterType<ISalarieAcompteManager, SalarieAcompteManager>();
            DependencyInjectionService.RegisterType<ISeuilValidationManager, SeuilValidationManager>();
            DependencyInjectionService.RegisterType<ISiteManager, SiteManager>();
            DependencyInjectionService.RegisterType<ISocieteClassificationManager, SocieteClassificationManager>();
            DependencyInjectionService.RegisterType<ISoldeFarService, SoldeFarService>();
            DependencyInjectionService.RegisterType<IStatutAbsenceManager, StatutAbsenceManager>();
            DependencyInjectionService.RegisterType<IStatutCommandeManager, StatutCommandeManager>();
            DependencyInjectionService.RegisterType<ISystemeExterneManager, SystemeExterneManager>();
            DependencyInjectionService.RegisterType<ISystemeImportManager, SystemeImportManager>();
            DependencyInjectionService.RegisterType<ITableauSyntheseExportManager, TableauSyntheseExportManager>();
            DependencyInjectionService.RegisterType<ITableauSyntheseFraisGenerauxService, TableauSyntheseFraisGenerauxService>();
            DependencyInjectionService.RegisterType<ITableauSyntheseRecetteService, TableauSyntheseRecetteService>();
            DependencyInjectionService.RegisterType<ITableauSyntheseCalculatedFieldsService, TableauSyntheseCalculatedFieldsService>();
            DependencyInjectionService.RegisterType<ITableauSyntheseRowsComptabilisesService, TableauSyntheseRowsComptabilisesService>();
            DependencyInjectionService.RegisterType<ITableauSyntheseRowsCorrectifService, TableauSyntheseRowsCorrectifService>();
            DependencyInjectionService.RegisterType<ITableauSyntheseRowsProviderService, TableauSyntheseRowsProviderService>();
            DependencyInjectionService.RegisterType<ITableauSyntheseTotalDebourseService, TableauSyntheseTotalDebourseService>();
            DependencyInjectionService.RegisterType<ITableauSyntheseTotalRecetteService, TableauSyntheseTotalRecetteService>();
            DependencyInjectionService.RegisterType<ITableauxSyntheseMergeService, TableauxSyntheseMergeService>();
            DependencyInjectionService.RegisterType<ITacheManager, TacheManager>();
            DependencyInjectionService.RegisterType<ITranscoImportManager, TranscoImportManager>();
            DependencyInjectionService.RegisterType<ITypeDepenseManager, TypeDepenseManager>();
            DependencyInjectionService.RegisterType<ITypeEnergieManager, TypeEnergieManager>();
            DependencyInjectionService.RegisterType<ITypeOrganisationManager, TypeOrganisationManager>();
            DependencyInjectionService.RegisterType<ITypeParticipationSepManager, TypeParticipationSepManager>();
            DependencyInjectionService.RegisterType<ITypeRattachementManager, TypeRattachementManager>();
            DependencyInjectionService.RegisterType<ITypeSocieteManager, TypeSocieteManager>();
            DependencyInjectionService.RegisterType<IUniteManager, UniteManager>();
            DependencyInjectionService.RegisterType<IUniteReferentielEtenduManager, UniteReferentielEtenduManager>();
            DependencyInjectionService.RegisterType<IUtilisateurManager, UtilisateurManager>();
            DependencyInjectionService.RegisterType<IValidationPointageManager, ValidationPointageManager>();
            DependencyInjectionService.RegisterType<IValorisationManager, ValorisationManager>();
            DependencyInjectionService.RegisterType<IBaremeValorisationManager, BaremeValorisationManager>();
            DependencyInjectionService.RegisterType<IVerificationDesTempsManager, VerificationDesTempsManager>();
            DependencyInjectionService.RegisterType<IZoneDeTravailManager, ZoneDeTravailManager>();
            DependencyInjectionService.RegisterType<IEtatContratInterimaireManager, EtatContratInterimaireManager>();
            DependencyInjectionService.RegisterType<IContratInterimaireImportManager, ContratInterimaireImportManager>();
            DependencyInjectionService.RegisterType<IAffectationSeuilUtilisateurManager, AffectationSeuilUtilisateurManager>();
        }
    }
}