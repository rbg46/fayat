using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Fred.Business.RapportPrime;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel;
using Fred.Entities.RapportPrime;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.RapportPrime.Get;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.RapportPrime
{
    [TestClass]
    public class RapportPrimeTests
    {
        private RapportPrimeManager rapportPrimeManager;
        private Mock<IRapportPrimeRepository> rapportPrimeRepositoryMock;
        private Mock<IUtilisateurManager> utilisateurManagerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            IUnitOfWork uow = new Mock<IUnitOfWork>().Object;
            IRapportPrimeLigneManager rapportPrimeLigneManager = new Mock<IRapportPrimeLigneManager>().Object;
            utilisateurManagerMock = new Mock<IUtilisateurManager>();
            rapportPrimeRepositoryMock = new Mock<IRapportPrimeRepository>();

            rapportPrimeManager = new RapportPrimeManager(uow,
                rapportPrimeRepositoryMock.Object, utilisateurManagerMock.Object, rapportPrimeLigneManager);
        }

        #region GetAsync

        [TestMethod]
        public async Task GetAsync_ShouldReturnRapport()
        {
            List<int> userCis = new List<int>();
            RapportPrimeGetModel rapportPrimeGetExpected = new RapportPrimeGetModel();
            DateTime date = DateTime.Now;

            utilisateurManagerMock.Setup(u => u.GetAllCIbyUserAsync(It.IsAny<int>(), false, null)).ReturnsAsync(userCis);
            rapportPrimeRepositoryMock.Setup(r => r.GetRapportPrimeByDateAsync(date, userCis)).ReturnsAsync(rapportPrimeGetExpected);

            RapportPrimeGetModel rapportPrimeGetReturned = await rapportPrimeManager.GetAsync(date);

            rapportPrimeGetReturned.Should().Be(rapportPrimeGetExpected);
        }

        #endregion

        #region AddAsync

        [TestMethod]
        public void AddAsync_RapportPrimeAlreadyExist_ShouldThrowBusinessException()
        {
            rapportPrimeRepositoryMock.Setup(r => r.RapportPrimeExistsAsync(It.IsAny<DateTime>())).ReturnsAsync(true);

            rapportPrimeManager
                .Awaiting(y => y.AddAsync())
                .Should().Throw<FredBusinessException>().WithMessage(FeatureRapportPrime.RapportPrime_Error_AddNewRapportPrime_RapportPrimeAlreadyExist);
        }

        [TestMethod]
        public void AddAsync_RapportPrimeNotExist_ShouldNotThrowBusinessException()
        {
            rapportPrimeRepositoryMock.Setup(r => r.RapportPrimeExistsAsync(It.IsAny<DateTime>())).ReturnsAsync(false);

            rapportPrimeManager
                .Awaiting(y => y.AddAsync())
                .Should().NotThrow<FredBusinessException>();
        }

        [TestMethod]
        public async Task AddAsync_RapportPrimeIsInserted_ReturnedValueShouldBeEquivalentToInsertedValue()
        {
            var date = DateTime.UtcNow;
            var user = new UtilisateurEnt
            {
                Personnel = new PersonnelEnt
                {
                    SocieteId = 1
                }
            };
            var rapportPrimeExpected = new RapportPrimeEnt
            {
                AuteurCreationId = user.UtilisateurId,
                DateCreation = date,
                DateRapportPrime = date,
                SocieteId = user.Personnel.SocieteId.Value
            };

            rapportPrimeRepositoryMock.Setup(r => r.RapportPrimeExistsAsync(date)).ReturnsAsync(false);
            utilisateurManagerMock.Setup(u => u.GetContextUtilisateurAsync()).ReturnsAsync(user);

            RapportPrimeEnt rapportPrimeReturned = await rapportPrimeManager.AddAsync();

            rapportPrimeReturned.TruncateDates();
            rapportPrimeExpected.TruncateDates();

            rapportPrimeReturned.Should().BeEquivalentTo(rapportPrimeExpected);
        }
        #endregion
    }
}
