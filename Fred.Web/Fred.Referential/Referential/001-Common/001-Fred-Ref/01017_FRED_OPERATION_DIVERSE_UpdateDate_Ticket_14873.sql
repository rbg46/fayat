-- =======================================================================================================================================
-- Description :	MAJ de la date comptable des ODs
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY
	
	-- MAJ de la date comptable des OD manuelles
	UPDATE OD 
	SET OD.DateComptable = DATEFROMPARTS(YEAR(OD.DateComptable), MONTH(OD.DateComptable), 15)
	FROM 
		FRED_OPERATION_DIVERSE OD
		INNER JOIN FRED_CI C on C.CiId = OD.CiId
	WHERE 
		DAY(OD.DateComptable) <> 15 
		AND OdEcart = 0

	-- MAJ de la date comptable des OD automatiques
	UPDATE OD 
	SET OD.DateComptable = EOMONTH(DateComptable)
	FROM 
		FRED_OPERATION_DIVERSE OD
		INNER JOIN FRED_CI C on C.CiId = OD.CiId
	WHERE 
		DAY(EOMONTH(DateComptable)) <> DAY(DateComptable)
		AND OdEcart = 1

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