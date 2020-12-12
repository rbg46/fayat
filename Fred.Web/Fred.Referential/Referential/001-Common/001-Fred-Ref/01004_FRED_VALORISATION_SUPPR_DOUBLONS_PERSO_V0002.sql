DECLARE
    @RapportId int,
    @RapportLigneId int,
	@CiId int,
    @TacheId int,
    @PersonnelId int,
    @Date datetime,
	@VerrouPeriode int,
	@ValorisationId int
CREATE TABLE #ValorisationDoublonsPersonnel (
    [RapportId] [int],
	[RapportLigneId] [int],
    [CiId] [int],
	[TacheId] [int],
    [PersonnelId] [int],
    [Date] datetime,
);
INSERT INTO #ValorisationDoublonsPersonnel (
    [RapportId],
	[RapportLigneId],
    [CiId],
	[TacheId],
    [PersonnelId],
    [Date]
)
SELECT 
    v.RapportId,
    v.RapportLigneId,
    v.CiId,
	v.TacheId,
    v.PersonnelId,
    v.Date 
FROM 
    FRED_VALORISATION v WITH(NOLOCK)
WHERE 
    PersonnelId is not null 
	and Source <> 'Annulation Intérimaire'
GROUP BY 
	v.RapportId,
    v.RapportLigneId,
    v.CiId,
	v.TacheId,
    v.PersonnelId,
    v.Date 
HAVING count([Date]) > 1

WHILE EXISTS(SELECT TOP(1) 1 FROM #ValorisationDoublonsPersonnel)
BEGIN
    SET @RapportId = NULL;
	SET @RapportLigneId = NULL;
    SET @CiId = NULL;
    SET @TacheId = NULL;
    SET @PersonnelId = NULL;
	SET @Date = NULL;
	SET @VerrouPeriode = NULL;
	SET @ValorisationId = NULL;

    SELECT TOP 1
        @RapportId = RapportId,
        @RapportLigneId = RapportLigneId,
        @CiId = CiId,
		@TacheId = TacheId,
        @PersonnelId = PersonnelId,
        @Date = Date
    FROM #ValorisationDoublonsPersonnel
    
	-- Recupération du doublon le plus ancien
	SELECT TOP 1 
		@ValorisationId = ValorisationId,
		@VerrouPeriode = VerrouPeriode
	FROM FRED_VALORISATION 
	WHERE 
		RapportId = @RapportId 
		AND RapportLigneId = @RapportLigneId 
		AND CiId = @CiId 
		AND TacheId = @TacheId 
		AND PersonnelId = @PersonnelId 
		AND Date = @Date
	ORDER BY DateCreation

	IF (@VerrouPeriode = 1)
		-- On supprime le doublon le plus récent
		DELETE FROM FRED_VALORISATION
		WHERE ValorisationId = (SELECT TOP 1 ValorisationId
								FROM FRED_VALORISATION 
								WHERE 
									RapportId = @RapportId 
									AND RapportLigneId = @RapportLigneId 
									AND CiId = @CiId 
									AND TacheId = @TacheId 
									AND PersonnelId = @PersonnelId 
									AND Date = @Date 
								ORDER BY DateCreation desc)
	ELSE 
		-- On supprime le doublon le plus ancien
		DELETE FROM FRED_VALORISATION
		WHERE ValorisationId = @ValorisationId
    
    DELETE FROM #ValorisationDoublonsPersonnel WHERE RapportLigneId = @RapportLigneId;
END

DROP TABLE #ValorisationDoublonsPersonnel