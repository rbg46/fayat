/* Mise en commentaire BPO : plus utilisé

----SUPPRESSION DU MAPPING REL NATURE SOCIETE
DELETE FROM FRED_SOCIETE_RESSOURCE_NATURE WHERE RessourceId IN (
SELECT  FRED_RESSOURCE.RessourceId FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE, FRED_CHAPITRE
 WHERE FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
 AND	FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId AND 
 FRED_CHAPITRE.GroupeId = (SELECT GroupeId FROM FRED_SOCIETE where Code = '500')
 AND (FRED_SOUS_CHAPITRE.Code = '6000'))

----SUPPRESSION DES RESSOURCES LIEES AU CHAPITRE 6000
DELETE  FRED_RESSOURCE WHERE SousChapitreId IN  (SELECT DISTINCT FRED_SOUS_CHAPITRE.SousChapitreId FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE, FRED_CHAPITRE
 WHERE FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
 AND	FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId AND 
 FRED_CHAPITRE.GroupeId = (SELECT GroupeId FROM FRED_SOCIETE where Code = '500')
 AND (FRED_SOUS_CHAPITRE.Code = '6000'))





-- PS D'INJECTION RESSOURCES
IF OBJECT_ID ( 'Fred_ToolBox_InsertRessources', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_InsertRessources;  
GO  
CREATE PROCEDURE Fred_ToolBox_InsertRessources   
    @GroupeCode nvarchar(50),
	@SousChapitreCode nvarchar(50),
	@Code nvarchar(50),   
    @Libelle nvarchar(50) 
	

AS   



	DECLARE @GroupeId Int;
	DECLARE @ControleExistanceRessourceCodeDansGroupe Int;
	

	SET @GroupeId = (SELECT groupeId FROM FRED_GROUPE where Code = @GroupeCode);
	PRINT @GroupeId
	SET @ControleExistanceRessourceCodeDansGroupe = 
		(SELECT COUNT(RessourceId) FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE, FRED_CHAPITRE 
		WHERE FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId 
		AND FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId 
		AND FRED_CHAPITRE.GroupeId = @GroupeId AND FRED_RESSOURCE.code = @Code);

		IF(@ControleExistanceRessourceCodeDansGroupe = 0 )
		BEGIN
		DECLARE @SousChapitreId int;
		SET @SousChapitreId = 
			(SELECT FRED_SOUS_CHAPITRE.SousChapitreId 
			FROM FRED_SOUS_CHAPITRE, FRED_CHAPITRE
			WHERE FRED_SOUS_CHAPITRE.ChapitreId =  FRED_CHAPITRE.ChapitreId
			AND FRED_CHAPITRE.GroupeId = @GroupeId
			AND FRED_SOUS_CHAPITRE.Code = @SousChapitreCode)
			


			
				-- Récupération du Chapitre ID
				INSERT INTO FRED_RESSOURCE ([Code], [Libelle], [SousChapitreId], [Active], [DateCreation],[AuteurCreationId])
				VALUES (@Code, @Libelle,@SousChapitreId,1,GETDATE(),1 )
		END

GO 


EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600100',' PERSONNEL STE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600101','QSE'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600102','ADMINISTRATIF'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600103','MECANICIEN'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600104','MATERIEL'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600105','AIDE OUVRIER'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600106','CONDUITE DE TRAVAUX'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600107','APPRENTI'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600108','INGENIEUR TRAVAUX'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600109','BUREAU ETUDES'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600110','CHEF DE CHANTIER'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600111','OUVRIER'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600112','CHARGE D''AFFAIRES'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600113','EXPORT'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600114','CHEF D''EQUIPE'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600115','CONTREMAITRE'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600116','DIRECTION'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600117','COMMERCIAL'
EXEC Fred_ToolBox_InsertRessources  'FON', '6000','600118','RESPONSABLE SECTEUR'


---- PROCEDURE INJECTION SOCIETE
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
	 
     

	 SET @RessourcesId= (SELECT FRED_RESSOURCE.RessourceId FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE, FRED_CHAPITRE
		 WHERE FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
		 AND	FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId AND 
		 FRED_CHAPITRE.GroupeId = (SELECT GroupeId FROM FRED_SOCIETE where Code = @SocieteCode)
		 AND (FRED_RESSOURCE.Code = @RessourcesCode))


	 SET @NatureId= (SELECT [dbo].[FRED_NATURE].[NatureId] FROM [FRED_NATURE] WHERE [FRED_NATURE].Code = @NatureCode AND SocieteId = (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode));
	 
	 SET @SocieteId = (SELECT [dbo].[FRED_SOCIETE].[SocieteId] FROM [dbo].[FRED_SOCIETE] WHERE [dbo].[FRED_SOCIETE].[Code] = @SocieteCode);
	 
	 
	 --PRINT @RessourcesId;
	 --PRINT @NatureId;
	 --PRINT @SocieteId;
	
	 --INSERT INTO [dbo].[FRED_SOCIETE_RESSOURCE_NATURE] ([SocieteId],[RessourceId],[NatureId]) VALUES (@SocieteId,@RessourcesId,@NatureId);
GO  	



EXECUTE FredInjectRessourcesNature '600100','6001', '500';
EXECUTE FredInjectRessourcesNature '600101','6001', '500';
EXECUTE FredInjectRessourcesNature '600102','6001', '500';
EXECUTE FredInjectRessourcesNature '600103','6001', '500';
EXECUTE FredInjectRessourcesNature '600104','6001', '500';
EXECUTE FredInjectRessourcesNature '600105','6001', '500';
EXECUTE FredInjectRessourcesNature '600106','6001', '500';
EXECUTE FredInjectRessourcesNature '600107','6001', '500';
EXECUTE FredInjectRessourcesNature '600108','6001', '500';
EXECUTE FredInjectRessourcesNature '600109','6001', '500';
EXECUTE FredInjectRessourcesNature '600110','6001', '500';
EXECUTE FredInjectRessourcesNature '600111','6001', '500';
EXECUTE FredInjectRessourcesNature '600112','6001', '500';
EXECUTE FredInjectRessourcesNature '600113','6001', '500';
EXECUTE FredInjectRessourcesNature '600114','6001', '500';
EXECUTE FredInjectRessourcesNature '600115','6001', '500';
EXECUTE FredInjectRessourcesNature '600116','6001', '500';
EXECUTE FredInjectRessourcesNature '600117','6001', '500';
EXECUTE FredInjectRessourcesNature '600118','6001', '500';

EXECUTE FredInjectRessourcesNature '600100','6001', '700';
EXECUTE FredInjectRessourcesNature '600101','6001', '700';
EXECUTE FredInjectRessourcesNature '600102','6001', '700';
EXECUTE FredInjectRessourcesNature '600103','6001', '700';
EXECUTE FredInjectRessourcesNature '600104','6001', '700';
EXECUTE FredInjectRessourcesNature '600105','6001', '700';
EXECUTE FredInjectRessourcesNature '600106','6001', '700';
EXECUTE FredInjectRessourcesNature '600107','6001', '700';
EXECUTE FredInjectRessourcesNature '600108','6001', '700';
EXECUTE FredInjectRessourcesNature '600109','6001', '700';
EXECUTE FredInjectRessourcesNature '600110','6001', '700';
EXECUTE FredInjectRessourcesNature '600111','6001', '700';
EXECUTE FredInjectRessourcesNature '600112','6001', '700';
EXECUTE FredInjectRessourcesNature '600113','6001', '700';
EXECUTE FredInjectRessourcesNature '600114','6001', '700';
EXECUTE FredInjectRessourcesNature '600115','6001', '700';
EXECUTE FredInjectRessourcesNature '600116','6001', '700';
EXECUTE FredInjectRessourcesNature '600117','6001', '700';
EXECUTE FredInjectRessourcesNature '600118','6001', '700';
EXECUTE FredInjectRessourcesNature '6002','6002', '700';
EXECUTE FredInjectRessourcesNature '6003','6003', '700';
EXECUTE FredInjectRessourcesNature '6004','6004', '700';
EXECUTE FredInjectRessourcesNature '6010','6010', '700';
EXECUTE FredInjectRessourcesNature '6011','6011', '700';
EXECUTE FredInjectRessourcesNature '6012','6012', '700';
EXECUTE FredInjectRessourcesNature '6013','6013', '700';
EXECUTE FredInjectRessourcesNature '6014','6014', '700';
EXECUTE FredInjectRessourcesNature '6020','6020', '700';
EXECUTE FredInjectRessourcesNature '6021','6021', '700';
EXECUTE FredInjectRessourcesNature '6022','6022', '700';
EXECUTE FredInjectRessourcesNature '6030','6030', '700';
EXECUTE FredInjectRessourcesNature '6031','6031', '700';
EXECUTE FredInjectRessourcesNature '6032','6032', '700';
EXECUTE FredInjectRessourcesNature '6033','6033', '700';
EXECUTE FredInjectRessourcesNature '6034','6034', '700';
EXECUTE FredInjectRessourcesNature '6040','6040', '700';
EXECUTE FredInjectRessourcesNature '6041','6041', '700';
EXECUTE FredInjectRessourcesNature '6042','6042', '700';
EXECUTE FredInjectRessourcesNature '6043','6043', '700';
EXECUTE FredInjectRessourcesNature '6044','6044', '700';
EXECUTE FredInjectRessourcesNature '6045','6045', '700';
EXECUTE FredInjectRessourcesNature '6046','6046', '700';
EXECUTE FredInjectRessourcesNature '6050','6050', '700';
EXECUTE FredInjectRessourcesNature '6051','6051', '700';
EXECUTE FredInjectRessourcesNature '6052','6052', '700';
EXECUTE FredInjectRessourcesNature '6053','6053', '700';
EXECUTE FredInjectRessourcesNature '6054','6054', '700';
EXECUTE FredInjectRessourcesNature '6055','6055', '700';
EXECUTE FredInjectRessourcesNature '6056','6056', '700';
EXECUTE FredInjectRessourcesNature '6057','6057', '700';
EXECUTE FredInjectRessourcesNature '6060','6060', '700';
EXECUTE FredInjectRessourcesNature '6061','6061', '700';
EXECUTE FredInjectRessourcesNature '6062','6062', '700';
EXECUTE FredInjectRessourcesNature '6070','6070', '700';
EXECUTE FredInjectRessourcesNature '6071','6071', '700';
EXECUTE FredInjectRessourcesNature '6072','6072', '700';
EXECUTE FredInjectRessourcesNature '6073','6073', '700';
EXECUTE FredInjectRessourcesNature '6074','6074', '700';
EXECUTE FredInjectRessourcesNature '6075','6075', '700';
EXECUTE FredInjectRessourcesNature '6076','6076', '700';
EXECUTE FredInjectRessourcesNature '6077','6077', '700';
EXECUTE FredInjectRessourcesNature '6078','6078', '700';
EXECUTE FredInjectRessourcesNature ' 607A ',' 607A', '700';
EXECUTE FredInjectRessourcesNature ' 607B ',' 607B', '700';
EXECUTE FredInjectRessourcesNature ' 607c ',' 607c', '700';
EXECUTE FredInjectRessourcesNature ' 607D ',' 607D', '700';
EXECUTE FredInjectRessourcesNature ' 607E ',' 607E', '700';
EXECUTE FredInjectRessourcesNature ' 607F ',' 607F', '700';
EXECUTE FredInjectRessourcesNature ' 607G ',' 607G', '700';
EXECUTE FredInjectRessourcesNature ' 607H ',' 607H', '700';
EXECUTE FredInjectRessourcesNature ' 607I ',' 607I', '700';
EXECUTE FredInjectRessourcesNature '6080','6080', '700';
EXECUTE FredInjectRessourcesNature '6081','6081', '700';
EXECUTE FredInjectRessourcesNature '6082','6082', '700';
EXECUTE FredInjectRessourcesNature '6090','6090', '700';
EXECUTE FredInjectRessourcesNature '6091','6091', '700';
EXECUTE FredInjectRessourcesNature '6092','6092', '700';
EXECUTE FredInjectRessourcesNature '6093','6093', '700';
EXECUTE FredInjectRessourcesNature '6094','6094', '700';
EXECUTE FredInjectRessourcesNature '6095','6095', '700';
EXECUTE FredInjectRessourcesNature '6096','6096', '700';
EXECUTE FredInjectRessourcesNature '6097','6097', '700';
EXECUTE FredInjectRessourcesNature '6098','6098', '700';
EXECUTE FredInjectRessourcesNature '6100','6100', '700';
EXECUTE FredInjectRessourcesNature '6101','6101', '700';
EXECUTE FredInjectRessourcesNature '6102','6102', '700';
EXECUTE FredInjectRessourcesNature '6103','6103', '700';
EXECUTE FredInjectRessourcesNature '6104','6104', '700';
EXECUTE FredInjectRessourcesNature '6110','6110', '700';
EXECUTE FredInjectRessourcesNature '6111','6111', '700';
EXECUTE FredInjectRessourcesNature '6112','6112', '700';
EXECUTE FredInjectRessourcesNature '6113','6113', '700';
EXECUTE FredInjectRessourcesNature '6114','6114', '700';
EXECUTE FredInjectRessourcesNature '6115','6115', '700';
EXECUTE FredInjectRessourcesNature '6120','6120', '700';
EXECUTE FredInjectRessourcesNature '6130','6130', '700';
EXECUTE FredInjectRessourcesNature '6131','6131', '700';
EXECUTE FredInjectRessourcesNature '6132','6132', '700';
EXECUTE FredInjectRessourcesNature '6133','6133', '700';
EXECUTE FredInjectRessourcesNature '6134','6134', '700';
EXECUTE FredInjectRessourcesNature '6135','6135', '700';
EXECUTE FredInjectRessourcesNature '6136','6136', '700';
EXECUTE FredInjectRessourcesNature '6137','6137', '700';
EXECUTE FredInjectRessourcesNature '6138','6138', '700';
EXECUTE FredInjectRessourcesNature '6140','6140', '700';
EXECUTE FredInjectRessourcesNature '6150','6150', '700';
EXECUTE FredInjectRessourcesNature '6151','6151', '700';
EXECUTE FredInjectRessourcesNature '6152','6152', '700';
EXECUTE FredInjectRessourcesNature '6153','6153', '700';
EXECUTE FredInjectRessourcesNature '6160','6160', '700';
EXECUTE FredInjectRessourcesNature '6161','6161', '700';
EXECUTE FredInjectRessourcesNature '6162','6162', '700';
EXECUTE FredInjectRessourcesNature '6163','6163', '700';
EXECUTE FredInjectRessourcesNature '6164','6164', '700';
EXECUTE FredInjectRessourcesNature '6170','6170', '700';
EXECUTE FredInjectRessourcesNature '6171','6171', '700';
EXECUTE FredInjectRessourcesNature '6172','6172', '700';
EXECUTE FredInjectRessourcesNature '6173','6173', '700';
EXECUTE FredInjectRessourcesNature '6174','6174', '700';
EXECUTE FredInjectRessourcesNature '6175','6175', '700';
EXECUTE FredInjectRessourcesNature '6176','6176', '700';
EXECUTE FredInjectRessourcesNature '6177','6177', '700';
EXECUTE FredInjectRessourcesNature '6178','6178', '700';
EXECUTE FredInjectRessourcesNature '6180','6180', '700';
EXECUTE FredInjectRessourcesNature '6181','6181', '700';
EXECUTE FredInjectRessourcesNature '6190','6190', '700';
EXECUTE FredInjectRessourcesNature '6191','6191', '700';
EXECUTE FredInjectRessourcesNature '6192','6192', '700';
EXECUTE FredInjectRessourcesNature '6800','6800', '700';


*/