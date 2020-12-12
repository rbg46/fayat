using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Business.CI;
using Fred.Business.OperationDiverse;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Journal.Mock;
using Fred.Common.Tests.Data.Nature.Mock;
using Fred.Common.Tests.Data.OperationDiverse.Builder;
using Fred.Common.Tests.Data.Societe.Mock;
using Fred.DataAccess.Interfaces;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Web.Shared.Models.Nature;
using Fred.Web.Shared.Models.OperationDiverse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.OperationDiverse
{
    /// <summary>
    /// Test de classe de <see cref="FamilleOperationDiverseManager"/>
    /// </summary>
    [TestClass]
    public class FamilleOperationDiverseManagerTest : BaseTu<FamilleOperationDiverseManager>
    {
        private readonly Mock<IUnitOfWork> UnitOfWork = new Mock<IUnitOfWork>();
        private readonly JournalMocks JournalMocks = new JournalMocks();
        private readonly NatureMocks NatureMocks = new NatureMocks();
        private readonly SocieteMocks SocieteMocks = new SocieteMocks();
        private readonly FamilleOperationDiverseBuilder builder = new FamilleOperationDiverseBuilder();
        private readonly Mock<IFamilleOperationDiverseRepository> FamilleOperationDiverseRepository = new Mock<IFamilleOperationDiverseRepository>();
        private FamilleOperationDiverseEnt FamilleOperationDiverseMockEnt = new FamilleOperationDiverseEnt();
        private SocieteEnt SocieteMockEnt = new SocieteEnt();
        FamilleOperationDiverseNatureJournalModel FamilleOperationDiverseNatureJournalMockModel = new FamilleOperationDiverseNatureJournalModel();
        FamilleOperationDiverseModel FamilleOperationDiverseMockModel = new FamilleOperationDiverseModel();
        FamilleOperationDiverseModel FodMockModel = new FamilleOperationDiverseModel();
        List<ParametrageFamilleOperationDiverseModel> AllParamBySociete = new List<ParametrageFamilleOperationDiverseModel>();
        private Mock<ICIManager> fakeCiManager;

        [TestInitialize]
        public void Initialize()
        {
            var context = GetMocked<FredDbContext>();
            context.Object.FamilleOperationDiverse = builder.BuildFakeDbSet(1, builder.New());
            context.Object.Journals = JournalMocks.GetFakeDbSet();
            context.Object.Natures = NatureMocks.GetFakeDbSet();
            context.Object.Societes = SocieteMocks.GetFakeDbSet();
            AllParamBySociete.Add(new ParametrageFamilleOperationDiverseModel { FamilleOperationDiverse = new FamilleOperationDiverseEnt {FamilleOperationDiverseId = 1,Code = "RCT" }, Nature = new Entities.Referential.NatureEnt {NatureId = 17,Code= "102202" }, Journal = new Entities.Journal.JournalEnt { JournalId=16,Code="ACH"} });
            List<NatureFamilleOdModel> AssociatedNatures = new List<NatureFamilleOdModel>();
            SubstituteConstructorArgument(UnitOfWork.Object);

            SocieteMockEnt = new SocieteEnt()
            {
                SocieteId = 3,
                GroupeId = 1
            };

            FamilleOperationDiverseMockEnt = new FamilleOperationDiverseEnt()
            {
                Code = "MO",
                FamilleOperationDiverseId = 1,
                Libelle = "MO POINTEE (Hors Interim)",
                LibelleCourt = "Déboursé MO",
                Societe = context.Object.Societes.Find(6)
            };

            FamilleOperationDiverseNatureJournalMockModel = new FamilleOperationDiverseNatureJournalModel { FamilleOperationDiverse = FamilleOperationDiverseMockEnt, Nature = context.Object.Natures.FirstOrDefault(), Journal = context.Object.Journals.FirstOrDefault() };
            FamilleOperationDiverseMockModel = new FamilleOperationDiverseModel { FamilleOperationDiverseId = 1,SocieteId = 1, AssociatedJournaux = JournalMocks.GetListJournalFamilleODModel(), AssociatedNatures = NatureMocks.GetListNatureFamilleODModel() };
            FodMockModel = new FamilleOperationDiverseModel { FamilleOperationDiverseId = 0, SocieteId = 1};

            fakeCiManager = GetMocked<ICIManager>();
            fakeCiManager.Setup(m => m.GetSocieteByCIId(3, false)).Returns(SocieteMockEnt);

            //setup repositories
            FamilleOperationDiverseRepository.Setup(la => la.FindById(1)).Returns(FamilleOperationDiverseMockEnt);
            FamilleOperationDiverseRepository.Setup(la => la.GetAllParametrageFamilleOperationDiverseNaturesJournaux(1)).Returns(AllParamBySociete);
            FamilleOperationDiverseRepository.Setup(la => la.GetFamilyBySociety(3)).Returns(builder.BuildFakeDbSet(builder.New()));
            SubstituteConstructorArgument(FamilleOperationDiverseRepository.Object);
        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseManager")]
        public void GetFamiliesBySociety_Returns_OnlyContain_SocietyExpected()
        {
            Actual.GetFamiliesBySociety(3).Should().OnlyContain(r => r.SocieteId.Equals(1));
        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseManager")]
        public void GetFamiliesByCI_Returns_OnlyContain_CiExpected()
        {
            Actual.GetFamiliesByCI(3).Should().OnlyContain(r => r.SocieteId.Equals(1));
        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseManager")]
        public void GetFamille_Returns_BeEquivalentTo_FamilleOperationDiverseExpected()
        {
            FamilleOperationDiverseModel expected =
                new FamilleOperationDiverseModelBuilder()
                    .FamilleMO()
                    .SocieteCode("50338")
                    .Build();

            Actual.GetFamille(1).Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseManager")]
        public void LaunchControleParametrageForJournal_Returns_BeEmpty_SocietyNotExist()
        {
            Actual.LaunchControleParametrageForJournal(99999).Should().BeEmpty();
        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseManager")]
        public void LaunchControleParametrageForNature_Returns_BeEmpty_SocietyNotExist()
        {
            Actual.LaunchControleParametrageForNature(99999).Should().BeEmpty();
        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseManager")]
        public void UpdateFamilleOperationDiverse_WithIncorrectsInputs_ThrowException()
        {
            Invoking(async () => await Actual.UpdateFamilleOperationDiverseAsync(null).ConfigureAwait(false)).Should().NotThrow<Exception>();
        }

        [TestMethod]
        [TestCategory("FamilleOperationDiverseManager")]
        public void SetParametrageNaturesJournaux_WithDoublons()
        {
            Invoking(async () => await Actual.SetParametrageNaturesJournaux(FamilleOperationDiverseMockModel).ConfigureAwait(false)).Should().NotBeNull();
        }

        [TestMethod]
        [Ignore]
        [TestCategory("FamilleOperationDiverseManager")]
        public void UpdateFamilleOperationDiverse_WithEmptyShortLabel_ThrowException()
        {
            FamilleOperationDiverseMockModel.LibelleCourt = string.Empty;
            Invoking(async () => await Actual.UpdateFamilleOperationDiverseAsync(FamilleOperationDiverseMockModel).ConfigureAwait(false)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        [Ignore]
        [TestCategory("FamilleOperationDiverseManager")]
        public void UpdateFamilleOperationDiverse_WithLongShortLabel_ThrowException()
        {
            FamilleOperationDiverseMockModel.LibelleCourt = "un label de plus de 20 caracteres.";
            Invoking(async () => await Actual.UpdateFamilleOperationDiverseAsync(FamilleOperationDiverseMockModel).ConfigureAwait(false)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        [Ignore]
        [TestCategory("FamilleOperationDiverseManager")]
        public void UpdateFamilleOperationDiverse_WithCorrectShortLabel_NotThrowException()
        {
            FamilleOperationDiverseMockModel.LibelleCourt = "Libelle court";
            Invoking(async () => await Actual.UpdateFamilleOperationDiverseAsync(FamilleOperationDiverseMockModel).ConfigureAwait(false)).Should().NotThrow<ArgumentException>();
        }
    }
}
