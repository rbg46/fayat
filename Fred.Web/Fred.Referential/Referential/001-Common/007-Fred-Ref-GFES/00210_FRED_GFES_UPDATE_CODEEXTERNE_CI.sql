-- =======================================================================================================================================
-- Author:		Yoann Collet  15/01/2020
--
-- Description:
--      - Mise à jour des code externe null pour les cis des sociétés du groupe GFES
--
-- =======================================================================================================================================



BEGIN TRAN
	DECLARE @ci_id int;
	DECLARE ci_cursor CURSOR FOR
		SELECT CiId FROM FRED_CI C
		INNER JOIN FRED_SOCIETE S ON S.SocieteId = C.SocieteId
		INNER JOIN FRED_GROUPE G ON G.GroupeId = S.GroupeId
		WHERE G.Code = 'GFES' AND C.CodeExterne IS NULL
	OPEN ci_cursor;
	FETCH NEXT FROM ci_cursor INTO @ci_id;
	WHILE @@FETCH_STATUS = 0 BEGIN

			DECLARE @codeExterne nvarchar(6) = (SELECT RIGHT(Code, 6) FROM FRED_CI WHERE CiId = @ci_id)

			UPDATE FRED_CI SET CodeExterne = @codeExterne WHERE CiId = @ci_id

			FETCH NEXT FROM ci_cursor INTO @ci_id;
	END
	CLOSE ci_cursor;
	DEALLOCATE ci_cursor;
COMMIT TRAN

