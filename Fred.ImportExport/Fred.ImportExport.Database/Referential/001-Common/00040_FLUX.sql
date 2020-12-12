-- Concaténation de tous les fichiers FLUX.sql

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'CI_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'CI_RZB', N'Import des CI RAZEL-BEC', NULL, N'Import des CI RAZEL-BEC', 1, N'1000', NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'FOURNISSEUR_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'FOURNISSEUR_RZB', N'Import des fournisseurs RAZEL-BEC', NULL, N'Import des fournisseurs RAZEL-BEC', 1, N'1000', N'1', NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'PERSONNEL_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'PERSONNEL_RZB', N'Import du personnel RAZEL-BEC', NULL, N'Import du personnel RAZEL-BEC', 1, N'RZB', N'MO2', NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'VALIDATION_POINTAGE_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'VALIDATION_POINTAGE_RZB', N'Validation des lots de pointage personnel (Contrôle Chantier, Contrôle Vrac, Remontée Vrac)', NULL, N'Validation des lots de pointage personnel (Contrôle Chantier, Contrôle Vrac, Remontée Vrac)', 1, N'RZB', N'', NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'ETABLISSEMENT_COMPTABLE_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'ETABLISSEMENT_COMPTABLE_RZB', N'Import des établissements comptables RAZEL-BEC', NULL, N'Import des établissements comptables RAZEL-BEC', 1, N'1000', NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'ECRITURE_COMPTABLE_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'ECRITURE_COMPTABLE_RZB', N'Import des écritures comptables d Anael Fi vers FRED', NULL, N'Import des écritures comptables d Anael Fi vers FRED', 1, N'1000', NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'MATERIEL_STORM')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'MATERIEL_STORM', N'Import du matériel Storm', NULL, N'Import du matériel depuis Storm', 1, NULL, NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'KLM_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'KLM_RZB', N'Export des indemnités de déplacement', NULL, N'Export des indemnités de déplacement', 1, N'1000', NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'IMPORT_CI')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'IMPORT_CI', N'Import des CI', NULL, N'Import des CI', 1, NULL, NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'STAIR_RZB_INDICATEURS_SAFE')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'STAIR_RZB_INDICATEURS_SAFE', N'Import des indicateurs safetytab', NULL, N'Import des indicateurs safetytab', 1, NULL, NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'IMPORT_MATERIEL')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'IMPORT_MATERIEL', N'Import des materiels', NULL, N'Import des materiels', 1, NULL, NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'PERSONNEL_FES')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'PERSONNEL_FES', N'Import du perspnnel FES', NULL, N'Import du personnel FES', 1, N'001,002', N'MO2', NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'IMPORT_FOURNISSEUR')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'IMPORT_FOURNISSEUR', N'Import des fournisseurs', NULL, N'Import des fournisseurs', 1, NULL, NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'RECEPTION_INTERIMAIRE_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'RECEPTION_INTERIMAIRE_RZB', N'Exporter des réceptions intérimaire', NULL, N'Export des réceptions intérimaire', 1, 'RZB', NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'PERSONNEL_FTP')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'PERSONNEL_FTP', N'Import du perspnnel FTP', NULL, N'Import du personnel FTP', 1, N'FTP', N'MO2', NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'STAIR_RZB_EXPORT_PERSONNEL')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'STAIR_RZB_EXPORT_PERSONNEL', N'Export du personnel', NULL, N'Export du personnel RZB STAIR', 1, NULL, NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'STAIR_RZB_EXPORT_CI')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'STAIR_RZB_EXPORT_CI', N'STAIR - Export des CI GRZB', NULL, N'STAIR - Export des CI GRZB', 1, NULL, NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'PERSONNEL_GRZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'PERSONNEL_GRZB', N'Import du personnel Groupe RZB', NULL, N'Import du personnel Groupe RZB (hors société RZB)', 1, N'GEO,800,LHE,620,320', N'MO2', NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'CI_GRZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'CI_GRZB', N'Import des CI Groupe RZB (hors société RZB)', NULL, N'Import des CI Groupe RZB (hors société RZB)', 1, N'1600,800,200,6200,300', NULL, NULL)
END

IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'VALIDATION_POINTAGE_FES')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) VALUES (N'VALIDATION_POINTAGE_FES', N'Validation des lots de pointage personnel (Contrôle Chantier, Contrôle Vrac, Remontée Vrac)', NULL, N'Validation des lots de pointage personnel (Contrôle Chantier, Contrôle Vrac, Remontée Vrac)', 1, N'', N'', NULL)
END