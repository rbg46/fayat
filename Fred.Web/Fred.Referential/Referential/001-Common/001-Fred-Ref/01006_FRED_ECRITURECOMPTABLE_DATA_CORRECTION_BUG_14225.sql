-- =======================================================================================================================================
-- Description:
--          BUG 14225 : Le montant des cumuls n'est pas égal au montant de l'ecriture comptable
--
-- =======================================================================================================================================

UPDATE EC
SET EC.Montant = ECC.Total
FROM
    FRED_ECRITURE_COMPTABLE EC
    JOIN (
        SELECT EcritureComptableId, SUM(Montant) AS Total
        FROM FRED_ECRITURE_COMPTABLE_CUMUL
        GROUP BY EcritureComptableId
    ) ECC ON
        ECC.EcritureComptableId = EC.EcritureComptableId
        AND Total <> EC.Montant
    JOIN FRED_CI CI ON CI.CiId = EC.CiId
    LEFT JOIN FRED_DATES_CLOTURE_COMPTABLE DCC ON
        DCC.CiId = CI.CiId
        AND DCC.Annee = YEAR(EC.DateComptable)
        AND DCC.Mois = MONTH(EC.DateComptable)
        AND DateCloture IS NOT NULL
        AND DCC.Historique = 0
WHERE
    DCC.DatesClotureComptableId IS NULL