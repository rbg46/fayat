
-- FES - Gestion des moyens
-- Permissions pour gérer l'Affichage du button rapport des moyens 

 DECLARE @Code as nvarchar(8)
SELECT @Code ='1511';

INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
VALUES ('button.show.rapport.moyens', 1,@Code, 'Affichage du button rapport des moyens', 0)



