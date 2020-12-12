using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class US6472 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContratId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HeuresInsertion",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_ContratId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "ContratId");

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_CONTRAT_INTERIMAIRE_ContratId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "ContratId",
                principalSchema: "dbo",
                principalTable: "FRED_CONTRAT_INTERIMAIRE",
                principalColumn: "PersonnelFournisseurSocieteId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_CONTRAT_INTERIMAIRE_ContratId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE");

            migrationBuilder.DropIndex(
                name: "IX_FRED_RAPPORT_LIGNE_ContratId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE");

            migrationBuilder.DropColumn(
                name: "ContratId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE");

            migrationBuilder.DropColumn(
                name: "HeuresInsertion",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");
        }
    }
}
