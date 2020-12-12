-- =======================================================================================================================================
-- Author:		Yoann Collet  06/03/2019
--
-- Description:
--      - Mise à jour des montants de valorisation (tronqué -> arrondi au supérieur)
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Mise à jour des montants de valorisation (tronqué -> arrondi au supérieur)
	DECLARE @valorisation_id int;
	DECLARE valorisation_cursor CURSOR FOR
		select ValorisationId from FRED_VALORISATION
	OPEN valorisation_cursor;
	FETCH NEXT FROM valorisation_cursor INTO @valorisation_id;
	WHILE @@FETCH_STATUS = 0 BEGIN

		UPDATE FRED_VALORISATION SET  Montant =  ROUND(PUHT * Quantite, 2)  where ValorisationId = @valorisation_id;

		FETCH NEXT FROM valorisation_cursor INTO @valorisation_id;
	END
	CLOSE valorisation_cursor;
	DEALLOCATE valorisation_cursor;

COMMIT TRAN

