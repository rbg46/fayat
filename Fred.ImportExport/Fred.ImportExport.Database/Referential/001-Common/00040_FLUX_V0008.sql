-- =======================================================================================================================================
-- Author:		Yoann Collet    12/08/2019
--
-- Description:
--      - Ajout du flux VALIDATION_POINTAGE_FTP
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'VALIDATION_POINTAGE_FTP')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution])VALUES (N'VALIDATION_POINTAGE_FTP', N'Validation des lots de pointage personnel Fayat TP', NULL, N'Validation des lots de pointage personnel Fayat TP (Contrôle Vrac, Remontée Vrac)', 1, N'', N'', NULL)
END
