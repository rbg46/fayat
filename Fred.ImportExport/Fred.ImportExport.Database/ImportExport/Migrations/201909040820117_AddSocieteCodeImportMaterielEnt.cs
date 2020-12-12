namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSocieteCodeImportMaterielEnt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "importExport.SocieteCodeImportMaterielEnt",
                c => new
                    {
                        SocieteCodeImportMaterielID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        GroupCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SocieteCodeImportMaterielID);
            
        }
        
        public override void Down()
        {
            DropTable("importExport.SocieteCodeImportMaterielEnt");
        }
    }
}
