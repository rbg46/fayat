-- =======================================================================================================================================
-- Description :	il faudrait mettre à jour le PU côté FRED car il est différent de celui de SAP, 
--					ce qui génère des erreurs d'affichage dans l'explorateur
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

	-- Liste des 2 lignes à mettre à jour
	DECLARE @DepenseIdsToUpdate TABLE (Id INT);
	INSERT INTO @DepenseIdsToUpdate VALUES (32046), (32047);

	UPDATE DA SET PUHT = 710
	FROM
		FRED_DEPENSE_ACHAT DA
		INNER JOIN @DepenseIdsToUpdate DITO ON DA.DepenseId = DITO.Id

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