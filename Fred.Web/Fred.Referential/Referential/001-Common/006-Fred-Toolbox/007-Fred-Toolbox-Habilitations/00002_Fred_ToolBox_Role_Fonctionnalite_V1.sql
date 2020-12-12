

-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX INJECTION D'UNE ASSOCIATION ROLE et FONCTIONNALITE
-- MODOP : 
--    Extraction via EXEC Fred_ToolBox_Extract_Role_Fonctionnalite @SocieteCode='0143', @GroupeCode='GFTP'
-- -------------------------------------------------- 
 
IF OBJECT_ID ( 'Fred_ToolBox_Role_Fonctionnalite', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Role_Fonctionnalite; 
GO 
CREATE PROCEDURE Fred_ToolBox_Role_Fonctionnalite
 		@ModuleId varchar(max),
		@ModuleCode varchar(max),
		@RoleCodeNomFamilier varchar(max),
		@RoleCode varchar(max),
		@Mode INT,
		@GroupeCode varchar(max),
		@SocieteCode  varchar(max),
		@verbose INT = NULL,
		@version INT = NULL
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
				PRINT 'INJECTION DES FRED_ROLE (PS Fred_ToolBox_Role)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 
		
		-- RECUPERATON DU CODE SOCIETE
		DECLARE @SocieteId INT;
		SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode));
		IF(@SocieteId IS NULL)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERROR : Société non identifié. Ajout Rôle annulé'
		END 


		PRINT @SocieteId
		-- RECUPERATION DU ROLE ID
		-- CONTROLE COUNT
		DECLARE @RoleCount INT;
		SET @RoleCount =  (SELECT COUNT(RoleId)  FROM FRED_ROLE WHERE CodeNomFamilier = @RoleCodeNomFamilier AND Code = @RoleCode AND SocieteId = @SocieteId )
		IF(@RoleCount > 1)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERROR : ATTENTION Role  non unique'
		END


		IF @ERROR = 0
		BEGIN
			DECLARE @RoleId INT;
			SET @RoleId =  (SELECT RoleId  FROM FRED_ROLE WHERE CodeNomFamilier = @RoleCodeNomFamilier AND Code = @RoleCode AND SocieteId = @SocieteId )
			IF(@RoleId IS NULL)
			BEGIN
				SET @ERROR = 1;
				PRINT 'ERROR : Rôle non identifié. Ajout Rôle annulé'
			END 
		END


		-- RECUPERATION DU FONCTIONNALITE ID
		DECLARE @FonctionnaliteId INT;
		SET @FonctionnaliteId = (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE Where ModuleId = @ModuleId AND Code  = @ModuleCode)
		IF(@FonctionnaliteId IS NULL)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERROR : Fonctionnalité non identifié. Ajout Rôle annulé'
		END 



		-- CHECK POUR EVITER D'INJECTER DEUX FOIS LA REFERENCE
		
		PRINT '@RoleId : ' +  CONVERT(nvarchar, @RoleId)
		PRINT '@FonctionnaliteId : ' +  CONVERT(nvarchar, @FonctionnaliteId)
		PRINT '@Mode : ' +  CONVERT(nvarchar, @Mode)


		DECLARE @Check INT =(
					SELECT COUNT(*) FROM FRED_ROLE_FONCTIONNALITE WHERE
					RoleId = @RoleId
					AND FonctionnaliteId = @FonctionnaliteId
					)
		IF (@Check <> 0)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERROR : Association Existante. Pas de Mise à jour possible'
		END


IF @ERROR = 0
	BEGIN
		IF @Check = 0
		BEGIN
				INSERT INTO FRED_ROLE_FONCTIONNALITE(RoleId, FonctionnaliteId, Mode)
				VALUES (@RoleId, @FonctionnaliteId,  @Mode)
				IF @verbose = 1 
					BEGIN
						PRINT 'INFO : Ajout réalisée'
					END
				PRINT 'Association Ajoutée'
			END
		ELSE
			BEGIN
				PRINT 'Association Déjà existante'
			END

	END
GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Role  POUR TABLE  FRED_ROLE
 -- ----------------------------------------------------------