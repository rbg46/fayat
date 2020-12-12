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

-- Mise à jour du script le 26/12/2018 YCO --
-- Ajout des valeurs pour la nouvelle colonne code --
-- Script de rattrapage des données inclus dans la migration avec l'ajout de la colonne Code et de l'index unique de celle ci"


SET IDENTITY_INSERT  FRED_FACTURATION_TYPE ON;
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (3, 3, 'Article Non Commandé')
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (4, 4, 'Facturation En Montant')
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (5, 5, 'Chargement Provision FAR')
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (6, 6, 'Déchargement Provision FAR')
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (7, 7, 'Annulation FAR')
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (8, 8, 'Avoir Sans Commande')
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (9, 9, 'Avoir Sur Commande En Montant')
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (10, 10, 'Avoir Sur Commande En Quantité')
SET IDENTITY_INSERT FRED_FACTURATION_TYPE  OFF;