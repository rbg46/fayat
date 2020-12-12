-- =======================================================================================================================================
-- Author:		Anass BOUHAFER  04/03/2020
--
-- Description:
--          Bug_12371 : Optimisation du temps de chargement lors d'une recherche d'un personnel 
--
-- =======================================================================================================================================

IF OBJECT_ID ( 'Fred_Picklist_GetPersonnelsByFilter', 'P' ) IS NOT NULL   
	DROP PROCEDURE Fred_Picklist_GetPersonnelsByFilter; 
GO

Create PROCEDURE Fred_Picklist_GetPersonnelsByFilter
	@ValueText nvarchar(50) = '',
	@ValueTextNom nvarchar(50) = '',
	@ValueTextPrenom nvarchar(50) = '',
	@EtablissementPaieCode nvarchar(50) = '',
	@SocieteCode nvarchar(50) = '',
	@IsActif bit = 0,
	@IsInterne bit = 0,
	@IsUser bit = 0,
	@IsNonPointable bit = 0,
	@IsSuperAdmin bit = 0,
	@CurrentGroupId int = 0,
	@IsInterimaire bit = 0,
	@WithSplitFilters bit = 0,
	@Page int = 1,
	@PageSize int = 50
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Statements for procedure
	Select societe.Code + '-' + societe.Libelle as Societe,
    personnel.Matricule, 
    personnel.Nom, personnel.Prenom, 
    utilisateur.UtilisateurId, 
	personnel.DateSuppression, 
    etabPaie.Code + '-' + etabPaie.Libelle as EtablissementPaie, 
    personnel.DateEntree, 
    personnel.DateSortie, 
    personnel.IsInterne, 
	extDirectory.IsActived as IsActivedExtDirectory, 
    personnel.IsInterimaire, 
    personnel.PersonnelId, 
    personnel.IsPersonnelNonPointable, 
    societe.SocieteId,
	CASE 
	WHEN (personnel.DateSuppression is null and (personnel.DateSortie is null or personnel.DateSortie >= GETUTCDATE())
	and (personnel.DateEntree is null or convert(varchar(10), personnel.DateEntree, 120) <= convert(varchar(10),GETUTCDATE(), 120))) then CAST(1 AS bit)
	Else CAST(0 AS bit)
	end as IsActif
	from [FRED_PERSONNEL] personnel
	left join [FRED_ETABLISSEMENT_PAIE] etabPaie on personnel.EtablissementPayeId = etabPaie.EtablissementPaieId
	left join [FRED_ETABLISSEMENT_PAIE] etabRatt on personnel.EtablissementPayeId = etabRatt.EtablissementPaieId
	left join [FRED_PAYS] pays on personnel.[PaysId] = pays.[PaysId]
	left join FRED_SOCIETE societe on personnel.SocieteId = societe.SocieteId
	left join FRED_UTILISATEUR utilisateur on utilisateur.UtilisateurId = personnel.PersonnelId
	left join [FRED_EXTERNALDIRECTORY] extDirectory on extDirectory.FayatAccessDirectoryId = utilisateur.FayatAccessDirectoryId
	left join FRED_RESSOURCE ressource on personnel.RessourceId = ressource.RessourceId
	Where (@WithSplitFilters = 0  and (  
	((@IsSuperAdmin = 1 Or @CurrentGroupId = societe.GroupeId)
	and (@IsActif = 0 
		or 
		(@IsActif = 1 
			and (personnel.DateSuppression is null 
			and (personnel.DateSortie is null or personnel.DateSortie >= GETUTCDATE())
			and (personnel.DateEntree is null or convert(varchar(10), personnel.DateEntree, 120) <= convert(varchar(10),GETUTCDATE(), 120)))))
	and (@IsInterimaire = 0 or personnel.IsInterimaire = @IsInterimaire)
	and (@IsUser = 0 or (utilisateur.UtilisateurId = personnel.PersonnelId and utilisateur.IsDeleted != 1))
	and (@IsNonPointable = 0 or personnel.IsPersonnelNonPointable = @IsNonPointable)
	and (@IsInterne = 0 or personnel.IsInterne = @IsInterne)
	and (
		ISNULL(@ValueText, '') = '' 
		or personnel.Nom like '%' + @ValueText +'%'
		or personnel.Prenom like '%' + @ValueText +'%'
		or personnel.Matricule like '%' + @ValueText +'%'
		or societe.Code like '%' + @ValueText +'%'
		or societe.Libelle like '%' + @ValueText +'%'
		)
	))) 	
	or
	(@WithSplitFilters = 1 and 
	(((@IsSuperAdmin = 1 Or @CurrentGroupId = societe.GroupeId)
	and (@IsActif = 0 
		or 
		(@IsActif = 1 
			and (personnel.DateSuppression is null 
			and (personnel.DateSortie is null or personnel.DateSortie >= GETUTCDATE())
			and (personnel.DateEntree is null or convert(varchar(10), personnel.DateEntree, 120) <= convert(varchar(10),GETUTCDATE(), 120)))))
	and (@IsInterimaire = 0 or personnel.IsInterimaire = @IsInterimaire)
	and (@IsUser = 0 or (utilisateur.UtilisateurId = personnel.PersonnelId and utilisateur.IsDeleted != 1))
	and (@IsNonPointable = 0 or personnel.IsPersonnelNonPointable = @IsNonPointable)
	and (@IsInterne = 0 or personnel.IsInterne = @IsInterne)
	and (
			ISNULL(@ValueTextPrenom, '') = ''
			or personnel.Prenom like '%' + @ValueTextPrenom +'%'
		)
	and (
			ISNULL(@ValueTextNom, '') = ''
			or personnel.Nom like '%' + @ValueTextNom +'%'
		)
	and (
			ISNULL(@ValueText, '') = ''
			or societe.Code like '%' + @ValueText +'%'
			or societe.Libelle like '%' + @ValueText +'%'
			or personnel.Matricule like '%' + @ValueText +'%'
			or etabPaie.Libelle like '%' + @ValueText +'%'
			or etabPaie.Code like '%' + @ValueText +'%'
		)
	and (
			ISNULL(@EtablissementPaieCode, '') = ''
			or etabPaie.Code like '%' + @EtablissementPaieCode +'%' 
		)
	and (
			ISNULL(@SocieteCode, '') = ''
			or societe.Code like '%' + @SocieteCode +'%' 
		)
	)))

	order by societe.Code, societe.Libelle, personnel.Matricule
	Offset ((@Page - 1) * @pagesize) Rows
	Fetch Next @Pagesize Rows Only

End