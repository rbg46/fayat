-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_DEPENSE_TYPE ON;
--INSERT INTO FRED_DEPENSE_TYPE (DepenseTypeId,Libelle) VALUES ('1','Réception'); 
--INSERT INTO FRED_DEPENSE_TYPE (DepenseTypeId,Libelle) VALUES ('2','Facture'); 
--INSERT INTO FRED_DEPENSE_TYPE (DepenseTypeId,Libelle) VALUES ('3','Facture Ecart'); 
--SET IDENTITY_INSERT FRED_DEPENSE_TYPE OFF;


-- Mise à jour du script le 26/12/2018 YCO --
-- Ajout des valeurs pour la nouvelle colonne code --
-- Script de rattrapage des données inclut dans la migration avec la 'ajout de la colonne Code et de l'index unique de celle ci"


SET IDENTITY_INSERT   FRED_DEPENSE_TYPE ON;
INSERT INTO FRED_DEPENSE_TYPE(DepenseTypeId, Code, Libelle) VALUES(4, 4, 'Facture Non Cmdé')
INSERT INTO FRED_DEPENSE_TYPE(DepenseTypeId, Code, Libelle) VALUES(5, 5, 'Avoir')
INSERT INTO FRED_DEPENSE_TYPE(DepenseTypeId, Code, Libelle) VALUES(6, 6, 'Avoir Ecart')
INSERT INTO FRED_DEPENSE_TYPE(DepenseTypeId, Code, Libelle) VALUES(7, 7, 'Ajustement FAR SAP')
INSERT INTO FRED_DEPENSE_TYPE(DepenseTypeId, Code, Libelle) VALUES(8, 8, 'Extourne FAR')
SET IDENTITY_INSERT   FRED_DEPENSE_TYPE OFF;