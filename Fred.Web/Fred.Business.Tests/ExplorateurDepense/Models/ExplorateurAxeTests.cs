using FluentAssertions;
using Fred.Business.ExplorateurDepense;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.ExplorateurDepense.Models
{
    [TestClass]
    public class ExplorateurAxeTests
    {

        [TestMethod]
        public void GetIdentifier_should_be_the_concat_of_all_keys_of_tree()
        {
            // Arrange
            var parent = new ExplorateurAxe();
            parent.Key = "Chapitre 26";

            var childLevel1 = new ExplorateurAxe();
            childLevel1.Key = "SousChapitre 40";
            childLevel1.Parent = parent;

            var childLevel2 = new ExplorateurAxe();
            childLevel2.Key = "Ressource 4982";
            childLevel2.Parent = childLevel1;

            // parent
            var parentResult = parent.GetIdentifier();
            var childLevel1Result = childLevel1.GetIdentifier();
            var childLevel2Result = childLevel2.GetIdentifier();

            // Assert
            parentResult.Should().Be("Chapitre 26");
            childLevel1Result.Should().Be("Chapitre 26 SousChapitre 40");
            childLevel2Result.Should().Be("Chapitre 26 SousChapitre 40 Ressource 4982");
        }

    }
}
