using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Fred.Business.Commande.Services;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Commande.Builder;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Commande
{
    [TestClass]
    public class CommandeFluxServiceTest : BaseTu<CommandeFluxService>
    {
        private CommandeBuilder CommandeBuilder { get; set; }

        private CommandeLigneBuilder CommandeLigneBuilder { get; set; }


        [TestInitialize]
        public void TestInitialize()
        {
            //Initialisation des builders
            CommandeBuilder = new CommandeBuilder();
            CommandeLigneBuilder = new CommandeLigneBuilder();
        }

        /// <summary>
        ///   Teste que lorsque la commande possède un avenant ayant un numéro 0 que cet avenant est enlevé lors de l'export vers SAP
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeAvenantExport")]
        public async Task ExportCommandeAvenantToSap_WithNumeroAvenant1_ShouldNotReturnAvenantWithNumero0()
        {
            //1. Preparation des donnees

            // Mocker le repository des commandes
            var commandeRepositoryFake = GetMocked<ICommandeRepository>();

            // Mocker méthode
            commandeRepositoryFake.Setup(m => m.GetCommandeAvenantSAPAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(
                    CommandeBuilder.AddLignes(
                        new List<CommandeLigneEnt>() {
                            CommandeLigneBuilder.AvenantLigne(0).Build()
                        })
                    .Build()
               );

            // Init commandeFluxService
            CommandeFluxService commandeFluxService = new CommandeFluxService(commandeRepositoryFake.Object);

            //2. Definition action
            var commande = await Actual.GetCommandeAvenantSAPAsync(It.IsAny<int>(), 1, It.IsAny<string>());

            //3. Assertion
            commande.Lignes.Should().BeEmpty();
        }
    }
}
