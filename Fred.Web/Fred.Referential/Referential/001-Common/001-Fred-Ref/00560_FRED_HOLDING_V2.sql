-- =======================================================================================================================================
-- Author:		Yoann Collet  20/12/2018
--
-- Description:
--      - Mise à jour des code et libelle de la table holding
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Mise à jour des code et libelle de la table holding afin qu'il n'y est plus d'espace inutile
	DECLARE @holding_id int;
	DECLARE holding_cursor CURSOR FOR
		SELECT HoldingId from FRED_HOLDING
	OPEN holding_cursor;
	FETCH NEXT FROM holding_cursor INTO @holding_id;
	WHILE @@FETCH_STATUS = 0 BEGIN


		DECLARE @code_sans_espace nvarchar(150) = ( SELECT TRIM(Code) from FRED_HOLDING WHERE  HoldingId = @holding_id ) ;
		DECLARE @libelle_sans_espace nvarchar(150) = ( SELECT TRIM(Libelle) from FRED_HOLDING WHERE  HoldingId = @holding_id ) ;

		UPDATE FRED_HOLDING SET Code = @code_sans_espace, Libelle = @libelle_sans_espace where HoldingId = @holding_id;

		FETCH NEXT FROM holding_cursor INTO @holding_id;
	END
	CLOSE holding_cursor;
	DEALLOCATE holding_cursor;

COMMIT TRAN

