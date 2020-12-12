DECLARE @MoyenModuleId INT;
SET @MoyenModuleId = (SELECT ModuleId FROM FRED_MODULE WHERE Code='51')

-- Insertion des fonctionnalités 

-- Affichage du champs filtre par Ci

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1361')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1361','Recherche : Affichage de la lookup de filtre par Ci', 0, NULL, 'Recherche : Affichage de la lookup de filtre par Ci');
END

-- Affichage du champs responsable ou manager

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1362')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, '1362','Recherche : Affichage du champs responsable ou manager', 0, NULL, 'Recherche : Affichage du champs responsable ou manager');
END

-- Affichage du filtre par Ci
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0125' and f.code='1361')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0125' and f.code='1361'
END

-- Affichage du champs responsable ou manager
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0126' and f.code='1362')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0126' and f.code='1362'
END