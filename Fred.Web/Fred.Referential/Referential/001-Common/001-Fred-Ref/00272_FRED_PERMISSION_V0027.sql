DECLARE @Code INT;
SET @Code = (SELECT MAX(PermissionId) FROM FRED_PERMISSION);
SELECT @Code + 1

IF NOT EXISTS (SELECT PermissionKey FROM FRED_PERMISSION WHERE PermissionKey = 'menu.show.classificationsociete.index')
BEGIN
    INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle])
    VALUES('menu.show.classificationsociete.index',1, @Code + 1,'Affichage du menu / Accès à la page ''Gérer les classification sociétés''.',0) 
END