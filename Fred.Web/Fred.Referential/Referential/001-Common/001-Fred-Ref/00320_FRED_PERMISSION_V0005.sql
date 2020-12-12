--Insertion des permissions pour : Affichage Rapport Prime

INSERT INTO[dbo].[FRED_PERMISSION] 
([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
 VALUES('menu.show.rapport.prime.index', 1,'0054','Affichage du menu / Accès à la page ''Rapport prime''.',0)
 
 --Fin d'insertion des permissions