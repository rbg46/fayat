namespace Fred.ImportExport.Database.ImportExport.StairMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddcolonnesContactRecipientsandUserRecipientsMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.STAIR_Indicateurs", "ContactRecipients", c => c.String());
            AddColumn("dbo.STAIR_Indicateurs", "UserRecipients", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.STAIR_Indicateurs", "UserRecipients");
            DropColumn("dbo.STAIR_Indicateurs", "ContactRecipients");
        }
    }
}
