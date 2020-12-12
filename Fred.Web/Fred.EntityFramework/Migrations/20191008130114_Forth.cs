using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class Forth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuteurPremiereImpressionBrouillonId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePremiereImpressionBrouillon",
                schema: "dbo",
                table: "FRED_COMMANDE",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FournisseurProvisoire",
                schema: "dbo",
                table: "FRED_COMMANDE",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_AuteurPremiereImpressionBrouillonId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AuteurPremiereImpressionBrouillonId");

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_FRED_UTILISATEUR_AuteurPremiereImpressionBrouillonId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AuteurPremiereImpressionBrouillonId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_COMMANDE_FRED_UTILISATEUR_AuteurPremiereImpressionBrouillonId",
                schema: "dbo",
                table: "FRED_COMMANDE");

            migrationBuilder.DropIndex(
                name: "IX_FRED_COMMANDE_AuteurPremiereImpressionBrouillonId",
                schema: "dbo",
                table: "FRED_COMMANDE");

            migrationBuilder.DropColumn(
                name: "AuteurPremiereImpressionBrouillonId",
                schema: "dbo",
                table: "FRED_COMMANDE");

            migrationBuilder.DropColumn(
                name: "DatePremiereImpressionBrouillon",
                schema: "dbo",
                table: "FRED_COMMANDE");

            migrationBuilder.DropColumn(
                name: "FournisseurProvisoire",
                schema: "dbo",
                table: "FRED_COMMANDE");
        }
    }
}
