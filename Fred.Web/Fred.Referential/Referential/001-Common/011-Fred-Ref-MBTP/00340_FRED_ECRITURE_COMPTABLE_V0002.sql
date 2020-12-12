SELECT ec.*
INTO #SRC
FROM FRED_ECRITURE_COMPTABLE ec
INNER JOIN FRED_FAMILLE_OPERATION_DIVERSE fod on fod.FamilleOperationDiverseId = ec.FamilleOperationDiverseId
INNER JOIN FRED_SOCIETE fs on fs.SocieteId = fod.SocieteId
WHERE fod.IsAccrued = 1
AND fs.Code = 'MBTP'
AND ec.DateComptable BETWEEN '2019-09-30' AND '2019-10-31' 

DELETE ec FROM FRED_ECRITURE_COMPTABLE ec 
INNER JOIN #SRC s on s.CiId = ec.CiId and s.DateComptable = ec.DateComptable and s.FamilleOperationDiverseId  = ec.FamilleOperationDiverseId
LEFT OUTER JOIN FRED_ECRITURE_COMPTABLE_CUMUL ecc on ecc.CiId = ec.CiId and ecc.EcritureComptableId = ec.EcritureComptableId
WHERE ecc.EcritureComptableId IS NULL 

DROP TABLE #SRC