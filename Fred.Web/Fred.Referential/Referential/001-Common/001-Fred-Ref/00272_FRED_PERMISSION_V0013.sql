-- Alimentation de la table FRED_PERMISSION avec les permissions pour la gestion des moyens 
-- Habilitations sur les actions 

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

-- Affectations des moyens
EXEC #SET_FRED_PERMISSION @PermissionKey='button.show.affectation.moyens', @PermissionType=1, @Code='0111', @Libelle='Affichage du bouton d''affectation', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='button.show.restitution.moyens', @PermissionType=1, @Code='0112', @Libelle='Affichage du bouton de restitution', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='button.show.location.moyens', @PermissionType=1, @Code='0113', @Libelle='Affichage du bouton de location', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='button.show.maintenance.moyens', @PermissionType=1, @Code='0114', @Libelle='Affichage du bouton de maintenance', @PermissionContextuelle=0;

-- Filtre recherche par CI et Personnel
EXEC #SET_FRED_PERMISSION @PermissionKey='show.filtre.recherche.personnel.moyens', @PermissionType=1, @Code='0115', @Libelle='Recherche : Affichage du de la lookup de filtre par personnel', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.filtre.recherche.tous.moyens', @PermissionType=1, @Code='0116', @Libelle='Recherche : Affichage par personnel et Ci (Tous)', @PermissionContextuelle=0;

-- Séléction pour affectation
EXEC #SET_FRED_PERMISSION @PermissionKey='show.select.moyens', @PermissionType=1, @Code='0117', @Libelle='Affectation : séléction des moyens pour affectation', @PermissionContextuelle=0;

-- Filtre recherche avancée 
EXEC #SET_FRED_PERMISSION @PermissionKey='show.filtre.site.moyens', @PermissionType=1, @Code='0118', @Libelle='Filtre avancé : filtrer par site', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.filtre.statut.moyens', @PermissionType=1, @Code='0119', @Libelle='Filtre avancé : filtrer par statut d''affectation', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.filtre.date.fin.moyens', @PermissionType=1, @Code='0120', @Libelle='Filtre avancé : filtrer par date de fin', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.filtre.moyen.rapatrier.moyens', @PermissionType=1, @Code='0121', @Libelle='Affectation : filtrer par moyen à rapatrier', @PermissionContextuelle=0;

-- Validation et annulation 
EXEC #SET_FRED_PERMISSION @PermissionKey='show.bouton.valider.affectation.moyens', @PermissionType=1, @Code='0122', @Libelle='Validation de l''affectation', @PermissionContextuelle=0;
EXEC #SET_FRED_PERMISSION @PermissionKey='show.bouton.annuler.affectation.moyens', @PermissionType=1, @Code='0123', @Libelle='Annulation de l''affectation', @PermissionContextuelle=0;

-- Accés date de fin d'affectation
EXEC #SET_FRED_PERMISSION @PermissionKey='show.date.fin.affectation.moyens', @PermissionType=1, @Code='0124', @Libelle='Accès à la date de fin d''affectation', @PermissionContextuelle=0;


-- Fin insertion

DROP PROCEDURE #SET_FRED_PERMISSION