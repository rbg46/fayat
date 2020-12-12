CREATE PROCEDURE #UpdateEtablissementPaie
	@etabPaieCode nvarchar(40),
	@etabComptableCode nvarchar(40),
	@societeCode nvarchar(40)
AS
BEGIN
	DECLARE @societeId int
	SET @societeId = (SELECT s.SocieteId
		FROM FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId
		WHERE s.Code=@societeCode and g.Code='GFES')

	UPDATE FRED_ETABLISSEMENT_PAIE SET EtablissementComptableId =
		(SELECT ec.EtablissementComptableId
		FROM FRED_ETABLISSEMENT_COMPTABLE ec
		WHERE ec.Code=@etabComptableCode and ec.SocieteId=@societeId)
	WHERE Code=@etabPaieCode and SocieteId = @societeId
END
GO

EXEC #UpdateEtablissementPaie '09', '0075', 'E001';
EXEC #UpdateEtablissementPaie '10', '0010', 'E001';
EXEC #UpdateEtablissementPaie '11', '0010', 'E001';
EXEC #UpdateEtablissementPaie '14', '0010', 'E001';
EXEC #UpdateEtablissementPaie '15', '0010', 'E001';
EXEC #UpdateEtablissementPaie '16', '0010', 'E001';
EXEC #UpdateEtablissementPaie '17', '0010', 'E001';
EXEC #UpdateEtablissementPaie '18', '0050', 'E001';
EXEC #UpdateEtablissementPaie '20', '0020', 'E001';
EXEC #UpdateEtablissementPaie '21', '0020', 'E001';
EXEC #UpdateEtablissementPaie '22', '0020', 'E001';
EXEC #UpdateEtablissementPaie '23', '0020', 'E001';
EXEC #UpdateEtablissementPaie '24', '0020', 'E001';
EXEC #UpdateEtablissementPaie '25', '0020', 'E001';
EXEC #UpdateEtablissementPaie '27', '0020', 'E001';
EXEC #UpdateEtablissementPaie '28', '0020', 'E001';
EXEC #UpdateEtablissementPaie '30', '0030', 'E001';
EXEC #UpdateEtablissementPaie '40', '0040', 'E001';
EXEC #UpdateEtablissementPaie '42', '0040', 'E001';
EXEC #UpdateEtablissementPaie '69', '0010', 'E001';
EXEC #UpdateEtablissementPaie '2A', '0020', 'E001';
EXEC #UpdateEtablissementPaie '2B', '0020', 'E001';
EXEC #UpdateEtablissementPaie '10', '0001', 'E002';
EXEC #UpdateEtablissementPaie '11', '0001', 'E002';
EXEC #UpdateEtablissementPaie '12', '0001', 'E002';
EXEC #UpdateEtablissementPaie '13', '0001', 'E002';
EXEC #UpdateEtablissementPaie '20', '0001', 'E002';
EXEC #UpdateEtablissementPaie '22', '0001', 'E002';
EXEC #UpdateEtablissementPaie '30', '0001', 'E002';
EXEC #UpdateEtablissementPaie '40', '0001', 'E002';
EXEC #UpdateEtablissementPaie '50', '0001', 'E002';
EXEC #UpdateEtablissementPaie '12', '0010', 'E003';
EXEC #UpdateEtablissementPaie '13', '0010', 'E003';
EXEC #UpdateEtablissementPaie '20', '0010', 'E003';
EXEC #UpdateEtablissementPaie '12', '0010', 'E006';
EXEC #UpdateEtablissementPaie '30', '0010', 'E006';
EXEC #UpdateEtablissementPaie '10', '0041', 'E010';
EXEC #UpdateEtablissementPaie '11', '0041', 'E010';
EXEC #UpdateEtablissementPaie '20', '0041', 'E010';
EXEC #UpdateEtablissementPaie '30', '0041', 'E010';
EXEC #UpdateEtablissementPaie '40', '0041', 'E010';
EXEC #UpdateEtablissementPaie '50', '0041', 'E010';
EXEC #UpdateEtablissementPaie '60', '0041', 'E010';
EXEC #UpdateEtablissementPaie '70', '0041', 'E010';
EXEC #UpdateEtablissementPaie '80', '0041', 'E010';
EXEC #UpdateEtablissementPaie '10', '0042', 'E011';
EXEC #UpdateEtablissementPaie '20', '0042', 'E011';
EXEC #UpdateEtablissementPaie '10', '0043', 'E012';
EXEC #UpdateEtablissementPaie '20', '0043', 'E012';
EXEC #UpdateEtablissementPaie '11', '0011', 'E013';
EXEC #UpdateEtablissementPaie '20', '0020', 'E014';
EXEC #UpdateEtablissementPaie '41', '0060', 'E015';
EXEC #UpdateEtablissementPaie '42', '0060', 'E015';
EXEC #UpdateEtablissementPaie '43', '0060', 'E015';
EXEC #UpdateEtablissementPaie '44', '0060', 'E015';
EXEC #UpdateEtablissementPaie '14', '0013', 'E016';
EXEC #UpdateEtablissementPaie '97', '0974', 'E017';
EXEC #UpdateEtablissementPaie '98', '0974', 'E017';
EXEC #UpdateEtablissementPaie '14', '0014', 'E018';
EXEC #UpdateEtablissementPaie '27', '0014', 'E018';
EXEC #UpdateEtablissementPaie '56', '0014', 'E018';
EXEC #UpdateEtablissementPaie '27', '0021', 'E019';
EXEC #UpdateEtablissementPaie '23', '0021', 'E019';

DROP PROCEDURE #UpdateEtablissementPaie