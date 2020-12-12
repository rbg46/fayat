

-- IsPrimeAstreinte = NULL pour les codes différents de : 
-- 'ASTRS', 'ASTRWE' 

UPDATE P
SET 
	P.IsPrimeAstreinte = NULL
FROM [FRED_PRIME] P
INNER JOIN FRED_SOCIETE soc on P.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%' 
AND P.Code NOT IN ('ASTRS', 'ASTRWE')