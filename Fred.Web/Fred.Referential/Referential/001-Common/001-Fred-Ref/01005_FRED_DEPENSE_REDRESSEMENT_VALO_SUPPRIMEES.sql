-- =======================================================================================================================================
-- Author:		BENNAI Naoufal  26/05/2020
--
-- Description:
--          CB_14168 : Suppression des valorisations liées à des rapports lignes supprimés 
--
-- =======================================================================================================================================

BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			
		  DELETE FROM FRED_VALORISATION WHERE ValorisationId IN 
		  (SELECT ValorisationId 
			FROM FRED_VALORISATION v
			LEFT JOIN FRED_RAPPORT_LIGNE rl 
			ON rl.RapportLigneId = v.RapportLigneId
			WHERE rl.DateSuppression IS NOT NULL
			AND VerrouPeriode = 0); 

		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		DECLARE @ERROR INT, @MESSAGE VARCHAR(4000), @XSTATE INT;
		SELECT @ERROR = ERROR_NUMBER(), @MESSAGE = ERROR_MESSAGE(), @XSTATE = XACT_STATE();
		ROLLBACK TRANSACTION FRED_VALO_REDRESSEMENT;

		RAISERROR('01005_FRED_DEPENSE_REDRESSEMENT_VALO_SUPPRIMEES: %d: %s', 16, 1, @error, @message) ;
	END CATCH
END