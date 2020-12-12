-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_COMMANDE_TYPE ON;
--INSERT INTO FRED_COMMANDE_TYPE (CommandeTypeId,Code,Libelle) VALUES ('1','F','Fourniture'); 
--INSERT INTO FRED_COMMANDE_TYPE (CommandeTypeId,Code,Libelle) VALUES ('2','L','Location'); 
--INSERT INTO FRED_COMMANDE_TYPE (CommandeTypeId,Code,Libelle) VALUES ('3','P','Prestation'); 
--SET IDENTITY_INSERT FRED_COMMANDE_TYPE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S11 - Type_Commande
-- --------------------------------------------------
SET IDENTITY_INSERT  FRED_COMMANDE_TYPE ON;
INSERT INTO  FRED_COMMANDE_TYPE(CommandeTypeId, Code, Libelle) VALUES(1,'F','Fourniture')
INSERT INTO  FRED_COMMANDE_TYPE (CommandeTypeId, Code, Libelle) VALUES(2,'L','Location')
INSERT INTO  FRED_COMMANDE_TYPE (CommandeTypeId, Code, Libelle) VALUES(3,'P','Prestation')
SET IDENTITY_INSERT  FRED_COMMANDE_TYPE OFF;