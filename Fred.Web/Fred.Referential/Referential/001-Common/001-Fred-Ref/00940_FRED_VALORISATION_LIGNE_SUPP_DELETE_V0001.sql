-- =======================================================================================================================================
-- Author:		JS Moreau  20/08/2019
--
-- Description:
--      - Suppression Valorisations liées à des lignes supprimées et pour lesquelles le flag VerrouPeriode n'est pas actif
--
-- =======================================================================================================================================
DELETE FRED_VALORISATION 
WHERE ValorisationId IN (
	(SELECT ValorisationId
	   FROM FRED_VALORISATION V
	   INNER JOIN  FRED_RAPPORT_LIGNE RL ON RL.RapportLigneId = V.RapportLigneId
	   AND V.VerrouPeriode = 0
	   AND RL.DateSuppression IS NOT NULL))