namespace Fred.ImportExport.Database.ImportExport.StairMigrations
{
    using System.Data.Entity.Migrations;

    public partial class AddForeignKeyTableSphinxQuestionReponse : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE [dbo].[SPHINX_Question] "
                + " WHERE SPHINXFormulaireId NOT IN(SELECT SPHINXFormulaireId"
                + " FROM[dbo].[SPHINX_Formulaire])");

            Sql("DELETE [dbo].[SPHINX_Reponse] "
                + " WHERE SPHINXQuestionId NOT IN (SELECT SPHINXQuestionId"
                + " FROM [dbo].[SPHINX_Question])");

            AddForeignKey("dbo.SPHINX_Reponse", "SPHINXQuestionId", "dbo.SPHINX_Question", "SPHINXQuestionId", cascadeDelete: false);
            AddForeignKey("dbo.SPHINX_Question", "SPHINXFormulaireId", "dbo.SPHINX_Formulaire", "SPHINXFormulaireId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SPHINX_Question", "SPHINXFormulaireId", "dbo.SPHINX_Formulaire");
            DropForeignKey("dbo.SPHINX_Reponse", "SPHINXQuestionId", "dbo.SPHINX_Question");
        }
    }
}
