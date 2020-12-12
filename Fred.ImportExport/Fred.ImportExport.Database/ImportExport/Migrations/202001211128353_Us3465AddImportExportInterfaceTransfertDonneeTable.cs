namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Us3465AddImportExportInterfaceTransfertDonneeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "importExport.INTERFACE_TRANSFERT_DONNEE",
                c => new
                    {
                        InterfaceTransfertDonneeId = c.Int(nullable: false, identity: true),
                        CodeInterface = c.String(),
                        CodeOrganisation = c.String(),
                        DonneeType = c.String(),
                        DonneeID = c.String(),
                        DateCreation = c.DateTime(nullable: false),
                        Statut = c.Int(nullable: false),
                        DateTraitement = c.DateTime(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.InterfaceTransfertDonneeId);
            
        }
        
        public override void Down()
        {
            DropTable("importExport.INTERFACE_TRANSFERT_DONNEE");
        }
    }
}
