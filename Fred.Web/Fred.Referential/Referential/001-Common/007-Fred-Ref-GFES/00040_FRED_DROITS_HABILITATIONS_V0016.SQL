-- =======================================================================================================================================
-- Author:		YM 02/01/2019
--
-- Description:
--      - Insertion de la permission pour l'affichage de l'ecran validation de mes affaires
--
-- =======================================================================================================================================

BEGIN TRAN
--Declaration
DECLARE @pointageModuleId INT;
DECLARE @permissionCode INT;
DECLARE @newPermissionCode INT;
DECLARE @fonctionnaliteCode NVARCHAR(20);
DECLARE @newfonctionnaliteCode NVARCHAR(20);

SET @pointageModuleId = (SELECT ModuleId FROM FRED_MODULE WHERE Code='4'); -- code 4 pour le module du pointage
SET @permissionCode  = (SELECT TOP 1 Code FROM FRED_PERMISSION ORDER BY PermissionId DESC);
SET @newPermissionCode = @permissionCode + 1;
SET @fonctionnaliteCode = (SELECT TOP 1 Code FROM FRED_FONCTIONNALITE ORDER BY FonctionnaliteId DESC);
SET @newfonctionnaliteCode = @fonctionnaliteCode + 1;

--Insertion

IF NOT EXISTS (SELECT 1 FROM FRED_PERMISSION WHERE PermissionKey = 'menu.show.validation.affaires.ouvriers.forResponsable')
BEGIN
INSERT INTO[dbo].[FRED_PERMISSION] 
([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
 VALUES('menu.show.validation.affaires.ouvriers.forResponsable', 1,@newPermissionCode,'Affichage écran validation affaires ouvriers par reponsable',0);
 END

IF NOT EXISTS (SELECT 1 FROM FRED_FONCTIONNALITE WHERE Libelle = 'Affichage écran validation affaires ouvriers par reponsable')
BEGIN
INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	   VALUES (@pointageModuleId, @newfonctionnaliteCode ,'Affichage écran validation affaires ouvriers par reponsable', 0, NULL, 'Affichage écran validation affaires ouvriers par reponsable');
END

IF NOT EXISTS (SELECT 1 FROM FRED_PERMISSION_FONCTIONNALITE pf, FRED_PERMISSION p, FRED_FONCTIONNALITE f WHERE p.Code=@newPermissionCode and f.code = @newfonctionnaliteCode)
BEGIN
INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
       SELECT P.PermissionId, f.FonctionnaliteId
       FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
       where p.Code=@newPermissionCode and f.code = @newfonctionnaliteCode;
END
COMMIT TRAN
