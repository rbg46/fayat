-- =======================================================================================================================================
-- Author:		Yoann Collet    16/05/2019
--
-- Description:
--      - Ajout du flux d'export réception matériel externe
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'RECEPTION_MATERIEL_EXTERNE_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'RECEPTION_MATERIEL_EXTERNE_RZB', N'Exporter des réceptions de matériels externe', NULL, N'Export des réceptions de matériels externe', 1, '1000', NULL, NULL)
END
