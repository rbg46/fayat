-- Correction du BUG 4466
-- ce controle UI n'apparait pas dans tous les environnements

IF EXISTS (SELECT * FROM FRED_PERMISSION WHERE PermissionKey LIKE 'menu.show.compte.exploitation.index' AND Code LIKE '0048') AND NOT EXISTS (SELECT * FROM FRED_PERMISSION WHERE Code LIKE '0060')
BEGIN
	DECLARE @PermissionId INT;
	
	INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) VALUES('menu.show.compte.exploitation.index',1,'0060','Affichage du menu / Accès à la page ''Editions Compte Exploitation''.',0)
	
	SELECT @PermissionId = PermissionId FROM FRED_PERMISSION WHERE PermissionKey LIKE 'menu.show.compte.exploitation.index' AND Code LIKE '0048';
	UPDATE FRED_PERMISSION_FONCTIONNALITE SET PermissionId = @@IDENTITY WHERE PermissionId = @PermissionId;

	DELETE FROM FRED_PERMISSION WHERE PermissionKey LIKE 'menu.show.compte.exploitation.index' AND Code LIKE '0048';		
END 
ELSE IF NOT EXISTS (SELECT * FROM FRED_PERMISSION WHERE PermissionKey LIKE 'menu.show.compte.exploitation.index')
BEGIN
	INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) VALUES('menu.show.compte.exploitation.index',1,'0060','Affichage du menu / Accès à la page ''Editions Compte Exploitation''.',0)
END