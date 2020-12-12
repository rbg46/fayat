-- =======================================================================================================================================
-- Author:		Yoann Collet  15/02/2019
--
-- Description:
--      - Insertion de la permission pour l'affichage d'une checkbox matériel à pointer dans l'en-tête des commandes
--
-- =======================================================================================================================================



IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0134')
BEGIN
	INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('checkbox.show.commande.materielapointer', 1, '0134', 'Affichage de l''option ''Matériel à pointer dans les rapports'' dans une commande Location', 0)
END