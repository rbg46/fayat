DECLARE @Code INT;
SET @Code = (SELECT MAX(PermissionId) FROM FRED_PERMISSION);
SELECT @Code + 1

IF NOT EXISTS (SELECT PermissionKey FROM FRED_PERMISSION WHERE PermissionKey = 'functionality.enable.create.commande.fournisseur.provisoire')
BEGIN
    INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle])
    VALUES('functionality.enable.create.commande.fournisseur.provisoire',1, @Code + 1,'Autoriser à créer et imprimer un brouillon de commande avec un fournisseur provisoire et des quantités à 0 ',0) 
END