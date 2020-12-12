using System;
using FluentAssertions;
using Fred.Business.Commande.Reporting;
using Fred.Business.Images;
using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Commande.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Entities.Commande;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Fred.Entities.Constantes;
using Fred.Entities.Societe;
using Moq;
using Fred.Entities.Groupe;

namespace Fred.Business.Tests.Commande.Reporting
{
    [TestClass]
    public class CommandeExportServiceTest : BaseTu<CommandeExportService>
    {
        private CommandeBuilder CommandeBuilder { get; set; }
        private SocieteBuilder SocieteBuilder { get; set; }
        private Mock<IPersonnelImageManager> PersonnelImageManagerMock { get; set; }
        private Mock<IImageManager> ImageManagerMock { get; set; }
        private Mock<IUtilisateurManager> UserManagerMock { get; set; }

    [TestInitialize]
        public void TestInitialize()
        {
            CommandeBuilder = new CommandeBuilder();
            SocieteBuilder = new SocieteBuilder();

            PersonnelImageManagerMock = GetMocked<IPersonnelImageManager>();
            ImageManagerMock = GetMocked<IImageManager>();
            UserManagerMock = new Mock<IUtilisateurManager>();
            var user = new UtilisateurEnt
            {
                Personnel = new PersonnelEnt
                {
                    Societe = new SocieteEnt
                    {
                        Groupe = new GroupeEnt
                        {
                            Code = CodeGroupeFTP
                        }
                    }
                }
            };
            UserManagerMock.Setup(u => u.GetContextUtilisateur()).Returns(user);
        }

        [TestMethod]
        [TestCategory("CommandeExport")]
        public void Convert_WithNullParameters_RenvoieException()
        {
            Invoking(() => Actual.Convert(null, null, null, null, null))
            .Should().Throw<NullReferenceException>();
        }

        /// <summary>
        ///   Teste que lorsque la commande possède un numéro externe, le modèle contient bien ce numéro
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeExport")]
        public void Convert_WithNumeroCommandeExterne_NumeroEqualsNumeroExterne()
        {
            CommandeEnt commande =
                CommandeBuilder
                .NumeroExterne("numéro externe")
                .Build();
            Actual.Convert(commande, SocieteBuilder.Build(), PersonnelImageManagerMock.Object, ImageManagerMock.Object, UserManagerMock.Object)
            .Numero.Should().Be(commande.NumeroCommandeExterne);
        }

        /// <summary>
        ///   Teste que lorsque la commande ne possède pas de numéro externe, le modèle contient le numéro de commande
        /// </summary>
        [TestMethod]
        [TestCategory("CommandeExport")]
        public void Convert_WithEmptyNumeroCommandeExterne_NumeroEqualsNumero()
        {
            CommandeEnt commande =
                CommandeBuilder
                .Numero("Numéro")
                .NumeroExterne(string.Empty)
                .Build();
            Actual.Convert(commande, SocieteBuilder.Build(), PersonnelImageManagerMock.Object, ImageManagerMock.Object, UserManagerMock.Object)
            .Numero.Should().Be(commande.Numero);
        }
    }
}
