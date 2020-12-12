-- =======================================================================================================================================
-- Author:		Yannick DEFAY  08/02/2019
--
-- Description:
--      - Réaffecte le bon code prime astreinte pour les pointages weekend
--
-- =======================================================================================================================================

BEGIN TRAN

DECLARE @rapportLignePrimeId INT
DECLARE @primeId INT

DECLARE db_cursor CURSOR FOR
SELECT LP.RapportLignePrimeId, LP.PrimeId
FROM FRED_RAPPORT_LIGNE L, FRED_RAPPORT_LIGNE_PRIME LP, FRED_PRIME P
WHERE	L.RapportLigneId = LP.RapportLigneId
	AND LP.PrimeId = P.PrimeId
	AND (P.Code LIKE 'ASTRS')
	AND CHOOSE(DATEPART(dw, L.DatePointage), 1, 0, 0, 0 ,0, 0, 1) = 1
ORDER BY L.DatePointage

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @rapportLignePrimeId, @primeId

WHILE @@FETCH_STATUS = 0  
BEGIN  
	DECLARE @primeIdWeekend INT = (SELECT TOP(1) primeId FROM FRED_PRIME WHERE SocieteId = (SELECT TOP(1) SocieteId FROM FRED_PRIME WHERE PrimeId = @primeId) AND Code LIKE 'ASTRW');

	IF (@primeIdWeekend IS NOT NULL)
	BEGIN
	UPDATE FRED_RAPPORT_LIGNE_PRIME SET PrimeId = @primeIdWeekend WHERE RapportLignePrimeId = @rapportLignePrimeId
	END

	FETCH NEXT FROM db_cursor INTO @rapportLignePrimeId, @primeId     
END 

CLOSE db_cursor  
DEALLOCATE db_cursor

COMMIT TRAN
