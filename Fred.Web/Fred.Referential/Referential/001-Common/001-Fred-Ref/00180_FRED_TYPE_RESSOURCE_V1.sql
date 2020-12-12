-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_TYPE_RESSOURCE ON;
--INSERT INTO FRED_TYPE_RESSOURCE (TypeRessourceId,Code,Libelle) VALUES ('1','MAT','Matériel'); 
--INSERT INTO FRED_TYPE_RESSOURCE (TypeRessourceId,Code,Libelle) VALUES ('2','PERS','Personnel'); 
--SET IDENTITY_INSERT FRED_TYPE_RESSOURCE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S40 - Types_Ressources
-- --------------------------------------------------
SET IDENTITY_INSERT  FRED_TYPE_RESSOURCE ON;
INSERT INTO  FRED_TYPE_RESSOURCE(TypeRessourceId, Code, Libelle) VALUES(1,'MAT','Matériel');
INSERT INTO  FRED_TYPE_RESSOURCE(TypeRessourceId, Code, Libelle) VALUES(2,'PERS','Personnel');
SET IDENTITY_INSERT  FRED_TYPE_RESSOURCE OFF;
