-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_RAPPORT_STATUT ON;
--INSERT INTO FRED_RAPPORT_STATUT (RapportStatutId,Code,Libelle) VALUES ('1', 'EC','En cours');
--INSERT INTO FRED_RAPPORT_STATUT (RapportStatutId,Code,Libelle) VALUES ('2', 'V1','Validé 1');
--INSERT INTO FRED_RAPPORT_STATUT (RapportStatutId,Code,Libelle) VALUES ('3', 'V2','Validé 2');
--INSERT INTO FRED_RAPPORT_STATUT (RapportStatutId,Code,Libelle) VALUES ('4', 'V3','Validé 3');
--INSERT INTO FRED_RAPPORT_STATUT (RapportStatutId,Code,Libelle) VALUES ('5', 'VE','Verrouillé');
--SET IDENTITY_INSERT FRED_RAPPORT_STATUT OFF;



-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S32 - Rapport_Statut
-- --------------------------------------------------
SET IDENTITY_INSERT FRED_RAPPORT_STATUT ON;
INSERT INTO FRED_RAPPORT_STATUT(RapportStatutId,Code,Libelle) VALUES ('1', 'EC','En cours');
INSERT INTO FRED_RAPPORT_STATUT(RapportStatutId,Code,Libelle) VALUES ('2', 'V1','Validé 1');
INSERT INTO FRED_RAPPORT_STATUT(RapportStatutId,Code,Libelle) VALUES ('3','V2','Validé 2');
INSERT INTO FRED_RAPPORT_STATUT(RapportStatutId,Code,Libelle) VALUES ('4','V3','Validé 3');
INSERT INTO FRED_RAPPORT_STATUT(RapportStatutId,Code,Libelle) VALUES ('5','VE','Verrouillé');
SET IDENTITY_INSERT FRED_RAPPORT_STATUT OFF;