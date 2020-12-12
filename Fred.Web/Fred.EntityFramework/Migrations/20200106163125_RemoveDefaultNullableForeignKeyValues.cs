using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class RemoveDefaultNullableForeignKeyValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
