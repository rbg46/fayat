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
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (1, 1, 'Facturation')
INSERT INTO  FRED_FACTURATION_TYPE (FacturationTypeId, Code, Libelle) VALUES  (2, 2, 'Coût Additionnel')
SET IDENTITY_INSERT FRED_FACTURATION_TYPE  OFF;