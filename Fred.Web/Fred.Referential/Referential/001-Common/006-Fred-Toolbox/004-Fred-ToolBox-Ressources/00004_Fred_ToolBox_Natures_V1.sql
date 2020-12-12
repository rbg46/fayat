-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_NATURE 
-- MODOP : 
--    EXEC Fred_ToolBox_Nature @Code='RUE',  @Libelle='Je traverse la rue et je  trouve un job',  @GroupeCode='GFES', @SocieteCode='E001',  @IsActif='Non', @verbose=1  
--    EXEC Fred_ToolBox_Insert_Nature_Analytique @GroupeCode='GFES',@SocieteCode='E001',@Code='60000',@Libelle='Nature 1'
-- -------------------------------------------------- 
 SELECT * FROM FRED_NATURE WHERE NatureId = 369
 
IF OBJECT_ID ( 'Fred_ToolBox_Nature', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Nature; 
GO 
CREATE PROCEDURE Fred_ToolBox_Nature
		@GroupeCode  nvarchar(50),
		@verbose INT =NULL,
		@version INT =NULL,
		@Code  nvarchar(50),
		@Libelle  nvarchar(500),
		@SocieteCode  varchar(max), 
		@DateCreation  datetime=NULL, 
		@AuteurCreationCode  varchar(max)=NULL, 
		@DateModification  datetime=NULL, 
		@AuteurModificationCode  varchar(max)=NULL, 
		@IsActif varchar(max) =NULL
 AS
-- DECLARATION DES VARIABLES
DECLARE @ERROR INT;
SET @ERROR =0;

DECLARE @PrimaryKeyField varchar(max);
DECLARE @PrimaryKeyValue INT;

		IF @verbose = 1 
		BEGIN
				PRINT '------------------------------'
				PRINT 'FAYAT IT - 2018 '
				PRINT 'INJECTION DES FRED_NATURE (PS Fred_ToolBox_Nature)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 
		-- ----------------------------
		-- BEGIN OUI/NON
				DECLARE @_IsActif bit;
			SET @_IsActif =(SELECT dbo.FredGetBoolean (@IsActif));
		-- END OUI/NON
		-- ----------------------------
 
 
		-- ----------------------------
		-- END FOREIGN KEYS

			DECLARE @SocieteId INT;
			SET @SocieteId=  (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode))
			-- CONTROLE SUR SOCIETE - SI CODE SOCIETE NON TROUVE > UNE ERREUR EST SOULEVEE
			IF(@SocieteId IS NULL)
			BEGIN
				SET @ERROR = 1;
				PRINT 'ERREUR : Code Société non identifié'
			END 
 


		

		-- END FOREIGN KEYS
		-- ----------------------------
 
		DECLARE @NatureId INT
		SET @NatureId= 
		(
		SELECT NatureId FROM FRED_NATURE WHERE code = @code AND SocieteId= @SocieteId
		)



IF @ERROR = 0
BEGIN
IF @NatureId IS NULL
		BEGIN
			-- ---------------------------- 
			-- ORDRE AJOUT 
			-- ---------------------------- 
			INSERT INTO FRED_NATURE (
				Code, 
				Libelle, 
				SocieteId, 
				DateCreation, 
				AuteurCreationId, 
				IsActif  
		)
		VALUES 
		(
				@Code, 
				@Libelle, 
				@SocieteId, 
				GETDATE(), 
				1, 
				@_IsActif  
		);
		IF @verbose = 1 
			BEGIN
				PRINT 'INFO : Ajout réalisée'
			END
		END
	ELSE
		BEGIN
		 -- REPRISE DES ANCIENNES VALEURS
				IF(@Code IS NULL)
				BEGIN
					SET @Code = (SELECT Code FROM FRED_NATURE WHERE NatureId = @NatureId)
				END
				IF(@Libelle IS NULL)
				BEGIN
					SET @Libelle = (SELECT Libelle FROM FRED_NATURE WHERE NatureId = @NatureId)
				END
				IF(@SocieteId IS NULL)
				BEGIN
					SET @SocieteId = (SELECT SocieteId FROM FRED_NATURE WHERE NatureId = @NatureId)
				END
				
				IF(@IsActif IS NULL)
				BEGIN
					SET @_IsActif = (SELECT IsActif FROM FRED_NATURE WHERE NatureId = @NatureId)
				END
		 -- REPRISE DES ANCIENNES VALEURS
				-- ---------------------------- 
				-- MISE A JOUR 
				-- ---------------------------- 
					UPDATE  FRED_NATURE
					SET
						Code= @Code, 
						Libelle= @Libelle, 
						SocieteId= @SocieteId, 
						DateModification= GETDATE(), 
						AuteurModificationId= 1, 
						IsActif= @_IsActif  
 					WHERE 
						NatureId= @NatureId
			IF @verbose = 1 
				BEGIN
					PRINT 'INFO : Mise à jour réalisée'
				END
			END
END
GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Nature  POUR TABLE  FRED_NATURE
 -- ----------------------------------------------------------
