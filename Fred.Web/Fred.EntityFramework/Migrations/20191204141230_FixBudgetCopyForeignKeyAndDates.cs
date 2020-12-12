using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class FixBudgetCopyForeignKeyAndDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_BUDGET_COPY_HISTO_FRED_BUDGET_BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO");

            migrationBuilder.DropIndex(
                name: "IX_FRED_BUDGET_COPY_HISTO_BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO");

            migrationBuilder.DropColumn(
                name: "BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_COPY_HISTO_BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                column: "BudgetEntBudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_COPY_HISTO_FRED_BUDGET_BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                column: "BudgetEntBudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
