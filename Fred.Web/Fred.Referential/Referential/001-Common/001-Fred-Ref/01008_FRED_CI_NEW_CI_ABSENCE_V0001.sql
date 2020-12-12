BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DECLARE @oldCiAbsence TABLE(CiId INT);
			DECLARE @oldCiNewCiCorrespondance TABLE(CiIdOld INT, CiIdNew INT);
            DECLARE @ciTypeIdSection INT;
            DECLARE @societeId INT;
            DECLARE @codeCiAbsence NVARCHAR (50) = 'ABSENCES';
            DECLARE @libelleCiAbsence NVARCHAR (50) = 'Gestion des absences';
            DECLARE @codeExterneCiAbsence NVARCHAR (50) = 'CIABS';

            PRINT N'------------------------ Récupérer id du ci type section----------------------------'; 
            SELECT @ciTypeIdSection = CiTypeId FROM FRED_CI_TYPE WHERE CODE LIKE 'S';
            
            PRINT N'------------------------ Récupération des anciens Ci absences ----------------------------'; 
			INSERT INTO @oldCiAbsence
			SELECT DISTINCT CiId FROM FRED_CI WHERE IsAbsence = 1 AND EtablissementComptableId IS NOT NULL AND Code like 'ABSENCES';
            
            PRINT N'------------------------ Récupérer les sociétés des Ci Absences ----------------------------'; 
            IF CURSOR_STATUS('global','db_cursor_societe') >= -1
			 BEGIN
			  IF CURSOR_STATUS('global','db_cursor_societe') > -1
			   BEGIN
				CLOSE db_cursor_societe
			   END
			 DEALLOCATE db_cursor_societe
			END;

			DECLARE db_cursor_societe CURSOR FOR
				SELECT DISTINCT SocieteId
                FROM FRED_CI
                WHERE CiId in (SELECT CiId FROM @oldCiAbsence);
			OPEN db_cursor_societe;
            
			FETCH NEXT FROM db_cursor_societe INTO @societeId;
			WHILE @@FETCH_STATUS = 0  
			BEGIN
                PRINT N'------------------------ Ajout Organisation pour la société : ' + CAST(@societeId AS NVARCHAR(10)) + '----------------------------'; 
                INSERT INTO FRED_ORGANISATION (PereId, TypeOrganisationId)
                VALUES ((SELECT OrganisationId FROM FRED_SOCIETE WHERE SocieteId = @societeId), 8);
                
                PRINT N'------------------------ Ajout CI pour la société : ' + CAST(@societeId AS NVARCHAR(10)) + '----------------------------'; 
                INSERT INTO FRED_CI (Code, Libelle, [Description], CodeExterne, SocieteId, OrganisationId, CITypeId, DateOuverture, IsAbsence, FacturationEtablissement)
                VALUES (@codeCiAbsence, @libelleCiAbsence, @libelleCiAbsence, @codeExterneCiAbsence, @societeId, @@IDENTITY, @ciTypeIdSection, '1990-01-01', 1, 0);

				FETCH NEXT FROM db_cursor_societe INTO @societeId;
			END;
			CLOSE db_cursor_societe;
			DEALLOCATE db_cursor_societe;
              
            INSERT INTO @oldCiNewCiCorrespondance
            SELECT DISTINCT old.CiId, new.CiId
            FROM @oldCiAbsence ci
            INNER JOIN FRED_CI old
            ON ci.CiId = old.CiId
            INNER JOIN FRED_CI new
            ON old.IsAbsence = new.IsAbsence AND OLD.SocieteId = new.SocieteId AND new.EtablissementComptableId IS NULL

            PRINT N'------------------------ Un script qui mettra à jour l’id du CI des rapports lignes et des rapports----------------------------'; 

            UPDATE FRED_RAPPORT_LIGNE
            SET CiId = (SELECT CiIdNew FROM @oldCiNewCiCorrespondance WHERE CiIdOld = CiId)
            WHERE CiId IN (SELECT old.CiId FROM @oldCiAbsence old);    

			UPDATE FRED_RAPPORT
            SET CiId = (SELECT CiIdNew FROM @oldCiNewCiCorrespondance WHERE CiIdOld = CiId)
            WHERE CiId IN (SELECT old.CiId FROM @oldCiAbsence old); 
            
            PRINT N'------------------------ Supprimer les VALO----------------------------'; 
            DELETE FROM FRED_VALORISATION WHERE CiId IN (SELECT old.CiId FROM @oldCiAbsence old);
            
            PRINT N'------------------------ Supprimer les Baremes expliatation----------------------------';  
            DELETE FROM FRED_BAREME_EXPLOITATION_CI WHERE CiId IN (SELECT old.CiId FROM @oldCiAbsence old);         
            
            PRINT N'------------------------ Modifier les tâches des anciens CI génériques existants----------------------------'; 
			UPDATE FRED_TACHE
            SET CiId = (SELECT CiIdNew FROM @oldCiNewCiCorrespondance WHERE CiIdOld = CiId)
            WHERE CiId IN (SELECT old.CiId FROM @oldCiAbsence old);          
            
            PRINT N'------------------------ Modifier les ci devise des anciens CI génériques existants----------------------------'; 
			UPDATE FRED_CI_DEVISE
            SET CiId = (SELECT CiIdNew FROM @oldCiNewCiCorrespondance WHERE CiIdOld = CiId)
            WHERE CiId IN (SELECT old.CiId FROM @oldCiAbsence old);    
            
			PRINT N'------------------------ Supprimer les anciens CI génériques existants au niveau de l’établissement comptable----------------------------'; 
			DELETE FROM FRED_CI WHERE CiId IN (SELECT old.CiId FROM @oldCiAbsence old);

			PRINT N'------------------------ Supprimer les anciens CI génériques de l’arbre des organisations----------------------------'; 
			DELETE FROM FRED_ORGANISATION WHERE OrganisationId IN (SELECT OrganisationId FROM Fred_CI ci INNER JOIN @oldCiAbsence a ON ci.CiId = a.CiId);
            
		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		DECLARE @ERROR INT, @MESSAGE VARCHAR(4000), @XSTATE INT;
		SELECT @ERROR = ERROR_NUMBER(), @MESSAGE = ERROR_MESSAGE(), @XSTATE = XACT_STATE();
		ROLLBACK TRANSACTION;

		RAISERROR('001007_FRED_CI_NEW_CI_ABSENCE_V0001: %d: %s', 16, 1, @error, @message) ;
	END CATCH
END