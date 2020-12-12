-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_SOCIETE ON;
--INSERT INTO FRED_SOCIETE (SocieteId,OrganisationId,GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Adresse,CodePostal,Ville,SIRET,Libelle,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture)VALUES ('3','9','2','FCI',NULL,NULL,'2 avenue general de Gaulle','91170','Viry-Chatillon',NULL,'FAYAT CONSTRUCTION INFORMATIQUE','0','1',NULL,NULL,'0','0'); 
--SET IDENTITY_INSERT FRED_SOCIETE OFF;



-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S5 - Societe
-- --------------------------------------------------

SET IDENTITY_INSERT  FRED_SOCIETE ON;
INSERT INTO  FRED_SOCIETE (SocieteId,GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Libelle,Adresse,Ville,CodePostal,SIRET,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture,OrganisationId,TransfertAS400,ImageScreenLogin,ImageLogoHeader,ImageLogoId,ImageLoginId,IsInterimaire,CodeSocieteStorm) VALUES (2,2,'FIT','NULL','NULL','FAYAT IT','2 avenue general de Gaulle','Viry-Chatillon','91170',NULL,0,1,NULL,NULL,0,0,8,0,NULL,NULL,NULL,NULL,0,'0169');
SET IDENTITY_INSERT  FRED_SOCIETE OFF;
