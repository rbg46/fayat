using System;
using System.Linq;
using Fred.Business.Achat.Calculation;
using Fred.Business.Budget;
using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.Commande.Validators;
using Fred.Business.DatesCalendrierPaie;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense;
using Fred.Business.EtatPaie;
using Fred.Business.Facture;
using Fred.Business.Favori;
using Fred.Business.IndemniteDeplacement;
using Fred.Business.Module;
using Fred.Business.ObjectifFlash;
using Fred.Business.Personnel;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeAbsence;
using Fred.Business.Referential.Materiel;
using Fred.Business.Referential.Nature;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielFixe;
using Fred.Business.Role;
using Fred.Business.Societe;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace Fred.Business.Tests.Integration
{
    public abstract class FredBaseTest : IDisposable
    {
        protected const int SuperAdminUserId = SetupAssemblyInitializer.SuperAdminUserId;
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private readonly Random random = new Random();
        public static readonly UnityContainer Container = SetupAssemblyInitializer.Container;

        private IUnitOfWork unitOfWork;
        public IUnitOfWork Uow => unitOfWork ??= Container.Resolve<IUnitOfWork>();

        private FredDbContext context;
        public FredDbContext FredContext => context ??= Container.Resolve<FredDbContext>();

        #region ----------------------------------------- SERVICES -----------------------------------------------------
        public ISoldeFarService SoldeFarService => Container.Resolve<ISoldeFarService>();
        #endregion

        #region ----------------------------------------- MANAGERS -----------------------------------------------------
        public ICacheManager CacheMgr => Container.Resolve<ICacheManager>();
        public ICIManager CIMgr => Container.Resolve<ICIManager>();
        public ICommandeManager CommandeMgr => Container.Resolve<ICommandeManager>();
        public ITacheManager TacheMgr => Container.Resolve<ITacheManager>();
        public ILogManager LogMgr => Container.Resolve<ILogManager>();
        public ISocieteManager SocieteMgr => Container.Resolve<ISocieteManager>();
        public IRapportManager RapportMgr => Container.Resolve<IRapportManager>();
        public IPointageManager PointageMgr => Container.Resolve<IPointageManager>();
        public IControlePointageManager ControlePointageMgr => Container.Resolve<IControlePointageManager>();
        public ILotPointageManager LotPointageMgr => Container.Resolve<ILotPointageManager>();
        public IUniteManager UniteMgr => Container.Resolve<IUniteManager>();
        public IUtilisateurManager UserMgr => Container.Resolve<IUtilisateurManager>();
        public IPersonnelManager PersonnelMgr => Container.Resolve<IPersonnelManager>();
        public IDatesCalendrierPaieManager DatesCalendrierPaieMgr => Container.Resolve<IDatesCalendrierPaieManager>();
        public IDepenseManager DepenseMgr => Container.Resolve<IDepenseManager>();
        public IEtatPaieManager EtatPaieMgr => Container.Resolve<IEtatPaieManager>();
        public IFactureManager FactureMgr => Container.Resolve<IFactureManager>();
        public IFavoriManager FavoriMgr => Container.Resolve<IFavoriManager>();
        public IModuleManager ModuleMgr => Container.Resolve<IModuleManager>();
        public ICodeAbsenceManager CodeAbsenceMgr => Container.Resolve<ICodeAbsenceManager>();
        public ICodeZoneDeplacementManager CodeZoneDeplacementMgr => Container.Resolve<ICodeZoneDeplacementManager>();
        public ICodeMajorationManager CodeMajorationMgr => Container.Resolve<ICodeMajorationManager>();
        public IDeviseManager DeviseMgr => Container.Resolve<IDeviseManager>();
        public IEtablissementComptableManager EtabComptableMgr => Container.Resolve<IEtablissementComptableManager>();
        public IEtablissementPaieManager EtabPaieMgr => Container.Resolve<IEtablissementPaieManager>();
        public IFournisseurManager FournisseurMgr => Container.Resolve<IFournisseurManager>();
        public IIndemniteDeplacementManager IndemniteDepMgr => Container.Resolve<IIndemniteDeplacementManager>();
        public IMaterielManager MaterielMgr => Container.Resolve<IMaterielManager>();
        public INatureManager NatureMgr => Container.Resolve<INatureManager>();
        public IPrimeManager PrimeMgr => Container.Resolve<IPrimeManager>();
        public ITypeDepenseManager TypeDepenseMgr => Container.Resolve<ITypeDepenseManager>();
        public IReferentielFixeManager RefFixeMgr => Container.Resolve<IReferentielFixeManager>();
        public IRoleManager RoleMgr => Container.Resolve<IRoleManager>();
        public IRemonteeVracManager RemonteeVracMgr => Container.Resolve<IRemonteeVracManager>();
        public ICloturesPeriodesManager CloturesPeriodesManager => Container.Resolve<ICloturesPeriodesManager>();
        public IObjectifFlashManager ObjectifFlashMgr => Container.Resolve<IObjectifFlashManager>();
        public IObjectifFlashTacheManager ObjectifFlashTacheMgr => Container.Resolve<IObjectifFlashTacheManager>();
        #endregion

        #region ----------------------------------------- REPOSITORIES ---------------------------------------------------
        public IPersonnelRepository PersonnelRepository => Container.Resolve<IPersonnelRepository>();
        public IJournalRepository JournalRepository => Container.Resolve<IJournalRepository>();
        public ILogImportRepository LogImportRepository => Container.Resolve<ILogImportRepository>();
        public ICommandeRepository CommandeRepository => Container.Resolve<ICommandeRepository>();
        public ISoldeFarRepository SoldeFarRepository => Container.Resolve<ISoldeFarRepository>();
        public IUtilisateurRepository UtilisateurRepository => Container.Resolve<IUtilisateurRepository>();
        public ICodeMajorationRepository CodeMajorationRepository => Container.Resolve<ICodeMajorationRepository>();
        public ITacheRepository TacheRepository => Container.Resolve<ITacheRepository>();
        public IRepository<CIEnt> CiRepository => Container.Resolve<IRepository<CIEnt>>();
        public IEtablissementComptableRepository EtablissementComptableRepository => Container.Resolve<IEtablissementComptableRepository>();
        public ISocieteRepository SocieteRepository => Container.Resolve<ISocieteRepository>();
        public IDatesClotureComptableRepository DatesClotureComptableRepository => Container.Resolve<IDatesClotureComptableRepository>();
        #endregion

        #region ----------------------------------------- VALIDATORS -----------------------------------------------------
        public ICIValidator CIVal => Container.Resolve<ICIValidator>();
        public ITacheRecetteValidatorOld TacheRecetteVal => Container.Resolve<ITacheRecetteValidatorOld>();
        public ICommandeValidator CommandeVal => Container.Resolve<ICommandeValidator>();
        public ICommandeLigneValidator CommandeLigneVal => Container.Resolve<ICommandeLigneValidator>();
        public IRessourceValidator RessourceVal => Container.Resolve<IRessourceValidator>();
        public IRoleValidator RoleVal => Container.Resolve<IRoleValidator>();
        public ITacheValidator TacheVal => Container.Resolve<ITacheValidator>();
        public IModuleValidator ModuleVal => Container.Resolve<IModuleValidator>();
        #endregion


        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Dispose();
        }

        /// <summary>
        ///   Génération aléatoire d'une chaine de caractères
        /// </summary>
        /// <param name="length">Longueur de la chaine</param>
        /// <returns>Chaine de caractères</returns>
        protected string GenerateString(int length)
        {
            return new string(Enumerable.Repeat(Chars, length)
                                        .Select(s => s[this.random.Next(s.Length)])
                                        .ToArray());
        }

        public void Dispose()
        {
            if (this.FredContext != null)
            {
                this.FredContext.Dispose();
            }
        }
    }
}
