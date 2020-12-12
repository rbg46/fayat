-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_NATURE 
-- MODOP : 
--    EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFES',@SocieteCode='E001',@Achats='Oui',@RessourceCode='ENCA-15',@NatureCode='E001'





-- -------------------------------------------------- 

 
IF OBJECT_ID ( 'Fred_Toolbox_Nature_Ressources', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_Toolbox_Nature_Ressources; 
GO 
CREATE PROCEDURE Fred_Toolbox_Nature_Ressources
		@verbose INT =NULL,
		@version INT =NULL,
		@GroupeCode  nvarchar(50),
		@SocieteCode nvarchar(50),
		@RessourceCode  nvarchar(50),
		@NatureCode  nvarchar(500),
		@Achats nvarchar(Max)

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
				PRINT 'INJECTION DES FRED_SOCIETE_RESSOURCE_NATURE (PS Fred_Toolbox_Nature_Ressources)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 
		-- ----------------------------
		-- BEGIN OUI/NON
				DECLARE @_Achats bit;
			SET @_Achats =(SELECT dbo.FredGetBoolean (@Achats));
		-- END OUI/NON
		-- ----------------------------
 
 
		-- ----------------------------
		-- BEGIN FOREIGN KEYS
			-- VALIDATION SOCIETE
			DECLARE @SocieteId INT;
			SET @SocieteId =  (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId = (SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode))
			-- CONTROLE SUR SOCIETE - SI CODE SOCIETE NON TROUVE > UNE ERREUR EST SOULEVEE
			IF(@SocieteId IS NULL)
			BEGIN
				SET @ERROR = 1;
				PRINT 'ERREUR : Code Société non identifié'
			END 
			
			-- VALIDATION RESSOURCES
			DECLARE @RessourceID INT; 
			SET @RessourceID = 
							(
							SELECT FRED_RESSOURCE.RessourceId
							FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE
							WHERE FRED_SOUS_CHAPITRE.SousChapitreId IN 
								(
									SELECT FRED_SOUS_CHAPITRE.SousChapitreId 
									FROM FRED_SOUS_CHAPITRE, FRED_CHAPITRE 
									WHERE FRED_CHAPITRE.ChapitreId IN 
										(
										SELECT ChapitreId FROM FRED_CHAPITRE WHERE GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GFES'
										)
								AND FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId)
								AND FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
								)
								
							AND FRED_RESSOURCE.Code = 'ENCA-15'
							)
			
			IF(@RessourceID IS NULL)
			BEGIN
				SET @ERROR = 1;
				PRINT 'ERREUR : Code Ressources non identifié'
			END 
		

			-- VALIDATION NATURE
			DECLARE @NatureID INT; 
			SET @NatureID = (SELECT NatureID FROM FRED_NATURE Where Code = @NatureCode AND SocieteId = @SocieteId);
			IF(@NatureID IS NULL)
			BEGIN
				SET @ERROR = 1;
				PRINT 'ERREUR : Code Nature non identifié'
			END 


			DECLARE @ReferentielEtenduId INT;
			SET @ReferentielEtenduId = ( SELECT ReferentielEtenduId FROM FRED_SOCIETE_RESSOURCE_NATURE where RessourceId = @RessourceID AND NatureId = @NatureID)
			




		-- END FOREIGN KEYS
		-- ----------------------------




IF @ERROR = 0
BEGIN
IF @ReferentielEtenduId IS NULL
		BEGIN
			-- ---------------------------- 
			-- ORDRE AJOUT 
			-- ---------------------------- 
			INSERT INTO FRED_SOCIETE_RESSOURCE_NATURE (
				RessourceId,
				NatureId,
				Achats,
				SocieteId
			)
			VALUES 
			(
					@RessourceId,
					@NatureId,
					@_Achats,
					@SocieteId
			);
			IF @verbose = 1 
				BEGIN
					PRINT 'INFO : Ajout réalisée'
				END
			END
	ELSE
		BEGIN

				-- ---------------------------- 
				-- MISE A JOUR 
				-- ---------------------------- 
					UPDATE  FRED_SOCIETE_RESSOURCE_NATURE
					SET
						@RessourceId=@RessourceId,
						@NatureId=@NatureId 
 					WHERE 
						ReferentielEtenduId= @ReferentielEtenduId
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
