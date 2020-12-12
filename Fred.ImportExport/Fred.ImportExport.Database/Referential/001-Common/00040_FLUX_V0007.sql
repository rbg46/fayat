-- =======================================================================================================================================
-- Author:		Benoit Puyo    17/06/2019
--
-- Description:
--      - Ajout du flux d'import en masse des journaux comptables
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'JOURNAUX_COMPTABLE_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'JOURNAUX_COMPTABLE_RZB', N'Import des Journaux Comptables RAZEL-BEC', NULL, N'Import des Journaux Comptables RAZEL-BEC', 1, '1000', NULL, NULL)
END