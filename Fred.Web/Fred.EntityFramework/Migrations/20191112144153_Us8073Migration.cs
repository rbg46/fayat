using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class Us8073Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommandeId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RapportLigneId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentFamilyODWithOrder",
                schema: "dbo",
                table: "FRED_NATURE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentFamilyODWithoutOrder",
                schema: "dbo",
                table: "FRED_NATURE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_NATURE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeRef",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantite",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndResource",
                schema: "dbo",
                table: "FRED_NATURE",
                column: "RessourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UniqueCodeAndResource",
                schema: "dbo",
                table: "FRED_NATURE");

            migrationBuilder.DropColumn(
                name: "CommandeId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE");

            migrationBuilder.DropColumn(
                name: "RapportLigneId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE");

            migrationBuilder.DropColumn(
                name: "ParentFamilyODWithOrder",
                schema: "dbo",
                table: "FRED_NATURE");

            migrationBuilder.DropColumn(
                name: "ParentFamilyODWithoutOrder",
                schema: "dbo",
                table: "FRED_NATURE");

            migrationBuilder.DropColumn(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_NATURE");

            migrationBuilder.DropColumn(
                name: "CodeRef",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE");

            migrationBuilder.DropColumn(
                name: "Quantite",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE");
        }
    }
}
