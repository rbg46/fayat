using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class Fix13984ddIndexUnicityForCodeAndSocieteAndEtablissement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_MATERIEL");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSocieteAndEtablissementComptable",
                schema: "dbo",
                table: "FRED_MATERIEL",
                columns: new[] { "Code", "SocieteId", "EtablissementComptableId" },
                unique: true,
                filter: "[Code] IS NOT NULL AND [EtablissementComptableId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UniqueCodeAndSocieteAndEtablissementComptable",
                schema: "dbo",
                table: "FRED_MATERIEL");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_MATERIEL",
                columns: new[] { "Code", "SocieteId" },
                unique: true);
        }
    }
}
