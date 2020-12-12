--Insertion des permissions pour la gestion des journaux comptables

INSERT INTO[dbo].[FRED_PERMISSION] 
([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
 VALUES('menu.show.journalcomptable.index',1,'0051','Gestion des journaux comptables',1)