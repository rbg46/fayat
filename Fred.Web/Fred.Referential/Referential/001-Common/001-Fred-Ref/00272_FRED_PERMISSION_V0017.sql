IF NOT EXISTS(SELECT * FROM FRED_PERMISSION WHERE CODE  ='00149' AND PermissionKey = 'button.show.validationprime.validationprime.index')
BEGIN
	-- Si le code est la permission n'existe pas 
	INSERT INTO FRED_PERMISSION   
	SELECT 'button.show.validationprime.validationprime.index',1,'00149','Affichage du bouton ''Valider les primes mensuelles''.',0
END
ELSE IF NOT EXISTS(SELECT * FROM FRED_PERMISSION WHERE  PermissionKey = 'button.show.validationprime.validationprime.index')
BEGIN
	-- Si le code existe déjà mais pas la permission
	DECLARE @Code as nvarchar(8)
	SELECT @Code ='00'+CAST(MAX(CAST(CODE AS INT)) +10 as nvarchar ) FROM FRED_PERMISSION
	INSERT INTO FRED_PERMISSION   
	SELECT 'button.show.validationprime.validationprime.index',1, @Code, 'Affichage du bouton ''Valider les primes mensuelles''.',0
END

IF EXISTS (SELECT * FROM FRED_PERMISSION WHERE  PermissionKey = 'button.show.remonteeprime.validationprime.index' having count(*) >1) 
BEGIN 
	DELETE FROM FRED_PERMISSION WHERE  PermissionId = (SELECT  TOP 1 PermissionId FROM FRED_PERMISSION WHERE  PermissionKey = 'button.show.remonteeprime.validationprime.index' order by PermissionId desc)
END