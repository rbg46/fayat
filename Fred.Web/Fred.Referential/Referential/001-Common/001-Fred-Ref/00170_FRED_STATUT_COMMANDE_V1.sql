-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_STATUT_COMMANDE ON;
--INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('1','BR','Brouillon'); 
--INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('2','AV','A valider'); 
--INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('3','VA','Validée'); 
--INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('4','CL','Cloturée'); 
--INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('5','MVA','Manuelle validée'); 
--SET IDENTITY_INSERT FRED_STATUT_COMMANDE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S36 - Statut_Commande
-- --------------------------------------------------
SET IDENTITY_INSERT FRED_STATUT_COMMANDE ON;
INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('1','BR','Brouillon'); 
INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('2','AV','A valider'); 
INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('3','VA','Validée'); 
INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('4','CL','Cloturée'); 
INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('5','MVA','Manuelle validée'); 
SET IDENTITY_INSERT FRED_STATUT_COMMANDE OFF;
