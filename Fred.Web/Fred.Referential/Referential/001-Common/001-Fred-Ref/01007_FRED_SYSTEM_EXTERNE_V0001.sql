DECLARE @FtpSocieteId INT = (SELECT SocieteId FROM FRED_SOCIETE WHERE Code = '0001');
DECLARE @SomapaSocieteId INT = (SELECT SocieteId FROM FRED_SOCIETE WHERE Code = '0143');
DECLARE @SystemeExterneTypeId INT = (SELECT SystemeExterneTypeId FROM FRED_SYSTEME_EXTERNE_TYPE WHERE Libelle = 'Commandes');
DECLARE @SystemeImportId INT = (SELECT SystemeImportId FROM FRED_SYSTEME_IMPORT WHERE Code = 'STORM_ARTICLE');

IF(@FtpSocieteId <> 0 AND NOT EXISTS(SELECT 1 FROM FRED_SYSTEME_EXTERNE WHERE [SocieteId] = @FtpSocieteId AND SystemeExterneTypeId = @SystemeExterneTypeId))
BEGIN
	INSERT INTO [dbo].[FRED_SYSTEME_EXTERNE] 
	([Code],
	[Libelle],
	[Description],
	[SocieteId],
	[SystemeExterneTypeId],
	[SystemeImportId]
	)
	VALUES(
	'STORM_COMMANDE_FTP',
	'Commande STORM',
	'Import des commandes STORM pour la société FAYAT TP',
	@FtpSocieteId,
	@SystemeExterneTypeId,
	@SystemeImportId
	);
END	

IF(@SomapaSocieteId <> 0 AND NOT EXISTS(SELECT 1 FROM FRED_SYSTEME_EXTERNE WHERE [SocieteId] = @SomapaSocieteId AND SystemeExterneTypeId = @SystemeExterneTypeId))
BEGIN
	INSERT INTO [dbo].[FRED_SYSTEME_EXTERNE] 
	([Code],
	[Libelle],
	[Description],
	[SocieteId],
	[SystemeExterneTypeId],
	[SystemeImportId]
	)
	VALUES
	('STORM_COMMANDE_SOM',
	'Commande STORM',
	'Import des commandes STORM pour la société SOMOPA',
	@SomapaSocieteId,
	@SystemeExterneTypeId,
	@SystemeImportId
	);
END