-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_SOCIETE 
-- MODOP : EXEC Fred_ToolBox_Societe @GroupeCode='GFES2',  @Code='FES3',  @CodeSocietePaye='15',  @CodeSocieteComptable='15',  @Libelle='Socie 1',  @Adresse='18, rue des Pres',  @Ville='ISSY LES MOULINEAUX',  @CodePostal='75000',  @SIRET='12536975',  @Externe='Non',  @Active='Oui',  @MoisDebutExercice='10',  @MoisFinExercice='9',  @IsGenerationSamediCPActive='Oui',  @ImportFacture='Oui',    @TransfertAS400='Oui',   @IsInterimaire='Oui',  @CodeSocieteStorm='001526' , @verbose=1
  --  SELECT * FROM FRED_SOCIETE
-- -------------------------------------------------- 
 
 



IF OBJECT_ID ( 'Fred_ToolBox_Societe', 'P' ) IS NOT NULL   
	DROP PROCEDURE Fred_ToolBox_Societe; 
GO 
CREATE PROCEDURE Fred_ToolBox_Societe
 
 
		@verbose INT =NULL,
		@version INT =NULL,
		@GroupeCode  varchar(max), 
		@Code  nvarchar(20),
		@CodeSocietePaye  nvarchar(20)=NULL,
		@CodeSocieteComptable  nvarchar(20)=NULL,
		@Libelle  nvarchar(500),
		@Adresse  nvarchar(250)=NULL,
		@Ville  nvarchar(50)=NULL,
		@CodePostal  nvarchar(10)=NULL,
		@SIRET  nvarchar(19)=NULL,
		@Externe  nvarchar(max)=NULL, 
		@Active  nvarchar(max)=NULL, 
		@MoisDebutExercice  int=NULL, 
		@MoisFinExercice  int=NULL, 
		@IsGenerationSamediCPActive  nvarchar(max)=NULL, 
		@ImportFacture  nvarchar(max)=NULL, 
		@TransfertAS400  nvarchar(max)=NULL, 
		@IsInterimaire  nvarchar(max)=NULL, 
		@CodeSocieteStorm  nvarchar(max)=NULL
 AS
		DECLARE @ERROR INT;
		SET @ERROR =0;


		DECLARE @_Externe bit;
		DECLARE @_Active bit;
		DECLARE @_IsInterimaire bit;
		DECLARE @_TransfertAS400 bit;
		DECLARE @_ImportFacture bit;
		DECLARE @_IsGenerationSamediCPActive bit;
		
		
		SET @_Externe =(SELECT dbo.FredGetBoolean (@Externe));
		SET @_Active =(SELECT dbo.FredGetBoolean (@Active));
		SET @_IsInterimaire = (SELECT dbo.FredGetBoolean (@IsInterimaire));
		SET @_TransfertAS400 =(SELECT dbo.FredGetBoolean (@TransfertAS400)); 
		SET @_ImportFacture =(SELECT dbo.FredGetBoolean (@ImportFacture));
		SET @_IsGenerationSamediCPActive =(SELECT dbo.FredGetBoolean (@IsGenerationSamediCPActive));
		
		IF @verbose = 1 
		BEGIN
				PRINT @_Externe;
				PRINT @_Active;
				PRINT @_IsInterimaire;
				PRINT @_TransfertAS400;
				PRINT @_ImportFacture;
				PRINT @_IsGenerationSamediCPActive;


		END

		IF (@IsInterimaire IS NULL)
		BEGIN
			SET @_IsInterimaire = 0;
		END


		IF @verbose = 1 
		BEGIN
				PRINT '------------------------------'
				PRINT 'FAYAT IT - 2018 '
				PRINT 'INJECTION DES FRED_SOCIETE (PS Fred_ToolBox_Societe)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 
 
 

			-- Recherche GROUPE
			DECLARE @GroupeId INT;
			SET @GroupeId= dbo.FredIdFromCode('GroupeId',  @GroupeCode )
			IF @verbose = 1 
			BEGIN
					PRINT '@GroupeId'
					PRINT @GroupeId
			END


		-- Recherche Si SOCIETE existe pour le groupe
		DECLARE @SocieteId INT;
		SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @Code AND GroupeId = @GroupeId);
		


		IF (@Libelle ='' OR @Code ='')
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : Libellé ou Code non alimentée. Ajout annulé'
		END 


IF @ERROR = 0
BEGIN
IF @SocieteId IS NULL
		BEGIN

			IF @verbose = 1 
			BEGIN
				PRINT 'Ajout société en cours'
			END

			-- ---------------------------- 
			-- ORDRE AJOUT 
			-- ---------------------------- 
		
			-- AJOUT DANS LA TABLE ORGANISATION
			DECLARE @GroupeOrganisationId INT;
			DECLARE @organisationId INT;

			-- Cle Organisation Groupe
			SET @GroupeOrganisationId = (SELECT organisationID FROM FRED_GROUPE where Code = @GroupeCode);
			-- Création de la clé organisation pour la société

			INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('4',@GroupeOrganisationId); 
			SET @OrganisationId = @@IDENTITY;

			IF @verbose = 1 
			BEGIN
				PRINT 'Code organisation OK'
				PRINT @OrganisationId;
			END


			INSERT INTO FRED_SOCIETE (
				GroupeId, 
				Code, 
				CodeSocietePaye, 
				CodeSocieteComptable, 
				Libelle, 
				Adresse, 
				Ville, 
				CodePostal, 
				SIRET, 
				Externe, 
				Active, 
				MoisDebutExercice, 
				MoisFinExercice, 
				IsGenerationSamediCPActive, 
				ImportFacture, 
				OrganisationId, 
				TransfertAS400, 
				IsInterimaire, 
				CodeSocieteStorm  
		)
		VALUES 
		(
				@GroupeId, 
				@Code, 
				@CodeSocietePaye, 
				@CodeSocieteComptable, 
				@Libelle, 
				@Adresse, 
				@Ville, 
				@CodePostal, 
				@SIRET, 
				@_Externe, 
				@_Active, 
				@MoisDebutExercice, 
				@MoisFinExercice, 
				@_IsGenerationSamediCPActive, 
				@_ImportFacture, 
				@OrganisationId, 
				@_TransfertAS400, 
				@_IsInterimaire, 
				@CodeSocieteStorm  
		);


		IF @verbose = 1 
		BEGIN
				PRINT 'Ajout de la société réalisé'
		END

		END
	ELSE
		BEGIN
			-- ---------------------------- 
			-- MISE A JOUR 
			-- ---------------------------- 
				UPDATE  FRED_SOCIETE
				SET
					GroupeId= @GroupeId, 
					Code= @Code, 
					CodeSocietePaye= @CodeSocietePaye, 
					CodeSocieteComptable= @CodeSocieteComptable, 
					Libelle= @Libelle, 
					Adresse= @Adresse, 
					Ville= @Ville, 
					CodePostal= @CodePostal, 
					SIRET= @SIRET, 
					Externe= @_Externe, 
					Active= @_Active, 
					MoisDebutExercice= @MoisDebutExercice, 
					MoisFinExercice= @MoisFinExercice, 
					IsGenerationSamediCPActive= @_IsGenerationSamediCPActive, 
					ImportFacture= @_ImportFacture, 
					TransfertAS400= @_TransfertAS400, 
					IsInterimaire= @_IsInterimaire, 
					CodeSocieteStorm= @CodeSocieteStorm  
 		WHERE 
			SocieteId= @SocieteId
		IF @verbose = 1 
			BEGIN
				PRINT 'Mise à jour réalisée'
			END
		END
END
GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Societe  POUR TABLE  FRED_SOCIETE
 -- ----------------------------------------------------------
