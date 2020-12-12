DECLARE @Code INT;
SET @Code = (SELECT TOP 1 Code FROM FRED_PERMISSION ORDER BY PermissionId DESC);
SELECT @Code + 1

INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
VALUES ('menu.show.ressourcesspecifiquesci.index', 1, @Code + 1, 'Affichage du menu / Accès à la page ''Gérer les ressources spécifiques CI''.', 0)
