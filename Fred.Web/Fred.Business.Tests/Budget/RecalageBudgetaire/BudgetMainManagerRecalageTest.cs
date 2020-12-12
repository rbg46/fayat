using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Budget;
using Fred.Business.Budget.Avancement;
using Fred.Business.CI;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielFixe;
using Fred.Business.Unite;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Fred.Business.Tests.ModelBuilder.BudgetEtat;
using static Fred.Business.Tests.ModelBuilder.BudgetEtBudgetT4;
using static Fred.Business.Tests.ModelBuilder.CiOrganisationSociete;
using static Fred.Business.Tests.ModelBuilder.TacheRessource;
using static Fred.Entities.Constantes;

namespace Fred.Business.Tests.Budget.RecalageBudgetaire
{
    [TestClass]
    public class BudgetMainManagerRecalageTest
    {
        private readonly Mock<IBudgetManager> budgetManager = new Mock<IBudgetManager>();
        private readonly Mock<IBudgetCopieManager> budgetCopieManager = new Mock<IBudgetCopieManager>();
        private readonly Mock<IBudgetT4Manager> budgetT4Manager = new Mock<IBudgetT4Manager>();
        private readonly Mock<IUniteManager> uniteManager = new Mock<IUniteManager>();
        private readonly Mock<IAvancementManager> avancementManager = new Mock<IAvancementManager>();
        private readonly Mock<ITacheManager> tacheManager = new Mock<ITacheManager>();
        private readonly Mock<ICIManager> ciManager = new Mock<ICIManager>();
        private readonly Mock<IBudgetSousDetailManager> budgetSousDetailManager = new Mock<IBudgetSousDetailManager>();
        private readonly Mock<IReferentielFixeManager> referentielFixeManager = new Mock<IReferentielFixeManager>();
        private readonly Mock<ITacheRepository> tacheRepository = new Mock<ITacheRepository>();
        private readonly Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

        private BudgetMainManagerTest budgetMainManagerTest;
        private readonly BudgetEnt fakeBudgetEnApplication = GetFakeBudgetEnApplication();
        private readonly BudgetEnt fakeCopieBudget = GetFakeBudgetEnApplication();
        private readonly UniteEnt fakeUniteForfait = GetFakeUnite();
        private const int fakeUtilisateurId = 0;

        private readonly IList<BudgetSousDetailEnt> budgetSousDetailInseres = new List<BudgetSousDetailEnt>();
        private readonly IList<BudgetT4Ent> budgetT4Inseres = new List<BudgetT4Ent>();
        private readonly IList<TacheEnt> tache4Inserees = new List<TacheEnt>();
        private readonly ICollection<int?> tache3IdAyantDesDepenses = new List<int?>();
        private readonly ICollection<int?> ressourceIdAyantDesDepenses = new List<int?>();
        private List<IGrouping<TacheEnt, BudgetT4Ent>> budgetT4GrouppedByT3;
        private const int periode = 201811;

        /// <summary>
        /// Faux montant dépenses utilisées pour le test, faire très attention en changant cette valeur
        /// car elle est pour l'instant utilisée pour donner une valeur de dépenses à tous les T3 liés à la valeur de retour de la fonction GetFakeRessourceId()
        /// </summary>
        private const int fakeMontantDepense = 500;

        [TestInitialize]
        public void Init()
        {
            fakeBudgetEnApplication.PeriodeDebut = periode;
            fakeBudgetEnApplication.PeriodeFin = periode;

            fakeBudgetEnApplication.Version = "1.0";
            fakeCopieBudget.Version = "1.1";
            fakeCopieBudget.BudgetId = fakeBudgetEnApplication.BudgetId + 1;
            fakeCopieBudget.BudgetEtat = GetFakeBudgetEtatBrouillon();
            fakeCopieBudget.BudgetEtatId = fakeCopieBudget.BudgetEtat.BudgetEtatId;
            fakeCopieBudget.Workflows = new List<BudgetWorkflowEnt>()
            {
                new BudgetWorkflowEnt()
                {
                    AuteurId = fakeUtilisateurId,
                    Budget = fakeCopieBudget,
                    EtatInitialId = null,
                    EtatCibleId = fakeCopieBudget.BudgetEtatId,
                    Date = DateTime.Now
                }
            };

            fakeBudgetEnApplication.BudgetT4s = new List<BudgetT4Ent>()
            {
                GetFakeBudgetT4()
            };

            fakeCopieBudget.BudgetT4s = new List<BudgetT4Ent>()
            {
                GetFakeBudgetT4()
            };

            var societe = GetFakeSociete();
            ciManager.Setup(cim => cim.GetSocieteByCIId(fakeBudgetEnApplication.CiId, false))
                .Returns(societe);

            var fakeRessource = GetFakeRessource();
            referentielFixeManager.Setup(rfm => rfm.GetListRessourceIdByRessourceWithSameNatureInRefEtendu(It.IsAny<int>(), societe.SocieteId))
                .Returns(new List<int>() { fakeRessource.RessourceId });


            budgetCopieManager.Setup(bm => bm.CopierBudgetDansMemeCiAsync(fakeBudgetEnApplication.BudgetId, fakeUtilisateurId, false))
                    .Returns(Task.FromResult(fakeCopieBudget));

            budgetManager.Setup(bm => bm.GetBudget(fakeBudgetEnApplication.BudgetId, false))
                    .Returns(fakeBudgetEnApplication);

            budgetManager.Setup(bm => bm.FindById(fakeBudgetEnApplication.BudgetId))
                    .Returns(fakeBudgetEnApplication);

            uniteManager.Setup(um => um.GetUnite(CodeUnite.Forfait))
                    .Returns(fakeUniteForfait);

            budgetT4GrouppedByT3 = fakeBudgetEnApplication.BudgetT4s.GroupBy(bt4 => bt4.T4.Parent).ToList();
            budgetT4Manager.Setup(bt4m => bt4m.GetT4GroupByT3ByBudgetId(fakeBudgetEnApplication.BudgetId))
                    .Returns(budgetT4GrouppedByT3);


            //Pour le test on fait simple et on dit qu'on a des dépenses sur tous les t3 du budgets
            foreach (var t3Id in budgetT4GrouppedByT3.Select(group => group.Key))
            {
                tache3IdAyantDesDepenses.Add(t3Id.TacheId);
            }

            ressourceIdAyantDesDepenses.Add(fakeRessource.RessourceId);

            budgetSousDetailManager.Setup(bsdm => bsdm.InsereSousDetail(It.IsAny<BudgetSousDetailEnt>()))
                .Callback<BudgetSousDetailEnt>(param => budgetSousDetailInseres.Add(param));

            budgetT4Manager.Setup(bt4m => bt4m.Add(It.IsAny<BudgetT4Ent>()))
                .Callback<BudgetT4Ent>(param => budgetT4Inseres.Add(param));

            tacheManager.Setup(tm => tm.AddTache4(It.IsAny<TacheEnt>(), fakeCopieBudget.BudgetId, It.IsAny<List<TacheEnt>>()))
                .Callback<TacheEnt, int>((tache, budgetId) => tache4Inserees.Add(tache));


            budgetMainManagerTest = new BudgetMainManagerTest(
                avancementManager.Object,
                null,
                null,
                null,
                budgetManager.Object,
                null,
                budgetSousDetailManager.Object,
                budgetT4Manager.Object,
                null,
                null,
                tacheManager.Object,
                ciManager.Object,
                null,
                referentielFixeManager.Object,
                null,
                null,
                uniteManager.Object,
                null,
                null,
                null,
                uow.Object,
                tacheRepository.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                budgetCopieManager.Object,
                null,
                null,
                null,
                null);
        }

        [TestMethod]
        [Ignore]
        public async Task TestUpdateBudgetT4FromAvancementQuantite()
        {
            var fakeT4AvancementQuantite = GetFakeBudgetT4();
            fakeT4AvancementQuantite.TypeAvancement = (int)TypeAvancementBudget.Quantite;

            budgetT4Manager.Setup(t4m => t4m.GetByTacheIdAndBudgetId(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(fakeT4AvancementQuantite);

            var quantiteAvancement = fakeT4AvancementQuantite.QuantiteARealiser - 1;
            var avancementTest = new AvancementEnt
            {
                QuantiteSousDetailAvancee = quantiteAvancement

            };

            avancementManager.Setup(am => am.GetAvancement(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(avancementTest);

            await budgetMainManagerTest.RecalageBudgetaireAsync(fakeBudgetEnApplication.BudgetId, fakeUtilisateurId, periode).ConfigureAwait(false);

            //La quantité attendue est nécessairement de 1 car elle doit etre calculée comme étant la différence entre la quantité prévue à l'origine 
            // et la quantité avancée, dans le cas du test, cette valeur est forcément à 1
            var expectedQuantite = 1;
            Assert.AreEqual(expectedQuantite, fakeT4AvancementQuantite.QuantiteARealiser);
            Assert.AreEqual(fakeT4AvancementQuantite.MontantT4, expectedQuantite * fakeT4AvancementQuantite.PU);

        }

        [TestMethod]
        [Ignore]
        public async Task TestUpdateBudgetT4FromAvancementPourcentage()
        {
            var fakeT4AvancementPourcentage = GetFakeBudgetT4();
            fakeT4AvancementPourcentage.TypeAvancement = (int)TypeAvancementBudget.Pourcentage;

            budgetT4Manager.Setup(t4m => t4m.GetByTacheIdAndBudgetId(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(fakeT4AvancementPourcentage);


            var avancementTest = new AvancementEnt
            {
                PourcentageSousDetailAvance = 50m

            };
            var expectedQuantite = fakeT4AvancementPourcentage.QuantiteARealiser - (fakeT4AvancementPourcentage.QuantiteARealiser * avancementTest.PourcentageSousDetailAvance / 100);

            avancementManager.Setup(am => am.GetAvancement(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(avancementTest);

            await budgetMainManagerTest.RecalageBudgetaireAsync(fakeBudgetEnApplication.BudgetId, fakeUtilisateurId, periode).ConfigureAwait(false);

            Assert.AreEqual(expectedQuantite, fakeT4AvancementPourcentage.QuantiteARealiser);
            Assert.AreEqual(fakeT4AvancementPourcentage.MontantT4, expectedQuantite * fakeT4AvancementPourcentage.PU);
        }

        [TestMethod]
        [Ignore]
        public async Task RecalageTest()
        {
            await budgetMainManagerTest.RecalageBudgetaireAsync(fakeBudgetEnApplication.BudgetId, fakeUtilisateurId, periode).ConfigureAwait(false);

            CheckTache4Inseree();
            CheckBudgetT4Insere();
            CheckBudgetSousDetailInsere();
        }

        private void CheckTache4Inseree()
        {
            //Le recalage créé une nouvelle tache de niveau 4 pour tous les t3 du budget, donc on ne doit pas avoir plus de Tache4 
            //insérée que ce qu'on a de T3 
            Assert.AreEqual(budgetT4GrouppedByT3.Count, tache4Inserees.Count);

            foreach (var group in budgetT4GrouppedByT3)
            {
                var t3 = group.Key;
                var t4CreesPourt3 = tache4Inserees.Where(t4 => t4.ParentId == t3.TacheId);

                Assert.IsTrue(t4CreesPourt3.Any());
                foreach (var t4Cree in t4CreesPourt3)
                {
                    Assert.AreEqual(4, t4Cree.Niveau);
                    Assert.AreEqual(fakeBudgetEnApplication.CiId, t4Cree.CiId);
                    Assert.AreEqual(fakeCopieBudget.BudgetId, t4Cree.BudgetId);
                    StringAssert.StartsWith(t4Cree.Code, "T4REV");
                    Assert.AreEqual(t4Cree.Libelle, "T4 Réalisée");
                }
            }
        }

        private void CheckBudgetT4Insere()
        {
            //Même chose que pour les taches de niveau 4, le recalage insère autant de budget T4 qu'il y a de T3 dans le budget
            Assert.AreEqual(budgetT4GrouppedByT3.Count, budgetT4Inseres.Count);


            foreach (var group in budgetT4GrouppedByT3)
            {
                var t3 = group.Key;
                var budgetT4PourT3 = budgetT4Inseres.Where(t4 => t4.T4.ParentId == t3.TacheId);

                Assert.IsTrue(budgetT4PourT3.Any());
                foreach (var t4Insere in budgetT4PourT3)
                {
                    Assert.AreEqual(1, t4Insere.QuantiteARealiser);
                    Assert.AreEqual(1, t4Insere.QuantiteDeBase);

                    //Cette valeur repose sur le fait que dans la fonction [TestInitialize] on affecte une depense ayant la valeur fakeMontantDepense
                    //Pour tous les t3 du budgets
                    var countSousDetailsInAllT3 = group.ToList().Sum(bt4 => bt4.BudgetSousDetails.Count);
                    var expectedMontantDepense = fakeMontantDepense * countSousDetailsInAllT3;

                    Assert.AreEqual(fakeMontantDepense, t4Insere.MontantT4);
                    Assert.AreEqual(fakeMontantDepense, t4Insere.PU);
                    Assert.AreEqual(fakeUniteForfait.UniteId, t4Insere.UniteId);
                    Assert.IsTrue(t4Insere.IsReadOnly);
                    Assert.AreEqual((int)TypeAvancementBudget.Pourcentage, t4Insere.TypeAvancement);
                }
            }
        }

        private void CheckBudgetSousDetailInsere()
        {
            //On n'insère autant de sous détail qu'il y a de dépenses sur le budget
            Assert.AreEqual(tache3IdAyantDesDepenses.Count, budgetSousDetailInseres.Count);


            foreach (var budgetT4Origine in fakeBudgetEnApplication.BudgetT4s)
            {
                foreach (var sousDetailOrigine in budgetT4Origine.BudgetSousDetails)
                {

                    var sousDetailInsere = budgetSousDetailInseres.FirstOrDefault(sd => sd.RessourceId == sousDetailOrigine.RessourceId);
                    //Il est tout à fait possible qu'un des sous détails qu'on avait à l'origine (appelons le A) ne possède pas de dépenses et donc
                    //aucun sous détail ne sera inséré pour la ressource de ce sous détail A
                    if (sousDetailInsere != null)
                    {
                        //Le montant dépense est le même pour tous dans ce test
                        var expectedQuantite = fakeMontantDepense / sousDetailOrigine.PU;
                        Assert.AreEqual(expectedQuantite, sousDetailInsere.Quantite);
                        Assert.AreEqual(sousDetailOrigine.PU, sousDetailInsere.PU);
                        Assert.AreEqual(fakeMontantDepense, sousDetailInsere.Montant);
                        Assert.IsNotNull(sousDetailInsere.BudgetT4);
                    }
                }
            }
        }
    }
}
