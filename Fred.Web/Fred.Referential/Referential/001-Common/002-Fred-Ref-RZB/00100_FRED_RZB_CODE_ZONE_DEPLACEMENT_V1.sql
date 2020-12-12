-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_CODE_ZONE_DEPLACEMENT ON;
--INSERT INTO FRED_CODE_ZONE_DEPLACEMENT (CodeZoneDeplacementId,Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES ('1','Z1A','Zone 1A',GETDATE(),'1',NULL,NULL,'1','1'); 
--INSERT INTO FRED_CODE_ZONE_DEPLACEMENT (CodeZoneDeplacementId,Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES ('2','Z1B','Zone 1B',GETDATE(),'1',NULL,NULL,'1','1'); 
--INSERT INTO FRED_CODE_ZONE_DEPLACEMENT (CodeZoneDeplacementId,Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES ('3','Z2','Zone 2',GETDATE(),'1',NULL,NULL,'1','1'); 
--INSERT INTO FRED_CODE_ZONE_DEPLACEMENT (CodeZoneDeplacementId,Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES ('4','Z3','Zone 3',GETDATE(),'1',NULL,NULL,'1','1'); 
--INSERT INTO FRED_CODE_ZONE_DEPLACEMENT (CodeZoneDeplacementId,Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES ('5','Z4','Zone 4',GETDATE(),'1',NULL,NULL,'1','1'); 
--INSERT INTO FRED_CODE_ZONE_DEPLACEMENT (CodeZoneDeplacementId,Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES ('6','Z5','Zone 5',GETDATE(),'1',NULL,NULL,'1','1'); 
--SET IDENTITY_INSERT FRED_CODE_ZONE_DEPLACEMENT OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S10 - Code_Zone_Deplacement
-- --------------------------------------------------
SET IDENTITY_INSERT FRED_CODE_ZONE_DEPLACEMENT ON;
INSERT INTO FRED_CODE_ZONE_DEPLACEMENT(CodeZoneDeplacementId, Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES (1, 'Z1A', 'Zone 1A', GETDATE(), 1, NULL, NULL, 1, 1);
INSERT INTO FRED_CODE_ZONE_DEPLACEMENT(CodeZoneDeplacementId, Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES (2, 'Z1B', 'Zone 1B', GETDATE(), 1, NULL, NULL, 1, 1);
INSERT INTO FRED_CODE_ZONE_DEPLACEMENT(CodeZoneDeplacementId, Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES (3, 'Z2', 'Zone 2', GETDATE(), 1, NULL, NULL, 1, 1);
INSERT INTO FRED_CODE_ZONE_DEPLACEMENT(CodeZoneDeplacementId, Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES (4, 'Z3', 'Zone 3', GETDATE(), 1, NULL, NULL, 1, 1);
INSERT INTO FRED_CODE_ZONE_DEPLACEMENT(CodeZoneDeplacementId, Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES (5, 'Z4', 'Zone 4', GETDATE(), 1, NULL, NULL, 1, 1);
INSERT INTO FRED_CODE_ZONE_DEPLACEMENT(CodeZoneDeplacementId, Code,Libelle,DateCreation,AuteurCreation,DateModification,AuteurModification,IsActif,SocieteId) VALUES (6, 'Z5', 'Zone 5', GETDATE(), 1, NULL, NULL, 1, 1);
SET IDENTITY_INSERT FRED_CODE_ZONE_DEPLACEMENT OFF;