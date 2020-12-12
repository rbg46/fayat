using System;
using System.Collections.Generic;
using FluentAssertions;
using Fred.Business.Budget.BudgetComparaison;
using Fred.Business.Budget.BudgetComparaison.Dto;
using Fred.Business.Budget.BudgetComparaison.Dto.Comparaison;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Budget.BudgetComparaison.Dto.Comparaison;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Budget.BudgetComparaison
{
    [TestClass]
    public class BudgetComparerTest : BaseTu<BudgetComparer>
    {
        Mock<IBudgetRepository> BudgetRepositoryFake;
        Mock<IBudgetSousDetailRepository> BudgetSousDetailRepository;
        BudgetEnt ExpectedBudget;

        [TestInitialize]
        public void Initialize()
        {
            ExpectedBudget = new BudgetEnt { BudgetId = 1, CiId = 1, DeviseId = 1 };
            BudgetRepositoryFake = GetMocked<IBudgetRepository>();
            BudgetRepositoryFake.Setup(c => c.GetBudget(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(ExpectedBudget);
            BudgetSousDetailRepository = GetMocked<IBudgetSousDetailRepository>();
            BudgetSousDetailRepository.Setup(r => r.GetSousDetailPourBudgetComparaison(It.IsAny<int>()))
                .Returns(new List<SousDetailDao>());
        }

        [TestMethod]
        [TestCategory("BudgetComparer")]
        public void Compare_WithNewRequestDto_Returns_ResultDtoNotNull()
        {
            Actual.Compare(new RequestDto()).Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("BudgetComparer")]
        [Description("La méthode compare renvoie une exception si l'argument est null")]
        public void Compare_WithNullRequestDto_throwException()
        {
            Invoking(() => Actual.Compare(null)).Should().Throw<NullReferenceException>();
        }

        [TestMethod]
        [TestCategory("BudgetComparer")]
        public void Compare_WithValideRequestDtoAndGetBudgetNull_Returns_Error()
        {
            var builder = new RequestDtoBuilder();

            Actual.Compare(builder.Axes(new List<AxeType>()).Build())
                .Erreur.Should().Contain(string.Format(FeatureBudgetComparaison.BudgetComparaison_Erreur_BudgetNonTrouve, 1));
        }
    }
}
