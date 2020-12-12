-- =======================================================================================================================================
-- Author:		JS Moreau  20/08/2019
--
-- Description:
--      - Correction des montant de T4Ressources ne correspondant pas au PU*Quantité
--      - Correction des montant de Budget_T4 ne correspondant pas à la somme des montants de T4_Ressources
--
-- =======================================================================================================================================


-- Mise à jour des montants invalides
UPDATE FRED_BUDGET_T4_RESSOURCE
SET Montant = PU * Quantite
WHERE (PU * Quantite) != Montant
AND PU IS NOT NULL
AND Quantite IS NOT NULL
AND Montant IS NOT NULL

-- Mise à jour des montants nulls et qui ne devraient pas l'etre
UPDATE FRED_BUDGET_T4_RESSOURCE
SET Montant = PU * Quantite
WHERE PU IS NOT NULL
AND Quantite IS NOT NULL
AND Montant IS NULL

-- Mise à jour des montants non nulls et qui devraient l'etre
UPDATE FRED_BUDGET_T4_RESSOURCE
SET Montant = NULL
WHERE (PU IS NULL OR Quantite IS NULL)
AND Montant IS NOT NULL

-- Mise à jour des montants de budget T4 à partir du cumul des montants de ressources
UPDATE T4U
SET T4U.MontantT4 = T4RSUM.SumT4RMontant
FROM FRED_BUDGET_T4 T4U 
INNER JOIN (
	SELECT T4.BudgetT4Id, SUM(T4R.Montant) SumT4RMontant
	FROM FRED_BUDGET_T4 T4 
	INNER JOIN FRED_BUDGET_T4_RESSOURCE T4R ON T4.BudgetT4Id = T4R.BudgetT4Id
	WHERE Montant IS NOT NULL
	GROUP BY T4.BudgetT4Id,MontantT4
	HAVING MontantT4 != SUM(T4R.Montant)) T4RSUM ON T4U.BudgetT4Id = T4RSUM.BudgetT4Id
