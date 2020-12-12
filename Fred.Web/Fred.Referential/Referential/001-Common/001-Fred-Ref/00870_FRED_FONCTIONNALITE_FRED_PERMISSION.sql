-- =======================================================================================================================================
-- Author:		Yannick DEFAY  28/05/2019
--
-- Description:
--      - Insertion de la permission pour l'affichage du bouton Pointage par défaut Ecran liste pointage personnel
--
-- =======================================================================================================================================

BEGIN TRAN

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0061')
BEGIN
    INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('button.show.btn-pointage-default', 1, '0061', 'Permet l''affichage du bouton Pointage par défaut dans la liste du pointage personnel', 0)
END
DECLARE @ModulePointageId INT;
SET @ModulePointageId = (SELECT ModuleId FROM FRED_MODULE WHERE Code = '4');
IF NOT EXISTS ( SELECT 1 FROM FRED_FONCTIONNALITE WHERE Code = '423')
BEGIN
    INSERT INTO FRED_FONCTIONNALITE (ModuleId, Code, Libelle, HorsOrga, DateSuppression, Description) 
	VALUES (@ModulePointageId, '423','Affichage du bouton Pointage par défaut', 0, NULL, 'Permet l''affichage du bouton Pointage par défaut dans la liste du pointage personnel');
END

DECLARE @PermissionId INT;
SET @PermissionId = (SELECT PermissionId FROM FRED_PERMISSION WHERE Code = '0061');
DECLARE @FonctionnaliteId INT;
SET @FonctionnaliteId = (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '423');

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION_FONCTIONNALITE WHERE PermissionId = @PermissionId AND FonctionnaliteId = @FonctionnaliteId)
BEGIN
    INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionId, FonctionnaliteId) 
	VALUES (@PermissionId, @FonctionnaliteId);
END

DELETE FROM FRED_PARAM_VALUE WHERE Value LIKE 'btn-pointage-default'

COMMIT TRAN
