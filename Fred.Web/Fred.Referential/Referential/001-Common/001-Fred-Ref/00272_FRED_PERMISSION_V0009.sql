IF NOT EXISTS(SELECT * FROM FRED_PERMISSION WHERE CODE  ='0080' AND PermissionKey = 'button.show.remonteeprime.validationprime.index')
	-- Si le code est la permission n'existe pas 
	INSERT INTO FRED_PERMISSION   
	SELECT 'button.show.remonteeprime.validationprime.index',1,'0080','Affichage du bouton ''Valider les primes mensuelles.''.',0
ELSE IF NOT EXISTS(SELECT * FROM FRED_PERMISSION WHERE  PermissionKey = 'button.show.remonteeprime.validationprime.index')
	-- Si le code existe déjà mais pas la permission
	DECLARE @Code as nvarchar(8)
	SELECT @Code ='00'+CAST(MAX(CAST(CODE AS INT)) +10 as nvarchar ) FROM FRED_PERMISSION
	PRINT @Code
	INSERT INTO FRED_PERMISSION   
	SELECT 'button.show.remonteeprime.validationprime.index',1, @Code, 'Affichage du bouton ''Valider les primes mensuelles.''.',0

-- Ajout des permissions pour la gestion des moyens
INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
VALUES ('menu.show.gestion.moyen', 1, '0100', 'Gestion des moyens', 0)