-- =======================================================================================================================================
-- Author:		Yoann Collet  11/02/2019
--
-- Description:
--      - Mise à jour des libelles des flux personnel FTP et FES
--
-- =======================================================================================================================================



UPDATE  [importExport].[Flux] Set Libelle = 'Import du personnel FTP' where Code ='PERSONNEL_FTP';
UPDATE  [importExport].[Flux] Set Libelle = 'Import du personnel FES' where Code = 'PERSONNEL_FES';

