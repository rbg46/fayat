WITH duplicates AS (
	SELECT
		NumeroPiece,
		Montant,
		ROW_NUMBER() OVER (
			PARTITION BY
				NumeroPiece,
				Montant
			ORDER BY
				NumeroPiece,
				Montant
		) RowNumber
	FROM FRED_ECRITURE_COMPTABLE_CUMUL
)

DELETE
FROM duplicates
WHERE RowNumber > 1

UPDATE EC
SET EC.Montant = Total
FROM
	FRED_ECRITURE_COMPTABLE EC
	JOIN (
		SELECT EcritureComptableId, SUM(Montant) AS Total
		FROM FRED_ECRITURE_COMPTABLE_CUMUL
		GROUP BY EcritureComptableId
	) ECC ON ECC.EcritureComptableId = EC.EcritureComptableId
WHERE
	EC.Montant <> Total
	AND EC.EcritureComptableId NOT IN (322785, 498310, 498072) -- Voir PJ ticket 14007