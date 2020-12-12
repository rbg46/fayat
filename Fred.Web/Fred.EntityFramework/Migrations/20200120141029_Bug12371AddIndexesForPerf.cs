using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class Bug12371AddIndexesForPerf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_Code_Libelle",
                schema: "dbo",
                table: "FRED_SOCIETE",
                columns: new[] { "Code", "Libelle" });

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_Nom_Prenom_Matricule",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                columns: new[] { "Nom", "Prenom", "Matricule" });

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_PAIE_Code_Libelle",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_PAIE",
                columns: new[] { "Code", "Libelle" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FRED_SOCIETE_Code_Libelle",
                schema: "dbo",
                table: "FRED_SOCIETE");

            migrationBuilder.DropIndex(
                name: "IX_FRED_PERSONNEL_Nom_Prenom_Matricule",
                schema: "dbo",
                table: "FRED_PERSONNEL");

            migrationBuilder.DropIndex(
                name: "IX_FRED_ETABLISSEMENT_PAIE_Code_Libelle",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_PAIE");
        }
    }
}
