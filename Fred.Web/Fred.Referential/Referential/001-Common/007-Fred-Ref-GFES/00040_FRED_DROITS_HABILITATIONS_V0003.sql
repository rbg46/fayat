-- Ajout des fonctionnalités de pointage au rôle Délégué CI

DECLARE @GroupeFESId INT;
SET @GroupeFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code='GFES')

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId,
		FRED_FONCTIONNALITE f
	WHERE s.GroupeId=@GroupeFESId and r.Code='DCI'
		and f.code in ('400', '401', '402', '404')

-- Migration des rôles d'utilisateur CDC / CDT vers DCI
DECLARE @SocId INT;
SET @SocId = (SELECT SocieteId FROM FRED_SOCIETE WHERE Code='E001')
DECLARE @RoleDCIId INT;
SET @RoleDCIId = (SELECT RoleId from FRED_ROLE WHERE Code='DCI' and SocieteId=@SocId)

UPDATE FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE
	SET RoleId=@RoleDCIId
	WHERE UtilisateurId IN (SELECT PersonnelId FROM FRED_PERSONNEL WHERE SocieteId=@SocId)
		AND RoleId IN (SELECT RoleId FROM FRED_ROLE WHERE SocieteId=@SocId AND CodeNomFamilier IN ('CDC', 'CDT'))