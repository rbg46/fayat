-- =======================================================================================================================================
-- Author:		Yoann Collet
--
-- Description:
--      - Ajoute les tâches d'écart intérim aux CI
--
-- =======================================================================================================================================

BEGIN TRAN

	-- Ajoute les tâches d'écart interim aux CI
	DECLARE @ci_id int;
	DECLARE ci_cursor CURSOR FOR
		SELECT CiId from FRED_CI
		WHERE NOT EXISTS (SELECT 1 FROM FRED_TACHE WHERE FRED_TACHE.CiId = FRED_CI.CiId and FRED_TACHE.TacheType = 8)
	OPEN ci_cursor;
	FETCH NEXT FROM ci_cursor INTO @ci_id;
	WHILE @@FETCH_STATUS = 0 BEGIN


		DECLARE @parent_id int = ( SELECT TOP(1) TacheId from FRED_TACHE WHERE Niveau = 2  and  CiId = @ci_id ) ;

		-- Insert la tâche d'écart intérim
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId, TacheType)
			VALUES ('999996', 'ECART INTERIM', 0, 3, 1, @ci_id, @parent_id, 8)

		FETCH NEXT FROM ci_cursor INTO @ci_id;
	END
	CLOSE ci_cursor;
	DEALLOCATE ci_cursor;

COMMIT TRAN
