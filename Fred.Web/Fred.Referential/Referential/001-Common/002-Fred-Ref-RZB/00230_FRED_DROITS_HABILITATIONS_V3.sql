-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S18 - Droits & Habilitation
-- --------------------------------------------------

-- CORRECTION DEMANDE PAR LA MOA (DOCUMENT Ajustement de Mise en production)
UPDATE FRED_ROLE SET NiveauPaie = 1  WHERE RoleId = 135;
UPDATE FRED_ROLE SET NiveauPaie = 2  WHERE RoleId = 134;
UPDATE FRED_ROLE SET NiveauPaie = 3  WHERE RoleId = 133;
UPDATE FRED_ROLE SET NiveauPaie = 3  WHERE RoleId = 132;
UPDATE FRED_ROLE SET NiveauPaie = 4  WHERE RoleId = 137;
UPDATE FRED_ROLE SET NiveauPaie = 5  WHERE RoleId = 136;
UPDATE FRED_ROLE SET NiveauPaie = 5  WHERE RoleId = 116;
UPDATE FRED_ROLE SET NiveauPaie = 6  WHERE RoleId = 1;
UPDATE FRED_ROLE SET NiveauPaie = 6  WHERE RoleId = 128;