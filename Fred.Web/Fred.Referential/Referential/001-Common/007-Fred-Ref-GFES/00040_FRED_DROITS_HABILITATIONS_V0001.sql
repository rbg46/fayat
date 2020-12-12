-- --------------------------------------------------
-- MEP OCTOBRE 2018 - INJECTION DES DONNES
-- Droits & Habilitation
-- --------------------------------------------------

-- On préfixe les anciennes versions avec un _ pour les différencier
UPDATE fred_groupe SET libelle='_GROUPE SATELEC', code='_GSAT' WHERE code='GSAT'
UPDATE fred_groupe SET libelle='_GROUPE SEMERU', code='_GSEM' WHERE code='GSEM'
UPDATE fred_societe SET libelle='_SATELEC', code='_FES' WHERE code='FES'
UPDATE fred_societe SET libelle='_SEMERU', code='_SEM' WHERE code='SEM'

-- Ajout des rôles pour les sociétés
DECLARE @GroupeFESId INT;
SET @GroupeFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code='GFES')

INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier)
  SELECT 'DRA','Admin Appli',NULL,0,1,3,5,NULL,s.SocieteId,'ADM' FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId
INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier)
  SELECT 'DRA','Gestionnaire Paye',NULL,0,1,3,5,NULL,s.SocieteId,'GSP' FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId
INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier)
  SELECT 'DRA','Chef de chantier',NULL,0,1,3,5,NULL,s.SocieteId,'CDC' FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId
INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier)
  SELECT 'DRA','Conducteur de travaux',NULL,0,1,3,5,NULL,s.SocieteId,'CDT' FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId
  INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier)
  SELECT 'DRA','Chef de secteur',NULL,0,1,3,5,NULL,s.SocieteId,'CDS' FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId
INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier, Specification) 
  SELECT 'RCI','Responsable CI',NULL,0,1,1,1,'Responsable CI',s.SocieteId,'RCI', 1 FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId
INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier, Specification) 
  SELECT 'DCI','Délégué CI',NULL,0,1,1,1,'Délégué CI',s.SocieteId,'DCI', 2 FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId

--Association des roles : ResponsableCI et Délégué aux fonctionnalités gestion équipe et délégation
INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
  FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
  where r.Specification=1	and s.GroupeId=@GroupeFESId and f.code='1102'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r.RoleId, f.FonctionnaliteId, 2
  FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
  where r.Specification=1	and s.GroupeId=@GroupeFESId and f.code='1103'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
  FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
  where r.Specification=1	and s.GroupeId=@GroupeFESId and f.code='1102'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r.RoleId, f.FonctionnaliteId, 1
  FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
  where r.Specification=1	and s.GroupeId=@GroupeFESId and f.code='1103'


-- Association entre les fonctionnalités et les rôles
INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r2.RoleId, rf.FonctionnaliteId, rf.Mode
  FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId,
    FRED_ROLE r2 inner join FRED_SOCIETE s on r2.SocieteId = s.SocieteId
  WHERE r.CodeNomFamilier = 'ADM' and r.SocieteId = 1 and r2.CodeNomFamilier = 'ADM' and s.GroupeId=@GroupeFESId

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r2.RoleId, rf.FonctionnaliteId, rf.Mode
  FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId,
    FRED_ROLE r2 inner join FRED_SOCIETE s on r2.SocieteId = s.SocieteId
  WHERE r.CodeNomFamilier = 'GSP' and r.SocieteId = 1 and r2.CodeNomFamilier = 'GSP' and s.GroupeId=@GroupeFESId

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r2.RoleId, rf.FonctionnaliteId, rf.Mode
  FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId,
    FRED_ROLE r2 inner join FRED_SOCIETE s on r2.SocieteId = s.SocieteId
  WHERE r.CodeNomFamilier = 'CDC' and r.SocieteId = 1 and r2.CodeNomFamilier = 'CDC' and s.GroupeId=@GroupeFESId

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r2.RoleId, rf.FonctionnaliteId, rf.Mode
  FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId,
    FRED_ROLE r2 inner join FRED_SOCIETE s on r2.SocieteId = s.SocieteId
  WHERE r.CodeNomFamilier = 'CDT' and r.SocieteId = 1 and r2.CodeNomFamilier = 'CDT' and s.GroupeId=@GroupeFESId

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r2.RoleId, rf.FonctionnaliteId, rf.Mode
  FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId,
    FRED_ROLE r2 inner join FRED_SOCIETE s on r2.SocieteId = s.SocieteId
  WHERE r.CodeNomFamilier = 'CDS' and r.SocieteId = 1 and r2.CodeNomFamilier = 'CDS' and s.GroupeId=@GroupeFESId

-- Activation des sociétés du groupe FES
UPDATE FRED_SOCIETE SET Active=1 WHERE GroupeId=@GroupeFESId