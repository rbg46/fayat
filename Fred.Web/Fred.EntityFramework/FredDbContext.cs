using System;
using System.Configuration;
using System.Linq;
using Fred.Entities;
using Fred.Entities.Action;
using Fred.Entities.Adresse;
using Fred.Entities.Affectation;
using Fred.Entities.Avis;
using Fred.Entities.Bareme;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Entities.Budget.Recette;
using Fred.Entities.Carburant;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.DatesCalendrierPaie;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Delegation;
using Fred.Entities.Depense;
using Fred.Entities.Directory;
using Fred.Entities.EcritureComptable;
using Fred.Entities.Email;
using Fred.Entities.EntityBase;
using Fred.Entities.Facturation;
using Fred.Entities.Facture;
using Fred.Entities.Favori;
using Fred.Entities.FeatureFlipping;
using Fred.Entities.Fonctionnalite;
using Fred.Entities.FonctionnaliteDesactive;
using Fred.Entities.Groupe;
using Fred.Entities.Holding;
using Fred.Entities.Image;
using Fred.Entities.Import;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Journal;
using Fred.Entities.Log;
using Fred.Entities.LogImport;
using Fred.Entities.Module;
using Fred.Entities.ModuleDesactive;
using Fred.Entities.Moyen;
using Fred.Entities.Notification;
using Fred.Entities.ObjectifFlash;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Organisation;
using Fred.Entities.OrganisationGenerique;
using Fred.Entities.Params;
using Fred.Entities.Permission;
using Fred.Entities.PermissionFonctionnalite;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Pole;
using Fred.Entities.Rapport;
using Fred.Entities.RapportPrime;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.RepartitionEcart;
using Fred.Entities.RessourcesRecommandees;
using Fred.Entities.Role;
using Fred.Entities.RoleFonctionnalite;
using Fred.Entities.Societe;
using Fred.Entities.Societe.Classification;
using Fred.Entities.Utilisateur;
using Fred.Entities.ValidationPointage;
using Fred.Entities.Valorisation;
using Fred.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fred.EntityFramework
{
    public class FredDbContext : DbContext
    {
        public static readonly ILoggerFactory LoggerFactory = new LoggerFactory().AddDebug();

        public FredDbContext()
        {
            Database.SetCommandTimeout(600);
        }

        public FredDbContext(DbContextOptions<FredDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            base.OnConfiguring(optionsBuilder);

            optionsBuilder
                .UseLoggerFactory(LoggerFactory)
                .EnableSensitiveDataLogging()
                .UseSqlServer(ConfigurationManager.ConnectionStrings["FredConnection"].ConnectionString, o => o.CommandTimeout(600));
        }

        public DbQuery<OrganisationLightEnt> OrganisationLight { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des commandes.
        /// </summary>
        /// <value>
        ///   Une commande.
        /// </value>
        public DbSet<CommandeEnt> Commandes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des fournisseurs.
        /// </summary>
        /// <value>
        ///   Un fournisseur.
        /// </value>
        public DbSet<FournisseurEnt> Fournisseurs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des actions.
        /// </summary>
        /// <value>
        ///   Une action
        /// </value>
        public DbSet<ActionEnt> Action { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des jobs d'action.
        /// </summary>
        /// <value>
        ///   action job
        /// </value>
        public DbSet<ActionJobEnt> ActionJob { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des statut d'action.
        /// </summary>
        /// <value>
        ///   action statut
        /// </value>
        public DbSet<ActionStatusEnt> ActionStatus { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes de commande.
        /// </summary>
        /// <value>
        ///   action type
        /// </value>
        public DbSet<ActionTypeEnt> ActionType { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des barèmes CI.
        /// </summary>
        /// <value>
        ///   Barèmes par CI.
        /// </value>
        public DbSet<BaremeExploitationCIEnt> BaremeExploitationCIs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des barèmes Organisation.
        /// </summary>
        /// <value>
        ///   Barèmes par Organisation.
        /// </value>
        public DbSet<BaremeExploitationOrganisationEnt> BaremeExploitationOrganisations { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des surcharges et exceptions barèmes CI.
        /// </summary>
        /// <value>
        ///   Surcharges et exceptions   barèmes par CI.
        /// </value>
        public DbSet<SurchargeBaremeExploitationCIEnt> SurchargeBaremeExploitationCIs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des unités par société.
        /// </summary>
        /// <value>
        ///   Unité Société.
        /// </value>
        public DbSet<UniteSocieteEnt> UniteSocietes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste du personnel.
        /// </summary>
        /// <value>
        ///   Un membre du personnel.
        /// </value>
        public DbSet<PersonnelEnt> Personnels { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des affaires.
        /// </summary>
        /// <value>
        ///   Une affaire.
        /// </value>
        public DbSet<CIEnt> CIs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des relations CI devises.
        /// </summary>
        /// <value>
        ///   liste des relations CI Devise
        /// </value>
        public DbSet<CIDeviseEnt> CIDevises { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des relations CI primes.
        /// </summary>
        /// <value>
        ///   liste des relations CI Prime
        /// </value>
        public DbSet<CIPrimeEnt> CIPrimes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des taches.
        /// </summary>
        /// <value>
        ///   Une tâche
        /// </value>
        public DbSet<TacheEnt> Taches { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des types de dépenses.
        /// </summary>
        /// <value>
        ///   Un type de dépense
        /// </value>
        public DbSet<TypeDepenseEnt> TypesDepense { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes de commande.
        /// </summary>
        /// <value>
        ///   Une ligne de commande
        /// </value>
        public DbSet<CommandeLigneEnt> CommandeLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes de commande.
        /// </summary>
        /// <value>
        ///   Une ligne de commande
        /// </value>
        public DbSet<ActionCommandeLigneEnt> ActionCommandeLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des types de commande.
        /// </summary>
        /// <value>
        ///   Un type de commande
        /// </value>
        public DbSet<CommandeTypeEnt> CommandeTypes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des statuts de commande.
        /// </summary>
        /// <value>
        ///   Un type de commande
        /// </value>
        public DbSet<StatutCommandeEnt> StatutsCommande { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des établissements comptables.
        /// </summary>
        /// <value>
        ///   Un établissement comptable
        /// </value>
        public DbSet<EtablissementComptableEnt> EtablissementsComptables { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des zone de travail
        /// </summary>
        /// <value>
        ///   Une zone de travail
        /// </value>
        public DbSet<ZoneDeTravailEnt> ZonesDeTravail { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des matricules externes
        /// </summary>
        /// <value>
        ///   Une zone de travail
        /// </value>
        public DbSet<MatriculeExterneEnt> MatriculeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des établissements de paie.
        /// </summary>
        /// <value>
        ///   Un établissement de paie
        /// </value>
        public DbSet<EtablissementPaieEnt> EtablissementsPaie { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des sociétés.
        /// </summary>
        /// <value>
        ///   Une société
        /// </value>
        public DbSet<SocieteEnt> Societes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Holdings.
        /// </summary>
        /// <value>
        ///   Une société
        /// </value>
        public DbSet<HoldingEnt> Holdings { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Poles.
        /// </summary>
        /// <value>
        ///   Une organisation
        /// </value>
        public DbSet<PoleEnt> Poles { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Organisations Generiques.
        /// </summary>
        /// <value>
        ///   Une organisation
        /// </value>
        public DbSet<OrganisationGeneriqueEnt> OrganisationsGeneriques { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des associations devise sociétés.
        /// </summary>
        /// <value>
        ///   Une société
        /// </value>
        public DbSet<SocieteDeviseEnt> SocietesDevises { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des DatesCalendrierPaie.
        /// </summary>
        /// <value>
        ///   Une société
        /// </value>
        public DbSet<DatesCalendrierPaieEnt> DatesCalendrierPaies { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Dates Clotures comptable.
        /// </summary>
        /// <value>
        ///   Une date cloture comptable
        /// </value>
        public DbSet<DatesClotureComptableEnt> DatesCloturesComptables { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des utilisateurs
        /// </summary>
        /// <value>
        ///   Un Utilisateur
        /// </value>
        public DbSet<UtilisateurEnt> Utilisateurs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des dépenses
        /// </summary>
        /// <value>Une dépense</value>
        public DbSet<DepenseAchatEnt> DepenseAchats { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des dépenses temporaires
        /// </summary>
        /// <value>Une dépense temporaires</value>
        public DbSet<DepenseTemporaireEnt> DepensesTemporaires { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des pays
        /// </summary>
        /// <value>Un Pays</value>
        public DbSet<PaysEnt> Pays { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des utilisateurs
        /// </summary>
        /// <value>Un Pays</value>
        public DbSet<ExternalDirectoryEnt> ExternalDirectory { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des rôles
        /// </summary>
        public DbSet<RoleEnt> Roles { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des modules
        /// </summary>
        public DbSet<ModuleEnt> Modules { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des codes majoration
        /// </summary>
        /// <value>Un code majoration</value>
        public DbSet<CodeMajorationEnt> CodesMajoration { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des codes déplacement
        /// </summary>
        /// <value>Un code déplacement</value>
        public DbSet<CodeDeplacementEnt> CodesDeplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des codes d'absence
        /// </summary>
        /// <value>Un code d'absence</value>
        public DbSet<CodeAbsenceEnt> CodeAbsences { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des materiaux
        /// </summary>
        /// <value>Un code d'absence</value>
        public DbSet<MaterielEnt> Materiels { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des CICodesMajoration
        /// </summary>
        /// <value>Un CI associé code majoration</value>
        public DbSet<CICodeMajorationEnt> CICodeMajorations { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des devises
        /// </summary>
        /// <value>Une devise</value>
        public DbSet<DeviseEnt> Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des fonctionnalités
        /// </summary>
        public DbSet<FonctionnaliteEnt> Fonctionnalites { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des fonctionnalités
        /// </summary>
        public DbSet<AffectationSeuilUtilisateurEnt> UtilOrgaRoleDevises { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des fonctionnalités
        /// </summary>
        public DbSet<AffectationSeuilOrgaEnt> SeuilRoleOrgas { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des organisations
        /// </summary>
        public DbSet<OrganisationEnt> Organisations { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des organisations
        /// </summary>
        public DbSet<TypeOrganisationEnt> OrganisationTypes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des seuils de validation
        /// </summary>
        public DbSet<SeuilValidationEnt> SeuilValidations { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des primes
        /// </summary>
        public DbSet<PrimeEnt> Primes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des rapports
        /// </summary>
        public DbSet<RapportEnt> Rapports { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes de rapport
        /// </summary>
        public DbSet<RapportLigneEnt> RapportLignes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes de rapport
        /// </summary>
        public DbSet<PointageAnticipeEnt> PointagesAnticipes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des primes rattachées à une ligne de rapport
        /// </summary>
        public DbSet<RapportLignePrimeEnt> RapportLignePrimes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des primes rattachées à un pointage anticipé
        /// </summary>
        public DbSet<PointageAnticipePrimeEnt> PointageAnticipePrimes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des tâches rattachées à une ligne de rapport
        /// </summary>
        public DbSet<RapportLigneTacheEnt> RapportLigneTaches { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des sorties astreintes rattachées à une ligne de rapport
        /// </summary>
        public DbSet<RapportLigneAstreinteEnt> RapportLigneAstreintes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des rapports
        /// </summary>
        public DbSet<RapportStatutEnt> RapportStatuts { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des RapportTache
        /// </summary>
        public DbSet<RapportTacheEnt> RapportTache { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des favoris.
        /// </summary>
        /// <value>
        ///   Un fournisseur.
        /// </value>
        public DbSet<FavoriEnt> Favori { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des notifications.
        /// </summary>
        /// <value>
        ///   Un fournisseur.
        /// </value>
        public DbSet<NotificationEnt> Notifications { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des groupes.
        /// </summary>
        /// <value>
        ///   Un groupe.
        /// </value>
        public DbSet<GroupeEnt> Groupes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des code zone déplacement
        /// </summary>
        public DbSet<CodeZoneDeplacementEnt> CodeZoneDeplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des indemnites de déplacement
        /// </summary>
        public DbSet<IndemniteDeplacementEnt> IndemniteDeplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des chapitres
        /// </summary>
        public DbSet<ChapitreEnt> Chapitres { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des sous-chapitres
        /// </summary>
        public DbSet<SousChapitreEnt> SousChapitres { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des ressources
        /// </summary>
        public DbSet<RessourceEnt> Ressources { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des ressource taches
        /// </summary>
        public DbSet<RessourceTacheEnt> RessourceTaches { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des unités / référentiel étendu
        /// </summary>
        public DbSet<UniteReferentielEtenduEnt> UnitesReferentielEtendu { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des ressource taches Devises
        /// </summary>
        public DbSet<RessourceTacheDeviseEnt> RessourceTacheDevises { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des types de ressource
        /// </summary>
        public DbSet<TypeRessourceEnt> TypesRessource { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des liaisons ressource/carburant
        /// </summary>
        public DbSet<CarburantEnt> Carburants { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des natures
        /// </summary>
        public DbSet<NatureEnt> Natures { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Paramétrage de Referentiels Etendus
        /// </summary>
        public DbSet<ReferentielEtenduEnt> ReferentielEtendus { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Referentiels Etendus
        /// </summary>
        public DbSet<ParametrageReferentielEtenduEnt> ParametragesReferentielEtendu { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des logs d'import
        /// </summary>
        public DbSet<LogImportEnt> LogImports { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des factures à rapprocher
        /// </summary>
        public DbSet<FactureEnt> FactureARs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes de factures à rapprocher
        /// </summary>
        public DbSet<FactureLigneEnt> FactureLigneARs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des journaux
        /// </summary>
        public DbSet<JournalEnt> Journals { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des parametres
        /// </summary>
        public DbSet<ParametreEnt> Parametres { get; set; }

        /// <summary>
        ///   Obtient ou définit les budgets
        /// </summary>
        public DbSet<BudgetEnt> Budgets { get; set; }

        /// <summary>
        ///   Obtient ou définit les révisions de budgets
        /// </summary>
        public DbSet<BudgetRevisionEnt> BudgetRevisions { get; set; }

        /// <summary>
        ///   Obtient ou définit les états de budgets
        /// </summary>
        public DbSet<BudgetEtatEnt> BudgetEtats { get; set; }

        /// <summary>
        ///   Obtient ou définit l'historique de copie d'un budget.
        /// </summary>
        public DbSet<BudgetCopyHistoEnt> BudgetCopyHistos { get; set; }

        /// <summary>
        ///   Obtient ou définit les sous-détails de budgets
        /// </summary>
        public DbSet<BudgetSousDetailEnt> BudgetSousDetails { get; set; }

        /// <summary>
        ///   Obtient ou définit les tâches de niveau 4 de budgets
        /// </summary>
        public DbSet<BudgetT4Ent> BudgetT4 { get; set; }

        /// <summary>
        ///   Obtient ou définit les liaisons budget/tâche
        /// </summary>
        public DbSet<BudgetTacheEnt> BudgetTaches { get; set; }

        /// <summary>
        ///   Obtient ou définit les workflow de budgets
        /// </summary>
        public DbSet<BudgetWorkflowEnt> BudgetWorkflows { get; set; }

        /// <summary>
        ///   Obtient ou définit les recettes du budget
        /// </summary>
        public DbSet<BudgetRecetteEnt> BudgetRecettes { get; set; }

        /// <summary>
        ///   Obtient ou définit les avancements des recettes du budget
        /// </summary>
        public DbSet<AvancementRecetteEnt> AvancementRecettes { get; set; }

        /// <summary>
        ///   Obtient ou définit les avancements
        /// </summary>
        public DbSet<AvancementEnt> Avancements { get; set; }

        /// <summary>
        ///   Obtient ou définit les taches d'avancement
        /// </summary>
        public DbSet<AvancementTacheEnt> AvancementTaches { get; set; }

        /// <summary>
        /// Les différents controles budgétaires
        /// </summary>
        public DbSet<ControleBudgetaireEnt> ControleBudgetaires { get; set; }

        /// <summary>
        /// Obient ou définit les valeurs des controles budgétaire
        /// </summary>
        public DbSet<ControleBudgetaireValeursEnt> ControleBudgetaireValeurs { get; set; }

        /// <summary>
        ///   Obtient ou définit les workflow avancement
        /// </summary>
        public DbSet<AvancementWorkflowEnt> AvancementWorkflows { get; set; }

        /// <summary>
        ///   Obtient ou définit les états de l'avancement
        /// </summary>
        public DbSet<AvancementEtatEnt> AvancementEtats { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des état d'un contrat intérimaire.
        /// </summary>
        public DbSet<EtatContratInterimaireEnt> EtatContratInterimaires { get; set; }

        /// <summary>
        /// <summary>
        ///   Obtient ou définit la liste des affectations des intérimaires
        /// </summary>
        public DbSet<ContratInterimaireEnt> ContratInterimaires { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des CIRessources
        /// </summary>
        public DbSet<CIRessourceEnt> CIRessources { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Unités
        /// </summary>
        public DbSet<UniteEnt> Unites { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des CarburantOrganisationDevise
        /// </summary>
        public DbSet<CarburantOrganisationDeviseEnt> CarburantOrganisationDevises { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lot de FARs
        /// </summary>
        public DbSet<LotFarEnt> LotFars { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des logs
        /// </summary>
        public DbSet<NLogEnt> NLogs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Lot de Pointage
        /// </summary>
        public DbSet<LotPointageEnt> LotPointages { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Validations de Pointage
        /// </summary>
        public DbSet<ControlePointageEnt> ControlePointages { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Erreur de validation de pointage
        /// </summary>
        public DbSet<ControlePointageErreurEnt> ControlePointageErreurs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Images
        /// </summary>
        public DbSet<ImageEnt> Images { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Logs d'authentification
        /// </summary>
        public DbSet<AuthentificationLogEnt> AuthentificationLogs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Remontées Vrac
        /// </summary>
        public DbSet<RemonteeVracEnt> RemonteeVracs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Erreurs de Remontée Vrac
        /// </summary>
        public DbSet<RemonteeVracErreurEnt> RemonteeVracErreurs { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Permissions
        /// </summary>
        public DbSet<PermissionEnt> Permissions { get; set; }

        /// <summary>
        ///    Obtient ou définit la liste des PermissionFonctionnalites
        /// </summary>
        public DbSet<PermissionFonctionnaliteEnt> PermissionFonctionnalites { get; set; }

        /// <summary>
        ///    Obtient ou définit la liste des Modules désactive
        /// </summary>
        public DbSet<ModuleDesactiveEnt> ModuleDesactives { get; set; }

        /// <summary>
        ///    Obtient ou définit la liste desliens entre les roles et les fonctionnalites
        /// </summary>
        public DbSet<RoleFonctionnaliteEnt> RoleFonctionnalites { get; set; }

        /// <summary>
        ///    Obtient ou définit la liste des Modules désactive
        /// </summary>
        public DbSet<FonctionnaliteDesactiveEnt> FonctionnaliteDesactives { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des ecritures comptables
        /// </summary>
        public DbSet<EcritureComptableEnt> EcritureComptables { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des ecritures comptables cumulées
        /// </summary>
        public DbSet<EcritureComptableCumulEnt> EcritureComptablesCumul { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des ecritures comptables rejetées
        /// </summary>
        public DbSet<EcritureComptableRejetEnt> EcritureComptablesRejet { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des OD
        /// </summary>
        public DbSet<OperationDiverseEnt> OperationDiverses { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des Repartition d' écarts.
        /// </summary>
        public DbSet<RepartitionEcartEnt> RepartitionEcarts { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des valorisations.
        /// </summary>
        /// <value>
        ///   Valorisations.
        /// </value>
        public DbSet<ValorisationEnt> Valorisations { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des facturations.
        /// </summary>
        public DbSet<FacturationEnt> Facturations { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des des types de facturation.
        /// </summary>
        public DbSet<FacturationTypeEnt> FacturationTypes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des des types de depense.
        /// </summary>
        public DbSet<DepenseTypeEnt> DepenseTypes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des objectifs Flash.
        /// </summary>
        public DbSet<ObjectifFlashEnt> ObjectifFlash { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des taches associées aux objectifs Flash.
        /// </summary>
        public DbSet<ObjectifFlashTacheEnt> ObjectifFlashTache { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des quantités realisées d'une tache d'Objectif Flash pour un rapport.
        /// </summary>
        public DbSet<ObjectifFlashTacheRapportRealiseEnt> ObjectifFlashTacheRapportRealise { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des journalisations des taches associées aux objectifs Flash.
        /// </summary>
        public DbSet<ObjectifFlashTacheJournalisationEnt> ObjectifFlashTacheJournalisation { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des ressources associées aux taches d'objectifs Flash.
        /// </summary>
        public DbSet<ObjectifFlashTacheRessourceEnt> ObjectifFlashTacheRessource { get; set; }

        /// <summary>
        ///   Obtient ou définit la journalisation des ressources associées aux taches d'objectifs Flash.
        /// </summary>
        public DbSet<ObjectifFlashTacheRessourceJournalisationEnt> ObjectifFlashTacheRessourceJournalisation { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des systèmes.
        /// </summary>
        public DbSet<SystemeImportEnt> SystemeImport { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des transcos entre les Fred un système externe.
        /// </summary>
        public DbSet<TranscoImportEnt> TranscoImport { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des types des centres d'imputations.
        /// </summary>
        public DbSet<CITypeEnt> CIType { get; set; }

        /// <summary>
        /// Obtient ou definit la liste des equipes
        /// </summary>
        public DbSet<EquipeEnt> Equipe { get; set; }

        /// <summary>
        /// Obtient ou definit la liste des astreintes
        /// </summary>
        public DbSet<AstreinteEnt> Astreinte { get; set; }

        /// <summary>
        /// Obtient ou definit la liste des affectations
        /// </summary>
        public DbSet<AffectationEnt> Affectation { get; set; }

        /// <summary>
        /// Obtient ou definit la liste dess equipes personnels
        /// </summary>
        public DbSet<EquipePersonnelEnt> EquipePersonnel { get; set; }

        /// <summary>
        ///   Obtient la liste des images associées à un personnel (signature + photo de profil)
        /// </summary>
        public DbSet<PersonnelImageEnt> PersonnelImages { get; set; }

        /// <summary>
        ///   Obtient la liste des familles d'OD
        /// </summary>
        public DbSet<FamilleOperationDiverseEnt> FamilleOperationDiverse { get; set; }

        /// <summary>
        ///   Obtient la liste des délégation
        /// </summary>
        public DbSet<DelegationEnt> Delegation { get; set; }

        /// <summary>
        ///   Obtient la liste des commande contrat intérimaire
        /// </summary>
        public DbSet<CommandeContratInterimaireEnt> CommandeContratInterimaire { get; set; }

        /// <summary>
        ///   Obtient la liste des commande contrat intérimaire
        /// </summary>
        public DbSet<MotifRemplacementEnt> MotifRemplacement { get; set; }

        /// <summary>
        /// Obtient la liste des Features Flippings
        /// </summary>
        public DbSet<FeatureFlippingEnt> FeatureFlippings { get; set; }

        /// <summary>
        /// Obtient la liste des Groupes de remplacement des tâches
        /// </summary>
        public DbSet<GroupeRemplacementTacheEnt> GroupeRemplacementTache { get; set; }

        /// <summary>
        /// Obtient la liste des Remplacement des tâches
        /// </summary>
        public DbSet<RemplacementTacheEnt> RemplacementTache { get; set; }

        /// <summary>
        /// Obtient la liste des RapportPrime (Rapport mensuel)
        /// </summary>
        public DbSet<RapportPrimeEnt> RapportPrime { get; set; }

        /// <summary>
        /// Obtient la liste des RapportPrimeLigne au sein d'un RapportPrime
        /// </summary>
        public DbSet<RapportPrimeLigneEnt> RapportPrimeLigne { get; set; }

        /// <summary>
        /// Obtient la liste des (Astreintes) RapportPrimeLigneAstreinte au sein d'un RapportPrimeLigne
        /// </summary>
        public DbSet<RapportPrimeLigneAstreinteEnt> RapportPrimeLigneAstreinte { get; set; }

        /// <summary>
        /// Obtient la liste des (Primes) RapportPrimeLignePrime au sein d'un RapportPrimeLigne
        /// </summary>
        public DbSet<RapportPrimeLignePrimeEnt> RapportPrimeLignePrime { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des majorations rattachées à une ligne de rapport
        /// </summary>
        public DbSet<RapportLigneMajorationEnt> RapportLigneMajorations { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des param keys
        /// </summary>
        public DbSet<ParamKeyEnt> ParamKeys { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des param/Valeur
        /// </summary>
        public DbSet<ParamValueEnt> ParamValues { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des affectation des moyens
        /// </summary>
        public DbSet<AffectationMoyenEnt> AffectationMoyens { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des matériel locations
        /// </summary>
        public DbSet<MaterielLocationEnt> MaterielLocation { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des types des affectation des moyens
        /// </summary>
        public DbSet<AffectationMoyenTypeEnt> AffectationMoyenTypes { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des familles d'affectation des moyens
        /// </summary>
        public DbSet<AffectationMoyenFamilleEnt> AffectationMoyenFamilles { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des sites
        /// </summary>
        public DbSet<SiteEnt> Sites { get; set; }

        /// <summary>
        /// Obtient ou définit les Codes Astreintes
        /// </summary>
        public DbSet<CodeAstreinteEnt> CodeAstreintes { get; set; }

        /// <summary>
        /// Obtient ou définit les codes Astreintes d 'un rapport ligne
        /// </summary>
        public DbSet<RapportLigneCodeAstreinteEnt> RapportLigneCodeAstreintes { get; set; }

        /// <summary>
        /// Obtient ou définit les souscription d'email
        /// </summary>
        public DbSet<EmailSouscriptionEnt> EmailSouscriptions { get; set; }

        /// <summary>
        ///     Dbset Types de Société 
        /// </summary>
        public DbSet<TypeSocieteEnt> TypeSocietes { get; set; }

        /// <summary>
        ///     Dbset Associes Sep
        /// </summary>
        public DbSet<AssocieSepEnt> AssocieSeps { get; set; }

        /// <summary>
        ///     Dbset Type Energie
        /// </summary>
        public DbSet<TypeEnergieEnt> TypeEnergies { get; set; }

        /// <summary>
        ///     Dbset Types Participation Sep
        /// </summary>
        public DbSet<TypeParticipationSepEnt> TypeParticipationSeps { get; set; }

        /// <summary>
        /// Gets or sets the ressource recommandees.
        /// </summary>
        /// <value>
        /// The ressource recommandees.
        /// </value>
        public DbSet<RessourceRecommandeeEnt> RessourceRecommandees { get; set; }

        /// <summary>
        ///  Les bibliotheques des prix
        /// </summary>
        public DbSet<BudgetBibliothequePrixEnt> BibliothequePrix { get; set; }

        /// <summary>
        /// Les items des bibliotheques des prix
        /// </summary>
        public DbSet<BudgetBibliothequePrixItemEnt> BibliothequePrixItem { get; set; }

        /// <summary>
        /// Les items historiques des bibliotheques des prix
        /// </summary>
        public DbSet<BudgetBibliothequePrixItemValuesHistoEnt> BibliothequePrixItemValues { get; set; }

        /// <summary>
        /// Gets or sets les classifications des sociétés
        /// </summary>
        public DbSet<SocieteClassificationEnt> SocietesClassifications { get; set; }

        /// <summary>
        /// Dbset Pièce jointes
        /// </summary>
        public DbSet<PieceJointeEnt> PieceJointes { get; set; }

        /// <summary>
        /// Dbset Attachement Pièce jointes - Commande
        /// </summary>
        public DbSet<PieceJointeCommandeEnt> PieceJointeCommandes { get; set; }

        /// <summary>
        /// Dbset Attachement Pièce jointes - Dépense
        /// </summary>
        public DbSet<PieceJointeReceptionEnt> PieceJointeReceptions { get; set; }

        /// <summary>
        /// Dbset Adresses
        /// </summary>
        public DbSet<AdresseEnt> Adresses { get; set; }

        /// <summary>
        /// Dbset Fournisseurs - Agences
        /// </summary>
        public DbSet<AgenceEnt> Agences { get; set; }

        /// <summary>
        /// Dbset Avis
        /// </summary>
        public DbSet<AvisEnt> Avis { get; set; }

        /// <summary>
        /// Dbset Avis - Commande
        /// </summary>
        public DbSet<AvisCommandeEnt> AvisCommandes { get; set; }

        /// <summary>
        /// Dbset Avis - CommandeAvenant
        /// </summary>
        public DbSet<AvisCommandeAvenantEnt> AvisCommandesAvenants { get; set; }

        /// <summary>
        /// Dbset Avis - Avenant d'une Commande
        /// </summary>
        public DbSet<CommandeAvenantEnt> CommandeAvenant { get; set; }

        /// <summary>
        /// Dbset Affectation Absences
        /// </summary>
        public DbSet<AffectationAbsenceEnt> AffectationAbsences { get; set; }

        /// <summary>
        /// Dbset Statut Absences
        /// </summary>
        public DbSet<StatutAbsenceEnt> StatutAbsences { get; set; }

        /// <summary>
        /// Dbset TypeJournees
        /// </summary>
        public DbSet<TypeJourneeEnt> TypeJournees { get; set; }

        /// <summary>
        ///   Sauvegarde le contexte en modifiant automatiquement l'utilisateur de création, la date de création, l'utilisateur de la modification et la date de modification
        /// </summary>
        /// <param name="currentUserId">Identifiant de l'utilisateur</param>
        /// <returns>The number of state entries written to the underlying database. This can include
        ///     state entries for entities and/or relationships. Relationship state entries are
        ///     created for many-to-many relationships and relationships where there is no foreign
        ///     key property included in the entity class (often referred to as independent associations)
        /// </returns>
        public int SaveChangesWithAudit(int currentUserId)
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        entity.AuteurCreationId = currentUserId;
                        entity.DateCreation = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.AuteurCreationId).IsModified = false;
                        base.Entry(entity).Property(x => x.DateCreation).IsModified = false;
                    }

                    entity.AuteurModificationId = currentUserId;
                    entity.DateModification = now;
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ActionCommandeLigneEntConfiguration());
            modelBuilder.ApplyConfiguration(new ActionEntConfiguration());
            modelBuilder.ApplyConfiguration(new ActionJobEntConfiguration());
            modelBuilder.ApplyConfiguration(new ActionStatusEntConfiguration());
            modelBuilder.ApplyConfiguration(new ActionTypeEntConfiguration());
            modelBuilder.ApplyConfiguration(new AdresseEntConfiguration());
            modelBuilder.ApplyConfiguration(new AffectationAbsenceEntConfiguration());
            modelBuilder.ApplyConfiguration(new AffectationEntConfiguration());
            modelBuilder.ApplyConfiguration(new AffectationMoyenEntConfiguration());
            modelBuilder.ApplyConfiguration(new AffectationMoyenFamilleEntConfiguration());
            modelBuilder.ApplyConfiguration(new AffectationMoyenTypeEntConfiguration());
            modelBuilder.ApplyConfiguration(new AffectationSeuilOrgaEntConfiguration());
            modelBuilder.ApplyConfiguration(new AffectationSeuilUtilisateurEntConfiguration());
            modelBuilder.ApplyConfiguration(new AgenceEntConfiguration());
            modelBuilder.ApplyConfiguration(new AssocieSepConfiguration());
            modelBuilder.ApplyConfiguration(new AstreinteEntConfiguration());
            modelBuilder.ApplyConfiguration(new AuthentificationLogEntConfiguration());
            modelBuilder.ApplyConfiguration(new AvancementEntConfiguration());
            modelBuilder.ApplyConfiguration(new AvancementEtatEntConfiguration());
            modelBuilder.ApplyConfiguration(new AvancementRecetteEntConfiguration());
            modelBuilder.ApplyConfiguration(new AvancementTacheEntConfiguration());
            modelBuilder.ApplyConfiguration(new AvancementWorkflowEntConfiguration());
            modelBuilder.ApplyConfiguration(new AvisCommandeAvenantEntConfiguration());
            modelBuilder.ApplyConfiguration(new AvisCommandeEntConfiguration());
            modelBuilder.ApplyConfiguration(new AvisEntConfiguration());
            modelBuilder.ApplyConfiguration(new BaremeExploitationCIEntConfiguration());
            modelBuilder.ApplyConfiguration(new BaremeExploitationOrganisationEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetBibliothequePrixEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetBibliothequePrixItemEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetBibliothequePrixItemValuesHistoEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetCopyHistoEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetEtatEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetRecetteEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetRevisionEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetSousDetailEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetT4EntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetTacheEntConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetWorkflowEntConfiguration());
            modelBuilder.ApplyConfiguration(new CICodeMajorationEntConfiguration());
            modelBuilder.ApplyConfiguration(new CIDeviseEntConfiguration());
            modelBuilder.ApplyConfiguration(new CIEntConfiguration());
            modelBuilder.ApplyConfiguration(new CIPrimeEntConfiguration());
            modelBuilder.ApplyConfiguration(new CIRessourceEntConfiguration());
            modelBuilder.ApplyConfiguration(new CITypeEntConfiguration());
            modelBuilder.ApplyConfiguration(new CarburantEntConfiguration());
            modelBuilder.ApplyConfiguration(new CarburantOrganisationDeviseEntConfiguration());
            modelBuilder.ApplyConfiguration(new ChapitreEntConfiguration());
            modelBuilder.ApplyConfiguration(new CodeAbsenceEntConfiguration());
            modelBuilder.ApplyConfiguration(new CodeAstreinteEntConfiguration());
            modelBuilder.ApplyConfiguration(new CodeDeplacementEntConfiguration());
            modelBuilder.ApplyConfiguration(new CodeMajorationEntConfiguration());
            modelBuilder.ApplyConfiguration(new CodeZoneDeplacementEntConfiguration());
            modelBuilder.ApplyConfiguration(new CommandeAvenantEntConfiguration());
            modelBuilder.ApplyConfiguration(new CommandeContratInterimaireEntConfiguration());
            modelBuilder.ApplyConfiguration(new CommandeEntConfiguration());
            modelBuilder.ApplyConfiguration(new CommandeLigneAvenantEntConfiguration());
            modelBuilder.ApplyConfiguration(new CommandeLigneEntConfiguration());
            modelBuilder.ApplyConfiguration(new CommandeTypeEntConfiguration());
            modelBuilder.ApplyConfiguration(new ContratInterimaireEntConfiguration());
            modelBuilder.ApplyConfiguration(new ContratInterimaireImportEntConfiguration());
            modelBuilder.ApplyConfiguration(new ControleBudgetaireEntConfiguration());
            modelBuilder.ApplyConfiguration(new ControleBudgetaireValeursEntConfiguration());
            modelBuilder.ApplyConfiguration(new ControlePointageEntConfiguration());
            modelBuilder.ApplyConfiguration(new ControlePointageErreurEntConfiguration());
            modelBuilder.ApplyConfiguration(new DatesCalendrierPaieEntConfiguration());
            modelBuilder.ApplyConfiguration(new DatesClotureComptableEntConfiguration());
            modelBuilder.ApplyConfiguration(new DelegationEntConfiguration());
            modelBuilder.ApplyConfiguration(new DepenseAchatEntConfiguration());
            modelBuilder.ApplyConfiguration(new DepenseTemporaireEntConfiguration());
            modelBuilder.ApplyConfiguration(new DepenseTypeEntConfiguration());
            modelBuilder.ApplyConfiguration(new DeviseEntConfiguration());
            modelBuilder.ApplyConfiguration(new EcritureComptableCumulEntConfiguration());
            modelBuilder.ApplyConfiguration(new EcritureComptableEntConfiguration());
            modelBuilder.ApplyConfiguration(new EcritureComptableRejetEntConfiguration());
            modelBuilder.ApplyConfiguration(new EmailSouscriptionEntConfiguration());
            modelBuilder.ApplyConfiguration(new EquipeEntConfiguration());
            modelBuilder.ApplyConfiguration(new EquipePersonnelEntConfiguration());
            modelBuilder.ApplyConfiguration(new EtablissementComptableEntConfiguration());
            modelBuilder.ApplyConfiguration(new EtablissementPaieEntConfiguration());
            modelBuilder.ApplyConfiguration(new EtatContratInterimaireEntConfiguration());
            modelBuilder.ApplyConfiguration(new ExternalDirectoryEntConfiguration());
            modelBuilder.ApplyConfiguration(new FacturationEntConfiguration());
            modelBuilder.ApplyConfiguration(new FacturationTypeEntConfiguration());
            modelBuilder.ApplyConfiguration(new FactureEntConfiguration());
            modelBuilder.ApplyConfiguration(new FactureLigneEntConfiguration());
            modelBuilder.ApplyConfiguration(new FamilleOperationDiverseEntConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriEntConfiguration());
            modelBuilder.ApplyConfiguration(new FeatureFlippingEntConfiguration());
            modelBuilder.ApplyConfiguration(new FonctionnaliteDesactiveEntConfiguration());
            modelBuilder.ApplyConfiguration(new FonctionnaliteEntConfiguration());
            modelBuilder.ApplyConfiguration(new FournisseurEntConfiguration());
            modelBuilder.ApplyConfiguration(new GroupeEntConfiguration());
            modelBuilder.ApplyConfiguration(new GroupeRemplacementTacheEntConfiguration());
            modelBuilder.ApplyConfiguration(new HoldingEntConfiguration());
            modelBuilder.ApplyConfiguration(new ImageEntConfiguration());
            modelBuilder.ApplyConfiguration(new IndemniteDeplacementCalculTypeEntConfiguration());
            modelBuilder.ApplyConfiguration(new IndemniteDeplacementEntConfiguration());
            modelBuilder.ApplyConfiguration(new JournalEntConfiguration());
            modelBuilder.ApplyConfiguration(new LogImportEntConfiguration());
            modelBuilder.ApplyConfiguration(new LotFarEntConfiguration());
            modelBuilder.ApplyConfiguration(new LotPointageEntConfiguration());
            modelBuilder.ApplyConfiguration(new MaterielEntConfiguration());
            modelBuilder.ApplyConfiguration(new MaterielLocationEntConfiguration());
            modelBuilder.ApplyConfiguration(new MatriculeExterneEntConfiguration());
            modelBuilder.ApplyConfiguration(new ModuleDesactiveConfiguration());
            modelBuilder.ApplyConfiguration(new ModuleEntConfiguration());
            modelBuilder.ApplyConfiguration(new MotifRemplacementEntConfiguration());
            modelBuilder.ApplyConfiguration(new NLogEntConfiguration());
            modelBuilder.ApplyConfiguration(new NatureEntConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationEntConfiguration());
            modelBuilder.ApplyConfiguration(new ObjectifFlashEntConfiguration());
            modelBuilder.ApplyConfiguration(new ObjectifFlashTacheEntConfiguration());
            modelBuilder.ApplyConfiguration(new ObjectifFlashTacheJournalisationEntConfiguration());
            modelBuilder.ApplyConfiguration(new ObjectifFlashTacheRapportRealiseEntConfiguration());
            modelBuilder.ApplyConfiguration(new ObjectifFlashTacheRessourceEntConfiguration());
            modelBuilder.ApplyConfiguration(new ObjectifFlashTacheRessourceJournalisationEntConfiguration());
            modelBuilder.ApplyConfiguration(new OperationDiverseEntConfiguration());
            modelBuilder.ApplyConfiguration(new OrganisationEntConfiguration());
            modelBuilder.ApplyConfiguration(new OrganisationGeneriqueEntConfiguration());
            modelBuilder.ApplyConfiguration(new OrganisationLienEntConfiguration());
            modelBuilder.ApplyConfiguration(new ParamKeyEntConfiguration());
            modelBuilder.ApplyConfiguration(new ParamValueEntConfiguration());
            modelBuilder.ApplyConfiguration(new ParametrageReferentielEtenduEntConfiguration());
            modelBuilder.ApplyConfiguration(new ParametreEntConfiguration());
            modelBuilder.ApplyConfiguration(new PaysEntConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionEntConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionFonctionnaliteEntConfiguration());
            modelBuilder.ApplyConfiguration(new PersonnelEntConfiguration());
            modelBuilder.ApplyConfiguration(new PersonnelImageEntConfiguration());
            modelBuilder.ApplyConfiguration(new PieceJointeCommandeEntConfiguration());
            modelBuilder.ApplyConfiguration(new PieceJointeEntConfiguration());
            modelBuilder.ApplyConfiguration(new PieceJointeReceptionEntConfiguration());
            modelBuilder.ApplyConfiguration(new PointageAnticipeEntConfiguration());
            modelBuilder.ApplyConfiguration(new PointageAnticipePrimeEntConfiguration());
            modelBuilder.ApplyConfiguration(new PoleEntConfiguration());
            modelBuilder.ApplyConfiguration(new PrimeEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportLigneAstreintEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportLigneCodeAstreinteEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportLigneEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportLigneMajorationEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportLignePrimeEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportLigneTacheEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportPrimeEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportPrimeLigneAstreinteEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportPrimeLigneEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportPrimeLignePrimeEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportStatutEntConfiguration());
            modelBuilder.ApplyConfiguration(new RapportTacheEntConfiguration());
            modelBuilder.ApplyConfiguration(new ReferentielEtenduEntConfiguration());
            modelBuilder.ApplyConfiguration(new RemonteeVracEntConfiguration());
            modelBuilder.ApplyConfiguration(new RemonteeVracErreurEntConfiguration());
            modelBuilder.ApplyConfiguration(new RemplacementTacheEntConfiguration());
            modelBuilder.ApplyConfiguration(new RepartitionEcartEntConfiguration());
            modelBuilder.ApplyConfiguration(new RessourceEntConfiguration());
            modelBuilder.ApplyConfiguration(new RessourceRecommandeeEntConfiguration());
            modelBuilder.ApplyConfiguration(new RessourceTacheDeviseEntConfiguration());
            modelBuilder.ApplyConfiguration(new RessourceTacheEntConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntConfiguration());
            modelBuilder.ApplyConfiguration(new RoleFonctionnaliteEntConfiguration());
            modelBuilder.ApplyConfiguration(new SeuilValidationEntConfiguration());
            modelBuilder.ApplyConfiguration(new SiteEntConfiguration());
            modelBuilder.ApplyConfiguration(new SocieteClassificationEntConfiguration());
            modelBuilder.ApplyConfiguration(new SocieteDeviseEntConfiguration());
            modelBuilder.ApplyConfiguration(new SocieteEntConfiguration());
            modelBuilder.ApplyConfiguration(new SousChapitreEntConfiguration());
            modelBuilder.ApplyConfiguration(new StatutAbsenceEntConfiguration());
            modelBuilder.ApplyConfiguration(new StatutCommandeEntConfiguration());
            modelBuilder.ApplyConfiguration(new SurchargeBaremeExploitationCIEntConfiguration());
            modelBuilder.ApplyConfiguration(new SystemeExterneEntConfiguration());
            modelBuilder.ApplyConfiguration(new SystemeExterneTypeEntConfiguration());
            modelBuilder.ApplyConfiguration(new SystemeImportEntConfiguration());
            modelBuilder.ApplyConfiguration(new TacheEntConfiguration());
            modelBuilder.ApplyConfiguration(new TacheRecetteEntConfiguration());
            modelBuilder.ApplyConfiguration(new TranscoImportEntConfiguration());
            modelBuilder.ApplyConfiguration(new TypeDepenseEntConfiguration());
            modelBuilder.ApplyConfiguration(new TypeEnergieEntConfiguration());
            modelBuilder.ApplyConfiguration(new TypeJourneeEntConfiguration());
            modelBuilder.ApplyConfiguration(new TypeOrganisationEntConfiguration());
            modelBuilder.ApplyConfiguration(new TypeParticipationSepEntConfiguration());
            modelBuilder.ApplyConfiguration(new TypeRessourceEntConfiguration());
            modelBuilder.ApplyConfiguration(new TypeSocieteEntConfiguration());
            modelBuilder.ApplyConfiguration(new UniteEntConfiguration());
            modelBuilder.ApplyConfiguration(new UniteReferentielEtenduEntConfiguration());
            modelBuilder.ApplyConfiguration(new UniteSocieteEntConfiguration());
            modelBuilder.ApplyConfiguration(new UtilisateurEntConfiguration());
            modelBuilder.ApplyConfiguration(new ValorisationEntConfiguration());
            modelBuilder.ApplyConfiguration(new ZoneDeTravailEntConfiguration());

            modelBuilder.Query<PersonnelListResultViewModel>();
        }
    }
}
