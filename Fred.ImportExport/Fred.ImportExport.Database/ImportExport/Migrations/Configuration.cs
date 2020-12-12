namespace Fred.ImportExport.Database.ImportExport.Migrations
{
  using System.Data.Entity.Migrations;

  /// <summary>
  /// Gestionnaire de la base de donn�es, partie Import/Export.
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
    /// Permet de mettre � jour les donn�es de la base, fonctionne apr�s la mise � niveau vers la derni�re migration.
    /// </summary>
    /// <param name="context">Le context de la base de donn�e</param>
    protected override void Seed(Fred.ImportExport.Database.ImportExport.ImportExportContext context)
    {
      // nothing to do here for the moment...
    }
  }
}
