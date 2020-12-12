--Insertion des permissions pour : Affichage Rapport Prime

INSERT INTO[dbo].[FRED_PERMISSION] 
([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
 VALUES('menu.show.budget.controle-budgetaire.index', 1,'0055','Affichage du menu / Accès à la page ''Contrôle budgétaire''.',0)
 
 INSERT INTO[dbo].[FRED_PERMISSION] 
([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
 VALUES('menu.show.budget.avancement.index', 1,'0056','Affichage du menu / Accès à la page ''Avancement''.',0)

 DELETE FROM [dbo].[FRED_PERMISSION]  WHERE PermissionId = 37
 
 --Fin d'insertion des permissions