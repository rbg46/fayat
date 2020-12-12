-- Alimentation de la table FRED_PERMISSION avec les permissions pour la gestion des listes des affectations moyens
-- et la gestion des CI / Personnels pour les affectations moyens
CREATE PROCEDURE #SET_FRED_PERMISSION
	@PermissionKey nvarchar(max), 
	@PermissionType int, 
	@Code nvarchar(max), 
	@Libelle nvarchar(max), 
	@PermissionContextuelle bit

AS
BEGIN
	IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = @Code)
	BEGIN
		INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
		VALUES (@PermissionKey, @PermissionType, @Code, @Libelle, @PermissionContextuelle)
	END
END
GO

-- Insertion des données

EXEC #SET_FRED_PERMISSION @PermissionKey='show.all.affectation.moyen', @PermissionType=1, @Code='0101', @Libelle='Affichage de toute la liste des affectations des moyens', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.manager.personnel.affectation.moyen', @PermissionType=1, @Code='0102', @Libelle='Affichage de la liste des affectations moyens liées au personnels d''un manager', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.responsable.ci.affectation.moyen', @PermissionType=1, @Code='0103', @Libelle='Affichage de la liste des affectations liées au CI d''un responsable CI', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.delegue.ci.affectation.moyen', @PermissionType=1, @Code='0104', @Libelle='Affichage de la liste des affectations moyens liées au CI d''un délégué', @PermissionContextuelle=0;

EXEC #SET_FRED_PERMISSION @PermissionKey='show.all.ci', @PermissionType=1, @Code='0105', @Libelle='Affichage de toute la liste des CI', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.responsable.ci.cilist', @PermissionType=1, @Code='0106', @Libelle='Affichage de la liste des CI d''un responsable CI', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.delegue.ci.cilist', @PermissionType=1, @Code='0107', @Libelle='Affichage de la liste des CI d''un délégué', @PermissionContextuelle=0;

EXEC #SET_FRED_PERMISSION @PermissionKey='show.all.personnels', @PermissionType=1, @Code='0108', @Libelle='Affichage de toute la liste des personnels', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.manager.personnel.members', @PermissionType=1, @Code='0109', @Libelle='Affichage de la liste des personnels d''un manager', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.responsable.ci.personnelList', @PermissionType=1, @Code='0110', @Libelle='Affichage de la liste des personnels affectés aux CI d''un responsable CI', @PermissionContextuelle=0;

-- Fin insertion

DROP PROCEDURE #SET_FRED_PERMISSION