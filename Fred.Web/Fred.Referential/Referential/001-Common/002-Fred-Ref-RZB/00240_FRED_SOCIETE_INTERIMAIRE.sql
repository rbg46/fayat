-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- Ajout des sociétés intérimaire
-- --------------------------------------------------


DECLARE @INTERIM_ID INT;
SET @INTERIM_ID = (SELECT COUNT(*) FROM FRED_SOCIETE where code = 'SOC_INTERIM_RZB')

IF @INTERIM_ID = 0
BEGIN

	DECLARE @ORGANISATION_ID INT; 
	INSERT INTO FRED_ORGANISATION (TypeOrganisationId, PereId) VALUES (4,5)

	SET @ORGANISATION_ID = (SELECT SCOPE_IDENTITY());
	
	INSERT INTO FRED_SOCIETE (GroupeId, Code, Libelle, Externe, Active, IsGenerationSamediCPActive, ImportFacture, TransfertAS400, IsInterimaire, OrganisationId)
	VALUES (1, 'SOC_INTERIM_RZB', 'SOCIETE INTERIMAIRE', 1, 1, 0, 0,0, 1, @ORGANISATION_ID)

END
