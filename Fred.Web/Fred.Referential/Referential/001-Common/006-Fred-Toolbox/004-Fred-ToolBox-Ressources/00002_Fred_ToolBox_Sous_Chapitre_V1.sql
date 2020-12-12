
/*Ajout d'un Sous - Chapitre*/
-- --------------------------------------------------
-- FRED 2017 - R4 - SEPTEMBRE 2018 
-- TOOLBOX INJECTION SOUS CHAPITRE
-- MODOP :  EXEC Fred_ToolBox_SousChapitre @GroupeCode = 'GFTP', @ChapitreCode='60', @Code = '10-450', @Libelle='MO ENCADREMENT', @Version=1, @verbose=1
-- --------------------------------------------------
SELECT * FROM FRED_SOUS_CHAPITRE Where code = '10-450'
IF OBJECT_ID ( 'Fred_ToolBox_SousChapitre', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_SousChapitre;  
GO  
CREATE PROCEDURE Fred_ToolBox_SousChapitre 
		@GroupeCode varchar(500),
		@ChapitreCode varchar(500),
		@Code varchar(50),
		@Libelle varchar(500),
		@Version bit = NULL,
		@verbose bit = NULL
AS   

	IF @version = 1 
	BEGIN
		PRINT 'Version 0.1'
	END

	IF @verbose = 1 
	BEGIN
		PRINT '------------------------------'
		PRINT ' FAYAT IT - 2018 '
		PRINT 'INJECTION DES DONNEES SOUS-CHAPITRE (PS Fred_ToolBox_SousChapitre)'
		PRINT '------------------------------'
	END


	DECLARE @GroupeId int; 
	SET @GroupeId = (SELECT GroupeId FROM FRED_GROUPE Where Code = @GroupeCode);
	
	DECLARE @ChapitreId int; 
	SET @ChapitreId = (SELECT chapitreId FROM FRED_CHAPITRE where Code = @ChapitreCode AND GroupeId = @GroupeId);

	DECLARE @SousChapitreId int;
	SET @SousChapitreId = (SELECT FRED_SOUS_CHAPITRE.SousChapitreId FROM FRED_CHAPITRE, FRED_SOUS_CHAPITRE WHERE FRED_SOUS_CHAPITRE.Code = @Code AND FRED_CHAPITRE.ChapitreId = FRED_SOUS_CHAPITRE.ChapitreId AND FRED_CHAPITRE.GroupeId = @GroupeId);


	IF @SousChapitreId IS NULL
	BEGIN
			INSERT INTO FRED_SOUS_CHAPITRE
			(
				
				[ChapitreId],
				[Code],
				[Libelle],
				[DateCreation],
				[AuteurCreationId]

			)
			VALUES
			(
				
				@ChapitreId,
				@Code,
				@Libelle,
				GETDATE(),
				1
			)

			IF @verbose = 1 
			BEGIN
			PRINT 'Sous Chapitre ' + @code + '/'+ @libelle + ' ajouté'
			END
	END
	ELSE
	BEGIN
			
			
			-- CHAPITRE SELECTIONNE
			SET @ChapitreId = (SELECT chapitreId FROM FRED_CHAPITRE where Code = @ChapitreCode AND GroupeId = @GroupeId);

			-- ANCIEN CHAPITRE
			DECLARE @_ChapitreId INT;
			SET @_ChapitreId = (SELECT FRED_SOUS_CHAPITRE.chapitreId FROM FRED_CHAPITRE, FRED_SOUS_CHAPITRE WHERE FRED_SOUS_CHAPITRE.Code = @Code AND FRED_CHAPITRE.ChapitreId = FRED_SOUS_CHAPITRE.ChapitreId AND FRED_CHAPITRE.GroupeId = @GroupeId);

			UPDATE FRED_SOUS_CHAPITRE
			SET 
				Libelle = @Libelle,
				Code = @Code,
				DateModification = GETDATE(),
				AuteurModificationId = 1,
				ChapitreId = @ChapitreId
			WHERE SousChapitreId  = @SousChapitreId

			IF @verbose = 1 
			BEGIN
			PRINT 'Sous Chapitre ' + @code + '/'+ @libelle + ' mis à jour'
			IF @ChapitreId <>  @_ChapitreId
				BEGIN
					PRINT 'Attention, un changement de Chapitre est intervenu'
				END
			END

	END

GO 

