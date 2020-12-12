-- =======================================================================================================================================
-- Description:
--      - Ajout du flux PERSONNEL_FON pour l'import personnel fondation 
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'PERSONNEL_FON')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'PERSONNEL_FON', N'Import du personnel FON', NULL, N'Import du personnel FON', 1, N'500,700', N'MO2', NULL)
END