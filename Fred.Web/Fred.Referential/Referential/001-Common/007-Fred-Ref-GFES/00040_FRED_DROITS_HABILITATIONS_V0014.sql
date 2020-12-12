-- FES - Gestion des moyens
-- Association aux role : Gestionnaire de moyens 
-- Fonctionnalité : Affichage du button rapport moyens
-- Permission : button.show.rapport.moyens

DECLARE @MoyenModuleId INT;
SET @MoyenModuleId = (SELECT ModuleId FROM FRED_MODULE WHERE Code='51')


DECLARE @permissionCode as nvarchar(8)
SELECT TOP 1 @permissionCode=p.code FROM FRED_PERMISSION p WHERE  code='1511'

IF NOT EXISTS (
    SELECT f.FonctionnaliteId
    FROM FRED_FONCTIONNALITE f,FRED_PERMISSION_FONCTIONNALITE pf, FRED_PERMISSION p
    WHERE f.FonctionnaliteId=pf.FonctionnaliteId
	and	  pf.PermissionId=p.PermissionId
	and   p.Code=@permissionCode)
BEGIN

    -- Insertion des fonctionnalités 

    DECLARE @fonctionnaliteCode as nvarchar(8)
	SELECT @fonctionnaliteCode ='1511'

	INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	VALUES (@MoyenModuleId, @fonctionnaliteCode,'Affichage du button rapport des moyens', 0, NULL, 'Affichage du button rapport des moyens');

    -- Association des permissions aux fonctionnalités

    INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
        SELECT P.PermissionId, f.FonctionnaliteId
        FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
        where p.Code=@permissionCode and f.code = @fonctionnaliteCode
    
    --Association des roles : Gestionnaire des moyens (RoleSepecification = 3)

     INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
        SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
        FROM FRED_ROLE r, FRED_FONCTIONNALITE f
        where r.Specification=3 and f.code=@fonctionnaliteCode
END
