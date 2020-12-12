--Insertion des permissions pour : gestion d'équipe et délégation

INSERT INTO[dbo].[FRED_PERMISSION] 
([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
 VALUES('button.enabled.validate.rapport.index', 1,'0053','Affichage et activation du bouton valider rapport.',1)
 
 --Fin d'insertion des permissions