CREATE PROCEDURE #InsertUser (
	@UtilisateurLogin NVARCHAR(50),
	@UtilisateurPassword NVARCHAR(250),
	@Matricule NVARCHAR(10),
	@Nom NVARCHAR(150),
	@Prenom NVARCHAR(150),
	@SocieteCode NVARCHAR(20),
	@RessourceCode NVARCHAR(20),
	@Statut NVARCHAR(1),
	@CategoriePerso NVARCHAR(1),
	@EtablissementPaieCode NVARCHAR(20) = NULL
)
AS
BEGIN
	DECLARE @SocieteId INT
	SELECT @SocieteId = SocieteId FROM FRED_SOCIETE WHERE Code = @SocieteCode

	DECLARE @ResourceId INT
	SELECT @ResourceId = RessourceId FROM FRED_RESSOURCE WHERE Code = @RessourceCode
		
	DECLARE @EtablissementPaieId INT
	SELECT @EtablissementPaieId = EtablissementPaieId FROM FRED_ETABLISSEMENT_PAIE WHERE Code = @EtablissementPaieCode

	INSERT INTO FRED_PERSONNEL (
		SocieteId,
		Matricule,
		Nom,
		Prenom,
		RessourceId,
		Statut,
		CategoriePerso,
		IsInterne,
		UtilisateurIdCreation,
		IsInterimaire,
		EtablissementPayeId,
		EtablissementRattachementId,
		TypeRattachement,
		DateEntree
	) VALUES (
		@SocieteId,
		@Matricule,
		@Nom,
		@Prenom,
		@ResourceId,
		@Statut,
		@CategoriePerso,
		0,
		1,
		0,
		@EtablissementPaieId,
		@EtablissementPaieId,
		'A',
		DATEADD(YEAR, -1, GETDATE())
	)

	DECLARE @PersonnelId INT
	SELECT @PersonnelId = @@IDENTITY

	INSERT INTO FRED_EXTERNALDIRECTORY (
		MotDePasse,
		IsActived,
		DateExpiration
	) VALUES (
		@UtilisateurPassword,
		1,
		DATEADD(YEAR, 5, GETDATE())
	)

	DECLARE @FayatAccessDirectoryId INT
	SELECT @FayatAccessDirectoryId = @@IDENTITY

	INSERT INTO FRED_UTILISATEUR (
		UtilisateurId,
		Login,
		FayatAccessDirectoryId,
		IsDeleted,
		DateCreation,
		UtilisateurIdCreation,
		CommandeManuelleAllowed
	) VALUES (
		@PersonnelId,
		@UtilisateurLogin,
		@FayatAccessDirectoryId,
		0,
		DATEADD(YEAR, -1, GETDATE()),
		1,
		1
	)

	RETURN @PersonnelId
END
GO

CREATE PROCEDURE #InsertRole (
	@UtilisateurId INT,
	@SocieteCode NVARCHAR(20),
	@RoleCodeNomFamilier NVARCHAR(20),
	@OrganisationSocieteCode NVARCHAR(20),
	@DeviseIsoCode NVARCHAR(3) = NULL,
	@CommandeSeuil DECIMAL(11, 2) = NULL
)
AS
BEGIN
	DECLARE @RoleId INT
	SELECT @RoleId =
		RoleId
		FROM
			FRED_ROLE R
			JOIN FRED_SOCIETE S ON S.SocieteId = R.SocieteId
		WHERE
			S.Code = @SocieteCode
			AND R.CodeNomFamilier = @RoleCodeNomFamilier

	DECLARE @OrganisationId INT
	SELECT @OrganisationId = OrganisationId FROM FRED_SOCIETE WHERE Code = @OrganisationSocieteCode

	DECLARE @DeviseId INT
	SELECT @DeviseId = DeviseId FROM FRED_DEVISE WHERE IsoCode = @DeviseIsoCode

	IF (@RoleId IS NOT NULL AND @OrganisationId IS NOT NULL)
	BEGIN
		INSERT INTO FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE (
			UtilisateurId,
			RoleId,
			OrganisationId,
			DeviseId,
			CommandeSeuil
		) VALUES (
			@UtilisateurId,
			@RoleId,
			@OrganisationId,
			@DeviseId,
			@CommandeSeuil
		)

		RETURN @@IDENTITY
	END

	RETURN 0
END
GO

BEGIN TRANSACTION

BEGIN TRY
	DECLARE @UtilisateurId INT

	IF NOT EXISTS (SELECT 1 FROM FRED_UTILISATEUR WHERE Login = 'ANASS_FIT')
	BEGIN
		EXEC @UtilisateurId = #InsertUser
			@UtilisateurLogin = 'ANASS_FIT',
			@UtilisateurPassword = 'ANASS_FIT',
			@Matricule = 'ANASS_FIT',
			@Nom = 'LYAZID',
			@Prenom = 'ANASS',
			@SocieteCode = 'RB',
			@RessourceCode = 'ENCA-35',
			@Statut = '3',
			@CategoriePerso = 'M',
			@EtablissementPaieCode = '00'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'RB',
			@RoleCodeNomFamilier = 'ADM',
			@OrganisationSocieteCode = 'RB'
	END

	IF NOT EXISTS (SELECT 1 FROM FRED_UTILISATEUR WHERE Login = 'HASSAN_FIT')
	BEGIN
		EXEC @UtilisateurId = #InsertUser
			@UtilisateurLogin = 'HASSAN_FIT',
			@UtilisateurPassword = 'HASSAN_FIT',
			@Matricule = 'HASSANFIT',
			@Nom = 'ENNAJMAOUI',
			@Prenom = 'HASSAN',
			@SocieteCode = 'RB',
			@RessourceCode = 'ENCA-10',
			@Statut = '2',
			@CategoriePerso = 'H',
			@EtablissementPaieCode = '00'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'RB',
			@RoleCodeNomFamilier = 'ADM',
			@OrganisationSocieteCode = 'RB',
			@DeviseIsoCode = 'EUR',
			@CommandeSeuil = 1400

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'RB',
			@RoleCodeNomFamilier = 'ADM',
			@OrganisationSocieteCode = 'RB-SEP'
	END
	
	IF NOT EXISTS (SELECT 1 FROM FRED_UTILISATEUR WHERE Login = 'HASSAN_FES')
	BEGIN
		EXEC @UtilisateurId = #InsertUser
			@UtilisateurLogin = 'HASSAN_FES',
			@UtilisateurPassword = 'HASSAN_FES',
			@Matricule = '007007',
			@Nom = 'ENNAJMAOUI',
			@Prenom = 'HASSAN',
			@SocieteCode = 'E001',
			@RessourceCode = '00450',
			@Statut = '3',
			@CategoriePerso = 'M',
			@EtablissementPaieCode = '10'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'E001',
			@RoleCodeNomFamilier = 'ADM',
			@OrganisationSocieteCode = 'E001'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'E001',
			@RoleCodeNomFamilier = 'GSM',
			@OrganisationSocieteCode = 'E001'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'E001',
			@RoleCodeNomFamilier = 'RCI',
			@OrganisationSocieteCode = 'E010'
	END
	
	IF NOT EXISTS (SELECT 1 FROM FRED_UTILISATEUR WHERE Login = 'aazizi')
	BEGIN
		EXEC @UtilisateurId = #InsertUser
			@UtilisateurLogin = 'aazizi',
			@UtilisateurPassword = 'aazizi',
			@Matricule = '999999',
			@Nom = 'AZIZI',
			@Prenom = 'AZIZ',
			@SocieteCode = 'E001',
			@RessourceCode = '00775',
			@Statut = '3',
			@CategoriePerso = 'M',
			@EtablissementPaieCode = '09'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'E001',
			@RoleCodeNomFamilier = 'GSP',
			@OrganisationSocieteCode = 'E001'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'E001',
			@RoleCodeNomFamilier = 'GSP',
			@OrganisationSocieteCode = 'E003'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'E001',
			@RoleCodeNomFamilier = 'IAC',
			@OrganisationSocieteCode = 'E001'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'E001',
			@RoleCodeNomFamilier = 'MP',
			@OrganisationSocieteCode = 'E010'
	END

	IF NOT EXISTS (SELECT 1 FROM FRED_UTILISATEUR WHERE Login = 'cdc')
	BEGIN
		EXEC @UtilisateurId = #InsertUser
			@UtilisateurLogin = 'cdc',
			@UtilisateurPassword = 'winter=1',
			@Matricule = '789456',
			@Nom = 'DE CAZENOVE',
			@Prenom = 'Clemence',
			@SocieteCode = 'RB',
			@RessourceCode = 'MO-01',
			@Statut = '3',
			@CategoriePerso = 'M'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'RB',
			@RoleCodeNomFamilier = 'ADM',
			@OrganisationSocieteCode = 'RB'
	END

	IF NOT EXISTS (SELECT 1 FROM FRED_UTILISATEUR WHERE Login = 'CDCTP')
	BEGIN
		EXEC @UtilisateurId = #InsertUser
			@UtilisateurLogin = 'CDCTP',
			@UtilisateurPassword = 'Winter=1',
			@Matricule = '234',
			@Nom = 'CDCTP',
			@Prenom = 'clémence',
			@SocieteCode = '0001',
			@RessourceCode = 'PERS-0020',
			@Statut = '3',
			@CategoriePerso = 'M'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = '0001',
			@RoleCodeNomFamilier = 'ADM',
			@OrganisationSocieteCode = '0001',
			@DeviseIsoCode = 'EUR',
			@CommandeSeuil = '2350.00'
	END

	IF NOT EXISTS (SELECT 1 FROM FRED_UTILISATEUR WHERE Login = 'FADILA_RZB')
	BEGIN
		EXEC @UtilisateurId = #InsertUser
			@UtilisateurLogin = 'FADILA_RZB',
			@UtilisateurPassword = 'Fadila12*',
			@Matricule = '12345',
			@Nom = 'EL YAAGOUBI',
			@Prenom = 'Fadila',
			@SocieteCode = 'RB',
			@RessourceCode = 'ENCA-35',
			@Statut = '3',
			@CategoriePerso = 'M'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'RB',
			@RoleCodeNomFamilier = 'ADM',
			@OrganisationSocieteCode = 'RB',
			@DeviseIsoCode = 'EUR',
			@CommandeSeuil = '500000.00'
	END

	IF NOT EXISTS (SELECT 1 FROM FRED_UTILISATEUR WHERE Login = 'VFT')
	BEGIN
		EXEC @UtilisateurId = #InsertUser
			@UtilisateurLogin = 'VFT',
			@UtilisateurPassword = 'Secure=1!',
			@Matricule = '363636',
			@Nom = 'FEUYANG TEKEU',
			@Prenom = 'Vanessa',
			@SocieteCode = 'RB',
			@RessourceCode = 'MO-01',
			@Statut = '3',
			@CategoriePerso = 'M'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = 'RB',
			@RoleCodeNomFamilier = 'ADM',
			@OrganisationSocieteCode = 'RB'
	END
	

	IF NOT EXISTS (SELECT 1 FROM FRED_UTILISATEUR WHERE Login = 'VFTTP')
	BEGIN
		EXEC @UtilisateurId = #InsertUser
			@UtilisateurLogin = 'VFTTP',
			@UtilisateurPassword = 'Secure=1!',
			@Matricule = '363',
			@Nom = 'FEUYANG TEKEU',
			@Prenom = 'Vanessa',
			@SocieteCode = '0001',
			@RessourceCode = 'PERS-0020',
			@Statut = '3',
			@CategoriePerso = 'M'

		EXEC #InsertRole
			@UtilisateurId = @UtilisateurId,
			@SocieteCode = '0001',
			@RoleCodeNomFamilier = 'ADM',
			@OrganisationSocieteCode = '0001',
			@DeviseIsoCode = 'EUR',
			@CommandeSeuil = '2350.00'
	END
END TRY  
BEGIN CATCH  
	SELECT   
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_STATE() AS ErrorState,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage

	IF @@TRANCOUNT > 0 
		ROLLBACK TRANSACTION;
END CATCH
  
IF @@TRANCOUNT > 0  
	COMMIT TRANSACTION
GO

DROP PROCEDURE #InsertUser
DROP PROCEDURE #InsertRole