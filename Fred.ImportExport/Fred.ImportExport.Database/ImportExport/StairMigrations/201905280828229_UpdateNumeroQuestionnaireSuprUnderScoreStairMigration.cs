namespace Fred.ImportExport.Database.ImportExport.StairMigrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateNumeroQuestionnaireSuprUnderScoreStairMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SPHINX_Question", "SPHINX_FormulaireId", "dbo.SPHINX_Formulaire");
            DropForeignKey("dbo.SPHINX_Reponse", "SPHINX_QuestionId", "dbo.SPHINX_Question");

            RenameColumn(table: "dbo.SPHINX_Question", name: "SPHINX_FormulaireId", newName: "SPHINXFormulaireId");
            RenameColumn(table: "dbo.SPHINX_Reponse", name: "SPHINX_QuestionId", newName: "SPHINXQuestionId");
            RenameIndex(table: "dbo.SPHINX_Question", name: "IX_SPHINX_FormulaireId", newName: "IX_SPHINXFormulaireId");
            RenameIndex(table: "dbo.SPHINX_Reponse", name: "IX_SPHINX_QuestionId", newName: "IX_SPHINXQuestionId");

            DropPrimaryKey("dbo.SPHINX_Formulaire");
            DropPrimaryKey("dbo.SPHINX_Question");
            DropPrimaryKey("dbo.SPHINX_Reponse");
            DropPrimaryKey("dbo.STAIR_Indicateurs");
            DropPrimaryKey("dbo.STAIR_Plan_Actions");

            DropColumn("dbo.SPHINX_Formulaire", "SPHINX_FormulaireId");
            DropColumn("dbo.SPHINX_Question", "SPHINX_QuestionId");
            DropColumn("dbo.SPHINX_Reponse", "SPHINX_ReponseId");
            DropColumn("dbo.STAIR_Indicateurs", "STAIR_IndicateurId");
            DropColumn("dbo.STAIR_Plan_Actions", "STAIR_PlanActionId");
            DropColumn("dbo.STAIR_Plan_Actions", "Id_Enregistrement");

            AddColumn("dbo.SPHINX_Formulaire", "SPHINXFormulaireId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.SPHINX_Question", "SPHINXQuestionId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.SPHINX_Reponse", "SPHINXReponseId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.SPHINX_Reponse", "NumeroQuestionnaire", c => c.Int(nullable: false));
            AddColumn("dbo.STAIR_Indicateurs", "STAIRIndicateurId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.STAIR_Plan_Actions", "STAIRPlanActionId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.STAIR_Plan_Actions", "IdEnregistrement", c => c.Int(nullable: false));

            AddPrimaryKey("dbo.SPHINX_Formulaire", "SPHINXFormulaireId");
            AddPrimaryKey("dbo.SPHINX_Question", "SPHINXQuestionId");
            AddPrimaryKey("dbo.SPHINX_Reponse", "SPHINXReponseId");
            AddPrimaryKey("dbo.STAIR_Indicateurs", "STAIRIndicateurId");
            AddPrimaryKey("dbo.STAIR_Plan_Actions", "STAIRPlanActionId");



        }
        
        public override void Down()
        {
            AddColumn("dbo.STAIR_Plan_Actions", "Id_Enregistrement", c => c.Int(nullable: false));
            AddColumn("dbo.STAIR_Plan_Actions", "STAIR_PlanActionId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.STAIR_Indicateurs", "STAIR_IndicateurId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.SPHINX_Reponse", "SPHINX_ReponseId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.SPHINX_Question", "SPHINX_QuestionId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.SPHINX_Formulaire", "SPHINX_FormulaireId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.SPHINX_Reponse", "SPHINXQuestionId", "dbo.SPHINX_Question");
            DropForeignKey("dbo.SPHINX_Question", "SPHINXFormulaireId", "dbo.SPHINX_Formulaire");
            DropPrimaryKey("dbo.STAIR_Plan_Actions");
            DropPrimaryKey("dbo.STAIR_Indicateurs");
            DropPrimaryKey("dbo.SPHINX_Reponse");
            DropPrimaryKey("dbo.SPHINX_Question");
            DropPrimaryKey("dbo.SPHINX_Formulaire");
            DropColumn("dbo.STAIR_Plan_Actions", "IdEnregistrement");
            DropColumn("dbo.STAIR_Plan_Actions", "STAIRPlanActionId");
            DropColumn("dbo.STAIR_Indicateurs", "STAIRIndicateurId");
            DropColumn("dbo.SPHINX_Reponse", "NumeroQuestionnaire");
            DropColumn("dbo.SPHINX_Reponse", "SPHINXReponseId");
            DropColumn("dbo.SPHINX_Question", "SPHINXQuestionId");
            DropColumn("dbo.SPHINX_Formulaire", "SPHINXFormulaireId");
            AddPrimaryKey("dbo.STAIR_Plan_Actions", "STAIR_PlanActionId");
            AddPrimaryKey("dbo.STAIR_Indicateurs", "STAIR_IndicateurId");
            AddPrimaryKey("dbo.SPHINX_Reponse", "SPHINX_ReponseId");
            AddPrimaryKey("dbo.SPHINX_Question", "SPHINX_QuestionId");
            AddPrimaryKey("dbo.SPHINX_Formulaire", "SPHINX_FormulaireId");
            RenameIndex(table: "dbo.SPHINX_Reponse", name: "IX_SPHINXQuestionId", newName: "IX_SPHINX_QuestionId");
            RenameIndex(table: "dbo.SPHINX_Question", name: "IX_SPHINXFormulaireId", newName: "IX_SPHINX_FormulaireId");
            RenameColumn(table: "dbo.SPHINX_Reponse", name: "SPHINXQuestionId", newName: "SPHINX_QuestionId");
            RenameColumn(table: "dbo.SPHINX_Question", name: "SPHINXFormulaireId", newName: "SPHINX_FormulaireId");
            AddForeignKey("dbo.SPHINX_Reponse", "SPHINX_QuestionId", "dbo.SPHINX_Question", "SPHINX_QuestionId", cascadeDelete: true);
            AddForeignKey("dbo.SPHINX_Question", "SPHINX_FormulaireId", "dbo.SPHINX_Formulaire", "SPHINX_FormulaireId", cascadeDelete: true);
        }
    }
}
