-- =======================================================================================================================================
-- Author:		JS Moreau  04/02/2019
--
-- Description:
--      - Ajout de la clef Budget.ValidationRecette pour gérer la saisie de recette obligatoire pour la validation d'un budget
--
-- =======================================================================================================================================

IF NOT EXISTS(SELECT * FROM [FRED_PARAM_KEY] WHERE Libelle ='Budget.SaisieRecette')
BEGIN
	INSERT INTO [dbo].[FRED_PARAM_KEY] ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId])
	VALUES ('Budget.SaisieRecette' ,'la Clé pour identifier si la saisie de la recette est obligatoire pour la validation du budget', GETDATE(), GETDATE(), 1, 1);
END
