using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Reception.Services;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Reception.Services
{
    [TestClass]
    public class VisableReceptionProviderServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IDepenseRepository> mockDepenseRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockDepenseRepository = this.mockRepository.Create<IDepenseRepository>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private VisableReceptionProviderService CreateService()
        {
            return new VisableReceptionProviderService(
                this.mockDepenseRepository.Object);
        }

        private DepenseAchatEnt CreateVisableReception()
        {
            return new DepenseAchatEnt
            {
                DepenseId = 1,
                DateVisaReception = null,
                IsReceptionInterimaire = false
            };
        }

        private DepenseAchatEnt CreateNotVisableReception()
        {
            return new DepenseAchatEnt
            {
                DepenseId = 1,
                DateVisaReception = DateTime.UtcNow,
                IsReceptionInterimaire = false
            };
        }

        private List<DepenseAchatEnt> CreateListReceptions(params DepenseAchatEnt[] receptions)
        {
            var result = new List<DepenseAchatEnt>();
            foreach (var reception in receptions)
            {
                result.Add(reception);
            }
            return result;
        }

        [TestMethod]
        public void GetReceptionsNotVisees_une_reception_visable_doit_etre_retournee()
        {
            // Arrange
            var service = this.CreateService();

            // une reception est visable dans la base
            var receptionVisables = CreateListReceptions(CreateVisableReception());

            this.mockDepenseRepository.Setup(x => x.Get(It.IsAny<List<Expression<Func<DepenseAchatEnt, bool>>>>(),
                                                        It.IsAny<Func<IQueryable<DepenseAchatEnt>, IOrderedQueryable<DepenseAchatEnt>>>(),
                                                        It.IsAny<List<Expression<Func<DepenseAchatEnt, object>>>>(),
                                                        It.IsAny<int?>(),
                                                        It.IsAny<int?>(),
                                                        It.IsAny<bool>()))
                                        .Returns(receptionVisables);

            var receptionsWeAreLookingForIfTheyAreVisables = CreateListReceptions(CreateVisableReception());

            var receptionsWeAreLookingForIfTheyAreVisablesIds = receptionsWeAreLookingForIfTheyAreVisables.Select(r => r.DepenseId).ToList();

            // Act
            var response = service.GetReceptionsVisables(receptionsWeAreLookingForIfTheyAreVisablesIds);

            // Assert
            Assert.AreEqual(response.ReceptionsVisables.Count, 1, "Une reception doit etre retournée car elle est visable.");

            Assert.AreEqual(response.ReceptionsNotVisables.Count, 0, "Une reception doit etre retournée car elle est visable.");

            Assert.AreEqual(response.ReceptionsVisables.First().DepenseId, receptionsWeAreLookingForIfTheyAreVisables.First().DepenseId, "Une reception doit etre retournée car elle est visable.");
        }

        [TestMethod]
        public void GetReceptionsNotVisees_une_reception_non_visable_doit_pas_etre_retournee()
        {
            // Arrange
            var service = this.CreateService();

            //aucune reception n'est visable dans la base
            var receptionVisables = CreateListReceptions();

            this.mockDepenseRepository.Setup(x => x.Get(It.IsAny<List<Expression<Func<DepenseAchatEnt, bool>>>>(),
                                                        It.IsAny<Func<IQueryable<DepenseAchatEnt>, IOrderedQueryable<DepenseAchatEnt>>>(),
                                                        It.IsAny<List<Expression<Func<DepenseAchatEnt, object>>>>(),
                                                        It.IsAny<int?>(),
                                                        It.IsAny<int?>(),
                                                        It.IsAny<bool>()))
                                        .Returns(receptionVisables);

            var receptionsWeAreLookingForIfTheyAreVisables = CreateListReceptions(CreateNotVisableReception());

            var receptionsWeAreLookingForIfTheyAreVisablesIds = receptionsWeAreLookingForIfTheyAreVisables.Select(r => r.DepenseId).ToList();


            // Act
            var response = service.GetReceptionsVisables(receptionsWeAreLookingForIfTheyAreVisablesIds);

            // Assert
            Assert.AreEqual(response.ReceptionsVisables.Count, 0, "Il n'y a aucune reception visable car la bdd ne retourne aucune reception visable.");

            Assert.AreEqual(response.ReceptionsNotVisables.Count, 1, "Il n'y a aucune reception visable car la bdd ne retourne aucune reception visable..");

            Assert.AreEqual(response.ReceptionsNotVisables.First(), receptionsWeAreLookingForIfTheyAreVisables.First().DepenseId, "Il n'y a aucune reception visable car la bdd ne retourne aucune reception visable.");
        }


    }
}
