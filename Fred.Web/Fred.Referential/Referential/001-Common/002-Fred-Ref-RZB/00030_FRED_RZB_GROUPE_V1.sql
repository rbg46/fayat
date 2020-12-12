-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_GROUPE ON;
--INSERT INTO FRED_GROUPE (GroupeId,OrganisationId,PoleId,Code,Libelle) VALUES ('1','5','1','GRZB','GROUPE RAZEL-BEC'); 
--SET IDENTITY_INSERT FRED_GROUPE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S20 - Organisation_Groupe
-- --------------------------------------------------
SET IDENTITY_INSERT  FRED_GROUPE ON;
INSERT INTO FRED_GROUPE(GroupeId,Code,Libelle,PoleId,OrganisationId) VALUES(1,'GRZB','GROUPE RAZEL-BEC',1,5)
SET IDENTITY_INSERT  FRED_GROUPE OFF;