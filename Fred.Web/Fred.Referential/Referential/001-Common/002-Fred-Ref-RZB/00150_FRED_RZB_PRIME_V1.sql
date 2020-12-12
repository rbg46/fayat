-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_PRIME ON;
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('1','101','PRIME APPLICATEURS ENROBÉ','1','1','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('2','102','PRIME DE TUNNEL GALERIE','1','2','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('3','103','Prime de chaux','0','0','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('4','104','PRIME D''HABILLAGE NGE','0','0','1','0','0','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('5','105','PRIME MARTEAU PIQUEUR','1','8','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('6','106','PRIME DE CASSE-CROUTE NGE','0','0','1','0','0','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('7','107','PRIME MARTEAU-PIQUEUR NGE','1','8','1','0','0','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('8','108','PRIME DE POSTE','0','0','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('9','109','PRIME DE POSTE DÉCALÉ','0','0','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('10','110','INDEMN INSULARITE','0','0','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('11','111','PRIME TRAVAIL EGOUT','1','3','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('12','112','Voyage Corse  n-c','0','0','1','0','0','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('13','113','Voyage Corse C. Mars','0','0','1','0','0','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('14','114','Prime air comprimé','1','1','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('15','115','Pime de collecteur servic','0','0','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('16','116','Prime d''entretien','0','0','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('17','117','PRIME PRISE DE POSTE NGE','0','0','1','0','0','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('18','118','Prime Journ. continue NGE','0','0','1','0','0','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('19','119','Prime T.Chantier/camion.','0','0','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('20','120','Prime amiante','1','1','1','0','1','1'); 
--INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId) VALUES ('21','121','Voyage Corse C. Bord','0','0','1','0','0','1'); 
--SET IDENTITY_INSERT FRED_PRIME OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S31 - Prime
-- --------------------------------------------------

SET IDENTITY_INSERT FRED_PRIME ON;
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (1, '101', 'PRIME APPLICATEURS ENROBÉ',1,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (2, '102', 'PRIME DE TUNNEL GALERIE',1,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (3, '103', 'Prime de chaux',0,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (4, '104', 'PRIME D''HABILLAGE ',0,5,1,0,0,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (5, '105', 'PRIME MARTEAU PIQUEUR',1,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (6, '108', 'PRIME DE POSTE',0,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (7, '109', 'PRIME DE POSTE DÉCALÉ',0,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (8, '110', 'INDEMN INSULARITE',0,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (9, '111', 'PRIME TRAVAIL EGOUT',1,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (10, '112', 'Voyage Corse  n-c',0,5,1,0,0,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (11, '113', 'Voyage Corse C. Mars',0,5,1,0,0,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (12, '114', 'Prime air comprimé',1,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (13, '115', 'Pime de collecteur servic',0,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (14, '116', 'Prime d''entretien',0,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (15, '119', 'Prime T.Chantier/camion.',0,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (16, '120', 'Prime amiante',1,5,1,0,1,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (17, '121', 'Voyage Corse C. Bord',0,5,1,0,0,1,GETDATE(),NULL,NULL,1,1,1);
INSERT INTO FRED_PRIME (PrimeId,Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId) VALUES (18, '122', 'Prime de nuit EOLE',1,5,1,0,0,1,GETDATE(),NULL,NULL,1,1,1);
SET IDENTITY_INSERT FRED_PRIME OFF;