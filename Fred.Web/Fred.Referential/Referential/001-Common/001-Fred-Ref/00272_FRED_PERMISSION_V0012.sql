--Script correctif d'insertion, à cause des mauvais nommage des fichiers pour les permissions

IF NOT EXISTS (SELECT 1 FROM [dbo].[FRED_PERMISSION] WHERE Code='0070')
BEGIN
    INSERT INTO [dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
    VALUES('menu.show.export.pointagepersonnel.index', 1,'0070','Affichage du menu / Accès à la page ''Export pointage personnel''.',0)
END