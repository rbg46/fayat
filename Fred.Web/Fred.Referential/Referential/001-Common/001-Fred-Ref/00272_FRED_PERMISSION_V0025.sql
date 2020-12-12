DECLARE @Code INT;
SET @Code = (SELECT TOP 1 Code FROM FRED_PERMISSION ORDER BY PermissionId DESC);
SELECT @Code + 1

INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
VALUES ('button.show.validation.commande.energie.detail', 1, @Code + 1, 'Affichage du bouton de validation d''une commande énergie', 0)
