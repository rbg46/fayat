-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- ALIMENTATION PERSONNEL
-- --------------------------------------------------

IF OBJECT_ID ( 'Fred_ToolBox_Personnel', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_Personnel;  
GO  
CREATE PROCEDURE Fred_ToolBox_Personnel   
    @Matricule nvarchar(50),   
    @IsInterne int = NULL, 
	@IsInterimaire int = NULL,
	@Nom  nvarchar(50),
	@Prenom  nvarchar(50),
	@CategoriePerso  nvarchar(50) = NULL,
	@statut nvarchar(50) = NULL,
	@DateEntree nvarchar(50) = NULL,
	@Adresse1  nvarchar(50) = NULL,
	@Adresse2  nvarchar(50) = NULL,
	@Adresse3  nvarchar(50) = NULL,
	@CodePostal  nvarchar(50) = NULL,
	@Ville  nvarchar(50) = NULL,
	@PaysLabel  nvarchar(50) = NULL,
	@SocieteCode  nvarchar(50),
	@GroupeCode nvarchar(50),
	@EtablissementPayeCode nvarchar(50),
	@EtablissementRattachementCode nvarchar(50)

AS   

	DECLARE @PaysId int; 
	DECLARE @SocieteId int;
	DECLARE @EtablissementComptableId int;
	DECLARE @EtablissementPaieId int;
	DECLARE @ERROR INT;

	SET @ERROR = 0;

	--SET @PaysId = (SELECT PaysId FROM FRED_PAYS Where Libelle = UPPER(@PaysLabel));
	SET @PaysId = 1;
	
	
	-- CONTROLE SUR LE CODE SOCIETE en FONCTION DU GROUPE
	SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode))
	IF(@SocieteId IS NULL)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : Code Société non identifié. Ajout annulé'
		END 


	-- RECHERCHE DES ETABLISSEMENTS

	
	SET @EtablissementComptableId = (SELECT EtablissementPaieId FROM FRED_ETABLISSEMENT_PAIE Where Code = @EtablissementPayeCode AND SocieteId = @SocieteId);
	SET @EtablissementPaieId = (SELECT EtablissementPaieId FROM FRED_ETABLISSEMENT_PAIE Where Code = @EtablissementPayeCode AND SocieteId = @SocieteId);

	
	

	DECLARE @_DateEntree datetime;
	SET @_DateEntree = (SELECT convert(datetime, @DateEntree, 103));
	

	-- RECHERCHE DU CODE PERSONNEL POUR SAVOIR SI MATRICULE DEJA EXISTANT
	DECLARE @PersonnelId INT;
	SET @PersonnelId = (
		SELECT PersonnelId FROM FRED_PERSONNEL where Matricule = @Matricule 
												AND SocieteId = 
												(
												SELECT SocieteId FROM FRED_SOCIETE 
												WHERE code = @SocieteCode 
												AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode)));
	


	-- GESTION INTERIMAIRE
	IF(@IsInterne IS NULL)
	SET @IsInterne = 1;

	-- GESTION  INTERNE
	IF(@IsInterimaire IS NULL)
	SET @IsInterimaire = 0;



	-- GESTION DU STATUT RH
	-- Grille de Correspondance
	--	ETAM_ARTICLE_36  = 5
	--	CADRE = 3
	--	ETAM_BUREAU = 4
	--	ETAM_CHANTIER = 2
	--	OUVRIER = 1

	DECLARE @_Statut VARCHAR(1);

	IF(UPPER(@statut) = 'ETAM')
		SET @_Statut = 'E'

	IF(UPPER(@statut) = 'ETAM_ARTICLE_36')
		SET @_Statut = 'E'

	IF(UPPER(@statut) = 'CADRE')
		SET @_Statut = 'C'

	IF(UPPER(@statut) = 'IAC')
		SET @_Statut = 'C'

	IF(UPPER(@statut) = 'ETAM_BUREAU')
		SET @_Statut = 'E'

	IF(UPPER(@statut) = 'ETAM_CHANTIER')
		SET @_Statut = 'E'

	IF(UPPER(@statut) = 'OUVRIER')
		SET @_Statut = 'O'

	IF(UPPER(@statut) = 'O')
		SET @_Statut = 'O'

	IF(UPPER(@statut) = 'E')
		SET @_Statut = 'E'

	IF(UPPER(@statut) = 'C')
		SET @_Statut = 'C'





	IF @ERROR = 0
	BEGIN
		IF @PersonnelId IS NULL
		BEGIN
			-- AJOUT DU PERSONNEL
			INSERT INTO FRED_PERSONNEL 
			(
				Matricule, 
				IsInterne, 
				IsInterimaire,
				Nom, 
				Prenom, 
				CategoriePerso, 
				Statut, 
				DateEntree, 
				Adresse1, 
				Adresse2, 
				Adresse3, 
				CodePostal, 
				Ville, 
				PaysId, 
				SocieteId, 
				EtablissementRattachementId, 
				EtablissementPayeId
			)
			VALUES (
				@Matricule, 
				@IsInterne, 
				@IsInterimaire,
				@Nom, 
				@Prenom, 
				@CategoriePerso, 
				@_Statut, 
				@DateEntree, 
				@Adresse1, 
				@Adresse2, 
				@Adresse3, 
				@CodePostal, 
				@Ville, 
				@PaysId, 
				@SocieteId, 
				@EtablissementComptableId, 
				@EtablissementPaieId
				)
			PRINT 'INFO : AJOUT DU PERSONNEL'
		END
		ELSE
		BEGIN
			-- MISE à JOUR DES DONNEES. 
			-- ATTENTION CETTE MISE A JOUR PORTE UNIQUEMENT SUR LES CHAMPS RENSEIGNEES
			IF (@Nom IS NOT NULL)
			BEGIN
				UPDATE FRED_PERSONNEL SET Nom = @Nom WHERE PersonnelId = @PersonnelId
			END

			IF (@Prenom IS NOT NULL)
			BEGIN
				UPDATE FRED_PERSONNEL SET Prenom = @Prenom WHERE PersonnelId = @PersonnelId
			END

			IF(@_Statut IS NOT NULL)
			BEGIN
				UPDATE FRED_PERSONNEL SET Statut = @_Statut WHERE PersonnelId = @PersonnelId
			END

			IF(@IsInterne IS NOT NULL)
			BEGIN
				UPDATE FRED_PERSONNEL SET IsInterne = @IsInterne WHERE PersonnelId = @PersonnelId
			END

			IF(@IsInterimaire IS NOT NULL)
			BEGIN
				UPDATE FRED_PERSONNEL SET IsInterimaire = @IsInterimaire WHERE PersonnelId = @PersonnelId
			END

		END
	END

	

GO 
