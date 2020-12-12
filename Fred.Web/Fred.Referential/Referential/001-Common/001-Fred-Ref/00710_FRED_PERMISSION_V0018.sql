-- NPI :
-- Il y a 35 scripts qui concernent FRED_PERMISSION
-- Certaines permissions ont des identifiants/codes identiques, certaines non
-- Il est difficile de savoir là où on en est
-- Pour celà, je repars avec un identifiant et un code = 1000

SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] ON

INSERT INTO[dbo].[FRED_PERMISSION] (
	[PermissionId],
	[PermissionKey],
	[PermissionType],
	[Code],
	[Libelle],
	[PermissionContextuelle])
VALUES(
	1000,
	'menu.show.budget.bibliotheque-prix.index',
	1,
	'1000',
	'Affichage du menu / Accès à la page ''Bibliotheque des prix''.',
	0) 
 
SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] OFF
