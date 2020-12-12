namespace Fred.ImportExport.Database.ImportExport.StairMigrations
{
  using System;
  using System.Data.Entity.Migrations;

  public partial class AddTableSphinxFormulaireQuestionReponse : DbMigration
  {
    public override void Up()
    {
      CreateTable(
          "dbo.SPHINX_Formulaire",
          c => new
          {
            SPHINX_FormulaireId = c.Int(nullable: false, identity: true),
            TitreFormulaire = c.String(),
            NombreEnregistrement = c.Int(nullable: false),
            DateCreationFormulaire = c.String(),
            DateDerniereReponse = c.String(),
            IsOpen = c.Boolean(nullable: false),
          })
          .PrimaryKey(t => t.SPHINX_FormulaireId);

      CreateTable(
          "dbo.SPHINX_Question",
          c => new
          {
            SPHINX_QuestionId = c.Int(nullable: false, identity: true),
            TitreQuestion = c.String(),
            LibelleQuestion = c.String(),
            SPHINX_FormulaireId = c.Int(nullable: false),
          })
          .PrimaryKey(t => t.SPHINX_QuestionId)
          .ForeignKey("dbo.SPHINX_Formulaire", t => t.SPHINX_FormulaireId, cascadeDelete: true)
          .Index(t => t.SPHINX_FormulaireId);

      CreateTable(
          "dbo.SPHINX_Reponse",
          c => new
          {
            SPHINX_ReponseId = c.Int(nullable: false, identity: true),
            LibelleReponse = c.String(),
            SPHINX_QuestionId = c.Int(nullable: false),
          })
          .PrimaryKey(t => t.SPHINX_ReponseId)
          .ForeignKey("dbo.SPHINX_Question", t => t.SPHINX_QuestionId, cascadeDelete: true)
          .Index(t => t.SPHINX_QuestionId);

    }

    public override void Down()
    {
      DropForeignKey("dbo.SPHINX_Question", "SPHINX_FormulaireId", "dbo.SPHINX_Formulaire");
      DropForeignKey("dbo.SPHINX_Reponse", "SPHINX_QuestionId", "dbo.SPHINX_Question");
      DropIndex("dbo.SPHINX_Reponse", new[] { "SPHINX_QuestionId" });
      DropIndex("dbo.SPHINX_Question", new[] { "SPHINX_FormulaireId" });
      DropTable("dbo.SPHINX_Reponse");
      DropTable("dbo.SPHINX_Question");
      DropTable("dbo.SPHINX_Formulaire");
    }
  }
}
