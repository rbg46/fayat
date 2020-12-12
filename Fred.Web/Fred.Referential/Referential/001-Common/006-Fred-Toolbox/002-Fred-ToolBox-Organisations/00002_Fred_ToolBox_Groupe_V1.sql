-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_GROUPE 
-- MODOP : 
--    EXEC Fred_ToolBox_Groupe @PoleCode='PFES2', @Code='GFES3',  @Libelle='Groupe FAYAT ENERGIES SERVICE', @verbose=1
-- -------------------------------------------------- 
 


IF OBJECT_ID ( 'Fred_ToolBox_Groupe', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Groupe; 
GO 
CREATE PROCEDURE Fred_ToolBox_Groupe

		@verbose INT =NULL,
		@version INT =NULL,
		@Libelle  varchar(max), 
		@Code  varchar(max),
		@PoleCode varchar(max)
		
 AS
 
		DECLARE @ERROR INT;
		SET @ERROR=0;

		IF @verbose = 1 
		BEGIN
				PRINT '------------------------------'
				PRINT 'FAYAT IT - 2018 '
				PRINT 'INJECTION DES FRED_GROUPE (PS Fred_ToolBox_Groupe)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 
 
 
		-- ----------------------------
		-- BEGIN FOREIGN KEYS

			DECLARE @PoleId INT;
			SET @PoleId = (SELECT PoleId FROM FRED_POLE WHERE Code = @PoleCode);
 
			-- Erreur si Pole non identifié
			 IF @PoleId IS NULL
			 BEGIN
				SET @ERROR = 1;
				PRINT 'ERREUR : Aucun Code Pole trouvé en base de données'
			 END

		-- END FOREIGN KEYS
		-- ----------------------------

		



-- RECHERCHE DE LA CLE ORGANISATION
DECLARE @GroupeId INT
SET @GroupeId= (SELECT GroupeId FROM FRED_GROUPE WHERE  Code = @Code)

IF @verbose = 1 
	BEGIN
		PRINT 'GROUPE ID >'
		PRINT @GroupeId
		PRINT 'GROUPE CODE >'
		PRINT @Code
	END


IF @ERROR =0
	BEGIN
	IF @GroupeId IS NULL
			BEGIN
				
				-- RECHERCHE DU POLE
				DECLARE @PoleOrganisationId INT;
				SET @PoleOrganisationId = (SELECT OrganisationId FROM FRED_POLE WHERE Code = @PoleCode);
				IF @verbose = 1 
				BEGIN
					PRINT 'Recherche Identifiant Organisation Pole >'
					PRINT @PoleOrganisationId
					PRINT 'Recherche Code Pole >'
					PRINT @Code
				END
				
				-- AJOUT DANS LA TABLE ORGANISATION
				DECLARE @GroupeOrganisationId INT;
				INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('3',@PoleOrganisationId); 
				SET @GroupeOrganisationId = @@IDENTITY;
				IF @verbose = 1 
				BEGIN
					PRINT 'Ajout Organisation avec ID suivant >'
					PRINT @GroupeOrganisationId
				END
				


				-- ---------------------------- 
				-- ORDRE AJOUT 
				-- ---------------------------- 
				INSERT INTO FRED_GROUPE 
					(
						Code, 
						Libelle, 
						PoleId, 
						OrganisationId  
					)
				VALUES 
					(
						@Code, 
						@Libelle, 
						@PoleId, 
						@GroupeOrganisationId  
					);

				IF @verbose = 1 
				BEGIN
					PRINT 'Ajout Groupe Dans la table FRED_GROUPE >'
					PRINT @@IDENTITY
				END
			END
			ELSE
			BEGIN
				IF @verbose = 1 
					BEGIN
						PRINT 'ERREUR : Désolé, le groupe existe déjà dans le système'
						
					END
			END
	END

	

GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Groupe  POUR TABLE  FRED_GROUPE
 -- ----------------------------------------------------------
