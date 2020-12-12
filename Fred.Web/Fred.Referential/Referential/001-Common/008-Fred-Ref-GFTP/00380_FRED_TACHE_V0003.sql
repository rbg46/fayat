SELECT SocieteId INTO #SOCIETE FROM FRED_SOCIETE WHERE Code = '0001'
SELECT CiId INTO #CI FROM FRED_CI INNER JOIN #SOCIETE ON #SOCIETE.SocieteId = FRED_CI.SocieteId
DECLARE @TACHEID int

-- On récupère l'Id de la première Tache de niveau 2, de Code 9999 appartenant à un CI d'une société FayatTP
SELECT TOP (1) @TACHEID = TacheId
FROM FRED_TACHE
INNER JOIN #CI ON #CI.CiId = FRED_TACHE.CiId
WHERE Niveau = 2 
AND Code = '9999'

-- On Update le ParentId avec l'Id trouvé sur la requête ci dessus (@TACHEID) de toutes les Taches appartenant à un CI d'une société FayatTP, de Libellé 'ECART RECETTES' ou de ParentId donné
UPDATE FRED_TACHE
SET ParentId = @TACHEID
FROM FRED_TACHE
INNER JOIN #CI ON #CI.CiId = FRED_TACHE.CiId
WHERE 1 = 1 
AND (ParentId IN(114571, 5) OR Libelle = 'ECART RECETTES')
DROP TABLE #SOCIETE
DROP TABLE #CI