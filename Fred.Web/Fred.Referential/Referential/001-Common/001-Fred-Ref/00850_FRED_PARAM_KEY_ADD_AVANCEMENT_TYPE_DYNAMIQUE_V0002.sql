-- =======================================================================================================================================
-- Author:		Charles COQUE  04/02/2019
--
-- Description:
--      - Ajout de la clé Budget.TypeAvancementDynamique 
--
-- =======================================================================================================================================

IF NOT EXISTS(SELECT * FROM [FRED_PARAM_KEY] WHERE Libelle ='Budget.TypeAvancementDynamique')
BEGIN
	INSERT INTO [dbo].[FRED_PARAM_KEY] ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId])
	VALUES ('Budget.TypeAvancementDynamique' ,'Clé indiquant si oui ou non le type d''avancement est défini comme dynamique pour l''organisation', GETDATE(), GETDATE(), 1, 1);
END
