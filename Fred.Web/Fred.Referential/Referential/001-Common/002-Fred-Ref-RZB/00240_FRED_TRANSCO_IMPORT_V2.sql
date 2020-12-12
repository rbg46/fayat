-- Correction de l'inversion des champs [CodeInterne] et [CodeExterne] en V1

-- On récupère l'identifant du "système import" et de la société RAZEL-BEC
DECLARE @SystemeImportId INTEGER;
DECLARE @SocieteId INTEGER;

SELECT @SystemeImportId = [SystemeImportId] FROM [FRED_SYSTEME_IMPORT] WHERE [Code] = 'STORM_CA'
SELECT @SocieteId = [SocieteId]  FROM [dbo].[FRED_SOCIETE] WHERE [Code] = 'RB'

-- On insert des "transco import"
DELETE FROM [dbo].[FRED_TRANSCO_IMPORT]

INSERT INTO [dbo].[FRED_TRANSCO_IMPORT] ([CodeExterne], [CodeInterne]  ,[SocieteId] ,[SystemeImportId])
       VALUES ('221170' ,'CARBU-04' ,@SocieteId ,@SystemeImportId)
INSERT INTO [dbo].[FRED_TRANSCO_IMPORT] ([CodeExterne], [CodeInterne]  ,[SocieteId] ,[SystemeImportId])
       VALUES ('221730' ,'REPAR-02' ,@SocieteId ,@SystemeImportId)
INSERT INTO [dbo].[FRED_TRANSCO_IMPORT] ([CodeExterne], [CodeInterne]  ,[SocieteId] ,[SystemeImportId])
       VALUES ('221750' ,'REPAR-04' ,@SocieteId ,@SystemeImportId)
INSERT INTO [dbo].[FRED_TRANSCO_IMPORT] ([CodeExterne], [CodeInterne]  ,[SocieteId] ,[SystemeImportId])
       VALUES ('624150' ,'FRET-01' ,@SocieteId ,@SystemeImportId)
INSERT INTO [dbo].[FRED_TRANSCO_IMPORT] ([CodeExterne], [CodeInterne]  ,[SocieteId] ,[SystemeImportId])
       VALUES ('611840' ,'TAXE-05' ,@SocieteId ,@SystemeImportId)
INSERT INTO [dbo].[FRED_TRANSCO_IMPORT] ([CodeExterne], [CodeInterne]  ,[SocieteId] ,[SystemeImportId])
       VALUES ('641000' ,'ASSU-01' ,@SocieteId ,@SystemeImportId)

