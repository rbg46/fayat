-- US_5118 : Fonctionnalité Déclôturer une commande

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0128')
BEGIN
	INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('button.show.decloture.commande.index', 1, '0128', 'Affichage du bouton "Déclôturer une commande"', 0)
END