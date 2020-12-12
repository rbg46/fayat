using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class Us8924AddEnumTypePersonnelToRapportEnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeStatutRapport",
                schema: "dbo",
                table: "FRED_RAPPORT",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeStatutRapport",
                schema: "dbo",
                table: "FRED_RAPPORT");
        }
    }
}
