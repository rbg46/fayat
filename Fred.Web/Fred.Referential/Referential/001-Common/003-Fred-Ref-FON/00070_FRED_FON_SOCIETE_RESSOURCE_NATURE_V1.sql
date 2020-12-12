-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- --------------------------------------------------
IF OBJECT_ID ( 'FredInjectRessourcesNature', 'P' ) IS NOT NULL   	
    DROP PROCEDURE FredInjectRessourcesNature;  	
GO  	
CREATE PROCEDURE FredInjectRessourcesNature   	
    @RessourcesCode nvarchar(20),   	
    @NatureCode nvarchar(50),	
	@SocieteCode nvarchar(20)
AS   	
	 DECLARE @RessourcesId int;  
	 DECLARE @NatureId int; 
	 DECLARE @SocieteId int; 
     DECLARE @GroupeId int; 
     
	 
	 --RECHERCHE DU GROUPE DE LA SOCIETE
	 SET @Groupeid = (SELECT groupeId FROM FRED_SOCIETE WHERE Code = @SocieteCode)

	 SET @SocieteId = (SELECT SocieteId FROM FRED_SOCIETE WHERE  Code = @SocieteCode)



	 SET @RessourcesId= (
	 
		SELECT [dbo].[FRED_RESSOURCE].[RessourceId]
		FROM [FRED_RESSOURCE], [FRED_SOUS_CHAPITRE], [FRED_CHAPITRE]
		WHERE [FRED_RESSOURCE].Code = @RessourcesCode
		AND [FRED_RESSOURCE].SousChapitreId = [FRED_SOUS_CHAPITRE].SousChapitreId
		AND [FRED_SOUS_CHAPITRE].ChapitreId = [FRED_CHAPITRE].ChapitreId
		AND [FRED_CHAPITRE].GroupeId = @Groupeid
		);	

	

	 SET @NatureId= (SELECT [dbo].[FRED_NATURE].[NatureId] FROM [FRED_NATURE] WHERE [FRED_NATURE].Code = @NatureCode AND FRED_NATURE.SocieteId = @SocieteId);
	 PRINT 'RESSOURCE';
	 PRINT @RessourcesId;
	 PRINT 'NAT';
	 PRINT @NatureId;
	 PRINT 'SOC';
	 PRINT @SocieteId;
	
	DECLARE @CheckUnicityCoupleSocieteRessourceNature int;

	SET @CheckUnicityCoupleSocieteRessourceNature = 
	(
		SELECT COUNT(ReferentielEtenduId) FROM [FRED_SOCIETE_RESSOURCE_NATURE]
		WHERE RessourceId = @RessourcesId
		AND NatureId = @NatureId
		AND SocieteId = @SocieteId
	)

	IF (@CheckUnicityCoupleSocieteRessourceNature = 0)
	BEGIN
	 INSERT INTO [dbo].[FRED_SOCIETE_RESSOURCE_NATURE] ([SocieteId],[RessourceId],[NatureId]) VALUES (@SocieteId,@RessourcesId,@NatureId);
	END
GO  	






EXECUTE FredInjectRessourcesNature '6001','6001', '500';
EXECUTE FredInjectRessourcesNature '6002','6002', '500';
EXECUTE FredInjectRessourcesNature '6003','6003', '500';
EXECUTE FredInjectRessourcesNature '6004','6004', '500';
EXECUTE FredInjectRessourcesNature '6010','6010', '500';
EXECUTE FredInjectRessourcesNature '6011','6011', '500';
EXECUTE FredInjectRessourcesNature '6012','6012', '500';
EXECUTE FredInjectRessourcesNature '6013','6013', '500';
EXECUTE FredInjectRessourcesNature '6014','6014', '500';
EXECUTE FredInjectRessourcesNature '6020','6020', '500';
EXECUTE FredInjectRessourcesNature '6021','6021', '500';
EXECUTE FredInjectRessourcesNature '6022','6022', '500';
EXECUTE FredInjectRessourcesNature '6030','6030', '500';
EXECUTE FredInjectRessourcesNature '6031','6031', '500';
EXECUTE FredInjectRessourcesNature '6032','6032', '500';
EXECUTE FredInjectRessourcesNature '6033','6033', '500';
EXECUTE FredInjectRessourcesNature '6034','6034', '500';
EXECUTE FredInjectRessourcesNature '6040','6040', '500';
EXECUTE FredInjectRessourcesNature '6041','6041', '500';
EXECUTE FredInjectRessourcesNature '6042','6042', '500';
EXECUTE FredInjectRessourcesNature '6043','6043', '500';
EXECUTE FredInjectRessourcesNature '6044','6044', '500';
EXECUTE FredInjectRessourcesNature '6045','6045', '500';
EXECUTE FredInjectRessourcesNature '6046','6046', '500';
EXECUTE FredInjectRessourcesNature '6050','6050', '500';
EXECUTE FredInjectRessourcesNature '6051','6051', '500';
EXECUTE FredInjectRessourcesNature '6052','6052', '500';
EXECUTE FredInjectRessourcesNature '6053','6053', '500';
EXECUTE FredInjectRessourcesNature '6054','6054', '500';
EXECUTE FredInjectRessourcesNature '6055','6055', '500';
EXECUTE FredInjectRessourcesNature '6056','6056', '500';
EXECUTE FredInjectRessourcesNature '6057','6057', '500';
EXECUTE FredInjectRessourcesNature '6060','6060', '500';
EXECUTE FredInjectRessourcesNature '6061','6061', '500';
EXECUTE FredInjectRessourcesNature '6062','6062', '500';
EXECUTE FredInjectRessourcesNature '6070','6070', '500';
EXECUTE FredInjectRessourcesNature '6071','6071', '500';
EXECUTE FredInjectRessourcesNature '6072','6072', '500';
EXECUTE FredInjectRessourcesNature '6073','6073', '500';
EXECUTE FredInjectRessourcesNature '6074','6074', '500';
EXECUTE FredInjectRessourcesNature '6075','6075', '500';
EXECUTE FredInjectRessourcesNature '6076','6076', '500';
EXECUTE FredInjectRessourcesNature '6077','6077', '500';
EXECUTE FredInjectRessourcesNature '6078','6078', '500';
EXECUTE FredInjectRessourcesNature '607A ','607A ', '500';
EXECUTE FredInjectRessourcesNature '607B ','607B ', '500';
EXECUTE FredInjectRessourcesNature '607c ','607c ', '500';
EXECUTE FredInjectRessourcesNature '607D ','607D ', '500';
EXECUTE FredInjectRessourcesNature '607E ','607E ', '500';
EXECUTE FredInjectRessourcesNature '607F ','607F ', '500';
EXECUTE FredInjectRessourcesNature '607G ','607G ', '500';
EXECUTE FredInjectRessourcesNature '607H ','607H ', '500';
EXECUTE FredInjectRessourcesNature '607I ','607I ', '500';
EXECUTE FredInjectRessourcesNature '6080','6080', '500';
EXECUTE FredInjectRessourcesNature '6081','6081', '500';
EXECUTE FredInjectRessourcesNature '6082','6082', '500';
EXECUTE FredInjectRessourcesNature '6090','6090', '500';
EXECUTE FredInjectRessourcesNature '6091','6091', '500';
EXECUTE FredInjectRessourcesNature '6092','6092', '500';
EXECUTE FredInjectRessourcesNature '6093','6093', '500';
EXECUTE FredInjectRessourcesNature '6094','6094', '500';
EXECUTE FredInjectRessourcesNature '6095','6095', '500';
EXECUTE FredInjectRessourcesNature '6096','6096', '500';
EXECUTE FredInjectRessourcesNature '6097','6097', '500';
EXECUTE FredInjectRessourcesNature '6098','6098', '500';
EXECUTE FredInjectRessourcesNature '6100','6100', '500';
EXECUTE FredInjectRessourcesNature '6101','6101', '500';
EXECUTE FredInjectRessourcesNature '6102','6102', '500';
EXECUTE FredInjectRessourcesNature '6103','6103', '500';
EXECUTE FredInjectRessourcesNature '6104','6104', '500';
EXECUTE FredInjectRessourcesNature '6110','6110', '500';
EXECUTE FredInjectRessourcesNature '6111','6111', '500';
EXECUTE FredInjectRessourcesNature '6112','6112', '500';
EXECUTE FredInjectRessourcesNature '6113','6113', '500';
EXECUTE FredInjectRessourcesNature '6114','6114', '500';
EXECUTE FredInjectRessourcesNature '6115','6115', '500';
EXECUTE FredInjectRessourcesNature '6120','6120', '500';
EXECUTE FredInjectRessourcesNature '6130','6130', '500';
EXECUTE FredInjectRessourcesNature '6131','6131', '500';
EXECUTE FredInjectRessourcesNature '6132','6132', '500';
EXECUTE FredInjectRessourcesNature '6133','6133', '500';
EXECUTE FredInjectRessourcesNature '6134','6134', '500';
EXECUTE FredInjectRessourcesNature '6135','6135', '500';
EXECUTE FredInjectRessourcesNature '6136','6136', '500';
EXECUTE FredInjectRessourcesNature '6137','6137', '500';
EXECUTE FredInjectRessourcesNature '6138','6138', '500';
EXECUTE FredInjectRessourcesNature '6140','6140', '500';
EXECUTE FredInjectRessourcesNature '6150','6150', '500';
EXECUTE FredInjectRessourcesNature '6151','6151', '500';
EXECUTE FredInjectRessourcesNature '6152','6152', '500';
EXECUTE FredInjectRessourcesNature '6153','6153', '500';
EXECUTE FredInjectRessourcesNature '6160','6160', '500';
EXECUTE FredInjectRessourcesNature '6161','6161', '500';
EXECUTE FredInjectRessourcesNature '6162','6162', '500';
EXECUTE FredInjectRessourcesNature '6163','6163', '500';
EXECUTE FredInjectRessourcesNature '6164','6164', '500';
EXECUTE FredInjectRessourcesNature '6170','6170', '500';
EXECUTE FredInjectRessourcesNature '6171','6171', '500';
EXECUTE FredInjectRessourcesNature '6172','6172', '500';
EXECUTE FredInjectRessourcesNature '6173','6173', '500';
EXECUTE FredInjectRessourcesNature '6174','6174', '500';
EXECUTE FredInjectRessourcesNature '6175','6175', '500';
EXECUTE FredInjectRessourcesNature '6176','6176', '500';
EXECUTE FredInjectRessourcesNature '6177','6177', '500';
EXECUTE FredInjectRessourcesNature '6178','6178', '500';
EXECUTE FredInjectRessourcesNature '6180','6180', '500';
EXECUTE FredInjectRessourcesNature '6181','6181', '500';
EXECUTE FredInjectRessourcesNature '6190','6190', '500';
EXECUTE FredInjectRessourcesNature '6191','6191', '500';
EXECUTE FredInjectRessourcesNature '6192','6192', '500';
EXECUTE FredInjectRessourcesNature '6800','6800', '500';



