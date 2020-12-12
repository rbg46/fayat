using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class us9133Prime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCadre",
                schema: "dbo",
                table: "FRED_PRIME",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsETAM",
                schema: "dbo",
                table: "FRED_PRIME",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOuvrier",
                schema: "dbo",
                table: "FRED_PRIME",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCadre",
                schema: "dbo",
                table: "FRED_PRIME");

            migrationBuilder.DropColumn(
                name: "IsETAM",
                schema: "dbo",
                table: "FRED_PRIME");

            migrationBuilder.DropColumn(
                name: "IsOuvrier",
                schema: "dbo",
                table: "FRED_PRIME");
        }
    }
}
