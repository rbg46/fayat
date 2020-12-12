namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNomServeurDansLogicielTiers : DbMigration
    {
        public override void Up()
        {
            DropIndex("importExport.FRED_LOGICIEL_TIERS", "IX_UniqueNomMandant");
            AddColumn("importExport.FRED_LOGICIEL_TIERS", "NomServeur", c => c.String(maxLength: 50));
            CreateIndex("importExport.FRED_LOGICIEL_TIERS", new[] { "NomLogiciel", "NomServeur", "Mandant" }, unique: true, name: "IX_UniqueNomLogicielNomServeurMandant");
        }
        
        public override void Down()
        {
            DropIndex("importExport.FRED_LOGICIEL_TIERS", "IX_UniqueNomLogicielNomServeurMandant");
            DropColumn("importExport.FRED_LOGICIEL_TIERS", "NomServeur");
            CreateIndex("importExport.FRED_LOGICIEL_TIERS", new[] { "NomLogiciel", "Mandant" }, unique: true, name: "IX_UniqueNomMandant");
        }
    }
}
