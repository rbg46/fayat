using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.CI.Builder;
using Fred.Common.Tests.Data.Groupe.Builder;
using Fred.Common.Tests.Data.Rapport.Builder;
using Fred.Common.Tests.Data.Rapport.Mock;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.PointagePersonnel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Rapport
{
    /// <summary>
    /// Classe de test de <see cref="RapportManager "/>
    /// </summary>
    [TestClass]
    public class RapportManagerTest : BaseTu<RapportManager>
    {
        private RapportMocks Mocks;
        private RapportLigneMocks LineMocks;

        [TestInitialize]
        public void Initialize()
        {
            Mocks = new RapportMocks();
            LineMocks = new RapportLigneMocks();

            //Setup
            var fakeRepository = GetMocked<IRapportRepository>();
            fakeRepository.Setup(r => r.GetAllSync())
                .Returns(new RapportMocks().RapportsStub.AsQueryable);
            fakeRepository.Setup(m => m.GetRapportsExportApi(It.IsAny<FilterRapportFesExport>()))
                .Returns(new RapportMocks().RapportsStub.AsQueryable);
            fakeRepository.Setup(m => m.CheckRepportsForFES(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new RapportMocks().RapportsStub.FirstOrDefault());
            fakeRepository.Setup(m => m.GetRapportsCis())
                .Returns(new RapportMocks().RapportsStub.AsQueryable);
            fakeRepository.Setup(m => m.GetRapportLigneVerrouillerByCiIdForReceptionInterimaire(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new RapportLigneMocks().RapportLignesStub);
            fakeRepository.Setup(m => m.GetRapportLigneVerrouillerByCiIdForReceptionMaterielExterne(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new RapportLigneMocks().RapportLignesStub);
            fakeRepository.Setup(m => m.GetRapportListWithRapportLignesBetweenDatesByCiList(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new RapportMocks().RapportsStub);

            //Mock les dependances du CommandeValidator
            var pointageMgrFake = GetMocked<IPointageManager>();
            pointageMgrFake.Setup(m => m.GetRapportLigneByRapportIdAndPersonnelId(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new RapportLigneBuilder().Build());

            var datesClotureComptableMgrFake = GetMocked<IDatesClotureComptableManager>();
            datesClotureComptableMgrFake.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Entities.DatesClotureComptable.DatesClotureComptableEnt() { CiId = 1 });
            datesClotureComptableMgrFake.Setup(m => m.IsPeriodClosed(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(false);

            var crudFeatureFake = GetMocked<ICrudFeature>();
            crudFeatureFake.Setup(m => m.GetRapportById(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.AddOrUpdateRapport(It.IsAny<RapportEnt>()));
            crudFeatureFake.Setup(m => m.AddRapport(It.IsAny<RapportEnt>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.AddNewPointageReelToRapport(It.IsAny<RapportEnt>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.AddPrimeToRapport(It.IsAny<RapportEnt>(), It.IsAny<PrimeEnt>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.AddTacheToRapport(It.IsAny<RapportEnt>(), It.IsAny<TacheEnt>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.UpdateRapport(It.IsAny<RapportEnt>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.GetRapportByIdWithoutValidation(It.IsAny<int>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.GetNewRapport(It.IsAny<int>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.ApplyValuesRgRapport(It.IsAny<RapportEnt>(), It.IsAny<string>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.VerrouillerRapport(It.IsAny<RapportEnt>(), It.IsAny<int>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.VerrouillerListeRapport(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<SearchRapportEnt>(), It.IsAny<string>()))
                .Returns(new LockRapportResponse { LockedRapports = Mocks.RapportsStub });
            crudFeatureFake.Setup(m => m.DuplicateRapport(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new DuplicateRapportResult());
            crudFeatureFake.Setup(m => m.DuplicateRapport(It.IsAny<RapportEnt>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.SaveListDuplicatedPointagesPersonnel(It.IsAny<List<RapportLigneEnt>>(), It.IsAny<int>(), out It.Ref<List<RapportEnt>>.IsAny, out It.Ref<List<RapportEnt>>.IsAny))
                .Returns(new PointagePersonnelSaveResultModel());
            crudFeatureFake.Setup(m => m.SaveListPointagesPersonnel(It.IsAny<List<RapportLigneEnt>>(), out It.Ref<List<RapportEnt>>.IsAny, out It.Ref<List<RapportEnt>>.IsAny))
                .Returns(new PointagePersonnelSaveResultModel());
            crudFeatureFake.Setup(m => m.FulfillAstreintesInformations(It.IsAny<RapportEnt>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());
            crudFeatureFake.Setup(m => m.GetRapportListWithRapportLignesNoTracking(It.IsAny<List<int>>()))
                .Returns(Mocks.RapportsStub);
            crudFeatureFake.Setup(m => m.GetRapportListBetweenDatesByCiList(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Mocks.RapportsStub);
            crudFeatureFake.Setup(m => m.DuplicateRapportForNewCi(It.IsAny<RapportEnt>()))
                .Returns(Mocks.RapportsStub.FirstOrDefault());

            var searchFeatureFake = GetMocked<ISearchFeature>();
            searchFeatureFake.Setup(m => m.GetFiltersList(It.IsAny<int>()))
                .Returns(new Entities.Rapport.Search.SearchRapportEnt());
            searchFeatureFake.Setup(m => m.SearchRapportWithFilter(It.IsAny<SearchRapportEnt>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(new SearchRapportListWithFilterResult());
            searchFeatureFake.Setup(m => m.RapportCanBeDeleted(It.IsAny<RapportEnt>()))
                .Returns(true);

            var utilitiesFeatureFake = GetMocked<IUtilitiesFeature>();
            utilitiesFeatureFake.Setup(m => m.IsStatutVerrouille(It.IsAny<RapportEnt>()))
                .Returns(false);

            var RapportValidator = new RapportValidator(pointageMgrFake.Object, datesClotureComptableMgrFake.Object);
            SubstituteConstructorArgument<IRapportValidator>(RapportValidator);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void CheckRapport_WithoutErrors()
        {
            RapportEnt rapport = new RapportBuilder()
                .CI(new CiBuilder().Societe(new SocieteBuilder()

                .Groupe(new GroupeBuilder().FayatFes().Build()).Build()
                ).Build()
                )
              .DateChantier(DateTime.UtcNow)
              .HoraireDebutS(DateTime.UtcNow)
              .HoraireFinS(DateTime.UtcNow)
              .AddLigne(new RapportLigneBuilder().Prototype())
              .Build();
            Actual.CheckRapport(rapport)
            .ListErreurs.Any().Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void CheckRapport_CINull_WithErrors()
        {
            RapportEnt rapport = new RapportBuilder().Build();
            Actual.CheckRapport(rapport)
            .ListErreurs.Any().Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void CheckRapport_CINotNull_WithErrors()
        {
            RapportEnt rapport = new RapportBuilder()
                .CI(new CiBuilder().Societe(new SocieteBuilder()
                .Groupe(new GroupeBuilder().RazelBec().Build()).Build()
                ).Build()
                )
              .HoraireDebutS(DateTime.UtcNow)
              .Build();
            Actual.CheckRapport(rapport)
            .ListErreurs.Any().Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void CheckRapport_ReportLine_WithErrors()
        {
            RapportEnt rapport = new RapportBuilder()
              .AddLigne(new RapportLigneBuilder().AddError("Fake error.").Build())
              .Build();
            Actual.CheckRapport(rapport)
            .ListErreurs.Any().Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetListRapportIdWithError()
        {
            List<RapportEnt> rapports = new RapportsBuilder(Mocks.RapportsStub).Build();
            Actual.GetListRapportIdWithError(rapports.Select(x => x.RapportId).ToList())
            .Count().Should().Be(0);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetFiltersList()
        {
            Actual.GetFiltersList(It.IsAny<int>())
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void SearchRapportWithFilter()
        {
            Actual.SearchRapportWithFilter(It.IsAny<SearchRapportEnt>())
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void AddOrUpdateRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Invoking(() => Actual.AddOrUpdateRapport(rapport))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void AddRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.AddRapport(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void AddNewPointageReelToRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.AddNewPointageReelToRapport(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void AddPrimeToRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.AddPrimeToRapport(rapport, new PrimeEnt())
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void AddTacheToRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.AddTacheToRapport(rapport, new Entities.Referential.TacheEnt())
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void UpdateRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.UpdateRapport(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void CheckRapportStatutChangedInDb()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Invoking(() => Actual.CheckRapportStatutChangedInDb(rapport, rapport))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void CheckRapportLignesMaterielChanged()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.CheckRapportLignesMaterielChanged(rapport, rapport)
            .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void CheckRapportLignesPersonnelChanged()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.CheckRapportLignesPersonnelChanged(rapport, rapport)
            .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportList()
        {
            List<RapportEnt> rapports = new RapportsBuilder(Mocks.RapportsStub).Build();
            Actual.GetRapportList(rapports.Select(x => x.RapportId).ToList())
            .Count().Should().Be(0);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportsMobile()
        {
            Actual.GetRapportsMobile(DateTime.UtcNow, 1)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportLightList()
        {
            Actual.GetRapportLightList(1, DateTime.UtcNow)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportById()
        {
            Actual.GetRapportById(1, false)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportByIdWithoutValidation()
        {
            Actual.GetRapportByIdWithoutValidation(1)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetNewRapport()
        {
            Actual.GetNewRapport(1)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void ApplyValuesRgRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.ApplyValuesRgRapport((rapport), string.Empty)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void VerrouillerRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.VerrouillerRapport((rapport), 1)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void DeverrouillerRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Invoking(() => Actual.DeverrouillerRapport((rapport), 1))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void VerrouillerListeRapport()
        {
            List<RapportEnt> rapports = new RapportsBuilder(Mocks.RapportsStub).Build();
            Actual.VerrouillerListeRapport(rapports.Select(x => x.RapportId).ToList(), 1, null, new SearchRapportEnt(), string.Empty)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void DeverrouillerListeRapport()
        {
            List<RapportEnt> rapports = new RapportsBuilder(Mocks.RapportsStub).Build();
            Invoking(() => Actual.DeverrouillerListeRapport(rapports.Select(x => x.RapportId).ToList(), 1))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void DuplicateRapportById_Returns_DuplicatedRapport()
        {
            Actual.DuplicateRapport(1, DateTime.UtcNow, DateTime.UtcNow)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void DuplicateRapportByRapportEnt_Returns_DuplicatedRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.DuplicateRapport(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void DuplicateRapportForNewCi_Returns_DuplicatedRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.DuplicateRapportForNewCi(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void ValidationRapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.ValidationRapport(rapport, 1)
            .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void SaveListDuplicatedPointagesPersonnel()
        {
            var rapportAdded = new List<RapportEnt>();
            var rapportUpdated = new List<RapportEnt>();

            List<RapportLigneEnt> rapportLignes = new RapportLignesBuilder(LineMocks.RapportLignesStub).Build();
            Actual.SaveListDuplicatedPointagesPersonnel(rapportLignes, 1, out rapportAdded, out rapportUpdated)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void SaveListPointagesPersonnel()
        {
            var rapportAdded = new List<RapportEnt>();
            var rapportUpdated = new List<RapportEnt>();

            List<RapportLigneEnt> rapportLignes = new RapportLignesBuilder(LineMocks.RapportLignesStub).Build();
            Actual.SaveListPointagesPersonnel(rapportLignes, out rapportAdded, out rapportUpdated)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void IsDateChantierInPeriodeCloture()
        {
            Actual.IsDateChantierInPeriodeCloture(new RapportBuilder().Prototype())
            .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportListAllSync()
        {
            Actual.GetRapportListAllSync()
            .ToList().Count().Should().Be(4);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportsExportApi()
        {
            FilterRapportFesExport rapport = new FilterRapportFesExport();
            Actual.GetRapportsExportApi(rapport)
            .ToList().Count().Should().Be(4);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void InitializeAstreintesInformations()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Invoking(() => Actual.InitializeAstreintesInformations(rapport))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void FulfillAstreintesInformations()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Actual.FulfillAstreintesInformations(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void CheckRepportsForFES()
        {
            Actual.CheckRepportsForFES(1, DateTime.UtcNow)
            .Should().Be(1);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportCi()
        {
            Actual.GetRapportCi(1)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportLigneVerrouillerByCiIdForReceptionInterimaire()
        {
            Actual.GetRapportLigneVerrouillerByCiIdForReceptionInterimaire(1, 1)
            .Count().Should().Be(2);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportLigneVerrouillerByCiIdForReceptionMaterielExterne()
        {
            Actual.GetRapportLigneVerrouillerByCiIdForReceptionMaterielExterne(1, 1)
            .Count().Should().Be(2);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportListWithRapportLignesNoTracking()
        {
            List<RapportEnt> rapports = new RapportsBuilder(Mocks.RapportsStub).Build();
            Actual.GetRapportListWithRapportLignesNoTracking(rapports.Select(x => x.RapportId).ToList())
            .Count().Should().Be(4);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void AddOrUpdateRapportList()
        {
            List<RapportEnt> rapports = new RapportsBuilder(Mocks.RapportsStub).Build();
            Invoking(() => Actual.AddOrUpdateRapportList(rapports))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportListBetweenDatesByCiList()
        {
            List<RapportEnt> rapports = new RapportsBuilder(Mocks.RapportsStub).Build();
            Actual.GetRapportListBetweenDatesByCiList(rapports.Select(x => x.RapportId).ToList(), DateTime.UtcNow, DateTime.UtcNow)
            .Count().Should().Be(4);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void AddRangeRapportList()
        {
            List<RapportEnt> rapports = new RapportsBuilder(Mocks.RapportsStub).Build();
            Invoking(() => Actual.AddRangeRapportList(rapports))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void GetRapportListWithRapportLignesBetweenDatesByCiList()
        {
            Actual.GetRapportListWithRapportLignesBetweenDatesByCiList(new List<int>() { 1, 2 }, DateTime.UtcNow, DateTime.UtcNow)
            .Count().Should().Be(4);
        }
    }
}
