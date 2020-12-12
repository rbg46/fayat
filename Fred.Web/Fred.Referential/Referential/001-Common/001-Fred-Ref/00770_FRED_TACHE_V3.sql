-- =======================================================================================================================================
-- Author:		Yoann Collet
--
-- Description:
--      - Ajoute les tâches "ECART MATERIEL EXTERNE" aux CI des sociétés du groupe Razel-Bec
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Ajoute les tâches "ECART MATERIEL EXTERNE" aux CI
	DECLARE @ci_id int;
	DECLARE ci_cursor CURSOR FOR
		SELECT CiId from FRED_CI
		WHERE NOT EXISTS (SELECT 1 FROM FRED_TACHE WHERE FRED_TACHE.CiId = FRED_CI.CiId and FRED_TACHE.TacheType = 10)
		And SocieteId in (select SocieteId from FRED_SOCIETE where GroupeId in 
					(select GroupeId from FRED_GROUPE where Code ='GRZB' and PoleId in 
					(select PoleId from FRED_POLE where Code = 'PTP' and HoldingId in 
					(select HoldingId from FRED_HOLDING where Code = 'FSA')))) 
	OPEN ci_cursor;
	FETCH NEXT FROM ci_cursor INTO @ci_id;
	WHILE @@FETCH_STATUS = 0 BEGIN


		DECLARE @parent_id int = ( SELECT TOP(1) TacheId from [dbo].[FRED_TACHE] WHERE [dbo].[FRED_TACHE].TacheType = 2  and  CiId = @ci_id ) ;

		-- Insert la tâche "litige - article non commande"
		INSERT INTO FRED_TACHE (Code, Libelle, TacheParDefaut, Niveau, Active, CiId, ParentId, TacheType, DateCreation)
			VALUES ('999997', 'ECART MATERIEL EXTERNE', 0, 3, 1, @ci_id, @parent_id, 10, GetDate())

		FETCH NEXT FROM ci_cursor INTO @ci_id;
	END
	CLOSE ci_cursor;
	DEALLOCATE ci_cursor;

COMMIT TRAN

