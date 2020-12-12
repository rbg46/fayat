-- =======================================================================================================================================
-- Author:		Yoann Collet  20/12/2018
--
-- Description:
--      - Mise à jour des code et libelle de la table pays
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Mise à jour des code et libelle de la table pays afin qu'il n'y est plus d'espace inutile
	DECLARE @pays_id int;
	DECLARE pays_cursor CURSOR FOR
		SELECT PaysId from FRED_PAYS
	OPEN pays_cursor;
	FETCH NEXT FROM pays_cursor INTO @pays_id;
	WHILE @@FETCH_STATUS = 0 BEGIN


		DECLARE @code_sans_espace nvarchar(150) = ( SELECT TRIM(Code) from FRED_PAYS WHERE  PaysId = @pays_id ) ;
		DECLARE @libelle_sans_espace nvarchar(150) = ( SELECT TRIM(Libelle) from FRED_PAYS WHERE  PaysId = @pays_id ) ;

		UPDATE FRED_PAYS SET Code = @code_sans_espace, Libelle = @libelle_sans_espace where PaysId = @pays_id;

		FETCH NEXT FROM pays_cursor INTO @pays_id;
	END
	CLOSE pays_cursor;
	DEALLOCATE pays_cursor;

COMMIT TRAN

