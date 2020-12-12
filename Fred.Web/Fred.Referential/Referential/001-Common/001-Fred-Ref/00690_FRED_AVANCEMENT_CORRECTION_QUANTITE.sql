-- =======================================================================================================================================
-- Author:		JS MOREAU 15/02/2019
--
-- Description:
--      - remplace les quantité d'avancement à 0 par null pour éviter les bugs dans le controle budgétaire
--
-- =======================================================================================================================================


UPDATE FRED_AVANCEMENT
SET QuantiteSousDetailAvancee = NULL
WHERE QuantiteSousDetailAvancee = 0