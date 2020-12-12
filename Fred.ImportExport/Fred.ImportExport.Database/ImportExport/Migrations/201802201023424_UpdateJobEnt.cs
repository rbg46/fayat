namespace Fred.ImportExport.Database.ImportExport.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class UpdateJobEnt : DbMigration
  {
    public override void Up()
    {
      AddColumn("importExport.Jobs", "Statut", c => c.Int(nullable: false));
    }

    public override void Down()
    {
      DropColumn("importExport.Jobs", "Statut");
    }
  }
}
