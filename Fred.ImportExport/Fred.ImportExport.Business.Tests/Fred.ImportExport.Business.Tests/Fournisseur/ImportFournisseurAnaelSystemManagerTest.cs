using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Fred.Business;
using Fred.Common.Tests;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.ImportExport.Business.Tests.Fournisseur
{
    [TestClass]
    public class ImportFournisseurAnaelSystemManagerTest : BaseTu<ImportFournisseurAnaelSystemManager>
    {
        private string regleGestions;
        private List<string> societeCodeAnael;
        private string societeModel;

        [TestInitialize]
        public void Initialize()
        {
            regleGestions = "'F'";
            societeCodeAnael = new List<string> { "1000" }; // Par défaut : société Razel-Bec
            societeModel = "1";

            //J'utilise le vrai extractor service
            SubstituteProperty<IExcelFournisseurExtractorService>(new ExcelFournisseurExtractorService());
        }

        [TestMethod]
        public void ImportFournisseursFromAnaelAndSendToSap_WithNoExcelFile_ThrowException()
        {
            //Arrange
            WithArrange(new ImportFournisseursByExcelInputs()
            {
                CodeSocietes = societeCodeAnael,
                ModeleSociete = societeModel,
                RegleGestion = regleGestions,
                ExcelStream = null
            })
            //Act
            .Invoking(() => Actual.ImportFournisseurByExcel(ArrangedObject as ImportFournisseursByExcelInputs))
            //Assert
            .Should().Throw<FredBusinessException>().WithMessage(BusinessResources.RepriseDonnees_Erreur_Extraction_Import_Fournisseurs);
        }

        [TestMethod]
        public void ImportFournisseursFromAnaelAndSendToSap_WithInvalidExcelFile_ThrowException()
        {
            Stream stream = ReadFileStream("ImportFournisseurs_invalide.xlsx");

            //Arrange
            WithArrange(new ImportFournisseursByExcelInputs()
            {
                CodeSocietes = societeCodeAnael,
                ModeleSociete = societeModel,
                RegleGestion = regleGestions,
                ExcelStream = stream
            })
            //Act
            .Invoking(() => Actual.ImportFournisseurByExcel(ArrangedObject as ImportFournisseursByExcelInputs))
            //Assert
            .Should().Throw<FredBusinessException>().WithMessage(BusinessResources.RepriseDonnees_Erreur_Extraction_Import_Fournisseurs);
        }

        private Stream ReadFileStream(string filename)
        {
            string strDoc = AppDomain.CurrentDomain.BaseDirectory + @"\Fournisseur\Template\" + filename;
            return File.Open(strDoc, FileMode.Open);
        }
    }
}
