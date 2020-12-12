-- Récupération des lignes en doublons à supprimer
SELECT *
INTO #LignesASupprimer
FROM ( SELECT v.CiId, v.RapportId, v.RapportLigneId, v.TacheId, v.MaterielId, v.DateCreation, v.PUHT, v.Quantite, v.Montant, v.ValorisationId, v.VerrouPeriode, ROW_NUMBER()
OVER (PARTITION BY v.CiId, v.RapportId, v.RapportLigneId, v.TacheId, v.MaterielId, v.PUHT, v.Quantite, v.Montant 
ORDER BY v.DateCreation desc ) NbDuplicate
FROM     FRED_VALORISATION v
INNER JOIN FRED_CI c on v.CiId = c.CiId
INNER JOIN FRED_SOCIETE s on c.SocieteId = s.SocieteId
WHERE	MaterielId is not null
AND		s.Code = 'MBTP') DuplicateCumulativeAccountingEntryLines
WHERE DuplicateCumulativeAccountingEntryLines .NbDuplicate > 1

-- Récupération des lignes en doublons à mettre à jour
SELECT *
INTO #LignesAMettreAJour
FROM ( SELECT v.CiId, v.RapportId, v.RapportLigneId, v.TacheId, v.MaterielId, v.DateCreation, v.PUHT, v.Quantite, v.Montant, v.ValorisationId, v.VerrouPeriode, ROW_NUMBER()
OVER (PARTITION BY v.CiId, v.RapportId, v.RapportLigneId, v.TacheId, v.MaterielId, v.PUHT, v.Quantite, v.Montant 
ORDER BY v.DateCreation ) NbDuplicate
FROM     FRED_VALORISATION v
INNER JOIN FRED_CI c on v.CiId = c.CiId
INNER JOIN FRED_SOCIETE s on c.SocieteId = s.SocieteId
WHERE	MaterielId is not null
AND		s.Code = 'MBTP') DuplicateCumulativeAccountingEntryLines
WHERE DuplicateCumulativeAccountingEntryLines .NbDuplicate > 1

-- Mise à jour (dans table temporaire) des lignes à conserver avec la valeur du champ VerrouPeriode de l'ancienne ligne (en doublon)
UPDATE lm
SET lm.VerrouPeriode = ls.VerrouPeriode
FROM #LignesAMettreAJour lm
INNER JOIN #LignesASupprimer ls on lm.CiId = ls.CiId and lm.RapportId = ls.RapportId and lm.RapportLigneId = ls.RapportLigneId 
and lm.TacheId = ls.TacheId and lm.MaterielId = ls.MaterielId and lm.PUHT = ls.PUHT and lm.Quantite = ls.Quantite and lm.Montant = ls.Montant
WHERE ls.VerrouPeriode = 1

-- Mise à jour des lignes issues de la table temporaire #LignesAMettreAJour
UPDATE v
SET v.VerrouPeriode = lm.VerrouPeriode
FROM FRED_VALORISATION v
INNER JOIN #LignesAMettreAJour lm on v.ValorisationId = lm.ValorisationId

-- Suppression des lignes en doublon issues de la table temporaire #LignesASupprimer
DELETE v FROM FRED_VALORISATION v
INNER JOIN #LignesASupprimer ls on v.ValorisationId = ls.ValorisationId

DROP TABLE #LignesASupprimer
DROP TABLE #LignesAMettreAJour