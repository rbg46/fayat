-- =======================================================================================================================================
-- Description :	Cas 1 supprimer la ligne DepenseId='197331' dans DepenseAchat (Ticket 14703)
--					Cas 2 supprimer la ligne DepenseID='197164' dans DepenseAchat (Ticket 14703)
--					Cas 3 supprimer la ligne DepenseID='226961' dans DepenseAchat (Ticket 15167)
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

	-- Liste des 3 dépenses à supprimer
	DECLARE @DepenseIdsToDelete TABLE (Id INT);
	INSERT INTO @DepenseIdsToDelete VALUES (197331), (197164), (226961);

	UPDATE f
	SET f.DepenseAchatFactureEcartId = NULL 
	FROM 
		FRED_FACTURATION f
		INNER JOIN @DepenseIdsToDelete dep ON dep.Id = f.DepenseAchatFactureEcartId
	

	DELETE da
	FROM 
		FRED_DEPENSE_ACHAT da
		INNER JOIN @DepenseIdsToDelete dep ON dep.Id = da.DepenseId

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