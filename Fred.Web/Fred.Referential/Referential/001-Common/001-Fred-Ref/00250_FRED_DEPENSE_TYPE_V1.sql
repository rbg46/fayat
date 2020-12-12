-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_DEPENSE_TYPE ON;
--INSERT INTO FRED_DEPENSE_TYPE (DepenseTypeId,Libelle) VALUES ('1','Réception'); 
--INSERT INTO FRED_DEPENSE_TYPE (DepenseTypeId,Libelle) VALUES ('2','Facture'); 
--INSERT INTO FRED_DEPENSE_TYPE (DepenseTypeId,Libelle) VALUES ('3','Facture Ecart'); 
--SET IDENTITY_INSERT FRED_DEPENSE_TYPE OFF;



-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S38 - Types_Depenses
-- --------------------------------------------------


-- Mise à jour du script le 26/12/2018 YCO --
-- Ajout des valeurs pour la nouvelle colonne code --
-- Script de rattrapage des données inclus dans la migration avec l'ajout de la colonne Code et de l'index unique de celle ci"


SET IDENTITY_INSERT   FRED_DEPENSE_TYPE ON;
INSERT INTO FRED_DEPENSE_TYPE(DepenseTypeId, Code, Libelle) VALUES(1, 1, 'Réception')
INSERT INTO FRED_DEPENSE_TYPE(DepenseTypeId, Code, Libelle) VALUES(2, 2, 'Facture')
INSERT INTO FRED_DEPENSE_TYPE(DepenseTypeId, Code, Libelle) VALUES(3, 3, 'Facture Ecart')
SET IDENTITY_INSERT   FRED_DEPENSE_TYPE OFF;