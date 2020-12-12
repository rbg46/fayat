DECLARE  @Code int

SELECT @Code =  MAX(Code) + 1
FROM [FRED_PERMISSION] 

INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
VALUES('menu.show.familleoperationdiverse.index',1,@Code,'Affichage du menu / Accès à la page ''Paramétrage des familles OD''.',0) 