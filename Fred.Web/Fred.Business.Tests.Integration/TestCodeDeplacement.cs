using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
  [TestClass]
  public class TestCodeDeplacement : FredBaseTest
  {
    //private ICodeDeplacementManager GetManager(IUnitOfWork uow = null)
    //{
    //  uow = uow ?? new UnitOfWork(new FredDbContext());
    //  return new CodeDeplacementManager(new CodeDeplacementValidator(), uow);
    //}

    ///// <summary>
    ///// Teste que la liste des codes majoration retournée n'est jamais égale à null
    ///// </summary>
    //[TestMethod]
    //[TestCategory("DBDepend")]
    //public void GetCodeDepListReturnNotNullList()
    //{
    //  int societeId = 1;
    //  var codeDepMgr = GetManager();
    //  var societes = codeDepMgr.GetCodeDeplacementList(societeId);
    //  Assert.IsTrue(societes != null);
    //}

    ///// <summary>
    ///// Teste que la liste des codes majoration retournée n'est jamais égale à null
    ///// </summary>
    //[TestMethod]
    //[TestCategory("DBDepend")]
    //public void AddCodeDeplacement()
    //{
    //  var uow = new UnitOfWork(new FredDbContext());
    //  var codeDepMgr = GetManager(uow);
    //  var societe = uow.GenericRepository<SocieteEnt>().Query().Get().FirstOrDefault();
    //  var codeDep = new CodeDeplacementEnt()
    //  {
    //    Code = "XTU",
    //    Libelle = "Code Dep TU",
    //    KmMini = 1,
    //    KmMaxi = 10,
    //    IGD = true,
    //    Actif = true,
    //    SocieteId = societe.SocieteId
    //  };
    //  var id = codeDepMgr.AddCodeDeplacement(codeDep);
    //  var codeDep2 = codeDepMgr.GetCodeDeplacementById(id);
    //  Assert.IsTrue(codeDep2 != null);
    //}

    /// <summary>
    /// Teste que la liste des codes majoration retournée n'est jamais égale à null
    /// </summary>
    //[TestMethod]
    //[TestCategory("DBDepend")]
    //public void UpdateCodeDeplacement()
    //{
    //  var uow = new UnitOfWork(new FredDbContext());
    //  var codeDepMgr = GetManager(uow);
    //  var societe = uow.GenericRepository<SocieteEnt>().Query().Get().First();
    //  CodeDeplacementEnt codeDep = new CodeDeplacementEnt()
    //  {
    //    Code = "XTU",
    //    Libelle = "Code Dep TU",
    //    KmMini = 1,
    //    KmMaxi = 10,
    //    IGD = true,
    //    Actif = true,
    //    SocieteId = societe.SocieteId
    //  };
    //  int id = codeDepMgr.AddCodeDeplacement(codeDep);
    //  string libBefore = codeDep.Libelle;
    //  codeDep.Libelle = "Code Dep TU 2";
    //  codeDepMgr.UpdateCodeDeplacement(codeDep);
    //  CodeDeplacementEnt codeDepAfter = codeDepMgr.GetCodeDeplacementById(id);
    //  Assert.AreNotEqual(libBefore, codeDepAfter.Libelle);
    //}

    ///// <summary>
    ///// Teste que la liste des codes majoration retournée n'est jamais égale à null
    ///// </summary>
    //[TestMethod]
    //[TestCategory("DBDepend")]
    //public void DeleteCodeDeplacementById()
    //{
    //  var uow = new UnitOfWork(new FredDbContext());
    //  var codeDepMgr = GetManager(uow);
    //  var societe = uow.GenericRepository<SocieteEnt>().Query().Get().First();
    //  CodeDeplacementEnt codeDep = new CodeDeplacementEnt()
    //  {
    //    Code = "XTU",
    //    Libelle = "Code Dep TU",
    //    KmMini = 1,
    //    KmMaxi = 10,
    //    IGD = true,
    //    Actif = true,
    //    SocieteId = societe.SocieteId
    //  };
    //  int codeDepId = codeDepMgr.AddCodeDeplacement(codeDep);
    //  int countBefore = codeDepMgr.GetCodeDeplacementList(societe.SocieteId).Count();
    //  Assert.IsTrue(codeDepMgr.DeleteCodeDeplacementById(codeDep));
    //  int countAfter = codeDepMgr.GetCodeDeplacementList(societe.SocieteId).Count();
    //  Assert.IsTrue(countBefore == countAfter + 1);
    //}
  }
}