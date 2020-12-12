namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteColumnConnexionChaineSourceAndAddIndexUniqueCodeInTableFluxEntMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("importExport.Flux", "Code", c => c.String(nullable: false, maxLength: 150));
            CreateIndex("importExport.Flux", "Code", unique: true, name: "UniqueCode");
            DropColumn("importExport.Flux", "ConnexionChaineSource");
        }
        
        public override void Down()
        {
            AddColumn("importExport.Flux", "ConnexionChaineSource", c => c.String());
            DropIndex("importExport.Flux", "UniqueCode");
            AlterColumn("importExport.Flux", "Code", c => c.String());
        }
    }
}
