using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace Fred.Business.Tests.Integration
{
  [TestClass]
  public class TestTypeDepense : FredBaseTest
  {
    /// <summary>
    ///   Teste la création d'un nouvel enregistrement en base de données.
    ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void AddNewTypeDepense()
    {
      TypeDepenseEnt typeDepense = new TypeDepenseEnt
      {
        Code = "100-07",
        Libelle = "Type Dépense YDY"
      };

      int countBefore = TypeDepenseMgr.GetTypeDepenseList().Count();

      TypeDepenseMgr.AddTypeDepense(typeDepense);

      int countAfter = TypeDepenseMgr.GetTypeDepenseList().Count();

      Assert.IsTrue((countBefore + 1) == countAfter);
    }

    /// <summary>
    ///   Teste la mise à jour d'un enregistrement existant en base de données.
    ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void UpdateExistingTypeDepense()
    {
      TypeDepenseEnt typeDepenseBefore = new TypeDepenseEnt
      {
        Code = "100-07",
        Libelle = "Type Dépense YDY"
      };

      int typeDepenseId = TypeDepenseMgr.AddTypeDepense(typeDepenseBefore);

      string libBefore = typeDepenseBefore.Libelle;
      typeDepenseBefore.Libelle = "Test Dépense";

      TypeDepenseMgr.UpdateTypeDepense(typeDepenseBefore);

      TypeDepenseEnt typeDepenseAfter = TypeDepenseMgr.GetTypeDepenseById(typeDepenseId);

      Assert.AreNotEqual(libBefore, typeDepenseAfter.Libelle);
    }

    /// <summary>
    ///   Teste la suppression d'un nouvel enregistrement en base de données.
    ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void DeleteExistingTypeDepense()
    {
      TypeDepenseEnt typeDepense = new TypeDepenseEnt
      {
        Code = "100-07",
        Libelle = "Type Dépense YDY"
      };

      int typeDepenseId = TypeDepenseMgr.AddTypeDepense(typeDepense);
      int countBefore = TypeDepenseMgr.GetTypeDepenseList().Count();
      TypeDepenseMgr.DeleteTypeDepenseById(typeDepenseId);
      int countAfter = TypeDepenseMgr.GetTypeDepenseList().Count();

      Assert.IsTrue(countBefore == (countAfter + 1));
    }

    /// <summary>
    ///   Teste recherche des types de dépenses
    ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void GetTypeDepenseListReturnAtLeastOneRecord()
    {

      var typesDepense = TypeDepenseMgr.GetTypeDepenseList().ToList();
      Assert.IsTrue(typesDepense.Count > 0);
    }

    /// <summary>
    ///   Teste la récupération d'un enregistrement spécifique en base de données.
    ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void GetNonExistingTypeDepenseReturnNull()
    {

      TypeDepenseEnt typeDepense = TypeDepenseMgr.GetTypeDepenseById(-1);
      Assert.IsNull(typeDepense);
    }

    /// <summary>
    ///   Teste que la liste des types de dépenses n'est jamais nulle.
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void GetTypeDepenseListReturnNotNullList()
    {

      var typesDepense = TypeDepenseMgr.GetTypeDepenseList();
      Assert.IsTrue(typesDepense != null);
    }
  }
}