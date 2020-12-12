-- --------------------------------------------------
-- FRED 2018 - INJECTION DES DONNES
-- --------------------------------------------------

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] ON
 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) VALUES(50, 'button.enabled.close.period.index',1,'0047','Affichage et activation du bouton de clôture des périodes comptables.',0)
SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] OFF