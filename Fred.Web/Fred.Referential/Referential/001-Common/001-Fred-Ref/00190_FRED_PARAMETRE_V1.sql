-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_PARAMETRE ON;
--INSERT INTO FRED_PARAMETRE (ParametreId, Code, Libelle, Valeur, GroupeId) VALUES ('1','1','Parametre GoogleAPI','{"Quota":100,"IndexCourant":0,"DateCourante":"2017-04-21T10:24:47.6346565+02:00"}', null); 
--SET IDENTITY_INSERT FRED_PARAMETRE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S27 - Parametre_Systeme
-- --------------------------------------------------

SET IDENTITY_INSERT  FRED_PARAMETRE ON;
INSERT INTO  FRED_PARAMETRE (ParametreId,Code,Libelle,Valeur,GroupeId) VALUES  (1,1,'Parametre GoogleAPI','{"Quota":100,"IndexCourant":0,"DateCourante":"2017-04-21T10:24:47.6346565+02:00"}',NULL)
SET IDENTITY_INSERT FRED_PARAMETRE  OFF;