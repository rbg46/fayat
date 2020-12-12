-- =======================================================================================================================================
-- Author:		NPI  16/09/2019
--
-- Description:
--      - Insertion de la permission pour la comparaison de budget
--
-- =======================================================================================================================================

BEGIN TRAN
    DECLARE @permissionCode nvarchar(20) = '2001'
    DECLARE @permissionKey nvarchar(max) = 'menu.show.budget.budget-comparaison.index'
    DECLARE @permissionLibelle nvarchar(max) ='Affichage du menu / Accès à la page ''Comparaison de budget''.'
    DECLARE @moduleCode varchar(255) = 12           -- Gestion budgetaire
    DECLARE @fonctionnaliteCode varchar(255) = '2001'
    DECLARE @fonctionnaliteLibelle varchar(255) = 'Comparaison de budget'
    DECLARE @fonctionnaliteDescription nvarchar(max) = 'Permet de comparer des budgets'

    IF NOT EXISTS (SELECT 1 FROM FRED_PERMISSION WHERE Code = @permissionCode)
    BEGIN
        INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	    VALUES (@permissionKey, 1, @permissionCode, @permissionLibelle, 0)
    END
    DECLARE @moduleId int;
    SET @moduleId = (SELECT ModuleId FROM FRED_MODULE WHERE Code = @moduleCode);
    IF NOT EXISTS (SELECT 1 FROM FRED_FONCTIONNALITE WHERE Code = @fonctionnaliteCode)
    BEGIN
        INSERT INTO FRED_FONCTIONNALITE (ModuleId, Code, Libelle, HorsOrga, Description) 
	    VALUES (@moduleId, @fonctionnaliteCode, @fonctionnaliteLibelle, 0, @fonctionnaliteDescription);
    END

    DECLARE @permissionId int;
    SET @permissionId = (SELECT PermissionId FROM FRED_PERMISSION WHERE Code = @permissionCode);
    DECLARE @fonctionnaliteId int;
    SET @fonctionnaliteId = (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = @fonctionnaliteCode);
    IF NOT EXISTS (SELECT 1 FROM FRED_PERMISSION_FONCTIONNALITE WHERE PermissionId = @permissionId AND FonctionnaliteId = @fonctionnaliteId)
    BEGIN
        INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionId, FonctionnaliteId) 
	    VALUES (@permissionId, @fonctionnaliteId);
    END
COMMIT TRAN
