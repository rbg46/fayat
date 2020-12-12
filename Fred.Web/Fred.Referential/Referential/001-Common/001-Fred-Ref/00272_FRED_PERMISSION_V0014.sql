
-- FES - Gestion des moyens
-- Permissions pour gérer l'affichage du filtre par Ci et le champs responsable ou manager
-- ces permissions sont nécessaires dans le cas ou aucun role (Gestionnaire moyen , Responsable Ci, Délégué Ci) n'est affécté à l'utilisateur .

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0125')
BEGIN
	INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('show.filtre.recherche.ci.moyens', 1, '0125', 'Recherche : Affichage de la lookup de filtre par Ci', 0)
END

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0126')
BEGIN
	INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('show.filtre.champs.responsable.moyens', 1, '0126', 'Recherche : Affichage du champs responsable ou manager', 0)
END