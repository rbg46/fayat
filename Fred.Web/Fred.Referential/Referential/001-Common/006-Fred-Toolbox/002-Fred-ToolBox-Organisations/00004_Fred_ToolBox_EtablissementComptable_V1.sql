
-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_ETABLISSEMENT_COMPTABLE 
-- MODOP : EXEC Fred_ToolBox_Etablissement_Comptable @GroupeCode='GFES2', @SocieteCode='FES3',  @Code='30',  @Libelle='SATELEC PIDF',  @Adresse='15, rue des Chantiers',  @Ville='MELUN',  @CodePostal='770000',  @ModuleCommandeEnabled='Oui',  @ModuleProductionEnabled='Oui',    @IsDeleted='Non',    @PaysCode='FR'  , @verbose=1, @version=1
-- -------------------------------------------------- 

 
IF OBJECT_ID ( 'Fred_ToolBox_Etablissement_Comptable', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Etablissement_Comptable; 
GO 
CREATE PROCEDURE Fred_ToolBox_Etablissement_Comptable
 
		@GroupeCode nvarchar(max),
		@verbose INT =NULL,
		@version INT =NULL,
		@SocieteCode  varchar(max), 
		@Code  nvarchar(20),
		@Libelle  nvarchar(500),
		@Adresse  nvarchar(500) =NULL,
		@Ville  nvarchar(500) =NULL,
		@CodePostal  nvarchar(20) =NULL,
		@ModuleCommandeEnabled  varchar(max) =NULL, 
		@ModuleProductionEnabled  varchar(max) =NULL, 
		@IsDeleted  varchar(max) =NULL, 
		@PaysCode  varchar(max)   =NULL
 AS
		DECLARE @ERROR INT;
		SET @ERROR =0;

		IF @verbose = 1 
		BEGIN
				PRINT '------------------------------'
				PRINT 'FAYAT IT - 2018 '
				PRINT 'INJECTION DES FRED_ETABLISSEMENT_COMPTABLE (PS Fred_ToolBox_Etablissement_Comptable)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 


		DECLARE @_ModuleCommandeEnabled  bit;
		DECLARE @_ModuleProductionEnabled  bit; 
		DECLARE @_IsDeleted  bit; 
		
		
		SET @_ModuleCommandeEnabled =(SELECT dbo.FredGetBoolean (@ModuleCommandeEnabled));
		SET @_ModuleProductionEnabled =(SELECT dbo.FredGetBoolean (@ModuleProductionEnabled));
		SET @_IsDeleted = (SELECT dbo.FredGetBoolean (@IsDeleted));
		IF @verbose = 1 
			BEGIN
				PRINT @_ModuleCommandeEnabled;
				PRINT @_ModuleProductionEnabled;
				PRINT @_IsDeleted; 
			END


 
		-- RECHERCHE SOCIETE
			DECLARE @SocieteId INT;
			SET @SocieteId= (SELECT SocieteId FROM FRED_SOCIETE Where Code = @SocieteCode AND GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = @GroupeCode));

			DECLARE @PaysId INT;

			IF(@PaysCode = 'FR')
			BEGIN 
					SET @PaysId= 1;
			END
			ELSE
			BEGIN
				SET @PaysId= (SELECT paysID FROM FRED_PAYS where Code = @PaysCode);
			END
			

			IF @version = 1 
			BEGIN
				PRINT 'INFO : ID SOCIETE'
				PRINT @SocieteId
				PRINT 'INFO : ID PAYS'
				PRINT @PaysId
			END
 
		-- RECHERCHE NULLITE LIBELLE DE L'ETABLISSEMENT
		IF (@Libelle ='' OR @Code ='')
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : Libellé ou Code non alimentée. Ajout annulé'
		END 

		
		-- CONTROLE SUR SOCIETE - SI CODE SOCIETE NON TROUVE > UNE ERREUR EST SOULEVEE
		IF(@SocieteId IS NULL)
		BEGIN
			SET @ERROR = 1;
			PRINT 'ERREUR : Code Société non identifié. Ajout annulé'
		END 


		DECLARE @EtablissementComptableId INT;
		SET @EtablissementComptableId = (SELECT EtablissementComptableId FROM FRED_ETABLISSEMENT_COMPTABLE WHERE Code = @Code AND SocieteId=@SocieteId)
		IF @verbose = 1 
			BEGIN
				PRINT 'INFO : ID ETB COMPTABLE'
				PRINT @EtablissementComptableId
			END


IF @ERROR = 0
BEGIN
	IF @EtablissementComptableId IS NULL
			BEGIN
				
				-- RECUPERATION DE LA CLE SOCIETE
				DECLARE @SocieteOrganisationId INT; 
				SET @SocieteOrganisationId = (SELECT organisationid FROM FRED_SOCIETE WHERE code = @SocieteCode)
	
				IF @version = 1 
				BEGIN
					PRINT 'INFO : ID ORGANISATION SOC'
					PRINT @SocieteOrganisationId
				END


				-- AJOUT DANS LA TABLE ORGANISATION
				DECLARE @OrganisationId INT;
				INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('7',@SocieteOrganisationId); 
				SET @OrganisationId = @@IDENTITY;

				IF @version = 1 
				BEGIN
					PRINT 'INFO : ID ORGANISATION ETB'
					PRINT @OrganisationId
				END


				-- ---------------------------- 
				-- ORDRE AJOUT 
				-- ---------------------------- 
				INSERT INTO FRED_ETABLISSEMENT_COMPTABLE (
					SocieteId, 
					Code, 
					Libelle, 
					Adresse, 
					Ville, 
					CodePostal, 
					ModuleCommandeEnabled, 
					ModuleProductionEnabled, 
					DateCreation, 
					AuteurCreationId, 
					IsDeleted, 
					OrganisationId, 
					PaysId  
				)
				VALUES 
				(
					@SocieteId, 
					@Code, 
					@Libelle, 
					@Adresse, 
					@Ville, 
					@CodePostal, 
					@_ModuleCommandeEnabled, 
					@_ModuleProductionEnabled, 
					GETDATE(), 
					1, 
					@_IsDeleted, 
					@OrganisationId, 
					@PaysId  
				);

				IF @version = 1 
				BEGIN
					PRINT 'INFO : ETB Ajouté'
				END

			END
		ELSE
			BEGIN
				-- ---------------------------- 
				-- MISE A JOUR 
				-- ---------------------------- 
					UPDATE  FRED_ETABLISSEMENT_COMPTABLE
					SET
						SocieteId= @SocieteId, 
						Code= @Code, 
						Libelle= @Libelle, 
						Adresse= @Adresse, 
						Ville= @Ville, 
						CodePostal= @CodePostal, 
						ModuleCommandeEnabled= @_ModuleCommandeEnabled, 
						ModuleProductionEnabled= @_ModuleProductionEnabled, 
						DateModification= GETDATE(), 
						AuteurModificationId= 1, 
						IsDeleted= @_IsDeleted, 
						PaysId= @PaysId  
 			WHERE 
				EtablissementComptableId= @EtablissementComptableId
			IF @verbose = 1 
				BEGIN
					PRINT 'Mise à jour réalisée'
				END
			END
END



GO
 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Etablissement_Comptable  POUR TABLE  FRED_ETABLISSEMENT_COMPTABLE
 -- ----------------------------------------------------------




