-- On récupère les valorisations intérimaires négatives avec une tâche "ECART INTERIM"
SELECT V.*, T.Libelle
INTO #InterimaireAvecTacheEcart
FROM FRED_VALORISATION V
INNER JOIN FRED_PERSONNEL P on P.PersonnelId = V.PersonnelId
INNER JOIN FRED_TACHE T on T.TacheId = V.TacheId and T.CiId = V.CiId
WHERE V.PersonnelId is not null
AND P.IsInterimaire = 1
AND V.Source  = 'Annulation intérimaire'
AND T.TacheType = 8 and T.Active = 1 and T.Niveau = 3

-- On récupère les valorisations intérimaires négatives avec une tâche différente de "ECART INTERIM"
SELECT V.*, T.Libelle
INTO #InterimaireSansTacheEcart
FROM FRED_VALORISATION V
INNER JOIN FRED_PERSONNEL P on P.PersonnelId = V.PersonnelId
INNER JOIN FRED_TACHE T on T.TacheId = V.TacheId and T.CiId = V.CiId
LEFT OUTER JOIN #InterimaireAvecTacheEcart IATE on IATE.ValorisationId = V.ValorisationId
WHERE V.PersonnelId is not null
AND P.IsInterimaire = 1
AND V.Source  = 'Annulation intérimaire'
AND IATE.ValorisationId IS NULL

-- On met à jour la liste temporaire en erreur avec la bonne tâche : "ECART INTERIM"
UPDATE ISTE
SET ISTE.TacheId = T.TacheId, ISTE.Libelle = T.Libelle
FROM #InterimaireSansTacheEcart ISTE
INNER JOIN FRED_TACHE T on T.CiId = ISTE.CiId
WHERE T.TacheType = 8 and T.Active = 1 and T.Niveau = 3

-- On met à jour la table en erreur avec la bonne tâche : "ECART INTERIM" depuis la liste temporaire
UPDATE V
SET V.TacheId = ISTE.TacheId
FROM FRED_VALORISATION V
INNER JOIN #InterimaireSansTacheEcart ISTE on V.ValorisationId = ISTE.ValorisationId

DROP TABLE #InterimaireAvecTacheEcart
DROP TABLE #InterimaireSansTacheEcart