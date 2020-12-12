-- Ajout des nouvelles fonctionnalités d'accès aux écrans de pointage "FES" pour les RCI, DCI et Gestionnaires de paye (GSP)

DECLARE @SocId INT;
SET @SocId = (SELECT SocieteId FROM FRED_SOCIETE WHERE Code='E001')

INSERT INTO FRED_ROLE_FONCTIONNALITE
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r, FRED_FONCTIONNALITE f
	WHERE r.SocieteId=@SocId and r.CodeNomFamilier='RCI' and f.Code in ('406', '407')

INSERT INTO FRED_ROLE_FONCTIONNALITE
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r, FRED_FONCTIONNALITE f
	WHERE r.SocieteId=@SocId and r.CodeNomFamilier='DCI' and f.Code in ('406')

INSERT INTO FRED_ROLE_FONCTIONNALITE
	SELECT r.RoleId, f.FonctionnaliteId, 2
	FROM FRED_ROLE r, FRED_FONCTIONNALITE f
	WHERE r.SocieteId=@SocId and r.CodeNomFamilier='GSP' and f.Code in ('406', '407', '408')