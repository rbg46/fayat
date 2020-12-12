-- US_5562

-- Dans tous les cas je créé les permissions
IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0131')
BEGIN
	INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('button.show.import.fournisseur.anael', 1,'0131','Déclencher manuellement un import des fournisseurs dans FRED',0)   
END

DECLARE @ModulePointageId INT;
SET @ModulePointageId = (SELECT ModuleId FROM FRED_MODULE WHERE Code = '6')
-- Si le module existe je créé la fonctionnalite
IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '604') AND @ModulePointageId > 0
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@ModulePointageId, '604','Import des fournisseurs dans FRED', 0, NULL, 'Import des fournisseurs dans FRED');

   

	IF NOT EXISTS (
		SELECT pf.PermissionFonctionnaliteId
		FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
			inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
			where p.Code='0131' and f.code='604')
	BEGIN
		INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
			SELECT P.PermissionId, f.FonctionnaliteId
			FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
			where p.Code='0131' and f.code='604'
	END
END
