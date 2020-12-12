-- =======================================================================================================================================
-- Description : Création des ressources/sous-chapitre par défaut s'ils n'existent pas
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

	SET NOCOUNT ON;  

	DECLARE @DateNow DATETIME;
	SET @DateNow = GETDATE();

	-- Liste des ressources à créer
	DECLARE @RessourcesToAdd TABLE (
		CodeRessource VARCHAR(15), 
		LibelleRessource VARCHAR(100), 
		CodeSousChapitre VARCHAR(15),
		CodeChapitre VARCHAR(15));

	INSERT INTO @RessourcesToAdd (
		CodeRessource, 
		LibelleRessource, 
		CodeSousChapitre,
		CodeChapitre )
	VALUES 
		('OD-PERSO-99', 'OD MAIN D''OEUVRE DIVERSES', '12-999', '12'), 
		('OD-MAT-99', 'OD MATERIEL', '29-999', '20'), 
		('OD-ACH-99', 'OD ACHATS FOURNITURES', '30-999', '30') , 
		('OD-STT-99', 'OD SOUS-TRAITANT', '40-999', '40'), 
		('OD-PREST-99', 'OD PRESTATIONS', '50-999', '50'), 
		('OD-DIVERS-99', 'OD AUTRES FRAIS', '60-999', '60'), 
		('OD-PROV-99', 'OD PROVISION', '70-999', '70');

	DECLARE @CodeRessource VARCHAR(15);
	DECLARE @LibelleRessource VARCHAR(50);
	DECLARE @CodeSousChapitre VARCHAR(15);
	DECLARE @CodeChapitre VARCHAR(15);
	DECLARE @IdSousChapitre INT;
	DECLARE @IdChapitre INT;

	DECLARE ressource_cursor CURSOR FOR   
	SELECT * FROM @RessourcesToAdd;

	OPEN ressource_cursor

	FETCH NEXT FROM ressource_cursor   
	INTO @CodeRessource, @LibelleRessource, @CodeSousChapitre, @CodeChapitre

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @IdSousChapitre = NULL;
		SET @IdChapitre = NULL;

		IF(NOT EXISTS(SELECT TOP 1 1 
						FROM FRED_SOUS_CHAPITRE sc 
						INNER JOIN FRED_CHAPITRE c ON c.ChapitreId = sc.ChapitreId 
						INNER JOIN FRED_GROUPE g ON 
							g.GroupeId = c.GroupeId 
							AND g.Code = 'GRZB'
						WHERE sc.Code = @CodeSousChapitre))
		BEGIN
			SELECT TOP 1 @IdChapitre = ChapitreId
			FROM 
				FRED_CHAPITRE c
				INNER JOIN FRED_GROUPE g ON 
					g.GroupeId = c.GroupeId
					AND g.Code = 'GRZB'
			WHERE c.Code = @CodeChapitre;

			INSERT INTO FRED_SOUS_CHAPITRE (
				ChapitreId, 
				Code, 
				Libelle, 
				DateCreation, 
				AuteurCreationId )
			VALUES (
				@IdChapitre,
				@CodeSousChapitre,
				@LibelleRessource,
				@DateNow,
				1 )
		END

		IF(NOT EXISTS(SELECT TOP 1 1 
						FROM FRED_RESSOURCE r
						INNER JOIN FRED_SOUS_CHAPITRE sc ON sc.SousChapitreId = r.SousChapitreId
						INNER JOIN FRED_CHAPITRE c ON c.ChapitreId = sc.ChapitreId
						INNER JOIN FRED_GROUPE g ON 
							g.GroupeId = c.GroupeId
							AND g.Code = 'GRZB'
						WHERE r.Code = @CodeRessource))
		BEGIN
			SELECT TOP 1 @IdSousChapitre = SousChapitreId 
			FROM 
				FRED_SOUS_CHAPITRE sc
				INNER JOIN FRED_CHAPITRE c ON c.ChapitreId = sc.ChapitreId
				INNER JOIN FRED_GROUPE g ON 
					g.GroupeId = c.GroupeId
					AND g.Code = 'GRZB'
			WHERE sc.Code = @CodeSousChapitre;

			INSERT INTO FRED_RESSOURCE (
				SousChapitreId, 
				Code, 
				Libelle, 
				Active, 
				TypeRessourceId, 
				DateCreation, 
				AuteurCreationId, 
				IsRessourceSpecifiqueCi )
			VALUES (
				@IdSousChapitre,
				@CodeRessource,
				@LibelleRessource,
				1,
				2,
				@DateNow,
				1,
				0 )
		END
		FETCH NEXT FROM ressource_cursor   
		INTO @CodeRessource, @LibelleRessource, @CodeSousChapitre, @CodeChapitre
	END
	CLOSE ressource_cursor;
	DEALLOCATE ressource_cursor;
END TRY  
BEGIN CATCH  
	SELECT   
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_STATE() AS ErrorState,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage

	IF @@TRANCOUNT > 0 
		ROLLBACK TRANSACTION;
END CATCH
  
IF @@TRANCOUNT > 0  
	COMMIT TRANSACTION
GO