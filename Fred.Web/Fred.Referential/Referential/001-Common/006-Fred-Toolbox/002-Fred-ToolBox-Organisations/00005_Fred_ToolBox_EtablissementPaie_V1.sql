-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_ETABLISSEMENT_PAIE 
-- MODOP : 
--    EXEC Fred_ToolBox_Etablissement_Paie @SocieteCode='FES3',	@AgenceRattachementCode='75',	@Code='1215',	@Libelle='SATELEC DG',	@Adresse='24 ave du Général de Gaulle, 91170 VIRY-CHATILLON',	@PaysCode='FR',  @Ville='VILLEPARISIS',  @CodePostal='93502', @verbose=1
-- -------------------------------------------------- 


IF OBJECT_ID ( 'Fred_ToolBox_Etablissement_Paie', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Etablissement_Paie; 
GO 
CREATE PROCEDURE Fred_ToolBox_Etablissement_Paie
 
		@GroupeCode nvarchar(max),
		@verbose INT =NULL,
		@version INT =NULL,
		@Code  nvarchar(20),
		@Libelle  nvarchar(500)=NULL,
		@Adresse  nvarchar(500)=NULL,
		@Latitude  float =NULL, 
		@Longitude  float =NULL, 
		@IsAgenceRattachement varchar(max) =NULL,
		@AgenceRattachementCode  varchar(max) =NULL, 
		@GestionIndemnites varchar(max) =NULL,
		@HorsRegion varchar(max) =NULL,
		@Actif varchar(max) =NULL,
		@SocieteCode  varchar(max) =NULL, 
		@Adresse2  nvarchar(500) =NULL,
		@Adresse3  nvarchar(500) =NULL,
		@Ville  nvarchar(500) =NULL,
		@CodePostal  nvarchar(500) =NULL,
		@PaysCode  varchar(max)   =NULL

 AS


 		-- ----------------------------
		-- BEGIN OUI/NON
			DECLARE @_IsAgenceRattachement bit;
			SET @_IsAgenceRattachement =(SELECT dbo.FredGetBoolean (@IsAgenceRattachement));

			DECLARE @_GestionIndemnites bit;
			SET @_GestionIndemnites =(SELECT dbo.FredGetBoolean (@GestionIndemnites));

			DECLARE @_HorsRegion bit;
			SET @_HorsRegion =(SELECT dbo.FredGetBoolean (@HorsRegion));

			DECLARE @_Actif bit;
			SET @_Actif =(SELECT dbo.FredGetBoolean (@Actif));

		-- END OUI/NON
		-- ----------------------------


DECLARE @ERROR INT;
SET @ERROR =0;
		IF @verbose = 1 
		BEGIN
				PRINT '------------------------------'
				PRINT 'FAYAT IT - 2018 '
				PRINT 'INJECTION DES FRED_ETABLISSEMENT_PAIE (PS Fred_ToolBox_Etablissement_Paie)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 
 
 
		-- ----------------------------
		-- END FOREIGN KEYS
			--DECLARE @AgenceRattachementId INT;
			--SET @AgenceRattachementId= dbo.FredIdFromCode('AgenceRattachementId',  @AgenceRattachementCode )
 
			DECLARE @SocieteId INT;
			SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE Where Code = @SocieteCode AND GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = @GroupeCode));
			

		-- GESTION DU CODE FR reproduit 2 Fois en DEV
		DECLARE @Paysid INT;
		IF(@PaysCode = 'FR')
		BEGIN 
			SET @PaysId= 1;
		END
		ELSE
		BEGIN
			SET @PaysId= (SELECT paysID FROM FRED_PAYS where Code = @PaysCode);
		END
 

		-- Recherche Etablissement de PAIE
		DECLARE @EtablissementPaieId INT;
		SET @EtablissementPaieId= (SELECT EtablissementPaieId FROM FRED_ETABLISSEMENT_PAIE WHERE Code = @Code AND SocieteId = @SocieteId);
 
		IF (@Libelle = '')
		BEGIN
			SET @Libelle = 'Etablissement de Paie de la societe '+ @SocieteCode
		END


		IF @EtablissementPaieId IS NULL
				BEGIN
					-- ---------------------------- 
					-- ORDRE AJOUT 
					-- ---------------------------- 
					INSERT INTO FRED_ETABLISSEMENT_PAIE (
						Code, 
						Libelle, 
						Adresse, 
						Latitude, 
						Longitude, 
						IsAgenceRattachement, 
						--AgenceRattachementId, 
						GestionIndemnites, 
						HorsRegion, 
						Actif, 
						SocieteId, 
						Adresse2, 
						Adresse3, 
						Ville, 
						CodePostal, 
						PaysId  
				)
				VALUES 
				(
						@Code, 
						@Libelle, 
						@Adresse, 
						@Latitude, 
						@Longitude, 
						@_IsAgenceRattachement, 
						--@AgenceRattachementId, 
						@_GestionIndemnites, 
						@_HorsRegion, 
						@_Actif, 
						@SocieteId, 
						@Adresse2, 
						@Adresse3, 
						@Ville, 
						@CodePostal, 
						@PaysId  
				);
				END
			ELSE
				BEGIN
					-- ---------------------------- 
					-- MISE A JOUR 
					-- ---------------------------- 
						UPDATE  FRED_ETABLISSEMENT_PAIE
						SET
							Code= @Code, 
							Libelle= @Libelle, 
							Adresse= @Adresse, 
							Latitude= @Latitude, 
							Longitude= @Longitude, 
							IsAgenceRattachement= @_IsAgenceRattachement, 
							--AgenceRattachementId= @AgenceRattachementId, 
							GestionIndemnites= @_GestionIndemnites, 
							HorsRegion= @_HorsRegion, 
							Actif= @_Actif, 
							SocieteId= @SocieteId, 
							Adresse2= @Adresse2, 
							Adresse3= @Adresse3, 
							Ville= @Ville, 
							CodePostal= @CodePostal, 
							PaysId= @PaysId  
 				WHERE 
					EtablissementPaieId= @EtablissementPaieId
				IF @verbose = 1 
					BEGIN
						PRINT 'Mise à jour réalisée pour etablissement ID'+CAST(@EtablissementPaieId as varchar(max))
					END
				END

GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Etablissement_Paie  POUR TABLE  FRED_ETABLISSEMENT_PAIE
 -- ----------------------------------------------------------
