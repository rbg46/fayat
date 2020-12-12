


/*Ajout d'une Ressources*/
-- --------------------------------------------------
-- FRED 2017 - R4 - SEPTEMBRE 2018 
-- TOOLBOX INJECTION RESSOURCES
-- MODOP :  EXEC Fred_ToolBox_Ressources  @GroupeCode='GFTP',@SousChapitreCode='10-900',@Code='ENCA-01',@Libelle='DIRECTEUR DE PROJET',@Type='Personnel', @verbose=1
--				EXEC Fred_ToolBox_Check_Ressources_ByCode @GroupeCode ='GFTP', @Code = 'ENCA-01'
-- CHANGE : 0.2 - Ajout des champs pour transport des données
-- --------------------------------------------------
IF OBJECT_ID ( 'Fred_ToolBox_Ressources', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_Ressources;  
GO  
CREATE PROCEDURE Fred_ToolBox_Ressources 
		@GroupeCode varchar(500),
		@SousChapitreCode varchar(500),
		@Code varchar(50),
		@Libelle varchar(500),
		@Type nvarchar(500)  = NULL,
		@TypeRessourceId int  = NULL,
		@Active int  = NULL,
		@CarburantId int  = NULL,
		@Consommation int  = NULL,
		@Version bit = NULL, 
		@verbose bit = NULL
AS   

	IF @version = 1 
	BEGIN
		PRINT 'Version 0.2'
	END

	IF @verbose = 1 
	BEGIN
		PRINT '------------------------------'
		PRINT ' FAYAT IT - 2018 '
		PRINT 'INJECTION DES DONNEES RESSOURCES (PS Fred_ToolBox_Ressources)'
		PRINT '------------------------------'
	END

	-- Recherche Groupe Id
	DECLARE @GroupeId int; 
	SET @GroupeId = (SELECT GroupeId FROM FRED_GROUPE Where Code = @GroupeCode);
	PRINT @GroupeId
	
	-- Recherche du Sous-Chapitre en fonction du Code du père et du code Groupe (issu du Chapitre)
	DECLARE @SousChapitreId int;
	SET @SousChapitreId = 
		(
		SELECT FRED_SOUS_CHAPITRE.SousChapitreId 
		FROM FRED_CHAPITRE, FRED_SOUS_CHAPITRE 
		WHERE 
		FRED_SOUS_CHAPITRE.Code = @SousChapitreCode 
		AND FRED_CHAPITRE.ChapitreId = FRED_SOUS_CHAPITRE.ChapitreId 
		AND FRED_CHAPITRE.GroupeId = @GroupeId
		);
		PRINT @SousChapitreId;



	-- Recherche Type Ressources
	-- Si le type est renseigné (Personnel, Materiel, ...)
	-- La valeur @@TypeRessourceId peut aussi être renseigné par le code si Transport
	IF(@Type IS NOT NULL)
	BEGIN
		SET @TypeRessourceId = (SELECT TypeRessourceId FROM FRED_TYPE_RESSOURCE WHERE UPPER(FRED_TYPE_RESSOURCE.Libelle) = UPPER(@Type))
	END
	



	-- Recherche si La ressource existe dans le Groupe
	DECLARE @RessourceId INT;
	SET @RessourceId = 
		(
			SELECT FRED_RESSOURCE.RessourceId FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE, FRED_CHAPITRE WHERE 
			FRED_RESSOURCE.Code = @Code
			AND FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
			--AND FRED_SOUS_CHAPITRE.code = @SousChapitreCode
			AND FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId
			AND FRED_CHAPITRE.GroupeId = @GroupeId
		)

	IF @RessourceId IS NULL
	BEGIN
			INSERT INTO FRED_RESSOURCE
			(
				[SousChapitreId],
				[Code],
				[Libelle],
				[TypeRessourceId],
				[Active],
				[DateCreation],
				[AuteurCreationId]
			)
			VALUES
			(
				
				@SousChapitreId,
				@Code,
				@Libelle,
				@TypeRessourceId,
				1,
				GETDATE(),
				1
			)

			IF @verbose = 1 
			BEGIN
			PRINT 'Ressource ' + @code + '/'+ @libelle + ' ajoutée'
			END
	END
	ELSE
	BEGIN
			
			-- CETTE INSTRUCTION SUPPOORTE EGALEMENT LE CHANGEMENT DE SOUS-CHAPITRE
			-- RECHERCHE DU SOUS_CHAPITRE ID en fonction du CODE et du Groupe
			DECLARE @_SousChapitreId INT;
			SET @_SousChapitreId = 
				(
				SELECT FRED_SOUS_CHAPITRE.SousChapitreId FROM FRED_SOUS_CHAPITRE, FRED_CHAPITRE 
				WHERE FRED_SOUS_CHAPITRE.Code = @SousChapitreCode
				AND FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId
				AND FRED_CHAPITRE.GroupeId = @GroupeId
				);
			
			-- RECHERCE de l'actuel Sous-Chapitre ID
			DECLARE @__SousChapitreId INT;
			SET @__SousChapitreId = (SELECT SousChapitreId FROM FRED_RESSOURCE Where RessourceId = @RessourceId);


			UPDATE FRED_RESSOURCE
			SET 
				Libelle = @Libelle,
				Code = @Code,
				TypeRessourceId = @TypeRessourceId,
				SousChapitreId = @_SousChapitreId,
				DateModification = GETDATE(),
				AuteurModificationId = 1
			WHERE RessourceId  = @RessourceId

			
			IF @verbose = 1 
			BEGIN
			PRINT 'Ressource ' + @code + '/'+ @libelle + ' mise à jour'
				--TEST SI CHANGEMENT DE PERE OPERE
				IF @_SousChapitreId <>  @__SousChapitreId
				BEGIN
					PRINT 'Attention, un changement de sous-chapitre est intervenu'
				END

			END
	END
GO 

