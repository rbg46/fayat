using AutoMapper;
using FluentAssertions;
using Fred.Business.OperationDiverse;
using Fred.Business.Unite;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.OperationDiverse.Builder;
using Fred.Common.Tests.Data.OperationDiverse.Fake;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.OperationDiverse;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Security;
using Fred.Web.Shared.Models.Enum;
using Fred.Web.Shared.Models.OperationDiverse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.Tests.OperationDiverse
{
    /// <summary>
    /// Test de classe de <see cref="OperationDiverseAbonnementManagerTest"/>
    /// </summary>
    [TestClass]
    public class OperationDiverseAbonnementManagerTest : BaseTu<OperationDiverseAbonnementManager>
    {
        private readonly OperationDiverseAbonnementModelBuilder abonnementBuilderModel = new OperationDiverseAbonnementModelBuilder();
        private readonly OperationDiverseBuilder oDtBuilderModel = new OperationDiverseBuilder();
        private Mock<FredDbContext> context;
        private Mock<IUniteManager> fakeUniteManager;
        private UniteEnt UniteEnt = new UniteEnt();
        private Mock<IDateTimeExtendManager> fakeDateTimeExtendManager;
        private Mock<IOperationDiverseRepository> operationDiverseRepositoryMock;
        private OperationDiverseRepository OperationDiverseRepositoryReal;

        [TestInitialize]
        public void TestInitialize()
        {
            UniteEnt = new UniteEnt()
            {
                UniteId = 1,
                Code = "FRT",
                Libelle = "Forfait"
            };
            fakeUniteManager = GetMocked<IUniteManager>();
            fakeUniteManager.Setup(m => m.FindById(1)).Returns(UniteEnt);

            fakeDateTimeExtendManager = GetMocked<IDateTimeExtendManager>();
            fakeDateTimeExtendManager.Setup(m => m.IsBusinessDay(It.IsAny<DateTime>())).Returns(true);

            operationDiverseRepositoryMock = GetMocked<IOperationDiverseRepository>();
            context = GetMocked<FredDbContext>();

            //Mock method
            operationDiverseRepositoryMock.Setup(m => m.Insert(It.IsAny<OperationDiverseEnt>())).Returns<OperationDiverseEnt>((OperationDiverseEnt result) => result);
            operationDiverseRepositoryMock.Setup(m => m.AddListOD(It.IsAny<IEnumerable<OperationDiverseEnt>>())).Returns<IEnumerable<OperationDiverseEnt>>((IEnumerable<OperationDiverseEnt> result) => result.ToList().AsReadOnly());
            operationDiverseRepositoryMock.Setup(m => m.UpdateListOD(It.IsAny<List<OperationDiverseEnt>>())).Returns<List<OperationDiverseEnt>>((List<OperationDiverseEnt> result) => result);
            operationDiverseRepositoryMock.Setup(m => m.SaveRange(It.IsAny<IEnumerable<OperationDiverseEnt>>()));
            operationDiverseRepositoryMock.Setup(m => m.SaveRange(It.IsAny<IEnumerable<OperationDiverseEnt>>()));
            operationDiverseRepositoryMock.Setup(m => m.UpdateOD(It.IsAny<OperationDiverseEnt>())).Returns<OperationDiverseEnt>((OperationDiverseEnt result) => result);

            // Use Real method with fake data
            operationDiverseRepositoryMock.Setup(m => m.GetAbonnementByODMere(It.IsAny<int>())).Returns<int>(c => OperationDiverseRepositoryReal.GetAbonnementByODMere(c));

            SubstituteConstructorArgument<IOperationDiverseRepository>(operationDiverseRepositoryMock.Object);

            IMapper fakeMapper = new OperationDiverseFake().FakeMapper;
            SubstituteConstructorArgument<IMapper>(fakeMapper);
        }

        /// <summary>
        /// Créé une fake database pour les ODs
        /// </summary>
        /// <param name="operationDiverseEntsList"></param>
        private void GenerateFakeDatabase(List<OperationDiverseEnt> operationDiverseEntsList)
        {
            context.Object.OperationDiverses = oDtBuilderModel.BuildFakeDbSet(operationDiverseEntsList);
            Mock<ISecurityManager> securityManager = GetMocked<ISecurityManager>();
            UnitOfWork uow = new UnitOfWork(context.Object, securityManager.Object);
            OperationDiverseRepositoryReal = new OperationDiverseRepository(null, uow, context.Object);
        }

        /// <summary>
        /// Récupère toutes les fréquences d'abonnement 
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetFrequenceAbonnement_GetAllValues()
        {
            Actual.GetFrequenceAbonnement().Should().HaveCount(6);
        }

        /// <summary>
        /// Ajoute une opération diverse avec la valeur null
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void AddAbonnement_WithIncorrectsInputs_ThrowException()
        {
            Invoking(() => Actual.Add(null)).Should().Throw<Exception>();
        }

        /// <summary>
        /// Test d'ajout d'un abonnement
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void AddAbonnement_WithCorrectsInputs_AbonnementExpected()
        {
            // Arrange
            EnumModel enumModel = new EnumModel()
            {
                Value = 1
            };

            DateTime date = new DateTime(2019, 10, 20);
            OperationDiverseAbonnementModel abonnement = abonnementBuilderModel
                 .EstUnAbonnement(true)
                 .DureeAbonnement(5)
                 .DateComptable(date)
                 .DatePremiereODAbonnement(date)
                 .DateProchaineODAbonnement(date)
                 .FrequenceAbonnementModel(enumModel)
                 .Build();

            //ACT
            IEnumerable<OperationDiverseEnt> actual = Actual.Add(abonnement);

            //ASSERT
            actual.Should()
                .HaveCount(5).And
                .OnlyHaveUniqueItems().And
                .Contain(x => x.EstUnAbonnement == true);

            actual.Where(x => x.OperationDiverseMereIdAbonnement == null)
                .Should().ContainSingle();

            actual.Where(x => x.OperationDiverseMereIdAbonnement != null)
                .Should().Contain(x => x.DateComptable > date).And.HaveCount(4);
        }

        /// <summary>
        /// Test d'ajout d'un abonnement
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void AddAbonnement_WithCorrectsInputs_AbonnementNegatifExpected()
        {
            // Arrange  
            EnumModel enumModel = new EnumModel()
            {
                Value = 1
            };

            DateTime date = new DateTime(2019, 10, 20);
            OperationDiverseAbonnementModel abonnement = abonnementBuilderModel
                 .EstUnAbonnement(true)
                 .DureeAbonnement(-5)
                 .DateComptable(date)
                 .DatePremiereODAbonnement(date)
                 .DateProchaineODAbonnement(date)
                 .FrequenceAbonnementModel(enumModel)
                 .Build();

            //ACT
            IEnumerable<OperationDiverseEnt> actual = Actual.Add(abonnement);

            //ASSERT
            actual.Should()
                .HaveCount(5).And
                .OnlyHaveUniqueItems().And
                .Contain(x => x.EstUnAbonnement == true);

            //ycourtel 14/01/2020 : a comparer avec le precedent assert (HaveCount 4 et dateComptable < date)
            actual.Where(x => x.OperationDiverseMereIdAbonnement != null)
                .Should().Contain(x => x.DateComptable <= date).And.HaveCount(5);
        }


        /// <summary>
        /// Suppression d'un abonnement avec l'opération diverse à null
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void DeleteAbonnement_WithIncorrectsInputs_ThrowException()
        {
            Invoking(() => Actual.Delete(null)).Should().Throw<Exception>();
        }

        /// <summary>
        /// Vérification que la méthode 'DeleteListOD' n'est jamais appelée
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void DeleteAbonnement_WithIncorrectsInputs_RepositoryNeverCall()
        {
            OperationDiverseAbonnementModel abonnement = abonnementBuilderModel
                .OperationDiverseId(1)
                .EstUnAbonnement(false)
                .Build();

            Actual.Delete(abonnement);

            operationDiverseRepositoryMock.Verify(m => m.DeleteListOD(It.IsAny<List<OperationDiverseEnt>>()), Times.Never);
        }

        /// <summary>
        /// Test de suppression de tout un abonnement
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void DeleteAbonnement_WithCorrectsInputs_DeleteAbonnementExpected()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>();
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 10, 22)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, 2).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(6).DateComptable(new DateTime(2019, 10, 21)).Build());

            GenerateFakeDatabase(operationDiverseEntsList);

            operationDiverseRepositoryMock.Setup(m => m.DeleteListOD(It.IsAny<List<OperationDiverseEnt>>()))
                .Callback<List<OperationDiverseEnt>>(b => b.Should().HaveCount(3));
            OperationDiverseAbonnementModel abonnement = abonnementBuilderModel
                .OperationDiverseId(1)
                .EstUnAbonnement(true)
                .Build();

            Actual.Delete(abonnement);

            operationDiverseRepositoryMock.Verify(m => m.DeleteListOD(It.IsAny<List<OperationDiverseEnt>>()), Times.Once);
        }

        /// <summary>
        /// Test de suppression d'une partie d'un abonnement
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void DeleteAbonnement_WithCorrectsInputs_DeleteAbonnemenChildtExpected()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>();
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 10, 22)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, 2).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(6).DateComptable(new DateTime(2019, 10, 21)).Build());

            GenerateFakeDatabase(operationDiverseEntsList);

            operationDiverseRepositoryMock.Setup(m => m.DeleteListOD(It.IsAny<List<OperationDiverseEnt>>()))
                .Callback<List<OperationDiverseEnt>>(b =>
               b.Should().HaveCount(2));
            OperationDiverseAbonnementModel abonnement = abonnementBuilderModel
                .OperationDiverseId(2)
                .EstUnAbonnement(true)
                .OperationDiverseMereIdAbonnement(1)
                .Build();

            Actual.Delete(abonnement);

            operationDiverseRepositoryMock.Verify(m => m.DeleteListOD(It.IsAny<List<OperationDiverseEnt>>()), Times.Once);
        }

        /// <summary>
        /// Test de suppression d'une partie d'un abonnement
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void DeleteAbonnement_WithCorrectsInputs_GetODAbonnementExpected()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>();
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 10, 22)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, 2).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(6).DateComptable(new DateTime(2019, 10, 21)).Build());

            GenerateFakeDatabase(operationDiverseEntsList);

            //ACT
            IEnumerable<OperationDiverseEnt> actual = Actual.GetODAbonnement(1);

            //ASSERT
            actual.Should()
                .HaveCount(3).And
                .OnlyHaveUniqueItems().And
                .Contain(x => x.EstUnAbonnement == true)
                ;

            actual.Where(x => x.OperationDiverseMereIdAbonnement == null)
                .Should().ContainSingle();
        }

        /// <summary>
        /// Test de suppression d'une partie d'un abonnement
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void DeleteAbonnement_WithIncorrectsInputs_NoDataExpected()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>();
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 10, 22)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, 2).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(6).DateComptable(new DateTime(2019, 10, 21)).Build());

            GenerateFakeDatabase(operationDiverseEntsList);

            //ACT
            Actual.GetODAbonnement(0).Should().HaveCount(0);
        }

        /// <summary>
        /// Test De récupération de la derniere date d'un abonnement
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithIncorrectsInputs_CurrentDateTime()
        {
            DateTime now = DateTime.Now;
            Actual.GetLastDayOfODAbonnement(now, 0, 0).Should().Be(now);
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement journalier
        /// Attention: ce test ne détecte pas les jours travaillés, mock en place qui renvoie toujours TRUE
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_BusinessDaySubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 1, 5).Should().Be(dateParam.AddDays(4));
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement journalier négatif
        /// Attention: ce test ne détecte pas les jours travaillés, mock en place qui renvoie toujours TRUE
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_NegativeBusinessDaySubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 1, -5).Should().Be(dateParam.AddDays(-4));
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement hebdomadaire
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_WeekSubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 2, 5).Should().Be(dateParam.AddDays(4 * 7));
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement hebdomadaire négatif
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_NagativeWeekSubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 2, -5).Should().Be(dateParam.AddDays(-4 * 7));
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement mensuel
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_MonthSubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 3, 5).Should().Be(dateParam.AddMonths(4));
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement mensuel négatif
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_NegativeMonthSubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 3, -5).Should().Be(dateParam.AddMonths(-4));
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement trimestriel
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_QuarterSubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 4, 5).Should().Be(dateParam.AddMonths(4 * 3));
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement trimestriel négatif
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_NegativeQuarterSubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 4, -5).Should().Be(dateParam.AddMonths(-4 * 3));
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement annuel
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_YearSubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 5, 5).Should().Be(dateParam.AddYears(4));
        }

        /// <summary>
        /// Test de récupération de la derniere date d'un abonnement pour un abonnement annuel négatif
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_NegativeYearSubcription()
        {
            DateTime dateParam = new DateTime(2019, 10, 20);
            Actual.GetLastDayOfODAbonnement(dateParam, 5, -5).Should().Be(dateParam.AddYears(-4));
        }

        /// <summary>
        /// Mise a jour d'un abonnement avec une OD à null
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithIncorrectsInputs_ThrowException()
        {
            Invoking(() => Actual.Update(null)).Should().Throw<Exception>();
        }

        /// <summary>
        /// Mets a jour partiellement un abonnement plus grand que celui d'origine
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_UpdateWithAddOperationDiverse()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>();
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 10, 22)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, 2).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(6).DateComptable(new DateTime(2019, 10, 21)).Build());

            GenerateFakeDatabase(operationDiverseEntsList);

            OperationDiverseEnt operationDiverseEntBaseUpdate = operationDiverseEntsList.Where(x => x.OperationDiverseId == 2).First();
            EnumModel enumModel = new EnumModel()
            {
                Value = 1
            };
            OperationDiverseAbonnementModel abonnement = abonnementBuilderModel
              .OperationDiverseId(operationDiverseEntBaseUpdate.OperationDiverseId)
              .EstUnAbonnement(true)
              .DureeAbonnement(4)
              .DateComptable(operationDiverseEntBaseUpdate.DateComptable)
              .DateProchaineODAbonnement(new DateTime(2019, 10, 5))
              .FrequenceAbonnementModel(enumModel)
              .OperationDiverseMereIdAbonnement(operationDiverseEntBaseUpdate.OperationDiverseMereIdAbonnement)
              .Build();

            operationDiverseRepositoryMock.Setup(m => m.DeleteListOD(It.IsAny<List<OperationDiverseEnt>>()))
              .Callback<List<OperationDiverseEnt>>(b => b.Should().HaveCount(0));

            IEnumerable<OperationDiverseEnt> actual = Actual.Update(abonnement);
            actual.Should().HaveCount(4);
            actual.Where(x => x.OperationDiverseId == 2).Should().HaveCount(1);
            actual.Where(x => x.OperationDiverseMereIdAbonnement == 2).Should().HaveCount(3);
        }

        /// <summary>
        /// Mets a jour partiellement un abonnement plus petit que celui d'origine
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseAbonnementManager")]
        public void GetLastDayOfODAbonnement_WithCorrectsInputs_UpdateWithDeleteOperationDiverse()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>();
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 10, 22)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 10, 21)).EstUnAbonnement(true, null).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 10, 20)).EstUnAbonnement(true, 2).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(6).DateComptable(new DateTime(2019, 10, 23)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(7).DateComptable(new DateTime(2019, 10, 24)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(8).DateComptable(new DateTime(2019, 10, 25)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(9).DateComptable(new DateTime(2019, 10, 26)).EstUnAbonnement(true, 1).Build());
            operationDiverseEntsList.Add(oDtBuilderModel.OperationDiverseId(10).DateComptable(new DateTime(2019, 10, 27)).Build());

            GenerateFakeDatabase(operationDiverseEntsList);

            OperationDiverseEnt operationDiverseEntBaseUpdate = operationDiverseEntsList.Where(x => x.OperationDiverseId == 2).First();
            EnumModel enumModel = new EnumModel()
            {
                Value = 1
            };

            DateTime date = new DateTime(2019, 10, 5);
            OperationDiverseAbonnementModel abonnement = abonnementBuilderModel
              .OperationDiverseId(operationDiverseEntBaseUpdate.OperationDiverseId)
              .EstUnAbonnement(true)
              .DureeAbonnement(4)
              .DateComptable(operationDiverseEntBaseUpdate.DateComptable)
              .DateProchaineODAbonnement(date)
              .FrequenceAbonnementModel(enumModel)
              .OperationDiverseMereIdAbonnement(operationDiverseEntBaseUpdate.OperationDiverseMereIdAbonnement)
              .Build();

            operationDiverseRepositoryMock.Setup(m => m.DeleteListOD(It.IsAny<List<OperationDiverseEnt>>()))
              .Callback<List<OperationDiverseEnt>>(b => b.Should().HaveCount(2));

            IEnumerable<OperationDiverseEnt> actual = Actual.Update(abonnement);
            actual.Should().HaveCount(4);
            actual.Where(x => x.OperationDiverseId == 2).Should().HaveCount(1).And.Contain(x => x.DateComptable == date);
            actual.Where(x => x.OperationDiverseMereIdAbonnement == 2).Should().HaveCount(3).And.Contain(x => x.DateComptable > date);
        }
    }
}
