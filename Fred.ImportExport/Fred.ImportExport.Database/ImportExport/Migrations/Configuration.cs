namespace Fred.ImportExport.Database.ImportExport.Migrations
{
  using System.Data.Entity.Migrations;

  /// <summary>
  /// Gestionnaire de la base de données, partie Import/Export.
  /// </summary>
  public sealed class Configuration : DbMigrationsConfiguration<Fred.ImportExport.Database.ImportExport.ImportExportContext>
  {
    /// <summary>
    /// Initialise une nouvelle instance.
    /// </summary>
    public Configuration()
    {
      AutomaticMigrationsEnabled = false;
      AutomaticMigrationDataLossAllowed = false;
      MigrationsDirectory = @"ImportExport\Migrations";
    }

    /// <summary>
    /// Permet de mettre à jour les données de la base, fonctionne après la mise à niveau vers la dernière migration.
    /// </summary>
    /// <param name="context">Le context de la base de donnée</param>
    protected override void Seed(Fred.ImportExport.Database.ImportExport.ImportExportContext context)
    {
      // nothing to do here for the moment...
    }
  }
}
