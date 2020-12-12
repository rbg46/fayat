-- =======================================================================================================================================
-- Author:		Yoann Collet
--
-- Description:
--      - modifie les tâches d'écart intérim aux CI
--
-- =======================================================================================================================================

BEGIN TRAN

	-- Modifie les tâches d'écart interim aux CI
	DECLARE @ci_id int;
	DECLARE ci_cursor CURSOR FOR
		SELECT CiId FROM [dbo].[FRED_TACHE] WHERE [dbo].[FRED_TACHE].TacheType = 8;
	OPEN ci_cursor;
	FETCH NEXT FROM ci_cursor INTO @ci_id;
	WHILE @@FETCH_STATUS = 0 BEGIN


		DECLARE @parent_id int = ( SELECT TOP(1) TacheId from [dbo].[FRED_TACHE] WHERE [dbo].[FRED_TACHE].TacheType = 2  and  CiId = @ci_id ) ;

		-- Modifie les parent Id
		UPDATE [dbo].[FRED_TACHE] SET [dbo].[FRED_TACHE].ParentId = @parent_id WHERE [dbo].[FRED_TACHE].TacheType = 8 and  CiId = @ci_id;

		FETCH NEXT FROM ci_cursor INTO @ci_id;
	END
	CLOSE ci_cursor;
	DEALLOCATE ci_cursor;

COMMIT TRAN
