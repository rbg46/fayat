-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] ON

 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle])
 VALUES(40,'menu.show.areas.pointage.views.rapport.new',1,'0040','Affichage du menu / Accès à la page ''Nouveau rapport''.',0) 

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] OFF
