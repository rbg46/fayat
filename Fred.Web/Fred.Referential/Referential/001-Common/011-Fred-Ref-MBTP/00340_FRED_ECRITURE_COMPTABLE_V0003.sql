DECLARE @SocieteId int, @NatureId int, @JournalId int, @FamilleOperationDiverseIdOth int, @FamilleOperationDiverseIdMit int

SELECT @SocieteId = SocieteId 
FROM FRED_SOCIETE 
WHERE Code ='MBTP'

SELECT @NatureId = NatureId 
FROM FRED_NATURE 
WHERE code ='222200' 
AND SocieteId = @SocieteId

SELECT @JournalId = JournalId 
FROM FRED_JOURNAL 
WHERE code ='YA0' 
AND SocieteId = @SocieteId

SELECT @FamilleOperationDiverseIdOth  =  FamilleOperationDiverseId 
FROM FRED_FAMILLE_OPERATION_DIVERSE 
WHERE Code ='OTH'
AND SocieteId = @SocieteId

SELECT @FamilleOperationDiverseIdMit  =  FamilleOperationDiverseId 
FROM FRED_FAMILLE_OPERATION_DIVERSE 
WHERE Code ='MIT'
AND SocieteId = @SocieteId

IF @SocieteId IS NOT NULL AND @NatureId IS NOT NULL AND @JournalId IS NOT NULL AND @FamilleOperationDiverseIdOth IS NOT NULL AND @FamilleOperationDiverseIdMit IS NOT NULL
BEGIN

    SELECT EC.EcritureComptableId
    INTO #SOURCE
    FROM FRED_ECRITURE_COMPTABLE EC 
    INNER JOIN  FRED_CI C ON C.CiId = EC.CiId AND C.SocieteId = @SocieteId
    WHERE 1=1
    AND JournalId = @JournalId
    AND NatureId = @NatureId
    AND FamilleOperationDiverseId = @FamilleOperationDiverseIdOth
    AND DateComptable < '2019-12-01'

    UPDATE EC SET EC.FamilleOperationDiverseId  = @FamilleOperationDiverseIdMit
    FROM FRED_ECRITURE_COMPTABLE EC 
    INNER JOIN #SOURCE SRC ON EC.EcritureComptableId = SRC.EcritureComptableId
    DROP TABLE #SOURCE

END