using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class US6001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstUnAbonnement",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OperationDiverseMereIdAbonnement",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_OperationDiverseMereIdAbonnement",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "OperationDiverseMereIdAbonnement");

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_OPERATION_DIVERSE_FRED_OPERATION_DIVERSE_OperationDiverseMereIdAbonnement",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "OperationDiverseMereIdAbonnement",
                principalSchema: "dbo",
                principalTable: "FRED_OPERATION_DIVERSE",
                principalColumn: "OperationDiverseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_OPERATION_DIVERSE_FRED_OPERATION_DIVERSE_OperationDiverseMereIdAbonnement",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE");

            migrationBuilder.DropIndex(
                name: "IX_FRED_OPERATION_DIVERSE_OperationDiverseMereIdAbonnement",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE");

            migrationBuilder.DropColumn(
                name: "EstUnAbonnement",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE");

            migrationBuilder.DropColumn(
                name: "OperationDiverseMereIdAbonnement",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE");
        }
    }
}
