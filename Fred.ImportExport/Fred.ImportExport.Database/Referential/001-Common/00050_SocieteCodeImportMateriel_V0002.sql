-- =======================================================================================================================================
-- Author:		KASSASSE Abdessamad   16/10/2019
--
-- Description:
--      - Ajout/UPDATE lignes pour les codes societes 
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[SocieteCodeImportMaterielEnt] WHERE Code = '0143')
BEGIN
  INSERT into [importExport].[SocieteCodeImportMaterielEnt]
  (  [Code]
      ,[GroupCode]) 
      values ('0143',2)
END

UPDATE  [importExport].[SocieteCodeImportMaterielEnt] set Code = '550' where Code = '500' 
