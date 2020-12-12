// Décommenter pour activer le vidage automatique du cache en DEBUG
// En revanche, ne permet plus le débogage du JS dans VS.
//#define AUTO_CLEAR_CACHE_IN_DEBUG

using System.Web.Optimization;
using Fred.Web.App_Start.Bundles;
using Fred.Web.App_Start.BundlesFactories;

namespace Fred.Web
{
    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // activation ou non de la minification      
#if DEBUG && AUTO_CLEAR_CACHE_IN_DEBUG
      BundleTable.EnableOptimizations = true;
#elif DEBUG
            BundleTable.EnableOptimizations = false;
#else
      BundleTable.EnableOptimizations = true;
#endif

            // suppression de la réorganisation microsoft, on s'occupe d'ordonner nos fichiers nous même
            bundles.FileSetOrderList.Clear();

            RegisterNewBundles(bundles);

#if DEBUG && AUTO_CLEAR_CACHE_IN_DEBUG
      // Using the ASP.Net bundler without minification : https://www.gembalabs.com/2015/04/20/using-the-asp-net-bundler-without-minification/
      foreach (var bundle in BundleTable.Bundles)
      {
        bundle.Transforms.Clear();
      }
#endif
        }

        private static void RegisterNewBundles(BundleCollection bundles)
        {
            new VendorBundleFactory(bundles).CreateBundle();
            new AppBundleFactory(bundles).CreateBundle();

            new AuthentificationLogIndexBundleFactory(bundles).CreateBundle();
            new BaremeExploitationOrgaBundleFactory(bundles).CreateBundle(false);
            new BaremeExploitationCIBundleFactory(bundles).CreateBundle(false);
            new BudgetIndexBundleFactory(bundles).CreateBundle();
            new BudgetBibliothequePrixBundleFactory(bundles).CreateBundle(false);
            new BudgetComparaisonBundleFactory(bundles).CreateBundle(false);
            new BudgetDetailBundleFactory(bundles).CreateBundle(false);
            new CIDetailBundleFactory(bundles).CreateBundle();
            new CIIndexBundleFactory(bundles).CreateBundle();
            new CodeAbsenceIndexBundleFactory(bundles).CreateBundle();
            new CodeDeplacementIndexBundleFactory(bundles).CreateBundle();
            new CodeMajorationIndexBundleFactory(bundles).CreateBundle();
            new CodeZoneDeplacementIndexBundleFactory(bundles).CreateBundle();
            new CommandeDetailBundleFactory(bundles).CreateBundle();
            new CommandeIndexBundleFactory(bundles).CreateBundle();
            new DatesCalendrierPaieIndexBundleFactory(bundles).CreateBundle();
            new DatesClotureComptableIndexBundleFactory(bundles).CreateBundle();
            new CloturesPeriodesBundleFactory(bundles).CreateBundle();
            new DepenseExplorateurBundleFactory(bundles).CreateBundle();
            new EtablissementComptableIndexBundleFactory(bundles).CreateBundle();
            new EtablissementPaieIndexBundleFactory(bundles).CreateBundle();
            new FactureIndexBundleFactory(bundles).CreateBundle();
            new FournisseurIndexBundleFactory(bundles).CreateBundle();
            new IndemniteDeplacementIndexBundleFactory(bundles).CreateBundle();
            new LookupIndexBundleFactory(bundles).CreateBundle();
            new MaterielIndexBundleFactory(bundles).CreateBundle();
            new ModuleIndexBundleFactory(bundles).CreateBundle();
            new NatureIndexBundleFactory(bundles).CreateBundle();
            new JournalComptableIndexBundleFactory(bundles).CreateBundle();
            new OrganisationIndexBundleFactory(bundles).CreateBundle();
            new ParamTarifsReferentielsIndexBundleFactory(bundles).CreateBundle();
            new PersonnelEditBundleFactory(bundles).CreateBundle();
            new PersonnelIndexBundleFactory(bundles).CreateBundle();
            new PersonnelEquipeBundleFactory(bundles).CreateBundle();
            new RapportJournalierBundleFactory(bundles).CreateBundle();
            new RapportListeBundleFactory(bundles).CreateBundle();
            new ListePointagePersonnelBundleFactory(bundles).CreateBundle();
            new PrimeIndexBundleFactory(bundles).CreateBundle();
            new RapprochementFactureIndexBundleFactory(bundles).CreateBundle();
            new ReceptionIndexBundleFactory(bundles).CreateBundle();
            new AreasReferentialViewsDeviseIndexBundleFactory(bundles).CreateBundle();
            new ReferentielEtenduIndexBundleFactory(bundles).CreateBundle();
            new ReferentielFixesIndexBundleFactory(bundles).CreateBundle();
            new RessourcesSpecifiquesCIBundleFactory(bundles).CreateBundle();
            new ReferentielTachesIndexBundleFactory(bundles).CreateBundle();
            new RoleIndexBundleFactory(bundles).CreateBundle();
            new SocieteIndexBundleFactory(bundles).CreateBundle();
            new ValidationPointageIndexBundleFactory(bundles).CreateBundle();
            new ValorisationIndexBundleFactory(bundles).CreateBundle();
            new ViewsAuthenticationConnectBundleFactory(bundles).CreateBundle();
            new ViewsHomeIndexBundleFactory(bundles).CreateBundle();
            new ViewsHomeMonitoringBundleFactory(bundles).CreateBundle();
            new ViewsLogIndexBundleFactory(bundles).CreateBundle();
            new OperationDiverseBundleFactory(bundles).CreateBundle();
            new ObjectifFlashListBundleFactory(bundles).CreateBundle();
            new ObjectifFlashBundleFactory(bundles).CreateBundle();
            new ReceptionTableauBundleFactory(bundles).CreateBundle();
            new ObjectifFlashListBundleFactory(bundles).CreateBundle();
            new CompteExploitationBundleFactory(bundles).CreateBundle();
            new AvancementBundleFactory(bundles).CreateBundle(false);
            new AvancementRecetteBundleFactory(bundles).CreateBundle();
            new BudgetControleBudgetaireBundleFactory(bundles).CreateBundle();
            new FeatureFlippingIndexBundleFactory(bundles).CreateBundle();
            new RapportHebdoBundleFactory(bundles).CreateBundle();
            new ExportPointagePersonnelBundleFactory(bundles).CreateBundle();
            new RapportPrimeIndexBundleFactory(bundles).CreateBundle();
            new ValidationDeMonServiceBundleFactory(bundles).CreateBundle();
            new MoyenBundleFactory(bundles).CreateBundle();
            new CacheBundleFactory(bundles).CreateBundle();
            new RessourcesRecommandeesBundleFactory(bundles).CreateBundle();
            new CommandeEnergieIndexBundleFactory(bundles).CreateBundle();
            new CommandeEnergieDetailBundleFactory(bundles).CreateBundle();
            new ClassificationSocietesBundleFactory(bundles).CreateBundle();
            new FamilleOperationDiverseBundleFactory(bundles).CreateBundle();
            new ValidationAffairesOuvriersBundleFactory(bundles).CreateBundle();
        }
    }
}
