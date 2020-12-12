-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_SOCIETE ON;
--INSERT INTO FRED_SOCIETE (SocieteId,OrganisationId,GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Adresse,CodePostal,Ville,SIRET,Libelle,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture) VALUES ('1','7','1','RB','RZB','1000','3 rue rené razel','91892','ORSAY',NULL,'RAZEL-BEC','0','1',NULL,NULL,'1','1'); 
--SET IDENTITY_INSERT FRED_SOCIETE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S5 - Societe
-- --------------------------------------------------
SET IDENTITY_INSERT FRED_SOCIETE ON;
INSERT INTO  FRED_SOCIETE (SocieteId,GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Libelle,Adresse,Ville,CodePostal,SIRET,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture,OrganisationId,TransfertAS400,ImageScreenLogin,ImageLogoHeader,ImageLogoId,ImageLoginId,IsInterimaire,CodeSocieteStorm) 
VALUES (1,1,'RB','RZB','1000','RAZEL-BEC','3 rue rené razel','ORSAY','91892',NULL,0,1,NULL,NULL,1,1,7,0,NULL,NULL,NULL,NULL,0,'0094');
SET IDENTITY_INSERT FRED_SOCIETE OFF;


