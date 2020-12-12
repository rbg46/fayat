
DECLARE @Code NVARCHAR(250),
		@CiId INT,
		@TacheId INT,
		@OldTachId INT,
		@ValorisationId INT;

	IF OBJECT_ID('tempdb..#tmp') IS NOT NULL 
		BEGIN
			DROP TABLE #tmp
		END

	CREATE TABLE #tmp
	(ValorisationId [INT] NOT NULL,
	 CiId INT NOT NULL,
	 TachId INT NOT NULL,
	 Code [NVARCHAR](500) NULL)

	-- Récupération de toutes les valos à MAJ(Ayant une tache avec CI autre que celui de la valo).
	INSERT INTO #tmp(ValorisationId,CiId,TachId,Code)
	SELECT v.ValorisationId,v.CiId,v.TacheId,t.Code FROM FRED_VALORISATION v
	INNER JOIN FRED_TACHE t on t.TacheId = v.TacheId 
	WHERE t.CiId <> v.CiId

	-- Pour chaque valo
	WHILE EXISTS(SELECT TOP(1) 1 FROM #tmp)
	BEGIN
		SELECT top 1  @ValorisationId = ValorisationId, @CiId= CiId, @Code = Code, @OldTachId = TachId FROM #tmp 

		-- Récuperation de l'id de la tâche de niveau 3 qui correspond à la valo
		SELECT TOP 1 @TacheId = TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 3 AND Code = @Code;

		-- Mise à jour de la valo.
		IF(@TacheId IS NOT NULL)
		BEGIN
			UPDATE FRED_VALORISATION
			SET TacheId = @TacheId
			WHERE ValorisationId = @ValorisationId;
		END

		DELETE FROM #tmp  WHERE ValorisationId = @ValorisationId;
	END
DROP TABLE #tmp;