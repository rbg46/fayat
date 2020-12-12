-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_ROLE 
-- MODOP : 
--    EXEC Fred_ToolBox_Role   @Code=' ',  @Libelle=' ',  @CommandeSeuilDefaut=' ',  @ModeLecture=' ',  @Actif=' ',  @SuperAdmin=' ',  @NiveauPaie=' ',  @NiveauCompta=' ',  @Description=' ',  @SocieteCode=' ',  @CodeNomFamilier=' ',  @Specification=' '  
-- -------------------------------------------------- 
 

 --EXEC Fred_ToolBox_Role  @GroupeCode='GFTP', @Code='ADM',  @Libelle='Admin',  @CommandeSeuilDefaut='99999',  @ModeLecture='1',  @Actif='1',  @SuperAdmin='1',  @NiveauPaie='9',  @NiveauCompta='9',  @Description='Rôle pour Admin',  @SocieteCode='0143',  @CodeNomFamilier='ADm',  @Specification=1  
 SELECT * FROM FRED_ROLE WHERE SocieteId= (SELECT SocieteId FROM FRED_SOCIETE WHERE code = '0143' AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = 'GFTP'))
IF OBJECT_ID ( 'Fred_ToolBox_Role', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Role; 
GO 
CREATE PROCEDURE Fred_ToolBox_Role
 		@GroupeCode varchar(max),
		@verbose INT =NULL,
		@version INT =NULL,
		@Code  varchar(max), 
		@Libelle  varchar(max), 
		@CommandeSeuilDefaut  varchar(max), 
		@ModeLecture varchar(max),
		@Actif varchar(max),
		@NiveauPaie  int, 
		@NiveauCompta  int, 
		@Description  nvarchar(max),
		@SocieteCode  varchar(max), 
		@CodeNomFamilier  nvarchar(max),
		@Specification  int  
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
 
		-- ----------------------------
		-- Convertion pour les marqueurs ModeLecture, Actif
				DECLARE @_ModeLecture bit;
			SET @_ModeLecture =(SELECT dbo.FredGetBoolean (@ModeLecture));
				DECLARE @_Actif bit;
			SET @_Actif =(SELECT dbo.FredGetBoolean (@Actif));
		-- END OUI/NON
		-- ----------------------------
 
 
		-- Recherche de la société et groupe
			DECLARE @SocieteId INT;
			SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode));
			IF(@SocieteId IS NULL)
			BEGIN
				SET @ERROR = 1;
				PRINT 'ERROR : Société non identifié. Ajout Rôle annulé'
			END 

		-- Recherche pour savoir si un Code + Société Existe déjà
			DECLARE @RoleId INT
			SET @RoleId= (SELECT RoleId FROM FRED_ROLE WHERE code = @code and SocieteId = @SocieteId)



IF @ERROR = 0
	BEGIN
		IF @RoleId IS NULL
				BEGIN
					-- ---------------------------- 
					-- ORDRE AJOUT 
					-- ---------------------------- 
					INSERT INTO FRED_ROLE (
						Code, 
						Libelle, 
						CommandeSeuilDefaut, 
						ModeLecture, 
						Actif, 
						NiveauPaie, 
						NiveauCompta, 
						Description, 
						SocieteId, 
						CodeNomFamilier, 
						Specification  
				)
				VALUES 
				(
						@Code, 
						@Libelle, 
						@CommandeSeuilDefaut, 
						@ModeLecture, 
						@_Actif, 
						@NiveauPaie, 
						@NiveauCompta, 
						@Description, 
						@SocieteId, 
						@CodeNomFamilier, 
						@Specification  
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
							SET @Code = (SELECT Code FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@Libelle IS NULL)
						BEGIN
							SET @Libelle = (SELECT Libelle FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@CommandeSeuilDefaut IS NULL)
						BEGIN
							SET @CommandeSeuilDefaut = (SELECT CommandeSeuilDefaut FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@ModeLecture IS NULL)
						BEGIN
							SET @ModeLecture = (SELECT ModeLecture FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@Actif IS NULL)
						BEGIN
							SET @Actif = (SELECT Actif FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@NiveauPaie IS NULL)
						BEGIN
							SET @NiveauPaie = (SELECT NiveauPaie FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@NiveauCompta IS NULL)
						BEGIN
							SET @NiveauCompta = (SELECT NiveauCompta FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@Description IS NULL)
						BEGIN
							SET @Description = (SELECT Description FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@SocieteId IS NULL)
						BEGIN
							SET @SocieteId = (SELECT SocieteId FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@CodeNomFamilier IS NULL)
						BEGIN
							SET @CodeNomFamilier = (SELECT CodeNomFamilier FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
						IF(@Specification IS NULL)
						BEGIN
							SET @Specification = (SELECT Specification FROM FRED_ROLE WHERE RoleId = @RoleId)
						END
				 -- REPRISE DES ANCIENNES VALEURS
					-- ---------------------------- 
					-- MISE A JOUR 
					-- ---------------------------- 
						UPDATE  FRED_ROLE
						SET
							Code= @Code, 
							Libelle= @Libelle, 
							CommandeSeuilDefaut= @CommandeSeuilDefaut, 
							ModeLecture= @_ModeLecture, 
							Actif= @_Actif, 
							NiveauPaie= @NiveauPaie, 
							NiveauCompta= @NiveauCompta, 
							Description= @Description, 
							SocieteId= @SocieteId, 
							CodeNomFamilier= @CodeNomFamilier, 
							Specification= @Specification  
 				WHERE 
					RoleId= @RoleId
				IF @verbose = 1 
					BEGIN
						PRINT 'INFO : Mise à jour réalisée'
					END
				END
	END
GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Role  POUR TABLE  FRED_ROLE
 -- ----------------------------------------------------------
