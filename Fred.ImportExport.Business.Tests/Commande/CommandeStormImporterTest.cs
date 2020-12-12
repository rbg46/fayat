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
        [TestCategory("CommandeStormImport")]
        public void ImportModel_AvecLignesEmpty_RenvoieErreur()
        {
            CommandeSapModel commande = commandeSapModelBuilder.Build();
            Invoking(() => Actual.Import(commande))
            .Should().Throw<FredIeBusinessException>();
        }

        [TestMethod]
        [TestCategory("CommandeStormImport")]
        [Ignore] //lazy servicelocator à résoudre avant
        public void ImportModel_Numero_EstAssocieANumeroCommandeExterne()
        {
            Invoking(() => Actual.Import(commandeSapModelBuilder
                .Numero("testnumero")
                .AddLigne(commandeLigneSapModelBuilder.New())
                .Build()))
            .Should().NotThrow();
        }

        [TestMethod]
        [TestCategory("CommandeStormImport")]
        [Ignore] //lazy servicelocator à résoudre avant
        public void ImportModel_CommandeLigneSap_EstAssocieANumeroCommandeLigneExterne()
        {
            Invoking(() => Actual.Import(commandeSapModelBuilder
                .AddLigne(commandeLigneSapModelBuilder.CommandeLigneSap("commandeLigneSap").Build())
                .Build()))
            .Should().NotThrow();
        }
    }
}
