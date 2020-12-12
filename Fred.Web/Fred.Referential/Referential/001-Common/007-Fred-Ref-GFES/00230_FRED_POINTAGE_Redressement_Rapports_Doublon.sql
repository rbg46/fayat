-- =======================================================================================================================================
-- Author:		Naoufal BENNAI  13/04/2020
--
-- Description:
--          Bug_13962 & 13921 : Script de redressement des rapports et rapports lignes
--
-- =======================================================================================================================================
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DECLARE @dateChantier DATETIME;
			DECLARE @ciId INT;
			DECLARE @typeStatutRapport INT;
			DECLARE @countDoublon INT;
			DECLARE @statutPeronnel1 INT = 1;
			DECLARE @statutPeronnel2 INT = 2;
			DECLARE @statutPeronnel3 INT = 3;

			-- Traiter les rapports doublons avec le même CI, date chantier et statut type personnel pour en garder que un.
			PRINT N'------------------------CURSOR TRAITEMENT RAPPORT DOUBLON----------------------------'; 
			IF CURSOR_STATUS('global','db_cursor_duplicated_reports') >= -1
			 BEGIN
			  IF CURSOR_STATUS('global','db_cursor_duplicated_reports') > -1
			   BEGIN
				CLOSE db_cursor_duplicated_reports
			   END
			 DEALLOCATE db_cursor_duplicated_reports
			END;

			DECLARE db_cursor_duplicated_reports CURSOR FOR
				SELECT r.DateChantier, r.CiId, r.TypeStatutRapport, COUNT(*) 
				FROM FRED_RAPPORT r
				INNER JOIN fred_ci ci
				ON ci.CiId = r.CiId
				INNER JOIN FRED_SOCIETE s
				ON s.SocieteId = ci.SocieteId
				INNER JOIN FRED_GROUPE g
				ON g.GroupeId = s.GroupeId AND g.Code = 'GFES'
				WHERE r.DateSuppression IS NULL
				AND r.DateChantier >= '2020-03-01'
				GROUP BY r.CiId, r.DateChantier, r.TypeStatutRapport
				HAVING count(*) = 2
				ORDER BY r.DateChantier, r.CiId DESC
			OPEN db_cursor_duplicated_reports;
			
			FETCH NEXT FROM db_cursor_duplicated_reports INTO @dateChantier, @ciId, @typeStatutRapport, @countDoublon;
			WHILE @@FETCH_STATUS = 0  
			BEGIN
				PRINT N'------------------DateChantier = ' + FORMAT(@dateChantier, 'dd/MM/yyyy') + ' CiId = ' + CAST(@ciId AS NVARCHAR(10)) + 'Statut = ' + CAST(@typeStatutRapport AS NVARCHAR(10)) + '----------------';
				DECLARE @firstRapportId INT = NULL;
				DECLARE @secondRapportId INT = NULL;

				SELECT TOP 1 @firstRapportId = RapportId FROM FRED_RAPPORT WHERE DateSuppression IS NULL AND DateChantier = @dateChantier AND CiId = @ciId AND TypeStatutRapport = @typeStatutRapport ORDER BY RapportId ASC;				
				SELECT TOP 1 @secondRapportId = RapportId FROM FRED_RAPPORT WHERE DateSuppression IS NULL AND DateChantier = @dateChantier AND CiId = @ciId AND TypeStatutRapport = @typeStatutRapport ORDER BY RapportId DESC;
				PRINT N'Rapport 1 = ' + CAST(@firstRapportId AS NVARCHAR(10)) + ', Rapport 2 = ' + CAST(@secondRapportId AS NVARCHAR(10)) + '----------------';
				
				-- Modifier les rapport ligne avec RapportId = @firstRapportId par @firstRapportId
				UPDATE FRED_RAPPORT_LIGNE SET RapportId = @secondRapportId WHERE RapportLigneId IN (SELECT RapportLigneId FROM FRED_RAPPORT_LIGNE WHERE RapportId = @firstRapportId);
				UPDATE FRED_RAPPORT SET DateSuppression = GETUTCDATE() WHERE RapportId = @firstRapportId;
			
				PRINT N'-------------------------------------------------------------------------------------'
				FETCH NEXT FROM db_cursor_duplicated_reports INTO @dateChantier, @ciId, @typeStatutRapport, @countDoublon;
			END;
			CLOSE db_cursor_duplicated_reports;
			DEALLOCATE db_cursor_duplicated_reports;

			
			-- Récupérer les Ci pointer à partir du mois Mars pour FES, non supprimés
			-- Récupérer Date de pointage à partir du mois Mars pour FES, non supprimés
			
			PRINT N'------------------CURSOR DEPLACEMENT DES RAPPORTS LIGNES VERS LES BONS RAPPORTS----------------';  
			
			IF CURSOR_STATUS('global','db_cursor') >= -1
			 BEGIN
			  IF CURSOR_STATUS('global','db_cursor') > -1
			   BEGIN
				CLOSE db_cursor
			   END
			 DEALLOCATE db_cursor
			END;

			DECLARE db_cursor CURSOR FOR
				SELECT DISTINCT r.DateChantier, r.CiId
				FROM FRED_RAPPORT r
				INNER JOIN fred_ci ci
				ON ci.CiId = r.CiId
				INNER JOIN FRED_SOCIETE s
				ON s.SocieteId = ci.SocieteId
				INNER JOIN FRED_GROUPE g
				ON g.GroupeId = s.GroupeId AND g.Code = 'GFES'
				WHERE r.DateSuppression IS NULL
				AND r.DateChantier >= '2020-03-01'
				ORDER BY r.DateChantier, r.CiId DESC
			OPEN db_cursor;

			-- Boucler sur le ci et la date chantier
			FETCH NEXT FROM db_cursor INTO @dateChantier, @ciId;
			WHILE @@FETCH_STATUS = 0  
			BEGIN  
				PRINT N'------------------DateChantier = ' + FORMAT(@dateChantier, 'dd/MM/yyyy') + ' CiId = ' + CAST(@ciId AS NVARCHAR(10)) + '----------------';

				-- Récupérer les rapports qui répondent à la condition ci + Date chantier et non supprimés
				DECLARE @PersonnelStatutIdUsed AS TABLE(Statut INT);
				DECLARE @RapportIdStatut0 INT = null;
				DECLARE @RapportIdStatut1 INT = null;
				DECLARE @RapportIdStatut2 INT = null;
				DECLARE @RapportIdStatut3 INT = null;
				-- Récupérer le Rapport avec Statut Personnel 0, 1, 2 et 3
				SELECT @RapportIdStatut0 = RapportId FROM FRED_RAPPORT WHERE DateChantier = @dateChantier AND CiId = @ciId AND TypeStatutRapport = 0;
				SELECT @RapportIdStatut1 = RapportId FROM FRED_RAPPORT WHERE DateChantier = @dateChantier AND CiId = @ciId AND TypeStatutRapport = 1;
				SELECT @RapportIdStatut2 = RapportId FROM FRED_RAPPORT WHERE DateChantier = @dateChantier AND CiId = @ciId AND TypeStatutRapport = 2;
				SELECT @RapportIdStatut3 = RapportId FROM FRED_RAPPORT WHERE DateChantier = @dateChantier AND CiId = @ciId AND TypeStatutRapport = 3;
				IF(@RapportIdStatut0 IS NULL) PRINT N'Rapport 0 = NULL' ; ELSE PRINT N'Rapport 0 = ' + CAST(@RapportIdStatut0 AS NVARCHAR(10));
				IF(@RapportIdStatut1 IS NULL) PRINT N'Rapport 1 = NULL' ; ELSE PRINT N'Rapport 1 = ' + CAST(@RapportIdStatut1 AS NVARCHAR(10));
				IF(@RapportIdStatut2 IS NULL) PRINT N'Rapport 2 = NULL' ; ELSE PRINT N'Rapport 2 = ' + CAST(@RapportIdStatut2 AS NVARCHAR(10));
				IF(@RapportIdStatut3 IS NULL) PRINT N'Rapport 3 = NULL' ; ELSE PRINT N'Rapport 3 = ' + CAST(@RapportIdStatut3 AS NVARCHAR(10));
				
				INSERT INTO @PersonnelStatutIdUsed
				SELECT DISTINCT CAST(p.Statut AS INT) FROM FRED_RAPPORT_LIGNE rl INNER JOIN FRED_PERSONNEL p ON rl.PersonnelId = p.PersonnelId WHERE rl.RapportId IN (@RapportIdStatut0, @RapportIdStatut1, @RapportIdStatut2, @RapportIdStatut3);
								
				PRINT N'------------------ CREATION DES RAPPORTS MANQUANTS ----------------';  
				IF(@RapportIdStatut1 IS NULL AND @statutPeronnel1 IN (SELECT Statut FROM @PersonnelStatutIdUsed))
				BEGIN
					IF(@RapportIdStatut0 IS NOT NULL)
						INSERT INTO [dbo].[FRED_RAPPORT] ([RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated] ,[TypeStatutRapport])     
						SELECT [RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated], 1
						FROM FRED_RAPPORT WHERE RapportId = @RapportIdStatut0;
					ELSE IF(@RapportIdStatut2 IS NOT NULL)
						INSERT INTO [dbo].[FRED_RAPPORT] ([RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated] ,[TypeStatutRapport])     
						SELECT [RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated], 1
						FROM FRED_RAPPORT WHERE RapportId = @RapportIdStatut2;
					ELSE IF(@RapportIdStatut3 IS NOT NULL)
						INSERT INTO [dbo].[FRED_RAPPORT] ([RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated] ,[TypeStatutRapport])     
						SELECT [RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated], 1
						FROM FRED_RAPPORT WHERE RapportId = @RapportIdStatut3;

					SELECT @RapportIdStatut1 = RapportId FROM FRED_RAPPORT WHERE DateChantier = @dateChantier AND CiId = @ciId AND TypeStatutRapport = 1;
				END
				
				IF(@RapportIdStatut2 IS NULL AND @statutPeronnel2 IN (SELECT Statut FROM @PersonnelStatutIdUsed))
				BEGIN
					IF(@RapportIdStatut0 IS NOT NULL)
						INSERT INTO [dbo].[FRED_RAPPORT] ([RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated] ,[TypeStatutRapport])     
						SELECT [RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated], 2
						FROM FRED_RAPPORT WHERE RapportId = @RapportIdStatut0;
					ELSE IF(@RapportIdStatut1 IS NOT NULL)
						INSERT INTO [dbo].[FRED_RAPPORT] ([RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated] ,[TypeStatutRapport])     
						SELECT [RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated], 2
						FROM FRED_RAPPORT WHERE RapportId = @RapportIdStatut1;
					ELSE IF(@RapportIdStatut3 IS NOT NULL)
						INSERT INTO [dbo].[FRED_RAPPORT] ([RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated] ,[TypeStatutRapport])     
						SELECT [RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated], 2
						FROM FRED_RAPPORT WHERE RapportId = @RapportIdStatut3;

					SELECT @RapportIdStatut2 = RapportId FROM FRED_RAPPORT WHERE DateChantier = @dateChantier AND CiId = @ciId AND TypeStatutRapport = 2;
				END
				
				IF(@RapportIdStatut3 IS NULL AND @statutPeronnel3 IN (SELECT Statut FROM @PersonnelStatutIdUsed))
				BEGIN
					IF(@RapportIdStatut0 IS NOT NULL)
						INSERT INTO [dbo].[FRED_RAPPORT] ([RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated] ,[TypeStatutRapport])     
						SELECT [RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated], 2
						FROM FRED_RAPPORT WHERE RapportId = @RapportIdStatut0;
					ELSE IF(@RapportIdStatut1 IS NOT NULL)
						INSERT INTO [dbo].[FRED_RAPPORT] ([RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated] ,[TypeStatutRapport])     
						SELECT [RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated], 2
						FROM FRED_RAPPORT WHERE RapportId = @RapportIdStatut1;
					ELSE IF(@RapportIdStatut2 IS NOT NULL)
						INSERT INTO [dbo].[FRED_RAPPORT] ([RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated] ,[TypeStatutRapport])     
						SELECT [RapportStatutId] ,[DateChantier] ,[HoraireDebutM] ,[HoraireFinM] ,[HoraireDebutS] ,[HoraireFinS] ,[Meteo] ,[Evenements] ,[AuteurCreationId] ,[AuteurModificationId] ,[AuteurSuppressionId] ,[AuteurVerrouId] ,[DateCreation] ,[DateModification] ,[DateSuppression] ,[DateVerrou] ,[CiId] ,[ValideurCDCId] ,[ValideurCDTId] ,[ValideurDRCId] ,[DateValidationCDC] ,[DateValidationCDT] ,[DateValidationDRC] ,[TypeRapport] ,[IsGenerated], 2
						FROM FRED_RAPPORT WHERE RapportId = @RapportIdStatut2;

					SELECT @RapportIdStatut3 = RapportId FROM FRED_RAPPORT WHERE DateChantier = @dateChantier AND CiId = @ciId AND TypeStatutRapport = 3;
				END
				IF(@RapportIdStatut0 IS NULL) PRINT N'Rapport 0 = NULL' ; ELSE PRINT N'Rapport 0 = ' + CAST(@RapportIdStatut0 AS NVARCHAR(10));
				IF(@RapportIdStatut1 IS NULL) PRINT N'Rapport 1 = NULL' ; ELSE PRINT N'Rapport 1 = ' + CAST(@RapportIdStatut1 AS NVARCHAR(10));
				IF(@RapportIdStatut2 IS NULL) PRINT N'Rapport 2 = NULL' ; ELSE PRINT N'Rapport 2 = ' + CAST(@RapportIdStatut2 AS NVARCHAR(10));
				IF(@RapportIdStatut3 IS NULL) PRINT N'Rapport 3 = NULL' ; ELSE PRINT N'Rapport 3 = ' + CAST(@RapportIdStatut3 AS NVARCHAR(10));
				
				
				PRINT N'------------------ ACTION DEPLACEMENT DES RAPPORTS LIGNE VERS LES BONS RAPPORTS ----------------'; 

				IF(@RapportIdStatut1 IS NOT NULL)
					UPDATE FRED_RAPPORT_LIGNE SET RapportId = @RapportIdStatut1
					WHERE RapportLigneId IN (SELECT rl.RapportLigneId 
											 FROM FRED_RAPPORT_LIGNE rl 
											 INNER JOIN FRED_PERSONNEL p 
											 ON rl.PersonnelId = p.PersonnelId 
											 WHERE rl.RapportId IN (@RapportIdStatut0, @RapportIdStatut1, @RapportIdStatut2, @RapportIdStatut3)
											 AND p.Statut = @statutPeronnel1);
								
				IF(@RapportIdStatut2 IS NOT NULL)
				UPDATE FRED_RAPPORT_LIGNE SET RapportId = @RapportIdStatut2
					WHERE RapportLigneId IN (SELECT rl.RapportLigneId 
											 FROM FRED_RAPPORT_LIGNE rl 
											 INNER JOIN FRED_PERSONNEL p 
											 ON rl.PersonnelId = p.PersonnelId 
											 WHERE rl.RapportId IN (@RapportIdStatut0, @RapportIdStatut1, @RapportIdStatut2, @RapportIdStatut3)
											 AND p.Statut = @statutPeronnel2);
								
				IF(@RapportIdStatut3 IS NOT NULL)		 
					UPDATE FRED_RAPPORT_LIGNE SET RapportId = @RapportIdStatut3
					WHERE RapportLigneId IN (SELECT rl.RapportLigneId 
											 FROM FRED_RAPPORT_LIGNE rl 
											 INNER JOIN FRED_PERSONNEL p 
											 ON rl.PersonnelId = p.PersonnelId 
											 WHERE rl.RapportId IN (@RapportIdStatut0, @RapportIdStatut1, @RapportIdStatut2, @RapportIdStatut3)
											 AND p.Statut = @statutPeronnel3);

				UPDATE FRED_RAPPORT SET DateSuppression = GETUTCDATE() WHERE RapportId = @RapportIdStatut0;
			
				PRINT N'-------------------------------------------------------------------------------------------'
				FETCH NEXT FROM db_cursor INTO @dateChantier, @ciId;
			END;
			CLOSE db_cursor;
			DEALLOCATE db_cursor;
			

			-- Supprimer les rapport qui n'ont pas de rapport ligne
			
			PRINT N'------------------SUPPRESION DES RAPPORTS SANS RAPPORT LIGNE----------------';  
			DECLARE @RapportWithNoRapportLigne AS TABLE(RapportId INT, countRapportLigne INT);
			INSERT @RapportWithNoRapportLigne 
			SELECT DISTINCT r.RapportId, COUNT(rl.RapportLigneId)
									FROM FRED_RAPPORT r
									LEFT JOIN FRED_RAPPORT_LIGNE rl
									ON r.RapportId = rl.RapportId
									INNER JOIN fred_ci ci
									ON ci.CiId = r.CiId
									INNER JOIN FRED_SOCIETE s
									ON s.SocieteId = ci.SocieteId
									INNER JOIN FRED_GROUPE g
									ON g.GroupeId = s.GroupeId AND g.Code = 'GFES'
									WHERE r.DateSuppression IS NULL
									AND r.DateChantier >= '2020-03-01'
									GROUP BY r.RapportId
									HAVING COUNT(rl.RapportLigneId) = 0;

			UPDATE FRED_RAPPORT SET DateSuppression = GETUTCDATE()
			WHERE RapportId IN (SELECT RapportId FROM @RapportWithNoRapportLigne);
			
			PRINT N'------------------FIN SCRIPT----------------';  
		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		DECLARE @ERROR INT, @MESSAGE VARCHAR(4000), @XSTATE INT;
		SELECT @ERROR = ERROR_NUMBER(), @MESSAGE = ERROR_MESSAGE(), @XSTATE = XACT_STATE();
		ROLLBACK TRANSACTION FRED_POINTAGE_REDRESSEMENT;

		RAISERROR('01005_FRED_POINTAGE_Redressement_Rapports_Doublon: %d: %s', 16, 1, @error, @message) ;
	END CATCH
END