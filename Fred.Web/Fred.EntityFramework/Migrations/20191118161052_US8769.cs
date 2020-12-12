using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class US8769 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_BUDGET_COPY_HISTO_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO");

            migrationBuilder.AddColumn<int>(
                name: "AffectationAbsenceId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAbsence",
                schema: "dbo",
                table: "FRED_CI",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCopy",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FRED_STATUT_ABSENCE",
                schema: "dbo",
                columns: table => new
                {
                    StatutAbsenceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: false),
                    Libelle = table.Column<string>(nullable: true),
                    Niveau = table.Column<int>(nullable: false),
                    IsActif = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_STATUT_ABSENCE", x => x.StatutAbsenceId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TYPE_JOURNEE",
                schema: "dbo",
                columns: table => new
                {
                    TypeJourneeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: false),
                    Libelle = table.Column<string>(nullable: true),
                    IsActif = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TYPE_JOURNEE", x => x.TypeJourneeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AFFECTATION_ABSENCE",
                schema: "dbo",
                columns: table => new
                {
                    AffectationAbsenceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PersonnelId = table.Column<int>(nullable: false),
                    CodeAbsenceId = table.Column<int>(nullable: false),
                    StatutAbsenceId = table.Column<int>(nullable: false),
                    DateCreation = table.Column<DateTime>(nullable: false),
                    DateDebut = table.Column<DateTime>(nullable: false),
                    DateFin = table.Column<DateTime>(nullable: true),
                    TypeDebutId = table.Column<int>(nullable: true),
                    TypeFinId = table.Column<int>(nullable: true),
                    DateValidation = table.Column<DateTime>(nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    AuteurValidationId = table.Column<int>(nullable: true),
                    EstProlonge = table.Column<bool>(nullable: false),
                    Commentaire = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AFFECTATION_ABSENCE", x => x.AffectationAbsenceId);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_ABSENCE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_ABSENCE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_ABSENCE_FRED_UTILISATEUR_AuteurSuppressionId",
                        column: x => x.AuteurSuppressionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_ABSENCE_FRED_UTILISATEUR_AuteurValidationId",
                        column: x => x.AuteurValidationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_ABSENCE_FRED_CODE_ABSENCE_CodeAbsenceId",
                        column: x => x.CodeAbsenceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CODE_ABSENCE",
                        principalColumn: "CodeAbsenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_ABSENCE_FRED_PERSONNEL_PersonnelId",
                        column: x => x.PersonnelId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_ABSENCE_FRED_STATUT_ABSENCE_StatutAbsenceId",
                        column: x => x.StatutAbsenceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_STATUT_ABSENCE",
                        principalColumn: "StatutAbsenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_ABSENCE_FRED_TYPE_JOURNEE_TypeDebutId",
                        column: x => x.TypeDebutId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TYPE_JOURNEE",
                        principalColumn: "TypeJourneeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_ABSENCE_FRED_TYPE_JOURNEE_TypeFinId",
                        column: x => x.TypeFinId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TYPE_JOURNEE",
                        principalColumn: "TypeJourneeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_AffectationAbsenceId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AffectationAbsenceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_COPY_HISTO_BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                column: "BudgetEntBudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_ABSENCE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_AFFECTATION_ABSENCE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_ABSENCE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_AFFECTATION_ABSENCE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_ABSENCE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_AFFECTATION_ABSENCE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_ABSENCE_AuteurValidationId",
                schema: "dbo",
                table: "FRED_AFFECTATION_ABSENCE",
                column: "AuteurValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_ABSENCE_CodeAbsenceId",
                schema: "dbo",
                table: "FRED_AFFECTATION_ABSENCE",
                column: "CodeAbsenceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_ABSENCE_PersonnelId",
                schema: "dbo",
                table: "FRED_AFFECTATION_ABSENCE",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_ABSENCE_StatutAbsenceId",
                schema: "dbo",
                table: "FRED_AFFECTATION_ABSENCE",
                column: "StatutAbsenceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_ABSENCE_TypeDebutId",
                schema: "dbo",
                table: "FRED_AFFECTATION_ABSENCE",
                column: "TypeDebutId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_ABSENCE_TypeFinId",
                schema: "dbo",
                table: "FRED_AFFECTATION_ABSENCE",
                column: "TypeFinId");

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_COPY_HISTO_FRED_BUDGET_BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                column: "BudgetEntBudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_COPY_HISTO_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_AFFECTATION_ABSENCE_AffectationAbsenceId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AffectationAbsenceId",
                principalSchema: "dbo",
                principalTable: "FRED_AFFECTATION_ABSENCE",
                principalColumn: "AffectationAbsenceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_BUDGET_COPY_HISTO_FRED_BUDGET_BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO");

            migrationBuilder.DropForeignKey(
                name: "FK_FRED_BUDGET_COPY_HISTO_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO");

            migrationBuilder.DropForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_AFFECTATION_ABSENCE_AffectationAbsenceId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE");

            migrationBuilder.DropTable(
                name: "FRED_AFFECTATION_ABSENCE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_STATUT_ABSENCE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_TYPE_JOURNEE",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_FRED_RAPPORT_LIGNE_AffectationAbsenceId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE");

            migrationBuilder.DropIndex(
                name: "IX_FRED_BUDGET_COPY_HISTO_BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO");

            migrationBuilder.DropColumn(
                name: "AffectationAbsenceId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE");

            migrationBuilder.DropColumn(
                name: "IsAbsence",
                schema: "dbo",
                table: "FRED_CI");

            migrationBuilder.DropColumn(
                name: "BudgetEntBudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCopy",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_COPY_HISTO_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
