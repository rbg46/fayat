SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID ( 'GetOrganisations', 'P' ) IS NOT NULL   
    DROP PROCEDURE [dbo].[GetOrganisations];  
GO  
CREATE PROCEDURE [dbo].[GetOrganisations]
  @text varchar(128),
  @types as [dbo].[TYPE_ORGANISATION_LIST] READONLY,
  @UtilisateurId int,
  @OrganisationIdPere int
AS
BEGIN

--Séléctionner toutes les organiastions d'une manière héarchique  
CREATE TABLE #OrgaTmp (OrganisationId Int,	PereId int,	TypeOrganisationId int,	Code varchar(500),	Libelle varchar(1000))

CREATE TABLE #OrgaTmp2(OrganisationId Int,	PereId int,	TypeOrganisationId int,	Code varchar(500),IdParents varchar(1000), CodeParents varchar(1000),Libelle varchar(1000),TypeOrganisation varchar(1000),CodeParent varchar(1000))

CREATE NONCLUSTERED INDEX IX_PEREID ON [dbo].[#OrgaTmp] ([PereId]) INCLUDE ([OrganisationId],[TypeOrganisationId],[Code],[Libelle])

INSERT INTO #OrgaTmp
SELECT orga.OrganisationId, orga.PereId, orga.TypeOrganisationId  
,RTRIM(COALESCE(CI.Code,ec.Code, gr.Code,h.Code,og.Code, pole.Code, societe.Code)) Code  
,RTRIM(COALESCE(CI.Libelle,ec.Libelle,gr.Libelle,h.Libelle,og.Libelle, pole.Libelle, societe.Libelle)) Libelle  
FROM				  FRED_ORGANISATION orga  
LEFT JOIN [dbo].[FRED_CI] AS ci ON orga.OrganisationId = ci.OrganisationId  
LEFT JOIN [dbo].[FRED_ETABLISSEMENT_COMPTABLE] ec ON orga.OrganisationId = ec.OrganisationId  
LEFT JOIN [dbo].[FRED_GROUPE] AS gr ON orga.[OrganisationId] = gr.OrganisationId  
LEFT JOIN [dbo].[FRED_HOLDING] AS h ON orga.[OrganisationId] = h.OrganisationId  
LEFT JOIN [dbo].[FRED_ORGANISATION_GENERIQUE] og ON orga.OrganisationId = og.OrganisationId  
LEFT JOIN [dbo].[FRED_POLE] AS pole ON orga.OrganisationId = pole.OrganisationId  
LEFT JOIN [dbo].[FRED_SOCIETE] AS societe ON orga.OrganisationId = societe.OrganisationId;  

 
with OrgaList as  
(Select orgaParent.OrganisationId, orgaParent.PereId, orgaParent.TypeOrganisationId, orgaParent.Code  
,cast(null as nvarchar(max)) as IdParents  
,cast(null as nvarchar(max)) as CodeParents  
, orgaParent.Libelle  
  FROM #OrgaTmp orgaParent  
  WHERE PereId IS NULL  
  UNION ALL  
  Select orga.OrganisationId, orga.PereId, orga.TypeOrganisationId, orga.Code  
  ,ISNULL(EL.IdParents + '|', '') + cast(EL.OrganisationId as nvarchar(max))  
  ,ISNULL(EL.CodeParents + '|', '') + EL.Code  
  , orga.Libelle  
  FROM #OrgaTmp orga  
  INNER JOIN OrgaList EL ON orga.PereId = EL.OrganisationId  
  INNER JOIN FRED_ORGANISATION parent ON orga.PereId = parent.OrganisationId  
  WHERE orga.PereId IS NOT NULL)  
  
  --Insérer les orga dans une deuxième table temporaire en appliquant les critères passées en paramètre  
insert into  #OrgaTmp2
SELECT OrgaList.*, typeOrga.Libelle as TypeOrganisation  
,case when  CHARINDEX('|', reverse(CodeParents)) = 0 then null  
else REVERSE(left(REVERSE(CodeParents), CHARINDEX('|', reverse(CodeParents)) -1)) end CodeParent  
FROM OrgaList  
JOIN [dbo].[FRED_TYPE_ORGANISATION] typeOrga ON typeOrga.TypeOrganisationId = OrgaList.TypeOrganisationId  

IF @UtilisateurId IS NOT NULL OR @OrganisationIdPere IS NOT NULL
BEGIN
  ALTER TABLE #OrgaTmp2 ADD Good bit NOT NULL DEFAULT(0)
  DECLARE @CursorOrganisationId int
  DECLARE @CursorIdComplet varchar(max)
  DECLARE @CursorCodeComplet varchar(max)
  DECLARE MY_CURSOR CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY
  FOR 
  SELECT #OrgaTmp2.OrganisationId, ISNULL(IdParents,'') + '|' + cast(#OrgaTmp2.OrganisationId as nvarchar(max)), ISNULL(CodeParents,'') + '|' + Code
  FROM #OrgaTmp2
  WHERE (@UtilisateurId IS NULL OR OrganisationId IN
    (SELECT OrganisationId FROM FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE RoleOrgaDevise where UtilisateurId = @UtilisateurId))
    AND (@OrganisationIdPere IS NULL OR #OrgaTmp2.OrganisationId = @OrganisationIdPere)

  OPEN MY_CURSOR
  FETCH NEXT FROM MY_CURSOR INTO @CursorOrganisationId, @CursorIdComplet, @CursorCodeComplet
  WHILE @@FETCH_STATUS = 0
  BEGIN 
      IF ( @CursorIdComplet LIKE '|%' )
				SET @CursorIdComplet = RIGHT(@CursorIdComplet, LEN(@CursorIdComplet)-1)
			IF ( @CursorCodeComplet LIKE '|%' )
	  		SET	@CursorCodeComplet = RIGHT(@CursorCodeComplet, LEN(@CursorCodeComplet)-1)     
      print @CursorIdComplet
      UPDATE #OrgaTmp2 SET Good = 1
      WHERE Good <> 1
        AND (@UtilisateurId IS NULL OR OrganisationId=@CursorOrganisationId OR IdParents = @CursorIdComplet OR IdParents LIKE @CursorIdComplet + '|%' OR CodeParents = @CursorCodeComplet OR CodeParents LIKE @CursorCodeComplet + '|%')
        AND (@OrganisationIdPere IS NULL OR OrganisationId=@CursorOrganisationId OR IdParents = @CursorIdComplet OR IdParents LIKE @CursorIdComplet + '|%')
      FETCH NEXT FROM MY_CURSOR INTO @CursorOrganisationId, @CursorIdComplet, @CursorCodeComplet
  END
  CLOSE MY_CURSOR
  DEALLOCATE MY_CURSOR
END
ELSE ALTER TABLE #OrgaTmp2 ADD Good bit NOT NULL DEFAULT(1)

-- Récupère seulement les Organisations en fonction des types (Holding, Pole, ..., Ets, CI) 
SELECT OrganisationId, PereId, TypeOrganisationId, Code, IdParents, CodeParents, Libelle, TypeOrganisation, CodeParent
 from #OrgaTmp2
 WHERE Good = 1  AND (#OrgaTmp2.TypeOrganisationId IN (SELECT * FROM @types))
  -- [TSA] : Ajout du filtre sur le Code ou Libelle de l'organisation à la fin de la procédure
 AND (@text IS NULL OR @text = '' 
 OR Code LIKE '%' + @text + '%' 
 OR (case when  charindex('|', reverse(CodeParents)) = 0 then null else reverse(left(reverse(CodeParents), charindex('|', reverse(CodeParents)) -1)) end) LIKE '%' + @text + '%'  
 OR Libelle LIKE '%' + @text + '%')

-- Supprime les tables temporaires
Drop table #OrgaTmp
Drop table #OrgaTmp2

END