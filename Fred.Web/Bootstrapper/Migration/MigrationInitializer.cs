using Fred.EntityFramework;

namespace Bootstrapper.Migration
{
    public static class MigrationInitializer
    {
        public static void Initialize()
        {
            var context = new FredDbContext();
            MigrationHelper.AssertTargetDatabaseCanMigrate(context);
            MigrationHelper.MigrateToLatestVersion(context);
        }
    }
}
