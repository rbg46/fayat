-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_CODE_MAJORATION 
-- MODOP : 
--    EXEC Fred_ToolBox_Majoration @Code=' ',  @Libelle=' ',  @EtatPublic=' ',  @IsActif=' ',  @GroupeCode=' ',  @DateCreation=' ',  @DateModification=' ',  @DateSuppression=' ',  @AuteurCreationCode=' ',  @AuteurModificationCode=' ',  @AuteurSuppressionId=' ',  @IsHeureNuit=' '  
-- -------------------------------------------------- 
 
 
IF OBJECT_ID ( 'Fred_ToolBox_Code_Majoration', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Code_Majoration; 
GO 
CREATE PROCEDURE Fred_ToolBox_Code_Majoration
 
 
		@verbose INT =NULL,
		@version INT =NULL,
		@Code  nvarchar(20),
		@Libelle  nvarchar(500),
		@EtatPublic varchar(max),
		@IsActif varchar(max)=NULL,
		@GroupeCode  varchar(max), 
		@IsHeureNuit varchar(max)=NULL
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
				PRINT 'INJECTION DES FRED_CODE_MAJORATION (PS Fred_ToolBox_Majoration)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 
		-- ----------------------------
		-- BEGIN OUI/NON
				DECLARE @_EtatPublic bit;
			SET @_EtatPublic =(SELECT dbo.FredGetBoolean (@EtatPublic));
				DECLARE @_IsActif bit;
			SET @_IsActif =(SELECT dbo.FredGetBoolean (@IsActif));
				DECLARE @_IsHeureNuit bit;
			SET @_IsHeureNuit =(SELECT dbo.FredGetBoolean (@IsHeureNuit));
		-- END OUI/NON
		-- ----------------------------
 
 
		-- ----------------------------
		-- END FOREIGN KEYS
			DECLARE @GroupeId INT;
			SET @GroupeId= (SELECT groupeID FROM FRED_GROUPE where Code = @groupeCode)
 

 
		-- END FOREIGN KEYS
		-- ----------------------------
		 -- RECUPERATION DE LA CLE PRIMAIRE EN FONCTION DU CODE ET DU GROUPE
		
		

		-- CONTROLE POUR UNICITE SUR LE CODE MAJORATION
		DECLARE @CodeMajorationIdCount INT =  (SELECT COUNT(*) FROM FRED_CODE_MAJORATION WHERE code = @code AND GroupeId = @GroupeId);
		IF(@CodeMajorationIdCount > 1)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : PLUSIEURS REFERENCES TROUVEES POUR CODE MAJORATION '+ @Code;
		END
		
		DECLARE @CodeMajorationId INT
		IF (@ERROR = 0)
		BEGIN
		SET @CodeMajorationId= (SELECT CodeMajorationId FROM FRED_CODE_MAJORATION WHERE code = @code AND GroupeId = @GroupeId)
		END

IF @ERROR = 0
BEGIN
IF @CodeMajorationId IS NULL
		BEGIN
			-- ---------------------------- 
			-- ORDRE AJOUT 
			-- ---------------------------- 
			INSERT INTO FRED_CODE_MAJORATION (
				Code, 
				Libelle, 
				EtatPublic, 
				IsActif, 
				GroupeId, 
				DateCreation, 
				
				AuteurCreationId, 
				
				IsHeureNuit  
		)
		VALUES 
		(
				@Code, 
				@Libelle, 
				@_EtatPublic, 
				@_IsActif, 
				@GroupeId, 
				GETDATE(), 
				
				1, 
				
				@_IsHeureNuit  
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
			SET @Code = (SELECT Code FROM FRED_CODE_MAJORATION WHERE CodeMajorationId = @CodeMajorationId)
		END
		IF(@Libelle IS NULL)
		BEGIN
			SET @Libelle = (SELECT Libelle FROM FRED_CODE_MAJORATION WHERE CodeMajorationId = @CodeMajorationId)
		END
		IF(@EtatPublic IS NULL)
		BEGIN
			SET @_EtatPublic = (SELECT EtatPublic FROM FRED_CODE_MAJORATION WHERE CodeMajorationId = @CodeMajorationId)
		END
		IF(@IsActif IS NULL)
		BEGIN
			SET @_IsActif = (SELECT IsActif FROM FRED_CODE_MAJORATION WHERE CodeMajorationId = @CodeMajorationId)
		END
		IF(@GroupeId IS NULL)
		BEGIN
			SET @GroupeId = (SELECT GroupeId FROM FRED_CODE_MAJORATION WHERE CodeMajorationId = @CodeMajorationId)
		END
		
		
		
		IF(@IsHeureNuit IS NULL)
		BEGIN
			SET @_IsHeureNuit = (SELECT IsHeureNuit FROM FRED_CODE_MAJORATION WHERE CodeMajorationId = @CodeMajorationId)
		END
 -- REPRISE DES ANCIENNES VALEURS
			-- ---------------------------- 
			-- MISE A JOUR 
			-- ---------------------------- 
				UPDATE  FRED_CODE_MAJORATION
				SET
					Code= @Code, 
					Libelle= @Libelle, 
					EtatPublic= @_EtatPublic, 
					IsActif= @_IsActif, 
					GroupeId= @GroupeId,  
					DateModification= GETDATE(), 
					AuteurModificationId= 1, 
					IsHeureNuit= @_IsHeureNuit  
 		WHERE 
			CodeMajorationId= @CodeMajorationId
		IF @verbose = 1 
			BEGIN
				PRINT 'INFO : Mise à jour réalisée'
			END
		END
END
GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Majoration  POUR TABLE  FRED_CODE_MAJORATION
 -- ----------------------------------------------------------
