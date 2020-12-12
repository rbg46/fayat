-- Récupération des lignes en doublons à supprimer
SELECT *
INTO #LignesPersonnelASupprimer
FROM ( SELECT v.CiId, v.RapportId, v.RapportLigneId, v.TacheId, v.PersonnelId, v.DateCreation, v.PUHT, v.Quantite, v.Montant, v.ValorisationId, v.VerrouPeriode, ROW_NUMBER()
OVER (PARTITION BY v.CiId, v.RapportId, v.RapportLigneId, v.TacheId, v.PersonnelId, v.PUHT, v.Quantite, v.Montant 
ORDER BY v.DateCreation desc ) NbDuplicate
FROM     FRED_VALORISATION v
INNER JOIN FRED_CI c on v.CiId = c.CiId
INNER JOIN FRED_SOCIETE s on c.SocieteId = s.SocieteId
WHERE	PersonnelId is not null
AND		(s.Code = 'MBTP' or s.Code = 'RB')) DuplicateCumulativeAccountingEntryLines
WHERE DuplicateCumulativeAccountingEntryLines .NbDuplicate > 1

-- Récupération des lignes en doublons à mettre à jour
SELECT *
INTO #LignesPersonnelAMettreAJour
FROM ( SELECT v.CiId, v.RapportId, v.RapportLigneId, v.TacheId, v.PersonnelId, v.DateCreation, v.PUHT, v.Quantite, v.Montant, v.ValorisationId, v.VerrouPeriode, ROW_NUMBER()
OVER (PARTITION BY v.CiId, v.RapportId, v.RapportLigneId, v.TacheId, v.PersonnelId, v.PUHT, v.Quantite, v.Montant 
ORDER BY v.DateCreation ) NbDuplicate
FROM     FRED_VALORISATION v
INNER JOIN FRED_CI c on v.CiId = c.CiId
INNER JOIN FRED_SOCIETE s on c.SocieteId = s.SocieteId
WHERE	PersonnelId is not null
AND		(s.Code = 'MBTP' or s.Code = 'RB')) DuplicateCumulativeAccountingEntryLines
WHERE DuplicateCumulativeAccountingEntryLines .NbDuplicate > 1

-- Mise à jour (dans table temporaire) des lignes à conserver avec la valeur du champ VerrouPeriode de l'ancienne ligne (en doublon)
UPDATE lm
SET lm.VerrouPeriode = ls.VerrouPeriode
FROM #LignesPersonnelAMettreAJour lm
INNER JOIN #LignesPersonnelASupprimer ls on lm.CiId = ls.CiId and lm.RapportId = ls.RapportId and lm.RapportLigneId = ls.RapportLigneId 
and lm.TacheId = ls.TacheId and lm.PersonnelId = ls.PersonnelId and lm.PUHT = ls.PUHT and lm.Quantite = ls.Quantite and lm.Montant = ls.Montant
WHERE ls.VerrouPeriode = 1

-- Mise à jour des lignes issues de la table temporaire ##LignesPersonnelAMettreAJour
UPDATE v
SET v.VerrouPeriode = lm.VerrouPeriode
FROM FRED_VALORISATION v
INNER JOIN #LignesPersonnelAMettreAJour lm on v.ValorisationId = lm.ValorisationId

-- Suppression des lignes en doublon issues de la table temporaire ##LignesPersonnelASupprimer
DELETE v FROM FRED_VALORISATION v
INNER JOIN #LignesPersonnelASupprimer ls on v.ValorisationId = ls.ValorisationId

DROP TABLE #LignesPersonnelASupprimer,#LignesPersonnelAMettreAJour