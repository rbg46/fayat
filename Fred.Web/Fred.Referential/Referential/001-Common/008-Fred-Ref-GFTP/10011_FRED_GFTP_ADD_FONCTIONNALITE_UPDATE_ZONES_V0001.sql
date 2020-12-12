------------------ Add Fonctionality of Update Zones ------------------

-- MAJ / Ajout de la fonctionnalitée
IF EXISTS (select FonctionnaliteId from FRED_FONCTIONNALITE where Code='1007')
UPDATE FRED_FONCTIONNALITE SET ModuleId=(select m.ModuleId from FRED_MODULE m where m.Code='6'),
Code='1007', Libelle='Modifier les zones libres des modeles de commande', 
HorsOrga=0, DateSuppression=NULL, Description=NULL WHERE Code='1007' 
ELSE INSERT INTO FRED_FONCTIONNALITE (ModuleId, Code, Libelle, HorsOrga, DateSuppression, [Description]) 
VALUES ((select m.ModuleId from FRED_MODULE m where m.Code='6'), '1007',
'Modifier les zones libres des modeles de commande', 0, NULL, NULL)

-- MAJ / Ajout de la permission
SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] ON
IF EXISTS (select PermissionId from FRED_PERMISSION where Code='001032') 
UPDATE FRED_PERMISSION SET PermissionKey='modifier.zones.libres.modeles.commande',
PermissionType=1, Code='001032', 
Libelle='Afficher le composant / Modifier les zones libres des modèles de commande''.', 
PermissionContextuelle=0 WHERE Code='001032' ELSE 
INSERT INTO FRED_PERMISSION (PermissionId, PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle) 
VALUES ('1032', 'modifier.zones.libres.modeles.commande', 1, '001032', 'Afficher le composant / Modifier les zones libres des modèles de commande''.', 0)
SET IDENTITY_INSERT [dbo].[FRED_PERMISSION] OFF

-- Ajout de la relation permission - fonctionnalitée
INSERT INTO FRED_PERMISSION_FONCTIONNALITE SELECT p.PermissionId, f.FonctionnaliteId FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f WHERE f.Code='1007' and p.Code='001032'

