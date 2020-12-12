-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_CODE_ABSENCE 
-- MODOP : 
--    EXEC Fred_ToolBox_Code_Absence @HoldingCode=' ',  @SocieteCode='FESEE',  @Code='ANA',  @Libelle='--Absence non autorisée',  @Intemperie='Oui',  @TauxDecote='7',  @NBHeuresDefautETAM='8.4',  @NBHeuresMinETAM='1',  @NBHeuresMaxETAM='8.4',  @NBHeuresDefautCO='8.5',  @NBHeuresMinCO='8.5',  @NBHeuresMaxCO='8.5',  @Actif='Oui'  , @verbose=1
-- -------------------------------------------------- 
IF OBJECT_ID ( 'Fred_ToolBox_Code_Absence', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Code_Absence; 
GO 
CREATE PROCEDURE Fred_ToolBox_Code_Absence
 
		@GroupeCode nvarchar(max),
		@verbose INT =NULL,
		@version INT =NULL,
		@HoldingCode  varchar(max)=NULL, 
		@SocieteCode  varchar(max), 
		@Code  nvarchar(20),
		@Libelle  nvarchar(500),
		@Intemperie varchar(max)=NULL,
		@TauxDecote  float, 
		@NBHeuresDefautETAM  float, 
		@NBHeuresMinETAM  float, 
		@NBHeuresMaxETAM  float, 
		@NBHeuresDefautCO  float, 
		@NBHeuresMinCO  float, 
		@NBHeuresMaxCO  float, 
		@Actif varchar(max)=NULL
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
				PRINT 'INJECTION DES FRED_CODE_ABSENCE (PS Fred_ToolBox_Code_Absence)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 
		-- ----------------------------
		-- BEGIN OUI/NON
				DECLARE @_Intemperie bit;
			SET @_Intemperie =(SELECT dbo.FredGetBoolean (@Intemperie));
				DECLARE @_Actif bit;
			SET @_Actif =(SELECT dbo.FredGetBoolean (@Actif));
		-- END OUI/NON
		-- ----------------------------

 
		-- ----------------------------
		-- END FOREIGN KEYS
			DECLARE @HoldingId INT;
			--SET @HoldingId= dbo.FredIdFromCode('HoldingId',  @HoldingCode )
 
			DECLARE @SocieteId INT;
			SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode))
 
		-- END FOREIGN KEYS
		-- ----------------------------
 
		-- CONTROLE SUR SOCIETE
		IF(@SocieteId IS NULL)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : Code Société non identifié'
		END 

		-- RECUPERATION DE LA CLE PRIMAIRE EN FONCTION DU CODE ET DE LA SOCIETE
		DECLARE @CodeAbsenceId INT
		SET @CodeAbsenceId= 
				(
				SELECT CodeAbsenceId FROM FRED_CODE_ABSENCE WHERE code = @code  AND SocieteId = @SocieteId
				)


IF @ERROR = 0
BEGIN
IF @CodeAbsenceId IS NULL
		BEGIN
			-- ---------------------------- 
			-- ORDRE AJOUT 
			-- ---------------------------- 
			INSERT INTO FRED_CODE_ABSENCE (
				HoldingId, 
				SocieteId, 
				Code, 
				Libelle, 
				Intemperie, 
				TauxDecote, 
				NBHeuresDefautETAM, 
				NBHeuresMinETAM, 
				NBHeuresMaxETAM, 
				NBHeuresDefautCO, 
				NBHeuresMinCO, 
				NBHeuresMaxCO, 
				Actif  
		)
		VALUES 
		(
				@HoldingId, 
				@SocieteId, 
				@Code, 
				@Libelle, 
				@_Intemperie, 
				@TauxDecote, 
				@NBHeuresDefautETAM, 
				@NBHeuresMinETAM, 
				@NBHeuresMaxETAM, 
				@NBHeuresDefautCO, 
				@NBHeuresMinCO, 
				@NBHeuresMaxCO, 
				@_Actif  
		);
		END

		IF @version = 1 
		BEGIN
			PRINT 'Ajout Code Absence '
		END

	ELSE
		BEGIN
 -- REPRISE DES ANCIENNES VALEURS
		
		IF(@HoldingId IS NULL)
		BEGIN
			SET @HoldingId = (SELECT HoldingId FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@SocieteId IS NULL)
		BEGIN
			SET @SocieteId = (SELECT SocieteId FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@Code IS NULL)
		BEGIN
			SET @Code = (SELECT Code FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@Libelle IS NULL)
		BEGIN
			SET @Libelle = (SELECT Libelle FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@Intemperie IS NULL)
			BEGIN
				SET @Intemperie = (SELECT Intemperie FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
			END
		ELSE
			BEGIN
				SET @Intemperie = @_Intemperie
			END

		IF(@TauxDecote IS NULL)
		BEGIN
			SET @TauxDecote = (SELECT TauxDecote FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@NBHeuresDefautETAM IS NULL)
		BEGIN
			SET @NBHeuresDefautETAM = (SELECT NBHeuresDefautETAM FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@NBHeuresMinETAM IS NULL)
		BEGIN
			SET @NBHeuresMinETAM = (SELECT NBHeuresMinETAM FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@NBHeuresMaxETAM IS NULL)
		BEGIN
			SET @NBHeuresMaxETAM = (SELECT NBHeuresMaxETAM FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@NBHeuresDefautCO IS NULL)
		BEGIN
			SET @NBHeuresDefautCO = (SELECT NBHeuresDefautCO FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@NBHeuresMinCO IS NULL)
		BEGIN
			SET @NBHeuresMinCO = (SELECT NBHeuresMinCO FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@NBHeuresMaxCO IS NULL)
		BEGIN
			SET @NBHeuresMaxCO = (SELECT NBHeuresMaxCO FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
		END
		
		IF(@Actif IS NULL)
			BEGIN
				SET @Actif = (SELECT Actif FROM FRED_CODE_ABSENCE WHERE CodeAbsenceId = @CodeAbsenceId)
			END
			ELSE
			BEGIN
				SET @Actif = @_Actif
			END
 -- REPRISE DES ANCIENNES VALEURS
			-- ---------------------------- 
			-- MISE A JOUR 
			-- ---------------------------- 
				UPDATE  FRED_CODE_ABSENCE
				SET
					HoldingId= @HoldingId, 
					SocieteId= @SocieteId, 
					Code= @Code, 
					Libelle= @Libelle, 
					Intemperie= @Intemperie, 
					TauxDecote= @TauxDecote, 
					NBHeuresDefautETAM= @NBHeuresDefautETAM, 
					NBHeuresMinETAM= @NBHeuresMinETAM, 
					NBHeuresMaxETAM= @NBHeuresMaxETAM, 
					NBHeuresDefautCO= @NBHeuresDefautCO, 
					NBHeuresMinCO= @NBHeuresMinCO, 
					NBHeuresMaxCO= @NBHeuresMaxCO, 
					Actif= @Actif  
 		WHERE 
			CodeAbsenceId= @CodeAbsenceId
		IF @verbose = 1 
			BEGIN
				PRINT 'Mise à jour réalisée'
			END
		END
END
GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Code_Absence  POUR TABLE  FRED_CODE_ABSENCE
 -- ----------------------------------------------------------
