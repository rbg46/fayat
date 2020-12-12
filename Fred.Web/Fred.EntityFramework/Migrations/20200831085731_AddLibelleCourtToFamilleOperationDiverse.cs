using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class AddLibelleCourtToFamilleOperationDiverse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LibelleCourt",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LibelleCourt",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE");
        }
    }
}
