SET IDENTITY_INSERT [dbo].[FRED_SYSTEME_IMPORT] ON;

INSERT INTO [dbo].[FRED_SYSTEME_IMPORT] ([SystemeImportId], [Code],[Libelle],[Description]) 
       VALUES(2, 'STORM_ARTICLE','Articles STORM','Transco code article STORM => code ressource FRED')

SET IDENTITY_INSERT [dbo].[FRED_SYSTEME_IMPORT] OFF;