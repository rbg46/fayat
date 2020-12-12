using System.Collections.Generic;
using FluentAssertions;
using Fred.Business.Referential.Service;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Fournisseur.Builder;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Referential.Fournisseur.Services
{
    /// <summary>
    /// classe de test <see cref="FournisseursImportService"/>
    /// </summary>
    [TestClass]
    public class FournisseursImportServiceTest : BaseTu<FournisseursImportService>
    {
        Mock<IFournisseurRepository> FournisseurRepo;

        [TestMethod]
        [TestCategory("FournisseursImportService")]
        public void AddOrUpdateFournisseurs_Returns_FournisseurAddedInResult()
        {
            //Initialize
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Save());
            FournisseurRepo = new Mock<IFournisseurRepository>();
            var agenceRepo = new Mock<IAgenceRepository>();
            var service = new FournisseursImportService(uow.Object, FournisseurRepo.Object, agenceRepo.Object);

            //Arrange
            var fournisseursFromFred = new List<FournisseurEnt>();
            var fournisseurAdded = new FournisseurEnt { Code = "002", GroupeId = 1 };
            var fournisseursFromSap = fournisseurAdded;
            var userId = 1;
            FournisseurRepo.Setup(r => r.AddFournisseurWithoutSaving(It.IsAny<FournisseurEnt>()))
                .Callback<FournisseurEnt>((f) => fournisseursFromFred.Add(f));

            //Act
            service.AddOrUpdateFournisseurs(fournisseursFromSap, userId);

            //Assert
            Assert.IsTrue(fournisseursFromFred.Contains(fournisseurAdded));
        }

        [TestMethod]
        [TestCategory("FournisseursImportService")]
        public void AddOrUpdateFournisseurs_Returns_FournisseurAddedInExpectedRedult()
        {
            //Initialize
            var uow = GetMocked<IUnitOfWork>();
            uow.Setup(u => u.Save());
            FournisseurRepo = GetMocked<IFournisseurRepository>();
            var agenceRepo = GetMocked<IAgenceRepository>();

            //Arrange
            var fournisseursFromFred = new List<FournisseurEnt>();
            var fournisseurAdded = new FournisseurBuilder().New();
            var fournisseursFromSap = fournisseurAdded;
            var userId = 1;
            FournisseurRepo.Setup(r => r.AddFournisseurWithoutSaving(It.IsAny<FournisseurEnt>()))
                .Callback<FournisseurEnt>((f) => fournisseursFromFred.Add(f));

            //Act
            Actual.AddOrUpdateFournisseurs(fournisseursFromSap, userId);

            //Assertion
            fournisseursFromFred.Should().Contain(fournisseurAdded);
        }
    }
}
