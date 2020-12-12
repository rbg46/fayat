SELECT EC.CIID, DATECOMPTABLE, EC.FAMILLEOPERATIONDIVERSEID, NATUREID, COUNT(*) NBOCCURENCE 
INTO #SRC
FROM FRED_ECRITURE_COMPTABLE EC
INNER JOIN  FRED_FAMILLE_OPERATION_DIVERSE FOD ON FOD.FAMILLEOPERATIONDIVERSEID = EC.FAMILLEOPERATIONDIVERSEID
WHERE FOD.ISACCRUED = 1
AND SOCIETEID = 1
GROUP BY EC.CIID, DATECOMPTABLE, EC.FAMILLEOPERATIONDIVERSEID, NATUREID
HAVING COUNT(*)>1

DELETE EC FROM FRED_ECRITURE_COMPTABLE ec 
INNER JOIN  #SRC s on s.CiId = ec.CiId and s.DateComptable = ec.DateComptable and s.FamilleOperationDiverseId  = ec.FamilleOperationDiverseId
LEFT OUTER JOIN FRED_ECRITURE_COMPTABLE_CUMUL ecc on ecc.CiId = ec.CiId and ecc.EcritureComptableId = ec.EcritureComptableId
WHERE ecc.EcritureComptableId is null 


DELETE EC FROM FRED_ECRITURE_COMPTABLE ec 
INNER JOIN  #SRC s on s.CiId = ec.CiId and s.DateComptable = ec.DateComptable and s.FamilleOperationDiverseId  = ec.FamilleOperationDiverseId
LEFT OUTER JOIN FRED_ECRITURE_COMPTABLE_CUMUL ecc on ecc.CiId = ec.CiId and ecc.EcritureComptableId = ec.EcritureComptableId
WHERE ecc.EcritureComptableId is NOT null 
AND ec.Libelle LIKE 'MATERIEL INTERNE POINTE%'

DROP TABLE #SRC
 
SELECT ecc.EcritureComptableId, ecc.Montant, ecc.NumeroPiece, ecc.CiId, MAX(ecc.DateCreation) as datecreation, COUNT(*) NBOCCURENCE
INTO #SRC2
FROM FRED_ECRITURE_COMPTABLE EC
INNER JOIN  FRED_FAMILLE_OPERATION_DIVERSE FOD ON FOD.FAMILLEOPERATIONDIVERSEID = EC.FAMILLEOPERATIONDIVERSEID
INNER JOIN FRED_ECRITURE_COMPTABLE_CUMUL ecc ON Ec.EcritureComptableId = ECC.EcritureComptableId
WHERE FOD.ISACCRUED = 1
AND SOCIETEID = 1
group by ecc.EcritureComptableId, ecc.Montant, ecc.NumeroPiece, ecc.CiId
HAVING COUNT(*)>1
order by ecc.EcritureComptableId, ecc.Montant
 
DELETE ECC FROM FRED_ECRITURE_COMPTABLE_CUMUL ecc
INNER JOIN #SRC2 src ON ecc.EcritureComptableId = src.EcritureComptableId 
    AND ecc.Montant = src.Montant
    AND ecc.NumeroPiece = src.NumeroPiece
    AND ecc.CiId = src.CiId
    AND ecc.DateCreation = src.datecreation
 
DROP TABLE #SRC2

DELETE FROM FRED_ECRITURE_COMPTABLE where Libelle LIKE 'DEBOURSE ACHATS AVEC COMMANDE%'

SELECT MAX(ec.DateCreation) as dateCreation, ec.DateComptable, ec.CiId, ec.FamilleOperationDiverseId, COUNT(*) NBOCCURENCE
INTO #SRP
from FRED_ECRITURE_COMPTABLE ec
INNER JOIN FRED_FAMILLE_OPERATION_DIVERSE f ON ec.FamilleOperationDiverseId = f.FamilleOperationDiverseId
    AND f.FamilleOperationDiverseId = 2
WHERE ec.NumeroPiece IS NULL
GROUP BY ec.DateComptable, ec.CiId, ec.FamilleOperationDiverseId
HAVING COUNT(*) > 1

DELETE ecc
FROM FRED_ECRITURE_COMPTABLE_CUMUL ecc
INNER JOIN FRED_ECRITURE_COMPTABLE ec ON ecc.EcritureComptableId = ec.EcritureComptableId 
INNER JOIN #SRP srp ON ec.CiId = srp.CiId
    AND ec.DateComptable = srp.DateComptable
    AND ec.FamilleOperationDiverseId = srp.FamilleOperationDiverseId
    AND ec.DateCreation = srp.dateCreation

DELETE ec
FROM FRED_ECRITURE_COMPTABLE ec
INNER JOIN #SRP srp ON ec.CiId = srp.CiId
    AND ec.DateComptable = srp.DateComptable
    AND ec.FamilleOperationDiverseId = srp.FamilleOperationDiverseId
    AND ec.DateCreation = srp.dateCreation

DROP TABLE #SRP


SELECT MIN(ec.DateCreation) as dateCreation, ec.DateComptable, ec.CiId, ec.FamilleOperationDiverseId, COUNT(*) NBOCCURENCE
INTO #SRP2
from FRED_ECRITURE_COMPTABLE ec
INNER JOIN FRED_FAMILLE_OPERATION_DIVERSE f ON ec.FamilleOperationDiverseId = f.FamilleOperationDiverseId
    AND f.FamilleOperationDiverseId = 3
WHERE ec.NumeroPiece IS NULL
GROUP BY ec.DateComptable, ec.CiId, ec.FamilleOperationDiverseId
HAVING COUNT(*) > 1

DELETE ecc
FROM FRED_ECRITURE_COMPTABLE_CUMUL ecc
INNER JOIN FRED_ECRITURE_COMPTABLE ec ON ecc.EcritureComptableId = ec.EcritureComptableId 
INNER JOIN #SRP2 srp ON ec.CiId = srp.CiId
    AND ec.DateComptable = srp.DateComptable
    AND ec.FamilleOperationDiverseId = srp.FamilleOperationDiverseId
    AND ec.DateCreation = srp.dateCreation

DELETE ec
FROM FRED_ECRITURE_COMPTABLE ec
INNER JOIN #SRP2 srp ON ec.CiId = srp.CiId
    AND ec.DateComptable = srp.DateComptable
    AND ec.FamilleOperationDiverseId = srp.FamilleOperationDiverseId
    AND ec.DateCreation = srp.dateCreation

DROP TABLE #SRP2