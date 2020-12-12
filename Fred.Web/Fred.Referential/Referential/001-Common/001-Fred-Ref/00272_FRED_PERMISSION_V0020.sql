DECLARE @Code INT;
SET @Code = (SELECT TOP 1 Code FROM FRED_PERMISSION ORDER BY PermissionId DESC);
SELECT @Code + 1

INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
VALUES ('button.show.renvoyerverssap.commande.detail', 1, @Code + 1, 'Renvoyer la commande vers le module rapprochement SAP', 0)   