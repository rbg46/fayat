using Fred.ImportExport.Database;

namespace Fred.ImportExport.Bootstrapper.Migration
{
    /// <summary>
    /// Gestionnaire de configuration pour la migration de la base de données.
    /// </summary>
    public static class MigrationInitializer
    {
        public static void Initialize()
        {
            // Initialisation de la migration pour la partie Import/Export
            var configurationIE = new Database.ImportExport.Migrations.Configuration();
            MigrationHelper.VerifyNoAutomatiqueMigrationsRequired(configurationIE);
            MigrationHelper.Update(configurationIE);

            // Initialisation de la migration pour la partie sécurité
            var configurationIdentity = new Database.Securite.Migrations.Configuration();
            MigrationHelper.VerifyNoAutomatiqueMigrationsRequired(configurationIdentity);
            MigrationHelper.Update(configurationIdentity);

            // Initialisation de la migration pour la partie STAIR
            var configurationStair = new Database.ImportExport.StairMigrations.Configuration();
            MigrationHelper.VerifyNoAutomatiqueMigrationsRequired(configurationStair);
            MigrationHelper.Update(configurationStair);
        }
    }
}
