-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_CODE_MAJORATION ON;
--INSERT INTO FRED_CODE_MAJORATION (CodeMajorationId,Code,Libelle,EtatPublic,IsActif,GroupeId) VALUES ('1','HM050','Heures majorées à  50%','1','1','1'); 
--INSERT INTO FRED_CODE_MAJORATION (CodeMajorationId,Code,Libelle,EtatPublic,IsActif,GroupeId) VALUES ('2','HM100','Heures majorées à 100%','1','1','1'); 
--INSERT INTO FRED_CODE_MAJORATION (CodeMajorationId,Code,Libelle,EtatPublic,IsActif,GroupeId) VALUES ('3','HS50','Heures souterraines 100%','1','1','1'); 
--SET IDENTITY_INSERT FRED_CODE_MAJORATION OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S9 - Code_Majoration
-- --------------------------------------------------
SET IDENTITY_INSERT FRED_CODE_MAJORATION ON;
INSERT INTO dbo.FRED_CODE_MAJORATION (CodeMajorationId,Code,Libelle,EtatPublic,IsActif,GroupeId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (1,'HM050', 'Heures majorées à  50%',1,1,1,GETDATE(),NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.FRED_CODE_MAJORATION (CodeMajorationId,Code,Libelle,EtatPublic,IsActif,GroupeId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (2,'HM100', 'Heures majorées à 100%',1,1,1,GETDATE(),NULL,NULL,NULL,NULL,NULL);
SET IDENTITY_INSERT FRED_CODE_MAJORATION OFF;



