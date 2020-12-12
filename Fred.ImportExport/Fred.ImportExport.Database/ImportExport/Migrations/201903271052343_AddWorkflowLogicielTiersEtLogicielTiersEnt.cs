namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkflowLogicielTiersEtLogicielTiersEnt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "importExport.FRED_LOGICIEL_TIERS",
                c => new
                    {
                        FredLogicielTiersId = c.Int(nullable: false, identity: true),
                        NomLogiciel = c.String(nullable: false, maxLength: 50),
                        Version = c.String(maxLength: 10),
                        Mandant = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.FredLogicielTiersId)
                .Index(t => new { t.NomLogiciel, t.Version, t.Mandant }, unique: true, name: "IX_UniqueNomVersionMandant");
            
            CreateTable(
                "importExport.FRED_WORKFLOW_LOGICIEL_TIERS",
                c => new
                    {
                        RapportLigneId = c.Int(nullable: false),
                        LogicielTiersId = c.Int(nullable: false),
                        AuteurId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.RapportLigneId, t.LogicielTiersId })
                .ForeignKey("importExport.FRED_LOGICIEL_TIERS", t => t.LogicielTiersId)
                .Index(t => t.LogicielTiersId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("importExport.FRED_WORKFLOW_LOGICIEL_TIERS", "LogicielTiersId", "importExport.FRED_LOGICIEL_TIERS");
            DropIndex("importExport.FRED_WORKFLOW_LOGICIEL_TIERS", new[] { "LogicielTiersId" });
            DropIndex("importExport.FRED_LOGICIEL_TIERS", "IX_UniqueNomVersionMandant");
            DropTable("importExport.FRED_WORKFLOW_LOGICIEL_TIERS");
            DropTable("importExport.FRED_LOGICIEL_TIERS");
        }
    }
}
