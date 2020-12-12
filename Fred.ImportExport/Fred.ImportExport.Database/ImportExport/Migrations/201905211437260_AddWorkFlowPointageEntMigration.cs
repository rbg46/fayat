namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkFlowPointageEntMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "importExport.FRED_WORKFLOW_POINTAGE",
                c => new
                    {
                        WorkflowId = c.Int(nullable: false, identity: true),
                        RapportLigneId = c.Int(nullable: false),
                        LogicielTiersId = c.Int(nullable: false),
                        AuteurId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        FluxName = c.String(),
                        CiId = c.Int(nullable: false),
                        DatePointage = c.DateTime(nullable: false),
                        MaterielId = c.Int(),
                        PersonnelId = c.Int(),
                        MaterielMarche = c.Double(nullable: false),
                        MaterielArret = c.Double(nullable: false),
                        MaterielPanne = c.Double(nullable: false),
                        MaterielIntemperie = c.Double(nullable: false),
                        HeureNormale = c.Double(nullable: false),
                        HeureMajoration = c.Double(nullable: false),
                        HeureAbsence = c.Double(nullable: false),
                        CodeAbsenceId = c.Int(),
                        CodeMajorationId = c.Int(),
                        CodeDeplacementId = c.Int(),
                        CodeZoneDeplacementId = c.Int(),
                        DeplacementIV = c.Boolean(),
                        HangfireJobId = c.String(),
                        Suppression = c.Boolean(nullable: false),
                        DateEnvoiSuppression = c.DateTime(),
                    })
                .PrimaryKey(t => t.WorkflowId)
                .ForeignKey("importExport.FRED_LOGICIEL_TIERS", t => t.LogicielTiersId, cascadeDelete: true)
                .Index(t => t.LogicielTiersId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("importExport.FRED_WORKFLOW_POINTAGE", "LogicielTiersId", "importExport.FRED_LOGICIEL_TIERS");
            DropIndex("importExport.FRED_WORKFLOW_POINTAGE", new[] { "LogicielTiersId" });
            DropTable("importExport.FRED_WORKFLOW_POINTAGE");
        }
    }
}
