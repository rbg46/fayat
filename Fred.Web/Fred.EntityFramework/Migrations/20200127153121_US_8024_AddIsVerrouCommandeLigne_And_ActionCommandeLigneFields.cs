using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class US_8024_AddIsVerrouCommandeLigne_And_ActionCommandeLigneFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerrou",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FRED_ACTION_JOB",
                schema: "dbo",
                columns: table => new
                {
                    ActionJobId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExternalJobId = table.Column<string>(maxLength: 20, nullable: false),
                    ExternalJobName = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ACTION_JOB", x => x.ActionJobId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ACTION_STATUS",
                schema: "dbo",
                columns: table => new
                {
                    ActionStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ACTION_STATUS", x => x.ActionStatusId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ACTION_TYPE",
                schema: "dbo",
                columns: table => new
                {
                    ActionTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ACTION_TYPE", x => x.ActionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ACTION",
                schema: "dbo",
                columns: table => new
                {
                    ActionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionTypeId = table.Column<int>(nullable: false),
                    ActionJobId = table.Column<int>(nullable: true),
                    ActionStatusId = table.Column<int>(nullable: true),
                    DateAction = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurId = table.Column<int>(nullable: true),
                    Message = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ACTION", x => x.ActionId);
                    table.ForeignKey(
                        name: "FK_FRED_ACTION_FRED_ACTION_JOB_ActionJobId",
                        column: x => x.ActionJobId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ACTION_JOB",
                        principalColumn: "ActionJobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ACTION_FRED_ACTION_STATUS_ActionStatusId",
                        column: x => x.ActionStatusId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ACTION_STATUS",
                        principalColumn: "ActionStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ACTION_FRED_ACTION_TYPE_ActionTypeId",
                        column: x => x.ActionTypeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ACTION_TYPE",
                        principalColumn: "ActionTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ACTION_COMMANDE_LIGNE",
                schema: "dbo",
                columns: table => new
                {
                    ActionCommandeLigneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommandeLigneId = table.Column<int>(nullable: false),
                    ActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ACTION_COMMANDE_LIGNE", x => x.ActionCommandeLigneId);
                    table.ForeignKey(
                        name: "FK_FRED_ACTION_COMMANDE_LIGNE_FRED_ACTION_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ACTION",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_ACTION_COMMANDE_LIGNE_FRED_COMMANDE_LIGNE_CommandeLigneId",
                        column: x => x.CommandeLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE_LIGNE",
                        principalColumn: "CommandeLigneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ACTION_ActionJobId",
                schema: "dbo",
                table: "FRED_ACTION",
                column: "ActionJobId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ACTION_ActionStatusId",
                schema: "dbo",
                table: "FRED_ACTION",
                column: "ActionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ACTION_ActionTypeId",
                schema: "dbo",
                table: "FRED_ACTION",
                column: "ActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ACTION_COMMANDE_LIGNE_ActionCommandeLigneId",
                schema: "dbo",
                table: "FRED_ACTION_COMMANDE_LIGNE",
                column: "ActionCommandeLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ACTION_COMMANDE_LIGNE_ActionId",
                schema: "dbo",
                table: "FRED_ACTION_COMMANDE_LIGNE",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ACTION_COMMANDE_LIGNE_CommandeLigneId",
                schema: "dbo",
                table: "FRED_ACTION_COMMANDE_LIGNE",
                column: "CommandeLigneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FRED_ACTION_COMMANDE_LIGNE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ACTION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ACTION_JOB",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ACTION_STATUS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ACTION_TYPE",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "IsVerrou",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE");
        }
    }
}
