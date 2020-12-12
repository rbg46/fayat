-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] ON
	INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) VALUES(49, 'button.show.remonteevrac.validationpointage.index',1,'0048','Affichage des boutons Remontée vrac',0)  
SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] OFF