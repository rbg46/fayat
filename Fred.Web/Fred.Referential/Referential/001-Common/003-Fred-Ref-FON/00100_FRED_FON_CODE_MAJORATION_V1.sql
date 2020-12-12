-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- CREATION DES CODES MAJORATION POUR SEFI ET FRANKI
-- --------------------------------------------------





INSERT INTO dbo.FRED_CODE_MAJORATION (Code,Libelle,EtatPublic,IsActif,GroupeId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES ('HM050', 'Heures majorées à  50%',1,1,4,GETDATE(),NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.FRED_CODE_MAJORATION (Code,Libelle,EtatPublic,IsActif,GroupeId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES ('HM100', 'Heures majorées à 100%',1,1,4,GETDATE(),NULL,NULL,NULL,NULL,NULL);