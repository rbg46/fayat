-- Alimentation de la table FRED_CHAPITRE

-- NOTE JNE : Il existe déjà des PS pour faire ça : Fred_ToolBox, merci de les utiliser


CREATE PROCEDURE #SET_FRED_CHAPITRE
	@Code nvarchar(20), 
	@Libelle nvarchar(500), 
	@DateCreation datetime
AS
BEGIN
	IF NOT EXISTS ( SELECT 1 FROM FRED_CHAPITRE WHERE Code = @Code)
	BEGIN
		INSERT INTO FRED_CHAPITRE (Code, Libelle, DateCreation, GroupeId)
		-- La valeur du group id doit étre insérée au départ et va étre remplacée
		VALUES (@Code, @Libelle, @DateCreation, 1)
	END
END
GO

-- Insertion des données

EXEC #SET_FRED_CHAPITRE @Code='EIMATERIEL', @Libelle='EI Matériel et outillage', @DateCreation='2010-02-04';
EXEC #SET_FRED_CHAPITRE @Code='EIROULANT', @Libelle='EIROULANT', @DateCreation='2010-02-04';

-- Fin insertion

-- Update des colonnes GroupId et AuteurCreationId

DECLARE @groupId int;
SET @groupId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code LIKE '%GFES%'); 

DECLARE @auteurCreationId int;
SET @auteurCreationId = (SELECT UtilisateurId FROM FRED_UTILISATEUR WHERE Login LIKE 'fred_ie'); 

Update FRED_CHAPITRE
SET GroupeId = @groupId,
	AuteurCreationId = @auteurCreationId
WHERE Code in ('EIMATERIEL', 'EIROULANT')

-- Fin update des colonnes GroupId et AuteurCreationId


DROP PROCEDURE #SET_FRED_CHAPITRE

