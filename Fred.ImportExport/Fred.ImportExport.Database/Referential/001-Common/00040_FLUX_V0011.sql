-- =======================================================================================================================================
-- Author:		KASSASSE Abdessamad   16/10/2019
--
-- Description:
--      - Ajout du flux EXPORT_Materiel_FAYATTP pour l'export des materiels FAYATTP 
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'IMPORT_MATERIEL_FAYATTP')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES ('IMPORT_MATERIEL_FAYATTP', 'Import du matériel FayatTp Storm', NULL, 'Import du matériel FayatTp depuis Storm', 1, NULL, NULL, NULL)
END