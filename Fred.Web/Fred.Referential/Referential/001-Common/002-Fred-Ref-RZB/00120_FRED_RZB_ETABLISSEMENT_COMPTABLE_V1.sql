-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_ETABLISSEMENT_COMPTABLE ON;
--INSERT INTO FRED_ETABLISSEMENT_COMPTABLE (EtablissementComptableId,OrganisationId,SocieteId,Code,Libelle,Adresse,Ville,CodePostal,PaysId,ModuleCommandeEnabled,ModuleProductionEnabled,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId,IsDeleted) VALUES ('1','52','1','01','DRN- Idf Est             ','526 AVENUE ALBERT EINSTEIN','MOISSY CRAMAYEL','77555','1','1','0',GETDATE(),NULL,NULL,NULL,NULL,NULL,'0'); 
--INSERT INTO FRED_ETABLISSEMENT_COMPTABLE (EtablissementComptableId,OrganisationId,SocieteId,Code,Libelle,Adresse,Ville,CodePostal,PaysId,ModuleCommandeEnabled,ModuleProductionEnabled,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId,IsDeleted) VALUES ('2','52','1','35','DRS-LR Montpellier','1111 AVENUE JUSTIN BEC','SAINT GEORGES D ORQUES','34680','1','1','1',GETDATE(),NULL,NULL,NULL,NULL,NULL,'0'); 
--SET IDENTITY_INSERT FRED_ETABLISSEMENT_COMPTABLE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES

-- --------------------------------------------------
SET IDENTITY_INSERT dbo.FRED_ETABLISSEMENT_COMPTABLE ON
insert into FRED_ETABLISSEMENT_COMPTABLE (EtablissementComptableId,SocieteId,Code,Libelle,Adresse,Ville,CodePostal,ModuleCommandeEnabled,ModuleProductionEnabled,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId,IsDeleted,OrganisationId,PaysId) VALUES (1,1, '11', 'GCNS-Génie Civ.Nucléaire', 'SITE DE BERNON-ROUTE MICHEL', 'TRESQUES', '30330',1,1,GETDATE(),NULL,NULL,NULL,NULL,NULL,0,101,1)
insert into FRED_ETABLISSEMENT_COMPTABLE (EtablissementComptableId,SocieteId,Code,Libelle,Adresse,Ville,CodePostal,ModuleCommandeEnabled,ModuleProductionEnabled,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId,IsDeleted,OrganisationId,PaysId) VALUES (2,1, '03', 'DRN- Idf Ouest', 'Route des gatines', 'ELANCOURT', '78990',1,1,GETDATE(),NULL,NULL,NULL,NULL,NULL,0,100,1)
insert into FRED_ETABLISSEMENT_COMPTABLE (EtablissementComptableId,SocieteId,Code,Libelle,Adresse,Ville,CodePostal,ModuleCommandeEnabled,ModuleProductionEnabled,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId,IsDeleted,OrganisationId,PaysId) VALUES (3,1, '35', 'DRS-LR Montpellier', '1111, AVENUE JUSTIN BEC', 'ST JEAN DE VEDAS', '34433',1,1,GETDATE(),NULL,NULL,NULL,NULL,NULL,0,102,1)
SET IDENTITY_INSERT dbo.FRED_ETABLISSEMENT_COMPTABLE OFF