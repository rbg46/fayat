using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation;
using Fred.Business.Societe.Classification;
using Fred.Business.Societe.Interfaces;
using Fred.Business.Societe.Validators;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Societe.Classification.Builder;
using Fred.Common.Tests.Data.Societe.Classification.Mock;
using Fred.Common.Tests.Data.Societe.Mock;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Societe.Classification;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Societe.Classification
{
    /// <summary>
    /// Test du manager des classifications des sociétés
    /// </summary>
    [TestClass]
    public class SocieteClassificationManagerTest : BaseTu<SocieteClassificationManager>
    {
        private const int IndexUpdatedExpected = 0;
        private SocieteClassificationMocks Mocks;

        [TestInitialize]
        public void TestInitialize()
        {
            Mocks = new SocieteClassificationMocks();

            //Setup
            var fakeRepository = GetMocked<ISocieteClassificationRepository>();
            fakeRepository.Setup(r => r.Get()).Returns(Mocks.SocietesClassificationsEntStub.AsQueryable());
            fakeRepository.Setup(r => r.GetOnlyActive()).Returns(Mocks.SocietesClassificationsEntStub.FindAll(c => c.Statut));
            fakeRepository.Setup(r => r.GetByGroupeId(It.IsAny<int>(), It.IsAny<bool?>())).Returns<int, bool?>((i, b) => Mocks.SocietesClassificationsEntStub.FindAll(c => c.GroupeId.Equals(i)));
            fakeRepository.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns<string, int, int>((s, p, n) => Mocks.SocietesClassificationsEntStub.FindAll(c => c.CodeLibelle.Contains(s)));
            fakeRepository.Setup(r => r.Insert(It.IsAny<SocieteClassificationEnt>())).Callback<SocieteClassificationEnt>(s => Mocks.SocietesClassificationsEntStub.Add(s));
            fakeRepository.Setup(r => r.Update(It.IsAny<SocieteClassificationEnt>())).Callback<SocieteClassificationEnt>(d => Mocks.SocietesClassificationsEntStub.ForEach(s => { if (d.SocieteClassificationId == s.SocieteClassificationId) d = s; }));
            fakeRepository.Setup(r => r.PopulateSocietes(It.IsAny<SocieteClassificationEnt>())).Returns<SocieteClassificationEnt>(c => c);
            //Real Validator     
            var SocieteClassificationValidator = new SocieteClassificationValidator();
            SubstituteConstructorArgument<ISocieteClassificationValidator>(SocieteClassificationValidator);
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void GetAll_ReturnsCompleteList()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub).Build();
            Actual.GetAll(false)
            .Should().HaveCount(classifications.Count());
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void GetAll_OnlyActif_ReturnsActiveList()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub).Build();
            Actual.GetAll(true)
            .Should().OnlyContain(c => c.Statut);
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void GetByGroupeId_ReturnsGroupeList()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub).Build();
            Actual.GetByGroupeId(SocieteMocks.GroupeIdGRZB, true)
            .Should().HaveCount(classifications.Count());
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void Search_ReturnsExpectedList()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub).Build();
            Actual.Search(Mocks.SocietesClassificationsEntStub[IndexUpdatedExpected].Libelle, 1, 20)
            .Should().Contain(c => c.SocieteClassificationId.Equals(classifications[IndexUpdatedExpected].SocieteClassificationId));
        }


        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void CreateOrUpdateRange_WhenAdd_ReturnsUpdatedList()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .AddItem(new SocieteClassificationBuilder().Prototype())
                .Build();
            Actual.CreateOrUpdateRange(classifications)
            .Should().HaveCount(classifications.Count());
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void DeleteRange_RemoveItemsFromList()
        {
            var ToDelete = new List<SocieteClassificationEnt> { Mocks.ClassificationAdded };

            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .AddItem(new SocieteClassificationBuilder().Prototype())
                .Do(c => GetMocked<ISocieteClassificationRepository>().Setup(r => r.DeleteById(It.IsAny<int>())).Callback<object>(i => c.RemoveAll(r => r.SocieteClassificationId.Equals(i))))
                .Build();
            Actual.DeleteRange(ToDelete);
            classifications
            .Should().NotContain(c => c.SocieteClassificationId.Equals(Mocks.ClassificationAdded.SocieteClassificationId));
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void CreateOrUpdateRange_WhenAddFail_ReturnsException()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .AddItem(new SocieteClassificationBuilder().New())
                .Build();
            Invoking(() => Actual.CreateOrUpdateRange(classifications))
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureSociete.SocieteClassification_Code_Required));
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void CreateOrUpdateRange_WhenUpdate_ReturnsUpdatedList()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .Do(c => c[IndexUpdatedExpected].Libelle = "Libellé Modifié")
                .Build();
            Actual.CreateOrUpdateRange(classifications)
            .Should().Contain(c => c.Libelle.Equals("Libellé Modifié", StringComparison.Ordinal));
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void CreateOrUpdateRange_WhenDisablingFail_ReturnsException()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .Do(c => c[IndexUpdatedExpected].Statut = false)
                .Build();
            Invoking(() => Actual.CreateOrUpdateRange(classifications))
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureSociete.SocieteClassification_Societes_Disable_NotEmpty));
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void DeleteRange_WhenRemoveFail()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .Do(c => c.Add(Mocks.SocietesClassificationsEntStub[IndexUpdatedExpected]))
                .Build();
            Invoking(() => Actual.DeleteRange(classifications))
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureSociete.SocieteClassification_Societes_Delete_NotEmpty));
        }

        [TestMethod]
        [TestCategory("SocieteClassificationManager")]
        public void CreateOrUpdateRange_Add_WithCodeAlreadyUsed_ReturnsException()
        {
            List<SocieteClassificationEnt> classifications = new SocietesClassificationsBuilder(Mocks.SocietesClassificationsEntStub)
                .AddItem(new SocieteClassificationBuilder()
                    .Code(Mocks.SocietesClassificationsEntStub[IndexUpdatedExpected].Code)
                    .Libelle("Classification avec Code déjà existant")
                    .Build())
                .Build();
            Invoking(() => Actual.CreateOrUpdateRange(classifications))
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(FeatureSociete.SocieteClassification_Code_AlreadyUsed));
        }
    }
}
