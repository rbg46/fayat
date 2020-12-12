-- =======================================================================================================================================
-- Author:		Yoann Collet
--
-- Description:
--      - Supprime les primes en doublons
--
-- =======================================================================================================================================


Delete t2 FROM [FRED_PRIME]  AS t1, [FRED_PRIME] AS t2 WHERE t1.PrimeId < t2.primeId AND t1.Code = t2.Code AND t1.SocieteId = t2.SocieteId