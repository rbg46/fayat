-- =======================================================================================================================================
-- Description :	1/ Matricule I_14390181
--					On supprime les valorisations du rapportLigneId = 813908 avec un montant à 0
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

  DELETE FROM FRED_VALORISATION where RapportLigneId = 813908 and Montant = 0

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