-- =======================================================================================================================================
-- Author:		NPI
--
-- Description:
--      - Change les libell�s et codes des t�ches par d�faut existantes :
--          - Libell� = TACHE PAR DEFAUT
--          - Code = 00 (t�che de niveau 1)
--          - Code = 0000 (t�che de niveau 2)
--          - Code = 000000 (t�che de niveau 3)
--      - Ajoute les t�ches par d�faut aux CI qui n'en poss�dent pas
--
-- =======================================================================================================================================

BEGIN TRAN

	-- Met � jour les libell�s et codes des t�ches par d�faut (niveau 3)
	UPDATE FRED_TACHE
	SET
		Code = '000000',
		Libelle = 'TACHE PAR DEFAUT'
	WHERE TacheParDefaut = 1

	-- Met � jour les libell�s et codes des parents des t�ches par d�faut (niveau 2)
	UPDATE FRED_TACHE
	SET
		Code = '0000',
		Libelle = 'TACHE PAR DEFAUT'
	WHERE TacheId IN (
		SELECT FRED_TACHE_level2.TacheId
		FROM FRED_TACHE as FRED_TACHE_level3
			INNER JOIN FRED_TACHE as FRED_TACHE_level2 on FRED_TACHE_level2.TacheId = FRED_TACHE_level3.ParentId
		WHERE FRED_TACHE_level3.TacheParDefaut = 1
	)

	-- Met � jour les libell�s et codes des parents des parents des t�ches par d�faut (niveau 1)
	UPDATE FRED_TACHE
	SET
		Code = '00',
		Libelle = 'TACHE PAR DEFAUT'
	WHERE TacheId IN (
		SELECT FRED_TACHE_level1.TacheId
		FROM FRED_TACHE as FRED_TACHE_level3
			INNER JOIN FRED_TACHE as FRED_TACHE_level2 on FRED_TACHE_level2.TacheId = FRED_TACHE_level3.ParentId
			INNER JOIN FRED_TACHE as FRED_TACHE_level1 on FRED_TACHE_level1.TacheId = FRED_TACHE_level2.ParentId
		WHERE FRED_TACHE_level3.TacheParDefaut = 1
	)

	-- Ajoute la t�che par d�faut est ses 2 parents aux CI qui n'en disposent pas
	DECLARE @ci_id int;
	DECLARE ci_sans_tache_par_defaut_cursor CURSOR FOR
		SELECT CiId from FRED_CI
		WHERE NOT EXISTS (SELECT 1 FROM FRED_TACHE WHERE FRED_TACHE.CiId = FRED_CI.CiId and FRED_TACHE.TacheParDefaut = 1)
	OPEN ci_sans_tache_par_defaut_cursor;
	FETCH NEXT FROM ci_sans_tache_par_defaut_cursor INTO @ci_id;
	WHILE @@FETCH_STATUS = 0 BEGIN
		-- Insert la t�che de niveau 1
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId)
			VALUES ('00', 'TACHE PAR DEFAUT', 0, 1, 1, @ci_id, NULL)

		-- Insert la t�che de niveau 2
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId)
			VALUES ('0000', 'TACHE PAR DEFAUT', 0, 2, 1, @ci_id, @@IDENTITY)
		
		-- Insert la t�che de niveau 3
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId)
			VALUES ('000000', 'TACHE PAR DEFAUT', 1, 3, 1, @ci_id, @@IDENTITY)

		FETCH NEXT FROM ci_sans_tache_par_defaut_cursor INTO @ci_id;
	END
	CLOSE ci_sans_tache_par_defaut_cursor;
	DEALLOCATE ci_sans_tache_par_defaut_cursor;

COMMIT TRAN
