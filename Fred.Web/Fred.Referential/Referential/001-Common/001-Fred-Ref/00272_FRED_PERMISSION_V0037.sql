DECLARE  @Code int

SELECT @Code =  MAX(Code) + 1
FROM [FRED_PERMISSION] 

INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
VALUES('menu.show.famillesoperationsdiverses.list',1,@Code,'Affichage du menu / Accès à la page ''Familles opérations diverses''.',0)

SELECT @Code =  MAX(Code) + 1
FROM [FRED_PERMISSION] 

INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
VALUES('menu.show.famillesoperationsdiverses.association',1,@Code,'Affichage du menu / Accès à la page ''Association Famille OD/Natures/Journaux''.',0) 