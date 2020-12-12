using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Business.Referential.Tache;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Attributes;
using Fred.Common.Tests.Data.Referential.Tache.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Common;
using Fred.DataAccess.Referential.Tache;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Referential.Tache
{
    /// <summary>
    /// Classe de test de <see cref="TacheManager"/>
    /// </summary>
    [TestClass]
    public class TacheManagerTest : BaseTu<TacheManager>
    {
        private readonly TacheBuilder builder = new TacheBuilder();
        private ITacheSearchHelper taskSearchHelper;
        private Mock<FredDbContext> Context;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            var builder = new TacheBuilder();
            Context = GetMocked<FredDbContext>();
            Context.Object.Taches = builder.BuildFakeDbSet(builder.Niveau1(), builder.Niveau2(), builder.Niveau3());
            taskSearchHelper = new TacheSearchHelper();

            var methode = GetType().GetMethod(TestContext.TestName);
            bool realRepoMode = methode.GetCustomAttributes(typeof(WithRealRepositoryAttribute), false).Any();

            if (realRepoMode)
            {
                var repository = new TacheRepository(Context.Object, GetMocked<ILogManager>().Object, taskSearchHelper);

                SubstituteConstructorArgument<ITacheRepository>(repository);
                SubstituteConstructorArgument<ITacheSearchHelper>(taskSearchHelper);
            }
            else
            {
                var tacheRepo = GetMocked<ITacheRepository>();
                tacheRepo.Setup(r => r.Insert(It.IsAny<TacheEnt>())).Returns<TacheEnt>(t =>
                {
                    Context.Object.Taches.Add(t);
                    return t;
                });

                var listTecheSource = builder.BuildFakeDbSet(builder.Code("003").Niveau(1).Build(), builder.Code("005").Niveau(1).Build());
                var listTecheDes = builder.BuildFakeDbSet(builder.Code("002").Niveau(1).Build(), builder.Code("001").Niveau(1).Build());
                tacheRepo.Setup(t => t.GetTacheListByCiId(It.IsAny<int>(), It.IsAny<bool>())).Returns(listTecheDes);
                tacheRepo.Setup(x => x.GetAllT1ByCiId(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<bool>())).Returns(listTecheSource);
                tacheRepo.Setup(t => t.GetTacheLevel1ByCiId(It.IsAny<int>(), It.IsAny<bool>())).Returns(listTecheDes);
                tacheRepo.Setup(t => t.GetTAssocitedToT1(It.IsAny<List<int>>()))
                    .Returns(builder.BuildFakeDbSet(builder.Code("003").Niveau(1).Build(), builder.Code("004").Niveau(1).Build(), builder.Niveau1()));

                var unitOfWork = GetMocked<IUnitOfWork>();

                SubstituteConstructorArgument<ITacheRepository>(tacheRepo.Object);
                SubstituteConstructorArgument<IUnitOfWork>(unitOfWork.Object);
            }
        }

        private void SetDbSet(FakeDbSet<TacheEnt> fakeList)
        {
            Context.Object.Taches = fakeList;
        }

        [TestMethod]
        [TestCategory("TacheManager")]
        [TestCategory("OperationDiverseExcelService")]
        [WithRealRepository]
        public void GetActiveTacheListByCiIdAndNiveau_WithNiveau3_ReturnOnlyContain_CiAndNiveauExpected()
        {
            Actual.GetActiveTacheListByCiIdAndNiveau(1, 3).Should().OnlyContain(t => t.Niveau.Equals(3)).And.HaveCount(1);
        }

        [TestMethod]
        [TestCategory("TacheManager")]
        public void CopyTachePartially_DifferentCI()
        {
            //Arrange
            List<int> listIds = new List<int>();
            listIds.Add(1);

            //Act
            Actual.CopyTachePartially(1, It.IsAny<int>(), listIds);

            //Assert
            Context.Object.Taches.Should().Contain(t => t.Code == "004");
        }

        [TestMethod]
        [TestCategory("TacheManager")]
        public void CopyPlanTache_PastesTO_DestinationList()
        {
            //Arrange
            SetDbSet(builder.BuildFakeDbSet(builder.Code("00").Niveau(1).Build()));

            List<int> listIds = new List<int>();
            listIds.Add(1);

            //Act
            Actual.CopyPlanTache(1, 2);

            //Assert
            Context.Object.Taches.Should().Contain(t => t.Code == "003");
        }

        [TestMethod]
        [TestCategory("TacheManager")]
        public void CopyPlanTache_SameCiId()
        {
            //Configuration
            SetDbSet(builder.BuildFakeDbSet(builder.Code("00").Niveau(1).Build()));

            //Arrange
            List<int> listIds = new List<int>();
            listIds.Add(1);

            //Act
            Actual.CopyPlanTache(1, 1);

            //Assert
            Context.Object.Taches.Should().HaveCount(1);
        }

        [TestMethod]
        [TestCategory("TacheManager")]
        public void CopyTachePartially_SameCI()
        {
            //Configuration
            SetDbSet(builder.BuildFakeDbSet(builder.Niveau1(), builder.Niveau2(), builder.Niveau3()));

            //Arrange
            List<int> listIds = new List<int>();
            listIds.Add(1);

            //Act
            Actual.CopyTachePartially(1, 1, listIds);

            //Assert
            Context.Object.Taches.Should().Contain(t => t.Code == "X-003");
        }

        [TestMethod]
        [TestCategory("TacheManager")]
        [WithRealRepository]
        public void SearchLight_WithTechnicalTaskFalse_ShouldNotReturnTacheEcart()
        {
            //Arrange
            int[] techTask = taskSearchHelper.GetTechnicalTasks();
            int countNonTechnicalTaskList = Context.Object.Taches.Count(t => !techTask.Contains(t.TacheType));

            //Act
            List<TacheEnt> returnedList = Actual.SearchLight("", 1, 100, null, activeOnly: true, isTechnicalTask: false).ToList();

            //Assert
            returnedList.Should().HaveCount(countNonTechnicalTaskList);
        }

        [TestMethod]
        [TestCategory("TacheManager")]
        [WithRealRepository]
        public void SearchLight_WithTechnicalTaskTrue_ShouldReturnAllTacheEcart()
        {
            //Arrange
            int countTaskList = Context.Object.Taches.Count();

            //Act
            List<TacheEnt> returnedList = Actual.SearchLight("", 1, 100, null, activeOnly: true, isTechnicalTask: true).ToList();

            //Assert
            returnedList.Should().HaveCount(countTaskList);
        }
    }
}
