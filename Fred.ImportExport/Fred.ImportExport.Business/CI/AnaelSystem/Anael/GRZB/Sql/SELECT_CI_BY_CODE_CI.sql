﻿SELECT 
A.RSTE As CodeSociete, 
A.RSESY As CodeAffaire,
CASE 
    WHEN  A.AOUVA >0 AND  A.AOUVM >0 AND A.AOUVJ >0
    THEN   CAST(A.AOUVA || '-' || A.AOUVM || '-' || A.AOUVJ  As DATE)
    ELSE NULL 
END  As DateOuverture,
CASE 
    WHEN  A.AFERA >0 AND  A.AFERM >0 AND A.AFERJ >0
    THEN   CAST(A.AFERA || '-' || A.AFERM || '-' || A.AFERJ  As DATE)
    ELSE NULL
END As DateFermeture,
A.ANOM1 As LibelleLong, 
A.ANOMR As Libelle,
ETAB.RETAB As CodeEtablissement,
A.AACTIV As ChantierFRED
FROM AXFILE.FCA010P1 A
LEFT JOIN AXFILE.FCA180P1 ETAB ON ETAB.RSTE = A.RSTE AND ETAB.RSECT =A.RSESY AND ETAB.REX=A.REX
INNER JOIN AXSPE.SSTEECP B on A.RSTE = B.RSTE AND A.REX = B.REXEC
WHERE 1=1
AND A.REX = B.REXEC
--AND (A.RSESY like '3%' or A.RSESY like '4%' or or A.RSESY like '2%' or or A.RSESY like '8%')  
AND A.RSTE = {0}
--AND (1={1} OR A.RSESY IN ( '234952','234953'))
AND (1={1} OR A.RSESY IN ( {2}))
