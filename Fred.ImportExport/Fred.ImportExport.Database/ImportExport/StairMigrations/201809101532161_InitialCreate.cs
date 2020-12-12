namespace Fred.ImportExport.Database.ImportExport.StairMigrations
{
  using System;
  using System.Data.Entity.Migrations;

  public partial class InitialCreate : DbMigration
  {
    public override void Up()
    {
      CreateTable(
          "dbo.STAIR_Indicateurs",
          c => new
          {
            STAIR_IndicateurId = c.Int(nullable: false, identity: true),
            CodeForm = c.String(),
            IDHistory = c.Int(nullable: false),
            UserGeneric = c.String(),
            Datetime = c.String(),
            ProjectPath = c.String(),
            CategoryPath = c.String(),
            IDQuestion = c.Int(nullable: false),
            LabelQuestion = c.String(),
            IDResponse = c.Int(nullable: false),
            TypeResponse = c.String(),
            LabelResponse = c.String(),
            AnswerData = c.String(),
          })
          .PrimaryKey(t => t.STAIR_IndicateurId);

    }

    public override void Down()
    {
      DropTable("dbo.STAIR_Indicateurs");
    }
  }
}
