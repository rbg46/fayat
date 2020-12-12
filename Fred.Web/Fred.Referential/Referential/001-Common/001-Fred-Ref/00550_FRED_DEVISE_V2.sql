-- =======================================================================================================================================
-- Author:		Yoann Collet  20/12/2018
--
-- Description:
--      - Mise à jour de la colonne codePaysIso de la table DEVISE
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Mise à jour de la colonne codePaysIso de la table DEVISE afin qu'il n'y est plus d'espace inutile
	DECLARE @devise_id int;
	DECLARE devise_cursor CURSOR FOR
		SELECT DeviseId from FRED_DEVISE
	OPEN devise_cursor;
	FETCH NEXT FROM devise_cursor INTO @devise_id;
	WHILE @@FETCH_STATUS = 0 BEGIN


		DECLARE @codePaysIso_sans_espace nvarchar(150) = ( SELECT TRIM(CodePaysIso) from FRED_DEVISE WHERE  DeviseId = @devise_id ) ;

		UPDATE FRED_DEVISE SET CodePaysIso = @codePaysIso_sans_espace where DeviseId = @devise_id;

		FETCH NEXT FROM devise_cursor INTO @devise_id;
	END
	CLOSE devise_cursor;
	DEALLOCATE devise_cursor;

COMMIT TRAN

