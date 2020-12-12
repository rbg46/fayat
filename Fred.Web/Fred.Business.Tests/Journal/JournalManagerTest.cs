using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using FluentValidation;
using Fred.Business.Journal;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Commande.Mock;
using Fred.Common.Tests.Data.Journal.Builder;
using Fred.Common.Tests.Data.Journal.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Journal;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Fred.Web.Models.Journal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Journal
{
    /// <summary>
    /// Test du manager des Journaux comptables
    /// </summary>
    [TestClass]
    public class JournalManagerTest : BaseTu<JournalManager>
    {
        private const string notNullErrorCode = "NotNullValidator";
        private readonly JournalMocks Mocks = new JournalMocks();
        private readonly List<Expression<Func<JournalEnt, object>>> FieldsToUpdate = new List<Expression<Func<JournalEnt, object>>>();
        private Mock<FredDbContext> FakeContext;
        private List<JournalEnt> journauxFromFakeContext;

        [TestInitialize]
        public void TestInitialize()
        {
            // Mocks
            FakeContext = GetMocked<FredDbContext>();
            FakeContext.Object.Journals = Mocks.GetFakeDbSet();

            var fakeContext = GetMocked<FredDbContext>();
            fakeContext.Object.Journals = Mocks.GetFakeDbSet();
            fakeContext.Object.Commandes = new CommandeMocks().GetFakeDbSet();

            // uow instance
            var securityManager = GetMocked<ISecurityManager>();
            UnitOfWork uow = new UnitOfWork(fakeContext.Object, securityManager.Object);
            SubstituteConstructorArgument<IUnitOfWork>(uow);

            // Validator instance
            JournalValidator validator = new JournalValidator();
            SubstituteConstructorArgument<IJournalValidator>(validator);

            // Fake Mapper
            SubstituteConstructorArgument(Mocks.FakeMapper);

            // Fields to update
            FieldsToUpdate.Add(x => x.Code);
            FieldsToUpdate.Add(x => x.Libelle);
            FieldsToUpdate.Add(x => x.DateCloture);

            journauxFromFakeContext = fakeContext.Object.Journals.ToList();
        }

        [TestMethod]
        [TestCategory("JournalManager")]
        public void UpdateJournal_WithParameter_CodeTooLong()
        {
            JournalModel journal = new JournalModelBuilder().Default()
                .Code("AZER").Build();
            Invoking(() => Actual.UpdateJournal(journal, null))
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(JournalResource.LongueurCode));
        }

        [TestMethod]
        [TestCategory("JournalManager")]
        public void UpdateJournal_WithParameter_CodeNull()
        {
            JournalModel journal = new JournalModelBuilder().Default()
                .Code(null).Build();
            Invoking(() => Actual.UpdateJournal(journal, null))
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains("Code") && x.ErrorCode.Equals(notNullErrorCode));
        }

        [TestMethod]
        [TestCategory("JournalManager")]
        public void UpdateJournal_WithParameter_LibelleTooLong()
        {
            // Test du Libellé trop long (251 caractère au lieu du 250 maximum autorisé)
            JournalModel journal = new JournalModelBuilder().Default()
                .Libelle("azertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopazertyuiopa").Build();
            Invoking(() => Actual.UpdateJournal(journal, null))
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x => x.ErrorMessage.Contains(JournalResource.LongueurLibelle));
        }

        [TestMethod]
        [TestCategory("JournalManager")]
        public void UpdateJournal_WithParameter_LibelleNull()
        {
            JournalModel journal = new JournalModelBuilder().Default()
                .Libelle(null).Build();
            Invoking(() => Actual.UpdateJournal(journal, null))
            .Should().Throw<ValidationException>()
                .Which.Errors.Should().Contain(x =>
                    x.ErrorMessage.Contains("Libelle") && x.ErrorCode.Equals(notNullErrorCode));
        }

        [TestMethod]
        [TestCategory("JournalManager")]
        [Ignore]
        // Ce test est IGNORE car actuellement il est impossible de tester correctement les méthodes du "DbRepository". Lorsque Lee Vin et Yann aurons réglés cela il faudra réactiver ce test.
        public void UpdateJournal_WithOnlySomeFieldsToUpdate_ReturnsUpdatedItem()
        {
            JournalModel journal = new JournalModelBuilder().Default()
                .TypeJournal("TEST").Build();
            Actual.UpdateJournal(journal, FieldsToUpdate)
            // Test de l'objet de retour
            .Should().Match<JournalModel>(j => j.Code.Equals((journal).Code))
            .And.Should().Match<JournalModel>(j => j.Libelle.Equals((journal).Libelle))
            .And.Should().Match<JournalModel>(j => j.DateCloture.Equals((journal).DateCloture))
            .And.Should().Match<JournalModel>(j => !j.TypeJournal.Equals((journal).TypeJournal))
            // Test fake context qui a été correctement modifié par le UpdateJournal
            .And.Match<JournalModel>(j => journauxFromFakeContext.Any(x => j.JournalId == x.JournalId && j.Code.Equals(x.Code)))
            .And.Match<JournalModel>(j => journauxFromFakeContext.Any(x => j.JournalId == x.JournalId && j.Libelle.Equals(x.Libelle)))
            .And.Match<JournalModel>(j => journauxFromFakeContext.Any(x => j.JournalId == x.JournalId && j.DateCloture.Equals(x.DateCloture)))
            .And.Match<JournalModel>(j => journauxFromFakeContext.Any(x => j.JournalId == x.JournalId && !j.TypeJournal.Equals(x.TypeJournal)));
        }
    }
}
