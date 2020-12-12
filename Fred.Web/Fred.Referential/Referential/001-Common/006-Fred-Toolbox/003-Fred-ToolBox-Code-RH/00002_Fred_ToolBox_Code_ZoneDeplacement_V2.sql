-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_CODE_ZONE_DEPLACEMENT 
-- MODOP : 
--    EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES2',@SocieteCode='FES3',@Code='Z1',@Libelle='Zone 1565',@IsActif='Oui'
-- -------------------------------------------------- 



IF OBJECT_ID ( 'Fred_ToolBox_Code_Zone_Deplacement', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Code_Zone_Deplacement; 
GO 
CREATE PROCEDURE Fred_ToolBox_Code_Zone_Deplacement
 
		@GroupeCode Varchar(max),
		@verbose INT =NULL,
		@version INT =NULL,
		@Code  nvarchar(20),
		@Libelle  varchar(500), 
		@IsActif varchar(max),
		@SocieteCode  varchar(max)  
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
				PRINT 'INJECTION DES FRED_CODE_ZONE_DEPLACEMENT (PS Fred_ToolBox_Code_Zone_Deplacement)'
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
			DECLARE @AuteurCreation INT;
			--SET @AuteurCreation= dbo.FredIdFromCode('AuteurCreation',  @AuteurCreation )
 
			DECLARE @AuteurModification INT;
			--SET @AuteurModification= dbo.FredIdFromCode('AuteurModification',  @AuteurModification )
 
			DECLARE @SocieteId INT;
			SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode))
		PRINT '----'

		-- END FOREIGN KEYS
		-- ----------------------------
 

		-- CONTROLE SUR SOCIETE - SI CODE SOCIETE NON TROUVE > UNE ERREUR EST SOULEVEE
		IF(@SocieteId IS NULL)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : Code Société non identifié'
		END 
		
		


		-- CONTROLE POUR SAVOIR SI PLUSIEURS CLE
		DECLARE @Count_CodeZoneDeplacementId INT;
		SET  @Count_CodeZoneDeplacementId = (SELECT COUNT(CodeZoneDeplacementId) FROM FRED_CODE_ZONE_DEPLACEMENT WHERE code = @code AND SocieteId = @SocieteId);
		IF @Count_CodeZoneDeplacementId > 1 
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : Plusieurs Références trouvées. Aucun traitement possible pour Code ' + @code
		END

		-- RECUPERATION DE LA CLE PRIMAIRE EN FONCTION DU CODE ET DE LA SOCIETE
		DECLARE @CodeZoneDeplacementId INT
		-- SI AUCUNE ERREUR SOULEVE PAR UN NOMBRE SUPERIEUR A 1
		IF (@ERROR=0)
		BEGIN
		SET @CodeZoneDeplacementId= 
		(
				SELECT CodeZoneDeplacementId FROM FRED_CODE_ZONE_DEPLACEMENT WHERE code = @code AND SocieteId = @SocieteId
		)
		END
		


		
		
		

		IF @verbose = 1 
			BEGIN
				PRINT 'INFO : Code Zone Déplacement' + CAST (@CodeZoneDeplacementId AS VARCHAR(Max))
			END


IF @ERROR = 0
BEGIN
IF @CodeZoneDeplacementId IS NULL
		BEGIN
			-- ---------------------------- 
			-- ORDRE AJOUT 
			-- ---------------------------- 
			INSERT INTO FRED_CODE_ZONE_DEPLACEMENT (
				Code, 
				Libelle, 
				DateCreation, 
				AuteurCreation, 
				
				IsActif, 
				SocieteId  
		)
		VALUES 
		(
				@Code, 
				@Libelle, 
				GETDATE(), 
				1, 
				@_IsActif, 
				@SocieteId  
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
					SET @Code = (SELECT Code FROM FRED_CODE_ZONE_DEPLACEMENT WHERE CodeZoneDeplacementId = @CodeZoneDeplacementId)
				END
				IF(@Libelle IS NULL)
				BEGIN
					SET @Libelle = (SELECT Libelle FROM FRED_CODE_ZONE_DEPLACEMENT WHERE CodeZoneDeplacementId = @CodeZoneDeplacementId)
				END
		
		
		
				IF(@IsActif IS NULL)
				BEGIN
					SET @_IsActif = (SELECT IsActif FROM FRED_CODE_ZONE_DEPLACEMENT WHERE CodeZoneDeplacementId = @CodeZoneDeplacementId)
				END
				IF(@SocieteId IS NULL)
				BEGIN
					SET @SocieteId = (SELECT SocieteId FROM FRED_CODE_ZONE_DEPLACEMENT WHERE CodeZoneDeplacementId = @CodeZoneDeplacementId)
				END
		 -- REPRISE DES ANCIENNES VALEURS
			-- ---------------------------- 
			-- MISE A JOUR 
			-- ---------------------------- 
				UPDATE  FRED_CODE_ZONE_DEPLACEMENT
				SET
					Code= @Code, 
					Libelle= @Libelle, 
					
					DateModification= GETDATE(), 
					AuteurModification= 1, 
					IsActif= @_IsActif, 
					SocieteId= @SocieteId  
 		WHERE 
			CodeZoneDeplacementId= @CodeZoneDeplacementId
		IF @verbose = 1 
			BEGIN
				PRINT 'INFO : Mise à jour réalisée'
			END
		END
END
GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Code_Zone_Deplacement  POUR TABLE  FRED_CODE_ZONE_DEPLACEMENT
 -- ----------------------------------------------------------
