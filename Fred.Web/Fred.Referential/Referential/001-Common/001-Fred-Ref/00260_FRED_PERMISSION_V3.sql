-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] ON

 -- Permissions pour les bar�mes exploitation organisation
 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle])
 VALUES(41, 'menu.show.bareme.exploitation.organisation.index', 1, '0041', 'Affichage du menu / Acc�s � la page ''Bar�me exploitation organisation''.', 0)

 -- Permissions pour les bar�mes exploitation CI
 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle])
 VALUES(42, 'menu.show.bareme.exploitation.ci.index', 1, '0042', 'Affichage du menu / Acc�s � la page ''Bar�me exploitation CI''.', 0)

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] OFF
