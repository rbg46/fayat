-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_FACTURATION_TYPE ON;
--INSERT INTO FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES ('1','Facturation'); 
--INSERT INTO FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES ('2','Coût Additionnel'); 
--SET IDENTITY_INSERT FRED_FACTURATION_TYPE OFF;


-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S17 - Facturation_Type
-- --------------------------------------------------
--SET IDENTITY_INSERT  FRED_FACTURATION_TYPE ON;
--INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES  (3,'Article Non Commandé')
--INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES  (4,'Facturation En Montant')
--INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES  (5,'Chargement Provision FAR')
--INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES  (6,'Déchargement Provision FAR')
--INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES  (7,'Annulation FAR')
--INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES  (8,'Avoir Sans Commande')
--INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES  (9,'Avoir Sur Commande En Montant')
--INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId,Libelle) VALUES  (10,'Avoir Sur Commande En Quantité')
--SET IDENTITY_INSERT FRED_FACTURATION_TYPE  OFF;

UPDATE FRED_FACTURATION_TYPE SET LIBELLE = 'Avoir Sur Commande En Quantité' WHERE FacturationTypeId = 7;
UPDATE FRED_FACTURATION_TYPE SET LIBELLE = 'Avoir Sur Commande En Montant' WHERE FacturationTypeId = 8;
UPDATE FRED_FACTURATION_TYPE SET LIBELLE = 'Annulation FAR' WHERE FacturationTypeId = 9;
UPDATE FRED_FACTURATION_TYPE SET LIBELLE = 'Avoir Sans Commande' WHERE FacturationTypeId = 10;