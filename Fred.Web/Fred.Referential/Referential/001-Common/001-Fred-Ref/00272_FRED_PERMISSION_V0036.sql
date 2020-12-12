DECLARE  @Code int

SELECT @Code =  MAX(Code) + 1
FROM [FRED_PERMISSION] 

INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
VALUES('functionality.lock.unlock.commandeLigne',1,@Code,'Habilitation pour verrouillage et déverrouillage manuel de ligne de commande',0) 