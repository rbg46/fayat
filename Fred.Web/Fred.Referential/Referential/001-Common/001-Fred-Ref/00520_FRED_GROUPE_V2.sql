-- =======================================================================================================================================
-- Author:		Yoann Collet 16/11/2018
--
-- Description:
--      - Mise à jour des code et libelle groupe 
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Mise à jour des code et libelle groupe afin qu'il n'y est plus d'espace inutile
	DECLARE @groupe_id int;
	DECLARE groupe_cursor CURSOR FOR
		SELECT GroupeId from FRED_GROUPE
	OPEN groupe_cursor;
	FETCH NEXT FROM groupe_cursor INTO @groupe_id;
	WHILE @@FETCH_STATUS = 0 BEGIN


		DECLARE @code_sans_espace nvarchar(150) = ( SELECT TRIM(code) from FRED_GROUPE WHERE  GroupeId = @groupe_id ) ;
		DECLARE @libelle_sans_espace nvarchar(150) = ( SELECT TRIM(libelle) from FRED_GROUPE WHERE  GroupeId = @groupe_id ) ;

		UPDATE FRED_GROUPE SET Code = @code_sans_espace, Libelle = @libelle_sans_espace where GroupeId = @groupe_id;

		FETCH NEXT FROM groupe_cursor INTO @groupe_id;
	END
	CLOSE groupe_cursor;
	DEALLOCATE groupe_cursor;

COMMIT TRAN

