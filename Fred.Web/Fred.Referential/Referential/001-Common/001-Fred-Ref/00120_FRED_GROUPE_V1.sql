-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_GROUPE ON;
--INSERT INTO FRED_GROUPE (GroupeId,OrganisationId,PoleId,Code,Libelle) VALUES ('2','6','2','FCI','FAYAT CONSTRUCTION INFORMATIQUE'); 
--SET IDENTITY_INSERT FRED_GROUPE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S20 - Organisation_Groupe
-- --------------------------------------------------

SET IDENTITY_INSERT  FRED_GROUPE ON;
INSERT INTO FRED_GROUPE(GroupeId,Code,Libelle,PoleId,OrganisationId) VALUES(2,'FIT','FAYAT IT',2,6);
SET IDENTITY_INSERT  FRED_GROUPE OFF;