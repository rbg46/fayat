-- =======================================================================================================================================
-- Author:		JS MOREAU 15/02/2019
--
-- Description:
--      - Recalcule des quantiteSD nulles ou mal arrondies
--
-- =======================================================================================================================================

UPDATE [FRED_BUDGET_T4_RESSOURCE]
SET QuantiteSD = T.QuantiteSDCalculee
FROM
  (SELECT R.BudgetT4SousDetailId,
          R.QuantiteSD,
          Round((Quantite/QuantiteARealiser)*QuantiteDeBase, 2) QuantiteSDCalculee
   FROM [FRED_BUDGET_T4_RESSOURCE] R
   INNER JOIN [dbo].[FRED_BUDGET_T4] T4 ON T4.[BudgetT4Id] = R.[BudgetT4Id]
   WHERE R.Quantite IS NOT NULL
     AND T4.QuantiteARealiser IS NOT NULL
     AND T4.QuantiteARealiser > 0
     AND T4.QuantiteDeBase IS NOT NULL
     AND T4.QuantiteDeBase > 0) T
WHERE (T.QuantiteSD != T.QuantiteSDCalculee
       OR T.QuantiteSD IS NULL)
  AND [FRED_BUDGET_T4_RESSOURCE].BudgetT4SousDetailId = T.BudgetT4SousDetailId
