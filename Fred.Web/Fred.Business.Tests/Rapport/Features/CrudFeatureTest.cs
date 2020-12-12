using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation;
using Fred.Business.Affectation;
using Fred.Business.CI;
using Fred.Business.FeatureFlipping;
using Fred.Business.Personnel;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Duplication;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Rapport.RapportHebdo;
using Fred.Business.RapportStatut;
using Fred.Business.Referential.Tache;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Affectation.Builder;
using Fred.Common.Tests.Data.Astreinte.Builder;
using Fred.Common.Tests.Data.CI.Builder;
using Fred.Common.Tests.Data.CI.Mock;
using Fred.Common.Tests.Data.Groupe.Builder;
using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.Common.Tests.Data.Rapport.Builder;
using Fred.Common.Tests.Data.Rapport.Mock;
using Fred.Common.Tests.Data.Referential.Materiel;
using Fred.Common.Tests.Data.Referential.Prime.Mock;
using Fred.Common.Tests.Data.Referential.Tache.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Rapport.Features
{
    /// <summary>
    /// Classe de test de <see cref="CrudFeature"/>
    /// </summary>
    [TestClass]
    public class CrudFeatureTest : BaseTu<CrudFeature>
    {
        private RapportMocks Mocks;
        private static Dictionary<int, bool> roles = new Dictionary<int, bool>() { { 1, true }, { 2, true }, { 3, true }, { 4, true } };
        private static Dictionary<string, int?> lotPointage = new Dictionary<string, int?>() { { DateTime.UtcNow.ToString("yyyyMM"), 1 }, { "000101", 1 } };

        [TestInitialize]
        public void Initialize()
        {
            Mocks = new RapportMocks();

            //Mock les dependances du CommandeValidator
            var pointageMgrFake = GetMocked<IPointageManager>();
            pointageMgrFake.Setup(m => m.GetNewPointageReelLight())
                .Returns(new RapportLigneBuilder().Build());
            pointageMgrFake.Setup(m => m.GetNewPointagePrime(It.IsAny<RapportLigneEnt>(), It.IsAny<PrimeEnt>()))
                .Returns(new RapportLignePrimeBuilder().Build());
            pointageMgrFake.Setup(m => m.ApplyValuesRGPointageReel(It.IsAny<RapportLigneEnt>(), It.IsAny<string>()))
                .Returns(new RapportLigneBuilder().Build());
            pointageMgrFake.Setup(m => m.UpdatePointage(It.IsAny<RapportLigneEnt>()));
            pointageMgrFake.Setup(m => m.CheckPointageForSave(It.IsAny<List<RapportLigneEnt>>()))
                .Returns(new RapportLigneMocks().RapportLignesStub);

            var utilisateureMgrFake = GetMocked<IUtilisateurManager>();
            utilisateureMgrFake.Setup(u => u.GetContextUtilisateurId()).Returns(1);
            utilisateureMgrFake.Setup(u => u.GetContextUtilisateur()).Returns(new UtilisateurBuilder().Personnel(new PersonnelBuilder().Prototype()).Build());
            utilisateureMgrFake.Setup(u => u.IsRolePaie(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            utilisateureMgrFake.Setup(u => u.IsRolePaie(It.IsAny<int>(), It.IsAny<List<int>>())).Returns(roles);
            utilisateureMgrFake.Setup(u => u.GetAllCIbyUser(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int?>())).Returns(new List<int> { 1, 2, 3 });
            utilisateureMgrFake.Setup(u => u.IsNiveauPaie3(2, It.IsAny<int>())).Returns(true);
            utilisateureMgrFake.Setup(u => u.IsNiveauPaie2(3, It.IsAny<int>())).Returns(true);
            utilisateureMgrFake.Setup(u => u.IsNiveauPaie1(4, It.IsAny<int>())).Returns(true);
            utilisateureMgrFake.Setup(u => u.IsUtilisateurOfGroupe(It.IsAny<string>())).Returns(true);

            var rapportHebdoMgrFake = GetMocked<IRapportHebdoManager>();

            var utilitiesMgrFake = GetMocked<IUtilitiesFeature>();
            utilitiesMgrFake.Setup(u => u.IsTodayInPeriodeCloture(It.IsAny<RapportEnt>())).Returns(true);
            utilitiesMgrFake.Setup(u => u.IsStatutVerrouille(It.IsAny<RapportEnt>())).Returns(false);

            var affectationMgrFake = GetMocked<IAffectationManager>();
            affectationMgrFake.Setup(u => u.GetAffectationsByCiId(It.IsAny<int>()))
                .Returns(new AffectationsBuilder().AddItem(new AffectationBuilder().Prototype()).Build());
            affectationMgrFake.Setup(u => u.GetAstreinte(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new AstreinteBuilder().Prototype());

            var tacheMgrFake = GetMocked<ITacheManager>();
            tacheMgrFake.Setup(u => u.GetTacheParDefaut(It.IsAny<int>()))
                .Returns(new TacheBuilder().Build());

            var featureFlippingMgrFake = GetMocked<IFeatureFlippingManager>();
            featureFlippingMgrFake.Setup(u => u.IsActivated(It.IsAny<Framework.FeatureFlipping.EnumFeatureFlipping>()))
                .Returns(true);

            var personnelMgrFake = GetMocked<IPersonnelManager>();
            personnelMgrFake.Setup(u => u.GetMaterielDefault(It.IsAny<int>()))
                .Returns(new MaterielBuilder().Prototype());
            personnelMgrFake.Setup(u => u.GetPersonnelGroupebyId(It.IsAny<int>()))
                .Returns(new GroupeBuilder().Prototype());
            personnelMgrFake.Setup(u => u.GetPersonnelById(It.IsAny<int>()))
                .Returns(new PersonnelBuilder().Prototype());

            var rapportDuplicationServiceFake = GetMocked<IRapportDuplicationService>();
            rapportDuplicationServiceFake.Setup(u => u.GetRapportForDuplication(It.IsAny<int>()))
                .Returns(new RapportMocks().RapportsStub.First());
            rapportDuplicationServiceFake.Setup(u => u.DuplicateRapport(It.IsAny<RapportEnt>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new DuplicateRapportResultBuilder().Prototype());
            rapportDuplicationServiceFake.Setup(u => u.DuplicateRapport(It.IsAny<RapportEnt>()))
                .Returns(new RapportMocks().RapportsStub.First());

            var rapportDuplicationNewCiServiceFake = GetMocked<IRapportDuplicationNewCiService>();
            rapportDuplicationNewCiServiceFake.Setup(u => u.DuplicateRapport(It.IsAny<RapportEnt>()))
                .Returns(new RapportMocks().RapportsStub.First());

            var lotPointageMgrFake = GetMocked<ILotPointageManager>();
            lotPointageMgrFake.Setup(u => u.GetorCreate(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new Entities.ValidationPointage.LotPointageEnt());
            lotPointageMgrFake.Setup(u => u.GetLotPointageId(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(1);
            lotPointageMgrFake.Setup(u => u.GetLotPointageId(It.IsAny<int>(), It.IsAny<List<string>>()))
                .Returns(lotPointage);

            var rapportStatutMgrFake = GetMocked<IRapportStatutManager>();
            rapportStatutMgrFake.Setup(u => u.GetRapportStatutByCode(It.IsAny<string>()))
                .Returns(new RapportStatutEnt());

            var ciMgrFake = GetMocked<ICIManager>();
            ciMgrFake.Setup(u => u.GetCIById(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(new CiBuilder().Prototype());

            var builder = new RapportBuilder();
            var context = GetMocked<FredDbContext>();
            context.Object.Rapports = new RapportMocks().GetFakeDbSet();
            context.Setup(c => c.Set<RapportEnt>()).Returns(new RapportMocks().GetFakeDbSet());
            var securityManager = GetMocked<ISecurityManager>();
            var unitOfWork = new UnitOfWork(context.Object, securityManager.Object);
            SubstituteConstructorArgument<IUnitOfWork>(unitOfWork);

            //Setup
            var fakeRepository = GetMocked<IRapportRepository>();
            fakeRepository.Setup(f => f.Query()).Returns(new RepositoryQuery<RapportEnt>(new DbRepository<RapportEnt>(context.Object)));
            fakeRepository.Setup(f => f.GetRapportList()).Returns(new RapportMocks().GetFakeDbSet());
            fakeRepository.Setup(f => f.GetAllSync())
                .Returns(new RapportMocks().RapportsStub.AsQueryable);
            fakeRepository.Setup(f => f.GetRapportById(It.IsAny<int>()))
                .Returns(new RapportMocks().RapportsStub.First());
            fakeRepository.Setup(f => f.GetRapportListWithRapportLignesNoTracking(It.IsAny<IEnumerable<int>>()))
                .Returns(new RapportMocks().RapportsStub);
            fakeRepository.Setup(f => f.GetRapportsMobile(It.IsAny<DateTime>()))
                .Returns(new RapportMocks().RapportsStub);
            fakeRepository.Setup(f => f.GetRapportToLock(It.IsAny<IEnumerable<int>>()))
                .Returns(new RapportMocks().RapportsStub);

            var rapportHebdoServiceMock = GetMocked<IRapportHebdoService>();
            SubstituteConstructorArgument(rapportHebdoServiceMock.Object);

        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        public void AddOrUpdateRapport_WithReportDoesNotExist_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder().Prototype();
            Invoking(() => Actual.AddOrUpdateRapport(rapport)).Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void AddRapport_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .Build();
            Actual.AddRapport(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void AddNewPointageReelToRapport_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .ListLignes(new RapportLignesBuilder()
                        .AddItem(new RapportLigneBuilder()
                        .ListRapportLigneTaches(new List<RapportLigneTacheEnt>() { new RapportLigneTacheBuilder().Prototype() })
                        .ListRapportLignePrimes(new List<RapportLignePrimeEnt>() { new RapportLignePrimeBuilder().Prototype() })
                        .Build())
                    .Build())
                .Build();
            Actual.AddNewPointageReelToRapport(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void AddNewPointageReelToRapport_WithCodeGroupe_FES_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .CI(new CiBuilder().Societe(new SocieteBuilder()
                .Groupe(new GroupeBuilder().FayatFes().Build()).Build()
                ).Build()
                )
                .Build();
            Actual.AddNewPointageReelToRapport(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void AddPrimeToRapport_WithPrimeNull_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .Build();
            Actual.AddPrimeToRapport(rapport, null)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void AddPrimeToRapport_WithPrime_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .ListLignes(new RapportLignesBuilder()
                        .AddItem(new RapportLigneBuilder()
                        .ListRapportLignePrimes(new List<RapportLignePrimeEnt>() { new RapportLignePrimeBuilder().Prototype() })
                        .Build())
                        .AddItem(new RapportLigneBuilder()
                        .ListRapportLignePrimes(new List<RapportLignePrimeEnt>() { new RapportLignePrimeBuilder().Build() })
                        .Build())
                    .Build())
                .Build();
            Actual.AddPrimeToRapport(rapport, new PrimeMocks().GetFakeDbSet().First())
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void AddTacheToRapport_WithTacheNull_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .Build();
            Actual.AddTacheToRapport(rapport, null)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void AddTacheToRapport_WithTache_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .ListLignes(new RapportLignesBuilder()
                        .AddItem(new RapportLigneBuilder()
                        .ListRapportLigneTaches(new List<RapportLigneTacheEnt>() { new RapportLigneTacheBuilder().Prototype() })
                        .Build())
                        .AddItem(new RapportLigneBuilder()
                        .ListRapportLigneTaches(new List<RapportLigneTacheEnt>() { new RapportLigneTacheBuilder().Build() })
                        .Build())
                    .Build())
                .Build();
            Actual.AddTacheToRapport(rapport, new TacheBuilder().Build())
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void ApplyValuesRgRapport_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .CI(new CiBuilder().Prototype())
                .ListLignes(new RapportLignesBuilder()
                        .AddItem(new RapportLigneBuilder().Build())
                        .Build())
                .Build();
            Actual.ApplyValuesRgRapport(rapport, Constantes.EntityType.CI)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void DuplicateRapport_OverAPeriod_Return_DuplicateRapport()
        {
            Actual.DuplicateRapport(1, DateTime.UtcNow, DateTime.UtcNow)
            .Rapports.Count().Should().Be(4);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void DuplicateRapport_Return_Rapport()
        {
            RapportEnt rapport = new RapportBuilder().Build();
            Actual.DuplicateRapport(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void DuplicateRapportForNewCi_Returns_DupicateRapport()
        {
            RapportEnt rapport = new RapportBuilder().Build();
            Actual.DuplicateRapportForNewCi(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void VerrouillerRapport_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .AddLigne(new RapportLigneBuilder().Build())
                .Build();
            Actual.VerrouillerRapport(rapport, 1)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void DeverrouillerRapport_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                .AddLigne(new RapportLigneBuilder().Build())
                .Build();
            Invoking(() => Actual.DeverrouillerRapport(rapport, 1))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void VerrouillerListeRapport_Returns_RapportList()
        {
            List<RapportEnt> rapports = new RapportMocks().RapportsStub;
            Actual.VerrouillerListeRapport(rapports.Select(r => r.RapportId), 1, null, new Entities.Rapport.Search.SearchRapportEnt(), string.Empty)
                .LockedRapports
                .Count().Should().Be(4);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void VerrouillerListeRapport_WithRapportIdsListNull_Returns_ArgumentException()
        {
            Invoking(() => Actual.VerrouillerListeRapport(null, 1, null, new Entities.Rapport.Search.SearchRapportEnt(), string.Empty))
            .Should().Throw<ArgumentException>();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void DeverrouillerListeRapport_Returns_RapportList()
        {
            List<RapportEnt> rapports = new RapportMocks().RapportsStub;
            Invoking(() => Actual.DeverrouillerListeRapport(rapports.Select(r => r.RapportId), 1))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void DeverrouillerListeRapport_WithRapportIdsListNull_Returns_ArgumentException()
        {
            Invoking(() => Actual.DeverrouillerListeRapport(null, 1))
            .Should().Throw<ArgumentException>();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void DeleteRapport()
        {
            RapportEnt rapport = new RapportMocks().GetFakeDbSet().FirstOrDefault();
            Invoking(() => Actual.DeleteRapport(rapport, 1, true))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void GetNewRapport_Returns_Rapport()
        {
            Actual.GetNewRapport(1)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void GetRapportById_Returns_Rapport()
        {
            Actual.GetRapportById(1, false)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void GetRapportByIdWithoutValidation_Returns_Rapport()
        {
            Actual.GetRapportByIdWithoutValidation(1)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void GetRapportLightList_Returns_RapportList()
        {
            Actual.GetRapportLightList(1, DateTime.UtcNow)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void GetRapportList_Returns_RapportList()
        {
            Actual.GetRapportList()
            .Count().Should().Be(4);
        }


        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void GetRapportList_PerListIds_Returns_RapportList()
        {
            Actual.GetRapportList(new List<int>() { 1, 2, 3 })
            .Count().Should().Be(3);
        }


        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void GetRapportListWithRapportLignesNoTracking_Returns_RapportList()
        {
            List<RapportEnt> rapports = new RapportMocks().RapportsStub;
            Actual.GetRapportListWithRapportLignesNoTracking(rapports.Select(r => r.RapportId))
            .Count().Should().Be(4);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void GetRapportsMobile_Returns_RapportList()
        {
            Actual.GetRapportsMobile(DateTime.UtcNow)
            .Count().Should().Be(4);
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void UpdateRapport_Returns_UpdatedRapport()
        {
            RapportEnt rapport = new RapportMocks().RapportsStub.First();
            Actual.UpdateRapport(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void ValidationRapport_NoValidationLevel_Returns_TFalse()
        {
            Actual.ValidationRapport(new RapportMocks().RapportsStub.First(), 1)
            .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void ValidationRapport_Niveau3_Returns_True()
        {
            Actual.ValidationRapport(new RapportMocks().RapportsStub.First(), 2)
            .Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void ValidationRapport_Niveau2_Returns_True()
        {
            Actual.ValidationRapport(new RapportMocks().RapportsStub.First(), 3)
            .Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void ValidationRapport_Niveau1_Returns_True()
        {
            Actual.ValidationRapport(new RapportMocks().RapportsStub.First(), 4)
            .Should().BeTrue();
        }

        [TestMethod]
        [Ignore]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void SaveListDuplicatedPointagesPersonnel_Returns_PointagePersonnelSaveResultModel()
        {
            var rapportsAdded = new List<RapportEnt>();
            var rapportsUpdated = new List<RapportEnt>();

            List<RapportLigneEnt> rapportLignes = new RapportLigneMocks().RapportLignesStub;
            Actual.SaveListDuplicatedPointagesPersonnel(rapportLignes, 1, out rapportsAdded, out rapportsUpdated)
            .Should().NotBeNull();
        }

        [TestMethod]
        [Ignore]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void SaveListPointagesPersonnel_Returns_PointagePersonnelSaveResultModel()
        {
            var rapportsAdded = new List<RapportEnt>();
            var rapportsUpdated = new List<RapportEnt>();

            List<RapportLigneEnt> rapportLignes = new RapportLigneMocks().RapportLignesStub;
            Actual.SaveListPointagesPersonnel(rapportLignes, out rapportsAdded, out rapportsUpdated)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void FulfillAstreintesInformations_Returns_Rapport()
        {
            RapportEnt rapport = new RapportBuilder()
                  .AddLigne(new RapportLigneBuilder()
                    .Personnel(new PersonnelBuilder().Prototype()).Build())
                  .Build();
            Actual.FulfillAstreintesInformations(rapport)
            .Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void AddOrUpdateRapportList()
        {
            List<RapportEnt> rapports = new RapportMocks().RapportsStub;
            Invoking(() => Actual.AddOrUpdateRapportList(rapports))
            .Should().NotThrow();
        }
        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void CheckRapportStatutChangedInDb_Returns_Nothing()
        {
            RapportEnt rapport = new RapportMocks().RapportsStub.First();
            Invoking(() => Actual.CheckRapportStatutChangedInDb(rapport, rapport))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void CheckRapportStatutChangedInDb_Returns_ArgumentException()
        {
            var rapportBeforeUpdate = new RapportMocks().RapportsStub.First();
            rapportBeforeUpdate.RapportStatutId = 2;
            rapportBeforeUpdate.RapportStatut = new RapportStatutEnt() { RapportStatutId = 2, Libelle = "Test" };

            RapportEnt rapport = new RapportMocks().RapportsStub.First();
            Invoking(() => Actual.CheckRapportStatutChangedInDb(rapport, rapportBeforeUpdate))
            .Should().Throw<ValidationException>();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void CheckRapportLignesMaterielChanged_Returns_False()
        {
            Actual.CheckRapportLignesMaterielChanged(new RapportMocks().RapportsStub.First(), new RapportMocks().RapportsStub.First())
            .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void CheckRapportLignesPersonnelChanged_Returns_False()
        {
            Actual.CheckRapportLignesPersonnelChanged(new RapportMocks().RapportsStub.First(), new RapportMocks().RapportsStub.First())
            .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void AddRangeRapportList()
        {
            List<RapportEnt> rapports = new RapportMocks().RapportsStub;
            Invoking(() => Actual.AddRangeRapportList(rapports))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("RapportManager")]
        [TestCategory("CrudFeature")]
        public void GetRapportListBetweenDatesByCiList_Returns_RapportList()
        {
            List<CIEnt> cis = new CiMocks().GetFakeDbSet().ToList();
            Actual.GetRapportListBetweenDatesByCiList(cis.Select(ci => ci.CiId).ToList(), DateTime.UtcNow, DateTime.UtcNow)
            .Should().NotBeNull();
        }
    }
}
