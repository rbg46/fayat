-- Correction des associations Rôle-Fonctionnalité

DECLARE @GroupeFESId INT;
SET @GroupeFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code='GFES')

DELETE FROM FRED_ROLE_FONCTIONNALITE 
WHERE RoleFonctionnaliteId in (
	SELECT rf.RoleFonctionnaliteId
	FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId
		inner join FRED_SOCIETE s on s.SocieteId = r.SocieteId
	WHERE s.GroupeId=@GroupeFESId and (r.Code='RCI' or r.Code='DCI'))


INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.Code='RCI' and s.GroupeId=@GroupeFESId and f.code='1102'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.Code='RCI'	and s.GroupeId=@GroupeFESId and f.code='1103'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.Code='RCI'	and s.GroupeId=@GroupeFESId and f.code='1201'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.Code='RCI'	and s.GroupeId=@GroupeFESId and f.code='1100'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.Code='DCI'	and s.GroupeId=@GroupeFESId and f.code='1102'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 1
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.Code='DCI'	and s.GroupeId=@GroupeFESId and f.code='1103'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId, FRED_FONCTIONNALITE f
	where r.Code='DCI'	and s.GroupeId=@GroupeFESId and f.code='1100'