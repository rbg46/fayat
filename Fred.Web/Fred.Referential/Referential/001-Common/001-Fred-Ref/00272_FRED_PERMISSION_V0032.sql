DECLARE @Code INT;
SET @Code = (SELECT MAX(PermissionId) FROM FRED_PERMISSION);
SELECT @Code + 1

IF NOT EXISTS (SELECT PermissionKey FROM FRED_PERMISSION WHERE PermissionKey = 'button.show.export.reception.interimaire')
BEGIN
    INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle])
    VALUES('button.show.export.reception.interimaire',1, @Code + 1,'Affichage du bouton ''Exporter les réceptions intérimaires''',0) 
END