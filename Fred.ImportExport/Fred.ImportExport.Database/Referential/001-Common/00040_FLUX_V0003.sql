IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'PERSONNEL_FES')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'PERSONNEL_FES', N'Import du perspnnel FES', NULL, N'Import du personnel FES', 1, N'001,002,003,004,006,007,013,014,015,016,017,018,019,101,102,103', N'MO2', NULL)
END
ELSE
BEGIN
    UPDATE [importExport].[Flux] SET [SocieteCode] =  (N'001,002,003,004,006,007,013,014,015,016,017,018,019,101,102,103') WHERE Code = 'PERSONNEL_FES'
END
