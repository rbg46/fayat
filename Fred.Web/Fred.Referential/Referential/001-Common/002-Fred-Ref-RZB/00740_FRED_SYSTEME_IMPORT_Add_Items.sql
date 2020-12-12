-- =======================================================================================================================================
-- Author:		BENNAI Naoufal  27/11/2019
--
-- Description:
--          RG_3465_016 : Ajout des données PIXID_STATUT et PIXID_MOTIF dans FRED_SYSTEM_IMPORT 
--
-- =======================================================================================================================================

SET IDENTITY_INSERT [dbo].[FRED_SYSTEME_IMPORT] ON;

IF NOT EXISTS(SELECT * FROM [FRED_SYSTEME_IMPORT] WHERE CODE = 'PIXID_STATUT')
BEGIN 
    INSERT INTO [FRED_SYSTEME_IMPORT] ([SystemeImportId], [Code], [Libelle], [Description])
     VALUES (3, 'PIXID_STATUT', 'Statuts contrats PIXID', 'Transco PIXID => FRED statuts contrats Interimaires')
END;

IF NOT EXISTS(SELECT * FROM [FRED_SYSTEME_IMPORT] WHERE CODE = 'PIXID_MOTIF')
BEGIN 
    INSERT INTO [FRED_SYSTEME_IMPORT] ([SystemeImportId], [Code], [Libelle], [Description])
     VALUES (4, 'PIXID_MOTIF', 'Motifs contrats PIXID', 'Transco PIXID => FRED motifs contrats Interimaires')
END;

SET IDENTITY_INSERT [dbo].[FRED_SYSTEME_IMPORT] OFF;


