CREATE PROCEDURE #ADD_FONCTIONNALITE_PERSMISSION
@CodeFonc VARCHAR(MAX),
@Libelle VARCHAR(MAX),
@Description NVARCHAR(MAX),
@PermissionCode NVARCHAR(MAX)
AS
BEGIN
	DECLARE @ModuleId INT;
	SET @ModuleId = (SELECT ModuleId FROM FRED_MODULE WHERE Code='4')
	
	DECLARE @LastFonctionnaliteId INT;
	INSERT INTO FRED_FONCTIONNALITE (ModuleId, Code, Libelle, HorsOrga, DateSuppression, [Description])
	VALUES (@ModuleId, @CodeFonc, @Libelle, 0, null, @Description)
	
	SET @LastFonctionnaliteId = (SELECT max(FonctionnaliteId) FROM FRED_FONCTIONNALITE)
	DECLARE @PermissionId INT;
	SET @PermissionId = (SELECT PermissionId FROM FRED_PERMISSION WHERE Code=@PermissionCode)
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE(PermissionId, FonctionnaliteId) VALUES (@PermissionId, @LastFonctionnaliteId)
END
GO

EXEC #ADD_FONCTIONNALITE_PERSMISSION '406', 'Affichage du Rapport par ouvrier', 'Saisie du Rapport hebdomadaire par ouvrier', '0057'
EXEC #ADD_FONCTIONNALITE_PERSMISSION '407', 'Affichage du Rapport ETAM IAC', 'Saisie du Rapport hebdomadaire ETAM IAC', '0058'
EXEC #ADD_FONCTIONNALITE_PERSMISSION '408', 'Affichage de la synthèse mensuelle', 'Affichage de la synthèse mensuelle pour les ETAM IAC', '0059'

DROP PROCEDURE #ADD_FONCTIONNALITE_PERSMISSION
GO