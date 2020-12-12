-- --------------------------------------------------
-- FRED 2018 - R4 - OCTOBRE 2018 
-- INJECTION DES DONNES POUR FRED - GROUPE FAYAT TP
-- --------------------------------------------------

SET NOCOUNT ON
/* -------------------
SUPPRESSION DES NATURES
---------------------- */
DECLARE @ReferentielEtenduId INT;
DECLARE A CURSOR FOR 
SELECT FRED_SOCIETE_RESSOURCE_NATURE.ReferentielEtenduId
	FROM FRED_SOCIETE_RESSOURCE_NATURE	WHERE SocieteId = ( SELECT SocieteId FROM FRED_SOCIETE Where Code = '0143' AND GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE code = 'GFTP'))
OPEN A 
FETCH NEXT FROM A INTO @ReferentielEtenduId  

WHILE @@FETCH_STATUS = 0  
BEGIN  
      
	-- TENTATIVE DE SUPPRESSION DE LA RESSOURCE
	BEGIN TRY  
		DELETE FROM FRED_SOCIETE_RESSOURCE_NATURE WHERE ReferentielEtenduId = @ReferentielEtenduId;
		--PRINT 'SUPPRESSION FRED_SOCIETE_RESSOURCE_NATURE #' + CONVERT(nvarchar,  @ReferentielEtenduId);
	END TRY  
	BEGIN CATCH  
		PRINT 'ERROR : FRED_SOCIETE_RESSOURCE_NATURE IMPOSSIBLE RESSOURCE #' + CONVERT(nvarchar,  @ReferentielEtenduId);
	END CATCH;   
	FETCH NEXT FROM A INTO @ReferentielEtenduId 

END 

CLOSE A
DEALLOCATE A


/* -------------------
SUPPRESSION DES NATURES
---------------------- */
DECLARE @NatureId INT;
DECLARE B CURSOR FOR 
SELECT FRED_NATURE.NatureId
	FROM FRED_NATURE	WHERE SocieteId = ( SELECT SocieteId FROM FRED_SOCIETE Where Code = '0143' AND GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE code = 'GFTP'))

OPEN B  
FETCH NEXT FROM B INTO @NatureId  

WHILE @@FETCH_STATUS = 0  
BEGIN  
      

	-- TENTATIVE DE SUPPRESSION DE LA RESSOURCE
	BEGIN TRY  
		DELETE FROM FRED_NATURE WHERE NatureId = @NatureId;
		--PRINT 'SUPPRESSION NATURE #' + CONVERT(nvarchar,  @NatureId);
	END TRY  
	BEGIN CATCH  
		PRINT 'ERROR : NATURE IMPOSSIBLE RESSOURCE #' + CONVERT(nvarchar,  @NatureId);
		UPDATE FRED_NATURE SET Libelle = '(DELETED)'+Libelle WHERE NatureId = @NatureId;
	END CATCH;   
	FETCH NEXT FROM B INTO @NatureId 

END 

CLOSE B
DEALLOCATE B



/* -------------------
SUPPRESSION DES RESSOURCES
---------------------- */
DECLARE @RessourceId INT;
DECLARE C CURSOR FOR 
SELECT FRED_RESSOURCE.RessourceId
					FROM    dbo.FRED_CHAPITRE INNER JOIN
							dbo.FRED_SOUS_CHAPITRE ON dbo.FRED_CHAPITRE.ChapitreId = dbo.FRED_SOUS_CHAPITRE.ChapitreId INNER JOIN
							dbo.FRED_RESSOURCE ON dbo.FRED_SOUS_CHAPITRE.SousChapitreId = dbo.FRED_RESSOURCE.SousChapitreId
							WHERE GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE code = 'GFTP')

OPEN C  
FETCH NEXT FROM C INTO @RessourceId  

WHILE @@FETCH_STATUS = 0  
BEGIN  
      

	-- TENTATIVE DE SUPPRESSION DE LA RESSOURCE
	BEGIN TRY  
		DELETE FROM FRED_RESSOURCE WHERE RessourceId = @RessourceId;
	--PRINT 'SUPPRESSION RESSOURCE #' + CONVERT(nvarchar,  @RessourceId);
	END TRY  
	BEGIN CATCH  
		PRINT 'ERROR : SUPPRESSION IMPOSSIBLE RESSOURCE #' + CONVERT(nvarchar,  @RessourceId);
		UPDATE FRED_RESSOURCE SET Libelle = '(DELETED)'+Libelle WHERE RessourceId = @RessourceId;
	END CATCH;   
	FETCH NEXT FROM C INTO @RessourceId 

END 

CLOSE C  
DEALLOCATE C 




/* -------------------
SUPPRESSION DES SOUS-CHAPITRE
---------------------- */
DECLARE @SousChapitreId INT;
DECLARE D CURSOR FOR 
SELECT        FRED_SOUS_CHAPITRE.SousChapitreId
FROM          dbo.FRED_CHAPITRE INNER JOIN
              dbo.FRED_SOUS_CHAPITRE ON dbo.FRED_CHAPITRE.ChapitreId = dbo.FRED_SOUS_CHAPITRE.ChapitreId
			  WHERE GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE code = 'GFTP')

OPEN D  
FETCH NEXT FROM D INTO @SousChapitreId  

WHILE @@FETCH_STATUS = 0  
BEGIN  
      

	-- TENTATIVE DE SUPPRESSION DE LA RESSOURCE
	BEGIN TRY  
		DELETE FROM FRED_SOUS_CHAPITRE WHERE SousChapitreId = @SousChapitreId;
	--PRINT 'SUPPRESSION SOUS-CHAPITRE #' + CONVERT(nvarchar,  @SousChapitreId);
	END TRY  
	BEGIN CATCH  
		PRINT 'ERROR : SUPPRESSION IMPOSSIBLE SOUS-CHAPITRE #' + CONVERT(nvarchar,  @SousChapitreId);
		UPDATE FRED_SOUS_CHAPITRE SET Libelle = '(DELETED)'+Libelle WHERE SousChapitreId = @SousChapitreId;
	END CATCH;   
	FETCH NEXT FROM D INTO @SousChapitreId 

END 

CLOSE D 
DEALLOCATE D 



/* -------------------
SUPPRESSION DES CHAPITRE
---------------------- */
DECLARE @ChapitreId INT;
DECLARE E CURSOR FOR 
SELECT        FRED_CHAPITRE.ChapitreId
FROM          dbo.FRED_CHAPITRE 
			  WHERE GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE code = 'GFTP')
			  
OPEN E  
FETCH NEXT FROM E INTO @ChapitreId  

WHILE @@FETCH_STATUS = 0  
BEGIN  
      

	-- TENTATIVE DE SUPPRESSION DE LA RESSOURCE
	BEGIN TRY  
		DELETE FROM FRED_CHAPITRE WHERE ChapitreId = @ChapitreId;
	--PRINT 'SUPPRESSION CHAPITRE #' + CONVERT(nvarchar,  @ChapitreId);
	END TRY  
	BEGIN CATCH  
		PRINT 'ERROR : SUPPRESSION IMPOSSIBLE CHAPITRE #' + CONVERT(nvarchar,  @ChapitreId);
		UPDATE FRED_CHAPITRE SET Libelle = '(DELETED)'+Libelle WHERE ChapitreId = @ChapitreId;
	END CATCH;   
	FETCH NEXT FROM E INTO @ChapitreId 

END 

CLOSE E 
DEALLOCATE E 



SET NOCOUNT OFF



-- AJOUT DES CHAPITRES
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='10',@Libelle='MATERIAUX & FOURNITURES'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='20',@Libelle='LOCATIONS'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='30',@Libelle='PRESTATION'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='40',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='60',@Libelle='MATERIEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFTP',@Code='50',@Libelle='PERSONNEL'




-- AJOUT DES SOUS-CHAPITRE
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='10',@Code='10-0143',@Libelle='MATÉRIAUX ET FOURNITURES '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='20',@Code='20-0143',@Libelle='LOCATIONS '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='30',@Code='30-0143',@Libelle='PRESTATION '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='40',@Code='40-0143',@Libelle='FRAIS GÉNÉRAUX '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='60',@Code='60-0143',@Libelle='MATERIEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFTP',@ChapitreCode='50',@Code='50-0143',@Libelle='PERSONNEL'




-- AJOUT DES RESSOURCES
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='ARMAT-00',@Libelle='ARMATURES BA ET BP',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='ASPBI-00',@Libelle='ASPHALTE ET BITUME',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='BETON-00',@Libelle='BETON PRET À L''EMPLOI',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='COFFR-00',@Libelle='COFFRAGE (BOIS,CONTREPLA)',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='BOICO-00',@Libelle='BOIS-CONTREPLAQUES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='CARBU-00',@Libelle='CARBURANTS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='LUBRI-00',@Libelle='LUBRIFIANTS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='CHAUX-00',@Libelle='CHAUX VIVE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='CIMEN-00',@Libelle='CIMENT',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='ENROB-00',@Libelle='ENROBES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='EMULS-00',@Libelle='EMULSION',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='INFOR-00',@Libelle='FOURN.INFORMATIQUE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='GEOCO-00',@Libelle='GEOCOMPOSITES & ASSIMILÉS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='GRANU-00',@Libelle='GRAVE-SABLE-AGREGATS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='TOPOG-00',@Libelle='FOURN.TOPOGRAPHIE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='MCOMP-00',@Libelle='MATERIAUX COMPOSITES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='METAL-00',@Libelle='PRODUITS METALLURGIQUES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='METAL-00',@Libelle='ELEMENTS FONTE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='MOBIL-00',@Libelle='MOBILIER URBAIN, MUR ET',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='OUTIL-00',@Libelle='OUTILLAGE TRAVAUX',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='OXGAZ-00',@Libelle='GAZ OXYGENE ACETYLENE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='PALPA-00',@Libelle='PALPLANCHES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='PAVDA-00',@Libelle='PIERRES, PAVES ET DALLES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='PHYTO-00',@Libelle='PRODUITS PHYTO SANITAIRES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='PREFA-00',@Libelle='ELEMENTS PREFABRIQUES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='TUYAU-03',@Libelle='TUYAU BÉTON ACHAT CONSO',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='TUYAU-04',@Libelle='TUYAU PVC ACHAT CONSO',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='TERRE-00',@Libelle='TERRE VEGETALE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='VEGET-00',@Libelle='VEGETAUX',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC01',@Libelle='LOCATION VU VL',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC02',@Libelle='LOCATION DE CAMION',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC03',@Libelle='LOCATION MATERIEL TERRASS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC04',@Libelle='LOC.MATL.LEVAGE & MANUT.',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC05',@Libelle='LOC.MATERIEL D''ENERGIE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC06',@Libelle='LOC.MATERIEL DE POMPAGE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC07',@Libelle='LOC.MATERIEL DE SONDAGE,',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC08',@Libelle='LOC.MATERIEL CONCASSAGE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC09',@Libelle='LOC.MATERIEL TOPOGRAPHIE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC10',@Libelle='LOC.COFFRAGES & ETAIEMENT',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC11',@Libelle='LOC.CONSTRUCTIONS MOBILES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC12',@Libelle='LOC.MATERIELS DIVERS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC13',@Libelle='LLD VU VL > 6 MOIS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC14',@Libelle='LLD MATERIEL >6MOIS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC15',@Libelle='LOC.DE MATERIEL DE BUREAU',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='FCONT-00',@Libelle='FRAIS D''ACTES ET CONTENTI',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='CTRLE-00',@Libelle='BUREAU DE CONTROLE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='ENTPR-00',@Libelle='PIECES DE RECHANGE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='ENTVH-00',@Libelle='ENTRETIEN ET REPARATIONS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='ENTRE-00',@Libelle='ENTRETIEN CONSTRUCTION',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='FESSA-00',@Libelle='PREST.LABO.-CONTR/ESSAIS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='FETUD-00',@Libelle='FRAIS D''ETUDES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='GARDI-00',@Libelle='GARDIENNAGE-SURVEILLANCE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='HONOR-30',@Libelle='CONSEILS FISCAUX & JURIDI',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='HONOR-09',@Libelle='CONSEILS & HONORAIRES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='ARCHI-00',@Libelle='ARCHITECTE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='MISSI-00',@Libelle='MISSIONS, RECEPTIONS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='HONOR-00',@Libelle='PILOTAGE,MANDATARIAT,GERA',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='PNEUM-00',@Libelle='PNEUMATIQUES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='FPOST-00',@Libelle='AFFRANCH, FRAIS POSTAUX',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='PAUTR-00',@Libelle='AUTRES PRESTATIONS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='TRDEC-01',@Libelle='REDEVANCE SUR DECHARGE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='STPAD-01',@Libelle='SS-TRAITANTS A PAIEMENT DIRECT',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='STPEN-02',@Libelle='SS-TRAITANTS A PAIEMENT ENTREP.',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='PTRSP-00',@Libelle='TRANSPORT SUR ACHAT',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='PCTVH-00',@Libelle='VISITE TECHNIQUE VEHICULE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FCOUR-00',@Libelle='COURSIERS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FABON-00',@Libelle='DOC.GENERALE  ABONNEMENTS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FREAU-00',@Libelle='EAU (UNITÉ=M3)',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FELEC-00',@Libelle='ELECTRICITE (UNITÉ=KWH)',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FBURO-00',@Libelle='FOURN.DE BUREAU',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FCADO-00',@Libelle='CADEAUX A LA CLIENTELE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FSEPI-00',@Libelle='EPI',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FREPR-00',@Libelle='REPRO.-PLANS ET TIRAGES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FTCOM-00',@Libelle='TELECOMMUNICATIONS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='RECYC-00',@Libelle='ACHAT DECHETS RECYCLES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='FOURN-00',@Libelle='FOURNITURES MAGASIN ',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='PEQUI-00',@Libelle='PETITS EQUIPEMENTS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='FOATU-00',@Libelle='FOURNITURES ATELIER USINE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='FTRSP-00',@Libelle='TRANSPORT SUR ACHAT',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='FACHA-00',@Libelle='FRAIS ACCESS. S/ACHAT',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='10-0143',@Code='MATPR-00',@Libelle='RRRO DE MAT PREMIERES ET FOURN',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='FETUD-00',@Libelle='RRRO ETUDES ET PRESTA DE SERVICES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='STAUT-00',@Libelle='SS TRAITANCE GENERALE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='20-0143',@Code='MAT-LOC99',@Libelle='LOC PONCTUELLES AUTRES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='ENTRI-00',@Libelle='ENTRETIEN BIENS IMMOBILIERS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='PERSD-00',@Libelle='PERSONNEL DETACHE OU PRETE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='30-0143',@Code='HONPA-00',@Libelle='HON. TECHNIQUES PADI',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FDIVS-00',@Libelle='FRAIS DIVERS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='ANNON-00',@Libelle='ANNONCES ET INSERTIONS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='DEPLA-00',@Libelle='VOYAGES ET DEPLACEMENTS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='RECEP-00',@Libelle='RECEPTIONS',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='DRMUT-00',@Libelle='DROITS DE MUTATION ET DE TIMBRES ',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='CHGEX-00',@Libelle='AUTRES CHARGES D''EXPLOITATION',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='40-0143',@Code='FRASS-00',@Libelle='FRANCHISES D''ASSURANCES',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='DIREC-00',@Libelle='DIRECTEUR',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='CONDU-00',@Libelle='CONDUCTEUR DE TRAVAUX',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='CCHAN-00',@Libelle='CHEF DE CHANTIER',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='ENSTA-00',@Libelle='STAGIAIRE ENCADREMENT',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='ENINT-00',@Libelle='INTÉRIM ENCADREMENT',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='CHAUF-00',@Libelle='CONDUCTEUR D''ENGIN',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='OUVRI-00',@Libelle='OUVRIER ',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PRDAP-00',@Libelle='APPRENTI PRODUCTION',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PRDST-00',@Libelle='STAGIAIRE PRODUCTION',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PRDIN-00',@Libelle='INTÉRIM PRODUCTION',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-1001',@Libelle='BERLINE CAT 1 CONDUCTEUR DE TRAVAUX',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-1301',@Libelle='FOURGON TOLÉ',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-1302',@Libelle='FOURGON BENNE PLATEAU',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-1506',@Libelle='CAMION BENNE 4 X 2 ET 4X4 GRUE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-1507',@Libelle='CAMION BENNE 6 X 4 ET 6X6 GRUE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-2203',@Libelle='PETITE REMORQUE TRANSPORT',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-2401',@Libelle='MINI PELLE < 3 T',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-2402',@Libelle='MINI PELLE >=3 < 5 T',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-2602',@Libelle='PELLE/PNEUS >=10 < 14 T',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-4802',@Libelle='CHARIOT ÉLÉVATEUR À MAT',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-4803',@Libelle='CHARIOT ÉLÉVATEUR TÉLESCOPIQUE',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='60-0143',@Code='MAT-1304',@Libelle='CHÂSSIS+BENNE, PLATEAU/FOURGON SIMPL CA',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0010',@Libelle='CONDUCTEUR DE TRAVAUX',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0011',@Libelle='CONDUCTEUR DE TRAVAUX Intérimaire',@Type='Matériel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0020',@Libelle='CHEF DE CHANTIER',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0021',@Libelle='CHEF DE CHANTIER Intérimaire',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0030',@Libelle='OUVRIER',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0031',@Libelle='OUVRIER Intérimaire',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0040',@Libelle='CONDUCTEUR ENGIN',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0041',@Libelle='CONDUCTEUR ENGIN Intérimaire',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0050',@Libelle='CHAUFFEUR CAMION',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0051',@Libelle='CHAUFFEUR CAMION Intérimaire',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0060',@Libelle='GÉOMÈTRE',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0061',@Libelle='GÉOMÈTRE Intérimaire',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0070',@Libelle='ELECTRICIEN',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0080',@Libelle='ATELIER',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0081',@Libelle='ATELIER Intérimaire',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0090',@Libelle='CHAUDRONNERIE',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0100',@Libelle='CHEF EQUIPE',@Type='Personnel'
EXEC Fred_ToolBox_Ressources @GroupeCode='GFTP',@SousChapitreCode='50-0143',@Code='PERS-0101',@Libelle='CHEF EQUIPE intérimaire',@Type='Personnel'





-- AJOUT DES NATURES
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60111010',@Libelle='Acier Achat Conso'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115013',@Libelle='Enrobés Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115012',@Libelle='Béton prêt emploi Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115017',@Libelle='Produit Naturel Conditionné Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115014',@Libelle='Produits métalliques manufacturés Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60614000',@Libelle='carburants & lubrifiants'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60614000',@Libelle='carburants & lubrifiants'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115011',@Libelle='Liants Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115011',@Libelle='Liants Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115013',@Libelle='Enrobés Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115011',@Libelle='Liants Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60641600',@Libelle='Fournitures informatiques'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115016',@Libelle='Produits plastique composite manufacturés Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115010',@Libelle='Granulats Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60680000',@Libelle='Autres matières et fournitures'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115016',@Libelle='Produits plastique composite manufacturés Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115014',@Libelle='Produits métalliques manufacturés Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115020',@Libelle='Tuyau béton Achat Conso'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115023',@Libelle='Pierres naturelles, dalles et pavés'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60631100',@Libelle='Fournitures d''entretien, petit outillage, petit équipement'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60612000',@Libelle='Oxygène et gaz soudure atelier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115014',@Libelle='Produits métalliques manufacturés Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115023',@Libelle='Pierres naturelles, dalles et pavés'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115019',@Libelle='Espaces Verts Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115015',@Libelle='Produits béton manufacturés Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115020',@Libelle='Tuyau béton Achat Conso'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115022',@Libelle='Tuyau PVC Achat Conso'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115019',@Libelle='Espaces Verts Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115019',@Libelle='Espaces Verts Achat'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61353000',@Libelle='Location véhicules'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61356000',@Libelle='Location engins de chantier'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61351000',@Libelle='Locations ponctuelles'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61355000',@Libelle='Locations longue durée > 6 mois'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61355000',@Libelle='Locations longue durée > 6 mois'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61351000',@Libelle='Locations ponctuelles'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62270000',@Libelle='Frais d''actes et de contentieux'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62265400',@Libelle='Honoraires techniques'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61551600',@Libelle='Entretien matériel & outillage Industriel'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61552000',@Libelle='Entretien des véhicules'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61561000',@Libelle='Maintenance'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60400000',@Libelle='Achats d''études et prestations de services'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60400000',@Libelle='Achats d''études et prestations de services'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60400000',@Libelle='Achats d''études et prestations de services'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62263000',@Libelle='Honoraires conseils juridiques'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62265400',@Libelle='Honoraires techniques'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62265400',@Libelle='Honoraires techniques'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62560000',@Libelle='Missions'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62260000',@Libelle='Honoraires divers'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61552000',@Libelle='Entretien des véhicules'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62610000',@Libelle='Affranchissements'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60400000',@Libelle='Achats d''études et prestations de services'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60530000',@Libelle='Retraitement de déchets'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60500405',@Libelle='sous-traitance travaux chantiers PADI'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60500400',@Libelle='sous-traitance travaux chantiers'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62410000',@Libelle='Transports sur achats'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61551600',@Libelle='Entretien matériel & outillage Industriel'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62881000',@Libelle='Frais divers'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61810000',@Libelle='Documentation générale'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60611200',@Libelle='eau'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60613000',@Libelle='électricité'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60641000',@Libelle='fournitures bureau'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62340000',@Libelle='Cadeaux à la clientèle'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60634000',@Libelle='vêtements de travail et  de sécurité'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60400000',@Libelle='Achats d''études et prestations de services'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62620000',@Libelle='Téléphone'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60115018',@Libelle='VALORISATION (ACHAT DÉCHETS RECYCLÉS) ACHAT'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60633000',@Libelle='FOURNITURE MAGASIN N/STOCKE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60681000',@Libelle='PETITS ÉQUIPEMENTS'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60682000',@Libelle='FOURNITURES ATELIER USINE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60810000',@Libelle='TRANSPORT S/ACHAT'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60820000',@Libelle='FRAIS ACCESS.S/ACHATS'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60910000',@Libelle='RRRO DE MATIÈRES PREMIÈRES ET FOURNITURES'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='60940000',@Libelle='RRRO ÉTUDES ET PRESTATIONS DE SERVICES'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61120000',@Libelle='SOUS TRAITANCE GÉNÉRALE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61350000',@Libelle='LOCATIONS PONCTUELLES AUTRES'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='61520000',@Libelle='ENTRETIEN BIENS IMMOBILIERS'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62140000',@Libelle='PERSONNEL DÉTACHÉ OU PRÊTÉ À L''ENTREPRISE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62265405',@Libelle='HONORAIRES TECHNIQUES PADI'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62280000',@Libelle='FRAIS DIVERS'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62310000',@Libelle='ANNONCES ET INSERTIONS'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62510000',@Libelle='VOYAGES ET DÉPLACEMENTS'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='62570000',@Libelle='RÉCEPTIONS'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='63540000',@Libelle='DROITS DE MUTATION & DE TIMBRES'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='65800000',@Libelle='AUTRES CHARGES D''EXPLOITATION'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='65820000',@Libelle='FRANCHISE D''ASSURANCES'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='DIRECTEUR'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='CONDUCTEUR DE TRAVAUX'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='CHEF DE CHANTIER'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='STAGIAIRE ENCADREMENT'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='INTÉRIM ENCADREMENT'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='CONDUCTEUR D''ENGIN'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='OUVRIER '
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='APPRENTI PRODUCTION'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='STAGIAIRE PRODUCTION'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='64110000',@Libelle='INTÉRIM PRODUCTION'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95101001',@Libelle='BERLINE CAT 1 CONDUCTEUR DE TRAVAUX'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95101301',@Libelle='FOURGON TOLÉ'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95101302',@Libelle='FOURGON BENNE PLATEAU'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95101506',@Libelle='CAMION BENNE 4 X 2 ET 4X4 GRUE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95101507',@Libelle='CAMION BENNE 6 X 4 ET 6X6 GRUE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95102203',@Libelle='PETITE REMORQUE TRANSPORT'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95102401',@Libelle='MINI PELLE < 3 T'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95102402',@Libelle='MINI PELLE >=3 < 5 T'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95102602',@Libelle='PELLE/PNEUS >=10 < 14 T'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95104802',@Libelle='CHARIOT ÉLÉVATEUR À MAT'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95104803',@Libelle='CHARIOT ÉLÉVATEUR TÉLESCOPIQUE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='95101304',@Libelle='CHÂSSIS+BENNE, PLATEAU/FOURGON SIMPL CA'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350010',@Libelle='CONDUCTEUR DE TRAVAUX'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350011',@Libelle='CONDUCTEUR DE TRAVAUX Intérimaire'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350020',@Libelle='CHEF DE CHANTIER'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350021',@Libelle='CHEF DE CHANTIER Intérimaire'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350030',@Libelle='OUVRIER'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350031',@Libelle='OUVRIER Intérimaire'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350040',@Libelle='CONDUCTEUR ENGIN'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350041',@Libelle='CONDUCTEUR ENGIN Intérimaire'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350050',@Libelle='CHAUFFEUR CAMION'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350051',@Libelle='CHAUFFEUR CAMION Intérimaire'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350060',@Libelle='GÉOMÈTRE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350061',@Libelle='GÉOMÈTRE Intérimaire'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350070',@Libelle='ELECTRICIEN'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350080',@Libelle='ATELIER'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350081',@Libelle='ATELIER Intérimaire'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350090',@Libelle='CHAUDRONNERIE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350100',@Libelle='CHEF EQUIPE'
EXEC Fred_ToolBox_Nature @GroupeCode='GFTP',@SocieteCode='0143',@Code='94350101',@Libelle='CHEF EQUIPE intérimaire'



-- AJOUT DES NATURES-SOCIETE
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='ARMAT-00',@NatureCode='60111010'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='ASPBI-00',@NatureCode='60115013'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='BETON-00',@NatureCode='60115012'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='COFFR-00',@NatureCode='60115017'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='BOICO-00',@NatureCode='60115014'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='CARBU-00',@NatureCode='60614000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='LUBRI-00',@NatureCode='60614000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='CHAUX-00',@NatureCode='60115011'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='CIMEN-00',@NatureCode='60115011'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='ENROB-00',@NatureCode='60115013'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='EMULS-00',@NatureCode='60115011'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='INFOR-00',@NatureCode='60641600'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='GEOCO-00',@NatureCode='60115016'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='GRANU-00',@NatureCode='60115010'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='TOPOG-00',@NatureCode='60680000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MCOMP-00',@NatureCode='60115016'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='METAL-00',@NatureCode='60115014'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='METAL-00',@NatureCode='60115020'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MOBIL-00',@NatureCode='60115023'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='OUTIL-00',@NatureCode='60631100'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='OXGAZ-00',@NatureCode='60612000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='PALPA-00',@NatureCode='60115014'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='PAVDA-00',@NatureCode='60115023'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='PHYTO-00',@NatureCode='60115019'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='PREFA-00',@NatureCode='60115015'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='TUYAU-03',@NatureCode='60115020'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='TUYAU-04',@NatureCode='60115022'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='TERRE-00',@NatureCode='60115019'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='VEGET-00',@NatureCode='60115019'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC01',@NatureCode='61353000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC02',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC03',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC04',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC05',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC06',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC07',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC08',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC09',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC10',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC11',@NatureCode='61356000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC12',@NatureCode='61351000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC13',@NatureCode='61355000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC14',@NatureCode='61355000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MAT-LOC15',@NatureCode='61351000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FCONT-00',@NatureCode='62270000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='CTRLE-00',@NatureCode='62265400'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='ENTPR-00',@NatureCode='61551600'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='ENTVH-00',@NatureCode='61552000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='ENTRE-00',@NatureCode='61561000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FESSA-00',@NatureCode='60400000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FETUD-00',@NatureCode='60400000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='GARDI-00',@NatureCode='60400000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='HONOR-30',@NatureCode='62263000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='HONOR-09',@NatureCode='62265400'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='ARCHI-00',@NatureCode='62265400'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='MISSI-00',@NatureCode='62560000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='HONOR-00',@NatureCode='62260000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='PNEUM-00',@NatureCode='61552000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FPOST-00',@NatureCode='62610000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='PAUTR-00',@NatureCode='60400000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='TRDEC-01',@NatureCode='60530000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='STPAD-01',@NatureCode='60500405'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='STPEN-02',@NatureCode='60500400'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='PTRSP-00',@NatureCode='62410000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='PCTVH-00',@NatureCode='61551600'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FCOUR-00',@NatureCode='62881000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FABON-00',@NatureCode='61810000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FREAU-00',@NatureCode='60611200'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FELEC-00',@NatureCode='60613000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FBURO-00',@NatureCode='60641000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FCADO-00',@NatureCode='62340000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FSEPI-00',@NatureCode='60634000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FREPR-00',@NatureCode='60400000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Oui',@RessourceCode='FTCOM-00',@NatureCode='62620000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='RECYC-00',@NatureCode='60115018'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='FOURN-00',@NatureCode='60633000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PEQUI-00',@NatureCode='60681000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='FOATU-00',@NatureCode='60682000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='FTRSP-00',@NatureCode='60810000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='FACHA-00',@NatureCode='60820000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MATPR-00',@NatureCode='60910000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='FETUD-00',@NatureCode='60940000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='STAUT-00',@NatureCode='61120000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-LOC99',@NatureCode='61350000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='ENTRI-00',@NatureCode='61520000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERSD-00',@NatureCode='62140000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='HONPA-00',@NatureCode='62265405'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='FDIVS-00',@NatureCode='62280000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='ANNON-00',@NatureCode='62310000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='DEPLA-00',@NatureCode='62510000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='RECEP-00',@NatureCode='62570000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='DRMUT-00',@NatureCode='63540000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='CHGEX-00',@NatureCode='65800000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='FRASS-00',@NatureCode='65820000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='DIREC-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='CONDU-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='CCHAN-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='ENSTA-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='ENINT-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='CHAUF-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='OUVRI-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PRDAP-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PRDST-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PRDIN-00',@NatureCode='64110000'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-1001',@NatureCode='95101001'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-1301',@NatureCode='95101301'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-1302',@NatureCode='95101302'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-1506',@NatureCode='95101506'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-1507',@NatureCode='95101507'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-2203',@NatureCode='95102203'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-2401',@NatureCode='95102401'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-2402',@NatureCode='95102402'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-2602',@NatureCode='95102602'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-4802',@NatureCode='95104802'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-4803',@NatureCode='95104803'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='MAT-1304',@NatureCode='95101304'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0010',@NatureCode='94350010'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0011',@NatureCode='94350011'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0020',@NatureCode='94350020'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0021',@NatureCode='94350021'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0030',@NatureCode='94350030'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0031',@NatureCode='94350031'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0040',@NatureCode='94350040'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0041',@NatureCode='94350041'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0050',@NatureCode='94350050'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0051',@NatureCode='94350051'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0060',@NatureCode='94350060'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0061',@NatureCode='94350061'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0070',@NatureCode='94350070'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0080',@NatureCode='94350080'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0081',@NatureCode='94350081'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0090',@NatureCode='94350090'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0100',@NatureCode='94350100'
EXEC Fred_Toolbox_Nature_Ressources @GroupeCode='GFTP',@SocieteCode='0143',@Achats='Non',@RessourceCode='PERS-0101',@NatureCode='94350101'




-- CHANGEMENT DES RESSOURCES
-- POSITIONNEMENT TOUS PERSONNELS SUR 
-- RESSOURCES OUVRIER
DECLARE @_RessourceID INT;
SET @_RessourceID = 
							(
							SELECT FRED_RESSOURCE.RessourceId
							FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE
							WHERE FRED_SOUS_CHAPITRE.SousChapitreId IN 
								(
									SELECT FRED_SOUS_CHAPITRE.SousChapitreId 
									FROM FRED_SOUS_CHAPITRE, FRED_CHAPITRE 
									WHERE FRED_CHAPITRE.ChapitreId IN 
										(
										SELECT ChapitreId FROM FRED_CHAPITRE WHERE GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GFTP'
										)
								AND FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId)
								AND FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
								)
								
							AND FRED_RESSOURCE.Code = 'PERS-0030'
							)
UPDATE FRED_PERSONNEL SET RessourceId = @_RessourceID WHERE  SocieteId = (SELECT SocieteId FROM FRED_SOCIETE WHERE code = '0143' AND GroupeId= (SELECT GroupeId FROM FRED_GROUPE where Code = 'GFTP'))

