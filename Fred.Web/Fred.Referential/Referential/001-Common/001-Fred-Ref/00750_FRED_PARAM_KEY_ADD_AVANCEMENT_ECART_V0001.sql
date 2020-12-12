-- =======================================================================================================================================
-- Author:		JS Moreau  04/02/2019
--
-- Description:
--      - Ajout de la clef Budget.AvancementEcart pour gérer l'activation de la saisie des ecarts avancement budget
--
-- =======================================================================================================================================

IF NOT EXISTS(SELECT * FROM [FRED_PARAM_KEY] WHERE Libelle ='Budget.AvancementEcart')
BEGIN
	INSERT INTO [dbo].[FRED_PARAM_KEY] ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId])
	VALUES ('Budget.AvancementEcart' ,'la Clé pour identifier l''activation de la saisie des ecarts avancement budget', GETDATE(), GETDATE(), 1, 1);
END
