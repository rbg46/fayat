namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePKWorkflowLogicielTiersEnt : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("importExport.FRED_WORKFLOW_LOGICIEL_TIERS");
            AlterColumn("importExport.FRED_WORKFLOW_LOGICIEL_TIERS", "FluxName", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("importExport.FRED_WORKFLOW_LOGICIEL_TIERS", new[] { "RapportLigneId", "LogicielTiersId", "FluxName" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("importExport.FRED_WORKFLOW_LOGICIEL_TIERS");
            AlterColumn("importExport.FRED_WORKFLOW_LOGICIEL_TIERS", "FluxName", c => c.String());
            AddPrimaryKey("importExport.FRED_WORKFLOW_LOGICIEL_TIERS", new[] { "RapportLigneId", "LogicielTiersId" });
        }
    }
}
