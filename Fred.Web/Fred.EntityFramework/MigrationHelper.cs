using System.Linq;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework
{
    public static class MigrationHelper
    {
        private static string fayatDatabaseServerName = "filibfred";

        public static void AssertTargetDatabaseCanMigrate(DbContext context)
        {
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();

#if (DEBUG || SPEEDDEBUG)                                                   
            if (pendingMigrations.Any())
            {
                var dbConnection = context.Database.GetDbConnection();
                if (dbConnection.ConnectionString.Contains(fayatDatabaseServerName))
                    throw new FredTechnicalException("ATTENTION : tu es en train d'envoyer une migration sur un serveur et pas sur ta base locale ! : " + dbConnection.ConnectionString);
            }
#endif
        }

        public static void MigrateToLatestVersion(DbContext context)
        {
            var pendingMigrations = context.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                var migrator = context.Database.GetService<IMigrator>();

                foreach (var pendingMigration in pendingMigrations)
                    migrator.Migrate(pendingMigration);
            }
        }
    }
}
