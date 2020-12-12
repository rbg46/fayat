using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FRED_VISA",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_OrganisationId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEntree",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebutInsertion",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFinInsertion",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HeuresInsertion",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CodeTVA",
                schema: "dbo",
                table: "FRED_FOURNISSEUR",
                maxLength: 19,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NatureId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgenceId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObjectifHeuresInsertion",
                schema: "dbo",
                table: "FRED_CI",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FRED_ADRESSE",
                schema: "dbo",
                columns: table => new
                {
                    AdresseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(nullable: true),
                    Ligne = table.Column<string>(maxLength: 250, nullable: true),
                    CodePostal = table.Column<string>(maxLength: 10, nullable: true),
                    Ville = table.Column<string>(maxLength: 50, nullable: true),
                    PaysId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ADRESSE", x => x.AdresseId);
                    table.ForeignKey(
                        name: "FK_FRED_ADRESSE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ADRESSE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ADRESSE_FRED_PAYS_PaysId",
                        column: x => x.PaysId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AVIS",
                schema: "dbo",
                columns: table => new
                {
                    AvisId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(nullable: true),
                    Commentaire = table.Column<string>(maxLength: 500, nullable: true),
                    TypeAvis = table.Column<int>(nullable: false),
                    ExpediteurId = table.Column<int>(nullable: true),
                    DestinataireId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AVIS", x => x.AvisId);
                    table.ForeignKey(
                        name: "FK_FRED_AVIS_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AVIS_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AVIS_FRED_UTILISATEUR_DestinataireId",
                        column: x => x.DestinataireId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AVIS_FRED_UTILISATEUR_ExpediteurId",
                        column: x => x.ExpediteurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_COPY_HISTO",
                schema: "dbo",
                columns: table => new
                {
                    BudgetCopyHistoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetId = table.Column<int>(nullable: false),
                    BudgetSourceCIId = table.Column<int>(nullable: false),
                    BudgetSourceVersion = table.Column<string>(nullable: false),
                    BibliothequePrixSourceCIId = table.Column<int>(nullable: true),
                    DateCopy = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_COPY_HISTO", x => x.BudgetCopyHistoId);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_COPY_HISTO_FRED_CI_BibliothequePrixSourceCIId",
                        column: x => x.BibliothequePrixSourceCIId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CI",
                        principalColumn: "CiId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_COPY_HISTO_FRED_BUDGET_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "dbo",
                        principalTable: "FRED_BUDGET",
                        principalColumn: "BudgetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_COPY_HISTO_FRED_CI_BudgetSourceCIId",
                        column: x => x.BudgetSourceCIId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CI",
                        principalColumn: "CiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AGENCE",
                schema: "dbo",
                columns: table => new
                {
                    AgenceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false),
                    Telephone = table.Column<string>(maxLength: 15, nullable: true),
                    Fax = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    SIRET = table.Column<string>(maxLength: 19, nullable: true),
                    DateCloture = table.Column<DateTime>(nullable: true),
                    AdresseId = table.Column<int>(nullable: true),
                    FournisseurId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AGENCE", x => x.AgenceId);
                    table.ForeignKey(
                        name: "FK_FRED_AGENCE_FRED_ADRESSE_AdresseId",
                        column: x => x.AdresseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ADRESSE",
                        principalColumn: "AdresseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AGENCE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AGENCE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AGENCE_FRED_FOURNISSEUR_FournisseurId",
                        column: x => x.FournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AVIS_COMMANDE",
                schema: "dbo",
                columns: table => new
                {
                    AvisCommandeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvisId = table.Column<int>(nullable: true),
                    CommandeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AVIS_COMMANDE", x => x.AvisCommandeId);
                    table.ForeignKey(
                        name: "FK_FRED_AVIS_COMMANDE_FRED_AVIS_AvisId",
                        column: x => x.AvisId,
                        principalSchema: "dbo",
                        principalTable: "FRED_AVIS",
                        principalColumn: "AvisId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AVIS_COMMANDE_FRED_COMMANDE_CommandeId",
                        column: x => x.CommandeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE",
                        principalColumn: "CommandeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AVIS_COMMANDE_AVENANT",
                schema: "dbo",
                columns: table => new
                {
                    AvisCommandeAvenantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvisId = table.Column<int>(nullable: true),
                    CommandeAvenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AVIS_COMMANDE_AVENANT", x => x.AvisCommandeAvenantId);
                    table.ForeignKey(
                        name: "FK_FRED_AVIS_COMMANDE_AVENANT_FRED_AVIS_AvisId",
                        column: x => x.AvisId,
                        principalSchema: "dbo",
                        principalTable: "FRED_AVIS",
                        principalColumn: "AvisId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AVIS_COMMANDE_AVENANT_FRED_COMMANDE_AVENANT_CommandeAvenantId",
                        column: x => x.CommandeAvenantId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE_AVENANT",
                        principalColumn: "CommandeAvenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_OrganisationId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ECRITURE_COMPTABLE_NatureId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "NatureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_AVENANT_AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_AgenceId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AgenceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ADRESSE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_ADRESSE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ADRESSE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_ADRESSE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ADRESSE_PaysId",
                schema: "dbo",
                table: "FRED_ADRESSE",
                column: "PaysId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AGENCE_AdresseId",
                schema: "dbo",
                table: "FRED_AGENCE",
                column: "AdresseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AGENCE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_AGENCE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AGENCE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_AGENCE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AGENCE_FournisseurId",
                schema: "dbo",
                table: "FRED_AGENCE",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVIS_AuteurCreationId",
                schema: "dbo",
                table: "FRED_AVIS",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVIS_AuteurModificationId",
                schema: "dbo",
                table: "FRED_AVIS",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVIS_DestinataireId",
                schema: "dbo",
                table: "FRED_AVIS",
                column: "DestinataireId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVIS_ExpediteurId",
                schema: "dbo",
                table: "FRED_AVIS",
                column: "ExpediteurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVIS_COMMANDE_AvisId",
                schema: "dbo",
                table: "FRED_AVIS_COMMANDE",
                column: "AvisId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVIS_COMMANDE_CommandeId",
                schema: "dbo",
                table: "FRED_AVIS_COMMANDE",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVIS_COMMANDE_AVENANT_AvisId",
                schema: "dbo",
                table: "FRED_AVIS_COMMANDE_AVENANT",
                column: "AvisId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVIS_COMMANDE_AVENANT_CommandeAvenantId",
                schema: "dbo",
                table: "FRED_AVIS_COMMANDE_AVENANT",
                column: "CommandeAvenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_COPY_HISTO_BibliothequePrixSourceCIId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                column: "BibliothequePrixSourceCIId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_COPY_HISTO_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_COPY_HISTO_BudgetSourceCIId",
                schema: "dbo",
                table: "FRED_BUDGET_COPY_HISTO",
                column: "BudgetSourceCIId");

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_FRED_AGENCE_AgenceId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AgenceId",
                principalSchema: "dbo",
                principalTable: "FRED_AGENCE",
                principalColumn: "AgenceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_AVENANT_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ECRITURE_COMPTABLE_FRED_NATURE_NatureId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "NatureId",
                principalSchema: "dbo",
                principalTable: "FRED_NATURE",
                principalColumn: "NatureId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_COMMANDE_FRED_AGENCE_AgenceId",
                schema: "dbo",
                table: "FRED_COMMANDE");

            migrationBuilder.DropForeignKey(
                name: "FK_FRED_COMMANDE_AVENANT_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT");

            migrationBuilder.DropForeignKey(
                name: "FK_FRED_ECRITURE_COMPTABLE_FRED_NATURE_NatureId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE");

            migrationBuilder.DropTable(
                name: "FRED_AGENCE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AVIS_COMMANDE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AVIS_COMMANDE_AVENANT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_COPY_HISTO",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ADRESSE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AVIS",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_OrganisationId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE");

            migrationBuilder.DropIndex(
                name: "IX_FRED_ECRITURE_COMPTABLE_NatureId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE");

            migrationBuilder.DropIndex(
                name: "IX_FRED_COMMANDE_AVENANT_AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT");

            migrationBuilder.DropIndex(
                name: "IX_FRED_COMMANDE_AgenceId",
                schema: "dbo",
                table: "FRED_COMMANDE");

            migrationBuilder.DropColumn(
                name: "DateDebutInsertion",
                schema: "dbo",
                table: "FRED_PERSONNEL");

            migrationBuilder.DropColumn(
                name: "DateFinInsertion",
                schema: "dbo",
                table: "FRED_PERSONNEL");

            migrationBuilder.DropColumn(
                name: "HeuresInsertion",
                schema: "dbo",
                table: "FRED_PERSONNEL");

            migrationBuilder.DropColumn(
                name: "CodeTVA",
                schema: "dbo",
                table: "FRED_FOURNISSEUR");

            migrationBuilder.DropColumn(
                name: "NatureId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE");

            migrationBuilder.DropColumn(
                name: "AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT");

            migrationBuilder.DropColumn(
                name: "AgenceId",
                schema: "dbo",
                table: "FRED_COMMANDE");

            migrationBuilder.DropColumn(
                name: "ObjectifHeuresInsertion",
                schema: "dbo",
                table: "FRED_CI");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEntree",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FRED_VISA",
                schema: "dbo",
                columns: table => new
                {
                    VisaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommandeId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    UtilisateurId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_VISA", x => x.VisaId);
                    table.ForeignKey(
                        name: "FK_FRED_VISA_FRED_COMMANDE_CommandeId",
                        column: x => x.CommandeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE",
                        principalColumn: "CommandeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_VISA_FRED_UTILISATEUR_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_OrganisationId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                column: "OrganisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VISA_CommandeId",
                schema: "dbo",
                table: "FRED_VISA",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VISA_UtilisateurId",
                schema: "dbo",
                table: "FRED_VISA",
                column: "UtilisateurId");
        }
    }
}
