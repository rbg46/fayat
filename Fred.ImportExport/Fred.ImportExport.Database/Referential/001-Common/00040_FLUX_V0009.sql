-- =======================================================================================================================================
-- Author:		Yoann Collet    10/09/2019
--
-- Description:
--      - Ajout du flux EXPORT_PERSONNEL_FES pour l'export des personnels FES vers TIBCO
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'EXPORT_PERSONNEL_FES')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution])VALUES (N'EXPORT_PERSONNEL_FES', N'Export du personnel du groupe FES vers TIBCO', NULL, N'Export du personnel du groupe FES vers TIBCO', 1, N'', N'', NULL)
END
