DECLARE @Code INT;
SET @Code = (SELECT MAX(Code) FROM FRED_PERMISSION);

INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
VALUES ('button.show.btn.avenant.always', 1, @Code + 1, 'Affichage permanent du bouton Avenant ', 0)
