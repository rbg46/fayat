-- =======================================================================================================================================
-- Author:		Yannick DEFAY  08/02/2019
--
-- Description:
--      - Supprime les doublons dans la table FRED_RAPPORT_LIGNE_PRIME
--
-- =======================================================================================================================================

BEGIN TRAN

DELETE FROM FRED_RAPPORT_LIGNE_PRIME
WHERE RapportLignePrimeId IN (
-- On prend les doublons 
SELECT DISTINCT RapportLignePrimeId
FROM FRED_RAPPORT_LIGNE_PRIME P1
WHERE EXISTS (  SELECT *
				FROM FRED_RAPPORT_LIGNE_PRIME P2
				WHERE P1.RapportLignePrimeId <> p2.RapportLignePrimeId
				AND   P1.RapportLigneId = P2.RapportLigneId
				AND	P1.PrimeId = P2.PrimeId)
	  AND P1.RapportLignePrimeId NOT IN -- On exclut un enregistrement unique de rapportLignePrime pour chaque couple RapportLigneId/PrimeId
	  (	SELECT MIN(P3.RapportLignePrimeId) As RapportLignePrimeId
		FROM FRED_RAPPORT_LIGNE_PRIME P3
		JOIN (	SELECT DISTINCT *
				FROM FRED_RAPPORT_LIGNE_PRIME P3
				WHERE EXISTS (  SELECT *
								FROM FRED_RAPPORT_LIGNE_PRIME P4
								WHERE P3.RapportLignePrimeId <> P4.RapportLignePrimeId
								AND   P3.RapportLigneId = P4.RapportLigneId
								AND	P3.PrimeId = P4.PrimeId )
				) P5 ON P5.RapportLignePrimeId = P3.RapportLignePrimeId
		GROUP BY P3.RapportLigneId, P3.PrimeId ))

COMMIT TRAN