-- --------------------------------------------------
-- MEP 2018 - Correction des données Code 
-- --------------------------------------------------

-- CORRECTION CODE ETABLISSEMENT PAIE 
UPDATE FRED_ETABLISSEMENT_PAIE SET CODE = '00' WHERE CODE = '0';

-- Correction HORS REGION pour Etablissement 10 
UPDATE FRED_ETABLISSEMENT_PAIE SET HorsRegion = 1  where Code = '10'