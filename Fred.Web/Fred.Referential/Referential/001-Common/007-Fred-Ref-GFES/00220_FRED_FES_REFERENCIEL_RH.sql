-- =======================================================================================================================================
-- Author:		Hachem ALAOUI ISMAILI 25/12/2019
--
-- Description:
--          Mise à jour des données (Delapcement, zone deplacement, prime,code absence,majoration) pour les socité appartenant au groupe FES
--
-- =======================================================================================================================================


--FRED_CODE_DEPLACEMENT
Update [FRED_CODE_DEPLACEMENT] 
  SET
	[IsCadre]=1,
	[IsETAM]=1,
	[IsOuvrier]=1
	FROM
		[FRED_CODE_DEPLACEMENT]  d
		inner join [FRED_SOCIETE]  s on s.[SocieteId]=d.[SocieteId]  
		inner join [FRED_GROUPE] g on g.GroupeId =s.GroupeId	
	where  g.[Code] like 'GFES'
	
	
--FRED_CODE_ZONE_DEPLACEMENT
Update [FRED_CODE_ZONE_DEPLACEMENT] 
  SET
	[IsCadre]=1,
	[IsETAM]=1,
	[IsOuvrier]=1
	FROM
		[FRED_CODE_ZONE_DEPLACEMENT]  d
		inner join [FRED_SOCIETE]  s on s.[SocieteId]=d.[SocieteId]  
		inner join [FRED_GROUPE] g on g.GroupeId =s.GroupeId
	where  g.[Code] like 'GFES'
	
--FRED_CODE_ABSENCE	
	Update [FRED_CODE_ABSENCE] 
  SET
	[IsCadre]=1,
	[IsETAM]=1,
	[IsOuvrier]=1
	FROM
		[FRED_CODE_ABSENCE]  d
		inner join [FRED_SOCIETE]  s on s.[SocieteId]=d.[SocieteId]  
		inner join [FRED_GROUPE] g on g.GroupeId =s.GroupeId
	where  g.[Code] like 'GFES'
	
--FRED_CODE_MAJORATION	
	Update [FRED_CODE_MAJORATION] 
	SET
		[IsCadre]=1,
		[IsETAM]=1,
		[IsOuvrier]=1
	FROM [FRED_CODE_MAJORATION] d
		inner join [FRED_GROUPE] g on g.GroupeId =d.GroupeId
	where  g.[Code] like 'GFES'

--FRED_PRIME

Update [FRED_PRIME] 
	SET
		[IsCadre]=1,
		[IsETAM]=1,
		[IsOuvrier]=1
  
	FROM [FRED_PRIME]  d
		inner join [FRED_SOCIETE]  s on s.[SocieteId]=d.[SocieteId]  
		inner join [FRED_GROUPE] g on g.GroupeId =s.GroupeId
	where  g.[Code] like 'GFES'



