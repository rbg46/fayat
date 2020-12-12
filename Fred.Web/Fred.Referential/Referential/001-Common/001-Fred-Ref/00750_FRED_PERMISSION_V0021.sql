-- =======================================================================================================================================
-- Author:		Yoann Collet  15/02/2019
--
-- Description:
--      - Insertion de la permission pour l'affichage d'une checkbox matériel à pointer dans l'en-tête des commandes
--
-- =======================================================================================================================================



IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0135')
BEGIN
    INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('menu.show.ressourcesRecommandees.index', 1, '0135', 'Affichage du menu "Ressources Recommandées"', 0)
END