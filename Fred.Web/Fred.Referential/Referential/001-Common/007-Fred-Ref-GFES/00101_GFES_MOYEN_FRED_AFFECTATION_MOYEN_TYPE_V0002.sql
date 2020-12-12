-- VERSION 2 : On force les Ids pour correspondre avec l'enum.
-- Alimentation de la table FRED_AFFECTATION_MOYEN_TYPE
/*
CREATE PROCEDURE #SET_FRED_AFFECTATION_MOYEN_TYPE
    @AffectationMoyenTypeId int,
    @Code NVARCHAR(MAX), 
    @Libelle NVARCHAR(MAX), 
    @AffectationMoyenFamilleId int

AS
BEGIN
    IF NOT EXISTS ( SELECT 1 FROM FRED_AFFECTATION_MOYEN_TYPE WHERE Code = @Code)
    BEGIN
        SET IDENTITY_INSERT FRED_AFFECTATION_MOYEN_TYPE ON;
        INSERT INTO FRED_AFFECTATION_MOYEN_TYPE (AffectationMoyenTypeId, Code, Libelle, AffectationMoyenFamilleId)
        VALUES (@AffectationMoyenTypeId, @Code, @Libelle, @AffectationMoyenFamilleId)
        SET IDENTITY_INSERT FRED_AFFECTATION_MOYEN_TYPE OFF;
    END
END
GO
-- Suppression des anciens valeurs
DELETE FROM FRED_AFFECTATION_MOYEN_TYPE;

-- Insertion des données
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=1, @Code='NoAffectation', @Libelle='Non affecté', @AffectationMoyenFamilleId=2;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=2, @Code='Personnel', @Libelle='Personnel', @AffectationMoyenFamilleId=1;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=3, @Code='CI', @Libelle='CI', @AffectationMoyenFamilleId=1;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=4, @Code='PARKING', @Libelle='Parking', @AffectationMoyenFamilleId=2;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=5, @Code='Depot', @Libelle='Dépôt', @AffectationMoyenFamilleId=2;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=6, @Code='Stock', @Libelle='Stock', @AffectationMoyenFamilleId=2;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=7, @Code='Reparation', @Libelle='Réparation', @AffectationMoyenFamilleId=4;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=8, @Code='Entretien', @Libelle='Entretien', @AffectationMoyenFamilleId=4;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=9, @Code='Controle', @Libelle='Contrôle', @AffectationMoyenFamilleId=4;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=10, @Code='RetourLoueur', @Libelle='Retour au loueur', @AffectationMoyenFamilleId=3;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @AffectationMoyenTypeId=11, @Code='ResteDisponible', @Libelle='Reste disponible', @AffectationMoyenFamilleId=3;

-- Fin insertion

DROP PROCEDURE #SET_FRED_AFFECTATION_MOYEN_TYPE
*/