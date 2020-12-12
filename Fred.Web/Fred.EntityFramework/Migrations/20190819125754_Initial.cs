using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DbUp.Engine;

namespace Fred.EntityFramework.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "nlog");

            migrationBuilder.CreateTable(
                name: "FRED_AFFECTATION_MOYEN_FAMILLE",
                schema: "dbo",
                columns: table => new
                {
                    AffectationMoyenFamilleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Libelle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AFFECTATION_MOYEN_FAMILLE", x => x.AffectationMoyenFamilleId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AUTHENTIFICATION_LOG",
                schema: "dbo",
                columns: table => new
                {
                    AuthentificationLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    ErrorType = table.Column<int>(nullable: false),
                    ErrorOrigin = table.Column<int>(nullable: false),
                    AdressIp = table.Column<string>(nullable: true),
                    RequestedUrl = table.Column<string>(nullable: true),
                    TechnicalError = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AUTHENTIFICATION_LOG", x => x.AuthentificationLogId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AVANCEMENT_ETAT",
                schema: "dbo",
                columns: table => new
                {
                    AvancementEtatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Libelle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AVANCEMENT_ETAT", x => x.AvancementEtatId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_ETAT",
                schema: "dbo",
                columns: table => new
                {
                    BudgetEtatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Libelle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_ETAT", x => x.BudgetEtatId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CI_TYPE",
                schema: "dbo",
                columns: table => new
                {
                    CITypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Designation = table.Column<string>(nullable: true),
                    RessourceKey = table.Column<string>(nullable: true),
                    Code = table.Column<string>(maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CI_TYPE", x => x.CITypeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CODE_ASTREINTE",
                schema: "dbo",
                columns: table => new
                {
                    CodeAstreinteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EstSorti = table.Column<bool>(nullable: false),
                    GroupeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CODE_ASTREINTE", x => x.CodeAstreinteId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_COMMANDE_TYPE",
                schema: "dbo",
                columns: table => new
                {
                    CommandeTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_COMMANDE_TYPE", x => x.CommandeTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_DEPENSE_TYPE",
                schema: "dbo",
                columns: table => new
                {
                    DepenseTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false, defaultValue: 0),
                    Libelle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_DEPENSE_TYPE", x => x.DepenseTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_DEVISE",
                schema: "dbo",
                columns: table => new
                {
                    DeviseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsoCode = table.Column<string>(maxLength: 3, nullable: true),
                    IsoNombre = table.Column<string>(maxLength: 5, nullable: true),
                    Symbole = table.Column<string>(maxLength: 10, nullable: true),
                    CodeHtml = table.Column<string>(maxLength: 10, nullable: true),
                    Libelle = table.Column<string>(maxLength: 150, nullable: true),
                    CodePaysIso = table.Column<string>(type: "nvarchar(2)", nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModification = table.Column<int>(nullable: true),
                    AuteurSuppression = table.Column<int>(nullable: true),
                    AuteurCreation = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_DEVISE", x => x.DeviseId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ECRITURE_COMPTABLE_REJET",
                schema: "dbo",
                columns: table => new
                {
                    EcritureComptableRejetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NumeroPiece = table.Column<string>(nullable: true),
                    DateRejet = table.Column<DateTime>(type: "datetime", nullable: false),
                    CiID = table.Column<int>(nullable: false),
                    RejetMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ECRITURE_COMPTABLE_REJET", x => x.EcritureComptableRejetId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_EXTERNALDIRECTORY",
                schema: "dbo",
                columns: table => new
                {
                    FayatAccessDirectoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MotDePasse = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    DateExpiration = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActived = table.Column<bool>(nullable: false),
                    Guid = table.Column<string>(nullable: true),
                    DateExpirationGuid = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_EXTERNALDIRECTORY", x => x.FayatAccessDirectoryId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FACTURATION_TYPE",
                schema: "dbo",
                columns: table => new
                {
                    FacturationTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false, defaultValue: 0),
                    Libelle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FACTURATION_TYPE", x => x.FacturationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FEATURE_FLIPPING",
                schema: "dbo",
                columns: table => new
                {
                    FeatureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false, defaultValue: 0),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    IsActived = table.Column<bool>(nullable: false),
                    DateActivation = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserActivation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FEATURE_FLIPPING", x => x.FeatureId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_GROUPE_REMPLACEMENT_TACHE",
                schema: "dbo",
                columns: table => new
                {
                    GroupeRemplacementTacheId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_GROUPE_REMPLACEMENT_TACHE", x => x.GroupeRemplacementTacheId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_IMAGE",
                schema: "dbo",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Path = table.Column<string>(nullable: true),
                    Credit = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_IMAGE", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_INDEMNITE_DEPLACEMENT_CALCUL_TYPE",
                schema: "dbo",
                columns: table => new
                {
                    IndemniteDeplacementCalculTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Libelle = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_INDEMNITE_DEPLACEMENT_CALCUL_TYPE", x => x.IndemniteDeplacementCalculTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_LOG_IMPORT",
                schema: "dbo",
                columns: table => new
                {
                    LogImportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeImport = table.Column<string>(maxLength: 50, nullable: false),
                    MessageErreur = table.Column<string>(maxLength: 500, nullable: false),
                    Data = table.Column<string>(nullable: false),
                    DateImport = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_LOG_IMPORT", x => x.LogImportId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_MODULE",
                schema: "dbo",
                columns: table => new
                {
                    ModuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Libelle = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_MODULE", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_MOTIF_REMPLACEMENT",
                schema: "dbo",
                columns: table => new
                {
                    MotifRemplacementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 5, nullable: false),
                    Libelle = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_MOTIF_REMPLACEMENT", x => x.MotifRemplacementId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_NOTIFICATION_UTILISATEUR",
                schema: "dbo",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UtilisateurId = table.Column<int>(nullable: false),
                    TypeNotification = table.Column<int>(nullable: false, defaultValue: 0),
                    Message = table.Column<string>(nullable: false, defaultValue: ""),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    EstConsulte = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_NOTIFICATION_UTILISATEUR", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PAYS",
                schema: "dbo",
                columns: table => new
                {
                    PaysId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(3)", fixedLength: true, nullable: true),
                    Libelle = table.Column<string>(type: "nvarchar(50)", fixedLength: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PAYS", x => x.PaysId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PERMISSION",
                schema: "dbo",
                columns: table => new
                {
                    PermissionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PermissionKey = table.Column<string>(nullable: true),
                    PermissionType = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Libelle = table.Column<string>(nullable: true),
                    PermissionContextuelle = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PERMISSION", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PERSONNEL_IMAGE",
                schema: "dbo",
                columns: table => new
                {
                    PersonnelImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PersonnelId = table.Column<int>(nullable: false),
                    Signature = table.Column<byte[]>(type: "image", nullable: true),
                    PhotoProfil = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PERSONNEL_IMAGE", x => x.PersonnelImageId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORT_STATUT",
                schema: "dbo",
                columns: table => new
                {
                    RapportStatutId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORT_STATUT", x => x.RapportStatutId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_SITE",
                schema: "dbo",
                columns: table => new
                {
                    SiteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Libelle = table.Column<string>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_SITE", x => x.SiteId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_SOCIETE_CLASSIFICATION",
                schema: "dbo",
                columns: table => new
                {
                    SocieteClassificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 4000, nullable: false),
                    Libelle = table.Column<string>(maxLength: 4000, nullable: false),
                    Statut = table.Column<bool>(nullable: false),
                    GroupeId = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_SOCIETE_CLASSIFICATION", x => x.SocieteClassificationId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_STATUT_COMMANDE",
                schema: "dbo",
                columns: table => new
                {
                    StatutCommandeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_STATUT_COMMANDE", x => x.StatutCommandeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_SYSTEME_EXTERNE_TYPE",
                schema: "dbo",
                columns: table => new
                {
                    SystemeExterneTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Libelle = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_SYSTEME_EXTERNE_TYPE", x => x.SystemeExterneTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_SYSTEME_IMPORT",
                schema: "dbo",
                columns: table => new
                {
                    SystemeImportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Libelle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_SYSTEME_IMPORT", x => x.SystemeImportId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TYPE_DEPENSE",
                schema: "dbo",
                columns: table => new
                {
                    TypeDepenseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TYPE_DEPENSE", x => x.TypeDepenseId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TYPE_ENERGIE",
                schema: "dbo",
                columns: table => new
                {
                    TypeEnergieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Libelle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TYPE_ENERGIE", x => x.TypeEnergieId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TYPE_ORGANISATION",
                schema: "dbo",
                columns: table => new
                {
                    TypeOrganisationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TYPE_ORGANISATION", x => x.TypeOrganisationId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TYPE_PARTICIPATION_SEP",
                schema: "dbo",
                columns: table => new
                {
                    TypeParticipationSepId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: false),
                    Libelle = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TYPE_PARTICIPATION_SEP", x => x.TypeParticipationSepId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TYPE_RESSOURCE",
                schema: "dbo",
                columns: table => new
                {
                    TypeRessourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TYPE_RESSOURCE", x => x.TypeRessourceId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TYPE_SOCIETE",
                schema: "dbo",
                columns: table => new
                {
                    TypeSocieteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: false),
                    Libelle = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TYPE_SOCIETE", x => x.TypeSocieteId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_UNITE",
                schema: "dbo",
                columns: table => new
                {
                    UniteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_UNITE", x => x.UniteId);
                });

            migrationBuilder.CreateTable(
                name: "NLogs",
                schema: "nlog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Application = table.Column<string>(maxLength: 50, nullable: true),
                    Logged = table.Column<DateTime>(type: "datetime", nullable: false),
                    Level = table.Column<string>(maxLength: 50, nullable: true),
                    Message = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(maxLength: 250, nullable: true),
                    ServerName = table.Column<string>(nullable: true),
                    Port = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Https = table.Column<bool>(nullable: false),
                    ServerAddress = table.Column<string>(maxLength: 100, nullable: true),
                    RemoteAddress = table.Column<string>(maxLength: 100, nullable: true),
                    Logger = table.Column<string>(maxLength: 250, nullable: true),
                    Callsite = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AFFECTATION_MOYEN_TYPE",
                schema: "dbo",
                columns: table => new
                {
                    AffectationMoyenTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Libelle = table.Column<string>(nullable: true),
                    CiCode = table.Column<string>(nullable: true),
                    AffectationMoyenFamilleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AFFECTATION_MOYEN_TYPE", x => x.AffectationMoyenTypeId);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_MOYEN_TYPE_FRED_AFFECTATION_MOYEN_FAMILLE_AffectationMoyenFamilleId",
                        column: x => x.AffectationMoyenFamilleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_AFFECTATION_MOYEN_FAMILLE",
                        principalColumn: "AffectationMoyenFamilleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FONCTIONNALITE",
                schema: "dbo",
                columns: table => new
                {
                    FonctionnaliteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModuleId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Libelle = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    HorsOrga = table.Column<bool>(nullable: false),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FONCTIONNALITE", x => x.FonctionnaliteId);
                    table.ForeignKey(
                        name: "FK_FRED_FONCTIONNALITE_FRED_MODULE_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_MODULE",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ORGANISATION",
                schema: "dbo",
                columns: table => new
                {
                    OrganisationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeOrganisationId = table.Column<int>(nullable: false),
                    PereId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ORGANISATION", x => x.OrganisationId);
                    table.ForeignKey(
                        name: "FK_FRED_ORGANISATION_FRED_ORGANISATION_PereId",
                        column: x => x.PereId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ORGANISATION_FRED_TYPE_ORGANISATION_TypeOrganisationId",
                        column: x => x.TypeOrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TYPE_ORGANISATION",
                        principalColumn: "TypeOrganisationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CARBURANT",
                schema: "dbo",
                columns: table => new
                {
                    CarburantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UniteId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CARBURANT", x => x.CarburantId);
                    table.ForeignKey(
                        name: "FK_FRED_CARBURANT_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PERMISSION_FONCTIONNALITE",
                schema: "dbo",
                columns: table => new
                {
                    PermissionFonctionnaliteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PermissionId = table.Column<int>(nullable: false),
                    FonctionnaliteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PERMISSION_FONCTIONNALITE", x => x.PermissionFonctionnaliteId);
                    table.ForeignKey(
                        name: "FK_FRED_PERMISSION_FONCTIONNALITE_FRED_FONCTIONNALITE_FonctionnaliteId",
                        column: x => x.FonctionnaliteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FONCTIONNALITE",
                        principalColumn: "FonctionnaliteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_PERMISSION_FONCTIONNALITE_FRED_PERMISSION_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERMISSION",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_HOLDING",
                schema: "dbo",
                columns: table => new
                {
                    HoldingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(type: "varchar(20)", fixedLength: true, nullable: false),
                    Libelle = table.Column<string>(type: "varchar(500)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_HOLDING", x => x.HoldingId);
                    table.ForeignKey(
                        name: "FK_FRED_HOLDING_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ORGANISATION_GENERIQUE",
                schema: "dbo",
                columns: table => new
                {
                    OrganisationGeneriqueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ORGANISATION_GENERIQUE", x => x.OrganisationGeneriqueId);
                    table.ForeignKey(
                        name: "FK_FRED_ORGANISATION_GENERIQUE_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_POLE",
                schema: "dbo",
                columns: table => new
                {
                    PoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", fixedLength: true, nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(500)", fixedLength: true, nullable: false),
                    HoldingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_POLE", x => x.PoleId);
                    table.ForeignKey(
                        name: "FK_FRED_POLE_FRED_HOLDING_HoldingId",
                        column: x => x.HoldingId,
                        principalSchema: "dbo",
                        principalTable: "FRED_HOLDING",
                        principalColumn: "HoldingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_POLE_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_GROUPE",
                schema: "dbo",
                columns: table => new
                {
                    GroupeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", fixedLength: true, nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(500)", fixedLength: true, nullable: false),
                    PoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_GROUPE", x => x.GroupeId);
                    table.ForeignKey(
                        name: "FK_FRED_GROUPE_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_GROUPE_FRED_POLE_PoleId",
                        column: x => x.PoleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_POLE",
                        principalColumn: "PoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FOURNISSEUR",
                schema: "dbo",
                columns: table => new
                {
                    FournisseurId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupeId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    Adresse = table.Column<string>(maxLength: 250, nullable: true),
                    CodePostal = table.Column<string>(maxLength: 10, nullable: true),
                    Ville = table.Column<string>(maxLength: 50, nullable: true),
                    SIRET = table.Column<string>(maxLength: 19, nullable: true),
                    SIREN = table.Column<string>(maxLength: 19, nullable: true),
                    Telephone = table.Column<string>(maxLength: 15, nullable: true),
                    Fax = table.Column<string>(maxLength: 15, nullable: true),
                    PaysId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    ModeReglement = table.Column<string>(maxLength: 255, nullable: true),
                    DateOuverture = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCloture = table.Column<DateTime>(type: "datetime", nullable: true),
                    TypeSequence = table.Column<string>(maxLength: 255, nullable: true),
                    TypeTiers = table.Column<string>(fixedLength: true, maxLength: 1, nullable: true),
                    RegleGestion = table.Column<string>(nullable: true),
                    IsProfessionLiberale = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FOURNISSEUR", x => x.FournisseurId);
                    table.ForeignKey(
                        name: "FK_FRED_FOURNISSEUR_FRED_GROUPE_GroupeId",
                        column: x => x.GroupeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE",
                        principalColumn: "GroupeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FOURNISSEUR_FRED_PAYS_PaysId",
                        column: x => x.PaysId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PARAMETRE",
                schema: "dbo",
                columns: table => new
                {
                    ParametreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false),
                    Valeur = table.Column<string>(maxLength: 500, nullable: false),
                    GroupeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PARAMETRE", x => x.ParametreId);
                    table.ForeignKey(
                        name: "FK_FRED_PARAMETRE_FRED_GROUPE_GroupeId",
                        column: x => x.GroupeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE",
                        principalColumn: "GroupeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_SOCIETE",
                schema: "dbo",
                columns: table => new
                {
                    SocieteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    GroupeId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    CodeSocietePaye = table.Column<string>(maxLength: 20, nullable: true),
                    CodeSocieteComptable = table.Column<string>(maxLength: 20, nullable: true),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    Adresse = table.Column<string>(maxLength: 250, nullable: true),
                    Ville = table.Column<string>(maxLength: 50, nullable: true),
                    CodePostal = table.Column<string>(maxLength: 10, nullable: true),
                    SIRET = table.Column<string>(nullable: true),
                    SIREN = table.Column<string>(maxLength: 19, nullable: true),
                    Externe = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MoisDebutExercice = table.Column<int>(nullable: true),
                    MoisFinExercice = table.Column<int>(nullable: true),
                    IsGenerationSamediCPActive = table.Column<bool>(nullable: false, defaultValue: false),
                    ImportFacture = table.Column<bool>(nullable: false, defaultValue: false),
                    TransfertAS400 = table.Column<bool>(nullable: false, defaultValue: false),
                    ImageScreenLogin = table.Column<string>(nullable: true),
                    ImageLogoHeader = table.Column<string>(nullable: true),
                    ImageLogoId = table.Column<int>(nullable: true),
                    CGAFournitureId = table.Column<int>(nullable: true),
                    CGALocationId = table.Column<int>(nullable: true),
                    CGAPrestationId = table.Column<int>(nullable: true),
                    IsInterimaire = table.Column<bool>(nullable: false, defaultValue: false),
                    ImageLoginId = table.Column<int>(nullable: true),
                    PiedDePage = table.Column<string>(maxLength: 400, nullable: true),
                    CodeSocieteStorm = table.Column<string>(nullable: true),
                    IndemniteDeplacementCalculTypeId = table.Column<int>(nullable: true),
                    TypeSocieteId = table.Column<int>(nullable: true),
                    FournisseurId = table.Column<int>(nullable: true),
                    EtablissementParDefaut = table.Column<bool>(nullable: false, defaultValue: false),
                    SocieteClassificationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_SOCIETE", x => x.SocieteId);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_IMAGE_CGAFournitureId",
                        column: x => x.CGAFournitureId,
                        principalSchema: "dbo",
                        principalTable: "FRED_IMAGE",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_IMAGE_CGALocationId",
                        column: x => x.CGALocationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_IMAGE",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_IMAGE_CGAPrestationId",
                        column: x => x.CGAPrestationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_IMAGE",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_FOURNISSEUR_FournisseurId",
                        column: x => x.FournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_GROUPE_GroupeId",
                        column: x => x.GroupeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE",
                        principalColumn: "GroupeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_IMAGE_ImageLoginId",
                        column: x => x.ImageLoginId,
                        principalSchema: "dbo",
                        principalTable: "FRED_IMAGE",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_IMAGE_ImageLogoId",
                        column: x => x.ImageLogoId,
                        principalSchema: "dbo",
                        principalTable: "FRED_IMAGE",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_INDEMNITE_DEPLACEMENT_CALCUL_TYPE_IndemniteDeplacementCalculTypeId",
                        column: x => x.IndemniteDeplacementCalculTypeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_INDEMNITE_DEPLACEMENT_CALCUL_TYPE",
                        principalColumn: "IndemniteDeplacementCalculTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_SOCIETE_CLASSIFICATION_SocieteClassificationId",
                        column: x => x.SocieteClassificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE_CLASSIFICATION",
                        principalColumn: "SocieteClassificationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_FRED_TYPE_SOCIETE_TypeSocieteId",
                        column: x => x.TypeSocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TYPE_SOCIETE",
                        principalColumn: "TypeSocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ASSOCIE_SEP",
                schema: "dbo",
                columns: table => new
                {
                    AssocieSepId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: false),
                    SocieteAssocieeId = table.Column<int>(nullable: false),
                    TypeParticipationSepId = table.Column<int>(nullable: false),
                    QuotePart = table.Column<decimal>(type: "numeric(12, 2)", nullable: false),
                    FournisseurId = table.Column<int>(nullable: false),
                    AssocieSepParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ASSOCIE_SEP", x => x.AssocieSepId);
                    table.ForeignKey(
                        name: "FK_FRED_ASSOCIE_SEP_FRED_ASSOCIE_SEP_AssocieSepParentId",
                        column: x => x.AssocieSepParentId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ASSOCIE_SEP",
                        principalColumn: "AssocieSepId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ASSOCIE_SEP_FRED_FOURNISSEUR_FournisseurId",
                        column: x => x.FournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ASSOCIE_SEP_FRED_SOCIETE_SocieteAssocieeId",
                        column: x => x.SocieteAssocieeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ASSOCIE_SEP_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ASSOCIE_SEP_FRED_TYPE_PARTICIPATION_SEP_TypeParticipationSepId",
                        column: x => x.TypeParticipationSepId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TYPE_PARTICIPATION_SEP",
                        principalColumn: "TypeParticipationSepId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CODE_ABSENCE",
                schema: "dbo",
                columns: table => new
                {
                    CodeAbsenceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: true),
                    HoldingId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    Intemperie = table.Column<bool>(nullable: false),
                    TauxDecote = table.Column<double>(nullable: false),
                    NBHeuresDefautETAM = table.Column<double>(nullable: false),
                    NBHeuresMinETAM = table.Column<double>(nullable: false),
                    NBHeuresMaxETAM = table.Column<double>(nullable: false),
                    NBHeuresDefautCO = table.Column<double>(nullable: false),
                    NBHeuresMinCO = table.Column<double>(nullable: false),
                    NBHeuresMaxCO = table.Column<double>(nullable: false),
                    Actif = table.Column<bool>(nullable: false),
                    GroupeId = table.Column<int>(nullable: true),
                    CodeAbsenceParentId = table.Column<int>(nullable: true),
                    Niveau = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CODE_ABSENCE", x => x.CodeAbsenceId);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_ABSENCE_FRED_CODE_ABSENCE_CodeAbsenceParentId",
                        column: x => x.CodeAbsenceParentId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CODE_ABSENCE",
                        principalColumn: "CodeAbsenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_ABSENCE_FRED_GROUPE_GroupeId",
                        column: x => x.GroupeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE",
                        principalColumn: "GroupeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_ABSENCE_FRED_HOLDING_HoldingId",
                        column: x => x.HoldingId,
                        principalSchema: "dbo",
                        principalTable: "FRED_HOLDING",
                        principalColumn: "HoldingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_ABSENCE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CODE_DEPLACEMENT",
                schema: "dbo",
                columns: table => new
                {
                    CodeDeplacementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false),
                    KmMini = table.Column<int>(nullable: false),
                    KmMaxi = table.Column<int>(nullable: false),
                    IGD = table.Column<bool>(nullable: false),
                    IndemniteForfaitaire = table.Column<bool>(nullable: false),
                    Actif = table.Column<bool>(nullable: false),
                    SocieteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CODE_DEPLACEMENT", x => x.CodeDeplacementId);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_DEPLACEMENT_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_DATES_CALENDRIER_PAIE",
                schema: "dbo",
                columns: table => new
                {
                    DatesCalendrierPaieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: false),
                    DateFinPointages = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateTransfertPointages = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_DATES_CALENDRIER_PAIE", x => x.DatesCalendrierPaieId);
                    table.ForeignKey(
                        name: "FK_FRED_DATES_CALENDRIER_PAIE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FONCTIONNALITE_DESACTIVE",
                schema: "dbo",
                columns: table => new
                {
                    FonctionnaliteDesactiveId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: false),
                    FonctionnaliteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FONCTIONNALITE_DESACTIVE", x => x.FonctionnaliteDesactiveId);
                    table.ForeignKey(
                        name: "FK_FRED_FONCTIONNALITE_DESACTIVE_FRED_FONCTIONNALITE_FonctionnaliteId",
                        column: x => x.FonctionnaliteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FONCTIONNALITE",
                        principalColumn: "FonctionnaliteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_FONCTIONNALITE_DESACTIVE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_MODULE_DESACTIVE",
                schema: "dbo",
                columns: table => new
                {
                    ModuleDesactiveId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_MODULE_DESACTIVE", x => x.ModuleDesactiveId);
                    table.ForeignKey(
                        name: "FK_FRED_MODULE_DESACTIVE_FRED_MODULE_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_MODULE",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_MODULE_DESACTIVE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ROLE",
                schema: "dbo",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(type: "varchar(255)", unicode: false, nullable: true),
                    CodeNomFamilier = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(255)", unicode: false, nullable: true),
                    CommandeSeuilDefaut = table.Column<string>(type: "varchar(255)", unicode: false, nullable: true),
                    ModeLecture = table.Column<bool>(nullable: false),
                    Actif = table.Column<bool>(nullable: false),
                    NiveauPaie = table.Column<int>(nullable: false),
                    NiveauCompta = table.Column<int>(nullable: false),
                    Specification = table.Column<int>(nullable: true),
                    SocieteId = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ROLE", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_FRED_ROLE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_SOCIETE_DEVISE",
                schema: "dbo",
                columns: table => new
                {
                    SocieteDeviseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    DeviseDeReference = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_SOCIETE_DEVISE", x => x.SocieteDeviseId);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_DEVISE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_DEVISE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_SYSTEME_EXTERNE",
                schema: "dbo",
                columns: table => new
                {
                    SystemeExterneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Libelle = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    SocieteId = table.Column<int>(nullable: false),
                    SystemeExterneTypeId = table.Column<int>(nullable: false),
                    SystemeImportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_SYSTEME_EXTERNE", x => x.SystemeExterneId);
                    table.ForeignKey(
                        name: "FK_FRED_SYSTEME_EXTERNE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SYSTEME_EXTERNE_FRED_SYSTEME_EXTERNE_TYPE_SystemeExterneTypeId",
                        column: x => x.SystemeExterneTypeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SYSTEME_EXTERNE_TYPE",
                        principalColumn: "SystemeExterneTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SYSTEME_EXTERNE_FRED_SYSTEME_IMPORT_SystemeImportId",
                        column: x => x.SystemeImportId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SYSTEME_IMPORT",
                        principalColumn: "SystemeImportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TRANSCO_IMPORT",
                schema: "dbo",
                columns: table => new
                {
                    TranscoImportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodeInterne = table.Column<string>(maxLength: 50, nullable: false),
                    CodeExterne = table.Column<string>(maxLength: 50, nullable: false),
                    SocieteId = table.Column<int>(nullable: false),
                    SystemeImportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TRANSCO_IMPORT", x => x.TranscoImportId);
                    table.ForeignKey(
                        name: "FK_FRED_TRANSCO_IMPORT_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_TRANSCO_IMPORT_FRED_SYSTEME_IMPORT_SystemeImportId",
                        column: x => x.SystemeImportId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SYSTEME_IMPORT",
                        principalColumn: "SystemeImportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_UNITE_SOCIETE",
                schema: "dbo",
                columns: table => new
                {
                    UniteSocieteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UniteId = table.Column<int>(nullable: false),
                    SocieteId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_UNITE_SOCIETE", x => x.UniteSocieteId);
                    table.ForeignKey(
                        name: "FK_FRED_UNITE_SOCIETE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_UNITE_SOCIETE_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ROLE_DEVISE",
                schema: "dbo",
                columns: table => new
                {
                    SeuilValidationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeviseId = table.Column<int>(nullable: false),
                    Montant = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ROLE_DEVISE", x => x.SeuilValidationId);
                    table.ForeignKey(
                        name: "FK_FRED_ROLE_DEVISE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ROLE_DEVISE_FRED_ROLE_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ROLE",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ROLE_FONCTIONNALITE",
                schema: "dbo",
                columns: table => new
                {
                    RoleFonctionnaliteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    FonctionnaliteId = table.Column<int>(nullable: false),
                    Mode = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ROLE_FONCTIONNALITE", x => x.RoleFonctionnaliteId);
                    table.ForeignKey(
                        name: "FK_FRED_ROLE_FONCTIONNALITE_FRED_FONCTIONNALITE_FonctionnaliteId",
                        column: x => x.FonctionnaliteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FONCTIONNALITE",
                        principalColumn: "FonctionnaliteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_ROLE_FONCTIONNALITE_FRED_ROLE_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ROLE",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ROLE_ORGANISATION_DEVISE",
                schema: "dbo",
                columns: table => new
                {
                    SeuilRoleOrgaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: true),
                    DeviseId = table.Column<int>(nullable: false),
                    OrganisationId = table.Column<int>(nullable: true),
                    Seuil = table.Column<decimal>(type: "decimal(11, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ROLE_ORGANISATION_DEVISE", x => x.SeuilRoleOrgaId);
                    table.ForeignKey(
                        name: "FK_FRED_ROLE_ORGANISATION_DEVISE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ROLE_ORGANISATION_DEVISE_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ROLE_ORGANISATION_DEVISE_FRED_ROLE_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ROLE",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ASTREINTE",
                schema: "dbo",
                columns: table => new
                {
                    AstreintId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AffectationId = table.Column<int>(nullable: false),
                    DateAstreinte = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ASTREINTE", x => x.AstreintId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORT_LIGNE",
                schema: "dbo",
                columns: table => new
                {
                    RapportLigneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RapportId = table.Column<int>(nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    AffectationMoyenId = table.Column<int>(nullable: true),
                    PrenomNomTemporaire = table.Column<string>(maxLength: 100, nullable: true),
                    PersonnelId = table.Column<int>(nullable: true),
                    HeureNormale = table.Column<double>(type: "float", nullable: false),
                    CodeMajorationId = table.Column<int>(nullable: true),
                    HeureMajoration = table.Column<double>(type: "float", nullable: false),
                    CodeAbsenceId = table.Column<int>(nullable: true),
                    HeureAbsence = table.Column<double>(type: "float", nullable: false),
                    NumSemaineIntemperieAbsence = table.Column<int>(nullable: true),
                    CodeDeplacementId = table.Column<int>(nullable: true),
                    CodeZoneDeplacementId = table.Column<int>(nullable: true),
                    CodeZoneDeplacementSaisiManuellement = table.Column<bool>(nullable: false, defaultValue: false),
                    DeplacementIV = table.Column<bool>(nullable: false),
                    MaterielMarche = table.Column<double>(type: "float", nullable: false),
                    MaterielArret = table.Column<double>(type: "float", nullable: false),
                    MaterielPanne = table.Column<double>(type: "float", nullable: false),
                    MaterielIntemperie = table.Column<double>(type: "float", nullable: false),
                    DatePointage = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaterielId = table.Column<int>(nullable: true),
                    AvecChauffeur = table.Column<bool>(nullable: false, defaultValue: false),
                    MaterielNomTemporaire = table.Column<string>(maxLength: 100, nullable: true),
                    LotPointageId = table.Column<int>(nullable: true),
                    IsGenerated = table.Column<bool>(nullable: false, defaultValue: false),
                    ReceptionInterimaire = table.Column<bool>(nullable: false, defaultValue: false),
                    ReceptionMaterielExterne = table.Column<bool>(nullable: false, defaultValue: false),
                    RapportLigneStatutId = table.Column<int>(nullable: true),
                    ValideurId = table.Column<int>(nullable: true),
                    DateValidation = table.Column<DateTime>(type: "datetime", nullable: true),
                    HeuresTotalAstreintes = table.Column<double>(nullable: false, defaultValue: 0.0),
                    HeuresMachine = table.Column<double>(nullable: false, defaultValue: 0.0),
                    Commentaire = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORT_LIGNE", x => x.RapportLigneId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_FRED_CODE_ABSENCE_CodeAbsenceId",
                        column: x => x.CodeAbsenceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CODE_ABSENCE",
                        principalColumn: "CodeAbsenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_FRED_CODE_DEPLACEMENT_CodeDeplacementId",
                        column: x => x.CodeDeplacementId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CODE_DEPLACEMENT",
                        principalColumn: "CodeDeplacementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_FRED_RAPPORT_STATUT_RapportLigneStatutId",
                        column: x => x.RapportLigneStatutId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_STATUT",
                        principalColumn: "RapportStatutId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORT_LIGNE_ASTREINTE",
                schema: "dbo",
                columns: table => new
                {
                    RapportLigneAstreinteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RapportLigneId = table.Column<int>(nullable: false),
                    AstreinteId = table.Column<int>(nullable: false),
                    DateDebutAstreinte = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFinAstreinte = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORT_LIGNE_ASTREINTE", x => x.RapportLigneAstreinteId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_ASTREINTE_FRED_ASTREINTE_AstreinteId",
                        column: x => x.AstreinteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ASTREINTE",
                        principalColumn: "AstreintId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_ASTREINTE_FRED_RAPPORT_LIGNE_RapportLigneId",
                        column: x => x.RapportLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_LIGNE",
                        principalColumn: "RapportLigneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORT_LIGNE_CODE_ASTREINTE",
                schema: "dbo",
                columns: table => new
                {
                    RapportLigneCodeAstreinteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RapportLigneId = table.Column<int>(nullable: false),
                    CodeAstreinteId = table.Column<int>(nullable: false),
                    RapportLigneAstreinteId = table.Column<int>(nullable: true),
                    IsPrimeNuit = table.Column<bool>(nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORT_LIGNE_CODE_ASTREINTE", x => x.RapportLigneCodeAstreinteId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_CODE_ASTREINTE_FRED_CODE_ASTREINTE_CodeAstreinteId",
                        column: x => x.CodeAstreinteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CODE_ASTREINTE",
                        principalColumn: "CodeAstreinteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_CODE_ASTREINTE_FRED_RAPPORT_LIGNE_ASTREINTE_RapportLigneAstreinteId",
                        column: x => x.RapportLigneAstreinteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_LIGNE_ASTREINTE",
                        principalColumn: "RapportLigneAstreinteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_CODE_ASTREINTE_FRED_RAPPORT_LIGNE_RapportLigneId",
                        column: x => x.RapportLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_LIGNE",
                        principalColumn: "RapportLigneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AFFECTATION_MOYEN",
                schema: "dbo",
                columns: table => new
                {
                    AffectationMoyenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MaterielId = table.Column<int>(nullable: false, defaultValue: 0),
                    CiId = table.Column<int>(nullable: true),
                    PersonnelId = table.Column<int>(nullable: true),
                    ConducteurId = table.Column<int>(nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    AffectationMoyenTypeId = table.Column<int>(nullable: false),
                    SiteId = table.Column<int>(nullable: true),
                    Commentaire = table.Column<string>(nullable: true),
                    MaterielLocationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AFFECTATION_MOYEN", x => x.AffectationMoyenId);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_MOYEN_FRED_AFFECTATION_MOYEN_TYPE_AffectationMoyenTypeId",
                        column: x => x.AffectationMoyenTypeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_AFFECTATION_MOYEN_TYPE",
                        principalColumn: "AffectationMoyenTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_AFFECTATION_MOYEN_FRED_SITE_SiteId",
                        column: x => x.SiteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SITE",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORTPRIME_LIGNE_ASTREINTE",
                schema: "dbo",
                columns: table => new
                {
                    RapportPrimeLigneAstreinteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RapportPrimeLigneId = table.Column<int>(nullable: false),
                    AstreinteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORTPRIME_LIGNE_ASTREINTE", x => x.RapportPrimeLigneAstreinteId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORTPRIME_LIGNE_ASTREINTE_FRED_ASTREINTE_AstreinteId",
                        column: x => x.AstreinteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ASTREINTE",
                        principalColumn: "AstreintId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AVANCEMENT_WORKFLOW",
                schema: "dbo",
                columns: table => new
                {
                    AvancementWorkflowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvancementId = table.Column<int>(nullable: false),
                    EtatInitialId = table.Column<int>(nullable: true),
                    EtatCibleId = table.Column<int>(nullable: false),
                    AuteurId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AVANCEMENT_WORKFLOW", x => x.AvancementWorkflowId);
                    table.ForeignKey(
                        name: "FK_FRED_AVANCEMENT_WORKFLOW_FRED_AVANCEMENT_ETAT_EtatCibleId",
                        column: x => x.EtatCibleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_AVANCEMENT_ETAT",
                        principalColumn: "AvancementEtatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AVANCEMENT_WORKFLOW_FRED_AVANCEMENT_ETAT_EtatInitialId",
                        column: x => x.EtatInitialId,
                        principalSchema: "dbo",
                        principalTable: "FRED_AVANCEMENT_ETAT",
                        principalColumn: "AvancementEtatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AVANCEMENT",
                schema: "dbo",
                columns: table => new
                {
                    AvancementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetSousDetailId = table.Column<int>(nullable: false, defaultValue: 0),
                    CiId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    Periode = table.Column<int>(nullable: false, defaultValue: 0),
                    AvancementEtatId = table.Column<int>(nullable: false),
                    QuantiteSousDetailAvancee = table.Column<decimal>(type: "decimal(20, 8)", nullable: true),
                    PourcentageSousDetailAvance = table.Column<decimal>(nullable: true),
                    DAD = table.Column<decimal>(type: "decimal(20, 8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AVANCEMENT", x => x.AvancementId);
                    table.ForeignKey(
                        name: "FK_FRED_AVANCEMENT_FRED_AVANCEMENT_ETAT_AvancementEtatId",
                        column: x => x.AvancementEtatId,
                        principalSchema: "dbo",
                        principalTable: "FRED_AVANCEMENT_ETAT",
                        principalColumn: "AvancementEtatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_AVANCEMENT_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_VALORISATION",
                schema: "dbo",
                columns: table => new
                {
                    ValorisationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CiId = table.Column<int>(nullable: false),
                    RapportId = table.Column<int>(nullable: false),
                    RapportLigneId = table.Column<int>(nullable: false, defaultValue: 0),
                    TacheId = table.Column<int>(nullable: false),
                    ChapitreId = table.Column<int>(nullable: false),
                    SousChapitreId = table.Column<int>(nullable: false),
                    ReferentielEtenduId = table.Column<int>(nullable: false),
                    BaremeId = table.Column<int>(nullable: true),
                    BaremeStormId = table.Column<int>(nullable: true),
                    UniteId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    PersonnelId = table.Column<int>(nullable: true),
                    MaterielId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    VerrouPeriode = table.Column<bool>(nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    Source = table.Column<string>(nullable: true),
                    PUHT = table.Column<decimal>(type: "decimal(18, 3)", nullable: false),
                    Quantite = table.Column<decimal>(type: "decimal(18, 3)", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(18, 3)", nullable: false),
                    TauxHoraireConverti = table.Column<decimal>(nullable: true),
                    GroupeRemplacementTacheId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_VALORISATION", x => x.ValorisationId);
                    table.ForeignKey(
                        name: "FK_FRED_VALORISATION_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_VALORISATION_FRED_GROUPE_REMPLACEMENT_TACHE_GroupeRemplacementTacheId",
                        column: x => x.GroupeRemplacementTacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE_REMPLACEMENT_TACHE",
                        principalColumn: "GroupeRemplacementTacheId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_VALORISATION_FRED_RAPPORT_LIGNE_RapportLigneId",
                        column: x => x.RapportLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_LIGNE",
                        principalColumn: "RapportLigneId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_VALORISATION_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AVANCEMENT_TACHE",
                schema: "dbo",
                columns: table => new
                {
                    AvancementTacheId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetId = table.Column<int>(nullable: false),
                    Periode = table.Column<int>(nullable: false),
                    TacheId = table.Column<int>(nullable: false),
                    Commentaire = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AVANCEMENT_TACHE", x => x.AvancementTacheId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_RECETTE",
                schema: "dbo",
                columns: table => new
                {
                    BudgetRecetteId = table.Column<int>(nullable: false, defaultValue: 0),
                    MontantMarche = table.Column<decimal>(nullable: true),
                    MontantAvenants = table.Column<decimal>(nullable: true),
                    SommeAValoir = table.Column<decimal>(nullable: true),
                    TravauxSupplementaires = table.Column<decimal>(nullable: true),
                    Revision = table.Column<decimal>(nullable: true),
                    AutresRecettes = table.Column<decimal>(nullable: true),
                    PenalitesEtRetenues = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_RECETTE", x => x.BudgetRecetteId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AVANCEMENT_RECETTE",
                schema: "dbo",
                columns: table => new
                {
                    AvancementRecetteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetRecetteId = table.Column<int>(nullable: false),
                    Periode = table.Column<int>(nullable: false),
                    MontantMarche = table.Column<decimal>(nullable: false),
                    MontantAvenants = table.Column<decimal>(nullable: false),
                    SommeAValoir = table.Column<decimal>(nullable: false),
                    TravauxSupplementaires = table.Column<decimal>(nullable: false),
                    Revision = table.Column<decimal>(nullable: false),
                    AutresRecettes = table.Column<decimal>(nullable: false),
                    PenalitesEtRetenues = table.Column<decimal>(nullable: false),
                    MontantMarchePFA = table.Column<decimal>(nullable: false),
                    MontantAvenantsPFA = table.Column<decimal>(nullable: false),
                    SommeAValoirPFA = table.Column<decimal>(nullable: false),
                    TravauxSupplementairesPFA = table.Column<decimal>(nullable: false),
                    RevisionPFA = table.Column<decimal>(nullable: false),
                    AutresRecettesPFA = table.Column<decimal>(nullable: false),
                    PenalitesEtRetenuesPFA = table.Column<decimal>(nullable: false),
                    Correctif = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    TauxFraisGeneraux = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    AjustementFraisGeneraux = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    TauxFraisGenerauxPFA = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    AjustementFraisGenerauxPFA = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    AvancementTauxFraisGeneraux = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    AvancementAjustementFraisGeneraux = table.Column<decimal>(nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AVANCEMENT_RECETTE", x => x.AvancementRecetteId);
                    table.ForeignKey(
                        name: "FK_FRED_AVANCEMENT_RECETTE_FRED_BUDGET_RECETTE_BudgetRecetteId",
                        column: x => x.BudgetRecetteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_BUDGET_RECETTE",
                        principalColumn: "BudgetRecetteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_REVISION",
                schema: "dbo",
                columns: table => new
                {
                    BudgetRevisionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Statut = table.Column<int>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    DateValidation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateaValider = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurValidationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_REVISION", x => x.BudgetRevisionId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_T4",
                schema: "dbo",
                columns: table => new
                {
                    BudgetT4Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetId = table.Column<int>(nullable: false),
                    T4Id = table.Column<int>(nullable: false),
                    UniteId = table.Column<int>(nullable: true),
                    QuantiteARealiser = table.Column<decimal>(type: "decimal(20, 3)", nullable: true),
                    MontantT4 = table.Column<decimal>(type: "decimal(20, 8)", nullable: true),
                    Commentaire = table.Column<string>(maxLength: 500, nullable: true),
                    TypeAvancement = table.Column<int>(nullable: true, defaultValue: 0),
                    QuantiteDeBase = table.Column<decimal>(type: "decimal(20, 3)", nullable: true, defaultValue: 0m),
                    PU = table.Column<decimal>(type: "decimal(20, 8)", nullable: true, defaultValue: 0m),
                    VueSD = table.Column<int>(nullable: false, defaultValue: 1),
                    IsReadOnly = table.Column<bool>(nullable: false, defaultValue: false),
                    T3Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_T4", x => x.BudgetT4Id);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_T4_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_TACHE",
                schema: "dbo",
                columns: table => new
                {
                    BudgetTacheId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetId = table.Column<int>(nullable: false),
                    TacheId = table.Column<int>(nullable: false),
                    Commentaire = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_TACHE", x => x.BudgetTacheId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_WORKFLOW",
                schema: "dbo",
                columns: table => new
                {
                    BudgetWorkflowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetId = table.Column<int>(nullable: false),
                    EtatInitialId = table.Column<int>(nullable: true),
                    EtatCibleId = table.Column<int>(nullable: false),
                    Commentaire = table.Column<string>(nullable: true),
                    AuteurId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_WORKFLOW", x => x.BudgetWorkflowId);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_WORKFLOW_FRED_BUDGET_ETAT_EtatCibleId",
                        column: x => x.EtatCibleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_BUDGET_ETAT",
                        principalColumn: "BudgetEtatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_WORKFLOW_FRED_BUDGET_ETAT_EtatInitialId",
                        column: x => x.EtatInitialId,
                        principalSchema: "dbo",
                        principalTable: "FRED_BUDGET_ETAT",
                        principalColumn: "BudgetEtatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CONTROLE_BUDGETAIRE",
                schema: "dbo",
                columns: table => new
                {
                    ControleBudgetaireId = table.Column<int>(nullable: false),
                    Periode = table.Column<int>(nullable: false, defaultValue: 0),
                    ControleBudgetaireEtatId = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CONTROLE_BUDGETAIRE", x => new { x.ControleBudgetaireId, x.Periode });
                    table.ForeignKey(
                        name: "FK_FRED_CONTROLE_BUDGETAIRE_FRED_BUDGET_ETAT_ControleBudgetaireEtatId",
                        column: x => x.ControleBudgetaireEtatId,
                        principalSchema: "dbo",
                        principalTable: "FRED_BUDGET_ETAT",
                        principalColumn: "BudgetEtatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TACHE",
                schema: "dbo",
                columns: table => new
                {
                    TacheId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 15, nullable: false),
                    Libelle = table.Column<string>(maxLength: 255, nullable: false),
                    TacheParDefaut = table.Column<bool>(nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Niveau = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    QuantiteBase = table.Column<double>(nullable: true),
                    PrixTotalQB = table.Column<double>(nullable: true),
                    PrixUnitaireQB = table.Column<double>(nullable: true),
                    TotalHeureMO = table.Column<double>(nullable: true),
                    HeureMOUnite = table.Column<double>(nullable: true),
                    QuantiteARealise = table.Column<double>(type: "float", nullable: true),
                    NbrRessourcesToParam = table.Column<int>(nullable: true),
                    TacheType = table.Column<int>(nullable: false, defaultValue: 999999),
                    BudgetId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    BudgetRevisionEntBudgetRevisionId = table.Column<int>(nullable: true),
                    UniteEntUniteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TACHE", x => x.TacheId);
                    table.ForeignKey(
                        name: "FK_FRED_TACHE_FRED_BUDGET_REVISION_BudgetRevisionEntBudgetRevisionId",
                        column: x => x.BudgetRevisionEntBudgetRevisionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_BUDGET_REVISION",
                        principalColumn: "BudgetRevisionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_TACHE_FRED_TACHE_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_TACHE_FRED_UNITE_UniteEntUniteId",
                        column: x => x.UniteEntUniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORT_LIGNE_TACHE",
                schema: "dbo",
                columns: table => new
                {
                    RapportLigneTacheId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RapportLigneId = table.Column<int>(nullable: false),
                    TacheId = table.Column<int>(nullable: false),
                    HeureTache = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORT_LIGNE_TACHE", x => x.RapportLigneTacheId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_TACHE_FRED_RAPPORT_LIGNE_RapportLigneId",
                        column: x => x.RapportLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_LIGNE",
                        principalColumn: "RapportLigneId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_TACHE_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_TACHE_RECETTE",
                schema: "dbo",
                columns: table => new
                {
                    TacheRecetteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TacheId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    Recette = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_TACHE_RECETTE", x => x.TacheRecetteId);
                    table.ForeignKey(
                        name: "FK_FRED_TACHE_RECETTE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_TACHE_RECETTE_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                schema: "dbo",
                columns: table => new
                {
                    BudgetBibliothequePrixItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetBibliothequePrixId = table.Column<int>(nullable: false),
                    Prix = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    UniteId = table.Column<int>(nullable: true),
                    RessourceId = table.Column<int>(nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM", x => x.BudgetBibliothequePrixItemId);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO",
                schema: "dbo",
                columns: table => new
                {
                    BudgetBibliothequePrixItemValuesHistoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateInsertion = table.Column<DateTime>(type: "datetime", nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    Prix = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    UniteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO", x => x.BudgetBibliothequePrixItemValuesHistoId);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "dbo",
                        principalTable: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                        principalColumn: "BudgetBibliothequePrixItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET",
                schema: "dbo",
                columns: table => new
                {
                    BudgetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CiId = table.Column<int>(nullable: false),
                    PeriodeDebut = table.Column<int>(nullable: true),
                    PeriodeFin = table.Column<int>(nullable: true),
                    DeviseId = table.Column<int>(nullable: false, defaultValue: 0),
                    Version = table.Column<string>(nullable: false, defaultValue: ""),
                    BudgetEtatId = table.Column<int>(nullable: false, defaultValue: 0),
                    Partage = table.Column<bool>(nullable: false, defaultValue: false),
                    DateSuppressionBudget = table.Column<DateTime>(type: "datetime", nullable: true),
                    Libelle = table.Column<string>(maxLength: 50, nullable: true),
                    DateDeleteNotificationNewTask = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET", x => x.BudgetId);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_FRED_BUDGET_ETAT_BudgetEtatId",
                        column: x => x.BudgetEtatId,
                        principalSchema: "dbo",
                        principalTable: "FRED_BUDGET_ETAT",
                        principalColumn: "BudgetEtatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_T4_RESSOURCE",
                schema: "dbo",
                columns: table => new
                {
                    BudgetT4SousDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BudgetT4Id = table.Column<int>(nullable: false),
                    RessourceId = table.Column<int>(nullable: false, defaultValue: 0),
                    Quantite = table.Column<decimal>(type: "decimal(20, 3)", nullable: true, defaultValue: 0m),
                    QuantiteFormule = table.Column<string>(maxLength: 200, nullable: true),
                    PU = table.Column<decimal>(type: "decimal(20, 8)", nullable: true),
                    Montant = table.Column<decimal>(type: "decimal(20, 8)", nullable: true),
                    QuantiteSD = table.Column<decimal>(type: "decimal(20, 3)", nullable: true),
                    QuantiteSDFormule = table.Column<string>(maxLength: 200, nullable: true),
                    Commentaire = table.Column<string>(maxLength: 200, nullable: true),
                    UniteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_T4_RESSOURCE", x => x.BudgetT4SousDetailId);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_T4_RESSOURCE_FRED_BUDGET_T4_BudgetT4Id",
                        column: x => x.BudgetT4Id,
                        principalSchema: "dbo",
                        principalTable: "FRED_BUDGET_T4",
                        principalColumn: "BudgetT4Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_T4_RESSOURCE_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CARBURANT_ORGANISATION_DEVISE",
                schema: "dbo",
                columns: table => new
                {
                    CarburantOrganisationDeviseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CarburantId = table.Column<int>(nullable: false),
                    OrganisationId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    Prix = table.Column<decimal>(type: "decimal(10, 5)", nullable: false),
                    Periode = table.Column<DateTime>(type: "date", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CARBURANT_ORGANISATION_DEVISE", x => x.CarburantOrganisationDeviseId);
                    table.ForeignKey(
                        name: "FK_FRED_CARBURANT_ORGANISATION_DEVISE_FRED_CARBURANT_CarburantId",
                        column: x => x.CarburantId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CARBURANT",
                        principalColumn: "CarburantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CARBURANT_ORGANISATION_DEVISE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CARBURANT_ORGANISATION_DEVISE_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RESSOURCE",
                schema: "dbo",
                columns: table => new
                {
                    RessourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SousChapitreId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    TypeRessourceId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    CarburantId = table.Column<int>(nullable: true),
                    Consommation = table.Column<decimal>(type: "decimal(7, 2)", nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    IsRessourceSpecifiqueCi = table.Column<bool>(nullable: false, defaultValue: false),
                    RessourceRattachementId = table.Column<int>(nullable: true),
                    SpecifiqueCiId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RESSOURCE", x => x.RessourceId);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_FRED_CARBURANT_CarburantId",
                        column: x => x.CarburantId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CARBURANT",
                        principalColumn: "CarburantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_FRED_RESSOURCE_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_FRED_RESSOURCE_RessourceRattachementId",
                        column: x => x.RessourceRattachementId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_FRED_TYPE_RESSOURCE_TypeRessourceId",
                        column: x => x.TypeRessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TYPE_RESSOURCE",
                        principalColumn: "TypeRessourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CONTROLE_BUDGETAIRE_VALEURS",
                schema: "dbo",
                columns: table => new
                {
                    ControleBudgetaireId = table.Column<int>(nullable: false),
                    Periode = table.Column<int>(nullable: false, defaultValue: 0),
                    TacheId = table.Column<int>(nullable: false),
                    RessourceId = table.Column<int>(nullable: false, defaultValue: 0),
                    Ajustement = table.Column<decimal>(type: "decimal(20, 3)", nullable: false),
                    CommentaireAjustement = table.Column<string>(nullable: true),
                    Pfa = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CONTROLE_BUDGETAIRE_VALEURS", x => new { x.ControleBudgetaireId, x.Periode, x.TacheId, x.RessourceId });
                    table.ForeignKey(
                        name: "FK_FRED_CONTROLE_BUDGETAIRE_VALEURS_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CONTROLE_BUDGETAIRE_VALEURS_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_CONTROLE_BUDGETAIRE_VALEURS_FRED_CONTROLE_BUDGETAIRE_ControleBudgetaireId_Periode",
                        columns: x => new { x.ControleBudgetaireId, x.Periode },
                        principalSchema: "dbo",
                        principalTable: "FRED_CONTROLE_BUDGETAIRE",
                        principalColumns: new[] { "ControleBudgetaireId", "Periode" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RESSOURCE_TACHE",
                schema: "dbo",
                columns: table => new
                {
                    RessourceTacheId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TacheId = table.Column<int>(nullable: false),
                    RessourceId = table.Column<int>(nullable: false),
                    QuantiteBase = table.Column<double>(type: "float", nullable: true),
                    Quantite = table.Column<double>(type: "float", nullable: true),
                    PrixUnitaire = table.Column<double>(type: "float", nullable: true),
                    Formule = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RESSOURCE_TACHE", x => x.RessourceTacheId);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_TACHE_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_TACHE_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RESSOURCE_TACHE_DEVISE",
                schema: "dbo",
                columns: table => new
                {
                    RessourceTacheDeviseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RessourceTacheId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    PrixUnitaire = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RESSOURCE_TACHE_DEVISE", x => x.RessourceTacheDeviseId);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_TACHE_DEVISE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_TACHE_DEVISE_FRED_RESSOURCE_TACHE_RessourceTacheId",
                        column: x => x.RessourceTacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE_TACHE",
                        principalColumn: "RessourceTacheId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_SOUS_CHAPITRE",
                schema: "dbo",
                columns: table => new
                {
                    SousChapitreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChapitreId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 200, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_SOUS_CHAPITRE", x => x.SousChapitreId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_AFFECTATION",
                schema: "dbo",
                columns: table => new
                {
                    AffectationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDelegue = table.Column<bool>(nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    PersonnelId = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false, defaultValue: false),
                    IsDelete = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_AFFECTATION", x => x.AffectationId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BAREME_EXPLOITATION_CI",
                schema: "dbo",
                columns: table => new
                {
                    BaremeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CIId = table.Column<int>(nullable: false),
                    ReferentielEtenduId = table.Column<int>(nullable: false),
                    UniteId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    Statut = table.Column<int>(nullable: false),
                    PeriodeDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    PeriodeFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    Prix = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    PrixChauffeur = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    PrixConduite = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BAREME_EXPLOITATION_CI", x => x.BaremeId);
                    table.ForeignKey(
                        name: "FK_FRED_BAREME_EXPLOITATION_CI_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_BAREME_EXPLOITATION_CI_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                schema: "dbo",
                columns: table => new
                {
                    SurchargeBaremeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CIId = table.Column<int>(nullable: false),
                    ReferentielEtenduId = table.Column<int>(nullable: false),
                    PersonnelId = table.Column<int>(nullable: true),
                    MaterielId = table.Column<int>(nullable: true),
                    UniteId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    PeriodeDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    PeriodeFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    Prix = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    PrixChauffeur = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    PrixConduite = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    Type = table.Column<int>(nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BAREME_EXPLOITATION_CI_SURCHARGE", x => x.SurchargeBaremeId);
                    table.ForeignKey(
                        name: "FK_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CI_CODE_MAJORATION",
                schema: "dbo",
                columns: table => new
                {
                    CiCodeMajorationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CiId = table.Column<int>(nullable: false),
                    CodeMajorationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CI_CODE_MAJORATION", x => x.CiCodeMajorationId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CI_DEVISE",
                schema: "dbo",
                columns: table => new
                {
                    CiDeviseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CiId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    Reference = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CI_DEVISE", x => x.CiDeviseId);
                    table.ForeignKey(
                        name: "FK_FRED_CI_DEVISE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CI_PRIME",
                schema: "dbo",
                columns: table => new
                {
                    CiPrimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CiId = table.Column<int>(nullable: false),
                    PrimeId = table.Column<int>(nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CI_PRIME", x => x.CiPrimeId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CI_RESSOURCE",
                schema: "dbo",
                columns: table => new
                {
                    CiRessourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CiId = table.Column<int>(nullable: false),
                    RessourceId = table.Column<int>(nullable: false),
                    Consommation = table.Column<decimal>(type: "decimal(7, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CI_RESSOURCE", x => x.CiRessourceId);
                    table.ForeignKey(
                        name: "FK_FRED_CI_RESSOURCE_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_COMMANDE",
                schema: "dbo",
                columns: table => new
                {
                    CommandeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Numero = table.Column<string>(maxLength: 10, nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    Libelle = table.Column<string>(maxLength: 250, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    FournisseurId = table.Column<int>(nullable: true),
                    DelaiLivraison = table.Column<string>(maxLength: 100, nullable: true),
                    DateMiseADispo = table.Column<DateTime>(type: "datetime", nullable: true),
                    StatutCommandeId = table.Column<int>(nullable: false),
                    MOConduite = table.Column<bool>(nullable: false),
                    EntretienMecanique = table.Column<bool>(nullable: false),
                    EntretienJournalier = table.Column<bool>(nullable: false),
                    Carburant = table.Column<bool>(nullable: false),
                    Lubrifiant = table.Column<bool>(nullable: false),
                    FraisAmortissement = table.Column<bool>(nullable: false),
                    FraisAssurance = table.Column<bool>(nullable: false),
                    ConditionSociete = table.Column<string>(nullable: true),
                    ConditionPrestation = table.Column<string>(nullable: true),
                    ContactId = table.Column<int>(nullable: true),
                    ContactTel = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    SuiviId = table.Column<int>(nullable: true),
                    ValideurId = table.Column<int>(nullable: true),
                    DateValidation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    LivraisonEntete = table.Column<string>(maxLength: 250, nullable: true),
                    LivraisonAdresse = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    LivraisonVille = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    LivraisonCPostale = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    LivraisonPaysId = table.Column<int>(nullable: true),
                    FacturationAdresse = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    FacturationVille = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    FacturationCPostale = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    FacturationPaysId = table.Column<int>(nullable: true),
                    FournisseurAdresse = table.Column<string>(nullable: true),
                    FournisseurVille = table.Column<string>(nullable: true),
                    FournisseurCPostal = table.Column<string>(nullable: true),
                    FournisseurPaysId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(nullable: true),
                    DateCloture = table.Column<DateTime>(nullable: true),
                    Justificatif = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    CommentaireFournisseur = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    CommentaireInterne = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    DeviseId = table.Column<int>(nullable: true),
                    AccordCadre = table.Column<bool>(nullable: false),
                    CommandeManuelle = table.Column<bool>(nullable: false),
                    NumeroCommandeExterne = table.Column<string>(nullable: true),
                    NumeroContratExterne = table.Column<string>(nullable: true),
                    HangfireJobId = table.Column<string>(nullable: true),
                    IsAbonnement = table.Column<bool>(nullable: false, defaultValue: false),
                    DureeAbonnement = table.Column<int>(nullable: true),
                    FrequenceAbonnement = table.Column<int>(nullable: true),
                    DatePremiereReception = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateProchaineReception = table.Column<DateTime>(type: "datetime", nullable: true),
                    SystemeExterneId = table.Column<int>(nullable: true),
                    OldFournisseurId = table.Column<int>(nullable: true),
                    IsMaterielAPointer = table.Column<bool>(nullable: false, defaultValue: false),
                    IsEnergie = table.Column<bool>(nullable: false, defaultValue: false),
                    TypeEnergieId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_COMMANDE", x => x.CommandeId);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_PAYS_FacturationPaysId",
                        column: x => x.FacturationPaysId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_FOURNISSEUR_FournisseurId",
                        column: x => x.FournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_PAYS_FournisseurPaysId",
                        column: x => x.FournisseurPaysId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_PAYS_LivraisonPaysId",
                        column: x => x.LivraisonPaysId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_FOURNISSEUR_OldFournisseurId",
                        column: x => x.OldFournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_STATUT_COMMANDE_StatutCommandeId",
                        column: x => x.StatutCommandeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_STATUT_COMMANDE",
                        principalColumn: "StatutCommandeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_SYSTEME_EXTERNE_SystemeExterneId",
                        column: x => x.SystemeExterneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SYSTEME_EXTERNE",
                        principalColumn: "SystemeExterneId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_TYPE_ENERGIE_TypeEnergieId",
                        column: x => x.TypeEnergieId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TYPE_ENERGIE",
                        principalColumn: "TypeEnergieId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_FRED_COMMANDE_TYPE_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE_TYPE",
                        principalColumn: "CommandeTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                schema: "dbo",
                columns: table => new
                {
                    CommandeId = table.Column<int>(nullable: false),
                    ContratId = table.Column<int>(nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    InterimaireId = table.Column<int>(nullable: false),
                    RapportLigneId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_COMMANDE_CONTRAT_INTERIMAIRE", x => new { x.CommandeId, x.ContratId, x.CiId });
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_CONTRAT_INTERIMAIRE_FRED_COMMANDE_CommandeId",
                        column: x => x.CommandeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE",
                        principalColumn: "CommandeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_CONTRAT_INTERIMAIRE_FRED_RAPPORT_LIGNE_RapportLigneId",
                        column: x => x.RapportLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_LIGNE",
                        principalColumn: "RapportLigneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CONTRAT_INTERIMAIRE",
                schema: "dbo",
                columns: table => new
                {
                    PersonnelFournisseurSocieteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InterimaireId = table.Column<int>(nullable: false),
                    FournisseurId = table.Column<int>(nullable: false),
                    SocieteId = table.Column<int>(nullable: false),
                    NumContrat = table.Column<string>(maxLength: 150, nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime", nullable: false),
                    Energie = table.Column<bool>(nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    RessourceId = table.Column<int>(nullable: false),
                    Source = table.Column<string>(maxLength: 50, nullable: false),
                    Qualification = table.Column<string>(maxLength: 150, nullable: true),
                    Statut = table.Column<int>(nullable: false),
                    UniteId = table.Column<int>(nullable: false, defaultValue: 0),
                    TarifUnitaire = table.Column<decimal>(nullable: false),
                    Valorisation = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    Souplesse = table.Column<int>(nullable: false),
                    MotifRemplacementId = table.Column<int>(nullable: false),
                    PersonnelRemplaceId = table.Column<int>(nullable: true),
                    Commentaire = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CONTRAT_INTERIMAIRE", x => x.PersonnelFournisseurSocieteId);
                    table.ForeignKey(
                        name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_FOURNISSEUR_FournisseurId",
                        column: x => x.FournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_MOTIF_REMPLACEMENT_MotifRemplacementId",
                        column: x => x.MotifRemplacementId,
                        principalSchema: "dbo",
                        principalTable: "FRED_MOTIF_REMPLACEMENT",
                        principalColumn: "MotifRemplacementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_DATES_CLOTURE_COMPTABLE",
                schema: "dbo",
                columns: table => new
                {
                    DatesClotureComptableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    CiId = table.Column<int>(nullable: false),
                    Annee = table.Column<int>(nullable: false, defaultValue: 0),
                    Mois = table.Column<int>(nullable: false, defaultValue: 0),
                    DateArretSaisie = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateTransfertFAR = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCloture = table.Column<DateTime>(type: "datetime", nullable: true),
                    Historique = table.Column<bool>(nullable: false, defaultValue: false),
                    AuteurSap = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_DATES_CLOTURE_COMPTABLE", x => x.DatesClotureComptableId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_DEPENSE_ACHAT",
                schema: "dbo",
                columns: table => new
                {
                    DepenseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommandeLigneId = table.Column<int>(nullable: true),
                    CiId = table.Column<int>(nullable: true),
                    FournisseurId = table.Column<int>(nullable: true),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    TacheId = table.Column<int>(nullable: true),
                    RessourceId = table.Column<int>(nullable: true),
                    Commentaire = table.Column<string>(maxLength: 500, nullable: true),
                    Quantite = table.Column<decimal>(type: "numeric(12, 3)", nullable: false),
                    PUHT = table.Column<decimal>(type: "numeric(18, 8)", nullable: false),
                    UniteId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    DeviseId = table.Column<int>(nullable: true),
                    NumeroBL = table.Column<string>(maxLength: 50, nullable: true),
                    DepenseParentId = table.Column<int>(nullable: true),
                    FarAnnulee = table.Column<bool>(nullable: false, defaultValue: false),
                    DepenseTypeId = table.Column<int>(nullable: true),
                    DateComptable = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateVisaReception = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateFacturation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurVisaReceptionId = table.Column<int>(nullable: true),
                    QuantiteDepense = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    HangfireJobId = table.Column<string>(nullable: true),
                    AfficherPuHt = table.Column<bool>(nullable: false, defaultValue: false),
                    AfficherQuantite = table.Column<bool>(nullable: false, defaultValue: false),
                    CompteComptable = table.Column<string>(nullable: true),
                    ErreurControleFar = table.Column<bool>(nullable: true),
                    DateControleFar = table.Column<DateTime>(type: "datetime", nullable: true),
                    StatutVisaId = table.Column<int>(nullable: true),
                    DateOperation = table.Column<DateTime>(type: "datetime", nullable: true),
                    MontantHtInitial = table.Column<decimal>(nullable: true),
                    IsReceptionInterimaire = table.Column<bool>(nullable: false, defaultValue: false),
                    IsReceptionMaterielExterne = table.Column<bool>(nullable: false, defaultValue: false),
                    GroupeRemplacementTacheId = table.Column<int>(nullable: true),
                    FactureLigneEntLigneFactureId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_DEPENSE_ACHAT", x => x.DepenseId);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_ACHAT_FRED_DEPENSE_ACHAT_DepenseParentId",
                        column: x => x.DepenseParentId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_ACHAT",
                        principalColumn: "DepenseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_ACHAT_FRED_DEPENSE_TYPE_DepenseTypeId",
                        column: x => x.DepenseTypeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_TYPE",
                        principalColumn: "DepenseTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_ACHAT_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_ACHAT_FRED_FOURNISSEUR_FournisseurId",
                        column: x => x.FournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_ACHAT_FRED_GROUPE_REMPLACEMENT_TACHE_GroupeRemplacementTacheId",
                        column: x => x.GroupeRemplacementTacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE_REMPLACEMENT_TACHE",
                        principalColumn: "GroupeRemplacementTacheId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_ACHAT_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_ACHAT_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_ACHAT_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_DEPENSE_TEMPORAIRE",
                schema: "dbo",
                columns: table => new
                {
                    DepenseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommandeLigneId = table.Column<int>(nullable: true),
                    CiId = table.Column<int>(nullable: true),
                    FournisseurId = table.Column<int>(nullable: true),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    TacheId = table.Column<int>(nullable: true),
                    RessourceId = table.Column<int>(nullable: true),
                    Commentaire = table.Column<string>(maxLength: 500, nullable: true),
                    Quantite = table.Column<decimal>(type: "numeric(11, 2)", nullable: false),
                    PUHT = table.Column<decimal>(type: "numeric(11, 2)", nullable: false),
                    UniteId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DeviseId = table.Column<int>(nullable: true),
                    NumeroBL = table.Column<string>(maxLength: 50, nullable: true),
                    DepenseOrigineId = table.Column<int>(nullable: true),
                    DepenseParentId = table.Column<int>(nullable: true),
                    TypeDepense = table.Column<string>(maxLength: 20, nullable: true),
                    FactureLigneId = table.Column<int>(nullable: true),
                    FactureId = table.Column<int>(nullable: false),
                    DateReception = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_DEPENSE_TEMPORAIRE", x => x.DepenseId);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_DEPENSE_ACHAT_DepenseOrigineId",
                        column: x => x.DepenseOrigineId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_ACHAT",
                        principalColumn: "DepenseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_DEPENSE_ACHAT_DepenseParentId",
                        column: x => x.DepenseParentId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_ACHAT",
                        principalColumn: "DepenseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_FOURNISSEUR_FournisseurId",
                        column: x => x.FournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ECRITURE_COMPTABLE",
                schema: "dbo",
                columns: table => new
                {
                    EcritureComptableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateComptable = table.Column<DateTime>(type: "datetime", nullable: true),
                    Libelle = table.Column<string>(nullable: true),
                    NumeroPiece = table.Column<string>(nullable: true),
                    JournalId = table.Column<int>(nullable: true, defaultValue: 0),
                    CiId = table.Column<int>(nullable: false),
                    Montant = table.Column<decimal>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    CommandeId = table.Column<int>(nullable: true, defaultValue: 0),
                    FamilleOperationDiverseId = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ECRITURE_COMPTABLE", x => x.EcritureComptableId);
                    table.ForeignKey(
                        name: "FK_FRED_ECRITURE_COMPTABLE_FRED_COMMANDE_CommandeId",
                        column: x => x.CommandeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE",
                        principalColumn: "CommandeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ECRITURE_COMPTABLE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ECRITURE_COMPTABLE_CUMUL",
                schema: "dbo",
                columns: table => new
                {
                    EcritureComptableCumulId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateComptable = table.Column<DateTime>(type: "datetime", nullable: true),
                    NumeroPiece = table.Column<string>(nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    Montant = table.Column<decimal>(nullable: false),
                    EcritureComptableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ECRITURE_COMPTABLE_CUMUL", x => x.EcritureComptableCumulId);
                    table.ForeignKey(
                        name: "FK_FRED_ECRITURE_COMPTABLE_CUMUL_FRED_ECRITURE_COMPTABLE_EcritureComptableId",
                        column: x => x.EcritureComptableId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ECRITURE_COMPTABLE",
                        principalColumn: "EcritureComptableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FACTURATION",
                schema: "dbo",
                columns: table => new
                {
                    FacturationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FacturationTypeId = table.Column<int>(nullable: false),
                    CommandeId = table.Column<int>(nullable: true),
                    DepenseAchatReceptionId = table.Column<int>(nullable: true),
                    DepenseAchatFactureEcartId = table.Column<int>(nullable: true),
                    DepenseAchatFactureId = table.Column<int>(nullable: true),
                    DepenseAchatFarId = table.Column<int>(nullable: true),
                    DeviseId = table.Column<int>(nullable: true),
                    MontantHT = table.Column<decimal>(nullable: false),
                    Quantite = table.Column<decimal>(type: "numeric(18, 3)", nullable: false),
                    EcartPu = table.Column<decimal>(type: "numeric(18, 3)", nullable: true),
                    EcartQuantite = table.Column<decimal>(type: "numeric(18, 3)", nullable: true),
                    IsFacturationFinale = table.Column<bool>(nullable: false),
                    QuantiteReconduite = table.Column<decimal>(type: "numeric(18, 3)", nullable: true),
                    NumeroFactureFMFI = table.Column<string>(nullable: true),
                    NumeroFactureSAP = table.Column<string>(nullable: true),
                    NumeroFactureFournisseur = table.Column<string>(nullable: true),
                    DatePieceSap = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateSaisie = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateComptable = table.Column<DateTime>(type: "datetime", nullable: false),
                    MontantTotalHT = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    Commentaire = table.Column<string>(nullable: true),
                    NatureCode = table.Column<string>(nullable: true),
                    FournisseurCode = table.Column<string>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurCreation = table.Column<string>(nullable: true),
                    TotalFarHt = table.Column<decimal>(type: "numeric(18, 3)", nullable: true),
                    MouvementFarHt = table.Column<decimal>(type: "numeric(18, 3)", nullable: true),
                    QuantiteFar = table.Column<decimal>(type: "numeric(18, 3)", nullable: true),
                    LitigeCode = table.Column<string>(nullable: true),
                    DebitCredit = table.Column<string>(nullable: true),
                    CompteComptable = table.Column<string>(nullable: true),
                    CiId = table.Column<int>(nullable: true),
                    DepenseAchatAjustementId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FACTURATION", x => x.FacturationId);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURATION_FRED_COMMANDE_CommandeId",
                        column: x => x.CommandeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE",
                        principalColumn: "CommandeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURATION_FRED_DEPENSE_ACHAT_DepenseAchatAjustementId",
                        column: x => x.DepenseAchatAjustementId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_ACHAT",
                        principalColumn: "DepenseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURATION_FRED_DEPENSE_ACHAT_DepenseAchatFactureEcartId",
                        column: x => x.DepenseAchatFactureEcartId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_ACHAT",
                        principalColumn: "DepenseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURATION_FRED_DEPENSE_ACHAT_DepenseAchatFactureId",
                        column: x => x.DepenseAchatFactureId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_ACHAT",
                        principalColumn: "DepenseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURATION_FRED_DEPENSE_ACHAT_DepenseAchatFarId",
                        column: x => x.DepenseAchatFarId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_ACHAT",
                        principalColumn: "DepenseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURATION_FRED_DEPENSE_ACHAT_DepenseAchatReceptionId",
                        column: x => x.DepenseAchatReceptionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_ACHAT",
                        principalColumn: "DepenseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURATION_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURATION_FRED_FACTURATION_TYPE_FacturationTypeId",
                        column: x => x.FacturationTypeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FACTURATION_TYPE",
                        principalColumn: "FacturationTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FACTURE_LIGNE",
                schema: "dbo",
                columns: table => new
                {
                    LigneFactureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NatureId = table.Column<int>(nullable: true),
                    AffaireId = table.Column<int>(nullable: true),
                    Quantite = table.Column<decimal>(type: "numeric(15, 3)", nullable: true),
                    PrixUnitaire = table.Column<decimal>(type: "numeric(15, 3)", nullable: true),
                    MontantHT = table.Column<decimal>(type: "numeric(15, 3)", nullable: true),
                    NoBonLivraison = table.Column<string>(maxLength: 100, nullable: true),
                    FactureId = table.Column<int>(nullable: true),
                    UtilisateurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    UtilisateurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    UtilisateurSuppressionId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FACTURE_LIGNE", x => x.LigneFactureId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_INDEMNITE_DEPLACEMENT",
                schema: "dbo",
                columns: table => new
                {
                    IndemniteDeplacementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PersonnelId = table.Column<int>(nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    NombreKilometres = table.Column<double>(nullable: false),
                    NombreKilometreVODomicileRattachement = table.Column<double>(nullable: true),
                    NombreKilometreVODomicileChantier = table.Column<double>(nullable: true),
                    NombreKilometreVOChantierRattachement = table.Column<double>(nullable: true),
                    DateDernierCalcul = table.Column<DateTime>(type: "datetime", nullable: true),
                    IVD = table.Column<bool>(nullable: false),
                    CodeDeplacementId = table.Column<int>(nullable: true),
                    CodeZoneDeplacementId = table.Column<int>(nullable: true),
                    SaisieManuelle = table.Column<bool>(nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreation = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModification = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppression = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_INDEMNITE_DEPLACEMENT", x => x.IndemniteDeplacementId);
                    table.ForeignKey(
                        name: "FK_FRED_INDEMNITE_DEPLACEMENT_FRED_CODE_DEPLACEMENT_CodeDeplacementId",
                        column: x => x.CodeDeplacementId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CODE_DEPLACEMENT",
                        principalColumn: "CodeDeplacementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_OBJECTIF_FLASH",
                schema: "dbo",
                columns: table => new
                {
                    ObjectifFlashId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Libelle = table.Column<string>(nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime", nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    IsActif = table.Column<bool>(nullable: false, defaultValue: false),
                    DateCloture = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalMontantObjectif = table.Column<decimal>(type: "decimal(18, 3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_OBJECTIF_FLASH", x => x.ObjectifFlashId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_OBJECTIF_FLASH_TACHE",
                schema: "dbo",
                columns: table => new
                {
                    ObjectifFlashTacheId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ObjectifFlashId = table.Column<int>(nullable: false),
                    TacheId = table.Column<int>(nullable: false),
                    QuantiteObjectif = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    UniteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_OBJECTIF_FLASH_TACHE", x => x.ObjectifFlashTacheId);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_FRED_OBJECTIF_FLASH_ObjectifFlashId",
                        column: x => x.ObjectifFlashId,
                        principalSchema: "dbo",
                        principalTable: "FRED_OBJECTIF_FLASH",
                        principalColumn: "ObjectifFlashId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_OBJECTIF_FLASH_TACHE_JOURNALISATION",
                schema: "dbo",
                columns: table => new
                {
                    ObjectifFlashTacheJournalisationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ObjectifFlashTacheId = table.Column<int>(nullable: false),
                    DateJournalisation = table.Column<DateTime>(type: "datetime", nullable: false),
                    QuantiteObjectif = table.Column<decimal>(type: "decimal(18, 3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_OBJECTIF_FLASH_TACHE_JOURNALISATION", x => x.ObjectifFlashTacheJournalisationId);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_JOURNALISATION_FRED_OBJECTIF_FLASH_TACHE_ObjectifFlashTacheId",
                        column: x => x.ObjectifFlashTacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_OBJECTIF_FLASH_TACHE",
                        principalColumn: "ObjectifFlashTacheId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_OBJECTIF_FLASH_TACHE_RESSOURCE",
                schema: "dbo",
                columns: table => new
                {
                    ObjectifFlashTacheRessourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ObjectifFlashTacheId = table.Column<int>(nullable: false),
                    RessourceId = table.Column<int>(nullable: false),
                    QuantiteObjectif = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    PuHT = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    UniteId = table.Column<int>(nullable: true),
                    IsRepartitionKey = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE", x => x.ObjectifFlashTacheRessourceId);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_FRED_OBJECTIF_FLASH_TACHE_ObjectifFlashTacheId",
                        column: x => x.ObjectifFlashTacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_OBJECTIF_FLASH_TACHE",
                        principalColumn: "ObjectifFlashTacheId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_JOURNALISATION",
                schema: "dbo",
                columns: table => new
                {
                    ObjectifFlashTacheRessourceJournalisationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ObjectifFlashTacheRessourceId = table.Column<int>(nullable: false),
                    DateJournalisation = table.Column<DateTime>(type: "datetime", nullable: false),
                    QuantiteObjectif = table.Column<decimal>(type: "decimal(18, 3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_JOURNALISATION", x => x.ObjectifFlashTacheRessourceJournalisationId);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_JOURNALISATION_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_ObjectifFlashTacheRessourceId",
                        column: x => x.ObjectifFlashTacheRessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_OBJECTIF_FLASH_TACHE_RESSOURCE",
                        principalColumn: "ObjectifFlashTacheRessourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_OPERATION_DIVERSE",
                schema: "dbo",
                columns: table => new
                {
                    OperationDiverseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    Libelle = table.Column<string>(maxLength: 250, nullable: true),
                    Commentaire = table.Column<string>(nullable: true),
                    CiId = table.Column<int>(nullable: false),
                    TacheId = table.Column<int>(nullable: false),
                    PUHT = table.Column<decimal>(nullable: false),
                    Quantite = table.Column<decimal>(nullable: false),
                    Montant = table.Column<decimal>(nullable: false),
                    UniteId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    Cloturee = table.Column<bool>(nullable: false),
                    OdEcart = table.Column<bool>(nullable: false, defaultValue: false),
                    DateCloture = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateComptable = table.Column<DateTime>(type: "datetime", nullable: true),
                    FamilleOperationDiverseId = table.Column<int>(nullable: false, defaultValue: 0),
                    RessourceId = table.Column<int>(nullable: false, defaultValue: 0),
                    GroupeRemplacementTacheId = table.Column<int>(nullable: true),
                    EcritureComptableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_OPERATION_DIVERSE", x => x.OperationDiverseId);
                    table.ForeignKey(
                        name: "FK_FRED_OPERATION_DIVERSE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_OPERATION_DIVERSE_FRED_ECRITURE_COMPTABLE_EcritureComptableId",
                        column: x => x.EcritureComptableId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ECRITURE_COMPTABLE",
                        principalColumn: "EcritureComptableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_OPERATION_DIVERSE_FRED_GROUPE_REMPLACEMENT_TACHE_GroupeRemplacementTacheId",
                        column: x => x.GroupeRemplacementTacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE_REMPLACEMENT_TACHE",
                        principalColumn: "GroupeRemplacementTacheId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_OPERATION_DIVERSE_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_OPERATION_DIVERSE_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_OPERATION_DIVERSE_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_POINTAGE_ANTICIPE",
                schema: "dbo",
                columns: table => new
                {
                    PointageAnticipeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CiId = table.Column<int>(nullable: false),
                    PrenomNomTemporaire = table.Column<string>(maxLength: 100, nullable: true),
                    PersonnelId = table.Column<int>(nullable: true),
                    HeureNormale = table.Column<double>(type: "float", nullable: false),
                    CodeMajorationId = table.Column<int>(nullable: true),
                    HeureMajoration = table.Column<double>(type: "float", nullable: false),
                    CodeAbsenceId = table.Column<int>(nullable: true),
                    HeureAbsence = table.Column<double>(type: "float", nullable: false),
                    NumSemaineIntemperieAbsence = table.Column<int>(nullable: true),
                    CodeDeplacementId = table.Column<int>(nullable: true),
                    CodeZoneDeplacementId = table.Column<int>(nullable: true),
                    DeplacementIV = table.Column<bool>(nullable: false),
                    DatePointage = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsGenerated = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_POINTAGE_ANTICIPE", x => x.PointageAnticipeId);
                    table.ForeignKey(
                        name: "FK_FRED_POINTAGE_ANTICIPE_FRED_CODE_ABSENCE_CodeAbsenceId",
                        column: x => x.CodeAbsenceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CODE_ABSENCE",
                        principalColumn: "CodeAbsenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_POINTAGE_ANTICIPE_FRED_CODE_DEPLACEMENT_CodeDeplacementId",
                        column: x => x.CodeDeplacementId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CODE_DEPLACEMENT",
                        principalColumn: "CodeDeplacementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORT",
                schema: "dbo",
                columns: table => new
                {
                    RapportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RapportStatutId = table.Column<int>(nullable: false),
                    DateChantier = table.Column<DateTime>(type: "datetime", nullable: false),
                    HoraireDebutM = table.Column<DateTime>(type: "datetime", nullable: true),
                    HoraireFinM = table.Column<DateTime>(type: "datetime", nullable: true),
                    HoraireDebutS = table.Column<DateTime>(type: "datetime", nullable: true),
                    HoraireFinS = table.Column<DateTime>(type: "datetime", nullable: true),
                    Meteo = table.Column<string>(maxLength: 255, nullable: true),
                    Evenements = table.Column<string>(maxLength: 255, nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    AuteurVerrouId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateVerrou = table.Column<DateTime>(type: "datetime", nullable: true),
                    CiId = table.Column<int>(nullable: false),
                    ValideurCDCId = table.Column<int>(nullable: true),
                    ValideurCDTId = table.Column<int>(nullable: true),
                    ValideurDRCId = table.Column<int>(nullable: true),
                    DateValidationCDC = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateValidationCDT = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateValidationDRC = table.Column<DateTime>(type: "datetime", nullable: true),
                    TypeRapport = table.Column<int>(nullable: false),
                    IsGenerated = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORT", x => x.RapportId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_FRED_RAPPORT_STATUT_RapportStatutId",
                        column: x => x.RapportStatutId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_STATUT",
                        principalColumn: "RapportStatutId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE",
                schema: "dbo",
                columns: table => new
                {
                    ObjectifFlashTacheRapportRealiseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ObjectifFlashTacheId = table.Column<int>(nullable: false),
                    RapportId = table.Column<int>(nullable: false),
                    QuantiteRealise = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    DateRealise = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE", x => x.ObjectifFlashTacheRapportRealiseId);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE_FRED_OBJECTIF_FLASH_TACHE_ObjectifFlashTacheId",
                        column: x => x.ObjectifFlashTacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_OBJECTIF_FLASH_TACHE",
                        principalColumn: "ObjectifFlashTacheId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE_FRED_RAPPORT_RapportId",
                        column: x => x.RapportId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT",
                        principalColumn: "RapportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORT_TACHE",
                schema: "dbo",
                columns: table => new
                {
                    RapportTacheId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RapportId = table.Column<int>(nullable: false),
                    TacheId = table.Column<int>(nullable: false),
                    Commentaire = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORT_TACHE", x => x.RapportTacheId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_TACHE_FRED_RAPPORT_RapportId",
                        column: x => x.RapportId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT",
                        principalColumn: "RapportId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_TACHE_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORTPRIME_LIGNE",
                schema: "dbo",
                columns: table => new
                {
                    RapportPrimeLigneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    RapportPrimeId = table.Column<int>(nullable: false),
                    CiId = table.Column<int>(nullable: true),
                    AuteurVerrouId = table.Column<int>(nullable: true),
                    DateVerrou = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurValidationId = table.Column<int>(nullable: true),
                    DateValidation = table.Column<DateTime>(type: "datetime", nullable: true),
                    PersonnelId = table.Column<int>(nullable: false),
                    IsValidated = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORTPRIME_LIGNE", x => x.RapportPrimeLigneId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_REPARTITION_ECART",
                schema: "dbo",
                columns: table => new
                {
                    RepartitionEcartId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowIndex = table.Column<int>(nullable: false),
                    CiId = table.Column<int>(nullable: false),
                    DateCloture = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateComptable = table.Column<DateTime>(type: "datetime", nullable: true),
                    Libelle = table.Column<string>(nullable: true),
                    ValorisationInitiale = table.Column<decimal>(nullable: false),
                    ValorisationRectifiee = table.Column<decimal>(nullable: false),
                    MontantCapitalise = table.Column<decimal>(nullable: false),
                    Ecart = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_REPARTITION_ECART", x => x.RepartitionEcartId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CI",
                schema: "dbo",
                columns: table => new
                {
                    CiId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    Sep = table.Column<bool>(nullable: false, defaultValue: false),
                    EtablissementComptableId = table.Column<int>(nullable: true),
                    SocieteId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    DateOuverture = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateFermeture = table.Column<DateTime>(type: "datetime", nullable: true),
                    Adresse = table.Column<string>(maxLength: 500, nullable: true),
                    Adresse2 = table.Column<string>(nullable: true),
                    Adresse3 = table.Column<string>(nullable: true),
                    Ville = table.Column<string>(maxLength: 500, nullable: true),
                    CodePostal = table.Column<string>(maxLength: 20, nullable: true),
                    PaysId = table.Column<int>(nullable: true),
                    EnteteLivraison = table.Column<string>(maxLength: 100, nullable: true),
                    AdresseLivraison = table.Column<string>(maxLength: 500, nullable: true),
                    CodePostalLivraison = table.Column<string>(maxLength: 20, nullable: true),
                    VilleLivraison = table.Column<string>(maxLength: 500, nullable: true),
                    PaysLivraisonId = table.Column<int>(nullable: true),
                    AdresseFacturation = table.Column<string>(maxLength: 500, nullable: true),
                    CodePostalFacturation = table.Column<string>(maxLength: 20, nullable: true),
                    VilleFacturation = table.Column<string>(maxLength: 500, nullable: true),
                    PaysFacturationId = table.Column<int>(nullable: true),
                    LongitudeLocalisation = table.Column<double>(nullable: true),
                    LatitudeLocalisation = table.Column<double>(nullable: true),
                    FacturationEtablissement = table.Column<bool>(nullable: false),
                    ResponsableChantier = table.Column<string>(maxLength: 100, nullable: true),
                    ResponsableChantierId = table.Column<int>(nullable: true),
                    ResponsableAdministratifId = table.Column<int>(nullable: true),
                    FraisGeneraux = table.Column<decimal>(type: "decimal(4, 2)", nullable: true),
                    TauxHoraire = table.Column<decimal>(type: "decimal(4, 2)", nullable: true),
                    HoraireDebutM = table.Column<DateTime>(type: "datetime", nullable: true),
                    HoraireFinM = table.Column<DateTime>(type: "datetime", nullable: true),
                    HoraireDebutS = table.Column<DateTime>(type: "datetime", nullable: true),
                    HoraireFinS = table.Column<DateTime>(type: "datetime", nullable: true),
                    TypeCI = table.Column<string>(maxLength: 100, nullable: true),
                    CITypeId = table.Column<int>(nullable: true),
                    MontantHT = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    MontantDeviseId = table.Column<int>(nullable: true),
                    ZoneModifiable = table.Column<bool>(nullable: false, defaultValue: false),
                    CarburantActif = table.Column<bool>(nullable: false, defaultValue: false),
                    DureeChantier = table.Column<int>(nullable: true),
                    ChantierFRED = table.Column<bool>(nullable: false, defaultValue: false),
                    DateImport = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateUpdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsAstreinteActive = table.Column<bool>(nullable: false, defaultValue: false),
                    CodeInterne = table.Column<string>(nullable: true, computedColumnSql: "CONCAT(Code, '-', EtablissementComptableId, '-', SocieteId)"),
                    CompteInterneSepId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CodeExterne = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CI", x => x.CiId);
                    table.ForeignKey(
                        name: "FK_FRED_CI_FRED_CI_TYPE_CITypeId",
                        column: x => x.CITypeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CI_TYPE",
                        principalColumn: "CITypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CI_FRED_CI_CompteInterneSepId",
                        column: x => x.CompteInterneSepId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CI",
                        principalColumn: "CiId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CI_FRED_DEVISE_MontantDeviseId",
                        column: x => x.MontantDeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CI_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CI_FRED_PAYS_PaysFacturationId",
                        column: x => x.PaysFacturationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CI_FRED_PAYS_PaysId",
                        column: x => x.PaysId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CI_FRED_PAYS_PaysLivraisonId",
                        column: x => x.PaysLivraisonId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CI_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORT_LIGNE_MAJORATION",
                schema: "dbo",
                columns: table => new
                {
                    RapportLigneMajorationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RapportLigneId = table.Column<int>(nullable: false),
                    CodeMajorationId = table.Column<int>(nullable: false),
                    HeureMajoration = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORT_LIGNE_MAJORATION", x => x.RapportLigneMajorationId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_MAJORATION_FRED_RAPPORT_LIGNE_RapportLigneId",
                        column: x => x.RapportLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_LIGNE",
                        principalColumn: "RapportLigneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_COMMANDE_AVENANT",
                schema: "dbo",
                columns: table => new
                {
                    CommandeAvenantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommandeId = table.Column<int>(nullable: false),
                    NumeroAvenant = table.Column<int>(nullable: false),
                    AuteurValidationId = table.Column<int>(nullable: true),
                    DateValidation = table.Column<DateTime>(type: "datetime", nullable: true),
                    HangfireJobId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_COMMANDE_AVENANT", x => x.CommandeAvenantId);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_AVENANT_FRED_COMMANDE_CommandeId",
                        column: x => x.CommandeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE",
                        principalColumn: "CommandeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_COMMANDE_LIGNE_AVENANT",
                schema: "dbo",
                columns: table => new
                {
                    CommandeLigneAvenantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvenantId = table.Column<int>(nullable: false),
                    IsDiminution = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_COMMANDE_LIGNE_AVENANT", x => x.CommandeLigneAvenantId);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_LIGNE_AVENANT_FRED_COMMANDE_AVENANT_AvenantId",
                        column: x => x.AvenantId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE_AVENANT",
                        principalColumn: "CommandeAvenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_COMMANDE_LIGNE",
                schema: "dbo",
                columns: table => new
                {
                    CommandeLigneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommandeId = table.Column<int>(nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    TacheId = table.Column<int>(nullable: true),
                    RessourceId = table.Column<int>(nullable: true),
                    Quantite = table.Column<decimal>(type: "numeric(11, 3)", nullable: false),
                    PUHT = table.Column<decimal>(type: "numeric(11, 2)", nullable: false),
                    UniteId = table.Column<int>(nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AvenantLigneId = table.Column<int>(nullable: true),
                    NumeroLigne = table.Column<int>(nullable: true),
                    MaterielId = table.Column<int>(nullable: true),
                    PersonnelId = table.Column<int>(nullable: true),
                    Commentaire = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_COMMANDE_LIGNE", x => x.CommandeLigneId);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_LIGNE_FRED_COMMANDE_LIGNE_AVENANT_AvenantLigneId",
                        column: x => x.AvenantLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE_LIGNE_AVENANT",
                        principalColumn: "CommandeLigneAvenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_LIGNE_FRED_COMMANDE_CommandeId",
                        column: x => x.CommandeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE",
                        principalColumn: "CommandeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_LIGNE_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_LIGNE_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_COMMANDE_LIGNE_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PIECE_JOINTE_COMMANDE",
                schema: "dbo",
                columns: table => new
                {
                    PieceJointeCommandeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    PieceJointeId = table.Column<int>(nullable: false),
                    CommandeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PIECE_JOINTE_COMMANDE", x => x.PieceJointeCommandeId);
                    table.ForeignKey(
                        name: "FK_FRED_PIECE_JOINTE_COMMANDE_FRED_COMMANDE_CommandeId",
                        column: x => x.CommandeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_COMMANDE",
                        principalColumn: "CommandeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_VISA",
                schema: "dbo",
                columns: table => new
                {
                    VisaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommandeId = table.Column<int>(nullable: false),
                    UtilisateurId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "FRED_ZONE_DE_TRAVAIL",
                schema: "dbo",
                columns: table => new
                {
                    ContratInterimaireId = table.Column<int>(nullable: false, defaultValue: 0),
                    EtablissementComptableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ZONE_DE_TRAVAIL", x => new { x.ContratInterimaireId, x.EtablissementComptableId });
                    table.ForeignKey(
                        name: "FK_FRED_ZONE_DE_TRAVAIL_FRED_CONTRAT_INTERIMAIRE_ContratInterimaireId",
                        column: x => x.ContratInterimaireId,
                        principalSchema: "dbo",
                        principalTable: "FRED_CONTRAT_INTERIMAIRE",
                        principalColumn: "PersonnelFournisseurSocieteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CONTROLE_POINTAGE_ERREUR",
                schema: "dbo",
                columns: table => new
                {
                    ControlePointageErreurId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(nullable: false),
                    ControlePointageId = table.Column<int>(nullable: false),
                    PersonnelId = table.Column<int>(nullable: false),
                    DateRapport = table.Column<DateTime>(type: "datetime", nullable: true),
                    CodeCi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CONTROLE_POINTAGE_ERREUR", x => x.ControlePointageErreurId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE",
                schema: "dbo",
                columns: table => new
                {
                    UtilisateurRoleOrganisationDeviseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UtilisateurId = table.Column<int>(nullable: false),
                    OrganisationId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: true),
                    CommandeSeuil = table.Column<decimal>(type: "decimal(11, 2)", nullable: true),
                    DelegationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE", x => x.UtilisateurRoleOrganisationDeviseId);
                    table.ForeignKey(
                        name: "FK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_FRED_ROLE_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ROLE",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PIECE_JOINTE_RECEPTION",
                schema: "dbo",
                columns: table => new
                {
                    PieceJointeReceptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    PieceJointeId = table.Column<int>(nullable: false),
                    ReceptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PIECE_JOINTE_RECEPTION", x => x.PieceJointeReceptionId);
                    table.ForeignKey(
                        name: "FK_FRED_PIECE_JOINTE_RECEPTION_FRED_DEPENSE_ACHAT_ReceptionId",
                        column: x => x.ReceptionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEPENSE_ACHAT",
                        principalColumn: "DepenseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                schema: "dbo",
                columns: table => new
                {
                    BaremeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    RessourceId = table.Column<int>(nullable: false),
                    UniteId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    Statut = table.Column<int>(nullable: false),
                    PeriodeDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    PeriodeFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    Prix = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    PrixChauffeur = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    PrixConduite = table.Column<decimal>(type: "decimal(18, 3)", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BAREME_EXPLOITATION_ORGANISATION", x => x.BaremeId);
                    table.ForeignKey(
                        name: "FK_FRED_BAREME_EXPLOITATION_ORGANISATION_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_BAREME_EXPLOITATION_ORGANISATION_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_BAREME_EXPLOITATION_ORGANISATION_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_BAREME_EXPLOITATION_ORGANISATION_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_BUDGET_BIBLIOTHEQUE_PRIX",
                schema: "dbo",
                columns: table => new
                {
                    BudgetBibliothequePrixId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_BUDGET_BIBLIOTHEQUE_PRIX", x => x.BudgetBibliothequePrixId);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FACTURE",
                schema: "dbo",
                columns: table => new
                {
                    FactureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: true),
                    FournisseurId = table.Column<int>(nullable: true),
                    EtablissementId = table.Column<int>(nullable: true),
                    NoFacture = table.Column<string>(maxLength: 50, nullable: true),
                    JournalId = table.Column<int>(nullable: true),
                    Commentaire = table.Column<string>(maxLength: 500, nullable: true),
                    NoBonlivraison = table.Column<string>(maxLength: 100, nullable: true),
                    NoBonCommande = table.Column<string>(maxLength: 100, nullable: true),
                    Typefournisseur = table.Column<string>(maxLength: 50, nullable: true),
                    CompteFournisseur = table.Column<string>(maxLength: 50, nullable: true),
                    DeviseId = table.Column<int>(nullable: true),
                    DateComptable = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateGestion = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateFacture = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateEcheance = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCloture = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurClotureId = table.Column<int>(nullable: true),
                    NoFactureFournisseur = table.Column<string>(maxLength: 100, nullable: true),
                    NoFMFI = table.Column<string>(maxLength: 50, nullable: true),
                    ModeReglement = table.Column<string>(maxLength: 100, nullable: true),
                    Folio = table.Column<string>(maxLength: 10, nullable: true),
                    MontantHT = table.Column<decimal>(type: "numeric(15, 3)", nullable: true),
                    MontantTVA = table.Column<decimal>(type: "numeric(15, 3)", nullable: true),
                    MontantTTC = table.Column<decimal>(type: "numeric(15, 3)", nullable: false),
                    CompteGeneral = table.Column<string>(maxLength: 50, nullable: true),
                    DateImport = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "getdate()"),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "getdate()"),
                    UtilisateurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    UtilisateurModificationId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    UtilisateurSupressionId = table.Column<int>(nullable: true),
                    DateRapprochement = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurRapprochementId = table.Column<int>(nullable: true),
                    Cachee = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FACTURE", x => x.FactureId);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURE_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURE_FRED_FOURNISSEUR_FournisseurId",
                        column: x => x.FournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FACTURE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                schema: "dbo",
                columns: table => new
                {
                    ParametrageReferentielEtenduId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    DeviseId = table.Column<int>(nullable: false),
                    ReferentielEtenduId = table.Column<int>(nullable: false),
                    UniteId = table.Column<int>(nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(11, 2)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU", x => x.ParametrageReferentielEtenduId);
                    table.ForeignKey(
                        name: "FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_FRED_DEVISE_DeviseId",
                        column: x => x.DeviseId,
                        principalSchema: "dbo",
                        principalTable: "FRED_DEVISE",
                        principalColumn: "DeviseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_EQUIPE_PERSONNEL",
                schema: "dbo",
                columns: table => new
                {
                    EquipePersonnelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EquipePersoId = table.Column<int>(nullable: false),
                    PersonnelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_EQUIPE_PERSONNEL", x => x.EquipePersonnelId);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PERSONNEL",
                schema: "dbo",
                columns: table => new
                {
                    PersonnelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: true),
                    Matricule = table.Column<string>(maxLength: 10, nullable: true),
                    Nom = table.Column<string>(maxLength: 150, nullable: true),
                    Prenom = table.Column<string>(maxLength: 150, nullable: true),
                    RessourceId = table.Column<int>(nullable: true),
                    MaterielId = table.Column<int>(nullable: true),
                    Statut = table.Column<string>(maxLength: 1, nullable: true),
                    CategoriePerso = table.Column<string>(maxLength: 1, nullable: true),
                    IsInterne = table.Column<bool>(nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    UtilisateurIdCreation = table.Column<int>(nullable: true),
                    UtilisateurIdModification = table.Column<int>(nullable: true),
                    UtilisateurIdSuppression = table.Column<int>(nullable: true),
                    IsInterimaire = table.Column<bool>(nullable: false),
                    EtablissementPayeId = table.Column<int>(nullable: true),
                    Adresse1 = table.Column<string>(maxLength: 250, nullable: true),
                    Adresse2 = table.Column<string>(maxLength: 250, nullable: true),
                    Adresse3 = table.Column<string>(maxLength: 250, nullable: true),
                    CodePostal = table.Column<string>(maxLength: 20, nullable: true),
                    Ville = table.Column<string>(maxLength: 250, nullable: true),
                    PaysLabel = table.Column<string>(maxLength: 150, nullable: true),
                    PaysId = table.Column<int>(nullable: true),
                    Tel1 = table.Column<string>(maxLength: 50, nullable: true),
                    Tel2 = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    LongitudeDomicile = table.Column<double>(nullable: true),
                    LatitudeDomicile = table.Column<double>(nullable: true),
                    EtablissementRattachementId = table.Column<int>(nullable: true),
                    TypeRattachement = table.Column<string>(type: "char", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    DateEntree = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateSortie = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsSaisieManuelle = table.Column<bool>(nullable: true),
                    EquipeFavoriteId = table.Column<int>(nullable: true),
                    PersonnelImageId = table.Column<int>(nullable: true),
                    ManagerId = table.Column<int>(nullable: true),
                    CodeEmploi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PERSONNEL", x => x.PersonnelId);
                    table.ForeignKey(
                        name: "FK_FRED_PERSONNEL_FRED_PERSONNEL_ManagerId",
                        column: x => x.ManagerId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PERSONNEL_FRED_PAYS_PaysId",
                        column: x => x.PaysId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PERSONNEL_FRED_PERSONNEL_IMAGE_PersonnelImageId",
                        column: x => x.PersonnelImageId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL_IMAGE",
                        principalColumn: "PersonnelImageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PERSONNEL_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PERSONNEL_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_DELEGATION",
                schema: "dbo",
                columns: table => new
                {
                    DelegationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PersonnelAuteurId = table.Column<int>(nullable: false, defaultValue: 0),
                    PersonnelDelegantId = table.Column<int>(nullable: false, defaultValue: 0),
                    PersonnelDelegueId = table.Column<int>(nullable: false, defaultValue: 0),
                    Activated = table.Column<bool>(nullable: false, defaultValue: false),
                    DateDeDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateDeFin = table.Column<DateTime>(type: "datetime", nullable: false),
                    Commentaire = table.Column<string>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateDesactivation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_DELEGATION", x => x.DelegationId);
                    table.ForeignKey(
                        name: "FK_FRED_DELEGATION_FRED_PERSONNEL_PersonnelAuteurId",
                        column: x => x.PersonnelAuteurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DELEGATION_FRED_PERSONNEL_PersonnelDelegantId",
                        column: x => x.PersonnelDelegantId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_DELEGATION_FRED_PERSONNEL_PersonnelDelegueId",
                        column: x => x.PersonnelDelegueId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_EMAIL_SUBSCRIPTION",
                schema: "dbo",
                columns: table => new
                {
                    EmailSouscriptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateDernierEnvoie = table.Column<DateTime>(type: "datetime", nullable: true),
                    PersonnelId = table.Column<int>(nullable: false),
                    EmailSouscriptionKey = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_EMAIL_SUBSCRIPTION", x => x.EmailSouscriptionId);
                    table.ForeignKey(
                        name: "FK_FRED_EMAIL_SUBSCRIPTION_FRED_PERSONNEL_PersonnelId",
                        column: x => x.PersonnelId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_EQUIPE",
                schema: "dbo",
                columns: table => new
                {
                    EquipeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProprietaireId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_EQUIPE", x => x.EquipeId);
                    table.ForeignKey(
                        name: "FK_FRED_EQUIPE_FRED_PERSONNEL_ProprietaireId",
                        column: x => x.ProprietaireId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_MATRICULE_EXTERNE",
                schema: "dbo",
                columns: table => new
                {
                    MatriculeExterneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PersonnelId = table.Column<int>(nullable: false),
                    Matricule = table.Column<string>(maxLength: 150, nullable: false),
                    Source = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_MATRICULE_EXTERNE", x => x.MatriculeExterneId);
                    table.ForeignKey(
                        name: "FK_FRED_MATRICULE_EXTERNE_FRED_PERSONNEL_PersonnelId",
                        column: x => x.PersonnelId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_UTILISATEUR",
                schema: "dbo",
                columns: table => new
                {
                    UtilisateurId = table.Column<int>(nullable: false, defaultValue: 0),
                    Login = table.Column<string>(maxLength: 50, nullable: true),
                    DateDerniereConnexion = table.Column<DateTime>(type: "datetime", nullable: true),
                    FayatAccessDirectoryId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSupression = table.Column<DateTime>(type: "datetime", nullable: true),
                    UtilisateurIdCreation = table.Column<int>(nullable: true),
                    UtilisateurIdModification = table.Column<int>(nullable: true),
                    UtilisateurIdSupression = table.Column<int>(nullable: true),
                    SuperAdmin = table.Column<bool>(nullable: false, defaultValue: false),
                    Folio = table.Column<string>(maxLength: 10, nullable: true),
                    CommandeManuelleAllowed = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_UTILISATEUR", x => x.UtilisateurId);
                    table.ForeignKey(
                        name: "FK_FRED_UTILISATEUR_FRED_EXTERNALDIRECTORY_FayatAccessDirectoryId",
                        column: x => x.FayatAccessDirectoryId,
                        principalSchema: "dbo",
                        principalTable: "FRED_EXTERNALDIRECTORY",
                        principalColumn: "FayatAccessDirectoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_UTILISATEUR_FRED_PERSONNEL_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_UTILISATEUR_FRED_UTILISATEUR_UtilisateurIdCreation",
                        column: x => x.UtilisateurIdCreation,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_UTILISATEUR_FRED_UTILISATEUR_UtilisateurIdModification",
                        column: x => x.UtilisateurIdModification,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_UTILISATEUR_FRED_UTILISATEUR_UtilisateurIdSupression",
                        column: x => x.UtilisateurIdSupression,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CHAPITRE",
                schema: "dbo",
                columns: table => new
                {
                    ChapitreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupeId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CHAPITRE", x => x.ChapitreId);
                    table.ForeignKey(
                        name: "FK_FRED_CHAPITRE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CHAPITRE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CHAPITRE_FRED_UTILISATEUR_AuteurSuppressionId",
                        column: x => x.AuteurSuppressionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CHAPITRE_FRED_GROUPE_GroupeId",
                        column: x => x.GroupeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE",
                        principalColumn: "GroupeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CODE_MAJORATION",
                schema: "dbo",
                columns: table => new
                {
                    CodeMajorationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    EtatPublic = table.Column<bool>(nullable: false),
                    IsActif = table.Column<bool>(nullable: false),
                    IsHeureNuit = table.Column<bool>(nullable: false, defaultValue: false),
                    GroupeId = table.Column<int>(nullable: false),
                    DateCreation = table.Column<DateTime>(nullable: true),
                    DateModification = table.Column<DateTime>(nullable: true),
                    DateSuppression = table.Column<DateTime>(nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CODE_MAJORATION", x => x.CodeMajorationId);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_MAJORATION_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_MAJORATION_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_MAJORATION_FRED_UTILISATEUR_AuteurSuppressionId",
                        column: x => x.AuteurSuppressionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_MAJORATION_FRED_GROUPE_GroupeId",
                        column: x => x.GroupeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE",
                        principalColumn: "GroupeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CODE_ZONE_DEPLACEMENT",
                schema: "dbo",
                columns: table => new
                {
                    CodeZoneDeplacementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Libelle = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    AuteurCreation = table.Column<int>(nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModification = table.Column<int>(nullable: true),
                    IsActif = table.Column<bool>(nullable: false),
                    SocieteId = table.Column<int>(nullable: false),
                    KmMini = table.Column<int>(nullable: false, defaultValue: 0),
                    KmMaxi = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CODE_ZONE_DEPLACEMENT", x => x.CodeZoneDeplacementId);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_ZONE_DEPLACEMENT_FRED_UTILISATEUR_AuteurCreation",
                        column: x => x.AuteurCreation,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_ZONE_DEPLACEMENT_FRED_UTILISATEUR_AuteurModification",
                        column: x => x.AuteurModification,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_CODE_ZONE_DEPLACEMENT_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ETABLISSEMENT_COMPTABLE",
                schema: "dbo",
                columns: table => new
                {
                    EtablissementComptableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    SocieteId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    Adresse = table.Column<string>(maxLength: 500, nullable: true),
                    Ville = table.Column<string>(maxLength: 500, nullable: true),
                    CodePostal = table.Column<string>(maxLength: 20, nullable: true),
                    PaysId = table.Column<int>(nullable: true),
                    ModuleCommandeEnabled = table.Column<bool>(nullable: false),
                    ModuleProductionEnabled = table.Column<bool>(nullable: false),
                    RessourcesRecommandeesEnabled = table.Column<bool>(nullable: false, defaultValue: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ETABLISSEMENT_COMPTABLE", x => x.EtablissementComptableId);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_COMPTABLE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_COMPTABLE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_COMPTABLE_FRED_UTILISATEUR_AuteurSuppressionId",
                        column: x => x.AuteurSuppressionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_COMPTABLE_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_COMPTABLE_FRED_PAYS_PaysId",
                        column: x => x.PaysId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_COMPTABLE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FAMILLE_OPERATION_DIVERSE",
                schema: "dbo",
                columns: table => new
                {
                    FamilleOperationDiverseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 6, nullable: true),
                    Libelle = table.Column<string>(maxLength: 250, nullable: true),
                    IsAccrued = table.Column<bool>(nullable: false, defaultValue: false),
                    MustHaveOrder = table.Column<bool>(nullable: false, defaultValue: false),
                    SocieteId = table.Column<int>(nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    IsValued = table.Column<bool>(nullable: false, defaultValue: false),
                    TacheId = table.Column<int>(nullable: false, defaultValue: 0),
                    RessourceId = table.Column<int>(nullable: false, defaultValue: 0),
                    CategoryValorisationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FAMILLE_OPERATION_DIVERSE", x => x.FamilleOperationDiverseId);
                    table.ForeignKey(
                        name: "FK_FRED_FAMILLE_OPERATION_DIVERSE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FAMILLE_OPERATION_DIVERSE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_FAMILLE_OPERATION_DIVERSE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_FAVORI_UTILISATEUR",
                schema: "dbo",
                columns: table => new
                {
                    FavoriId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UtilisateurId = table.Column<int>(nullable: false),
                    Libelle = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Couleur = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    TypeFavori = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    UrlFavori = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    Search = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_FAVORI_UTILISATEUR", x => x.FavoriId);
                    table.ForeignKey(
                        name: "FK_FRED_FAVORI_UTILISATEUR_FRED_UTILISATEUR_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_JOURNAL",
                schema: "dbo",
                columns: table => new
                {
                    JournalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: false),
                    ParentFamilyODWithOrder = table.Column<int>(nullable: false, defaultValue: 0),
                    ParentFamilyODWithoutOrder = table.Column<int>(nullable: false, defaultValue: 0),
                    Code = table.Column<string>(maxLength: 3, nullable: false),
                    Libelle = table.Column<string>(maxLength: 250, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateCloture = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurClotureId = table.Column<int>(nullable: true),
                    ImportFacture = table.Column<bool>(nullable: false, defaultValue: false),
                    TypeJournal = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_JOURNAL", x => x.JournalId);
                    table.ForeignKey(
                        name: "FK_FRED_JOURNAL_FRED_UTILISATEUR_AuteurClotureId",
                        column: x => x.AuteurClotureId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_JOURNAL_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_JOURNAL_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_JOURNAL_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_LOT_FAR",
                schema: "dbo",
                columns: table => new
                {
                    LotFarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NumeroLot = table.Column<int>(nullable: false),
                    DateComptable = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_LOT_FAR", x => x.LotFarId);
                    table.ForeignKey(
                        name: "FK_FRED_LOT_FAR_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_LOT_POINTAGE",
                schema: "dbo",
                columns: table => new
                {
                    LotPointageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    Periode = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateVisa = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurVisaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_LOT_POINTAGE", x => x.LotPointageId);
                    table.ForeignKey(
                        name: "FK_FRED_LOT_POINTAGE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_LOT_POINTAGE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_LOT_POINTAGE_FRED_UTILISATEUR_AuteurVisaId",
                        column: x => x.AuteurVisaId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_NATURE",
                schema: "dbo",
                columns: table => new
                {
                    NatureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    SocieteId = table.Column<int>(nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    IsActif = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_NATURE", x => x.NatureId);
                    table.ForeignKey(
                        name: "FK_FRED_NATURE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_NATURE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_NATURE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PARAM_KEY",
                schema: "dbo",
                columns: table => new
                {
                    ParamKeyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurModificationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PARAM_KEY", x => x.ParamKeyId);
                    table.ForeignKey(
                        name: "FK_FRED_PARAM_KEY_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PARAM_KEY_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PIECE_JOINTE",
                schema: "dbo",
                columns: table => new
                {
                    PieceJointeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    Libelle = table.Column<string>(maxLength: 250, nullable: true),
                    Url = table.Column<string>(nullable: false),
                    SizeInKo = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PIECE_JOINTE", x => x.PieceJointeId);
                    table.ForeignKey(
                        name: "FK_FRED_PIECE_JOINTE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PIECE_JOINTE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PRIME",
                schema: "dbo",
                columns: table => new
                {
                    PrimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false),
                    PrimeType = table.Column<int>(nullable: false, defaultValue: 0),
                    NombreHeuresMax = table.Column<double>(type: "float", nullable: false),
                    Actif = table.Column<bool>(nullable: false),
                    PrimePartenaire = table.Column<bool>(nullable: false),
                    Publique = table.Column<bool>(nullable: false),
                    SocieteId = table.Column<int>(nullable: true),
                    SeuilMensuel = table.Column<double>(nullable: true),
                    GroupeId = table.Column<int>(nullable: true),
                    TargetPersonnel = table.Column<int>(nullable: true, defaultValue: 0),
                    MultiPerDay = table.Column<bool>(nullable: true),
                    IsPrimeAstreinte = table.Column<bool>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PRIME", x => x.PrimeId);
                    table.ForeignKey(
                        name: "FK_FRED_PRIME_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PRIME_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PRIME_FRED_UTILISATEUR_AuteurSuppressionId",
                        column: x => x.AuteurSuppressionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PRIME_FRED_GROUPE_GroupeId",
                        column: x => x.GroupeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE",
                        principalColumn: "GroupeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PRIME_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORTPRIME",
                schema: "dbo",
                columns: table => new
                {
                    RapportPrimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    DateRapportPrime = table.Column<DateTime>(type: "datetime", nullable: false),
                    SocieteId = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORTPRIME", x => x.RapportPrimeId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORTPRIME_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORTPRIME_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORTPRIME_FRED_UTILISATEUR_AuteurSuppressionId",
                        column: x => x.AuteurSuppressionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORTPRIME_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_REMONTEE_VRAC",
                schema: "dbo",
                columns: table => new
                {
                    RemonteeVracId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    Statut = table.Column<int>(nullable: false),
                    Periode = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_REMONTEE_VRAC", x => x.RemonteeVracId);
                    table.ForeignKey(
                        name: "FK_FRED_REMONTEE_VRAC_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_REMPLACEMENT_TACHE",
                schema: "dbo",
                columns: table => new
                {
                    RemplacementTacheId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Annulable = table.Column<bool>(nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    DateComptableRemplacement = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateRemplacement = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    GroupeRemplacementTacheId = table.Column<int>(nullable: false),
                    RangRemplacement = table.Column<int>(nullable: false),
                    TacheId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_REMPLACEMENT_TACHE", x => x.RemplacementTacheId);
                    table.ForeignKey(
                        name: "FK_FRED_REMPLACEMENT_TACHE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_REMPLACEMENT_TACHE_FRED_UTILISATEUR_AuteurSuppressionId",
                        column: x => x.AuteurSuppressionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_REMPLACEMENT_TACHE_FRED_GROUPE_REMPLACEMENT_TACHE_GroupeRemplacementTacheId",
                        column: x => x.GroupeRemplacementTacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_GROUPE_REMPLACEMENT_TACHE",
                        principalColumn: "GroupeRemplacementTacheId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_REMPLACEMENT_TACHE_FRED_TACHE_TacheId",
                        column: x => x.TacheId,
                        principalSchema: "dbo",
                        principalTable: "FRED_TACHE",
                        principalColumn: "TacheId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ETABLISSEMENT_PAIE",
                schema: "dbo",
                columns: table => new
                {
                    EtablissementPaieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: true),
                    Adresse = table.Column<string>(maxLength: 500, nullable: true),
                    Adresse2 = table.Column<string>(nullable: true),
                    Adresse3 = table.Column<string>(nullable: true),
                    Ville = table.Column<string>(nullable: true),
                    CodePostal = table.Column<string>(nullable: true),
                    PaysId = table.Column<int>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    IsAgenceRattachement = table.Column<bool>(nullable: false),
                    AgenceRattachementId = table.Column<int>(nullable: true),
                    GestionIndemnites = table.Column<bool>(nullable: false),
                    HorsRegion = table.Column<bool>(nullable: false),
                    Actif = table.Column<bool>(nullable: false),
                    SocieteId = table.Column<int>(nullable: false),
                    EtablissementComptableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ETABLISSEMENT_PAIE", x => x.EtablissementPaieId);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_PAIE_FRED_ETABLISSEMENT_PAIE_AgenceRattachementId",
                        column: x => x.AgenceRattachementId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ETABLISSEMENT_PAIE",
                        principalColumn: "EtablissementPaieId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_PAIE_FRED_ETABLISSEMENT_COMPTABLE_EtablissementComptableId",
                        column: x => x.EtablissementComptableId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ETABLISSEMENT_COMPTABLE",
                        principalColumn: "EtablissementComptableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_PAIE_FRED_PAYS_PaysId",
                        column: x => x.PaysId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PAYS",
                        principalColumn: "PaysId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ETABLISSEMENT_PAIE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_MATERIEL",
                schema: "dbo",
                columns: table => new
                {
                    MaterielId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: false),
                    RessourceId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 70, nullable: false),
                    Libelle = table.Column<string>(maxLength: 500, nullable: false),
                    Actif = table.Column<bool>(nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaterielLocation = table.Column<bool>(nullable: false, defaultValue: false),
                    FournisseurId = table.Column<int>(nullable: true),
                    DateDebutLocation = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateFinLocation = table.Column<DateTime>(type: "datetime", nullable: true),
                    ClasseFamilleCode = table.Column<string>(nullable: true),
                    ClasseFamilleLibelle = table.Column<string>(nullable: true),
                    IsStorm = table.Column<bool>(nullable: false, defaultValue: false),
                    Fabriquant = table.Column<string>(nullable: true),
                    VIN = table.Column<string>(nullable: true),
                    DateMiseEnService = table.Column<DateTime>(type: "datetime", nullable: true),
                    Immatriculation = table.Column<string>(nullable: true),
                    DimensionH = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    DimensionL = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    Dimensiionl = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    Puissance = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    UnitePuissance = table.Column<string>(nullable: true),
                    UniteDimension = table.Column<string>(nullable: true),
                    SiteRestitution = table.Column<string>(nullable: true),
                    EtablissementComptableId = table.Column<int>(nullable: true),
                    Commentaire = table.Column<string>(nullable: true),
                    SiteAppartenanceId = table.Column<int>(nullable: true),
                    IsLocation = table.Column<bool>(nullable: false, defaultValue: false),
                    IsImported = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_MATERIEL", x => x.MaterielId);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_FRED_UTILISATEUR_AuteurSuppressionId",
                        column: x => x.AuteurSuppressionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_FRED_ETABLISSEMENT_COMPTABLE_EtablissementComptableId",
                        column: x => x.EtablissementComptableId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ETABLISSEMENT_COMPTABLE",
                        principalColumn: "EtablissementComptableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_FRED_FOURNISSEUR_FournisseurId",
                        column: x => x.FournisseurId,
                        principalSchema: "dbo",
                        principalTable: "FRED_FOURNISSEUR",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_FRED_SITE_SiteAppartenanceId",
                        column: x => x.SiteAppartenanceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SITE",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_ORGA_LIENS",
                schema: "dbo",
                columns: table => new
                {
                    OrgaLiensId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    SocieteId = table.Column<int>(nullable: true),
                    EtablissementComptableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_ORGA_LIENS", x => x.OrgaLiensId);
                    table.ForeignKey(
                        name: "FK_FRED_ORGA_LIENS_FRED_ETABLISSEMENT_COMPTABLE_EtablissementComptableId",
                        column: x => x.EtablissementComptableId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ETABLISSEMENT_COMPTABLE",
                        principalColumn: "EtablissementComptableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ORGA_LIENS_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_ORGA_LIENS_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_CONTROLE_POINTAGE",
                schema: "dbo",
                columns: table => new
                {
                    ControlePointageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    Statut = table.Column<int>(nullable: false),
                    TypeControle = table.Column<int>(nullable: false),
                    LotPointageId = table.Column<int>(nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_CONTROLE_POINTAGE", x => x.ControlePointageId);
                    table.ForeignKey(
                        name: "FK_FRED_CONTROLE_POINTAGE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_CONTROLE_POINTAGE_FRED_LOT_POINTAGE_LotPointageId",
                        column: x => x.LotPointageId,
                        principalSchema: "dbo",
                        principalTable: "FRED_LOT_POINTAGE",
                        principalColumn: "LotPointageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_SOCIETE_RESSOURCE_NATURE",
                schema: "dbo",
                columns: table => new
                {
                    ReferentielEtenduId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocieteId = table.Column<int>(nullable: false),
                    RessourceId = table.Column<int>(nullable: false),
                    NatureId = table.Column<int>(nullable: true),
                    Achats = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_SOCIETE_RESSOURCE_NATURE", x => x.ReferentielEtenduId);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_RESSOURCE_NATURE_FRED_NATURE_NatureId",
                        column: x => x.NatureId,
                        principalSchema: "dbo",
                        principalTable: "FRED_NATURE",
                        principalColumn: "NatureId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_RESSOURCE_NATURE_FRED_RESSOURCE_RessourceId",
                        column: x => x.RessourceId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RESSOURCE",
                        principalColumn: "RessourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_SOCIETE_RESSOURCE_NATURE_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_PARAM_VALUE",
                schema: "dbo",
                columns: table => new
                {
                    ParamValueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    ParamKeyId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 500, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuteurCreationId = table.Column<int>(nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_PARAM_VALUE", x => x.ParamValueId);
                    table.ForeignKey(
                        name: "FK_FRED_PARAM_VALUE_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PARAM_VALUE_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PARAM_VALUE_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_PARAM_VALUE_FRED_PARAM_KEY_ParamKeyId",
                        column: x => x.ParamKeyId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PARAM_KEY",
                        principalColumn: "ParamKeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_POINTAGE_ANTICIPE_PRIME",
                schema: "dbo",
                columns: table => new
                {
                    PointageAnticipePrimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PointageAnticipeId = table.Column<int>(nullable: false),
                    PrimeId = table.Column<int>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false),
                    HeurePrime = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_POINTAGE_ANTICIPE_PRIME", x => x.PointageAnticipePrimeId);
                    table.ForeignKey(
                        name: "FK_FRED_POINTAGE_ANTICIPE_PRIME_FRED_POINTAGE_ANTICIPE_PointageAnticipeId",
                        column: x => x.PointageAnticipeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_POINTAGE_ANTICIPE",
                        principalColumn: "PointageAnticipeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_POINTAGE_ANTICIPE_PRIME_FRED_PRIME_PrimeId",
                        column: x => x.PrimeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PRIME",
                        principalColumn: "PrimeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORT_LIGNE_PRIME",
                schema: "dbo",
                columns: table => new
                {
                    RapportLignePrimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RapportLigneId = table.Column<int>(nullable: false),
                    PrimeId = table.Column<int>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false),
                    HeurePrime = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORT_LIGNE_PRIME", x => x.RapportLignePrimeId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_PRIME_FRED_PRIME_PrimeId",
                        column: x => x.PrimeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PRIME",
                        principalColumn: "PrimeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORT_LIGNE_PRIME_FRED_RAPPORT_LIGNE_RapportLigneId",
                        column: x => x.RapportLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORT_LIGNE",
                        principalColumn: "RapportLigneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RAPPORTPRIME_LIGNE_PRIME",
                schema: "dbo",
                columns: table => new
                {
                    RapportPrimeLignePrimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PrimeId = table.Column<int>(nullable: false),
                    RapportPrimeLigneId = table.Column<int>(nullable: false),
                    Montant = table.Column<double>(nullable: true),
                    IsSendToAnael = table.Column<bool>(nullable: false, defaultValue: false),
                    UpdateUtilisateurId = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RAPPORTPRIME_LIGNE_PRIME", x => x.RapportPrimeLignePrimeId);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORTPRIME_LIGNE_PRIME_FRED_PRIME_PrimeId",
                        column: x => x.PrimeId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PRIME",
                        principalColumn: "PrimeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_RAPPORTPRIME_LIGNE_PRIME_FRED_RAPPORTPRIME_LIGNE_RapportPrimeLigneId",
                        column: x => x.RapportPrimeLigneId,
                        principalSchema: "dbo",
                        principalTable: "FRED_RAPPORTPRIME_LIGNE",
                        principalColumn: "RapportPrimeLigneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRED_REMONTEE_VRAC_ERREUR",
                schema: "dbo",
                columns: table => new
                {
                    RemonteeVracErreurId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RemonteeVracId = table.Column<int>(nullable: false),
                    SocieteId = table.Column<int>(nullable: false),
                    EtablissementPaieId = table.Column<int>(nullable: false),
                    PersonnelId = table.Column<int>(nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    CodeAbsenceFred = table.Column<string>(nullable: true),
                    CodeAbsenceAnael = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_REMONTEE_VRAC_ERREUR", x => x.RemonteeVracErreurId);
                    table.ForeignKey(
                        name: "FK_FRED_REMONTEE_VRAC_ERREUR_FRED_ETABLISSEMENT_PAIE_EtablissementPaieId",
                        column: x => x.EtablissementPaieId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ETABLISSEMENT_PAIE",
                        principalColumn: "EtablissementPaieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_REMONTEE_VRAC_ERREUR_FRED_PERSONNEL_PersonnelId",
                        column: x => x.PersonnelId,
                        principalSchema: "dbo",
                        principalTable: "FRED_PERSONNEL",
                        principalColumn: "PersonnelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_REMONTEE_VRAC_ERREUR_FRED_REMONTEE_VRAC_RemonteeVracId",
                        column: x => x.RemonteeVracId,
                        principalSchema: "dbo",
                        principalTable: "FRED_REMONTEE_VRAC",
                        principalColumn: "RemonteeVracId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_REMONTEE_VRAC_ERREUR_FRED_SOCIETE_SocieteId",
                        column: x => x.SocieteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE",
                        principalColumn: "SocieteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_MATERIEL_LOCATION",
                schema: "dbo",
                columns: table => new
                {
                    MaterielLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MaterielId = table.Column<int>(nullable: false),
                    Immatriculation = table.Column<string>(nullable: true),
                    Libelle = table.Column<string>(nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateSuppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuteurCreationId = table.Column<int>(nullable: true),
                    AuteurModificationId = table.Column<int>(nullable: true),
                    AuteurSuppressionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_MATERIEL_LOCATION", x => x.MaterielLocationId);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_LOCATION_FRED_UTILISATEUR_AuteurCreationId",
                        column: x => x.AuteurCreationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_LOCATION_FRED_UTILISATEUR_AuteurModificationId",
                        column: x => x.AuteurModificationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_LOCATION_FRED_UTILISATEUR_AuteurSuppressionId",
                        column: x => x.AuteurSuppressionId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UTILISATEUR",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FRED_MATERIEL_LOCATION_FRED_MATERIEL_MaterielId",
                        column: x => x.MaterielId,
                        principalSchema: "dbo",
                        principalTable: "FRED_MATERIEL",
                        principalColumn: "MaterielId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_RESSOURCE_RECOMMANDEE_ORGANISATION",
                schema: "dbo",
                columns: table => new
                {
                    RessourceRecommandeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<int>(nullable: false),
                    ReferentielEtenduId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_RESSOURCE_RECOMMANDEE_ORGANISATION", x => x.RessourceRecommandeeId);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_RECOMMANDEE_ORGANISATION_FRED_ORGANISATION_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "dbo",
                        principalTable: "FRED_ORGANISATION",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_RESSOURCE_RECOMMANDEE_ORGANISATION_FRED_SOCIETE_RESSOURCE_NATURE_ReferentielEtenduId",
                        column: x => x.ReferentielEtenduId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE_RESSOURCE_NATURE",
                        principalColumn: "ReferentielEtenduId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRED_UNITE_REFERENTIEL_ETENDU",
                schema: "dbo",
                columns: table => new
                {
                    UniteReferentielEtenduId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReferentielEtenduId = table.Column<int>(nullable: false),
                    UniteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRED_UNITE_REFERENTIEL_ETENDU", x => x.UniteReferentielEtenduId);
                    table.ForeignKey(
                        name: "FK_FRED_UNITE_REFERENTIEL_ETENDU_FRED_SOCIETE_RESSOURCE_NATURE_ReferentielEtenduId",
                        column: x => x.ReferentielEtenduId,
                        principalSchema: "dbo",
                        principalTable: "FRED_SOCIETE_RESSOURCE_NATURE",
                        principalColumn: "ReferentielEtenduId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FRED_UNITE_REFERENTIEL_ETENDU_FRED_UNITE_UniteId",
                        column: x => x.UniteId,
                        principalSchema: "dbo",
                        principalTable: "FRED_UNITE",
                        principalColumn: "UniteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_CiId",
                schema: "dbo",
                table: "FRED_AFFECTATION",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_PersonnelId",
                schema: "dbo",
                table: "FRED_AFFECTATION",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_MOYEN_AffectationMoyenTypeId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "AffectationMoyenTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_MOYEN_CiId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_MOYEN_ConducteurId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "ConducteurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_MOYEN_MaterielId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "MaterielId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_MOYEN_MaterielLocationId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "MaterielLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_MOYEN_PersonnelId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_MOYEN_SiteId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AFFECTATION_MOYEN_TYPE_AffectationMoyenFamilleId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN_TYPE",
                column: "AffectationMoyenFamilleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ASSOCIE_SEP_AssocieSepParentId",
                schema: "dbo",
                table: "FRED_ASSOCIE_SEP",
                column: "AssocieSepParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ASSOCIE_SEP_FournisseurId",
                schema: "dbo",
                table: "FRED_ASSOCIE_SEP",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ASSOCIE_SEP_SocieteAssocieeId",
                schema: "dbo",
                table: "FRED_ASSOCIE_SEP",
                column: "SocieteAssocieeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ASSOCIE_SEP_SocieteId",
                schema: "dbo",
                table: "FRED_ASSOCIE_SEP",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ASSOCIE_SEP_TypeParticipationSepId",
                schema: "dbo",
                table: "FRED_ASSOCIE_SEP",
                column: "TypeParticipationSepId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ASTREINTE_AffectationId",
                schema: "dbo",
                table: "FRED_ASTREINTE",
                column: "AffectationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_AvancementEtatId",
                schema: "dbo",
                table: "FRED_AVANCEMENT",
                column: "AvancementEtatId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_BudgetSousDetailId",
                schema: "dbo",
                table: "FRED_AVANCEMENT",
                column: "BudgetSousDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_CiId",
                schema: "dbo",
                table: "FRED_AVANCEMENT",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_DeviseId",
                schema: "dbo",
                table: "FRED_AVANCEMENT",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_AVANCEMENT_ETAT",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_RECETTE_BudgetRecetteId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_RECETTE",
                column: "BudgetRecetteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_TACHE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueBudgetPeriodeTache",
                schema: "dbo",
                table: "FRED_AVANCEMENT_TACHE",
                columns: new[] { "BudgetId", "Periode", "TacheId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_WORKFLOW_AuteurId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_WORKFLOW",
                column: "AuteurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_WORKFLOW_AvancementId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_WORKFLOW",
                column: "AvancementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_WORKFLOW_EtatCibleId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_WORKFLOW",
                column: "EtatCibleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_AVANCEMENT_WORKFLOW_EtatInitialId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_WORKFLOW",
                column: "EtatInitialId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_CIId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "CIId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_DeviseId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "ReferentielEtenduId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_UniteId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_CIId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "CIId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_DeviseId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_MaterielId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "MaterielId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_PersonnelId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "ReferentielEtenduId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_UniteId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_ORGANISATION_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_ORGANISATION_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_ORGANISATION_DeviseId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_ORGANISATION_OrganisationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_ORGANISATION_RessourceId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BAREME_EXPLOITATION_ORGANISATION_UniteId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BudgetEtatId",
                schema: "dbo",
                table: "FRED_BUDGET",
                column: "BudgetEtatId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_CiId",
                schema: "dbo",
                table: "FRED_BUDGET",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_DeviseId",
                schema: "dbo",
                table: "FRED_BUDGET",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_DeviseId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_OrganisationId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_BudgetBibliothequePrixId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "BudgetBibliothequePrixId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_RessourceId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_UniteId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO_ItemId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO_UniteId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_BUDGET_ETAT",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_REVISION_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BUDGET_REVISION",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_REVISION_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BUDGET_REVISION",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_REVISION_AuteurValidationId",
                schema: "dbo",
                table: "FRED_BUDGET_REVISION",
                column: "AuteurValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_REVISION_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_REVISION",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_T4_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_T4",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_T4_T3Id",
                schema: "dbo",
                table: "FRED_BUDGET_T4",
                column: "T3Id");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_T4_T4Id",
                schema: "dbo",
                table: "FRED_BUDGET_T4",
                column: "T4Id");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_T4_UniteId",
                schema: "dbo",
                table: "FRED_BUDGET_T4",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_T4_RESSOURCE_BudgetT4Id",
                schema: "dbo",
                table: "FRED_BUDGET_T4_RESSOURCE",
                column: "BudgetT4Id");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_T4_RESSOURCE_RessourceId",
                schema: "dbo",
                table: "FRED_BUDGET_T4_RESSOURCE",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_T4_RESSOURCE_UniteId",
                schema: "dbo",
                table: "FRED_BUDGET_T4_RESSOURCE",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_TACHE_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_TACHE",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_BUDGET_TACHE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_WORKFLOW_AuteurId",
                schema: "dbo",
                table: "FRED_BUDGET_WORKFLOW",
                column: "AuteurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_WORKFLOW_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_WORKFLOW",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_WORKFLOW_EtatCibleId",
                schema: "dbo",
                table: "FRED_BUDGET_WORKFLOW",
                column: "EtatCibleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_BUDGET_WORKFLOW_EtatInitialId",
                schema: "dbo",
                table: "FRED_BUDGET_WORKFLOW",
                column: "EtatInitialId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CARBURANT_Code",
                schema: "dbo",
                table: "FRED_CARBURANT",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CARBURANT_UniteId",
                schema: "dbo",
                table: "FRED_CARBURANT",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CARBURANT_ORGANISATION_DEVISE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_CARBURANT_ORGANISATION_DEVISE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CARBURANT_ORGANISATION_DEVISE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_CARBURANT_ORGANISATION_DEVISE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CARBURANT_ORGANISATION_DEVISE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_CARBURANT_ORGANISATION_DEVISE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CARBURANT_ORGANISATION_DEVISE_CarburantId",
                schema: "dbo",
                table: "FRED_CARBURANT_ORGANISATION_DEVISE",
                column: "CarburantId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CARBURANT_ORGANISATION_DEVISE_DeviseId",
                schema: "dbo",
                table: "FRED_CARBURANT_ORGANISATION_DEVISE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CARBURANT_ORGANISATION_DEVISE_OrganisationId",
                schema: "dbo",
                table: "FRED_CARBURANT_ORGANISATION_DEVISE",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CHAPITRE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_CHAPITRE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CHAPITRE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_CHAPITRE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CHAPITRE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_CHAPITRE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CHAPITRE_GroupeId",
                schema: "dbo",
                table: "FRED_CHAPITRE",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndGroupe",
                schema: "dbo",
                table: "FRED_CHAPITRE",
                columns: new[] { "Code", "GroupeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_CITypeId",
                schema: "dbo",
                table: "FRED_CI",
                column: "CITypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeInterne",
                schema: "dbo",
                table: "FRED_CI",
                column: "CodeInterne",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_CompteInterneSepId",
                schema: "dbo",
                table: "FRED_CI",
                column: "CompteInterneSepId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_EtablissementComptableId",
                schema: "dbo",
                table: "FRED_CI",
                column: "EtablissementComptableId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_MontantDeviseId",
                schema: "dbo",
                table: "FRED_CI",
                column: "MontantDeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_OrganisationId",
                schema: "dbo",
                table: "FRED_CI",
                column: "OrganisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_PaysFacturationId",
                schema: "dbo",
                table: "FRED_CI",
                column: "PaysFacturationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_PaysId",
                schema: "dbo",
                table: "FRED_CI",
                column: "PaysId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_PaysLivraisonId",
                schema: "dbo",
                table: "FRED_CI",
                column: "PaysLivraisonId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_ResponsableAdministratifId",
                schema: "dbo",
                table: "FRED_CI",
                column: "ResponsableAdministratifId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_ResponsableChantierId",
                schema: "dbo",
                table: "FRED_CI",
                column: "ResponsableChantierId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_SocieteId",
                schema: "dbo",
                table: "FRED_CI",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_CODE_MAJORATION_CiId",
                schema: "dbo",
                table: "FRED_CI_CODE_MAJORATION",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_CODE_MAJORATION_CodeMajorationId",
                schema: "dbo",
                table: "FRED_CI_CODE_MAJORATION",
                column: "CodeMajorationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_DEVISE_CiId",
                schema: "dbo",
                table: "FRED_CI_DEVISE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_DEVISE_DeviseId",
                schema: "dbo",
                table: "FRED_CI_DEVISE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_PRIME_AuteurCreationId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_PRIME_AuteurModificationId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_PRIME_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_PRIME_CiId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_PRIME_PrimeId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "PrimeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_RESSOURCE_CiId",
                schema: "dbo",
                table: "FRED_CI_RESSOURCE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CI_RESSOURCE_RessourceId",
                schema: "dbo",
                table: "FRED_CI_RESSOURCE",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_CI_TYPE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_ABSENCE_CodeAbsenceParentId",
                schema: "dbo",
                table: "FRED_CODE_ABSENCE",
                column: "CodeAbsenceParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_ABSENCE_GroupeId",
                schema: "dbo",
                table: "FRED_CODE_ABSENCE",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_ABSENCE_HoldingId",
                schema: "dbo",
                table: "FRED_CODE_ABSENCE",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_ABSENCE_SocieteId",
                schema: "dbo",
                table: "FRED_CODE_ABSENCE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndGroupe",
                schema: "dbo",
                table: "FRED_CODE_ASTREINTE",
                columns: new[] { "Code", "GroupeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_DEPLACEMENT_SocieteId",
                schema: "dbo",
                table: "FRED_CODE_DEPLACEMENT",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_CODE_DEPLACEMENT",
                columns: new[] { "Code", "SocieteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_MAJORATION_AuteurCreationId",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_MAJORATION_AuteurModificationId",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_MAJORATION_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_MAJORATION_GroupeId",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndGroupe",
                schema: "dbo",
                table: "FRED_CODE_MAJORATION",
                columns: new[] { "Code", "GroupeId" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_ZONE_DEPLACEMENT_AuteurCreation",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT",
                column: "AuteurCreation");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_ZONE_DEPLACEMENT_AuteurModification",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT",
                column: "AuteurModification");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CODE_ZONE_DEPLACEMENT_SocieteId",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT",
                columns: new[] { "Code", "SocieteId" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_CiId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_ContactId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_DeviseId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_FacturationPaysId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "FacturationPaysId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_FournisseurId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_FournisseurPaysId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "FournisseurPaysId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LivraisonPaysId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "LivraisonPaysId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_Numero",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_OldFournisseurId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "OldFournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_StatutCommandeId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "StatutCommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_SuiviId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "SuiviId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_SystemeExterneId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "SystemeExterneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_TypeEnergieId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "TypeEnergieId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_TypeId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_ValideurId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "ValideurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_AVENANT_AuteurValidationId",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT",
                column: "AuteurValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_AVENANT_CommandeId",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_CONTRAT_INTERIMAIRE_CiId",
                schema: "dbo",
                table: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_CONTRAT_INTERIMAIRE_CommandeId",
                schema: "dbo",
                table: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                column: "CommandeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_CONTRAT_INTERIMAIRE_ContratId",
                schema: "dbo",
                table: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                column: "ContratId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_CONTRAT_INTERIMAIRE_InterimaireId",
                schema: "dbo",
                table: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                column: "InterimaireId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_CONTRAT_INTERIMAIRE_RapportLigneId",
                schema: "dbo",
                table: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                column: "RapportLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_AvenantLigneId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "AvenantLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_CommandeId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_MaterielId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "MaterielId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_PersonnelId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_RessourceId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_TacheId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_UniteId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_COMMANDE_LIGNE_AVENANT_AvenantId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE_AVENANT",
                column: "AvenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_COMMANDE_TYPE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_CiId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_FournisseurId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_InterimaireId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "InterimaireId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_MotifRemplacementId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "MotifRemplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_PersonnelRemplaceId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "PersonnelRemplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_RessourceId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_SocieteId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTRAT_INTERIMAIRE_UniteId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTROLE_BUDGETAIRE_ControleBudgetaireEtatId",
                schema: "dbo",
                table: "FRED_CONTROLE_BUDGETAIRE",
                column: "ControleBudgetaireEtatId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTROLE_BUDGETAIRE_VALEURS_RessourceId",
                schema: "dbo",
                table: "FRED_CONTROLE_BUDGETAIRE_VALEURS",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTROLE_BUDGETAIRE_VALEURS_TacheId",
                schema: "dbo",
                table: "FRED_CONTROLE_BUDGETAIRE_VALEURS",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTROLE_POINTAGE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_CONTROLE_POINTAGE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTROLE_POINTAGE_LotPointageId",
                schema: "dbo",
                table: "FRED_CONTROLE_POINTAGE",
                column: "LotPointageId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTROLE_POINTAGE_ERREUR_ControlePointageId",
                schema: "dbo",
                table: "FRED_CONTROLE_POINTAGE_ERREUR",
                column: "ControlePointageId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_CONTROLE_POINTAGE_ERREUR_PersonnelId",
                schema: "dbo",
                table: "FRED_CONTROLE_POINTAGE_ERREUR",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DATES_CALENDRIER_PAIE_SocieteId",
                schema: "dbo",
                table: "FRED_DATES_CALENDRIER_PAIE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DATES_CLOTURE_COMPTABLE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_DATES_CLOTURE_COMPTABLE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DATES_CLOTURE_COMPTABLE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_DATES_CLOTURE_COMPTABLE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DATES_CLOTURE_COMPTABLE_CiId",
                schema: "dbo",
                table: "FRED_DATES_CLOTURE_COMPTABLE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DELEGATION_PersonnelAuteurId",
                schema: "dbo",
                table: "FRED_DELEGATION",
                column: "PersonnelAuteurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DELEGATION_PersonnelDelegantId",
                schema: "dbo",
                table: "FRED_DELEGATION",
                column: "PersonnelDelegantId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DELEGATION_PersonnelDelegueId",
                schema: "dbo",
                table: "FRED_DELEGATION",
                column: "PersonnelDelegueId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_AuteurCreationId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_AuteurModificationId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_AuteurVisaReceptionId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "AuteurVisaReceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_CiId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_CommandeLigneId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "CommandeLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_DepenseParentId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "DepenseParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_DepenseTypeId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "DepenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_DeviseId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_FactureLigneEntLigneFactureId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "FactureLigneEntLigneFactureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_FournisseurId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_GroupeRemplacementTacheId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "GroupeRemplacementTacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_RessourceId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_TacheId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_ACHAT_UniteId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_CiId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_CommandeLigneId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "CommandeLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_DepenseOrigineId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "DepenseOrigineId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_DepenseParentId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "DepenseParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_DeviseId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_FactureId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "FactureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_FactureLigneId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "FactureLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_FournisseurId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_RessourceId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_TacheId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_DEPENSE_TEMPORAIRE_UniteId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_DEPENSE_TYPE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueIsoCode",
                schema: "dbo",
                table: "FRED_DEVISE",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ECRITURE_COMPTABLE_CiId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ECRITURE_COMPTABLE_CommandeId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ECRITURE_COMPTABLE_DeviseId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ECRITURE_COMPTABLE_FamilleOperationDiverseId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "FamilleOperationDiverseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ECRITURE_COMPTABLE_JournalId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ECRITURE_COMPTABLE_CUMUL_EcritureComptableId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE_CUMUL",
                column: "EcritureComptableId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_EMAIL_SUBSCRIPTION_PersonnelId",
                schema: "dbo",
                table: "FRED_EMAIL_SUBSCRIPTION",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_EQUIPE_ProprietaireId",
                schema: "dbo",
                table: "FRED_EQUIPE",
                column: "ProprietaireId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_EQUIPE_PERSONNEL_EquipePersoId",
                schema: "dbo",
                table: "FRED_EQUIPE_PERSONNEL",
                column: "EquipePersoId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_EQUIPE_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_EQUIPE_PERSONNEL",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_OrganisationId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                column: "OrganisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_PaysId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                column: "PaysId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_COMPTABLE_SocieteId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_COMPTABLE",
                columns: new[] { "Code", "SocieteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_PAIE_AgenceRattachementId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_PAIE",
                column: "AgenceRattachementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_PAIE_EtablissementComptableId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_PAIE",
                column: "EtablissementComptableId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_PAIE_PaysId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_PAIE",
                column: "PaysId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ETABLISSEMENT_PAIE_SocieteId",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_PAIE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_ETABLISSEMENT_PAIE",
                columns: new[] { "Code", "SocieteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURATION_CiId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURATION_CommandeId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURATION_DepenseAchatAjustementId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "DepenseAchatAjustementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURATION_DepenseAchatFactureEcartId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "DepenseAchatFactureEcartId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURATION_DepenseAchatFactureId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "DepenseAchatFactureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURATION_DepenseAchatFarId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "DepenseAchatFarId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURATION_DepenseAchatReceptionId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "DepenseAchatReceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURATION_DeviseId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURATION_FacturationTypeId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "FacturationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_FACTURATION_TYPE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_AuteurClotureId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "AuteurClotureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_AuteurRapprochementId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "AuteurRapprochementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_DeviseId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_EtablissementId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "EtablissementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_FournisseurId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_JournalId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_SocieteId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_UtilisateurCreationId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "UtilisateurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_UtilisateurModificationId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "UtilisateurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_UtilisateurSupressionId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "UtilisateurSupressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_LIGNE_AffaireId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "AffaireId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_LIGNE_FactureId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "FactureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_LIGNE_NatureId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "NatureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_LIGNE_UtilisateurCreationId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "UtilisateurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_LIGNE_UtilisateurModificationId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "UtilisateurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FACTURE_LIGNE_UtilisateurSuppressionId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "UtilisateurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FAMILLE_OPERATION_DIVERSE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FAMILLE_OPERATION_DIVERSE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FAMILLE_OPERATION_DIVERSE_SocieteId",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_FAMILLE_OPERATION_DIVERSE",
                columns: new[] { "Code", "SocieteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FAVORI_UTILISATEUR_UtilisateurId",
                schema: "dbo",
                table: "FRED_FAVORI_UTILISATEUR",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueFeatureCode",
                schema: "dbo",
                table: "FRED_FEATURE_FLIPPING",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueFeatureName",
                schema: "dbo",
                table: "FRED_FEATURE_FLIPPING",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_FONCTIONNALITE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FONCTIONNALITE_ModuleId",
                schema: "dbo",
                table: "FRED_FONCTIONNALITE",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FONCTIONNALITE_DESACTIVE_FonctionnaliteId",
                schema: "dbo",
                table: "FRED_FONCTIONNALITE_DESACTIVE",
                column: "FonctionnaliteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FONCTIONNALITE_DESACTIVE_SocieteId",
                schema: "dbo",
                table: "FRED_FONCTIONNALITE_DESACTIVE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FOURNISSEUR_GroupeId",
                schema: "dbo",
                table: "FRED_FOURNISSEUR",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_FOURNISSEUR_PaysId",
                schema: "dbo",
                table: "FRED_FOURNISSEUR",
                column: "PaysId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_GROUPE_OrganisationId",
                schema: "dbo",
                table: "FRED_GROUPE",
                column: "OrganisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_GROUPE_PoleId",
                schema: "dbo",
                table: "FRED_GROUPE",
                column: "PoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndPole",
                schema: "dbo",
                table: "FRED_GROUPE",
                columns: new[] { "Code", "PoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_HOLDING",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_HOLDING_OrganisationId",
                schema: "dbo",
                table: "FRED_HOLDING",
                column: "OrganisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_INDEMNITE_DEPLACEMENT_AuteurCreation",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "AuteurCreation");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_INDEMNITE_DEPLACEMENT_AuteurModification",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "AuteurModification");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_INDEMNITE_DEPLACEMENT_AuteurSuppression",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "AuteurSuppression");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_INDEMNITE_DEPLACEMENT_CiId",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_INDEMNITE_DEPLACEMENT_CodeDeplacementId",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "CodeDeplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_INDEMNITE_DEPLACEMENT_CodeZoneDeplacementId",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "CodeZoneDeplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_INDEMNITE_DEPLACEMENT_PersonnelId",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_JOURNAL_AuteurClotureId",
                schema: "dbo",
                table: "FRED_JOURNAL",
                column: "AuteurClotureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_JOURNAL_AuteurCreationId",
                schema: "dbo",
                table: "FRED_JOURNAL",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_JOURNAL_AuteurModificationId",
                schema: "dbo",
                table: "FRED_JOURNAL",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_JOURNAL_SocieteId",
                schema: "dbo",
                table: "FRED_JOURNAL",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_JOURNAL",
                columns: new[] { "Code", "SocieteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_LOT_FAR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_LOT_FAR",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_LOT_POINTAGE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_LOT_POINTAGE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_LOT_POINTAGE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_LOT_POINTAGE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_LOT_POINTAGE_AuteurVisaId",
                schema: "dbo",
                table: "FRED_LOT_POINTAGE",
                column: "AuteurVisaId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_AuteurCreationId",
                schema: "dbo",
                table: "FRED_MATERIEL",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_AuteurModificationId",
                schema: "dbo",
                table: "FRED_MATERIEL",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_MATERIEL",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_EtablissementComptableId",
                schema: "dbo",
                table: "FRED_MATERIEL",
                column: "EtablissementComptableId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_FournisseurId",
                schema: "dbo",
                table: "FRED_MATERIEL",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_RessourceId",
                schema: "dbo",
                table: "FRED_MATERIEL",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_SiteAppartenanceId",
                schema: "dbo",
                table: "FRED_MATERIEL",
                column: "SiteAppartenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_SocieteId",
                schema: "dbo",
                table: "FRED_MATERIEL",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_MATERIEL",
                columns: new[] { "Code", "SocieteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_LOCATION_AuteurCreationId",
                schema: "dbo",
                table: "FRED_MATERIEL_LOCATION",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_LOCATION_AuteurModificationId",
                schema: "dbo",
                table: "FRED_MATERIEL_LOCATION",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_LOCATION_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_MATERIEL_LOCATION",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MATERIEL_LOCATION_MaterielId",
                schema: "dbo",
                table: "FRED_MATERIEL_LOCATION",
                column: "MaterielId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueMatriculeAndSource",
                schema: "dbo",
                table: "FRED_MATRICULE_EXTERNE",
                columns: new[] { "Matricule", "Source" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniquePersonnelIdAndSource",
                schema: "dbo",
                table: "FRED_MATRICULE_EXTERNE",
                columns: new[] { "PersonnelId", "Source" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_MODULE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MODULE_DESACTIVE_ModuleId",
                schema: "dbo",
                table: "FRED_MODULE_DESACTIVE",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_MODULE_DESACTIVE_SocieteId",
                schema: "dbo",
                table: "FRED_MODULE_DESACTIVE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_MOTIF_REMPLACEMENT",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_NATURE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_NATURE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_NATURE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_NATURE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_NATURE_SocieteId",
                schema: "dbo",
                table: "FRED_NATURE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_NATURE",
                columns: new[] { "Code", "SocieteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_AuteurCreationId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_AuteurModificationId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_CiId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_ObjectifFlashId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE",
                column: "ObjectifFlashId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_UniteId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_JOURNALISATION_ObjectifFlashTacheId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE_JOURNALISATION",
                column: "ObjectifFlashTacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE_ObjectifFlashTacheId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE",
                column: "ObjectifFlashTacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE_RapportId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE",
                column: "RapportId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_ObjectifFlashTacheId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE_RESSOURCE",
                column: "ObjectifFlashTacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_RessourceId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE_RESSOURCE",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_UniteId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE_RESSOURCE",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_JOURNALISATION_ObjectifFlashTacheRessourceId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_JOURNALISATION",
                column: "ObjectifFlashTacheRessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_CiId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_DeviseId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_EcritureComptableId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "EcritureComptableId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_FamilleOperationDiverseId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "FamilleOperationDiverseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_GroupeRemplacementTacheId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "GroupeRemplacementTacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_RessourceId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_TacheId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_OPERATION_DIVERSE_UniteId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGA_LIENS_EtablissementComptableId",
                schema: "dbo",
                table: "FRED_ORGA_LIENS",
                column: "EtablissementComptableId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGA_LIENS_OrganisationId",
                schema: "dbo",
                table: "FRED_ORGA_LIENS",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGA_LIENS_SocieteId",
                schema: "dbo",
                table: "FRED_ORGA_LIENS",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_PereId",
                schema: "dbo",
                table: "FRED_ORGANISATION",
                column: "PereId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_TypeOrganisationId",
                schema: "dbo",
                table: "FRED_ORGANISATION",
                column: "TypeOrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_AuteurCreationId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_AuteurModificationId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_DeviseId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_OrganisationId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "ReferentielEtenduId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_UniteId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_ORGANISATION_GENERIQUE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ORGANISATION_GENERIQUE_OrganisationId",
                schema: "dbo",
                table: "FRED_ORGANISATION_GENERIQUE",
                column: "OrganisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PARAM_KEY_AuteurCreationId",
                schema: "dbo",
                table: "FRED_PARAM_KEY",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PARAM_KEY_AuteurModificationId",
                schema: "dbo",
                table: "FRED_PARAM_KEY",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PARAM_VALUE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_PARAM_VALUE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PARAM_VALUE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_PARAM_VALUE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PARAM_VALUE_OrganisationId",
                schema: "dbo",
                table: "FRED_PARAM_VALUE",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PARAM_VALUE_ParamKeyId",
                schema: "dbo",
                table: "FRED_PARAM_VALUE",
                column: "ParamKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PARAMETRE_GroupeId",
                schema: "dbo",
                table: "FRED_PARAMETRE",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_PAYS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_PERMISSION",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERMISSION_FONCTIONNALITE_FonctionnaliteId",
                schema: "dbo",
                table: "FRED_PERMISSION_FONCTIONNALITE",
                column: "FonctionnaliteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERMISSION_FONCTIONNALITE_PermissionId",
                schema: "dbo",
                table: "FRED_PERMISSION_FONCTIONNALITE",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueEmail",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL AND [IsInterne]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_EquipeFavoriteId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "EquipeFavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_EtablissementPayeId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "EtablissementPayeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_EtablissementRattachementId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "EtablissementRattachementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_ManagerId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_MaterielId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "MaterielId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_PaysId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "PaysId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_PersonnelImageId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "PersonnelImageId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_RessourceId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PERSONNEL_SocieteId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueMatriculeAndSociete",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                columns: new[] { "Matricule", "SocieteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_COMMANDE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_COMMANDE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_COMMANDE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_COMMANDE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_COMMANDE_CommandeId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_COMMANDE",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_COMMANDE_PieceJointeId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_COMMANDE",
                column: "PieceJointeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_RECEPTION_AuteurCreationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_RECEPTION",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_RECEPTION_AuteurModificationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_RECEPTION",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_RECEPTION_PieceJointeId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_RECEPTION",
                column: "PieceJointeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PIECE_JOINTE_RECEPTION_ReceptionId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_RECEPTION",
                column: "ReceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_CiId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_CodeAbsenceId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "CodeAbsenceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_CodeDeplacementId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "CodeDeplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_CodeMajorationId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "CodeMajorationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_CodeZoneDeplacementId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "CodeZoneDeplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_PersonnelId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_PRIME_PointageAnticipeId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE_PRIME",
                column: "PointageAnticipeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POINTAGE_ANTICIPE_PRIME_PrimeId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE_PRIME",
                column: "PrimeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POLE_HoldingId",
                schema: "dbo",
                table: "FRED_POLE",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_POLE_OrganisationId",
                schema: "dbo",
                table: "FRED_POLE",
                column: "OrganisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndHolding",
                schema: "dbo",
                table: "FRED_POLE",
                columns: new[] { "Code", "HoldingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PRIME_AuteurCreationId",
                schema: "dbo",
                table: "FRED_PRIME",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PRIME_AuteurModificationId",
                schema: "dbo",
                table: "FRED_PRIME",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PRIME_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_PRIME",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PRIME_GroupeId",
                schema: "dbo",
                table: "FRED_PRIME",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_PRIME_SocieteId",
                schema: "dbo",
                table: "FRED_PRIME",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSociete",
                schema: "dbo",
                table: "FRED_PRIME",
                columns: new[] { "Code", "SocieteId" },
                unique: true,
                filter: "[Code] IS NOT NULL AND [SocieteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_AuteurCreationId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_AuteurModificationId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_AuteurVerrouId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "AuteurVerrouId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_CiId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_RapportStatutId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "RapportStatutId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_ValideurCDCId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "ValideurCDCId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_ValideurCDTId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "ValideurCDTId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_ValideurDRCId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "ValideurDRCId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_AffectationMoyenId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AffectationMoyenId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_CiId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_CodeAbsenceId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "CodeAbsenceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_CodeDeplacementId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "CodeDeplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_CodeMajorationId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "CodeMajorationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_CodeZoneDeplacementId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "CodeZoneDeplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_LotPointageId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "LotPointageId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_MaterielId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "MaterielId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_PersonnelId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_RapportId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "RapportId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_RapportLigneStatutId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "RapportLigneStatutId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_ValideurId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "ValideurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_ASTREINTE_AstreinteId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_ASTREINTE",
                column: "AstreinteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_ASTREINTE_RapportLigneId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_ASTREINTE",
                column: "RapportLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_CODE_ASTREINTE_CodeAstreinteId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_CODE_ASTREINTE",
                column: "CodeAstreinteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_CODE_ASTREINTE_RapportLigneAstreinteId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_CODE_ASTREINTE",
                column: "RapportLigneAstreinteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_CODE_ASTREINTE_RapportLigneId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_CODE_ASTREINTE",
                column: "RapportLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_MAJORATION_CodeMajorationId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_MAJORATION",
                column: "CodeMajorationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_MAJORATION_RapportLigneId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_MAJORATION",
                column: "RapportLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_PRIME_PrimeId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_PRIME",
                column: "PrimeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_PRIME_RapportLigneId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_PRIME",
                column: "RapportLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_TACHE_RapportLigneId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_TACHE",
                column: "RapportLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_LIGNE_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_TACHE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_STATUT_Code",
                schema: "dbo",
                table: "FRED_RAPPORT_STATUT",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_TACHE_RapportId",
                schema: "dbo",
                table: "FRED_RAPPORT_TACHE",
                column: "RapportId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORT_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_RAPPORT_TACHE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_AuteurCreationId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_AuteurModificationId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_SocieteId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_AuteurValidationId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_AuteurVerrouId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurVerrouId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_CiId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_PersonnelId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_RapportPrimeId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "RapportPrimeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_ASTREINTE_AstreinteId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE_ASTREINTE",
                column: "AstreinteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_ASTREINTE_RapportPrimeLigneId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE_ASTREINTE",
                column: "RapportPrimeLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_PRIME_PrimeId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE_PRIME",
                column: "PrimeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RAPPORTPRIME_LIGNE_PRIME_RapportPrimeLigneId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE_PRIME",
                column: "RapportPrimeLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REMONTEE_VRAC_AuteurCreationId",
                schema: "dbo",
                table: "FRED_REMONTEE_VRAC",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REMONTEE_VRAC_ERREUR_EtablissementPaieId",
                schema: "dbo",
                table: "FRED_REMONTEE_VRAC_ERREUR",
                column: "EtablissementPaieId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REMONTEE_VRAC_ERREUR_PersonnelId",
                schema: "dbo",
                table: "FRED_REMONTEE_VRAC_ERREUR",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REMONTEE_VRAC_ERREUR_RemonteeVracId",
                schema: "dbo",
                table: "FRED_REMONTEE_VRAC_ERREUR",
                column: "RemonteeVracId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REMONTEE_VRAC_ERREUR_SocieteId",
                schema: "dbo",
                table: "FRED_REMONTEE_VRAC_ERREUR",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REMPLACEMENT_TACHE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_REMPLACEMENT_TACHE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REMPLACEMENT_TACHE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_REMPLACEMENT_TACHE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REMPLACEMENT_TACHE_GroupeRemplacementTacheId",
                schema: "dbo",
                table: "FRED_REMPLACEMENT_TACHE",
                column: "GroupeRemplacementTacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REMPLACEMENT_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_REMPLACEMENT_TACHE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_REPARTITION_ECART_CiId",
                schema: "dbo",
                table: "FRED_REPARTITION_ECART",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_CarburantId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "CarburantId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_ParentId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_RessourceRattachementId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "RessourceRattachementId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_SousChapitreId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "SousChapitreId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_SpecifiqueCiId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "SpecifiqueCiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_TypeRessourceId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "TypeRessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndSousChapitreId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                columns: new[] { "Code", "SousChapitreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_RECOMMANDEE_ORGANISATION_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_RESSOURCE_RECOMMANDEE_ORGANISATION",
                column: "ReferentielEtenduId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueOrganisationIdAndReferentielEtenduId",
                schema: "dbo",
                table: "FRED_RESSOURCE_RECOMMANDEE_ORGANISATION",
                columns: new[] { "OrganisationId", "ReferentielEtenduId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_TACHE_RessourceId",
                schema: "dbo",
                table: "FRED_RESSOURCE_TACHE",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_RESSOURCE_TACHE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_TACHE_DEVISE_DeviseId",
                schema: "dbo",
                table: "FRED_RESSOURCE_TACHE_DEVISE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_RESSOURCE_TACHE_DEVISE_RessourceTacheId",
                schema: "dbo",
                table: "FRED_RESSOURCE_TACHE_DEVISE",
                column: "RessourceTacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ROLE_SocieteId",
                schema: "dbo",
                table: "FRED_ROLE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndGroupe",
                schema: "dbo",
                table: "FRED_ROLE",
                columns: new[] { "CodeNomFamilier", "SocieteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ROLE_DEVISE_DeviseId",
                schema: "dbo",
                table: "FRED_ROLE_DEVISE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ROLE_DEVISE_RoleId",
                schema: "dbo",
                table: "FRED_ROLE_DEVISE",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ROLE_FONCTIONNALITE_FonctionnaliteId",
                schema: "dbo",
                table: "FRED_ROLE_FONCTIONNALITE",
                column: "FonctionnaliteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ROLE_FONCTIONNALITE_RoleId",
                schema: "dbo",
                table: "FRED_ROLE_FONCTIONNALITE",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ROLE_ORGANISATION_DEVISE_DeviseId",
                schema: "dbo",
                table: "FRED_ROLE_ORGANISATION_DEVISE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ROLE_ORGANISATION_DEVISE_OrganisationId",
                schema: "dbo",
                table: "FRED_ROLE_ORGANISATION_DEVISE",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ROLE_ORGANISATION_DEVISE_RoleId",
                schema: "dbo",
                table: "FRED_ROLE_ORGANISATION_DEVISE",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_CGAFournitureId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "CGAFournitureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_CGALocationId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "CGALocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_CGAPrestationId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "CGAPrestationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_FournisseurId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_GroupeId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_ImageLoginId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "ImageLoginId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_ImageLogoId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "ImageLogoId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_IndemniteDeplacementCalculTypeId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "IndemniteDeplacementCalculTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_OrganisationId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "OrganisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_SocieteClassificationId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "SocieteClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_TypeSocieteId",
                schema: "dbo",
                table: "FRED_SOCIETE",
                column: "TypeSocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndGroupe",
                schema: "dbo",
                table: "FRED_SOCIETE",
                columns: new[] { "Code", "GroupeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_DEVISE_DeviseId",
                schema: "dbo",
                table: "FRED_SOCIETE_DEVISE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_DEVISE_SocieteId",
                schema: "dbo",
                table: "FRED_SOCIETE_DEVISE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_RESSOURCE_NATURE_NatureId",
                schema: "dbo",
                table: "FRED_SOCIETE_RESSOURCE_NATURE",
                column: "NatureId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_RESSOURCE_NATURE_RessourceId",
                schema: "dbo",
                table: "FRED_SOCIETE_RESSOURCE_NATURE",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOCIETE_RESSOURCE_NATURE_SocieteId",
                schema: "dbo",
                table: "FRED_SOCIETE_RESSOURCE_NATURE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOUS_CHAPITRE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_SOUS_CHAPITRE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOUS_CHAPITRE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_SOUS_CHAPITRE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOUS_CHAPITRE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_SOUS_CHAPITRE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SOUS_CHAPITRE_ChapitreId",
                schema: "dbo",
                table: "FRED_SOUS_CHAPITRE",
                column: "ChapitreId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeAndChapitreId",
                schema: "dbo",
                table: "FRED_SOUS_CHAPITRE",
                columns: new[] { "Code", "ChapitreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_STATUT_COMMANDE_Code",
                schema: "dbo",
                table: "FRED_STATUT_COMMANDE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SYSTEME_EXTERNE_SocieteId",
                schema: "dbo",
                table: "FRED_SYSTEME_EXTERNE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SYSTEME_EXTERNE_SystemeExterneTypeId",
                schema: "dbo",
                table: "FRED_SYSTEME_EXTERNE",
                column: "SystemeExterneTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_SYSTEME_EXTERNE_SystemeImportId",
                schema: "dbo",
                table: "FRED_SYSTEME_EXTERNE",
                column: "SystemeImportId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_AuteurCreationId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "AuteurCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_AuteurModificationId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "AuteurModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "AuteurSuppressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_BudgetId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_BudgetRevisionEntBudgetRevisionId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "BudgetRevisionEntBudgetRevisionId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_CiId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_ParentId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_UniteEntUniteId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "UniteEntUniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_RECETTE_DeviseId",
                schema: "dbo",
                table: "FRED_TACHE_RECETTE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TACHE_RECETTE_TacheId",
                schema: "dbo",
                table: "FRED_TACHE_RECETTE",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TRANSCO_IMPORT_SocieteId",
                schema: "dbo",
                table: "FRED_TRANSCO_IMPORT",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_TRANSCO_IMPORT_SystemeImportId",
                schema: "dbo",
                table: "FRED_TRANSCO_IMPORT",
                column: "SystemeImportId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeExterneAndSocieteAndSystemImport",
                schema: "dbo",
                table: "FRED_TRANSCO_IMPORT",
                columns: new[] { "CodeExterne", "SocieteId", "SystemeImportId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeInterneAndSocieteAndSystemImport",
                schema: "dbo",
                table: "FRED_TRANSCO_IMPORT",
                columns: new[] { "CodeInterne", "SocieteId", "SystemeImportId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_TYPE_ENERGIE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueTypeOrganisationCode",
                schema: "dbo",
                table: "FRED_TYPE_ORGANISATION",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_TYPE_RESSOURCE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCode",
                schema: "dbo",
                table: "FRED_UNITE",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UNITE_REFERENTIEL_ETENDU_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_UNITE_REFERENTIEL_ETENDU",
                column: "ReferentielEtenduId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UNITE_REFERENTIEL_ETENDU_UniteId",
                schema: "dbo",
                table: "FRED_UNITE_REFERENTIEL_ETENDU",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UNITE_SOCIETE_SocieteId",
                schema: "dbo",
                table: "FRED_UNITE_SOCIETE",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UNITE_SOCIETE_UniteId",
                schema: "dbo",
                table: "FRED_UNITE_SOCIETE",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_FayatAccessDirectoryId",
                schema: "dbo",
                table: "FRED_UTILISATEUR",
                column: "FayatAccessDirectoryId",
                unique: true,
                filter: "[FayatAccessDirectoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UTILISATEUR_UtilisateurIdCreation",
                schema: "dbo",
                table: "FRED_UTILISATEUR",
                column: "UtilisateurIdCreation");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UTILISATEUR_UtilisateurIdModification",
                schema: "dbo",
                table: "FRED_UTILISATEUR",
                column: "UtilisateurIdModification");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UTILISATEUR_UtilisateurIdSupression",
                schema: "dbo",
                table: "FRED_UTILISATEUR",
                column: "UtilisateurIdSupression");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_DelegationId",
                schema: "dbo",
                table: "FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE",
                column: "DelegationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_DeviseId",
                schema: "dbo",
                table: "FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_OrganisationId",
                schema: "dbo",
                table: "FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_RoleId",
                schema: "dbo",
                table: "FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_UtilisateurId",
                schema: "dbo",
                table: "FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_BaremeId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "BaremeId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_BaremeStormId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "BaremeStormId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_ChapitreId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "ChapitreId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_CiId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "CiId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_DeviseId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_GroupeRemplacementTacheId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "GroupeRemplacementTacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_MaterielId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "MaterielId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_PersonnelId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_RapportId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "RapportId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_RapportLigneId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "RapportLigneId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "ReferentielEtenduId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_SousChapitreId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "SousChapitreId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_TacheId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "TacheId");

            migrationBuilder.CreateIndex(
                name: "IX_FRED_VALORISATION_UniteId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "UniteId");

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

            migrationBuilder.CreateIndex(
                name: "IX_FRED_ZONE_DE_TRAVAIL_EtablissementComptableId",
                schema: "dbo",
                table: "FRED_ZONE_DE_TRAVAIL",
                column: "EtablissementComptableId");

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ASTREINTE_FRED_AFFECTATION_AffectationId",
                schema: "dbo",
                table: "FRED_ASTREINTE",
                column: "AffectationId",
                principalSchema: "dbo",
                principalTable: "FRED_AFFECTATION",
                principalColumn: "AffectationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_MATERIEL_MaterielId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "MaterielId",
                principalSchema: "dbo",
                principalTable: "FRED_MATERIEL",
                principalColumn: "MaterielId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_UTILISATEUR_ValideurId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "ValideurId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_CODE_MAJORATION_CodeMajorationId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "CodeMajorationId",
                principalSchema: "dbo",
                principalTable: "FRED_CODE_MAJORATION",
                principalColumn: "CodeMajorationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_LOT_POINTAGE_LotPointageId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "LotPointageId",
                principalSchema: "dbo",
                principalTable: "FRED_LOT_POINTAGE",
                principalColumn: "LotPointageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_CODE_ZONE_DEPLACEMENT_CodeZoneDeplacementId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "CodeZoneDeplacementId",
                principalSchema: "dbo",
                principalTable: "FRED_CODE_ZONE_DEPLACEMENT",
                principalColumn: "CodeZoneDeplacementId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_RAPPORT_RapportId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "RapportId",
                principalSchema: "dbo",
                principalTable: "FRED_RAPPORT",
                principalColumn: "RapportId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_FRED_AFFECTATION_MOYEN_AffectationMoyenId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE",
                column: "AffectationMoyenId",
                principalSchema: "dbo",
                principalTable: "FRED_AFFECTATION_MOYEN",
                principalColumn: "AffectationMoyenId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AFFECTATION_MOYEN_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AFFECTATION_MOYEN_FRED_PERSONNEL_ConducteurId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "ConducteurId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AFFECTATION_MOYEN_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AFFECTATION_MOYEN_FRED_MATERIEL_MaterielId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "MaterielId",
                principalSchema: "dbo",
                principalTable: "FRED_MATERIEL",
                principalColumn: "MaterielId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AFFECTATION_MOYEN_FRED_MATERIEL_LOCATION_MaterielLocationId",
                schema: "dbo",
                table: "FRED_AFFECTATION_MOYEN",
                column: "MaterielLocationId",
                principalSchema: "dbo",
                principalTable: "FRED_MATERIEL_LOCATION",
                principalColumn: "MaterielLocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORTPRIME_LIGNE_ASTREINTE_FRED_RAPPORTPRIME_LIGNE_RapportPrimeLigneId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE_ASTREINTE",
                column: "RapportPrimeLigneId",
                principalSchema: "dbo",
                principalTable: "FRED_RAPPORTPRIME_LIGNE",
                principalColumn: "RapportPrimeLigneId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AVANCEMENT_WORKFLOW_FRED_UTILISATEUR_AuteurId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_WORKFLOW",
                column: "AuteurId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AVANCEMENT_WORKFLOW_FRED_AVANCEMENT_AvancementId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_WORKFLOW",
                column: "AvancementId",
                principalSchema: "dbo",
                principalTable: "FRED_AVANCEMENT",
                principalColumn: "AvancementId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AVANCEMENT_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_AVANCEMENT",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AVANCEMENT_FRED_BUDGET_T4_RESSOURCE_BudgetSousDetailId",
                schema: "dbo",
                table: "FRED_AVANCEMENT",
                column: "BudgetSousDetailId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET_T4_RESSOURCE",
                principalColumn: "BudgetT4SousDetailId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_MATERIEL_MaterielId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "MaterielId",
                principalSchema: "dbo",
                principalTable: "FRED_MATERIEL",
                principalColumn: "MaterielId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "TacheId",
                principalSchema: "dbo",
                principalTable: "FRED_TACHE",
                principalColumn: "TacheId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_SOCIETE_RESSOURCE_NATURE_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "ReferentielEtenduId",
                principalSchema: "dbo",
                principalTable: "FRED_SOCIETE_RESSOURCE_NATURE",
                principalColumn: "ReferentielEtenduId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_RAPPORT_RapportId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "RapportId",
                principalSchema: "dbo",
                principalTable: "FRED_RAPPORT",
                principalColumn: "RapportId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_SOUS_CHAPITRE_SousChapitreId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "SousChapitreId",
                principalSchema: "dbo",
                principalTable: "FRED_SOUS_CHAPITRE",
                principalColumn: "SousChapitreId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_CHAPITRE_ChapitreId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "ChapitreId",
                principalSchema: "dbo",
                principalTable: "FRED_CHAPITRE",
                principalColumn: "ChapitreId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_BAREME_EXPLOITATION_CI_BaremeId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "BaremeId",
                principalSchema: "dbo",
                principalTable: "FRED_BAREME_EXPLOITATION_CI",
                principalColumn: "BaremeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VALORISATION_FRED_BAREME_EXPLOITATION_ORGANISATION_BaremeStormId",
                schema: "dbo",
                table: "FRED_VALORISATION",
                column: "BaremeStormId",
                principalSchema: "dbo",
                principalTable: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                principalColumn: "BaremeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AVANCEMENT_TACHE_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_TACHE",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AVANCEMENT_TACHE_FRED_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_AVANCEMENT_TACHE",
                column: "TacheId",
                principalSchema: "dbo",
                principalTable: "FRED_TACHE",
                principalColumn: "TacheId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_RECETTE_FRED_BUDGET_BudgetRecetteId",
                schema: "dbo",
                table: "FRED_BUDGET_RECETTE",
                column: "BudgetRecetteId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_REVISION_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_REVISION",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_REVISION_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BUDGET_REVISION",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_REVISION_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BUDGET_REVISION",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_REVISION_FRED_UTILISATEUR_AuteurValidationId",
                schema: "dbo",
                table: "FRED_BUDGET_REVISION",
                column: "AuteurValidationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_T4_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_T4",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_T4_FRED_TACHE_T3Id",
                schema: "dbo",
                table: "FRED_BUDGET_T4",
                column: "T3Id",
                principalSchema: "dbo",
                principalTable: "FRED_TACHE",
                principalColumn: "TacheId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_T4_FRED_TACHE_T4Id",
                schema: "dbo",
                table: "FRED_BUDGET_T4",
                column: "T4Id",
                principalSchema: "dbo",
                principalTable: "FRED_TACHE",
                principalColumn: "TacheId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_TACHE_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_TACHE",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_TACHE_FRED_TACHE_TacheId",
                schema: "dbo",
                table: "FRED_BUDGET_TACHE",
                column: "TacheId",
                principalSchema: "dbo",
                principalTable: "FRED_TACHE",
                principalColumn: "TacheId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_WORKFLOW_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_BUDGET_WORKFLOW",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_WORKFLOW_FRED_UTILISATEUR_AuteurId",
                schema: "dbo",
                table: "FRED_BUDGET_WORKFLOW",
                column: "AuteurId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTROLE_BUDGETAIRE_FRED_BUDGET_ControleBudgetaireId",
                schema: "dbo",
                table: "FRED_CONTROLE_BUDGETAIRE",
                column: "ControleBudgetaireId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_TACHE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_TACHE_FRED_BUDGET_BudgetId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_TACHE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_TACHE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_TACHE_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_TACHE",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_FRED_RESSOURCE_RessourceId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "RessourceId",
                principalSchema: "dbo",
                principalTable: "FRED_RESSOURCE",
                principalColumn: "RessourceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_FRED_BUDGET_BIBLIOTHEQUE_PRIX_BudgetBibliothequePrixId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                column: "BudgetBibliothequePrixId",
                principalSchema: "dbo",
                principalTable: "FRED_BUDGET_BIBLIOTHEQUE_PRIX",
                principalColumn: "BudgetBibliothequePrixId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_BUDGET",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_T4_RESSOURCE_FRED_RESSOURCE_RessourceId",
                schema: "dbo",
                table: "FRED_BUDGET_T4_RESSOURCE",
                column: "RessourceId",
                principalSchema: "dbo",
                principalTable: "FRED_RESSOURCE",
                principalColumn: "RessourceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CARBURANT_ORGANISATION_DEVISE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_CARBURANT_ORGANISATION_DEVISE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CARBURANT_ORGANISATION_DEVISE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_CARBURANT_ORGANISATION_DEVISE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CARBURANT_ORGANISATION_DEVISE_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_CARBURANT_ORGANISATION_DEVISE",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RESSOURCE_FRED_CI_SpecifiqueCiId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "SpecifiqueCiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RESSOURCE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RESSOURCE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RESSOURCE_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RESSOURCE_FRED_SOUS_CHAPITRE_SousChapitreId",
                schema: "dbo",
                table: "FRED_RESSOURCE",
                column: "SousChapitreId",
                principalSchema: "dbo",
                principalTable: "FRED_SOUS_CHAPITRE",
                principalColumn: "SousChapitreId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_SOUS_CHAPITRE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_SOUS_CHAPITRE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_SOUS_CHAPITRE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_SOUS_CHAPITRE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_SOUS_CHAPITRE_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_SOUS_CHAPITRE",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_SOUS_CHAPITRE_FRED_CHAPITRE_ChapitreId",
                schema: "dbo",
                table: "FRED_SOUS_CHAPITRE",
                column: "ChapitreId",
                principalSchema: "dbo",
                principalTable: "FRED_CHAPITRE",
                principalColumn: "ChapitreId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AFFECTATION_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_AFFECTATION",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_AFFECTATION_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_AFFECTATION",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_FRED_CI_CIId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "CIId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_FRED_SOCIETE_RESSOURCE_NATURE_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI",
                column: "ReferentielEtenduId",
                principalSchema: "dbo",
                principalTable: "FRED_SOCIETE_RESSOURCE_NATURE",
                principalColumn: "ReferentielEtenduId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_FRED_CI_CIId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "CIId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_FRED_MATERIEL_MaterielId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "MaterielId",
                principalSchema: "dbo",
                principalTable: "FRED_MATERIEL",
                principalColumn: "MaterielId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_CI_SURCHARGE_FRED_SOCIETE_RESSOURCE_NATURE_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                column: "ReferentielEtenduId",
                principalSchema: "dbo",
                principalTable: "FRED_SOCIETE_RESSOURCE_NATURE",
                principalColumn: "ReferentielEtenduId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_CODE_MAJORATION_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_CI_CODE_MAJORATION",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_CODE_MAJORATION_FRED_CODE_MAJORATION_CodeMajorationId",
                schema: "dbo",
                table: "FRED_CI_CODE_MAJORATION",
                column: "CodeMajorationId",
                principalSchema: "dbo",
                principalTable: "FRED_CODE_MAJORATION",
                principalColumn: "CodeMajorationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_DEVISE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_CI_DEVISE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_PRIME_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_PRIME_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_PRIME_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_PRIME_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_PRIME_FRED_PRIME_PrimeId",
                schema: "dbo",
                table: "FRED_CI_PRIME",
                column: "PrimeId",
                principalSchema: "dbo",
                principalTable: "FRED_PRIME",
                principalColumn: "PrimeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_RESSOURCE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_CI_RESSOURCE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_FRED_PERSONNEL_ContactId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "ContactId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_FRED_PERSONNEL_SuiviId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "SuiviId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_FRED_UTILISATEUR_ValideurId",
                schema: "dbo",
                table: "FRED_COMMANDE",
                column: "ValideurId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_CONTRAT_INTERIMAIRE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_CONTRAT_INTERIMAIRE_FRED_PERSONNEL_InterimaireId",
                schema: "dbo",
                table: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                column: "InterimaireId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_CONTRAT_INTERIMAIRE_FRED_CONTRAT_INTERIMAIRE_ContratId",
                schema: "dbo",
                table: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                column: "ContratId",
                principalSchema: "dbo",
                principalTable: "FRED_CONTRAT_INTERIMAIRE",
                principalColumn: "PersonnelFournisseurSocieteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_PERSONNEL_InterimaireId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "InterimaireId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTRAT_INTERIMAIRE_FRED_PERSONNEL_PersonnelRemplaceId",
                schema: "dbo",
                table: "FRED_CONTRAT_INTERIMAIRE",
                column: "PersonnelRemplaceId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DATES_CLOTURE_COMPTABLE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_DATES_CLOTURE_COMPTABLE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DATES_CLOTURE_COMPTABLE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_DATES_CLOTURE_COMPTABLE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DATES_CLOTURE_COMPTABLE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_DATES_CLOTURE_COMPTABLE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_ACHAT_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_ACHAT_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_ACHAT_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_ACHAT_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_ACHAT_FRED_UTILISATEUR_AuteurVisaReceptionId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "AuteurVisaReceptionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_ACHAT_FRED_COMMANDE_LIGNE_CommandeLigneId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "CommandeLigneId",
                principalSchema: "dbo",
                principalTable: "FRED_COMMANDE_LIGNE",
                principalColumn: "CommandeLigneId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_ACHAT_FRED_FACTURE_LIGNE_FactureLigneEntLigneFactureId",
                schema: "dbo",
                table: "FRED_DEPENSE_ACHAT",
                column: "FactureLigneEntLigneFactureId",
                principalSchema: "dbo",
                principalTable: "FRED_FACTURE_LIGNE",
                principalColumn: "LigneFactureId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_COMMANDE_LIGNE_CommandeLigneId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "CommandeLigneId",
                principalSchema: "dbo",
                principalTable: "FRED_COMMANDE_LIGNE",
                principalColumn: "CommandeLigneId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_FACTURE_LIGNE_FactureLigneId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "FactureLigneId",
                principalSchema: "dbo",
                principalTable: "FRED_FACTURE_LIGNE",
                principalColumn: "LigneFactureId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_DEPENSE_TEMPORAIRE_FRED_FACTURE_FactureId",
                schema: "dbo",
                table: "FRED_DEPENSE_TEMPORAIRE",
                column: "FactureId",
                principalSchema: "dbo",
                principalTable: "FRED_FACTURE",
                principalColumn: "FactureId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ECRITURE_COMPTABLE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ECRITURE_COMPTABLE_FRED_FAMILLE_OPERATION_DIVERSE_FamilleOperationDiverseId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "FamilleOperationDiverseId",
                principalSchema: "dbo",
                principalTable: "FRED_FAMILLE_OPERATION_DIVERSE",
                principalColumn: "FamilleOperationDiverseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ECRITURE_COMPTABLE_FRED_JOURNAL_JournalId",
                schema: "dbo",
                table: "FRED_ECRITURE_COMPTABLE",
                column: "JournalId",
                principalSchema: "dbo",
                principalTable: "FRED_JOURNAL",
                principalColumn: "JournalId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURATION_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_FACTURATION",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_LIGNE_FRED_CI_AffaireId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "AffaireId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_LIGNE_FRED_UTILISATEUR_UtilisateurCreationId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "UtilisateurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_LIGNE_FRED_UTILISATEUR_UtilisateurModificationId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "UtilisateurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_LIGNE_FRED_UTILISATEUR_UtilisateurSuppressionId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "UtilisateurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_LIGNE_FRED_FACTURE_FactureId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "FactureId",
                principalSchema: "dbo",
                principalTable: "FRED_FACTURE",
                principalColumn: "FactureId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_LIGNE_FRED_NATURE_NatureId",
                schema: "dbo",
                table: "FRED_FACTURE_LIGNE",
                column: "NatureId",
                principalSchema: "dbo",
                principalTable: "FRED_NATURE",
                principalColumn: "NatureId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_INDEMNITE_DEPLACEMENT_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_INDEMNITE_DEPLACEMENT_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_INDEMNITE_DEPLACEMENT_FRED_UTILISATEUR_AuteurCreation",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "AuteurCreation",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_INDEMNITE_DEPLACEMENT_FRED_UTILISATEUR_AuteurModification",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "AuteurModification",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_INDEMNITE_DEPLACEMENT_FRED_UTILISATEUR_AuteurSuppression",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "AuteurSuppression",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_INDEMNITE_DEPLACEMENT_FRED_CODE_ZONE_DEPLACEMENT_CodeZoneDeplacementId",
                schema: "dbo",
                table: "FRED_INDEMNITE_DEPLACEMENT",
                column: "CodeZoneDeplacementId",
                principalSchema: "dbo",
                principalTable: "FRED_CODE_ZONE_DEPLACEMENT",
                principalColumn: "CodeZoneDeplacementId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_OBJECTIF_FLASH_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_OBJECTIF_FLASH_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_OBJECTIF_FLASH_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_OBJECTIF_FLASH_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_OBJECTIF_FLASH",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_OPERATION_DIVERSE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_OPERATION_DIVERSE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_OPERATION_DIVERSE_FRED_FAMILLE_OPERATION_DIVERSE_FamilleOperationDiverseId",
                schema: "dbo",
                table: "FRED_OPERATION_DIVERSE",
                column: "FamilleOperationDiverseId",
                principalSchema: "dbo",
                principalTable: "FRED_FAMILLE_OPERATION_DIVERSE",
                principalColumn: "FamilleOperationDiverseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_POINTAGE_ANTICIPE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_POINTAGE_ANTICIPE_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_POINTAGE_ANTICIPE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_POINTAGE_ANTICIPE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_POINTAGE_ANTICIPE_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_POINTAGE_ANTICIPE_FRED_CODE_MAJORATION_CodeMajorationId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "CodeMajorationId",
                principalSchema: "dbo",
                principalTable: "FRED_CODE_MAJORATION",
                principalColumn: "CodeMajorationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_POINTAGE_ANTICIPE_FRED_CODE_ZONE_DEPLACEMENT_CodeZoneDeplacementId",
                schema: "dbo",
                table: "FRED_POINTAGE_ANTICIPE",
                column: "CodeZoneDeplacementId",
                principalSchema: "dbo",
                principalTable: "FRED_CODE_ZONE_DEPLACEMENT",
                principalColumn: "CodeZoneDeplacementId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_FRED_UTILISATEUR_AuteurVerrouId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "AuteurVerrouId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_FRED_UTILISATEUR_ValideurCDCId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "ValideurCDCId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_FRED_UTILISATEUR_ValideurCDTId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "ValideurCDTId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_FRED_UTILISATEUR_ValideurDRCId",
                schema: "dbo",
                table: "FRED_RAPPORT",
                column: "ValideurDRCId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORTPRIME_LIGNE_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORTPRIME_LIGNE_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORTPRIME_LIGNE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORTPRIME_LIGNE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORTPRIME_LIGNE_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORTPRIME_LIGNE_FRED_UTILISATEUR_AuteurValidationId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurValidationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORTPRIME_LIGNE_FRED_UTILISATEUR_AuteurVerrouId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "AuteurVerrouId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORTPRIME_LIGNE_FRED_RAPPORTPRIME_RapportPrimeId",
                schema: "dbo",
                table: "FRED_RAPPORTPRIME_LIGNE",
                column: "RapportPrimeId",
                principalSchema: "dbo",
                principalTable: "FRED_RAPPORTPRIME",
                principalColumn: "RapportPrimeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_REPARTITION_ECART_FRED_CI_CiId",
                schema: "dbo",
                table: "FRED_REPARTITION_ECART",
                column: "CiId",
                principalSchema: "dbo",
                principalTable: "FRED_CI",
                principalColumn: "CiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_FRED_PERSONNEL_ResponsableAdministratifId",
                schema: "dbo",
                table: "FRED_CI",
                column: "ResponsableAdministratifId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_FRED_PERSONNEL_ResponsableChantierId",
                schema: "dbo",
                table: "FRED_CI",
                column: "ResponsableChantierId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CI_FRED_ETABLISSEMENT_COMPTABLE_EtablissementComptableId",
                schema: "dbo",
                table: "FRED_CI",
                column: "EtablissementComptableId",
                principalSchema: "dbo",
                principalTable: "FRED_ETABLISSEMENT_COMPTABLE",
                principalColumn: "EtablissementComptableId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_RAPPORT_LIGNE_MAJORATION_FRED_CODE_MAJORATION_CodeMajorationId",
                schema: "dbo",
                table: "FRED_RAPPORT_LIGNE_MAJORATION",
                column: "CodeMajorationId",
                principalSchema: "dbo",
                principalTable: "FRED_CODE_MAJORATION",
                principalColumn: "CodeMajorationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_AVENANT_FRED_UTILISATEUR_AuteurValidationId",
                schema: "dbo",
                table: "FRED_COMMANDE_AVENANT",
                column: "AuteurValidationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_LIGNE_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_LIGNE_FRED_MATERIEL_MaterielId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "MaterielId",
                principalSchema: "dbo",
                principalTable: "FRED_MATERIEL",
                principalColumn: "MaterielId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_LIGNE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_COMMANDE_LIGNE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_COMMANDE_LIGNE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PIECE_JOINTE_COMMANDE_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_COMMANDE",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PIECE_JOINTE_COMMANDE_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_COMMANDE",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PIECE_JOINTE_COMMANDE_FRED_PIECE_JOINTE_PieceJointeId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_COMMANDE",
                column: "PieceJointeId",
                principalSchema: "dbo",
                principalTable: "FRED_PIECE_JOINTE",
                principalColumn: "PieceJointeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_VISA_FRED_UTILISATEUR_UtilisateurId",
                schema: "dbo",
                table: "FRED_VISA",
                column: "UtilisateurId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ZONE_DE_TRAVAIL_FRED_ETABLISSEMENT_COMPTABLE_EtablissementComptableId",
                schema: "dbo",
                table: "FRED_ZONE_DE_TRAVAIL",
                column: "EtablissementComptableId",
                principalSchema: "dbo",
                principalTable: "FRED_ETABLISSEMENT_COMPTABLE",
                principalColumn: "EtablissementComptableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTROLE_POINTAGE_ERREUR_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_CONTROLE_POINTAGE_ERREUR",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_CONTROLE_POINTAGE_ERREUR_FRED_CONTROLE_POINTAGE_ControlePointageId",
                schema: "dbo",
                table: "FRED_CONTROLE_POINTAGE_ERREUR",
                column: "ControlePointageId",
                principalSchema: "dbo",
                principalTable: "FRED_CONTROLE_POINTAGE",
                principalColumn: "ControlePointageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_FRED_UTILISATEUR_UtilisateurId",
                schema: "dbo",
                table: "FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE",
                column: "UtilisateurId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_FRED_DELEGATION_DelegationId",
                schema: "dbo",
                table: "FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE",
                column: "DelegationId",
                principalSchema: "dbo",
                principalTable: "FRED_DELEGATION",
                principalColumn: "DelegationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PIECE_JOINTE_RECEPTION_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_RECEPTION",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PIECE_JOINTE_RECEPTION_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_RECEPTION",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PIECE_JOINTE_RECEPTION_FRED_PIECE_JOINTE_PieceJointeId",
                schema: "dbo",
                table: "FRED_PIECE_JOINTE_RECEPTION",
                column: "PieceJointeId",
                principalSchema: "dbo",
                principalTable: "FRED_PIECE_JOINTE",
                principalColumn: "PieceJointeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_ORGANISATION_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BAREME_EXPLOITATION_ORGANISATION_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_BUDGET_BIBLIOTHEQUE_PRIX_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_BUDGET_BIBLIOTHEQUE_PRIX",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_FRED_UTILISATEUR_AuteurClotureId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "AuteurClotureId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_FRED_UTILISATEUR_AuteurRapprochementId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "AuteurRapprochementId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_FRED_UTILISATEUR_UtilisateurCreationId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "UtilisateurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_FRED_UTILISATEUR_UtilisateurModificationId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "UtilisateurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_FRED_UTILISATEUR_UtilisateurSupressionId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "UtilisateurSupressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_FRED_ETABLISSEMENT_COMPTABLE_EtablissementId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "EtablissementId",
                principalSchema: "dbo",
                principalTable: "FRED_ETABLISSEMENT_COMPTABLE",
                principalColumn: "EtablissementComptableId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_FACTURE_FRED_JOURNAL_JournalId",
                schema: "dbo",
                table: "FRED_FACTURE",
                column: "JournalId",
                principalSchema: "dbo",
                principalTable: "FRED_JOURNAL",
                principalColumn: "JournalId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_FRED_UTILISATEUR_AuteurCreationId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "AuteurCreationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_FRED_UTILISATEUR_AuteurModificationId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "AuteurModificationId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_FRED_UTILISATEUR_AuteurSuppressionId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "AuteurSuppressionId",
                principalSchema: "dbo",
                principalTable: "FRED_UTILISATEUR",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_FRED_SOCIETE_RESSOURCE_NATURE_ReferentielEtenduId",
                schema: "dbo",
                table: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                column: "ReferentielEtenduId",
                principalSchema: "dbo",
                principalTable: "FRED_SOCIETE_RESSOURCE_NATURE",
                principalColumn: "ReferentielEtenduId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_EQUIPE_PERSONNEL_FRED_PERSONNEL_PersonnelId",
                schema: "dbo",
                table: "FRED_EQUIPE_PERSONNEL",
                column: "PersonnelId",
                principalSchema: "dbo",
                principalTable: "FRED_PERSONNEL",
                principalColumn: "PersonnelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_EQUIPE_PERSONNEL_FRED_EQUIPE_EquipePersoId",
                schema: "dbo",
                table: "FRED_EQUIPE_PERSONNEL",
                column: "EquipePersoId",
                principalSchema: "dbo",
                principalTable: "FRED_EQUIPE",
                principalColumn: "EquipeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PERSONNEL_FRED_MATERIEL_MaterielId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "MaterielId",
                principalSchema: "dbo",
                principalTable: "FRED_MATERIEL",
                principalColumn: "MaterielId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PERSONNEL_FRED_EQUIPE_EquipeFavoriteId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "EquipeFavoriteId",
                principalSchema: "dbo",
                principalTable: "FRED_EQUIPE",
                principalColumn: "EquipeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PERSONNEL_FRED_ETABLISSEMENT_PAIE_EtablissementPayeId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "EtablissementPayeId",
                principalSchema: "dbo",
                principalTable: "FRED_ETABLISSEMENT_PAIE",
                principalColumn: "EtablissementPaieId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FRED_PERSONNEL_FRED_ETABLISSEMENT_PAIE_EtablissementRattachementId",
                schema: "dbo",
                table: "FRED_PERSONNEL",
                column: "EtablissementRattachementId",
                principalSchema: "dbo",
                principalTable: "FRED_ETABLISSEMENT_PAIE",
                principalColumn: "EtablissementPaieId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRED_RESSOURCE_FRED_CI_SpecifiqueCiId",
                schema: "dbo",
                table: "FRED_RESSOURCE");

            migrationBuilder.DropForeignKey(
                name: "FK_FRED_EQUIPE_FRED_PERSONNEL_ProprietaireId",
                schema: "dbo",
                table: "FRED_EQUIPE");

            migrationBuilder.DropForeignKey(
                name: "FK_FRED_UTILISATEUR_FRED_PERSONNEL_UtilisateurId",
                schema: "dbo",
                table: "FRED_UTILISATEUR");

            migrationBuilder.DropTable(
                name: "FRED_ASSOCIE_SEP",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AUTHENTIFICATION_LOG",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AVANCEMENT_RECETTE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AVANCEMENT_TACHE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AVANCEMENT_WORKFLOW",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BAREME_EXPLOITATION_CI_SURCHARGE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_TACHE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_WORKFLOW",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CARBURANT_ORGANISATION_DEVISE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CI_CODE_MAJORATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CI_DEVISE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CI_PRIME",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CI_RESSOURCE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_COMMANDE_CONTRAT_INTERIMAIRE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CONTROLE_BUDGETAIRE_VALEURS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CONTROLE_POINTAGE_ERREUR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_DATES_CALENDRIER_PAIE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_DATES_CLOTURE_COMPTABLE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_DEPENSE_TEMPORAIRE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ECRITURE_COMPTABLE_CUMUL",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ECRITURE_COMPTABLE_REJET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_EMAIL_SUBSCRIPTION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_EQUIPE_PERSONNEL",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FACTURATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FAVORI_UTILISATEUR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FEATURE_FLIPPING",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FONCTIONNALITE_DESACTIVE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_INDEMNITE_DEPLACEMENT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_LOG_IMPORT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_LOT_FAR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_MATRICULE_EXTERNE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_MODULE_DESACTIVE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_NOTIFICATION_UTILISATEUR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_OBJECTIF_FLASH_TACHE_JOURNALISATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_JOURNALISATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_OPERATION_DIVERSE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ORGA_LIENS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ORGANISATION_GENERIQUE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PARAM_VALUE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PARAMETRE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PERMISSION_FONCTIONNALITE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PIECE_JOINTE_COMMANDE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PIECE_JOINTE_RECEPTION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_POINTAGE_ANTICIPE_PRIME",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORT_LIGNE_CODE_ASTREINTE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORT_LIGNE_MAJORATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORT_LIGNE_PRIME",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORT_LIGNE_TACHE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORT_TACHE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORTPRIME_LIGNE_ASTREINTE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORTPRIME_LIGNE_PRIME",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_REMONTEE_VRAC_ERREUR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_REMPLACEMENT_TACHE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_REPARTITION_ECART",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RESSOURCE_RECOMMANDEE_ORGANISATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RESSOURCE_TACHE_DEVISE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ROLE_DEVISE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ROLE_FONCTIONNALITE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ROLE_ORGANISATION_DEVISE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_SOCIETE_DEVISE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_TACHE_RECETTE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_TRANSCO_IMPORT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_TYPE_DEPENSE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_UNITE_REFERENTIEL_ETENDU",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_UNITE_SOCIETE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_VALORISATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_VISA",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ZONE_DE_TRAVAIL",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NLogs",
                schema: "nlog");

            migrationBuilder.DropTable(
                name: "FRED_TYPE_PARTICIPATION_SEP",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_RECETTE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AVANCEMENT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CONTROLE_BUDGETAIRE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CONTROLE_POINTAGE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FACTURATION_TYPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_OBJECTIF_FLASH_TACHE_RESSOURCE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ECRITURE_COMPTABLE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PARAM_KEY",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PERMISSION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PIECE_JOINTE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_DEPENSE_ACHAT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_POINTAGE_ANTICIPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CODE_ASTREINTE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORT_LIGNE_ASTREINTE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PRIME",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORTPRIME_LIGNE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_REMONTEE_VRAC",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RESSOURCE_TACHE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FONCTIONNALITE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_DELEGATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ROLE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BAREME_EXPLOITATION_CI",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BAREME_EXPLOITATION_ORGANISATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CONTRAT_INTERIMAIRE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AVANCEMENT_ETAT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_T4_RESSOURCE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_BIBLIOTHEQUE_PRIX",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_OBJECTIF_FLASH_TACHE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FAMILLE_OPERATION_DIVERSE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_COMMANDE_LIGNE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_DEPENSE_TYPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FACTURE_LIGNE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_GROUPE_REMPLACEMENT_TACHE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ASTREINTE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORT_LIGNE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORTPRIME",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_MODULE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_SOCIETE_RESSOURCE_NATURE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_MOTIF_REMPLACEMENT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_T4",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_OBJECTIF_FLASH",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_COMMANDE_LIGNE_AVENANT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FACTURE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AFFECTATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AFFECTATION_MOYEN",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CODE_ABSENCE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CODE_DEPLACEMENT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CODE_MAJORATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CODE_ZONE_DEPLACEMENT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_LOT_POINTAGE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_NATURE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_TACHE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_COMMANDE_AVENANT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_JOURNAL",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AFFECTATION_MOYEN_TYPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_MATERIEL_LOCATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RAPPORT_STATUT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_REVISION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_COMMANDE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_AFFECTATION_MOYEN_FAMILLE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_STATUT_COMMANDE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_SYSTEME_EXTERNE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_TYPE_ENERGIE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_COMMANDE_TYPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_BUDGET_ETAT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_SYSTEME_EXTERNE_TYPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_SYSTEME_IMPORT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CI",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CI_TYPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_DEVISE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PERSONNEL",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_EQUIPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ETABLISSEMENT_PAIE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_MATERIEL",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PERSONNEL_IMAGE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ETABLISSEMENT_COMPTABLE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_RESSOURCE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_SITE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_SOCIETE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CARBURANT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_SOUS_CHAPITRE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_TYPE_RESSOURCE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_IMAGE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_FOURNISSEUR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_INDEMNITE_DEPLACEMENT_CALCUL_TYPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_SOCIETE_CLASSIFICATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_TYPE_SOCIETE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_UNITE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_CHAPITRE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_PAYS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_UTILISATEUR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_GROUPE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_EXTERNALDIRECTORY",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_POLE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_HOLDING",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_ORGANISATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRED_TYPE_ORGANISATION",
                schema: "dbo");
        }
    }
}
