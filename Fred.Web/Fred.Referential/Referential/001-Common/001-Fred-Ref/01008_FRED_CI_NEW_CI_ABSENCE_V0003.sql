BEGIN
	BEGIN TRY
		BEGIN TRANSACTION		
			PRINT N'------------------------  Suppression tache absence niveau 3 ----------------------------'; 
			DELETE FROM FRED_TACHE
			WHERE TacheId in (SELECT t.TacheId
			FROM FRED_TACHE t
			INNER JOIN FRED_CI ci
			ON ci.CiId = t.CiId
			AND ci.Code like 'ABSENCES'
			WHERE t.Niveau = 3)

			PRINT N'------------------------  Suppression tache absence niveau 2 ----------------------------'; 
			DELETE FROM FRED_TACHE
			WHERE TacheId in (SELECT t.TacheId
			FROM FRED_TACHE t
			INNER JOIN FRED_CI ci
			ON ci.CiId = t.CiId
			AND ci.Code like 'ABSENCES'
			WHERE t.Niveau = 2)

			PRINT N'------------------------  Suppression tache absence niveau 1 ----------------------------'; 
			DELETE FROM FRED_TACHE
			WHERE TacheId in (SELECT t.TacheId
			FROM FRED_TACHE t
			INNER JOIN FRED_CI ci
			ON ci.CiId = t.CiId
			AND ci.Code like 'ABSENCES'
			WHERE t.Niveau = 1)


		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		DECLARE @ERROR INT, @MESSAGE VARCHAR(4000), @XSTATE INT;
		SELECT @ERROR = ERROR_NUMBER(), @MESSAGE = ERROR_MESSAGE(), @XSTATE = XACT_STATE();
		ROLLBACK TRANSACTION;

		RAISERROR('001008_FRED_CI_NEW_CI_ABSENCE_V0003: %d: %s', 16, 1, @error, @message) ;
	END CATCH
END