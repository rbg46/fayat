DECLARE @Code INT;
SET @Code = (SELECT MAX(PermissionId) FROM FRED_PERMISSION);
SELECT @Code + 1

IF NOT EXISTS (SELECT PermissionKey FROM FRED_PERMISSION WHERE PermissionKey = 'button.show.apercubrouillon.commande.detail')
BEGIN
    INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
    VALUES ('button.show.apercubrouillon.commande.detail', 1, @Code + 1, 'Affichage du bouton aperçu brouillon', 0)
END