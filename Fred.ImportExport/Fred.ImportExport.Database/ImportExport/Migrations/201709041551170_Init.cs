namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "importExport.Flux",
                c => new
                    {
                        FluxId = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Libelle = c.String(),
                        Titre = c.String(),
                        Description = c.String(),
                        IsActif = c.Boolean(nullable: false),
                        SocieteCode = c.String(),
                        SocieteModeleCode = c.String(),
                        ConnexionChaineSource = c.String(),
                        DateDerniereExecution = c.DateTime(),
                    })
                .PrimaryKey(t => t.FluxId);
            
            CreateTable(
                "importExport.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        DebutDate = c.DateTime(nullable: false),
                        FinDate = c.DateTime(nullable: false),
                        HangFireId = c.Int(nullable: false),
                        FluxId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("importExport.Flux", t => t.FluxId, cascadeDelete: true)
                .Index(t => t.FluxId);
            
            CreateTable(
                "nlog.NLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Application = c.String(maxLength: 50),
                        Logged = c.DateTime(nullable: false),
                        Level = c.String(maxLength: 50),
                        Message = c.String(),
                        UserName = c.String(maxLength: 250),
                        ServerName = c.String(),
                        Port = c.String(),
                        Url = c.String(),
                        Https = c.Boolean(nullable: false),
                        ServerAddress = c.String(maxLength: 100),
                        RemoteAddress = c.String(maxLength: 100),
                        Logger = c.String(maxLength: 250),
                        Callsite = c.String(),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "importExport.TransfertDepense",
                c => new
                    {
                        TransfertDepenseId = c.Int(nullable: false, identity: true),
                        UtilisateurId = c.Int(nullable: false),
                        Nom = c.String(),
                        Prenom = c.String(),
                        Statut = c.Int(nullable: false),
                        Periode = c.DateTime(nullable: false),
                        DateDebut = c.DateTime(nullable: false),
                        DateFin = c.DateTime(),
                        CiId = c.Int(nullable: false),
                        CiCode = c.String(),
                        CiLibelle = c.String(),
                    })
                .PrimaryKey(t => t.TransfertDepenseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("importExport.Jobs", "FluxId", "importExport.Flux");
            DropIndex("importExport.Jobs", new[] { "FluxId" });
            DropTable("importExport.TransfertDepense");
            DropTable("nlog.NLogs");
            DropTable("importExport.Jobs");
            DropTable("importExport.Flux");
        }
    }
}
