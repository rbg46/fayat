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
                  
	--Création des tables temporaites et des index                
	CREATE TABLE #OrgaTmp (OrganisationId Int, PereId int, TypeOrganisationId int, Code varchar(500), Libelle varchar(1000))                  
	CREATE TABLE #OrgaTmp2(OrganisationId Int, PereId int, TypeOrganisationId int, Code varchar(500),IdParents varchar(1000), CodeParents varchar(1000),Libelle varchar(1000),TypeOrganisation varchar(1000),CodeParent varchar(1000))                  
                  
	CREATE NONCLUSTERED INDEX IX_PEREID ON [dbo].[#OrgaTmp] ([PereId]) INCLUDE ([OrganisationId],[TypeOrganisationId],[Code],[Libelle])                  
	CREATE NONCLUSTERED INDEX [IX_ORGANISATION] ON [dbo].[#OrgaTmp2] ([OrganisationId]) INCLUDE ([Code],[IdParents],[CodeParents])                
                
	--Séléctionner toutes les organiastions d'une manière hiérarchique                      
	INSERT INTO #OrgaTmp                  
	SELECT orga.OrganisationId, orga.PereId, orga.TypeOrganisationId                    
	,RTRIM(COALESCE(CI.Code,ec.Code, gr.Code,h.Code,og.Code, pole.Code, societe.Code)) Code                    
	,RTRIM(COALESCE(CI.Libelle,ec.Libelle,gr.Libelle,h.Libelle,og.Libelle, pole.Libelle, societe.Libelle)) Libelle                    
	FROM  [dbo].[FRED_ORGANISATION] AS orga WITH (READUNCOMMITTED)                     
	LEFT JOIN [dbo].[FRED_CI] AS ci WITH (READUNCOMMITTED)  ON orga.OrganisationId = ci.OrganisationId                    
	LEFT JOIN [dbo].[FRED_ETABLISSEMENT_COMPTABLE]  AS ec WITH (READUNCOMMITTED)  ON orga.OrganisationId = ec.OrganisationId                    
	LEFT JOIN [dbo].[FRED_GROUPE] AS gr WITH (READUNCOMMITTED)  ON orga.[OrganisationId] = gr.OrganisationId                    
	LEFT JOIN [dbo].[FRED_HOLDING]  AS h WITH (READUNCOMMITTED) ON orga.[OrganisationId] = h.OrganisationId                    
	LEFT JOIN [dbo].[FRED_ORGANISATION_GENERIQUE] AS og WITH (READUNCOMMITTED)  ON orga.OrganisationId = og.OrganisationId                    
	LEFT JOIN [dbo].[FRED_POLE]  AS pole WITH (READUNCOMMITTED)  ON orga.OrganisationId = pole.OrganisationId                    
	LEFT JOIN [dbo].[FRED_SOCIETE] AS societe WITH (READUNCOMMITTED)  ON orga.OrganisationId = societe.OrganisationId    
	WHERE 1=1    
		AND RTRIM(COALESCE(CI.Libelle,ec.Libelle,gr.Libelle,h.Libelle,og.Libelle, pole.Libelle, societe.Libelle)) IS NOT NULL    
		AND RTRIM(COALESCE(CI.Code,ec.Code, gr.Code,h.Code,og.Code, pole.Code, societe.Code))  IS NOT NULL;    
                  
	WITH OrgaList AS                    
		(Select orgaParent.OrganisationId, orgaParent.PereId, orgaParent.TypeOrganisationId, orgaParent.Code                    
		,CAST(null as nvarchar(max)) as IdParents                    
		,CAST(null as nvarchar(max)) as CodeParents                    
		,orgaParent.Libelle                    
		FROM #OrgaTmp orgaParent                    
		WHERE PereId IS NULL                    
                
	UNION ALL                    
                
		Select orga.OrganisationId, orga.PereId, orga.TypeOrganisationId, orga.Code                    
		,ISNULL(EL.IdParents + '|', '') + CAST(EL.OrganisationId as nvarchar(max))                    
		,ISNULL(EL.CodeParents + '|', '') + EL.Code                    
		,orga.Libelle                    
		FROM #OrgaTmp orga                    
		INNER JOIN OrgaList EL ON orga.PereId = EL.OrganisationId                    
		INNER JOIN FRED_ORGANISATION parent ON orga.PereId = parent.OrganisationId                    
		WHERE orga.PereId IS NOT NULL)                 
                    
	--Insérer les orga dans une deuxième table temporaire en appliquant les critères passées en paramètre                    
	INSERT INTO #OrgaTmp2                  
	SELECT OrgaList.*, typeOrga.Libelle as TypeOrganisation                    
	,CASE WHEN CHARINDEX('|', reverse(CodeParents)) = 0 THEN NULL ELSE REVERSE(left(REVERSE(CodeParents), CHARINDEX('|', reverse(CodeParents)) -1)) END CodeParent                    
	FROM OrgaList                    
	INNER JOIN [dbo].[FRED_TYPE_ORGANISATION] typeOrga ON typeOrga.TypeOrganisationId = OrgaList.TypeOrganisationId                    
                  
	--Si on a un utilisateur ou une organisation père                  
	IF @UtilisateurId IS NOT NULL OR @OrganisationIdPere IS NOT NULL                  
	BEGIN             
		CREATE TABLE #Temp (OrganisationId Int, CursorIdComplet nvarchar(max), CursorCodeComplet nvarchar(max))                  
           
		--Modification de la table #OrgaTmp2 en ajoutant un booléen afin de déterminer je ne sais pas quoi                
		ALTER TABLE #OrgaTmp2 ADD Good bit NOT NULL DEFAULT(0)                        
		--Récupération des RoleOrganisationDevise pour l'utilisateur donnée                
       
		IF(@UtilisateurId IS NOT NULL)      
		BEGIN      
			SELECT OrganisationId                 
			INTO #RoleOrgaDevise FROM FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE RoleOrgaDevise                 
			WHERE UtilisateurId = @UtilisateurId                
      
			INSERT INTO #Temp      
			SELECT DISTINCT #OrgaTmp2.OrganisationId, ISNULL(IdParents,'') + '|' + cast(#OrgaTmp2.OrganisationId as nvarchar(max)) AS CursorIdComplet, ISNULL(CodeParents,'') + '|' + Code  AS CursorCodeComplet                
			FROM #OrgaTmp2                  
			INNER JOIN #RoleOrgaDevise on #OrgaTmp2.OrganisationId = #RoleOrgaDevise.OrganisationId                 
			Where @OrganisationIdPere IS NULL OR #OrgaTmp2.OrganisationId = @OrganisationIdPere         
			Drop Table #RoleOrgaDevise                         
		END      
		ELSE IF (@OrganisationIdPere IS NOT NULL)      
		BEGIN      
			INSERT INTO #Temp      
			SELECT DISTINCT #OrgaTmp2.OrganisationId, ISNULL(IdParents,'') + '|' + cast(#OrgaTmp2.OrganisationId as nvarchar(max)) AS CursorIdComplet, ISNULL(CodeParents,'') + '|' + Code  AS CursorCodeComplet                
			FROM #OrgaTmp2                  
			Where @OrganisationIdPere IS NULL OR #OrgaTmp2.OrganisationId = @OrganisationIdPere                  
		END      
      
		UPDATE #OrgaTmp2 SET #OrgaTmp2.Good = 1                  
		FROM #OrgaTmp2                
		JOIN #Temp ON (@UtilisateurId IS NULL OR #OrgaTmp2.OrganisationId = #Temp.OrganisationID OR #OrgaTmp2.IdParents = #Temp.CursorIdComplet OR #OrgaTmp2.IdParents LIKE #Temp.CursorIdComplet + '|%' OR #OrgaTmp2.CodeParents = #Temp.CursorCodeComplet OR #OrgaTmp2.CodeParents LIKE #Temp.CursorCodeComplet +'|%')          
		AND(@OrganisationIdPere IS NULL OR #OrgaTmp2.OrganisationId = #Temp.OrganisationID Or #OrgaTmp2.IdParents = #Temp.CursorIdComplet Or #OrgaTmp2.IdParents LIKE #Temp.CursorIdComplet + '|%')           
		WHERE #OrgaTmp2.Good <> 1          

		Drop Table #Temp                        
	END                  
	ELSE                
	BEGIN                 
		ALTER TABLE #OrgaTmp2 ADD Good bit NOT NULL DEFAULT(1)                  
	END                
                  
	-- Récupère seulement les Organisations en fonction des types (Holding, Pole, ..., Ets, CI)                   
	SELECT OrganisationId, PereId, TypeOrganisationId, Code, IdParents, CodeParents, Libelle, TypeOrganisation, CodeParent                  
	FROM #OrgaTmp2                  
	WHERE Good = 1  AND (#OrgaTmp2.TypeOrganisationId IN (SELECT * FROM @types))                
	-- [TSA] : Ajout du filtre sur le Code ou Libelle de l'organisation à la fin de la procédure                  
	AND (  
			@text IS NULL OR @text = ''                   
			OR Code LIKE '%' + @text + '%'                   
			OR (case when  charindex('|', reverse(CodeParents)) = 0 then null else reverse(left(reverse(CodeParents), charindex('|', reverse(CodeParents)) -1)) end) LIKE '%' + @text + '%'                    
			OR Libelle LIKE '%' + @text + '%'                
		)                  
       
	-- Supprime les tables temporaires                  
	Drop table #OrgaTmp                  
	Drop table #OrgaTmp2                       
END