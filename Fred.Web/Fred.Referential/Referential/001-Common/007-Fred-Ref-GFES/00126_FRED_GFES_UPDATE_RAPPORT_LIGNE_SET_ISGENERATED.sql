UPDATE RL 
SET IsGenerated = 1 
FROM FRED_CI c
INNER JOIN  FRED_SOCIETE s on s.SocieteId = c.SocieteId
INNER JOIN  FRED_GROUPE g on g.groupeid = s.groupeid
INNER JOIN  FRED_RAPPORT_LIGNE rl on rl.CiId = c.CiId
WHERE 1=1
AND g.code ='GFES'
AND PersonnelId IS NULL
AND IsGenerated = 0