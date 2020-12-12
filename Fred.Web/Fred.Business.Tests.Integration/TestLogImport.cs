using Fred.DataAccess.LogImport;
using Fred.Entities.LogImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
namespace Fred.Business.Tests.Integration
{
  [TestClass]
  public class TestLogImport : FredBaseTest
  {
    private static LogImportEnt logImport;

    /// <summary>
    ///   Initialise l'ensemble des tests de la classe.
    /// </summary>
    /// <param name="context">Le contexte de tests.</param>
    [ClassInitialize]
    public static void InitAllTests(TestContext context)
    {
      logImport = new LogImportEnt
      {
        TypeImport = "testLog",
        DateImport = new DateTime(2016, 12, 21),
        MessageErreur = "ceci est un test d'enregistrement d'erreur",
        Data = "Ici ce sont les données de l'erreur"
      };
    }

    #region Log Import

    /// <summary>
    ///   Teste que la liste des LogImports retournée n'est jamais égale à null
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void GetLogImportList()
    {
      var resu = LogImportRepository.GetAllLogImport();
      Assert.IsTrue(resu != null);
    }

    /// <summary>
    ///   Teste l'ajout d'un log import
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void AddLogImport()
    {
      int resu = 0;
      resu = LogImportRepository.Add(logImport);
      Assert.IsTrue(resu > 0);
    }

    /// <summary>
    ///   Teste la recupération de log import par type d'import
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void GetLogImportByType()
    {
      var resu = LogImportRepository.GetLogImportByType("testLog");
      Assert.IsTrue(resu != null && resu.Any());
    }

    /// <summary>
    ///   Teste la recupération de log import par type d'import
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void GetLogImportByDate()
    {
      var resu = LogImportRepository.GetLogImportByDate(new DateTime(2016, 12, 21));
      Assert.IsTrue(resu != null && resu.Any());
    }

    #endregion
  }
}