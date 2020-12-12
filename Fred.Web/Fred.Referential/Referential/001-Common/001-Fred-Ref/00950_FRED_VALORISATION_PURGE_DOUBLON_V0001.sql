-- =======================================================================================================================================
-- Author:		Yannick DEFAY
--
-- Description:
--      - Purge des doublons de valorisation, et des valorisations erronées
--
-- =======================================================================================================================================

-- Valorisation materiel qui ont un materiel Id différent du pointage
BEGIN TRAN
DELETE FROM FRED_VALORISATION WHERE ValorisationId IN (
SELECT V.ValorisationId
FROM FRED_VALORISATION V
INNER JOIN FRED_RAPPORT_LIGNE RL ON RL.RapportLigneId = V.RapportLigneId
INNER JOIN FRED_RAPPORT_LIGNE_TACHE RLT ON RLT.RapportLigneId = V.RapportLigneId AND RLT.TacheId = V.TacheId
WHERE V.MaterielId != RL.MaterielId
AND V.MaterielId IS NOT NULL
AND RLT.TacheId = V.TacheId
AND V.VerrouPeriode = 0)
COMMIT TRAN

-- Valorisation personnel qui ont un personnel Id différent du pointage
BEGIN TRAN
DELETE FROM FRED_VALORISATION WHERE ValorisationId IN (
SELECT V.ValorisationId 
FROM FRED_VALORISATION V
INNER JOIN FRED_RAPPORT_LIGNE RL ON RL.RapportLigneId = V.RapportLigneId
INNER JOIN FRED_RAPPORT_LIGNE_TACHE RLT ON RLT.RapportLigneId = V.RapportLigneId AND RLT.TacheId = V.TacheId
WHERE V.PersonnelId != RL.PersonnelId
AND V.PersonnelId IS NOT NULL
AND V.VerrouPeriode = 0)
COMMIT TRAN

-- Suppression des doublons de valorisation personnel
BEGIN TRAN
DELETE FRED_VALORISATION WHERE ValorisationId IN (
SELECT V.ValorisationId
FROM
(
SELECT Max(ValorisationId) ValoToKeep, RapportLigneId, TacheId, PersonnelId
FROM FRED_VALORISATION
WHERE PersonnelId IS NOT NULL
  AND VerrouPeriode = 0
GROUP BY RapportLigneId, TacheId, PersonnelId
HAVING COUNT(*) > 1
) VDUP
INNER JOIN FRED_VALORISATION V ON V.RapportLigneId = VDUP.RapportLigneId AND V.TacheId = VDUP.TacheId AND V.PersonnelId = VDUP.PersonnelId
WHERE V.ValorisationId != VDUP.ValoToKeep)
COMMIT TRAN

-- Suppression des doublons de valorisation matériel
BEGIN TRAN
DELETE FRED_VALORISATION WHERE ValorisationId IN (
SELECT V.ValorisationId
FROM
(
SELECT Max(ValorisationId) ValoToKeep, RapportLigneId, TacheId, MaterielId
FROM FRED_VALORISATION
WHERE MaterielId IS NOT NULL
  AND VerrouPeriode = 0
GROUP BY RapportLigneId, TacheId, MaterielId
HAVING COUNT(*) > 1
) VDUP
INNER JOIN FRED_VALORISATION V ON V.RapportLigneId = VDUP.RapportLigneId AND V.TacheId = VDUP.TacheId AND V.MaterielId = VDUP.MaterielId
WHERE V.ValorisationId != VDUP.ValoToKeep)
COMMIT TRAN
