-- =======================================================================================================================================
-- Author:		Yoann Collet  21/03/2019
--
-- Description:
--      - Insertion de la permission pour l'affichage de l'icone amenant au tuto de fred
--
-- =======================================================================================================================================



IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '1516')
BEGIN
	INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('button.show.fred.tuto', 1, '1516', 'Affichage de l''icone amenant à FRED Tuto', 0)
END