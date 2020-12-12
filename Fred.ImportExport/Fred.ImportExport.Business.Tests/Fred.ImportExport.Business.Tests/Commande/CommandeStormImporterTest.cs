using FluentAssertions;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Commande.Builder;
using Fred.ImportExport.Business.Commande;
using Fred.ImportExport.Business.Tests.Commande.Builder;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Commande;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.ImportExport.Business.Tests.Commande
{
    [TestClass]
    public class CommandeStormImporterTest : BaseTu<CommandeStormImporter>
    {
        private CommandeSapModelBuilder commandeSapModelBuilder = null;
        private CommandeLigneSapModelBuilder commandeLigneSapModelBuilder = null;

        [TestInitialize]
        public void Initialize()
        {
            commandeSapModelBuilder = new CommandeSapModelBuilder();
            commandeLigneSapModelBuilder = new CommandeLigneSapModelBuilder();
        }

        [TestMethod]
        [Ignore]
        [TestCategory("CommandeStormImport")]
        public void ImportModel_AvecLignesEmpty_RenvoieErreur()
        {
            WithArrange(commandeSapModelBuilder.Build())
            .Invoking(() => Actual.Import(ArrangedObject as CommandeSapModel))
            .Should().Throw<FredIeBusinessException>();
        }

        [TestMethod]
        [Ignore]
        [TestCategory("CommandeStormImport")]
        public void ImportModel_Numero_EstAssocieANumeroCommandeExterne()
        {
            WithArrange(
                commandeSapModelBuilder
                .Numero("testnumero")
                .AddLigne(commandeLigneSapModelBuilder.New())
                .Build())
            .Invoking(() => Actual.Import(ArrangedObject as CommandeSapModel))
            .Should().NotThrow();
        }

        [TestMethod]
        [Ignore]
        [TestCategory("CommandeStormImport")]
        public void ImportModel_CommandeLigneSap_EstAssocieANumeroCommandeLigneExterne()
        {
            WithArrange(
                commandeSapModelBuilder
                .AddLigne(commandeLigneSapModelBuilder.CommandeLigneSap("commandeLigneSap").Build())
                .Build())
            .Invoking(() => Actual.Import(ArrangedObject as CommandeSapModel))
            .Should().NotThrow();
        }
    }
}
