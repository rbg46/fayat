namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FakeAddTableNlog2Migration : DbMigration
    {
        public override void Up()
        {
            // Oublie de refacto sur le DbContext, ce qui oblige à faire une deuxième migration... :(
            // please see comment on FakeAddTableNlogMigration

            ////RenameTable(name: "importExport.NLogEnts", newName: "NLogs");
            ////MoveTable(name: "importExport.NLogs", newSchema: "nlog");
            ////AlterColumn("nlog.NLogs", "Application", c => c.String(maxLength: 50));
            ////AlterColumn("nlog.NLogs", "Level", c => c.String(maxLength: 50));
            ////AlterColumn("nlog.NLogs", "UserName", c => c.String(maxLength: 250));
            ////AlterColumn("nlog.NLogs", "ServerAddress", c => c.String(maxLength: 100));
            ////AlterColumn("nlog.NLogs", "RemoteAddress", c => c.String(maxLength: 100));
            ////AlterColumn("nlog.NLogs", "Logger", c => c.String(maxLength: 250));
        }

        public override void Down()
        {
            ////AlterColumn("nlog.NLogs", "Logger", c => c.String());
            ////AlterColumn("nlog.NLogs", "RemoteAddress", c => c.String());
            ////AlterColumn("nlog.NLogs", "ServerAddress", c => c.String());
            ////AlterColumn("nlog.NLogs", "UserName", c => c.String());
            ////AlterColumn("nlog.NLogs", "Level", c => c.String());
            ////AlterColumn("nlog.NLogs", "Application", c => c.String());
            ////MoveTable(name: "nlog.NLogs", newSchema: "importExport");
            ////RenameTable(name: "importExport.NLogs", newName: "NLogEnts");
        }
    }
}
