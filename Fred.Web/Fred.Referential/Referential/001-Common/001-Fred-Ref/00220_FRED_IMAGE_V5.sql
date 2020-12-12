
-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

WITH CTE AS(
   SELECT Path, RN = ROW_NUMBER() OVER(PARTITION BY Path ORDER BY Path)
   FROM FRED_IMAGE
)
DELETE FROM CTE WHERE RN > 1 