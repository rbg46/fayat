/*Récupération des tâches de niveau 3*/
SELECT DISTINCT libelle, Code, TacheParDefaut,Niveau,Active, TacheType 
INTO #TacheN3
FROM fred_tache
WHERE TacheType in(11, 12, 13) --Les ID étant en dur dans le code et commun à tous les environnments on peux les laisser 

/*Liste de tous les CI qui n'ont pas ces tâches*/
SELECT DISTINCT tache.CiId
INTO #Temp
FROM FRED_TACHE tache
INNER JOIN FRED_CI c on c.CiId = tache.CiId
INNER JOIN FRED_SOCIETE s  on s.SocieteId = c.SocieteId
LEFT OUTER JOIN #TacheN3 N3 on N3.Libelle = tache.Libelle and tache.Libelle is null
WHERE 1=1
AND s.SocieteId = 1

/*Récupération des tâches de niveau 2 d'écart pour les CI qui n'ont pas les tâches de niveau 3*/
SELECT tache.* 
INTO #TacheN2
FROM FRED_TACHE tache
INNER JOIN FRED_CI c on c.CiId = tache.CiId
INNER JOIN FRED_SOCIETE s  on s.SocieteId = c.SocieteId
WHERE 1=1 
AND s.SocieteId = 1
AND tache.Code = '9999'
AND tache.Libelle = 'ECART'

INSERT INTO FRED_TACHE(Code, Libelle, TacheParDefaut, CiId, ParentId, Niveau, Active, TacheType, DateCreation, AuteurCreationId)
SELECT N3.Code, N3.Libelle, N3.TacheParDefaut, t.CiId, N2.TacheId, N3.Niveau, N3.Active, N3.TacheType, GETDATE(), 1
FROM #Temp t 
INNER JOIN #TacheN2 N2 on N2.CiId = t.CiId
CROSS JOIN #TacheN3 N3

DROP TABLE #TacheN3,#TacheN2,#Temp