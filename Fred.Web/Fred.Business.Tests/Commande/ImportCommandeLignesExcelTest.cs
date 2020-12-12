using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Fred.Business.CI;
using Fred.Business.Commande.Services;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielFixe;
using Fred.Business.Unite;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.CI.Mock;
using Fred.Common.Tests.Data.Commande.Fake;
using Fred.Common.Tests.Data.Referential.Tache.Builder;
using Fred.Common.Tests.Data.ReferentielFixe.Mock;
using Fred.Common.Tests.Data.Unite.Builder;
using Fred.Entities.CI;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static System.IO.File;

namespace Fred.Business.Test.Commande
{
    /// <summary>
    /// Test Import Lignes commane Lignes Excel
    /// </summary>

    [TestClass]
    public class ImportCommandeLignesExcelTest : BaseTu<CommandeImportExportExcelService>
    {
        private readonly CiMocks ciMock = new CiMocks();
        private readonly RessourceMocks ressourceMocks = new RessourceMocks();

        private string checkinValueAvenant = string.Empty;
        private string checkinValueCommande = string.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            var uniteManager = GetMocked<IUniteManager>();
            var cIManager = GetMocked<ICIManager>();
            var tacheManager = GetMocked<ITacheManager>();
            var referentielFixeManager = GetMocked<IReferentielFixeManager>();

            checkinValueAvenant = "F000403945"; //le numéro de la commande est celui du numéro commande du fichier Excel Avanant.xslx
            checkinValueCommande = null; //la date est celui de la date du fichier Excel Commande.xslx

            //Arrange
            cIManager.Setup(m => m.GetCIById(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(ciMock.GetFakeList().FirstOrDefault());

            uniteManager.Setup(m => m.SearchLight(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new UniteBuilder().UniteId(1).Code("FRT").Libelle("Forfait").BuildNObjects(1, true));

            tacheManager.Setup(m => m.GetTacheListByCiId(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(new TacheBuilder().TacheId(1).Code("00").Libelle("Tache par defaut").BuildNObjects(1, true));

            referentielFixeManager.Setup(m => m.SearchRessourcesRecommandees(It.IsAny<string>(), It.IsAny<CIEnt>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), null))
                .Returns(ressourceMocks.GetFakeList());
            var fakeMapper = new CommandeFake().FakeMapper;
            SubstituteConstructorArgument(fakeMapper);
        }

        /// <summary>
        ///   Teste si fichier Format correct
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesCommandeExcel_FichierInvalide_RenvoieException()
        {
            //Act
            Invoking(() => Actual.ImportCommandeLignes(checkinValueCommande, 0, null, false))
            //Assert
            .Should().Throw<FredBusinessException>().Which.Message.Should().Be(
                FeatureExportExcel.Invalid_FileFormat
            );
        }

        ///<summary>
        ///    Test désignation Vide
        ///    RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesCommandeExcel_ReadFile_LibelleVide()
        {
            Stream stream = ReadFileStream("commandeLibelleVide.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueCommande, 0, stream, false)
                .ErrorMessages.Should().Match(x => x.Any(l => l.Count == 1 && l.Any(err
                      => err.Contains(string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_ChampObligatoire, FeatureCommande.Commande_Detail_Ligne_Entete_Libelle)))));
        }

        /// <summary>
        /// Test Ressource invalide % CI
        /// RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesCommandeExcel_ReadFile_RessourceErrone()
        {
            Stream stream = ReadFileStream("commandeRessourceErrone.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueCommande, 0, stream, false)
                .ErrorMessages.Should().Match(x => x.Any(l => l.Count == 1 && l.Any(err
                    => err.Contains(FeatureCommande.Commande_Detail_Ligne_Entete_Ressource))));
        }

        /// <summary>
        /// Test Tache invalide % CI
        /// RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesCommandeExcel_ReadFile_TacheErrone()
        {
            Stream stream = ReadFileStream("commandeTacheErrone.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueCommande, 0, stream, false)
                .ErrorMessages.Should().Match(x => x.Any(l => l.Count == 1 && l.Any(err
                   => err.Contains(FeatureCommande.Commande_Detail_Ligne_Entete_Tache))));
        }

        /// <summary>
        /// Test Unite invalide % CI
        /// RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesCommandeExcel_ReadFile_UniteErrone()
        {
            Stream stream = ReadFileStream("commandeUniteErrone.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueCommande, 0, stream, false)
                .ErrorMessages.Should().Match(x => x.Any(l => l.Count == 1 && l.Any(err
                   => err.Contains(FeatureCommande.Commande_Detail_Ligne_Entete_Unite))));
        }

        /// <summary>
        /// Test Prix UH > 0 
        /// RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesCommandeExcel_ReadFile_PUzero()
        {
            Stream stream = ReadFileStream("commandePUzero.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueCommande, 0, stream, false)
                .ErrorMessages.Should().Match(x => x.Any(l => l.Any(err
                    => err.Contains(FeatureCommande.Commande_Detail_Ligne_Entete_PUHT))));
        }

        /// <summary>
        /// Test Quantite UH > 0 
        /// RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesCommandeExcel_ReadFile_Quantitezero()
        {
            Stream stream = ReadFileStream("commandeQuantitezero.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueCommande, 0, stream, false)
                .ErrorMessages.Should().Match(x => x.Any(l => l.Count == 1 && l.Any(err
                   => err.Contains(FeatureCommande.Commande_Detail_Ligne_Entete_Quantite))));
        }

        /// <summary>
        /// Test Lignes Commande Valide
        /// RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesCommandeExcel_ReadFile_Valide()
        {
            Stream stream = ReadFileStream("commandeValide.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueCommande, 0, stream, false)
                .ErrorMessages.Should().Match(x => !x.Any());
        }

        //------------------Test Avenant 

        /// <summary>
        ///   Test Numero commande Vide
        ///    RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesAvenantExcel_ReadFile_NumCommandeVide()
        {
            Stream stream = ReadFileStream("AvenantNumCommandeVide.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueAvenant, 0, stream, true)
                .ErrorMessages.Should().Match(x => x.Any(l => l.Count == 1 && l.Any(err
                      => err.Contains(FeatureCommande.Commande_Index_Table_NumeroCommande))));
        }

        /// <summary>
        ///    Test Numero commande celui de la commande
        ///    RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesAveantExcel_ReadFile_NumCommandeDiff()
        {
            Stream stream = ReadFileStream("AvenantNumCommandeDiff.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueAvenant, 0, stream, true)
                .ErrorMessages.Should().Match(x => x.Any(l => l.Count == 1 && l.Any(err
                   => err.Contains(FeatureCommande.Commande_Popin_ImportLignes_Erreur_NumCommande))));
        }

        /// <summary>
        ///    Test Format Diminution
        ///    RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesAvenantExcel_ReadFile_FormatDiminution()
        {
            Stream stream = ReadFileStream("AvenantFormatDiminution.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueAvenant, 0, stream, true)
                .ErrorMessages.Should().Match(x => x.Any(l => l.Count == 1 && l.Any(err
                    => err.Contains(FeatureCommande.Commande_Detail_Ligne_Entete_Diminution))));
        }

        /// <summary>
        /// Test Lignes Commande Valide
        /// RG_8101_007 
        /// </summary>
        [TestMethod]
        [TestCategory("ImportCommandeLignesExcel")]
        public void ImportLignesAvenantExcel_ReadFile_Valide()
        {
            Stream stream = ReadFileStream("AvenantValide.xlsx");
            //Act
            Actual.ImportCommandeLignes(checkinValueAvenant, 0, stream, true)
                .ErrorMessages.Should().Match(x => !x.Any());
        }

        private Stream ReadFileStream(string filename)
        {
            string strDoc = AppDomain.CurrentDomain.BaseDirectory + @"\Commande\ExempleExcel\" + filename;
            return Open(strDoc, FileMode.Open);
        }
    }
}
