-- Insertion des fonctionnalités de roles : ReponsableCI 
IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '1201')
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (11, '1201','Validation de rapport', 0, NULL, NULL);
END

-- Association des permissions aux fonctionnalités
IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0053' and f.code='1201')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0053' and f.code='1201'
END

--Association des roles : ResponsableCI aux fonctionnalités validation de rapport
IF NOT EXISTS (
	SELECT rf.RoleFonctionnaliteId
	FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId
		inner join FRED_FONCTIONNALITE f on rf.FonctionnaliteId = f.FonctionnaliteId
		where r.Specification=1 and f.code='1201')
BEGIN
	INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
	FROM FRED_ROLE r, FRED_FONCTIONNALITE f
	where r.Specification=1 and f.code='1201'
END