UPDATE DAP
SET DAP.DateSuppression = NULL
FROM
	FRED_DEPENSE_ACHAT DAF
	JOIN FRED_DEPENSE_TYPE DT ON
		DT.DepenseTypeId = DAF.DepenseTypeId
		AND DT.Libelle = 'Extourne FAR'
	JOIN FRED_DEPENSE_ACHAT DAP ON
		DAP.DepenseId = DAF.DepenseParentId 
		AND DAP.DateSuppression IS NOT NULL
		AND DAP.DateVisaReception IS NOT NULL
		AND (
			DAP.Commentaire IS NULL
			OR  DAP.Commentaire <> 'Reprise Ticket 13489'
		)
WHERE
	DAF.DateSuppression IS NULL