SELECT t.TacheId, t.CiId
INTO #TacheLitige
FROM FRED_TACHE t WHERE Code = '999981'

SELECT ff.DepenseAchatFactureEcartId
INTO #FacturationType10And11
FROM dbo.FRED_FACTURATION ff
WHERE ff.FacturationTypeId IN (10, 11)

SELECT DISTINCT fda.DepenseId, fda.TacheId
INTO #DepenseType10And11
FROM dbo.FRED_DEPENSE_ACHAT fda
INNER JOIN #FacturationType10And11 f ON f.DepenseAchatFactureEcartId = fda.DepenseId

SELECT DISTINCT fda.DepenseId
INTO #DepenseUpdateType10And11
FROM dbo.FRED_DEPENSE_ACHAT fda
INNER JOIN dbo.FRED_TACHE ft ON fda.TacheId = ft.TacheId
INNER JOIN #FacturationType10And11 f ON f.DepenseAchatFactureEcartId = fda.DepenseId
LEFT OUTER JOIN #TacheLitige tl ON ft.TacheId = tl.TacheId
WHERE 1 = 1
AND tl.TacheId IS NULL

UPDATE fda
SET fda.TacheId = tl.TacheId
FROM dbo.FRED_DEPENSE_ACHAT fda
INNER JOIN #TacheLitige tl ON tl.CiId = fda.CiId
INNER JOIN #DepenseUpdateType10And11 du ON fda.DepenseId = du.DepenseId
WHERE fda.DepenseId = du.DepenseId

DROP TABLE #FacturationType10And11, #DepenseType10And11, #DepenseUpdateType10And11

SELECT ff.DepenseAchatFarId
INTO #FacturationType5And6
FROM dbo.FRED_FACTURATION ff
WHERE ff.FacturationTypeId IN (5, 6)

SELECT DISTINCT fda.DepenseId, fda.TacheId
INTO #DepenseType5And6
FROM dbo.FRED_DEPENSE_ACHAT fda
INNER JOIN #FacturationType5And6 f ON f.DepenseAchatFarId = fda.DepenseId

SELECT DISTINCT fda.DepenseId
INTO #DepenseUpdateType5And6
FROM dbo.FRED_DEPENSE_ACHAT fda
INNER JOIN dbo.FRED_TACHE ft ON fda.TacheId = ft.TacheId
INNER JOIN #FacturationType5And6 f ON f.DepenseAchatFarId = fda.DepenseId
LEFT OUTER JOIN #TacheLitige tl ON ft.TacheId = tl.TacheId
WHERE 1 = 1
AND tl.TacheId IS NULL

UPDATE fda
SET fda.TacheId = tl.TacheId
FROM dbo.FRED_DEPENSE_ACHAT fda
INNER JOIN #TacheLitige tl ON tl.CiId = fda.CiId
INNER JOIN #DepenseUpdateType5And6 du ON fda.DepenseId = du.DepenseId
WHERE fda.DepenseId = du.DepenseId

DROP TABLE #FacturationType5And6, #DepenseType5And6, #DepenseUpdateType5And6

SELECT ff.DepenseAchatReceptionId
INTO #FacturationType1
FROM dbo.FRED_FACTURATION ff
WHERE ff.FacturationTypeId = 1

SELECT DISTINCT fda.DepenseId, fda.TacheId, fda.DepenseParentId
INTO #DepenseParent
FROM dbo.FRED_DEPENSE_ACHAT fda
inner join #FacturationType1 f on f.DepenseAchatReceptionId = fda.DepenseParentId
WHERE 1 = 1
AND fda.DepenseTypeId IN (2, 3, 4, 7, 8)

SELECT DISTINCT fda.TacheId, fda.DepenseId
INTO #DepenseType1
FROM dbo.FRED_DEPENSE_ACHAT fda
inner join #FacturationType1 f on f.DepenseAchatReceptionId = fda.DepenseId
WHERE 1=1
AND fda.DepenseTypeId = 1

SELECT DISTINCT fda.DepenseId
INTO #DepenseUpdateType1
FROM dbo.FRED_DEPENSE_ACHAT fda
INNER JOIN FRED_TACHE ft ON fda.TacheId = ft.TacheId
INNER JOIN #FacturationType1 f ON f.DepenseAchatReceptionId = fda.DepenseParentId
LEFT OUTER JOIN #TacheLitige tl ON tl.TacheId = fda.TacheId
WHERE 1 = 1
AND tl.TacheId IS NULL

UPDATE FDA
SET fda.TacheId = tl.TacheId
FROM dbo.FRED_DEPENSE_ACHAT fda
INNER JOIN #TacheLitige tl ON tl.CiId = fda.CiId
INNER JOIN #DepenseUpdateType1 du ON fda.DepenseId = du.DepenseId
WHERE fda.DepenseId = du.DepenseId

DROP TABLE #FacturationType1, #DepenseParent, #DepenseType1, #TacheLitige, #DepenseUpdateType1