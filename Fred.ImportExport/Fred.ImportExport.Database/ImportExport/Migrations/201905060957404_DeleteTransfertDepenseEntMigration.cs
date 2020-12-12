namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class DeleteTransfertDepenseEntMigration : DbMigration
    {
        public override void Up()
        {
            DropTable("importExport.TransfertDepense");
        }

        public override void Down()
        {
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
    }
}
