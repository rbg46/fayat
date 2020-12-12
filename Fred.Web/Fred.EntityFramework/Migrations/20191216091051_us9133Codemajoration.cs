using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class us9133Codemajoration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCadre",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsETAM",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOuvrier",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCadre",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION");

            migrationBuilder.DropColumn(
                name: "IsETAM",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION");

            migrationBuilder.DropColumn(
                name: "IsOuvrier",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION");
        }
    }
}
