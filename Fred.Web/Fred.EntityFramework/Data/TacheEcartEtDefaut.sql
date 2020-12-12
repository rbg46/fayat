-- =======================================================================================================================================
-- Author:		NPI
--
-- Description:
--		- Met le bon type aux tâches par défaut (= 0) car la migration "201803211417416_AddTacheType" a tout mis à 999999
--      - Ajoute les tâches d'écart aux CI
--
-- =======================================================================================================================================

BEGIN TRAN

	-- Met à jour le type des tâches par défaut
	UPDATE FRED_TACHE
	SET
		TacheType = 0
	WHERE 1=1
		AND Libelle = 'TACHE PAR DEFAUT'
		AND (Code = '00' OR Code = '0000' OR Code = '000000');

	-- Ajoute les tâches d'écart et leur 2 parents aux CI
	DECLARE @ci_id int;
	DECLARE ci_cursor CURSOR FOR
		SELECT CiId from FRED_CI
		WHERE NOT EXISTS (SELECT 1 FROM FRED_TACHE WHERE FRED_TACHE.CiId = FRED_CI.CiId and FRED_TACHE.TacheType = 1)
	OPEN ci_cursor;
	FETCH NEXT FROM ci_cursor INTO @ci_id;
	WHILE @@FETCH_STATUS = 0 BEGIN

		-- Insert la tâche d'écart de niveau 1
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId, TacheType)
			VALUES ('99', 'ECART', 0, 1, 1, @ci_id, NULL, 1)

		-- Insert la tâche d'écart de niveau 2
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId, TacheType)
			VALUES ('9999', 'ECART', 0, 2, 1, @ci_id, @@IDENTITY, 2)

		DECLARE @parent_id int = @@IDENTITY;

		-- Insert la tâche d'écart MO encadrement
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId, TacheType)
			VALUES ('999991', 'ECART MO ENCADREMENT', 0, 3, 1, @ci_id, @parent_id, 3)

		-- Insert la tâche d'écart MO production
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId, TacheType)
			VALUES ('999992', 'ECART MO PRODUCTION', 0, 3, 1, @ci_id, @parent_id, 4)

		-- Insert la tâche d'écart matériel
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId, TacheType)
			VALUES ('999993', 'ECART MATERIEL', 0, 3, 1, @ci_id, @parent_id, 5)

		-- Insert la tâche d'écart achat
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId, TacheType)
			VALUES ('999994', 'ECART ACHAT', 0, 3, 1, @ci_id, @parent_id, 6)

		-- Insert la tâche d'écart autre frais
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId, TacheType)
			VALUES ('999995', 'ECART AUTRE FRAIS', 0, 3, 1, @ci_id, @parent_id, 7)

		FETCH NEXT FROM ci_cursor INTO @ci_id;
	END
	CLOSE ci_cursor;
	DEALLOCATE ci_cursor;

COMMIT TRAN
