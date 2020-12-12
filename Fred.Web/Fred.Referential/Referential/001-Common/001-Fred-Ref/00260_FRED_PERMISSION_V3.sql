-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] ON

 -- Permissions pour les barèmes exploitation organisation
 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle])
 VALUES(41, 'menu.show.bareme.exploitation.organisation.index', 1, '0041', 'Affichage du menu / Accès à la page ''Barème exploitation organisation''.', 0)

 -- Permissions pour les barèmes exploitation CI
 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle])
 VALUES(42, 'menu.show.bareme.exploitation.ci.index', 1, '0042', 'Affichage du menu / Accès à la page ''Barème exploitation CI''.', 0)

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] OFF
