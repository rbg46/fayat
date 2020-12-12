using System;
using Fred.Business.Email.ActivitySummary.Builders;
using Fred.Entities.ActivitySummary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Email.ActivitySummary
{
    [TestClass]
    public class JalonColorProviderTest
    {


        [TestMethod]
        [TestCategory("No moq necessarry")]
        [TestCategory("JalonColorProvider")]
        public void Verifie_Si_N_supp_ou_egal_a_M_moins_1_mettre_le_fond_de_la_case_en_vert()
        {
            // RG:
            // M est la valeur du mois en cours, N est la valeur du mois à afficher dans le tableau :
            // Si N >= M – 1, mettre le fond de la case en vert.
            var jalonColorProvider = new JalonColorProvider();

            var now = new DateTime(2018, 02, 05);

            var dateToCheck = new DateTime(2018, 02, 05);

            var colorJalon = jalonColorProvider.GetJalonColor(now, dateToCheck);

            Assert.AreEqual(colorJalon, ColorJalon.ColorGreen, "Si N >= M – 1, mettre le fond de la case en vert");

        }


        [TestMethod]
        [TestCategory("No moq necessarry")]
        [TestCategory("JalonColorProvider")]
        public void Verifie_Si_N_egal_a_M_moins_2_mettre_le_fond_de_la_case_en_orange()
        {
            // RG:
            // M est la valeur du mois en cours, N est la valeur du mois à afficher dans le tableau :
            // Si N >= M – 1, mettre le fond de la case en vert.
            var jalonColorProvider = new JalonColorProvider();

            var now = new DateTime(2018, 02, 05);

            var dateToCheck = new DateTime(2017, 12, 25);

            var colorJalon = jalonColorProvider.GetJalonColor(now, dateToCheck);

            Assert.AreEqual(colorJalon, ColorJalon.ColorOrange, "Si N = M – 2, mettre le fond de la case en orange");

        }


        [TestMethod]
        [TestCategory("No moq necessarry")]
        [TestCategory("JalonColorProvider")]
        [Ignore]
        public void Verifie_Si_N_inferieur_a_M_moins_2_mettre_le_fond_de_la_case_en_rouge()
        {
            // RG:
            // M est la valeur du mois en cours, N est la valeur du mois à afficher dans le tableau :
            // Si N >= M – 1, mettre le fond de la case en vert.
            var jalonColorProvider = new JalonColorProvider();

            var now = new DateTime(2018, 02, 05);

            var dateToCheck = new DateTime(2016, 12, 25);

            var colorJalon = jalonColorProvider.GetJalonColor(now, dateToCheck);

            Assert.AreEqual(colorJalon, ColorJalon.ColorRed, "Si N < M – 2, mettre le fond de la case en rouge");

        }


        [TestMethod]
        [TestCategory("No moq necessarry")]
        [TestCategory("JalonColorProvider")]
        public void Verifie_Si_date_nulle_couleur_blue()
        {
            // RG:
            // M est la valeur du mois en cours, N est la valeur du mois à afficher dans le tableau :
            // Si N >= M – 1, mettre le fond de la case en vert.
            var jalonColorProvider = new JalonColorProvider();

            var now = new DateTime(2018, 02, 05);

            var colorJalon = jalonColorProvider.GetJalonColor(now, null);

            Assert.AreEqual(colorJalon, ColorJalon.ColorBlue, "Pas de date => coleur blue");

        }

    }
}
