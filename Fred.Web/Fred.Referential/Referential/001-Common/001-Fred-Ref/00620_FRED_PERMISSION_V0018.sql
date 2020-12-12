-- US_5171

-- dans tous les cas je creer les permissions
IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0129')
BEGIN
	INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('button.show.duplicate.pointagepersonnel.index', 1,'0129','Affichage du boutton de duplication sur la page ''Liste des pointages personnels''.',0)   
END

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0130')
BEGIN	
    INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('button.show.duplicate.rapport.detail', 1,'0130','Affichage du boutton de duplication sur la page ''Rapport journalier''.',0)
END


DECLARE @ModulePointageId INT;
SET @ModulePointageId = (SELECT ModuleId FROM FRED_MODULE WHERE Code = '4')
-- si le module existe je creer la fonctionnalite
IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '414') AND @ModulePointageId > 0
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@ModulePointageId, '414','Duplication des rapports en masse', 0, NULL, 'Duplication des rapports en masse');

   

IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0129' and f.code='414')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0129' and f.code='414'
END

IF NOT EXISTS (
	SELECT pf.PermissionFonctionnaliteId
	FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		where p.Code='0130' and f.code='414')
BEGIN
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		SELECT P.PermissionId, f.FonctionnaliteId
		FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		where p.Code='0130' and f.code='414'
END
END
