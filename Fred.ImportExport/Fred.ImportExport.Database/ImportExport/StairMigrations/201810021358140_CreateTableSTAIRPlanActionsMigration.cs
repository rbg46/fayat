namespace Fred.ImportExport.Database.ImportExport.StairMigrations
{
  using System;
  using System.Data.Entity.Migrations;

  public partial class CreateTableStairPlanActionsMigration : DbMigration
  {
    public override void Up()
    {
      CreateTable(
          "dbo.STAIR_Plan_Actions",
          c => new
          {
            STAIR_PlanActionId = c.Int(nullable: false, identity: true),
            Id_Enregistrement = c.Int(nullable: false),
            UserGeneric = c.String(),
            IDHistory = c.Int(nullable: false),
            IDQuestion = c.Int(nullable: false),
            IDResponse = c.Int(nullable: false),
            ProjectPath = c.String(),
            CategoryPath = c.String(),
            CreatedDate = c.String(),
            DueDate = c.String(),
            ResolutionDate = c.String(),
            Name = c.String(),
            Comment = c.String(),
            ResolutionComment = c.String(),
            Priority = c.Int(nullable: false),
            References = c.String(),
            Contacts = c.String(),
            Users = c.String(),
          })
          .PrimaryKey(t => t.STAIR_PlanActionId);

    }

    public override void Down()
    {
      DropTable("dbo.STAIR_Plan_Actions");
    }
  }
}
