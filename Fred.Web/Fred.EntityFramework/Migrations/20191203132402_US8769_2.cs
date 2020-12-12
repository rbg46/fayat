using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class US8769_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeAbsence",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeAbsence",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE");
        }
    }
}
