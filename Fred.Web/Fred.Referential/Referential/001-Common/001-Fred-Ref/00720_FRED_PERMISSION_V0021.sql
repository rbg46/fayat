
-- Dans tous les cas je créé les permissions
IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0132')
BEGIN
	INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('button.show.personnel.souscription.email.activity', 1,'0132','Envoi automatique quotidien d''un email récapitulatif aux utilisateurs FRED abonnés.',0)   
END

DECLARE @ModulePointageId INT;
SET @ModulePointageId = (SELECT ModuleId FROM FRED_MODULE WHERE Code = '3')
-- Si le module existe je créé la fonctionnalite
IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '303') AND @ModulePointageId > 0
BEGIN
	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@ModulePointageId, '303','Mailing automatique des utilisateurs FRED', 0, NULL, 'Mailing automatique des utilisateurs FRED');

   

	IF NOT EXISTS (
		SELECT pf.PermissionFonctionnaliteId
		FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
			inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
			where p.Code='0132' and f.code='303')
	BEGIN
		INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
			SELECT P.PermissionId, f.FonctionnaliteId
			FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
			where p.Code='0132' and f.code='303'
	END
END
