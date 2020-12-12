-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] ON
 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) VALUES(46,'menu.show.reception.tableau',1,'0046','Affichage du menu / Accès à la page ''Tableau des réceptions''.',0)  
SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] OFF
