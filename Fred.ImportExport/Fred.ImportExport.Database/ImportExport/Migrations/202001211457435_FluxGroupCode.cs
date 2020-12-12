namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System.Data.Entity.Migrations;
    using Fred.Entities;

    public partial class FluxGroupCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("importExport.Flux", "GroupCode", c => c.String(nullable: false, defaultValue: Constantes.CodeGroupeDefault));
        }

        public override void Down()
        {
            DropColumn("importExport.Flux", "GroupCode");
        }
    }
}
