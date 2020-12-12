namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFluxToWorkflowLogicielTiersSupprimeVersionDansLogicielTiers : DbMigration
    {
        public override void Up()
        {
            //On vide ces deux tables car la table fred_workflow_logiciels_tiers 
            //A un lien vers fred_logiciel_tiers qui sera vidée.
            //Fred_logiciel_tiers doit être vidé car sa structure va changer et on ne va plus enregistrer les mêmes données
            //Pas besoin de faire un script pour sauvegarder les données précédemment enregistrées, étant la donné la jeunesse de ces deux tables
            Sql("delete from importExport.FRED_WORKFLOW_LOGICIEL_TIERS");
            Sql("delete from importExport.FRED_LOGICIEL_TIERS");

            DropIndex("importExport.FRED_LOGICIEL_TIERS", "IX_UniqueNomVersionMandant");
            AddColumn("importExport.FRED_WORKFLOW_LOGICIEL_TIERS", "FluxName", c => c.String());
            CreateIndex("importExport.FRED_LOGICIEL_TIERS", new[] { "NomLogiciel", "Mandant" }, unique: true, name: "IX_UniqueNomMandant");
            DropColumn("importExport.FRED_LOGICIEL_TIERS", "Version");
        }
        
        public override void Down()
        {
            AddColumn("importExport.FRED_LOGICIEL_TIERS", "Version", c => c.String(maxLength: 10));
            DropIndex("importExport.FRED_LOGICIEL_TIERS", "IX_UniqueNomMandant");
            DropColumn("importExport.FRED_WORKFLOW_LOGICIEL_TIERS", "FluxName");
            CreateIndex("importExport.FRED_LOGICIEL_TIERS", new[] { "NomLogiciel", "Version", "Mandant" }, unique: true, name: "IX_UniqueNomVersionMandant");
        }
    }
}
