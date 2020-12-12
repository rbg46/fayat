--28-11-18 

-- Création du role gestionnaire des moyens

-- Ce role a été crée dans l'interface c'est pourquoi il a fallu le refaire
-- Netoyage du role crée par l'interface
Delete FROM [FRED_ROLE_ORGANISATION_DEVISE]
WHERE RoleId in (SELECT RoleId FROM FRED_ROLE WHERE CodeNomFamilier='GSM')

Delete FROM FRED_ROLE_DEVISE
WHERE RoleId in (SELECT RoleId FROM FRED_ROLE WHERE CodeNomFamilier='GSM') 

Delete FROM FRED_ROLE_FONCTIONNALITE
WHERE RoleId in (SELECT RoleId FROM FRED_ROLE WHERE CodeNomFamilier='GSM') 

Delete FROM FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE
WHERE RoleId in (SELECT RoleId FROM FRED_ROLE WHERE CodeNomFamilier='GSM') 

DELETE FROM FRED_ROLE WHERE CodeNomFamilier = 'GSM'



DECLARE @GroupeFESId INT;
SET @GroupeFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code='GFES')

INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,[Description],SocieteId,CodeNomFamilier,Specification)
SELECT 'GSM','Gestionnaire des moyens',NULL,0,1,3,5,NULL,s.SocieteId,'GSM',3 FROM FRED_SOCIETE s WHERE s.GroupeId = @GroupeFESId


-- Insertion du module gestionnaire des moyens
IF NOT EXISTS (SELECT ModuleId FROM FRED_MODULE WHERE Code = '51')
BEGIN
	INSERT INTO FRED_MODULE (Code,Libelle, DateSuppression,[Description]) 
	VALUES ('51','Gestion des moyens', NULL, 'Module pour la gestion des moyens');
END


DECLARE @MoyenModuleId INT;
SET @MoyenModuleId = (SELECT ModuleId FROM FRED_MODULE WHERE Code='51')

-- Insertion des fonctionnalités utilisée pour la gestion des moyens

-- Fonctionnalités liées à la liste des affectations des moyens
IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1311')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1311','Affichage de toute la liste des affectations des moyens', 0, NULL, 'Affichage de toute la liste des affectations des moyens');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1312')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1312','Affichage de la liste des affectations moyens liées au personnels d''un manager', 0, NULL, 'Affichage de la liste des affectations moyens liées au personnels d''un manager');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1313')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1313','Affichage de la liste des affectations liées au CI d''un responsable CI', 0, NULL, 'Affichage de la liste des affectations liées au CI d''un responsable CI');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1314')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1314','Affichage de la liste des affectations moyens liées au CI d''un délégué', 0, NULL, 'Affichage de la liste des affectations moyens liées au CI d''un délégué');
END

-- Fonctionnalités liées à la liste des CI
IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1315')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1315','Affichage de toute la liste des CI', 0, NULL, 'Affichage de toute la liste des CI');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1316')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1316','Affichage de la liste des CI d''un responsable CI', 0, NULL, 'Affichage de la liste des CI d''un responsable CI');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1317')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1317','Affichage de la liste des CI d''un délégué', 0, NULL, 'Affichage de la liste des CI d''un délégué');
END


-- Fonctionnalités liées à la liste des personnels
IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1318')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1318','Affichage de toute la liste des personnels', 0, NULL, 'Affichage de toute la liste des personnels');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1319')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1319','Affichage de la liste des personnels d''un manager', 0, NULL, 'Affichage de la liste des personnels d''un manager');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1320')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1320','Affichage de la liste des personnels affectés aux CI d''un responsable CI', 0, NULL, 'Affichage de la liste des personnels affectés aux CI d''un responsable CI');
END


------------------------------------------------------
-- Association des permissions aux fonctionnalités
------------------------------------------------------

-- Affichage de toute la liste des affectations des moyens
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0101' and f.code='1311')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0101' and f.code='1311'
END

-- Affichage de la liste des affectations moyens liées au personnels d'un manager
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0102' and f.code='1312')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0102' and f.code='1312'
END

-- Affichage de la liste des affectations liées au CI d'un responsable CI
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0103' and f.code='1313')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0103' and f.code='1313'
END

-- Affichage de la liste des affectations moyens liées au CI d'un délégué
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0104' and f.code='1314')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0104' and f.code='1314'
END

-- Affichage de toute la liste des CI
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0105' and f.code='1315')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0105' and f.code='1315'
END

-- Affichage de la liste des CI d'un responsable CI
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0106' and f.code='1316')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0106' and f.code='1316'
END


-- Affichage de la liste des CI d'un délégué
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0107' and f.code='1317')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0107' and f.code='1317'
END


-- Affichage de toute la liste des personnels
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0108' and f.code='1318')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0108' and f.code='1318'
END


-- Affichage de la liste des personnels d'un manager
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0109' and f.code='1319')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0109' and f.code='1319'
END

-- Affichage de la liste des personnels affectés aux CI d''un responsable CI
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0110' and f.code='1320')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0110' and f.code='1320'
END

------------------------------------------------------
-- Fin Association des permissions aux fonctionnalités
------------------------------------------------------


