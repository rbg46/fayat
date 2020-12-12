DECLARE
    @RapportId int,
    @RapportLigneId int,
	@CiId int,
    @TacheId int,
    @MaterielId int,
    @Date datetime,
	@VerrouPeriode int,
	@ValorisationId int
CREATE TABLE #ValorisationDoublonsMateriel (
    [RapportId] [int],
	[RapportLigneId] [int],
    [CiId] [int],
	[TacheId] [int],
    [MaterielId] [int],
    [Date] datetime,
);
INSERT INTO #ValorisationDoublonsMateriel (
    [RapportId],
	[RapportLigneId],
    [CiId],
	[TacheId],
    [MaterielId],
    [Date]
)
SELECT 
	v.RapportId,
    v.RapportLigneId,
    v.CiId,
	v.TacheId,
    v.MaterielId,
    v.Date 
FROM 
    FRED_VALORISATION v WITH(NOLOCK)
WHERE 
    MaterielId is not null 
	and Source <> 'Annulation Matériel Externe'
GROUP BY 
	v.RapportId,
    v.RapportLigneId,
    v.CiId,
	v.TacheId,
    v.MaterielId,
    v.Date 
HAVING count([Date]) > 1

WHILE EXISTS(SELECT TOP(1) 1 FROM #ValorisationDoublonsMateriel)
BEGIN
	SET @RapportId = NULL;
	SET @RapportLigneId = NULL;
    SET @CiId = NULL;
    SET @TacheId = NULL;
    SET @MaterielId = NULL;
	SET @Date = NULL;
	SET @VerrouPeriode = NULL;
	SET @ValorisationId = NULL;

    SELECT TOP 1
		@RapportId = RapportId,
        @RapportLigneId = RapportLigneId,
        @CiId = CiId,
		@TacheId = TacheId,
        @MaterielId = MaterielId,
        @Date = Date
    FROM #ValorisationDoublonsMateriel
    
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
		AND MaterielId = @MaterielId 
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
									AND MaterielId = @MaterielId 
									AND Date = @Date 
								ORDER BY DateCreation desc)
	ELSE
		-- On supprime le doublon le plus ancien
		DELETE FROM FRED_VALORISATION
		WHERE ValorisationId = @ValorisationId
    
    DELETE FROM #ValorisationDoublonsMateriel WHERE RapportLigneId = @RapportLigneId;
END

DROP TABLE #ValorisationDoublonsMateriel