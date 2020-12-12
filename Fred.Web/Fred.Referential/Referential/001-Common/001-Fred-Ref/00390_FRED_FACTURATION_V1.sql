-- =======================================================================================================================================
-- Author:		Nicolas Pinsard
--
-- Description:
--      - Change certains éléments null en 0 (TI 3996)
--
-- =======================================================================================================================================

BEGIN TRAN

	UPDATE FRED_FACTURATION SET EcartPU = 0 WHERE EcartPU IS NULL
	UPDATE FRED_FACTURATION SET Quantite = 0 WHERE Quantite IS NULL
	UPDATE FRED_FACTURATION SET EcartQuantite = 0 WHERE EcartQuantite IS NULL
	UPDATE FRED_FACTURATION SET QuantiteReconduite = 0 WHERE QuantiteReconduite IS NULL
	UPDATE FRED_FACTURATION SET MouvementFarHt = 0 WHERE MouvementFarHt IS NULL
	UPDATE FRED_FACTURATION SET TotalFarHt = 0 WHERE TotalFarHt IS NULL
	UPDATE FRED_FACTURATION SET QuantiteFar = 0 WHERE QuantiteFar IS NULL

COMMIT TRAN
