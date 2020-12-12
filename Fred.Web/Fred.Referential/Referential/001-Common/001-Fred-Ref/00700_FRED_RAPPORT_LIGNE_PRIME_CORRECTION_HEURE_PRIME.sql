-- =======================================================================================================================================
-- Author:		Yannick DEFAY 15/02/2019
--
-- Description:
--      - remet à null les HeurePrime pour les prime journalière
--
-- =======================================================================================================================================


BEGIN TRAN
    UPDATE LP
    SET LP.HeurePrime = NULL
    FROM  FRED_RAPPORT_LIGNE_PRIME LP,
	      FRED_PRIME P
    WHERE   LP.HeurePrime IS NOT NULL
	    AND P.PrimeId = LP.PrimeId
	    AND P.PrimeType = 0
COMMIT TRAN