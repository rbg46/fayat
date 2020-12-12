-- =======================================================================================================================================
-- Description :	Script d'insertion des libellés courts pour les familles d'opérations diverses
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

	DECLARE @ShortWordingToInsert TABLE (Code VARCHAR(5), LibelleCourt VARCHAR(30));
	INSERT INTO @ShortWordingToInsert 
	VALUES 
		('RCT', 'Recettes'), 
		('MO', 'Déboursé MO'),
		('ACH', 'Déboursé achats'),
		('MIT', 'Déboursé matériel Int'),
		('MI', 'Amortissement'),
		('OTH', 'Autres dépenses'),
		('FG', 'Frais généraux'),
		('OTHD', 'Autres hors débours');

	UPDATE fod SET fod.LibelleCourt = CASE WHEN fod.LibelleCourt IS NULL THEN SWTI.LibelleCourt END
	FROM 
		@ShortWordingToInsert SWTI
		INNER JOIN FRED_FAMILLE_OPERATION_DIVERSE fod ON fod.Code = SWTI.Code

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