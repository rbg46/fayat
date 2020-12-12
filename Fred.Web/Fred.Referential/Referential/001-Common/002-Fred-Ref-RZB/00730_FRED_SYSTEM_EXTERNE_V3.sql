SET IDENTITY_INSERT [dbo].[FRED_SYSTEME_EXTERNE] ON;

IF NOT EXISTS (select * from [dbo].[FRED_SYSTEME_EXTERNE] where SystemeExterneId = 1)
BEGIN
INSERT INTO [dbo].[FRED_SYSTEME_EXTERNE] ([SystemeExterneId], [Code],[Libelle],[Description],[SocieteId],[SystemeExterneTypeId],[SystemeImportId])
VALUES
	(1, 'STORM_FACTURATION_RZB', 'Facturation STORM', 'Import des articles en litiges et coûts additionnels facturés pour la société Razel-Bec', 1, 1, 1)
END

IF NOT EXISTS (select * from [dbo].[FRED_SYSTEME_EXTERNE] where SystemeExterneId = 2)
BEGIN
INSERT INTO [dbo].[FRED_SYSTEME_EXTERNE] ([SystemeExterneId], [Code],[Libelle],[Description],[SocieteId],[SystemeExterneTypeId],[SystemeImportId])
VALUES
	(2, 'STORM_COMMANDE_RZB', 'Commandes STORM', 'Import des commandes STORM pour la société Razel-Bec', 1, 2, 2)
END
SET IDENTITY_INSERT [dbo].[FRED_SYSTEME_EXTERNE] OFF;