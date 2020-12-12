
BEGIN TRAN SyncCIFES;  

-----------------------------------------------------------------------------------
--- Variables
-----------------------------------------------------------------------------------

--Set le groupe FES
DECLARE @GroupeFesId AS int
SELECT @GroupeFesId = Groupeid FROM FRED_GROUPE WHERE code = 'GFES'

-----------------------------------------------------------------------------------
--- 1) Changement affectation CIs -> Tous les sous CIs associes
-----------------------------------------------------------------------------------

-- Table de stockage temporaire
DECLARE @ListCisFesWithSousCisAndUserId TABLE (CiId int, OrganisationId int, SousCiId int, SousCiOrganisationId int, UserId int, AffecationId int)

INSERT INTO @ListCisFesWithSousCisAndUserId
SELECT listBase.CiId, listBase.OrganisationId, ciChild.CiId, foChild.OrganisationId, aff.UtilisateurId, aff.UtilisateurRoleOrganisationDeviseId FROM FRED_CI listBase
INNER JOIN dbo.FRED_ORGANISATION fo ON fo.OrganisationId = listBase.OrganisationId AND fo.TypeOrganisationId = 8
INNER JOIN dbo.FRED_ORGANISATION foChild ON foChild.PereId = fo.OrganisationId AND foChild.TypeOrganisationId = 9
INNER JOIN dbo.FRED_CI ciChild ON foChild.OrganisationId = ciChild.OrganisationId
INNER JOIN FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE aff on fo.OrganisationId = aff.OrganisationId
INNER JOIN FRED_ROLE rol on rol.RoleId = aff.RoleId 
	AND (rol.SocieteId = listBase.SocieteId) 
	AND rol.CodeNomFamilier IN ('RCI', 'DCI')
WHERE listBase.SocieteId IN (SELECT fs.SocieteId FROM dbo.FRED_SOCIETE fs WHERE fs.GroupeId  = @GroupeFesId)

-- Insertion des nouvelles affectations
INSERT INTO FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE
SELECT UtilisateurId, listCiAffectation.SousCiOrganisationId, RoleId, DeviseId, CommandeSeuil, DelegationId
FROM FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE aff 	
INNER JOIN @ListCisFesWithSousCisAndUserId listCiAffectation on aff.UtilisateurRoleOrganisationDeviseId = listCiAffectation.AffecationId
WHERE NOT EXISTS (
	SELECT 1 FROM dbo.FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE affect
	INNER JOIN dbo.FRED_ROLE fr ON fr.RoleId = affect.RoleId
	WHERE listCiAffectation.SousCiOrganisationId = affect.OrganisationId
	AND fr.CodeNomFamilier IN ('RCI', 'DCI')
)


-----------------------------------------------------------------------------------
--- 2) Resynchro CIs -> Etablissement depuis les donnees des tables CIs (pas l'arbre)
-----------------------------------------------------------------------------------

-- Tables de stockage temporaire
DECLARE @ListCisFes TABLE (CiId int, Code nvarchar(20), OrganisationId int, organisationPereId int, newOrganisationPereId int)

INSERT INTO @ListCisFes
SELECT ci.CiId, ci.Code, ci.OrganisationId, fo.PereId, fec.OrganisationId
FROM FRED_CI ci
INNER JOIN dbo.FRED_ORGANISATION fo ON ci.OrganisationId = fo.OrganisationId AND fo.TypeOrganisationId = 8
INNER JOIN dbo.FRED_ETABLISSEMENT_COMPTABLE fec ON ci.EtablissementComptableId = fec.EtablissementComptableId
WHERE ci.SocieteId IN (SELECT fs.SocieteId FROM dbo.FRED_SOCIETE fs WHERE fs.GroupeId = @GroupeFesId)

-- Update de l'organisation pere
UPDATE orga SET orga.PereId = newListCi.newOrganisationPereId
FROM FRED_ORGANISATION orga 
INNER JOIN @ListCisFes newListCi ON newListCi.OrganisationId = orga.OrganisationId		
WHERE newListCi.organisationPereId <> newListCi.newOrganisationPereId


-----------------------------------------------------------------------------------
--- 3) Resynchro sous CIs :
---    - sous CIs -> Etablissement depuis les donnees des tables CIs (pas l'arbre)
---    - passage de sous CIs en CIs
-----------------------------------------------------------------------------------

-- Tables de stockage temporaire
DECLARE @ListSousCisFes TABLE (CiId int, Code nvarchar(20), OrganisationId int, organisationPereId int, newOrganisationPereId int)

INSERT INTO @ListSousCisFes
SELECT ci.CiId, ci.Code, ci.OrganisationId, fo.PereId, fec.OrganisationId
FROM FRED_CI ci
INNER JOIN dbo.FRED_ORGANISATION fo ON ci.OrganisationId = fo.OrganisationId AND fo.TypeOrganisationId = 9
INNER JOIN dbo.FRED_ETABLISSEMENT_COMPTABLE fec ON ci.EtablissementComptableId = fec.EtablissementComptableId
WHERE ci.SocieteId IN (SELECT fs.SocieteId FROM dbo.FRED_SOCIETE fs WHERE fs.GroupeId = @GroupeFesId)

-- Update de l'organisation pere et changement du type organisation
UPDATE orga SET orga.TypeOrganisationId = 8, orga.PereId = newListSousCi.newOrganisationPereId
FROM FRED_ORGANISATION orga 
INNER JOIN @ListSousCisFes newListSousCi ON newListSousCi.OrganisationId = orga.OrganisationId		


-----------------------------------------------------------------------------------
--- 4) Correction du Code et Code externe pour les CIs et sous CIs
-----------------------------------------------------------------------------------

-- Tables de stockage temporaire
DECLARE @ListCisFesWithCode TABLE (CiId int, OrganisationId int, Code nvarchar(20), CodeExterne nvarchar(20), NewCode nvarchar(20), NewCodeExterne nvarchar(20))

INSERT INTO @ListCisFesWithCode
SELECT ciid, dbo.FRED_CI.OrganisationId, Code, CodeExterne, '', Code 
FROM FRED_CI 
INNER JOIN dbo.FRED_ORGANISATION fo ON dbo.FRED_CI.OrganisationId = fo.OrganisationId AND (fo.TypeOrganisationId = 8 OR fo.TypeOrganisationId = 9)
WHERE dbo.FRED_CI.SocieteId IN (
	SELECT fs.SocieteId FROM dbo.FRED_SOCIETE fs WHERE fs.GroupeId = @GroupeFesId
)
-- ycourtel 07/01/2019 : Pas la societe ERS et Etablissement ERS => A gerer dans un second script
AND NOT (EtablissementComptableId = 56 AND SocieteId = 20)
AND Code <> CodeExterne AND CodeExterne IS NOT NULL

UPDATE listCis SET listCis.NewCode = COALESCE(listCis.CodeExterne, listCis.Code), listCis.NewCodeExterne = listCis.Code
FROM @ListCisFesWithCode listCis

-- Update code et code externe
UPDATE ci SET ci.Code = newListCi.NewCode, ci.CodeExterne = newListCi.NewCodeExterne
FROM FRED_CI ci 
INNER JOIN @ListCisFesWithCode newListCi ON newListCi.CiId = ci.CiId

COMMIT TRAN SyncCIFES;
