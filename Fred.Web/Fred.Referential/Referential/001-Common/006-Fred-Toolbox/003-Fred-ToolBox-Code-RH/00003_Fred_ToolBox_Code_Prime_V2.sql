-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_PRIME 
-- MODOP : 
--    EXEC Fred_ToolBox_Code_Prime @Code=' ',  @Libelle=' ',  @NombreHeuresMax=' ',  @Actif=' ',  @PrimePartenaire=' ',  @Publique=' ',  @SocieteCode=' ',  @DateCreation=' ',  @DateModification=' ',  @DateSuppression=' ',  @AuteurCreationCode=' ',  @AuteurModificationCode=' ',  @AuteurSuppressionId=' ',  @PrimeType=' '  
--  CHNAGE : Type PRime par défaut à 0
-- -------------------------------------------------- 

IF OBJECT_ID ( 'Fred_ToolBox_Code_Prime', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Code_Prime; 
GO 
CREATE PROCEDURE Fred_ToolBox_Code_Prime
		@GroupeCode nvarchar(max),
		@verbose INT =NULL,
		@version INT =NULL,
		@Code  nvarchar(20),
		@Libelle  nvarchar(500),
		@NombreHeuresMax  float=NULL, 
		@Actif varchar(max)=NULL,
		@PrimePartenaire varchar(max)=NULL,
		@Publique varchar(max),
		@SocieteCode  varchar(max), 
		@DateCreation  datetime=NULL, 
		@DateModification  datetime=NULL, 
		@DateSuppression  datetime=NULL, 
		@AuteurCreationCode  varchar(max)=NULL, 
		@AuteurModificationCode  varchar(max)=NULL, 
		@AuteurSuppressionCode  int =NULL, 
		@PrimeType  varchar(max)  =NULL
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
				PRINT 'INJECTION DES FRED_PRIME (PS Fred_ToolBox_Code_Prime)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.2'
		END
 
		-- ----------------------------
		-- BEGIN OUI/NON
			DECLARE @_Actif bit;
			SET @_Actif =(SELECT dbo.FredGetBoolean (@Actif));
			DECLARE @_PrimePartenaire bit;
			SET @_PrimePartenaire =(SELECT dbo.FredGetBoolean (@PrimePartenaire));
			DECLARE @_Publique bit;
			SET @_Publique =(SELECT dbo.FredGetBoolean (@Publique));
		-- END OUI/NON
		-- ----------------------------
 
 
		-- ----------------------------
		-- END FOREIGN KEYS
			DECLARE @SocieteId INT;
			SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode))
			
			DECLARE @AuteurCreationId INT;
			DECLARE @AuteurSuppressionId INT;
			DECLARE @AuteurModificationId INT;
			
		-- END FOREIGN KEYS
		-- ----------------------------

		-- TYPE DE PRIME
		DECLARE @PrimeTypeId INT = 0;
		IF(UPPER(@PrimeType) = 'JOURNALIER')
		BEGIN
			SET @PrimeTypeId = 0;
	 	END

		IF(UPPER(@PrimeType) = 'HORAIRE')
		BEGIN
			SET @PrimeTypeId = 2;
		END

		IF(UPPER(@PrimeType) = 'MENSUELLE')
		BEGIN
			SET @PrimeTypeId = 2;
		END

 
		-- CONTROLE SUR SOCIETE - SI CODE SOCIETE NON TROUVE > UNE ERREUR EST SOULEVEE
		IF(@SocieteId IS NULL)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : Code Société non identifié'
		END 

		-- RECUPERATION DE LA CLE PRIMAIRE EN FONCTION DU CODE ET DE LA SOCIETE
		DECLARE @PrimeId INT
		SET @PrimeId= 
				(
				SELECT PrimeId FROM FRED_PRIME WHERE code = @code AND SocieteId = @SocieteId
				)


IF @ERROR = 0
BEGIN
IF @PrimeId IS NULL
		BEGIN
			-- ---------------------------- 
			-- ORDRE AJOUT 
			-- ---------------------------- 
			INSERT INTO FRED_PRIME (
				Code, 
				Libelle, 
				NombreHeuresMax, 
				Actif, 
				PrimePartenaire, 
				Publique, 
				SocieteId, 
				DateCreation, 
				AuteurCreationId, 
				PrimeType  
		)
		VALUES 
		(
				@Code, 
				@Libelle, 
				@NombreHeuresMax, 
				@_Actif, 
				@_PrimePartenaire, 
				@_Publique, 
				@SocieteId, 
				GETDATE(),  
				1, 
				@PrimeTypeId  
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
			SET @Code = (SELECT Code FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@Libelle IS NULL)
		BEGIN
			SET @Libelle = (SELECT Libelle FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@NombreHeuresMax IS NULL)
		BEGIN
			SET @NombreHeuresMax = (SELECT NombreHeuresMax FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@Actif IS NULL)
		BEGIN
			SET @_Actif = (SELECT Actif FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@PrimePartenaire IS NULL)
		BEGIN
			SET @_PrimePartenaire = (SELECT PrimePartenaire FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@Publique IS NULL)
		BEGIN
			SET @_Publique = (SELECT Publique FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		ELSE
		
		IF(@SocieteId IS NULL)
		BEGIN
			SET @SocieteId = (SELECT SocieteId FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@DateCreation IS NULL)
		BEGIN
			SET @DateCreation = (SELECT DateCreation FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@DateModification IS NULL)
		BEGIN
			SET @DateModification = (SELECT DateModification FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@DateSuppression IS NULL)
		BEGIN
			SET @DateSuppression = (SELECT DateSuppression FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@AuteurCreationId IS NULL)
		BEGIN
			SET @AuteurCreationId = (SELECT AuteurCreationId FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@AuteurModificationId IS NULL)
		BEGIN
			SET @AuteurModificationId = (SELECT AuteurModificationId FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@AuteurSuppressionId IS NULL)
		BEGIN
			SET @AuteurSuppressionId = (SELECT AuteurSuppressionId FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
		IF(@PrimeType IS NULL)
		BEGIN
			SET @PrimeTypeId = (SELECT PrimeType FROM FRED_PRIME WHERE PrimeId = @PrimeId)
		END
 -- REPRISE DES ANCIENNES VALEURS
			-- ---------------------------- 
			-- MISE A JOUR 
			-- ---------------------------- 
				UPDATE  FRED_PRIME
				SET
					Code= @Code, 
					Libelle= @Libelle, 
					NombreHeuresMax= @NombreHeuresMax, 
					Actif= @_Actif, 
					PrimePartenaire= @_PrimePartenaire, 
					Publique= @_Publique, 
					SocieteId= @SocieteId, 
					DateModification=GETDATE(), 
					AuteurModificationId= 1, 
					PrimeType= @PrimeTypeId  
 		WHERE 
			PrimeId= @PrimeId
		IF @verbose = 1 
			BEGIN
				PRINT 'INFO : Mise à jour réalisée'
			END
		END
END
GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Code_Prime  POUR TABLE  FRED_PRIME
 -- ----------------------------------------------------------
