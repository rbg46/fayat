using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class Us3465UpdateModelFredWeb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_MOTIF_REMPLACEMENT_MotifRemplacementId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_RESSOURCE_RessourceId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.AlterColumn<int>(
                name: "ContratInterimaireId",
                schema: "dbo",
                table: "FRED_ZONE_DE_TRAVAIL",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RapportLigneId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                schema: "dbo",
                table: "FRED_UTILISATEUR",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "GroupeId",
                schema: "dbo",
                table: "FRED_SOCIETE_CLASSIFICATION",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SocieteId",
                schema: "dbo",
                table: "FRED_ROLE",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "TimestampImport",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FamilleOperationDiverseId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SystemeInterimaire",
                schema: "dbo",
                table: "FRED_GROUPE",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BlocageContratInterimaireManuel",
                schema: "dbo",
                table: "FRED_FOURNISSEUR",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceSystemeInterimaire",
                schema: "dbo",
                table: "FRED_FOURNISSEUR",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TacheId",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "JournalId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FamilleOperationDiverseId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CommandeId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PersonnelDelegueId",
                schema: "dbo",
                table: "FRED_DELEGATION",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PersonnelDelegantId",
                schema: "dbo",
                table: "FRED_DELEGATION",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PersonnelAuteurId",
                schema: "dbo",
                table: "FRED_DELEGATION",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_CONTROLE_BUDGETAIRE_VALEURS",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ControleBudgetaireEtatId",
                schema: "dbo",
                table: "FRED_CONTROLE_BUDGETAIRE",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UniteId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Statut",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SocieteId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MotifRemplacementId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FournisseurId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CiId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModification",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EtatContratId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FournisseurReferenceExterne",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UtilisateurIdCreation",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UtilisateurIdModification",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_BUDGET_T4_RESSOURCE",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DeviseId",
                schema: "dbo",
                table: "FRED_BUDGET",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "BudgetEtatId",
                schema: "dbo",
                table: "FRED_BUDGET",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MaterielId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FRED_CONTRAT_INTERIMAIRE_IMPORT",
                schema: "dbo",
                columns: table => new
                {
                    ImportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContratInterimaireId = table.Column<int>(nullable: false),
                    TimestampImport = table.Column<long>(type: "bigint", nullable: false),
                    HistoriqueImport = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CONTRAT_INTERIMAIRE_IMPORT", x => x.ImportId);
                    table.ForeignKey(
                        name: "FK_FRED_CONTRAT_INTERIMAIRE_IMPORT_FRED_CONTRAT_INTERIMAIRE_ContratInterimaireId",
                        column: x => x.ContratInterimaireId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CONTRAT_INTERIMAIRE",
                        principalColumn: "PersonnelFournisseurSocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ETAT_CONTRAT_INTERIMAIRE",
                schema: "dbo",
                columns: table => new
                {
                    EtatContratInterimaireId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Libelle = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ETAT_CONTRAT_INTERIMAIRE", x => x.EtatContratInterimaireId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_EtatContratId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "EtatContratId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_IMPORT_ContratInterimaireId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE_IMPORT",
                column: "ContratInterimaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_ETAT_CONTRAT_INTERIMAIRE_EtatContratId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "EtatContratId",
                principalSchema: "dbo",
                principalTable: "FRED_ETAT_CONTRAT_INTERIMAIRE",
                principalColumn: "EtatContratInterimaireId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_MOTIF_REMPLACEMENT_MotifRemplacementId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "MotifRemplacementId",
                principalSchema: "dbo",
                principalTable: "FRED_MOTIF_REMPLACEMENT",
                principalColumn: "MotifRemplacementId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_RESSOURCE_RessourceId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "RessourceId",
                principalSchema: "dbo",
                principalTable: "FRED_RESSOURCE",
                principalColumn: "RessourceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_ETAT_CONTRAT_INTERIMAIRE_EtatContratId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_MOTIF_REMPLACEMENT_MotifRemplacementId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_RESSOURCE_RessourceId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropTable(
                name: "FRED_CONTRAT_INTERIMAIRE_IMPORT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ETAT_CONTRAT_INTERIMAIRE",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_EtatContratId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropColumn(
                name: "TimestampImport",
                schema: "dbo",
                table: "FRED_PERSONNEL");

            migrationBuilder.DropColumn(
                name: "SystemeInterimaire",
                schema: "dbo",
                table: "FRED_GROUPE");

            migrationBuilder.DropColumn(
                name: "BlocageContratInterimaireManuel",
                schema: "dbo",
                table: "FRED_FOURNISSEUR");

            migrationBuilder.DropColumn(
                name: "ReferenceSystemeInterimaire",
                schema: "dbo",
                table: "FRED_FOURNISSEUR");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropColumn(
                name: "DateModification",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropColumn(
                name: "EtatContratId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropColumn(
                name: "FournisseurReferenceExterne",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropColumn(
                name: "UtilisateurIdCreation",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.DropColumn(
                name: "UtilisateurIdModification",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE");

            migrationBuilder.AlterColumn<int>(
                name: "ContratInterimaireId",
                schema: "dbo",
                table: "FRED_ZONE_DE_TRAVAIL",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RapportLigneId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                schema: "dbo",
                table: "FRED_UTILISATEUR",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "GroupeId",
                schema: "dbo",
                table: "FRED_SOCIETE_CLASSIFICATION",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SocieteId",
                schema: "dbo",
                table: "FRED_ROLE",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FamilleOperationDiverseId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TacheId",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "JournalId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FamilleOperationDiverseId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CommandeId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonnelDelegueId",
                schema: "dbo",
                table: "FRED_DELEGATION",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PersonnelDelegantId",
                schema: "dbo",
                table: "FRED_DELEGATION",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PersonnelAuteurId",
                schema: "dbo",
                table: "FRED_DELEGATION",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_CONTROLE_BUDGETAIRE_VALEURS",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ControleBudgetaireEtatId",
                schema: "dbo",
                table: "FRED_CONTROLE_BUDGETAIRE",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "UniteId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Statut",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SocieteId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MotifRemplacementId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FournisseurId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CiId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RessourceId",
                schema: "dbo",
                table: "FRED_BUDGET_T4_RESSOURCE",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DeviseId",
                schema: "dbo",
                table: "FRED_BUDGET",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BudgetEtatId",
                schema: "dbo",
                table: "FRED_BUDGET",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MaterielId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_MOTIF_REMPLACEMENT_MotifRemplacementId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "MotifRemplacementId",
                principalSchema: "dbo",
                principalTable: "FRED_MOTIF_REMPLACEMENT",
                principalColumn: "MotifRemplacementId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_RESSOURCE_RessourceId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "RessourceId",
                principalSchema: "dbo",
                principalTable: "FRED_RESSOURCE",
                principalColumn: "RessourceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
