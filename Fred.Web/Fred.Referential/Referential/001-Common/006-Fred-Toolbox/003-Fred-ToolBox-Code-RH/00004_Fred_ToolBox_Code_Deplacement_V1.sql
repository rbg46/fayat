-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_CODE_DEPLACEMENT 
-- MODOP : 
--    EXEC Fred_ToolBox_Code_Deplacement @Code=' ',  @Libelle=' ',  @KmMini=' ',  @KmMaxi=' ',  @IGD=' ',  @IndemniteForfaitaire=' ',  @Actif=' ',  @SocieteCode=' '  
-- -------------------------------------------------- 
--EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES2',@SocieteCode='FES3',@Code='Z1',@Libelle='Zone 12',@KmMini='999',@KmMaxi='1000',@IGD='Oui',@IndemniteForfaitaire='Non'

-- SELECT * FROM FRED_CODE_DEPLACEMENT

IF OBJECT_ID ( 'Fred_ToolBox_Code_Deplacement', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Code_Deplacement; 
GO 
CREATE PROCEDURE Fred_ToolBox_Code_Deplacement
 
		@GroupeCode  nvarchar(500),
		@verbose INT =NULL,
		@version INT =NULL,
		@Code  nvarchar(20),
		@Libelle  nvarchar(500),
		@KmMini  int, 
		@KmMaxi  int, 
		@IGD varchar(max),
		@IndemniteForfaitaire varchar(max)=NULL,
		@Actif varchar(max)=NULL,
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
				PRINT 'INJECTION DES FRED_CODE_DEPLACEMENT (PS Fred_ToolBox_Code_Deplacement)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 
		-- ----------------------------
		-- BEGIN OUI/NON
				DECLARE @_IGD bit;
			SET @_IGD =(SELECT dbo.FredGetBoolean (@IGD));
				DECLARE @_IndemniteForfaitaire bit;
			SET @_IndemniteForfaitaire =(SELECT dbo.FredGetBoolean (@IndemniteForfaitaire));
				DECLARE @_Actif bit;
			SET @_Actif =(SELECT dbo.FredGetBoolean (@Actif));
		-- END OUI/NON
		-- ----------------------------
 
 
		-- ----------------------------
		-- END FOREIGN KEYS
			DECLARE @SocieteId INT;
			SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode))
		-- END FOREIGN KEYS
		-- ----------------------------

		-- CONTROLE SUR SOCIETE - SI CODE SOCIETE NON TROUVE > UNE ERREUR EST SOULEVEE
		IF(@SocieteId IS NULL)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : Code Société non identifié'
		END 


		 -- RECUPERATION DE LA CLE PRIMAIRE EN FONCTION DU CODE ET DE LA SOCIETE
		DECLARE @CodeDeplacementId INT
		SET @CodeDeplacementId= 
				(
				SELECT CodeDeplacementId FROM FRED_CODE_DEPLACEMENT WHERE code = @code  AND SocieteId = @SocieteId
				)


IF @ERROR = 0
BEGIN
IF @CodeDeplacementId IS NULL
		BEGIN
			-- ---------------------------- 
			-- ORDRE AJOUT 
			-- ---------------------------- 
			INSERT INTO FRED_CODE_DEPLACEMENT (
				Code, 
				Libelle, 
				KmMini, 
				KmMaxi, 
				IGD, 
				IndemniteForfaitaire, 
				Actif, 
				SocieteId  
		)
		VALUES 
		(
				@Code, 
				@Libelle, 
				@KmMini, 
				@KmMaxi, 
				@_IGD, 
				@_IndemniteForfaitaire, 
				@_Actif, 
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
			SET @Code = (SELECT Code FROM FRED_CODE_DEPLACEMENT WHERE CodeDeplacementId = @CodeDeplacementId)
		END
		IF(@Libelle IS NULL)
		BEGIN
			SET @Libelle = (SELECT Libelle FROM FRED_CODE_DEPLACEMENT WHERE CodeDeplacementId = @CodeDeplacementId)
		END
		IF(@KmMini IS NULL)
		BEGIN
			SET @KmMini = (SELECT KmMini FROM FRED_CODE_DEPLACEMENT WHERE CodeDeplacementId = @CodeDeplacementId)
		END
		IF(@KmMaxi IS NULL)
		BEGIN
			SET @KmMaxi = (SELECT KmMaxi FROM FRED_CODE_DEPLACEMENT WHERE CodeDeplacementId = @CodeDeplacementId)
		END
		IF(@IGD IS NULL)
		BEGIN
			SET @_IGD = (SELECT IGD FROM FRED_CODE_DEPLACEMENT WHERE CodeDeplacementId = @CodeDeplacementId)
		END
		IF(@IndemniteForfaitaire IS NULL)
		BEGIN
			SET @_IndemniteForfaitaire = (SELECT IndemniteForfaitaire FROM FRED_CODE_DEPLACEMENT WHERE CodeDeplacementId = @CodeDeplacementId)
		END
		IF(@Actif IS NULL)
		BEGIN
			SET @_Actif = (SELECT Actif FROM FRED_CODE_DEPLACEMENT WHERE CodeDeplacementId = @CodeDeplacementId)
		END
		IF(@SocieteId IS NULL)
		BEGIN
			SET @SocieteId = (SELECT SocieteId FROM FRED_CODE_DEPLACEMENT WHERE CodeDeplacementId = @CodeDeplacementId)
		END
 -- REPRISE DES ANCIENNES VALEURS
			-- ---------------------------- 
			-- MISE A JOUR 
			-- ---------------------------- 
				UPDATE  FRED_CODE_DEPLACEMENT
				SET
					Code= @Code, 
					Libelle= @Libelle, 
					KmMini= @KmMini, 
					KmMaxi= @KmMaxi, 
					IGD= @_IGD, 
					IndemniteForfaitaire= @_IndemniteForfaitaire, 
					Actif= @_Actif, 
					SocieteId= @SocieteId  
 		WHERE 
			CodeDeplacementId= @CodeDeplacementId
		IF @verbose = 1 
			BEGIN
				PRINT 'INFO : Mise à jour réalisée'
			END
		END
END
GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Code_Deplacement  POUR TABLE  FRED_CODE_DEPLACEMENT
 -- ----------------------------------------------------------
