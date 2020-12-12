-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] ON
 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) VALUES(43,'menu.show.opertiondiverse.index',1,'0043','Affichage du menu / Accès à la page ''Gérer les opérations diverses''.',0) 
 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) VALUES(44,'menu.show.bilanflash.index',1,'0044','Affichage du menu / Accès à la page ''Gérer les bilans flash''.',0) 
 INSERT INTO[dbo].[FRED_PERMISSION] ([PermissionId], [PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) VALUES(45,'menu.show.objectifflash.index',1,'0045','Accès à la page ''Gérer les objectifs flash''.',0) 
SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] OFF
