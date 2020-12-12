-- Alimentation de la table FRED_AFFECTATION_MOYEN_TYPE
/*
CREATE PROCEDURE #SET_FRED_AFFECTATION_MOYEN_TYPE
	@Code NVARCHAR(MAX), 
	@Libelle NVARCHAR(MAX), 
	@AffectationMoyenFamilleId int

AS
BEGIN
	IF NOT EXISTS ( SELECT 1 FROM FRED_AFFECTATION_MOYEN_TYPE WHERE Code = @Code)
	BEGIN
		INSERT INTO FRED_AFFECTATION_MOYEN_TYPE (Code, Libelle, AffectationMoyenFamilleId)
		VALUES (@Code, @Libelle, @AffectationMoyenFamilleId)
	END
END
GO

-- Insertion des données

EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='NoAffectation', @Libelle='Non affecté', @AffectationMoyenFamilleId=2;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='Personnel', @Libelle='Personnel', @AffectationMoyenFamilleId=1;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='CI', @Libelle='CI', @AffectationMoyenFamilleId=1;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='PARKING', @Libelle='Parking', @AffectationMoyenFamilleId=2;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='Depot', @Libelle='Dépôt', @AffectationMoyenFamilleId=2;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='Stock', @Libelle='Stock', @AffectationMoyenFamilleId=2;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='Reparation', @Libelle='Réparation', @AffectationMoyenFamilleId=4;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='Entretien', @Libelle='Entretien', @AffectationMoyenFamilleId=4;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='Controle', @Libelle='Contrôle', @AffectationMoyenFamilleId=4;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='RetourLoueur', @Libelle='Retour au loueur', @AffectationMoyenFamilleId=3;
EXEC #SET_FRED_AFFECTATION_MOYEN_TYPE @Code='ResteDisponible', @Libelle='Reste disponible', @AffectationMoyenFamilleId=3;


-- Fin insertion

DROP PROCEDURE #SET_FRED_AFFECTATION_MOYEN_TYPE
*/