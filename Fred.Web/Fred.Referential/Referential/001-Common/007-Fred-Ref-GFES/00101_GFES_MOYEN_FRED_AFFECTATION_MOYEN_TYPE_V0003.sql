-- VERSION 3 : On recherche les pour correspondre avec l'enum.
-- Alimentation de la table FRED_AFFECTATION_MOYEN_TYPE

CREATE PROCEDURE #SET_FRED_AFFECTATION_MOYEN_TYPE
    @AffectationMoyenTypeId int,
    @Code NVARCHAR(MAX), 
    @Libelle NVARCHAR(MAX), 
    @CodeFamille NVARCHAR(MAX)

AS
BEGIN
    IF NOT EXISTS ( SELECT 1 FROM FRED_AFFECTATION_MOYEN_TYPE WHERE Code = @Code)
    BEGIN
        DECLARE @familleid INT
        SET @familleid = (select AffectationMoyenFamilleId from FRED_AFFECTATION_MOYEN_FAMILLE where Code = @CodeFamille)

        SET IDENTITY_INSERT FRED_AFFECTATION_MOYEN_TYPE ON;
        INSERT INTO FRED_AFFECTATION_MOYEN_TYPE (AffectationMoyenTypeId, Code, Libelle, AffectationMoyenFamilleId)
        VALUES (@AffectationMoyenTypeId, @Code, @Libelle, @familleid)
        SET IDENTITY_INSERT FRED_AFFECTATION_MOYEN_TYPE OFF;
    END
END
GO
-- Suppression des anciens valeurs
DELETE FROM FRED_AFFECTATION_MOYEN_TYPE;

-- Insertion des données
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=1, @Code='NoAffectation', @Libelle='Non affecté', @CodeFamille='DISP';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=2, @Code='Personnel', @Libelle='Personnel', @CodeFamille='AFFECT';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=3, @Code='CI', @Libelle='CI', @CodeFamille='AFFECT';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=4, @Code='PARKING', @Libelle='Parking', @CodeFamille='DISP';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=5, @Code='Depot', @Libelle='Dépôt', @CodeFamille='DISP';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=6, @Code='Stock', @Libelle='Stock', @CodeFamille='DISP';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=7, @Code='Reparation', @Libelle='Réparation', @CodeFamille='MAINT';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=8, @Code='Entretien', @Libelle='Entretien', @CodeFamille='MAINT';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=9, @Code='Controle', @Libelle='Contrôle', @CodeFamille='MAINT';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=10, @Code='RetourLoueur', @Libelle='Retour au loueur', @CodeFamille='LOC';
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=11, @Code='ResteDisponible', @Libelle='Reste disponible', @CodeFamille='LOC';

-- Fin insertion

DROP PROCEDURE #SET_FRED_AFFECTATION_MOYEN_TYPE

