-- =======================================================================================================================================
-- Author:		Yannick DEFAY
--
-- Description:
--      - Mise à jour des lignes de rapport en doublon pour quelles pointent toutes vers un rapport unique par date et ciId
--
-- =======================================================================================================================================

-- Mise à des identifiants de rapports
BEGIN TRAN
UPDATE RL 
SET RL.RapportId = A.RapportToKeep
FROM FRED_RAPPORT_LIGNE RL
INNER JOIN 
(SELECT R.RapportId, RDUP.RapportToKeep 
FROM
	(
	SELECT MIN(RapportId) RapportToKeep, R.CiId CiId, R.DateChantier
	FROM FRED_RAPPORT R, FRED_CI C, FRED_SOCIETE S, FRED_GROUPE G
	WHERE R.CiId = C.CiId
	AND C.SocieteId = S.SocieteId
	AND S.GroupeId = G.GroupeId
	AND G.Code = 'GFES'
	GROUP BY R.CiId, R.DateChantier
	HAVING COUNT(R.RapportId) > 1) RDUP
INNER JOIN FRED_RAPPORT R ON R.CiId = RDUP.CiId AND R.DateChantier = RDUP.DateChantier
WHERE R.RapportId != RDUP.RapportToKeep) A ON RL.RapportId = A.RapportId
COMMIT TRAN

-- Mise à jour des valeurs des identifiants de rapport de la table Valorisation
BEGIN TRAN
UPDATE V 
SET V.RapportId = RL.RapportId
FROM FRED_VALORISATION V, FRED_RAPPORT_LIGNE RL
WHERE V.RapportLigneId = RL.RapportLigneId
AND V.RapportId != RL.RapportId
COMMIT TRAN

-- Suppression des doublons
BEGIN TRAN
DELETE FRED_RAPPORT WHERE RapportId IN (
SELECT R.RapportId
FROM
(
SELECT MIN(RapportId) RapportToKeep, R.CiId CiId, R.DateChantier
FROM FRED_RAPPORT R, FRED_CI C, FRED_SOCIETE S, FRED_GROUPE G
WHERE R.CiId = C.CiId
AND C.SocieteId = S.SocieteId
AND S.GroupeId = G.GroupeId
AND G.Code = 'GFES'
GROUP BY R.CiId, R.DateChantier
HAVING COUNT(R.RapportId) > 1
) RDUP
INNER JOIN FRED_RAPPORT R ON R.CiId = RDUP.CiId AND R.DateChantier = RDUP.DateChantier
WHERE R.RapportId != RDUP.RapportToKeep)
COMMIT TRAN