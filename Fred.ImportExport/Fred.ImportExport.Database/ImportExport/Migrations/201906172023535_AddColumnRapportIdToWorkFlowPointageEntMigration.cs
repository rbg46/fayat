namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnRapportIdToWorkFlowPointageEntMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("importExport.FRED_WORKFLOW_POINTAGE", "RapportId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("importExport.FRED_WORKFLOW_POINTAGE", "RapportId");
        }
    }
}
