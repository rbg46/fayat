using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class AddPersonnelNonPointableColumnPersonnelEntEtabPaieEnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPersonnelNonPointable",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPersonnelsNonPointables",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_PAIE",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisableForPointage",
                schema: "dbo",
                table: "FRED_CI",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueLogin",
                schema: "dbo",
                table: "FRED_UTILISATEUR",
                column: "Login",
                unique: true,
                filter: "[Login] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UniqueLogin",
                schema: "dbo",
                table: "FRED_UTILISATEUR");

            migrationBuilder.DropColumn(
                name: "IsPersonnelNonPointable",
                schema: "dbo",
                table: "FRED_PERSONNEL");

            migrationBuilder.DropColumn(
                name: "IsPersonnelsNonPointables",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_PAIE");

            migrationBuilder.DropColumn(
                name: "IsDisableForPointage",
                schema: "dbo",
                table: "FRED_CI");
        }
    }
}
