--Insertion des permissions pour la gestion des feature flipping

INSERT INTO[dbo].[FRED_PERMISSION] 
([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
 VALUES('menu.show.FeatureFlipping.index',1,'0052','Affichage du menu / Accès à la page ''Gérer les features flipping.''',0)