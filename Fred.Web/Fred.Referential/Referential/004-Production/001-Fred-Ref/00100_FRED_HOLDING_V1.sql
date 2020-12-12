

-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- --------------------------------------------------
--SET IDENTITY_INSERT  FRED_HOLDING ON;
--INSERT INTO  FRED_HOLDING(HoldingId, Code,Libelle,OrganisationId) VALUES(1,'FSA','FAYAT SA',1);
--SET IDENTITY_INSERT  FRED_HOLDING  OFF;
-- Suppression de la holding 2
DELETE FROM FRED_HOLDING WHERE HoldingID = 2