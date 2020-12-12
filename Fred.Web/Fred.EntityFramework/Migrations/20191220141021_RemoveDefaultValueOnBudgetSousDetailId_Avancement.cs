using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class RemoveDefaultValueOnBudgetSousDetailId_Avancement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BudgetSousDetailId",
                schema: "dbo",
                table: "FRED_AVANCEMENT",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BudgetSousDetailId",
                schema: "dbo",
                table: "FRED_AVANCEMENT",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));
        }
    }
}
