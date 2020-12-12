//using Fred.EntityFramework;
//using Fred.EntityFramework.Migrations;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Fred.Business.Tests.Integration
//{

//  [TestClass]
//  public class TestCodeFirst
//  {
//    /// <summary>
//    ///   Verification qu'il n'y aura pas de migration automatique lors de la mise a jour de la base par EF.
//    ///   ATTENTION, Ce test est utilisé dans la PIC.
//    /// </summary>
//    [TestMethod]
//    [TestCategory("CodeFirstVerificationMigrationFile")]
//    public void VerifyNoAutomatiqueMigrationsRequired()
//    {
//      var configuration = new Configuration();
//      // Voir code interieur pour explication des solutions possibles.
//      MigrationHelper.VerifyNoAutomatiqueMigrationsRequired(configuration);

//      Assert.IsTrue(true);
//    }


//    /// <summary>
//    ///   Crée ou modifie la base de donnée
//    /// </summary>
//    [TestMethod]
//    [TestCategory("CreateOrUpdateDatabase")]
//    public void CreateOrUpdateDatabase()
//    {
//      var configuration = new Configuration();
//      // Voir code interieur pour explication des solutions possibles.
//      MigrationHelper.VerifyNoAutomatiqueMigrationsRequired(configuration);
//      MigrationHelper.Update(configuration);
//      Assert.IsTrue(true);
//    }

//  }


//}
