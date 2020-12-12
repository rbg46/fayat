namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTranspoCodeEmploiToRessourceEnt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "importExport.TranspoCodeEmploiToRessource",
                c => new
                    {
                        TranspoCodeEmploiToRessourceId = c.Int(nullable: false, identity: true),
                        CodeSocieteImport = c.String(),
                        CodeEmploi = c.String(),
                        CodeRessource = c.String(),
                    })
                .PrimaryKey(t => t.TranspoCodeEmploiToRessourceId);
            
        }
        
        public override void Down()
        {
            DropTable("importExport.TranspoCodeEmploiToRessource");
        }
    }
}
