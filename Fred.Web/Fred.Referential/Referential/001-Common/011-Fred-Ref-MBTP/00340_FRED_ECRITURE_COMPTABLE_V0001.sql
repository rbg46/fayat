﻿DELETE FROM FRED_ECRITURE_COMPTABLE
WHERE CiId = 6063
AND DateComptable BETWEEN '2019-09-01' AND '2019-09-30'
AND FamilleOperationDiverseId = 6
AND NumeroPiece IS NOT NULL