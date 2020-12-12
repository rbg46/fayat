using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class AddNumeroFactureSAPToEcritureComptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroFactureSAP",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroFactureSAP",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE");
        }
    }
}
