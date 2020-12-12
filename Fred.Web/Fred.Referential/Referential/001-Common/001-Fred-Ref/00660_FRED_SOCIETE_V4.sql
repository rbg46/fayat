-- =======================================================================================================================================
-- Author:		Yoann Collet  20/12/2018
--
-- Description:
--      - Mise à jour des code et libelle de la table pole
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Mise à jour des code et libelle de la table pole afin qu'il n'y est plus d'espace inutile
	DECLARE @pole_id int;
	DECLARE pole_cursor CURSOR FOR
		SELECT PoleId from FRED_POLE
	OPEN pole_cursor;
	FETCH NEXT FROM pole_cursor INTO @pole_id;
	WHILE @@FETCH_STATUS = 0 BEGIN


		DECLARE @code_sans_espace nvarchar(150) = ( SELECT TRIM(Code) from FRED_POLE WHERE  PoleId = @pole_id ) ;
		DECLARE @libelle_sans_espace nvarchar(150) = ( SELECT TRIM(Libelle) from FRED_POLE WHERE  PoleId = @pole_id ) ;

		UPDATE FRED_POLE SET Code = @code_sans_espace, Libelle = @libelle_sans_espace where PoleId = @pole_id;

		FETCH NEXT FROM pole_cursor INTO @pole_id;
	END
	CLOSE pole_cursor;
	DEALLOCATE pole_cursor;

COMMIT TRAN

