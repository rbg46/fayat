-- 12-12-2018
-- Gestion des moyens
-- ** Création des fonctionnalités pour distinguer les actions des différents 
-- ** utilisateurs (Gestionnaire de moyens, responsable CI, Délégué Ci et autre)



DECLARE @MoyenModuleId INT;
SET @MoyenModuleId = (SELECT ModuleId FROM FRED_MODULE WHERE Code='51')

-- Insertion des fonctionnalités

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1350')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1350','Affichage du menu gestion des moyens', 0, NULL, 'Affichage du menu gestion des moyens');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1351')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1351','Affichage du bouton d''affectation', 0, NULL, 'Affichage du bouton d''affectation');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1352')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1352','Affichage du bouton de restitution', 0, NULL, 'Affichage du bouton de restitution');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1353')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1353','Affichage du bouton de location', 0, NULL, 'Affichage du bouton de location');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1354')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1354','Affichage du bouton de maintenance', 0, NULL, 'Affichage du bouton de maintenance');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1355')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1355','Recherche : Affichage du de la lookup de filtre par personnel', 0, NULL, 'Recherche : Affichage du de la lookup de filtre par personnel');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1356')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1356','Recherche : Affichage par personnel et Ci (Tous)', 0, NULL, 'Recherche : Affichage par personnel et Ci (Tous)');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1357')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1357','Affectation : séléction des moyens pour affectation', 0, NULL, 'Affectation : séléction des moyens pour affectation');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1358')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1358','Filtre : recherche avancée', 0, NULL, 'Filtre : recherche avancée (par site, statut, date de fin et moyens à rapatrier)');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1359')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1359','Validation ou annulation des affectations', 0, NULL, 'Validation ou annulation des affectations');
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1360')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1360','Accès à la date de fin d''affectation', 0, NULL, 'Accès à la date de fin d''affectation');
END

------------------------------------------------------
-- Association des permissions aux fonctionnalités
------------------------------------------------------

-- Affichage du menu
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0100' and f.code='1350')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0100' and f.code='1350'
END

-- Affichage du bouton d'affectation
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0111' and f.code='1351')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0111' and f.code='1351'
END

-- Affichage du bouton de restitution
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0112' and f.code='1352')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0112' and f.code='1352'
END

-- Affichage du bouton de location
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0113' and f.code='1353')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0113' and f.code='1353'
END

-- Affichage du bouton de maintenance
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0114' and f.code='1354')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0114' and f.code='1354'
END

-- Recherche : Affichage du de la lookup de filtre par personnel
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0115' and f.code='1355')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0115' and f.code='1355'
END

-- Recherche : Affichage par personnel et Ci (Tous)
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0116' and f.code='1356')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0116' and f.code='1356'
END

-- Affectation : séléction des moyens pour affectation
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0117' and f.code='1357')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0117' and f.code='1357'
END

-- Filtre : recherche avancée
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0118' and f.code='1358')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0118' and f.code='1358'
END

IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0119' and f.code='1358')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0119' and f.code='1358'
END

IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0120' and f.code='1358')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0120' and f.code='1358'
END

IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0121' and f.code='1358')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0121' and f.code='1358'
END

-- Validation et annulation
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0122' and f.code='1359')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0122' and f.code='1359'
END
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0123' and f.code='1359')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0123' and f.code='1359'
END

-- Date fin d'affectation
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0124' and f.code='1360')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0124' and f.code='1360'
END