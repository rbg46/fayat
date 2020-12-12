-- =======================================================================================================================================
-- Author:		YDE
--
-- Description:
--      - Supprime les doublons de valorisation qui ne sont plus en corrélations avec les données de la table FRED_RAPPORT_LIGNE_TACHE
--
-- =======================================================================================================================================

BEGIN TRAN
	DELETE
	FROM FRED_VALORISATION
	WHERE ValorisationId NOT IN
	(
		SELECT DISTINCT(V.ValorisationId)
		FROM FRED_VALORISATION V, FRED_RAPPORT_LIGNE_TACHE RLT
		WHERE V.RapportLigneId = RLT.RapportLigneId
		  AND V.TacheId = RLT.TacheId
		  AND VerrouPeriode = 0
	)
COMMIT TRAN