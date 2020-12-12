-- =======================================================================================================================================
-- Author:		Yannick DEFAY  18/07/2019
--
-- Description:
--      - Insertion de la permission pour l'affichage de la section déplacement du rapport journalier
--
-- =======================================================================================================================================

BEGIN TRAN

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0062')
BEGIN
    INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	VALUES ('show.rapportJournalier.deplacement', 1, '0062', 'Permet au rôle lié d’accéder à la section déplacement dans le rapport journalier', 0)
END
DECLARE @ModulePointageId INT;
SET @ModulePointageId = (SELECT ModuleId FROM FRED_MODULE WHERE Code = '4');
IF NOT EXISTS ( SELECT 1 FROM FRED_FONCTIONNALITE WHERE Code = '424')
BEGIN
    INSERT INTO FRED_FONCTIONNALITE (ModuleId, Code, Libelle, HorsOrga, DateSuppression, Description) 
	VALUES (@ModulePointageId, '424','Pré-Saisie des informations de déplacements par les opérationnels', 0, NULL, 'Permet au rôle lié d’accéder à la section déplacement dans le rapport journalier');
END

DECLARE @PermissionId INT;
SET @PermissionId = (SELECT PermissionId FROM FRED_PERMISSION WHERE Code = '0062');
DECLARE @FonctionnaliteId INT;
SET @FonctionnaliteId = (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code = '424');

IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION_FONCTIONNALITE WHERE PermissionId = @PermissionId AND FonctionnaliteId = @FonctionnaliteId)
BEGIN
    INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionId, FonctionnaliteId) 
	VALUES (@PermissionId, @FonctionnaliteId);
END

COMMIT TRAN
