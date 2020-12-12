-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

-- On insert un sous chapitre STORM

DECLARE @ChapitreId INTEGER; 
SELECT  @ChapitreId =  (SELECT ChapitreId FROM FRED_CHAPITRE WHERE Code= 20)

INSERT INTO FRED_SOUS_CHAPITRE (Code,Libelle,ChapitreId,DateCreation,AuteurCreationId,DateModification,AuteurModificationId,DateSuppression,AuteurSuppressionId)
VALUES ('STORM','Matériel STORM en attente de réaffectation',@ChapitreId,NULL,NULL,NULL,NULL,NULL,NULL); 

-- On récupère l'identifant du sous chapitre STORM
DECLARE @SousChapitreId INTEGER;
SELECT @SousChapitreId = SousChapitreId FROM FRED_SOUS_CHAPITRE WHERE Code = 'STORM'


-- On insert une ressource STORM
INSERT INTO FRED_RESSOURCE (Code,Libelle,SousChapitreId,ParentId,TypeRessourceId,Active,CarburantId,Consommation,DateCreation,AuteurCreationId,DateModification,AuteurModificationId,DateSuppression,AuteurSuppressionId) 
VALUES ('STORM','En Attente de réaffectation',@SousChapitreId,NULL,'1','1',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL); 





-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- --------------------------------------------------