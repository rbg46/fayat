using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class AddZonesFacturationPaiementEtablissementComptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facturation",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Paiement",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facturation",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE");

            migrationBuilder.DropColumn(
                name: "Paiement",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE");
        }
    }
}
