-- --------------------------------------------------
-- OCTOBRE 2018 - INJECTION DES DONNES
-- Droits & Habilitation pour les ETAM/IAC et les Managers
-- --------------------------------------------------

DECLARE @GroupeFESId INT;
SET @GroupeFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code='GFES')

INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier)
	SELECT 'ETAM','Employés, Techniciens et Agents de Maîtrise',NULL,0,1,3,5,NULL,s.SocieteId,'ETAM' FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId
INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier)
	SELECT 'IAC','Ingénieurs, assimilés cadres',NULL,0,1,3,5,NULL,s.SocieteId,'IAC' FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId
INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier)
	SELECT 'MP','Manager des personnels',NULL,0,1,3,5,NULL,s.SocieteId,'MP' FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.CodeNomFamilier = 'ETAM' and s.GroupeId=@GroupeFESId and f.code='407'
INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.CodeNomFamilier = 'IAC' and s.GroupeId=@GroupeFESId and f.code='407'
INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.CodeNomFamilier = 'MP' and s.GroupeId=@GroupeFESId and f.code='407'


INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
	SELECT P.PermissionId, f.FonctionnaliteId
	FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
	where p.Code='0058' and f.code='407'
