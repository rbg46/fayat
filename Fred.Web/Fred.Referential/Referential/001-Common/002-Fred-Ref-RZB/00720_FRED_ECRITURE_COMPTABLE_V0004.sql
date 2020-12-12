SELECT * 
INTO #SRC 
FROM (
        SELECT ecc.DateComptable, ecc.NumeroPiece, ecc.Montant, ecc.EcritureComptableId,ecc.CiId,ecc.EcritureComptableCumulId, ROW_NUMBER()
        OVER (PARTITION BY ecc.DateComptable, ecc.NumeroPiece, ecc.Montant, ecc.EcritureComptableId,ecc.CiId ORDER BY ecc.DateComptable, ecc.NumeroPiece, ecc.Montant, ecc.EcritureComptableId,ecc.CiId) NbDuplicate
        FROM FRED_ECRITURE_COMPTABLE_CUMUL ecc
        INNER JOIN FRED_ECRITURE_COMPTABLE ec on ec.EcritureComptableId = ecc.EcritureComptableId and ec.CiId <> ecc.CiId 
    )DuplicateCumulativeAccountingEntryLines  
WHERE DuplicateCumulativeAccountingEntryLines .NbDuplicate > 1

DELETE ECC FROM FRED_ECRITURE_COMPTABLE_CUMUL ECC
INNER JOIN #SRC SRC on SRC.EcritureComptableCumulId = ECC.EcritureComptableCumulId

DROP TABLE #SRC