using Fred.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Reflection;

namespace Fred.ImportExport.Database
{
  /// <summary>
  /// Utilitaire pour la migration code first
  /// </summary>
  public static class MigrationHelper
  {

    /// <summary>
    /// Permet de verifier s'il y a une migration automatique qui va s'effectuer sur la base.
    /// Soulève une exception si c'est la cas.
    /// </summary>
    /// <param name="configuration">La configuration utilisée pour procéder à la migration.</param>
    public static void VerifyNoAutomatiqueMigrationsRequired(DbMigrationsConfiguration configuration)
    {
      configuration.AutomaticMigrationDataLossAllowed = true;
      configuration.AutomaticMigrationsEnabled = true;
      var migrator = new DbMigrator(configuration);

      // 1) Test : envoie d'une migration autre que sur la base locale
#if (DEBUG || SPEEDDEBUG)                                                   // En mode Debug (car l'application est compilée en release sur Tfs). C'est moyen comme test : peut être passant si le développeur est en release
      if (migrator.GetPendingMigrations().Any())                            // S'il y a une migration à effectuer
      {
        var dbContextInfo = new System.Data.Entity.Infrastructure.DbContextInfo(configuration.ContextType);
        if (dbContextInfo.ConnectionString.Contains("filibfred"))  // Si la base de donnée est une base sur un serveur Fred
        {
          throw new FredTechnicalException("ATTENTION : tu es en train d'envoyer une migration sur un serveur et pas sur ta base locale ! : " + dbContextInfo.ConnectionString);
        }
      }
#endif

       // 2) Test : Automatic Migration
      var scriptor = new MigratorScriptingDecorator(migrator);
      var script = scriptor.ScriptUpdate(sourceMigration: null, targetMigration: null);
      if (script.Contains("_AutomaticMigration"))
      {
        // SI VOUS ETES ICI C EST QUE SOIT : 
        // - VOUS N AVEZ PAS CREE LE FICHIER DE MIGRATIONS CORRESPONDANT A VOTRE MODEL.
        //   LA SOLUTION CONSITE A LANCER LA COMMANDE Add-Migration suivi de votre nom (voir readme.md dans le meme projet)
        // - VOUS AVEZ RECUPERE DU CODE QUI CONTIENT UN FICHIER DE MIGRATION ANTERIEUR AU VOTRE.
        //   LA SOLUTION CONSITE A REVENIR A L ETAT INFERIEUR AU VOTRE AU NIVEAU DES FICHIERS DE MIGRATION
        //   update-database -TargetMigration:"*************NOM DU FICHIER DE MIGRATION ANTERIEUR***************"
        //   PUIS DE RECREER VOTRE FICHIER DE MIGRATION
        //   update-database -TargetMigration:"*************NOM DE VOTRE FICHIER DE MIGRATION***************"
        throw new FredTechnicalException("Une migration automatique n'est pas autorisée.\n");
      }
    }

    /// <summary>
    /// Permet de migrer la base de donnée.
    /// </summary>
    /// <param name="configuration">La configuration utilisée pour procéder à la migration.</param>
    public static void Update(DbMigrationsConfiguration configuration)
    {
      configuration.AutomaticMigrationsEnabled = false;
      configuration.AutomaticMigrationDataLossAllowed = false;
      var migrator = new DbMigrator(configuration);
      migrator.Update();
    }

  }
}
