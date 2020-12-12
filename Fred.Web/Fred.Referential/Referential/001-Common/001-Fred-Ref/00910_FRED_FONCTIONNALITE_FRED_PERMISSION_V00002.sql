-- =======================================================================================================================================
-- Author:		Nicolas PINSARD 24/07/2019
--
-- Description:
--      - Insertion de la permission pour l'affichage du bouton "Copier les ressources d''un budget"
--
-- =======================================================================================================================================

BEGIN TRAN

DECLARE @permissionCode nvarchar(20) = '811'
DECLARE @fonctionnaliteCode varchar(255) = '811'

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = @permissionCode)
BEGIN
    INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('button.show.copier.ressources.budget', 1, @permissionCode, 'Affichage du bouton "Copier les ressources d''un budget"', 0)
END
DECLARE @ModuleBudgetId INT;
SET @ModuleBudgetId = (SELECT ModuleId FROM FRED_MODULE WHERE Code = '12');
IF NOT EXISTS ( SELECT 1 FROM FRED_FONCTIONNALITE WHERE Code = @fonctionnaliteCode)
BEGIN
    INSERT INTO FRED_FONCTIONNALITE (ModuleId, Code, Libelle, HorsOrga, DateSuppression, Description) 
	VALUES (@ModuleBudgetId, @fonctionnaliteCode,'Bouton "Copier les ressources d''un budget"', 0, NULL, 'Permet d''afficher le bouton "Copier les ressources d''un budget"');
END

DECLARE @permissionId INT;
SET @permissionId = (SELECT PermissionId FROM FRED_PERMISSION WHERE Code = @permissionCode);
DECLARE @fonctionnaliteId INT;
SET @fonctionnaliteId = (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = @fonctionnaliteCode);

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION_FONCTIONNALITE WHERE PermissionId = @permissionId AND FonctionnaliteId = @fonctionnaliteId)
BEGIN
    INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionId, FonctionnaliteId) 
	VALUES (@permissionId, @fonctionnaliteId);
END

COMMIT TRAN
