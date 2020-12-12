-- Modification et insertion du validation rapport permission
IF NOT EXISTS (SELECT Code FROM FRED_PERMISSION WHERE Code = '0053')
BEGIN
	INSERT INTO FRED_PERMISSION ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
 VALUES('button.enabled.validate.rapport.index', 1,'0053','Affichage et activation du bouton valider rapport.',0)
END
IF EXISTS(SELECT Code FROM FRED_PERMISSION WHERE Code = '0053')
BEGIN
	UPDATE FRED_PERMISSION SET PermissionContextuelle = 0 WHERE  Code = '0053'
END