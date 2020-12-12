-- =======================================================================================================================================
-- Author:		YM 28/01/2019
--
-- Description:
--      - Insertion de la permission pour l'affichage des boutons pour l''export analytique
--
-- =======================================================================================================================================

BEGIN TRAN

DELETE FROM [dbo].[FRED_PERMISSION] WHERE PermissionKey = 'button.show.export.analytique';
--Declaration
DECLARE @pointageModuleId INT;
DECLARE @permissionCode INT;
DECLARE @newPermissionCode INT;
DECLARE @fonctionnaliteCode NVARCHAR(20);
DECLARE @newfonctionnaliteCode NVARCHAR(20);
DECLARE @fonctionnaliteId INt;
DECLARE @permissionId INT;

SET @pointageModuleId = (SELECT ModuleId FROM FRED_MODULE WHERE Code='4'); -- code 4 pour le module du pointage
SET @permissionCode  = (SELECT TOP 1 Code FROM FRED_PERMISSION ORDER BY PermissionId DESC);
SET @newPermissionCode = @permissionCode + 1;
SET @fonctionnaliteCode = (SELECT TOP 1 Code FROM FRED_FONCTIONNALITE ORDER BY FonctionnaliteId DESC);
SET @newfonctionnaliteCode = @fonctionnaliteCode + 1;

--Insertion

IF NOT EXISTS (SELECT 1 FROM FRED_PERMISSION WHERE PermissionKey = 'button.show.export.analytique')
BEGIN
INSERT INTO[dbo].[FRED_PERMISSION] 
([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
 VALUES('button.show.export.analytique', 1,@newPermissionCode,'Affichage des boutons pour l''export analytique',0);
 SET @permissionId = SCOPE_IDENTITY();
 END

IF NOT EXISTS (SELECT 1 FROM FRED_FONCTIONNALITE WHERE Libelle = 'Affichage des boutons pour l''export analytique')
BEGIN
INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
	   VALUES (@pointageModuleId, @newfonctionnaliteCode ,'Affichage des boutons pour l''export analytique', 0, NULL, 'Affichage des boutons pour l''export analytique');
       SET @fonctionnaliteId = SCOPE_IDENTITY();
END

IF NOT EXISTS (SELECT 1 FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on p.PermissionId = pf.PermissionId inner join FRED_FONCTIONNALITE f on  f.FonctionnaliteId = pf.FonctionnaliteId WHERE p.PermissionId=@permissionId and f.FonctionnaliteId = @fonctionnaliteId)
BEGIN
INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionId, FonctionnaliteId) values (@permissionId, @fonctionnaliteId)
       
END
COMMIT TRAN
