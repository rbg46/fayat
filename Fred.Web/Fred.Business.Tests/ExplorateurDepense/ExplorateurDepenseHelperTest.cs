using System;
using System.Collections.Generic;
using Fred.Business.ExplorateurDepense;
using Fred.Entities.CI;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Valorisation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.ExplorateurDepense
{
    /// <summary>
    /// Permet de tester les méthodes de l'helper du manager de l'explorateur de dépense
    /// </summary>
    [TestClass]
    public class ExplorateurDepenseHelperTest
    {
        /// <summary>
        /// Teste la conversion d'un ValorisationEnt en ExplorateurDepense
        /// </summary>
        [TestMethod]
        public void ConvertValorisationEntToExplorateurDepense()
        {
            /////////////
            // Arrange //
            /////////////

            ExplorateurDepenseHelper explorateurDepenseHelperTest = new ExplorateurDepenseHelper();
            DateTime periodeDebut = new DateTime(2019, 1, 15);
            DateTime periodeFin = new DateTime(2019, 6, 15);
            DateTime fakeDate = new DateTime(2019, 2, 15);
            const int fakePersonnelId = 1;
            const int fakeNatureId = 1;
            const int fakeRapportId = 9719;
            DatesClotureComptableEnt fakeDateClotureComptableEnt = new DatesClotureComptableEnt
            {
                DateCloture = fakeDate,
                DateTransfertFAR = fakeDate
            };
            SocieteEnt fakeSocieteEnt = new SocieteEnt
            {
                Code = "CodeTest"
            };
            PersonnelEnt fakePersonnelEnt = new PersonnelEnt
            {
                PersonnelId = fakePersonnelId,
                Matricule = "MatriculeTest",
                Prenom = "PrenomTest",
                Societe = fakeSocieteEnt,
                ContratInterimaires = new List<ContratInterimaireEnt> { new ContratInterimaireEnt() }
            };
            NatureEnt fakeNatureEnt = new NatureEnt
            {
                NatureId = fakeNatureId
            };
            ReferentielEtenduEnt fakeReferentielEtenduEnt = new ReferentielEtenduEnt
            {
                Ressource = new RessourceEnt(),
                NatureId = fakeNatureId,
                Nature = fakeNatureEnt,
            };
            ValorisationEnt valorisationEnt = new ValorisationEnt
            {
                CI = new CIEnt(),
                PersonnelId = fakePersonnelId,
                Personnel = fakePersonnelEnt,
                ReferentielEtendu = fakeReferentielEtenduEnt,
                TacheId = 1,
                Tache = new TacheEnt(),
                UniteId = 1,
                Unite = new UniteEnt(),
                DeviseId = 1,
                Devise = new DeviseEnt(),
                PUHT = 1,
                Quantite = 1,
                Montant = 1,
                Date = fakeDate,
                ValorisationId = 1,
                RapportId = fakeRapportId
            };

            /////////
            // Act //
            /////////

            IEnumerable<ExplorateurDepenseGeneriqueModel> rawResult
                = explorateurDepenseHelperTest.ConvertValorisation(new List<ValorisationEnt> { valorisationEnt },
                periodeDebut, periodeFin,
                new List<DatesClotureComptableEnt> { fakeDateClotureComptableEnt });

            List<ExplorateurDepenseGeneriqueModel> formatResult = (List<ExplorateurDepenseGeneriqueModel>)rawResult;

            ExplorateurDepenseGeneriqueModel result = formatResult[0];

            ////////////
            // Assert //
            ////////////

            Assert.IsNotNull(result);

            // US 6461 : dans le cas d'une valorisation, le Libelle2 doit contenir le rapportId
            Assert.IsTrue(result.Libelle2.Contains(fakeRapportId.ToString()));
        }
    }
}
