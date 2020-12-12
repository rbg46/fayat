-- =======================================================================================================================================
-- Author:		Yoann Collet  11/02/2019
--
-- Description:
--      - Mise à jour des Codes Societe Paye et Code societe Comptable 'NULL' => NULL
--
-- =======================================================================================================================================


-- Mise à jour du code societe comptable 'NULL' => NULL


BEGIN TRAN

	DECLARE @societe_id int;
	DECLARE societe_cursor CURSOR FOR
		select SocieteId from FRED_SOCIETE where CodeSocieteComptable = 'NULL'
	OPEN societe_cursor;
	FETCH NEXT FROM societe_cursor INTO @societe_id;
	WHILE @@FETCH_STATUS = 0 BEGIN

		UPDATE FRED_SOCIETE SET CodeSocieteComptable = NULL where SocieteId = @societe_id;

		FETCH NEXT FROM societe_cursor INTO @societe_id;
	END
	CLOSE societe_cursor;
	DEALLOCATE societe_cursor;

COMMIT TRAN

-- Mise à jour du code societe paye 'NULL' => NULL

BEGIN TRAN

	DECLARE @societe_id_2 int;
	DECLARE societe_cursor CURSOR FOR
		select SocieteId from FRED_SOCIETE where CodeSocietePaye = 'NULL'
	OPEN societe_cursor;
	FETCH NEXT FROM societe_cursor INTO @societe_id_2;
	WHILE @@FETCH_STATUS = 0 BEGIN

		UPDATE FRED_SOCIETE SET CodeSocietePaye = NULL where SocieteId = @societe_id_2;

		FETCH NEXT FROM societe_cursor INTO @societe_id_2;
	END
	CLOSE societe_cursor;
	DEALLOCATE societe_cursor;

COMMIT TRAN