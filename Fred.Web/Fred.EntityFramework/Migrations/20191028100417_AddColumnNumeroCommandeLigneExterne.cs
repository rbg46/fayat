using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class AddColumnNumeroCommandeLigneExterne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroCommandeLigneExterne",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroCommandeLigneExterne",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE");
        }
    }
}
