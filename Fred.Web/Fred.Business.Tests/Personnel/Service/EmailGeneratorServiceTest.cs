using FluentAssertions;
using Fred.Business.Personnel;
using Fred.Common.Tests;
using Fred.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Personnel.Service
{
    [TestClass]
    public class EmailGeneratorServiceTest : BaseTu<EmailGeneratorService>
    {

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestMethod]
        [TestCategory("GenerateEmail")]
        [Description("cas classique")]
        [DataTestMethod]
        [DataRow("Nom", "Prenom", "p.nom@razel-bec.fayat.com")]
        public void GenerateEmail_WithNameAndLastname_ReturnEmailWithInitialLetterOfName(string nom, string prenom, string expectedEmail)
        {
            var email = Actual.GenerateEmail(nom, prenom, Constantes.CodeSocietePayeRazelBec, 1);
            email.Should().Be(expectedEmail);
        }

        [TestMethod]
        [TestCategory("GenerateEmail")]
        [Description("cas prénoms composés")]
        [DataTestMethod]
        [DataRow("Dupont", "Marie-Pierre", "mp.dupont@razel-bec.fayat.com")]
        [DataRow("Dupont", "Marie Pierre", "mp.dupont@razel-bec.fayat.com")]
        public void GenerateEmail_WithNamesWithDashOrSpaceBetween_ReturnEmailWithInitialLetterOfNames(string nom, string prenom, string expectedEmail)
        {
            var email = Actual.GenerateEmail(nom, prenom, Constantes.CodeSocietePayeRazelBec, 1);
            email.Should().Be(expectedEmail);
        }



        [TestMethod]
        [TestCategory("GenerateEmail")]
        [Description("cas noms composés")]
        [DataTestMethod]
        [DataRow("Dupont Dutout", "Marie", "m.dupontdutout@razel-bec.fayat.com")]
        [DataRow("Dupont D'Utout", "Marie", "m.dupontdutout@razel-bec.fayat.com")]
        [DataRow("Dupont-Dutout", "Marie", "m.dupontdutout@razel-bec.fayat.com")]
        public void GenerateEmail_WithLastnamesWithSpaceOrApostropheOrDashBetween_ReturnEmailWithLastnamesWithoutApostropheOrSpaceOrDash(string nom, string prenom, string expectedEmail)
        {
            var email = Actual.GenerateEmail(nom, prenom, Constantes.CodeSocietePayeRazelBec, 1);
            email.Should().Be(expectedEmail);
        }



        [TestMethod]
        [TestCategory("GenerateEmail")]
        [Description("cas noms")]
        [DataTestMethod]
        [DataRow("D'Upont", "Marie", "m.dupont@razel-bec.fayat.com")]
        [DataRow("Le Bris", "Marie", "m.lebris@razel-bec.fayat.com")]
        [DataRow("Le-Bris", "Marie", "m.lebris@razel-bec.fayat.com")]
        public void GenerateEmail_WithLastnameWithApostropheOrSpaceOrDash_ReturnEmailWithLastnameWithoutApostropheOrSpaceOrDash(string nom, string prenom, string expectedEmail)
        {
            var email = Actual.GenerateEmail(nom, prenom, Constantes.CodeSocietePayeRazelBec, 1);
            email.Should().Be(expectedEmail);
        }


    }
}
