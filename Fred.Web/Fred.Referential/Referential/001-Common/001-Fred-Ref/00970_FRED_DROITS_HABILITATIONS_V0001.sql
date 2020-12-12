
-- Ce script est dans le Ref commun pour la corerction d'un bug qui concerne le champ permission code qu'est un type string mais on doit mettre des INT !

DECLARE @permissionCode INT;
DECLARE @newPermissionCode INT;
DECLARE @permissonKey NVARCHAR(MAX);
DECLARE @permissionId INT;

DECLARE @fonctionnaliteCode INT;
DECLARE @newFonctionnaliteCode INT;
DECLARE @fonctionnaliteLibelle NVARCHAR(MAX);
DECLARE @fonctionnaliteId INT;

DECLARE @pointageModuleId INT;

SET @fonctionnaliteLibelle = 'Affichage edition rapports dans liste des rapports';
SET @permissonKey = 'menu.show.edition.rapport.liste.rapport';

SET @pointageModuleId = (SELECT ModuleId FROM FRED_MODULE WHERE Code='4');

BEGIN

DELETE fonct FROM FRED_PERMISSION_FONCTIONNALITE fonct inner join FRED_PERMISSION perm on fonct.PermissionId = perm.PermissionId  WHERE perm.PermissionKey = @permissonKey  ;
DELETE FROM FRED_PERMISSION WHERE PermissionKey = @permissonKey;
DELETE FROM FRED_FONCTIONNALITE WHERE Libelle = @fonctionnaliteLibelle;

SET @permissionCode  = CONVERT(INT, (SELECT TOP 1 Code FROM FRED_PERMISSION ORDER BY PermissionId DESC));
SET @newPermissionCode = CONVERT(NVARCHAR, (@permissionCode + 1));

SET @fonctionnaliteCode = CONVERT(INT, (SELECT TOP 1 Code FROM FRED_FONCTIONNALITE ORDER BY FonctionnaliteId DESC));
SET @newFonctionnaliteCode = CONVERT(NVARCHAR, (@fonctionnaliteCode + 1));

-- Id est normalement auto incremente , c'est en dur juste pour eviter des problémes liées a les anciens scripts

INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
VALUES (@pointageModuleId, @newFonctionnaliteCode,@fonctionnaliteLibelle, 0, NULL, @fonctionnaliteLibelle);
SET @fonctionnaliteId = SCOPE_IDENTITY()

INSERT INTO FRED_PERMISSION ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
VALUES('menu.show.edition.rapport.liste.rapport', 1,@newPermissionCode,'Affichage edition rapports dans liste des rapports',0);
SET @permissionId = SCOPE_IDENTITY()

INSERT INTO FRED_PERMISSION_FONCTIONNALITE(PermissionId, FonctionnaliteId) Values (@permissionId, @fonctionnaliteId)

END
