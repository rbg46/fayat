-- =======================================================================================================================================
-- Author:		KASSASSE Abdessamad    04/09/2019
--
-- Description:
--      - Ajout lignes pour les codes societes 
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[SocieteCodeImportMaterielEnt] WHERE Code = '1000')
BEGIN
  INSERT into [importExport].[SocieteCodeImportMaterielEnt]
  (  [Code]
      ,[GroupCode]) 
      values ('1000',1)
END
IF NOT EXISTS (SELECT Code FROM [importExport].[SocieteCodeImportMaterielEnt] WHERE Code = '550')
BEGIN
  INSERT into [importExport].[SocieteCodeImportMaterielEnt]
  (  [Code]
      ,[GroupCode]) 
      values ('500',1)
END
IF NOT EXISTS (SELECT Code FROM [importExport].[SocieteCodeImportMaterielEnt] WHERE Code = '0001')
BEGIN
  INSERT into [importExport].[SocieteCodeImportMaterielEnt]
  (  [Code]
      ,[GroupCode]) 
      values ('0001',2)
END