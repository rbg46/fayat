SELECT CIid
INTO #CI
FROM [dbo].[FRED_CI]
WHERE Code = '411125' 
and Libelle = 'ORANO RENFORCEMT ESX ESY'
and SocieteId = 1 -- RZB
 

DELETE t1
FROM [dbo].[FRED_VALORISATION] AS t1
INNER JOIN [dbo].[FRED_VALORISATION] AS t2 ON t1.CiId = t2.CiId
INNER JOIN #CI CI on CI.CiId = t1.CiId
AND t1.RapportId = t2.RapportId
AND t1.TacheId = t2.TacheId
AND t1.ReferentielEtenduId = t2.ReferentielEtenduId
AND t1.RapportLigneId = t2.RapportLigneId
AND t1."Date" = t2."Date"
WHERE t1.DateCreation < t2.DateCreation


DROP TABLE #CI