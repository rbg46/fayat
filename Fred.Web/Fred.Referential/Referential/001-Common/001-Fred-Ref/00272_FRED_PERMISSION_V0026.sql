DECLARE @Code INT;
SET @Code = (SELECT MAX(PermissionId) FROM FRED_PERMISSION);
SELECT @Code + 1

IF NOT EXISTS (SELECT PermissionKey FROM FRED_PERMISSION WHERE PermissionKey = 'menu.show.societe.classification.index')
BEGIN
    INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
    VALUES ('menu.show.societe.classification.index', 1, @Code + 1, 'Affichage du menu / Accès à la page ''Gérer les classifications sociétés''', 0)
END