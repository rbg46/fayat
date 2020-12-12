using System;
using System.Linq;
using System.Threading;
using Fred.Entities.Facture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestFacture : FredBaseTest
    {
        private static FactureEnt factureArDupliquation;

        /// <summary>
        ///   Initialise l'ensemble des tests de la classe.
        /// </summary>
        /// <param name="context">Le contexte de tests.</param>
        [ClassInitialize]
        public static void InitAllTests(TestContext context)
        {
            factureArDupliquation = new FactureEnt
            {
                Commentaire = "Ceci est une facture de test",
                CompteFournisseur = "CompteFournisseur",
                CompteGeneral = "CompteGeneral",
                DateComptable = DateTime.Now.AddMonths(-2),
                DateCreation = DateTime.Now,
                DateEcheance = DateTime.Now.AddDays(-62),
                DateFacture = DateTime.Now.AddMonths(-3),
                DateGestion = DateTime.Now.AddDays(-55),
                DateImport = DateTime.Now,
                DeviseId = 1,

                //EtablissementId = 1,//NON SEP
                Folio = "JCA",
                JournalId = 1,
                FournisseurId = 1,
                ModeReglement = "Chèque",
                MontantHT = 1500,
                MontantTTC = 1800,
                MontantTVA = 300,
                NoBonCommande = "BC_001",
                NoBonlivraison = "BL_001",
                NoFacture = "FAC_001",
                NoFactureFournisseur = "FAC_FOURN_001",
                NoFMFI = "FMFI_001",
                SocieteId = 1, //SEP
                Typefournisseur = "FOURN_001"
            };
        }

        private static FactureEnt GenerateFacture()
        {
            Thread.Sleep(50);
            Random r = new Random();
            DateTime randomDate = new DateTime(r.Next(2010, 2016), r.Next(1, 12), r.Next(1, 28));
            return new FactureEnt
            {
                Commentaire = "Ceci est une facture de test",
                CompteFournisseur = "CompteFournisseur",
                CompteGeneral = "CompteGeneral",
                DateComptable = randomDate.AddMonths(-2),
                DateCreation = DateTime.Now,
                DateEcheance = randomDate.AddDays(-62),
                DateFacture = randomDate.AddMonths(-3),
                DateGestion = randomDate.AddDays(-55),
                DateImport = DateTime.Now,
                DeviseId = r.Next(1, 168),
                Folio = "JCA",
                JournalId = r.Next(1, 11),
                FournisseurId = r.Next(1, 5),
                ModeReglement = "Chèque",
                MontantHT = r.Next(0, 100000),
                MontantTTC = r.Next(0, 100000),
                MontantTVA = r.Next(0, 100000),
                NoBonCommande = string.Format("BC_{0}", r.Next(1, 100000)),
                NoBonlivraison = string.Format("BL_{0}", r.Next(1, 100000)),
                NoFacture = string.Format("FAC_{0}", r.Next(1, 100000)),
                NoFactureFournisseur = string.Format("FAC_FOURN_{0}", r.Next(1, 100000)),
                NoFMFI = string.Format("FMFI_{0}", r.Next(1, 100000)),
                Typefournisseur = string.Format("TYPE_FOURN_{0}", r.Next(1, 100000))
            };
        }

        private static FactureEnt GenerateFactureNonSep()
        {
            Thread.Sleep(50);
            Random r = new Random();
            FactureEnt resu = GenerateFacture();
            resu.EtablissementId = r.Next(1, 6); //NON SEP
            return resu;
        }

        private FactureEnt GenerateFactureSep()
        {
            Thread.Sleep(50);
            FactureEnt resu = GenerateFacture();
            resu.SocieteId = 1; //SEP
            return resu;
        }

        private static FactureLigneEnt GenerateFactureLigne()
        {
            Thread.Sleep(50);
            Random r = new Random();
            return new FactureLigneEnt
            {
                AffaireId = r.Next(1, 7),
                MontantHT = r.Next(0, 1000000),
                DateCreation = DateTime.Now,
                NatureId = r.Next(1, 270),
                NoBonLivraison = string.Format("BL_{0}", r.Next(1, 100000)),
                PrixUnitaire = r.Next(0, 100),
                Quantite = r.Next(0, 100)
            };
        }

        #region FactureAR manager

        /// <summary>
        ///   Teste que la liste des LogImports retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetFactureARList()
        {

            var resu = FactureMgr.GetFactureARList();
            Assert.IsTrue(resu != null);
        }

        /// <summary>
        ///   Teste la RG_645_002 - Un facture ne peut être importé qu'une seule fois
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void TestDupplicationFacture()
        {
            bool doublonDetecte = false;

            var lstFacture = FactureMgr.GetNewLigneFactureARList().ToList();

            Random r = new Random();
            int nbLigne = r.Next(1, 100);
            for (int i = 0; i < nbLigne; i++)
            {
                lstFacture.Add(GenerateFactureLigne());
            }

            factureArDupliquation.ListLigneFacture = lstFacture.ToArray();

            try
            {
                FactureMgr.Add(factureArDupliquation);
                factureArDupliquation.FactureId = 0;
                FactureMgr.Add(factureArDupliquation);
            }
            catch
            {
                doublonDetecte = true;
            }


            Assert.IsTrue(doublonDetecte);
        }

        /// <summary>
        ///   Teste l'ajout d'une facture Random
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void AddFactureAR_RandomSEP()
        {

            var lstFacture = FactureMgr.GetNewLigneFactureARList().ToList();

            Random r = new Random();
            int nbLigne = r.Next(1, 100);
            for (int i = 0; i < nbLigne; i++)
            {
                lstFacture.Add(GenerateFactureLigne());
            }


            FactureEnt factureRandom = GenerateFactureSep();
            factureRandom.ListLigneFacture = lstFacture.ToArray();

            int resu = FactureMgr.Add(factureRandom);
            Assert.IsTrue(resu > 0);
        }

        /// <summary>
        ///   Teste l'ajout d'une facture Random
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        [Ignore]
        public void AddFactureAR_RandomNonSEP()
        {

            var lstFacture = FactureMgr.GetNewLigneFactureARList().ToList();

            Random r = new Random();
            int nbLigne = r.Next(1, 100);
            for (int i = 0; i < nbLigne; i++)
            {
                lstFacture.Add(GenerateFactureLigne());
            }

            FactureEnt factureRandom = GenerateFactureNonSep();
            factureRandom.ListLigneFacture = lstFacture.ToArray();

            int resu = FactureMgr.Add(factureRandom);
            Assert.IsTrue(resu > 0);
        }

        #endregion
    }
}
