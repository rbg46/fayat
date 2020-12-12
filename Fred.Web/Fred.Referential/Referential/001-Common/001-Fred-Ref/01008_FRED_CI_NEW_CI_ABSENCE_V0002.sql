BEGIN
	BEGIN TRY
		BEGIN TRANSACTION		
			DECLARE @rapportId INT;
			DECLARE @rapportLigneId INT;
			DECLARE @personnelId INT;

            
            PRINT N'------------------------  Ne pas associer de devise aux nouveaux CI Absences ----------------------------'; 
            DELETE FROM FRED_CI_DEVISE
            WHERE CiId in (SELECT ciid
                            FROM FRED_CI ci
                            WHERE code like 'ABSENCES');
							            
            PRINT N'------------------------  -- Récupération des pointages à traiter ----------------------------';                  
            IF CURSOR_STATUS('global','db_cursor_pointage') >= -1
			 BEGIN
			  IF CURSOR_STATUS('global','db_cursor_pointage') > -1
			   BEGIN
				CLOSE db_cursor_pointage
			   END
			 DEALLOCATE db_cursor_pointage
			END;
      
			DECLARE db_cursor_pointage CURSOR FOR
				SELECT DISTINCT rl.RapportId, rl.RapportLigneId, rl.PersonnelId
                FROM FRED_RAPPORT_LIGNE rl
                INNER JOIN FRED_CI ci
                ON ci.CiId = rl.CiId
				INNER JOIN FRED_RAPPORT_LIGNE_TACHE t
				ON t.RapportLigneId = rl.RapportLigneId
                WHERE ci.code LIKE 'ABSENCES'
			OPEN db_cursor_pointage;

            FETCH NEXT FROM db_cursor_pointage INTO @rapportId, @rapportLigneId, @personnelId;
			WHILE @@FETCH_STATUS = 0  
			BEGIN
                DECLARE @defaultCiId INT;
                DECLARE @defaultTacheId INT;
				
				PRINT N'------------------------ Récupération des CI par défault du personnel ----------------------------';   
				SELECT @defaultCiId = CiId 
                FROM FRED_AFFECTATION a
				WHERE a.IsDefault = 1 
				AND a.IsDelete = 0
				AND PersonnelId = @personnelId;

				PRINT N'------------------------ Récupération de la tache par défault du personnels ----------------------------';  
                SELECT @defaultTacheId = TacheId
                FROM FRED_TACHE
                WHERE CiId = @defaultCiId
                AND TacheParDefaut = 1;
				
				PRINT N'------------------------ Mise à jour du pointage ----------------------------';  
                UPDATE FRED_RAPPORT_LIGNE
                SET CiId = @defaultCiId
                WHERE RapportLigneId = @rapportLigneId;
				
				PRINT N'------------------------ Mise à jour des taches ----------------------------';  
                UPDATE FRED_RAPPORT_LIGNE_TACHE
                SET TacheId = @defaultTacheId
                WHERE RapportLigneId = @rapportLigneId;
				
				PRINT N'------------------------ Mise à jour des rapports ----------------------------';  
                UPDATE FRED_RAPPORT
                SET CiId = @defaultCiId
                WHERE RapportId = @rapportId;                

				FETCH NEXT FROM db_cursor_pointage INTO @rapportId, @rapportLigneId, @personnelId;
			END;
			CLOSE db_cursor_pointage;
			DEALLOCATE db_cursor_pointage;

            PRINT N'------------------------ Supprimer les VALO liées au Ci absence ----------------------------'; 
            DELETE FROM FRED_VALORISATION
            WHERE valorisationId IN (SELECT valorisationId 
                                     FROM FRED_VALORISATION v
                                     INNER JOIN FRED_CI ci
                                     ON ci.CiId = v.CiId
                                     WHERE ci.Code LIKE 'ABSENCES');

            PRINT N'------------------------  Supprimer les Barèmes liées au Ci Absence ----------------------------'; 
            DELETE FROM FRED_BAREME_EXPLOITATION_CI
            WHERE BaremeId IN (SELECT BaremeId 
                                     FROM FRED_BAREME_EXPLOITATION_CI b
                                     INNER JOIN FRED_CI ci
                                     ON ci.CiId = b.CiId
                                     WHERE ci.Code LIKE 'ABSENCES');


		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		DECLARE @ERROR INT, @MESSAGE VARCHAR(4000), @XSTATE INT;
		SELECT @ERROR = ERROR_NUMBER(), @MESSAGE = ERROR_MESSAGE(), @XSTATE = XACT_STATE();
		ROLLBACK TRANSACTION;

		RAISERROR('001008_FRED_CI_NEW_CI_ABSENCE_V0002: %d: %s', 16, 1, @error, @message) ;
	END CATCH
END