using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Fred.Business.DatesClotureComptable;
using Fred.Business.DepenseGlobale;
using Fred.Business.EcritureComptable;
using Fred.Business.FeatureFlipping;
using Fred.Business.OperationDiverse;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.OperationDiverse.Builder;
using Fred.Common.Tests.Data.OperationDiverse.Fake;
using Fred.Common.Tests.Data.Unite.Builder;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.EcritureComptable;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.OperationDiverse;
using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Entities.RepartitionEcart;
using Fred.EntityFramework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Security;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.OperationDiverse
{
    [Ignore]
    [TestClass]
    public class OperationDiverseManagerTest : BaseTu<OperationDiverseManager>
    {
        private readonly OperationDiverseBuilder oDtBuilderModel = new OperationDiverseBuilder();
        private readonly UniteBuilder uniteBuilder = new UniteBuilder();
        private readonly UtilisateurBuilder utilisateurBuilder = new UtilisateurBuilder();
        private readonly FamilleOperationDiverseBuilder familleOperationDiverseBuilder = new FamilleOperationDiverseBuilder();


        private Mock<FredDbContext> context;
        private Mock<IOperationDiverseRepository> operationDiverseRepositoryMock;
        private OperationDiverseRepository OperationDiverseRepositoryReal;
        private Mock<IEcritureComptableRepository> ecritureComptableRepositoryMock;
        private EcritureComptableRepository EcritureComptableRepositoryReal;
        private Mock<IUnitOfWork> uowMock;

        private Mock<IUniteManager> fakeUniteManager;
        private Mock<IUtilisateurManager> fakeUtilisateurManager;
        private Mock<IDatesClotureComptableManager> fakeDateClotureComptableManager;
        private Mock<IFeatureFlippingManager> fakeFeatureFlippingManager;
        private Mock<IEcritureComptableManager> fakeEcritureComptableManager;


        [TestInitialize]
        public void Initialize()
        {
            operationDiverseRepositoryMock = GetMocked<IOperationDiverseRepository>();
            ecritureComptableRepositoryMock = GetMocked<IEcritureComptableRepository>();

            context = GetMocked<FredDbContext>();
            uowMock = GetMocked<IUnitOfWork>();
            fakeUniteManager = GetMocked<IUniteManager>();
            fakeUtilisateurManager = GetMocked<IUtilisateurManager>();
            fakeDateClotureComptableManager = GetMocked<IDatesClotureComptableManager>();
            fakeFeatureFlippingManager = GetMocked<IFeatureFlippingManager>();
            fakeEcritureComptableManager = GetMocked<IEcritureComptableManager>();
            //Mock method

            operationDiverseRepositoryMock.Setup(m => m.AddListOD(It.IsAny<IEnumerable<OperationDiverseEnt>>())).Returns<IEnumerable<OperationDiverseEnt>>((IEnumerable<OperationDiverseEnt> result) => result.ToList().AsReadOnly());
            operationDiverseRepositoryMock.Setup(m => m.UpdateListOD(It.IsAny<List<OperationDiverseEnt>>())).Returns<List<OperationDiverseEnt>>((List<OperationDiverseEnt> result) => result);
            operationDiverseRepositoryMock.Setup(m => m.SaveRange(It.IsAny<IEnumerable<OperationDiverseEnt>>()));
            operationDiverseRepositoryMock.Setup(m => m.SaveRange(It.IsAny<IEnumerable<OperationDiverseEnt>>()));

            fakeUniteManager.Setup(m => m.FindById(It.IsAny<int>())).Returns(uniteBuilder.Code("OD").UniteId(1).Libelle("Test OD").Build());
            fakeUtilisateurManager.Setup(m => m.GetContextUtilisateurId()).Returns(utilisateurBuilder.Prototype().UtilisateurId);
            fakeFeatureFlippingManager.Setup(m => m.IsActivated(Framework.FeatureFlipping.EnumFeatureFlipping.ActivationClotureOperationDiverses)).Returns(true);

            // Use Real method with fake data
            operationDiverseRepositoryMock.Setup(m => m.GetAllByCiIdAndDateComptableAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns<int, DateTime, DateTime>((i, db, df) => (OperationDiverseRepositoryReal.GetAllByCiIdAndDateComptableAsync(i, db, df)));
            //operationDiverseRepositoryMock.Setup(m => m.GetAllByCiIdAndDateComptableAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<bool>())).Returns<int, DateTime, bool>();
            //operationDiverseRepositoryMock.Setup(m => m.GetAllByCiIdAndDateComptableAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<bool>())).ReturnsAsync<Mock< IOperationDiverseRepository>,(NewMethod(1, new DateTime(1900, 1, 1), false));

            ecritureComptableRepositoryMock.Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<MonthLimits>())).Returns<int, MonthLimits>((i, d) => EcritureComptableRepositoryReal.GetAsync(i, d));


            operationDiverseRepositoryMock.Setup(m => m.GetOperationDiverseListAsync(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int?>())).Returns<int, int?, DateTime?, DateTime?, int?>((ci, r, db, df, d) => OperationDiverseRepositoryReal.GetOperationDiverseListAsync(ci, r, db, df, d));


            operationDiverseRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>((i) => OperationDiverseRepositoryReal.GetById(i));

            operationDiverseRepositoryMock.Setup(m => m.Insert(It.IsAny<OperationDiverseEnt>())).Returns<OperationDiverseEnt>((od) => od);
            operationDiverseRepositoryMock.Setup(m => m.UpdateOD(It.IsAny<OperationDiverseEnt>())).Returns<OperationDiverseEnt>((od) => od);
            operationDiverseRepositoryMock.Setup(m => m.UpdateListOD(It.IsAny<List<OperationDiverseEnt>>())).Returns<List<OperationDiverseEnt>>((ods) => ods);


            //operationDiverseRepositoryMock.Setup(m => m.Delete(It.IsAny<OperationDiverseEnt>(), It.IsAny<bool>())).Callback<OperationDiverseEnt>(c => c.OperationDiverseId = 0);

            operationDiverseRepositoryMock.Setup(m => m.GetODsAsync(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<bool>())).Returns<List<int>, DateTime, bool>((i, d, inc) => OperationDiverseRepositoryReal.GetODsAsync(i, d, inc));
            operationDiverseRepositoryMock.Setup(m => m.GetODsAsync(It.IsAny<List<int>>(), It.IsAny<List<DateTime?>>(), It.IsAny<List<DateTime?>>())).Returns<List<int>, List<DateTime?>, List<DateTime?>>((ids, dbs, dfs) => OperationDiverseRepositoryReal.GetODsAsync(ids, dbs, dfs));
            operationDiverseRepositoryMock.Setup(m => m.GetODsAsync(It.IsAny<List<int>>())).Returns<List<int>>((ecritureIds) => OperationDiverseRepositoryReal.GetODsAsync(ecritureIds));
            operationDiverseRepositoryMock.Setup(m => m.GetAsync(It.IsAny<int>())).Returns<int>(i => OperationDiverseRepositoryReal.GetAsync(i));
            operationDiverseRepositoryMock.Setup(m => m.GetByGroupRemplacementIdAsync(It.IsAny<int>())).Returns<int>(i => OperationDiverseRepositoryReal.GetByGroupRemplacementIdAsync(i));
            operationDiverseRepositoryMock.Setup(m => m.GetByCommandeIdsAsync(It.IsAny<List<int>>())).Returns<List<int>>(i => OperationDiverseRepositoryReal.GetByCommandeIdsAsync(i));

            IMapper fakeMapper = new OperationDiverseFake().FakeMapper;
            SubstituteConstructorArgument<IMapper>(fakeMapper);

        }

        /// <summary>
        /// Créé une fake database pour les ODs
        /// </summary>
        /// <param name="operationDiverseEntsList"></param>
        private void GenerateFakeDatabase(List<OperationDiverseEnt> operationDiverseEntsList)
        {
            Mock<ISecurityManager> securityManager = GetMocked<ISecurityManager>();
            UnitOfWork uow = new UnitOfWork(context.Object, securityManager.Object);
            context.Object.OperationDiverses = oDtBuilderModel.BuildFakeDbSet(operationDiverseEntsList);
            OperationDiverseRepositoryReal = new OperationDiverseRepository(null, uow, context.Object);
        }

        private void GenerateFakeDatabase()
        {
            Mock<ISecurityManager> securityManager = GetMocked<ISecurityManager>();
            UnitOfWork uow = new UnitOfWork(context.Object, securityManager.Object);
            OperationDiverseRepositoryReal = new OperationDiverseRepository(null, uow, context.Object);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void GetOperationDiverseByIdWithExist()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            OperationDiverseEnt actual = Actual.GetById(1);
            actual.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void GetOperationDiverseByIdWithoutExist()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            OperationDiverseEnt actual = Actual.GetById(99);
            actual.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetOperationDiverseListByCommandeIdsAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(new List<int> { 10 }).ConfigureAwait(false);
            actual.Should().HaveCount(2);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByGroupRemplacementIdAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).GroupeRemplacementTacheId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).GroupeRemplacementTacheId(null).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            OperationDiverseEnt actual = await Actual.GetByGroupRemplacementIdAsync(1).ConfigureAwait(false);
            actual.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiIdWithValideCiIdWithPuHtEqualsZeroAndQuantiteEqualsZeroAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1).ConfigureAwait(false);
            actual.Should().HaveCount(0);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiIdWithValideCiIdWithPuHtNotEqualsZeroAndQuantiteEqualsZeroAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).PUHT(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).PUHT(10).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1).ConfigureAwait(false);
            actual.Should().HaveCount(0);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiIdWithValideCiIdWithPuHtEqualsZeroAndQuantiteNotEqualsZeroAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).PUHT(0).Quantite(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).PUHT(0).Quantite(10).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1).ConfigureAwait(false);
            actual.Should().HaveCount(0);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiIdWithValideCiIdWithPuHtNotEqualsZeroAndQuantiteNotEqualsZeroAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).PUHT(10).Quantite(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).PUHT(10).Quantite(10).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1).ConfigureAwait(false);
            actual.Should().HaveCount(1);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiidWithInvalideCiIdAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(99).ConfigureAwait(false);
            actual.Should().HaveCount(0);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiAndDateComptableAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetAllByCiIdAndDateComptableAsync(1, new DateTime(2019, 12, 10), false).ConfigureAwait(false);
            actual.Should().HaveCount(1);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiAndDateComptableWithListOfCisAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            IEnumerable<OperationDiverseEnt> actual = await Actual.GetAllByCiIdAndDateComptableAsync(new List<int> { 1, 2 }, new DateTime(2019, 12, 10), false).ConfigureAwait(false);
            actual.Should().HaveCount(2);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiAndDateComptableWithOneDateAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2018, 12, 10)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);

            IEnumerable<OperationDiverseEnt> actual = await Actual.GetAllByCiIdAndDateComptableAsync(1, new DateTime(2019, 12, 10), false).ConfigureAwait(false);
            actual.Should().HaveCount(1);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiAndDateComptableWithTwoDateInvertedAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2018, 12, 10)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);

            IEnumerable<OperationDiverseEnt> actual = await Actual.GetAllByCiIdAndDateComptableAsync(1, new DateTime(2019, 12, 10), new DateTime(2018, 12, 10)).ConfigureAwait(false);
            actual.Should().HaveCount(0);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetByCiAndDateComptableWithTwoDateAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2018, 12, 10)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);

            IEnumerable<OperationDiverseEnt> actual = await Actual.GetAllByCiIdAndDateComptableAsync(1, new DateTime(2018, 12, 10), new DateTime(2019, 12, 1)).ConfigureAwait(false);
            actual.Should().HaveCount(2);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void AddOperationDiverseWithMinimalInformation()
        {
            GenerateFakeDatabase();

            OperationDiverseEnt od = oDtBuilderModel.DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build();
            OperationDiverseEnt updatedOd = Actual.AddOperationDiverse(od);
            updatedOd.DateCreation.Should().BeSameDateAs(DateTime.UtcNow);
            updatedOd.UniteId.Should().Be(1);
            updatedOd.AuteurCreationId.Should().Be(1);
            updatedOd.DeviseId.Should().Be(48);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void AddOperationDiverseWithAllInformation()
        {
            GenerateFakeDatabase();

            OperationDiverseEnt od = oDtBuilderModel.DateComptable(new DateTime(2019, 12, 10)).CiId(1).UniteId(99).AuteurCreationId(99).DeviseId(99).Build();
            OperationDiverseEnt updatedOd = Actual.AddOperationDiverse(od);
            updatedOd.DateCreation.Should().BeSameDateAs(DateTime.UtcNow);
            updatedOd.UniteId.Should().Be(99);
            updatedOd.AuteurCreationId.Should().Be(1);
            updatedOd.DeviseId.Should().Be(48);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void UpdateList()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);

            List<OperationDiverseEnt> updatedList = Actual.Update(operationDiverseEntsList);
            foreach (OperationDiverseEnt item in updatedList)
            {
                item.DateCreation.Date.Should().Be(DateTime.UtcNow.Date);
                item.AuteurCreationId.Should().Be(1);
                item.DeviseId.Should().Be(48);
                item.UniteId.Should().Be(1);
            }
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task UpdateODWithKnowIdAndDateCreationUnknowAsync()
        {
            GenerateFakeDatabase();

            OperationDiverseEnt od = oDtBuilderModel.OperationDiverseId(99).UniteId(1).AuteurCreationId(1).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build();
            OperationDiverseEnt updatedOd = await Actual.UpdateAsync(od).ConfigureAwait(false);
            updatedOd.DateCreation.Should().BeSameDateAs(DateTime.UtcNow);
            updatedOd.UniteId.Should().Be(1);
            updatedOd.AuteurCreationId.Should().Be(1);
            updatedOd.DeviseId.Should().Be(48);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task UpdateODWithKnowIdAndDateCreationKnowAsync()
        {
            GenerateFakeDatabase();

            OperationDiverseEnt od = oDtBuilderModel.OperationDiverseId(99).UniteId(1).AuteurCreationId(1).CommandeId(10).DateCreation(new DateTime(2019, 12, 31)).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build();
            OperationDiverseEnt updatedOd = await Actual.UpdateAsync(od).ConfigureAwait(false);
            updatedOd.DateCreation.Should().BeSameDateAs(new DateTime(2019, 12, 31));
            updatedOd.UniteId.Should().Be(1);
            updatedOd.AuteurCreationId.Should().Be(1);
            updatedOd.DeviseId.Should().Be(48);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task UpdateODWithUnknowIdAsync()
        {
            GenerateFakeDatabase();

            OperationDiverseEnt od = oDtBuilderModel.OperationDiverseId(99).UniteId(0).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build();
            OperationDiverseEnt updatedOd = await Actual.UpdateAsync(od).ConfigureAwait(false);
            updatedOd.DateCreation.Should().BeSameDateAs(DateTime.UtcNow);
            updatedOd.UniteId.Should().Be(1);
            updatedOd.AuteurCreationId.Should().Be(1);
            updatedOd.DeviseId.Should().Be(48);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void Delete()
        {
            GenerateFakeDatabase();

            OperationDiverseEnt od = oDtBuilderModel.OperationDiverseId(99).UniteId(0).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build();
            Invoking(() => Actual.Delete(od)).Should().BeNull();

        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void SaveOpeartionDiverseListForUnClosedPeriod()
        {
            fakeDateClotureComptableManager.Setup(m => m.IsPeriodClosed(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), null)).Returns(false);
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            Actual.Save(1, new DateTime(2019, 12, 1), operationDiverseEntsList);
            operationDiverseEntsList.Should().HaveCount(2);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void SaveOpeartionDiverseListForClosedPeriod()
        {
            fakeDateClotureComptableManager.Setup(m => m.IsPeriodClosed(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), null)).Returns(true);
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).CommandeId(10).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            Invoking(() => Actual.Save(1, new DateTime(2019, 12, 1), operationDiverseEntsList)).Should().ThrowExactly<FredBusinessException>().WithMessage(FeatureOperationDiverse.OperationDiverse_AjoutImpossible_MoisCloturer);
        }

        /// <summary>
        /// Test de la déclôture des OD
        /// Doit supprimer toutes les OD qui sont des OD d'écart et mettre à null les date de clôtures et passé à false la clôture des OD qui ne sont pas des OD d'écarts
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task UnCloseOds_WithCorrectCiIdAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 12, 10)).CiId(1).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 12, 10)).CiId(1).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 12, 10)).CiId(1).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 12, 10)).CiId(1).OdEcart(true).Build()
            };

            operationDiverseRepositoryMock.Setup(m => m.DeleteById(It.IsAny<object>())).Callback<object>(c => operationDiverseEntsList.RemoveAll(q => q.OperationDiverseId.Equals(c)));

            GenerateFakeDatabase(operationDiverseEntsList);
            await Actual.UnCloseOdsAsync(1, new DateTime(2019, 12, 31)).ConfigureAwait(false);

            operationDiverseEntsList.Should().HaveCount(1);
            foreach (OperationDiverseEnt item in operationDiverseEntsList)
            {
                item.DateCloture.Should().Be(null);
                item.Cloturee.Should().Be(false);
            }
        }

        /// <summary>
        /// Test de la déclôture des OD
        /// Doit supprimer toutes les OD qui sont des OD d'écart et mettre à null les date de clôtures et passé à false la clôture des OD qui ne sont pas des OD d'écarts
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task UnCloseOds_WithCorrectCiIdListAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 12, 10)).CiId(1).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 12, 10)).CiId(1).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 12, 10)).CiId(1).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 12, 10)).CiId(1).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 12, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 12, 10)).CiId(2).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 12, 10)).CiId(2).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 12, 10)).CiId(2).OdEcart(true).Build(),
                oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 12, 10)).CiId(2).OdEcart(true).Build()
            };

            operationDiverseRepositoryMock.Setup(m => m.DeleteById(It.IsAny<object>())).Callback<object>(c => operationDiverseEntsList.RemoveAll(q => q.OperationDiverseId.Equals(c)));

            GenerateFakeDatabase(operationDiverseEntsList);
            await Actual.UnCloseOdsAsync(new List<int> { 1, 2 }, new DateTime(2019, 12, 31)).ConfigureAwait(false);

            operationDiverseEntsList.Should().HaveCount(2);
            foreach (OperationDiverseEnt item in operationDiverseEntsList)
            {
                item.DateCloture.Should().Be(null);
                item.Cloturee.Should().Be(false);
            }
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithDepenseOneGlobalFilter_WithUniqueCIAndUniqueDateAsync()
        {

            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = new DateTime(2019, 1, 1),
                PeriodeFin = new DateTime(2019, 1, 1),
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false

            };
            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(1);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithDepenseOneGlobalFilter_WithUniqueCIAndMultipleDateAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = new DateTime(2019, 1, 1),
                PeriodeFin = new DateTime(2019, 2, 1),
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false

            };
            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(2);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithDepenseOneGlobalFilter_WithUniqueCIAndMultipleDateButWithStartDateNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = null,
                PeriodeFin = new DateTime(2019, 2, 1),
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false

            };
            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(2);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithDepenseOneGlobalFilter_WithUniqueCIAndMultipleDateButWithEndDateNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = new DateTime(2019, 1, 1),
                PeriodeFin = null,
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false

            };
            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(8);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithDepenseOneGlobalFilter_WithUniqueCIAndUniqueDateButWithStartDateNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = null,
                PeriodeFin = new DateTime(2019, 1, 1),
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false
            };

            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(1);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithDepenseOneGlobalFilter_WithMultipleCIAndUniqueDateButWithStartDateNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = null,
                PeriodeFin = new DateTime(2019, 2, 1),
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false
            };

            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(2);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithDepenseOneGlobalFilter_WithMultipleCIAndMultipleDateButWithEndDateNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = new DateTime(2019, 1, 1),
                PeriodeFin = null,
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false
            };

            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(2);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithMultipleDepenseGlobalFilter_WithMultipleCIAndMultipleDateButWithEndDateNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = new DateTime(2019, 1, 1),
                PeriodeFin = null,
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false
            };

            DepenseGlobaleFiltre filtre2 = new DepenseGlobaleFiltre
            {
                CiId = 2,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = new DateTime(2019, 3, 1),
                PeriodeFin = null,
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false
            };

            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre, filtre2 };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(8);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithMultipleDepenseGlobalFilter_WithMultipleCIAndUniqueDateButWithStartDateNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = null,
                PeriodeFin = new DateTime(2019, 1, 1),
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false
            };

            DepenseGlobaleFiltre filtre2 = new DepenseGlobaleFiltre
            {
                CiId = 2,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = null,
                PeriodeFin = new DateTime(2019, 3, 1),
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false
            };

            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre, filtre2 };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(3);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithMultipleDepenseGlobalFilter_WithMultipleCIAndUniqueDateButWithStartDateNullAndEndDateNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = null,
                PeriodeFin = null,
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false
            };

            DepenseGlobaleFiltre filtre2 = new DepenseGlobaleFiltre
            {
                CiId = 2,
                DeviseId = 48,
                IncludeFar = true,
                PeriodeDebut = null,
                PeriodeFin = null,
                RessourceId = 1,
                TacheId = 1,
                LastReplacedTask = false
            };

            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre, filtre2 };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(8);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        [TestCategory("DepenseGlobaleFiltre")]
        public async Task GetOperationDiverseList_WithMultipleDepenseGlobalFilter_WithMultipleCIAndUniqueDateButWithStartDateNullAndEndDateNullAndDeviseNullAndRessourceNullAndTacheIdNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 3, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 4, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 5, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(6).DateComptable(new DateTime(2019, 6, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(7).DateComptable(new DateTime(2019, 7, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(8).DateComptable(new DateTime(2019, 12, 31)).CiId(2).Build()
            };
            GenerateFakeDatabase(operationDiverseEntsList);

            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre
            {
                CiId = 1,
                DeviseId = null,
                IncludeFar = true,
                PeriodeDebut = null,
                PeriodeFin = null,
                RessourceId = null,
                TacheId = null,
                LastReplacedTask = false
            };

            DepenseGlobaleFiltre filtre2 = new DepenseGlobaleFiltre
            {
                CiId = 2,
                DeviseId = null,
                IncludeFar = true,
                PeriodeDebut = null,
                PeriodeFin = null,
                RessourceId = null,
                TacheId = null,
                LastReplacedTask = false
            };

            List<DepenseGlobaleFiltre> liste = new List<DepenseGlobaleFiltre> { filtre, filtre2 };

            (await Actual.GetOperationDiverseListAsync(liste).ConfigureAwait(false)).Should().HaveCount(8);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void CloseOds_WithUniqueCiUniqueSocieteId_WithCorrectionInput()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(1).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).FamilleOperationDiverseId(2).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).FamilleOperationDiverseId(3).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).FamilleOperationDiverseId(4).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).FamilleOperationDiverseId(5).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
            };

            EcritureComptableEnt ecritureComptableMo = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test1",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                JournalId = 1,
                Montant = 1000,
                FamilleOperationDiverseId = 1,
                NombreOD = 1,
                Quantite = 1
            };

            EcritureComptableEnt ecritureComptableAch = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test2",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 2,
                JournalId = 1,
                Montant = 2000,
                FamilleOperationDiverseId = 2,
                NombreOD = 0,
                Quantite = 1,
            };

            EcritureComptableEnt ecritureComptableMit = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test3",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 3,
                JournalId = 1,
                Montant = 3000,
                FamilleOperationDiverseId = 3,
                NombreOD = 0,
                Quantite = 1,
            };

            EcritureComptableEnt ecritureComptableMi = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test4",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 4,
                JournalId = 1,
                Montant = 4000,
                FamilleOperationDiverseId = 4,
                NombreOD = 0,
                Quantite = 1,
            };


            EcritureComptableEnt ecritureComptableOth = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test5",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 5,
                JournalId = 1,
                Montant = 5000,
                FamilleOperationDiverseId = 5,
                NombreOD = 0,
                Quantite = 1,
            };

            List<EcritureComptableEnt> ecritureComptableEnts = new List<EcritureComptableEnt> { ecritureComptableMo, ecritureComptableAch, ecritureComptableMit, ecritureComptableMi, ecritureComptableOth };

            fakeEcritureComptableManager.Setup(m => m.GetAllByCiIdAndDateComptableAsync(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(Task.FromResult<IEnumerable<EcritureComptableEnt>>(ecritureComptableEnts));

            GenerateFakeDatabase(operationDiverseEntsList);

            FamilleOperationDiverseEnt familleMo = familleOperationDiverseBuilder.NewMoFamily();
            FamilleOperationDiverseEnt familleAch = familleOperationDiverseBuilder.NewAchFamily();
            FamilleOperationDiverseEnt familleMit = familleOperationDiverseBuilder.NewMitFamily();
            FamilleOperationDiverseEnt familleMi = familleOperationDiverseBuilder.NewMiFamily();
            FamilleOperationDiverseEnt familleOth = familleOperationDiverseBuilder.NewOthFamily();

            RepartitionEcartEnt repartitionEcartMo = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleMo,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 100,
                EcritureComptables = new List<EcritureComptableEnt> { ecritureComptableMo },
                OperationDiverses = new List<OperationDiverseEnt> {
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(1).EcritureComptableId(1).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build(),
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(5).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build()
                }

            };

            RepartitionEcartEnt repartitionEcartAch = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleAch,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 200,
                EcritureComptables = new List<EcritureComptableEnt> { ecritureComptableAch },
                OperationDiverses = new List<OperationDiverseEnt> {
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(2).EcritureComptableId(2).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build(),
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(2).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build()
                }

            };

            RepartitionEcartEnt repartitionEcartMit = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleMit,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 300,
                EcritureComptables = new List<EcritureComptableEnt> { ecritureComptableMit },
                OperationDiverses = new List<OperationDiverseEnt> {
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(3).EcritureComptableId(3).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build(),
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(3).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build()
                }
            };

            RepartitionEcartEnt repartitionEcartMi = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleMi,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 400,
                EcritureComptables = new List<EcritureComptableEnt> { ecritureComptableMi },
                OperationDiverses = new List<OperationDiverseEnt> {
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(4).EcritureComptableId(4).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build(),
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(4).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build()
                }
            };

            RepartitionEcartEnt repartitionEcartOth = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleOth,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 500,
                EcritureComptables = new List<EcritureComptableEnt> { ecritureComptableOth },
                OperationDiverses = new List<OperationDiverseEnt> {
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(5).EcritureComptableId(5).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build(),
                    oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(5).Montant(100).DateComptable(new DateTime(2020, 1, 1)).CiId(1).Build()
                }
            };

            List<RepartitionEcartEnt> repartitionEcarts = new List<RepartitionEcartEnt>
            {
                repartitionEcartMo,
                repartitionEcartAch,
                repartitionEcartMi,
                repartitionEcartMit,
                repartitionEcartOth
            };

            // Actual.CloseOds(1, 1, new DateTime(2020, 1, 1), repartitionEcarts);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void CloseOds_WithMultipleCi_ToCompleted()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(1).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).FamilleOperationDiverseId(2).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).FamilleOperationDiverseId(3).DateComptable(new DateTime(2019, 3, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(4).FamilleOperationDiverseId(4).DateComptable(new DateTime(2019, 4, 10)).CiId(2).Build(),
                oDtBuilderModel.OperationDiverseId(5).FamilleOperationDiverseId(5).DateComptable(new DateTime(2019, 5, 10)).CiId(2).Build(),
            };

            EcritureComptableEnt ecritureComptableMo = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test1",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                JournalId = 1,
                Montant = 1000,
                FamilleOperationDiverseId = 5,
                NombreOD = 0,
                Quantite = 1,
            };

            List<EcritureComptableEnt> ecritureComptableEnts = new List<EcritureComptableEnt> { ecritureComptableMo };

            fakeEcritureComptableManager.Setup(m => m.GetAllByCiIdAndDateComptableAsync(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(Task.FromResult<IEnumerable<EcritureComptableEnt>>(ecritureComptableEnts));

            GenerateFakeDatabase(operationDiverseEntsList);

            FamilleOperationDiverseEnt familleMo = familleOperationDiverseBuilder.NewMoFamily();
            FamilleOperationDiverseEnt familleAch = familleOperationDiverseBuilder.NewAchFamily();
            FamilleOperationDiverseEnt familleMit = familleOperationDiverseBuilder.NewMitFamily();
            FamilleOperationDiverseEnt familleMi = familleOperationDiverseBuilder.NewMiFamily();
            FamilleOperationDiverseEnt familleOth = familleOperationDiverseBuilder.NewOthFamily();

            RepartitionEcartEnt repartitionEcartMo = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleMo,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 100

            };
            RepartitionEcartEnt repartitionEcartAch = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleAch,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 200
            };
            RepartitionEcartEnt repartitionEcartMit = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleMit,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 300
            };
            RepartitionEcartEnt repartitionEcartMi = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleMi,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 400
            };
            RepartitionEcartEnt repartitionEcartOth = new RepartitionEcartEnt
            {
                FamilleOperationDiverse = familleOth,
                CiId = 1,
                DateComptable = new DateTime(2020, 1, 1),
                Ecart = 500
            };
            List<RepartitionEcartEnt> repartitionEcarts = new List<RepartitionEcartEnt>
            {
                repartitionEcartMo,
                repartitionEcartAch,
                repartitionEcartMi,
                repartitionEcartMit,
                repartitionEcartOth
            };

            Actual.CloseOdsAsync(new List<int> { 1 }, new DateTime(2020, 1, 1), repartitionEcarts);
        }


        /// <summary>
        /// Retourne la liste des OD dans l'intervalle de temps avec tous les paramétre de renseigné
        /// Toutes les OD doivents être retournées
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetOperationDiverseListByCiIdAndRessourceAndPeriodeDebutAndPeriodeFinAndDeviseId_ReturnAllOdsAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1, 99, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31), 48).ConfigureAwait(false);
            actual.Should().HaveCount(8);
        }

        /// <summary>
        /// Retourne la liste des OD dans l'intervalle de temps qui ont une ressource de renseigné ou pas.
        /// Toutes les OD doivent être retournées
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetOperationDiverseListByCiIAndPeriodeDebutAndPeriodeFinAndDeviseId_WithRessourceIdNull_ReturnAllOdsAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1, null, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31), 48).ConfigureAwait(false);
            actual.Should().HaveCount(8);
        }

        /// <summary>
        /// Retourne la liste des OD dans l'intervalle de temps qui ont une devise de renseigné ou pas.
        /// Toutes les OD doivent être retournées
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetOperationDiverseListByCiIAndPeriodeDebutAndPeriodeFinAndRessourceId_WithDeviseIdNull_ReturnAllOdsAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1, 99, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31), null).ConfigureAwait(false);
            actual.Should().HaveCount(8);
        }

        /// <summary>
        /// Retourne la liste des OD dans l'intervalle de temps qui ont une devise de renseigné.
        /// Seules les OD avec une devise de renseigné doivent remontée
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetOperationDiverseListByCiIAndPeriodeDebutAndPeriodeFinAndRessourceId_WithDeviseIdNull_ReturnOdsDeviseIdNotNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).RessourceId(99).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).RessourceId(99).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).RessourceId(99).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).RessourceId(99).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).RessourceId(99).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1, 99, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31), 48).ConfigureAwait(false);
            actual.Should().HaveCount(1);
        }

        /// <summary>
        /// Retourne la liste des OD dans l'intervalle de temps qui ont une ressource de renseigné.
        /// Seules les OD avec une ressource de renseigné doivent remontée
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetOperationDiverseListByCiIAndPeriodeDebutAndPeriodeFinAndDeviseId_WithRessourceIdNull_ReturnOdsRessourceIdNotNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DeviseId(48).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DeviseId(48).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).DeviseId(48).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).DeviseId(48).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).DeviseId(48).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).DeviseId(48).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1, 99, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31), 48).ConfigureAwait(false);
            actual.Should().HaveCount(1);
        }

        /// <summary>
        /// Retourne la liste des OD dans l'intervalle de temps qui ont une devise de renseigné ou pas et une ressource de renseigné ou pass
        /// Toutes les OD doivent remontée
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetOperationDiverseListByCiIAndPeriodeDebutAndPeriodeFin_WithRessourceIdNullOrNotAndWithDeviseIdNullOrNot_ReturnAllOdsAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1, null, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31), null).ConfigureAwait(false);
            actual.Should().HaveCount(8);
        }

        /// <summary>
        /// Retourne la liste des OD dans l'intervalle de temps qui ont une devise de renseigné et une ressource de renseigné 
        /// Aucune OD ne doit remontée
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetOperationDiverseListByCiIAndPeriodeDebutAndPeriodeFin_WithRessourceIdNullOrNotAndWithDeviseIdNullOrNot_ReturnEmptyAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1, 99, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31), 48).ConfigureAwait(false);
            actual.Should().HaveCount(0);
        }

        /// <summary>
        /// Retourne la liste des OD dans l'intervalle de temps qui ont une devise de renseigné et une ressource de renseigné 
        /// Seules les OD avec la ressource et la devise doivent remontée
        /// </summary>
        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public async Task GetOperationDiverseListByCiIAndPeriodeDebutAndPeriodeFin_WithRessourceIdNullOrNotAndWithDeviseIdNullOrNot_ReturnOdsDeviseIdNotNullAndRessourceIdNotNullAsync()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(6).RessourceId(99).DeviseId(48).DateComptable(new DateTime(2019, 6, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(7).RessourceId(99).DateComptable(new DateTime(2019, 7, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(8).DeviseId(48).DateComptable(new DateTime(2019, 12, 31)).CiId(1).Build()
            };

            GenerateFakeDatabase(operationDiverseEntsList);
            IEnumerable<OperationDiverseEnt> actual = await Actual.GetOperationDiverseListAsync(1, 99, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31), 48).ConfigureAwait(false);
            actual.Should().HaveCount(1);
        }

        [TestMethod]
        [TestCategory("OperationDiverseManager")]
        public void GenerateRevertedOperationDiverse_WithCorrectInput()
        {
            List<OperationDiverseEnt> operationDiverseEntsList = new List<OperationDiverseEnt>
            {
                oDtBuilderModel.OperationDiverseId(1).FamilleOperationDiverseId(1).EcritureComptableId(1).DateComptable(new DateTime(2019, 1, 1)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(2).FamilleOperationDiverseId(2).EcritureComptableId(2).DateComptable(new DateTime(2019, 2, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(3).FamilleOperationDiverseId(3).EcritureComptableId(3).DateComptable(new DateTime(2019, 3, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(4).FamilleOperationDiverseId(4).EcritureComptableId(4).DateComptable(new DateTime(2019, 4, 10)).CiId(1).Build(),
                oDtBuilderModel.OperationDiverseId(5).FamilleOperationDiverseId(5).EcritureComptableId(5).DateComptable(new DateTime(2019, 5, 10)).CiId(1).Build(),
            };

            GenerateFakeDatabase(operationDiverseEntsList);

            EcritureComptableEnt ecritureComptableMo = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test1",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 1,
                JournalId = 1,
                Montant = 1000,
                FamilleOperationDiverseId = 1,
                NombreOD = 1,
                Quantite = 1
            };

            EcritureComptableEnt ecritureComptableAch = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test2",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 2,
                JournalId = 1,
                Montant = 2000,
                FamilleOperationDiverseId = 2,
                NombreOD = 0,
                Quantite = 1,
            };

            EcritureComptableEnt ecritureComptableMit = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test3",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 3,
                JournalId = 1,
                Montant = 3000,
                FamilleOperationDiverseId = 3,
                NombreOD = 0,
                Quantite = 1,
            };

            EcritureComptableEnt ecritureComptableMi = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test4",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 4,
                JournalId = 1,
                Montant = 4000,
                FamilleOperationDiverseId = 4,
                NombreOD = 0,
                Quantite = 1,
            };

            EcritureComptableEnt ecritureComptableOth = new EcritureComptableEnt
            {
                CiId = 1,
                CodeRef = "Test5",
                DateComptable = new DateTime(2020, 1, 1),
                DeviseId = 48,
                EcritureComptableId = 5,
                JournalId = 1,
                Montant = 5000,
                FamilleOperationDiverseId = 5,
                NombreOD = 0,
                Quantite = 1,
            };

            List<EcritureComptableEnt> ecritureComptableEnts = new List<EcritureComptableEnt> { ecritureComptableMo, ecritureComptableAch, ecritureComptableMit, ecritureComptableMi, ecritureComptableOth };

            Invoking(() => Actual.GenerateRevertedOperationDiverseAsync(ecritureComptableEnts)).Should().NotThrow();
        }
    }
}
