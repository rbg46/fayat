-- Suppression des données existantes
DELETE FROM FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE where RoleId in
	(SELECT RoleId FROM FRED_ROLE where Code in ('RCI', 'DCI'))

DELETE FROM FRED_ROLE_FONCTIONNALITE where FonctionnaliteId in
	(SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE where Code in ('1102', '1103'))

DELETE FROM FRED_ROLE where Code in ('RCI', 'DCI')

DELETE FROM FRED_PERMISSION_FONCTIONNALITE where FonctionnaliteId in
	(SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE where Code in ('1102', '1103'))

DELETE FROM FRED_FONCTIONNALITE WHERE Code in ('1102', '1103')

-- Insertion des fonctionnalités des nouveaux roles : ReponsableCI et délégué
INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
VALUES (18,'1102','Gestion du calendrier des affectations',0,NULL,'Permet de gérer les affectations des personnels sur une affaire.');

INSERT INTO FRED_FONCTIONNALITE (ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) 
VALUES (18,'1103','Gestion du calendrier des affectations - Délégation',0,NULL,'Permet par délégation de gérer les affectations des personnels sur une affaire.');


-- Association des permissions aux fonctionnalités

-- Déplacement d'instructions car auparavant mal placées dans les dossiers
IF NOT EXISTS (SELECT PermissionFonctionnaliteId FROM FRED_PERMISSION_FONCTIONNALITE where (PermissionFonctionnaliteId BETWEEN 31 AND 35) OR PermissionFonctionnaliteId = 39)
BEGIN
	SET IDENTITY_INSERT [dbo].[FRED_PERMISSION_FONCTIONNALITE] ON;
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (31,46,46);
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (32,42,47);
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (33,41,48);
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (34,43,49);
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (35,44,50);
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (39,45,50);
	SET IDENTITY_INSERT [dbo].[FRED_PERMISSION_FONCTIONNALITE] OFF;
END

INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
	SELECT P.PermissionId, f.FonctionnaliteId
	FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
	where p.Code='0049' and f.code='1102'

INSERT INTO FRED_PERMISSION_FONCTIONNALITE
	SELECT P.PermissionId, f.FonctionnaliteId
	FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
	where p.Code='0050' and f.code='1103'