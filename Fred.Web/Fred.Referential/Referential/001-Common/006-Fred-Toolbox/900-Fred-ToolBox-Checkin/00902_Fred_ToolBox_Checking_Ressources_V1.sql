

-- --------------------------------------------------
-- FRED 2017 - R4 - SEPTEMBRE 2018 
-- TOOLBOX Controle des organisations
-- MODOP : 
--    EXEC Fred_ToolBox_Check_Ressources
-- --------------------------------------------------


/*PROCEDURE STOCKEES*/
IF OBJECT_ID ( 'Fred_ToolBox_Check_Ressources', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_Check_Ressources;  
GO  
CREATE PROCEDURE Fred_ToolBox_Check_Ressources   

AS   
	-- BEGIN : LOOP GROUPE
	DECLARE @GroupeId INT;
	DECLARE @Code varchar(max);
	DECLARE @Libelle varchar(max);
	Declare A Cursor For SELECT GroupeId, Code ,Libelle FROM FRED_GROUPE
	Open A
	Fetch next From A into @GroupeId, @Code, @Libelle
	While @@Fetch_Status=0 Begin
   
	   PRINT 'GRP->' + @Code + '/' + @Libelle
   
			-- BEGIN : LOOP CHAPITRE
			DECLARE @ChapitreId INT;
			DECLARE @ChapitreCode varchar(20);
			DECLARE @ChapitreLibelle varchar(20);
			Declare B Cursor For SELECT ChapitreId, Code ,Libelle FROM FRED_CHAPITRE WHERE GroupeId = @GroupeId
			Open B
			Fetch next From B into @ChapitreId, @ChapitreCode, @ChapitreLibelle
			While @@Fetch_Status=0 Begin
   

				PRINT '    R1->' + @ChapitreCode + '/' + @ChapitreLibelle


				-- BEGIN : LOOP SOUS_CHAPITRE
				DECLARE @SousChapitreId INT;
				DECLARE @SousChapitreCode varchar(20);
				DECLARE @SousChapitreLibelle varchar(20);
				Declare C Cursor For SELECT SousChapitreId, Code ,Libelle FROM FRED_SOUS_CHAPITRE WHERE ChapitreId = @ChapitreId
				Open C
				Fetch next From C into @SousChapitreId, @SousChapitreCode, @SousChapitreLibelle
				While @@Fetch_Status=0 Begin
   

					PRINT '        R2->' + @SousChapitreCode + '/' + @SousChapitreLibelle
	

								-- BEGIN : LOOP  RESSOURCES
								DECLARE @RessourcesId INT;
								DECLARE @RessourcesCode varchar(20);
								DECLARE @RessourcesLibelle varchar(max);
								DECLARE @TypeRessource varchar(20);
								Declare D Cursor For SELECT FRED_RESSOURCE.RessourceId, FRED_RESSOURCE.Code ,FRED_RESSOURCE.Libelle, FRED_TYPE_RESSOURCE.Libelle FROM FRED_RESSOURCE, FRED_TYPE_RESSOURCE WHERE SousChapitreId = @SousChapitreId AND FRED_RESSOURCE.TypeRessourceId = FRED_TYPE_RESSOURCE.TypeRessourceId
								Open D
								Fetch next From D into @RessourcesId, @RessourcesCode, @RessourcesLibelle, @TypeRessource
								While @@Fetch_Status=0 Begin
   

									PRINT '                R3->' + @RessourcesCode + '/' + @RessourcesLibelle + '/' + @TypeRessource + '/ ID #' + CAST(@RessourcesId AS VARCHAR(50))


								   Fetch next From D into @RessourcesId, @RessourcesCode, @RessourcesLibelle, @TypeRessource
								End
								Close D
								Deallocate D
								-- END : LOOP RESSOURCES




				   Fetch next From C into @SousChapitreId, @SousChapitreCode, @SousChapitreLibelle
				End
				Close C
				Deallocate C
				-- END : LOOP CHAPITRE




			   Fetch next From B into @ChapitreId, @ChapitreCode, @ChapitreLibelle
			End
			Close B
			Deallocate B
			-- END : LOOP CHAPITRE



	   Fetch next From A into @GroupeId, @Code, @Libelle
	End
	Close A
	Deallocate A
	-- END : LOOP GROUPE
GO 








-- --------------------------------------------------
-- FRED 2017 - R4 - SEPTEMBRE 2018 
-- TOOLBOX Controle des organisations
-- MODOP : 
--    EXEC Fred_ToolBox_Check_Ressources_ByCode @GroupeCode ='GFTP', @Code = 'ENCA-01'
--    EXEC Fred_ToolBox_Check_Ressources_ByCode  @Code = 'ENCA-01'
-- --------------------------------------------------

/*PROCEDURE STOCKEES*/
IF OBJECT_ID ( 'Fred_ToolBox_Check_Ressources_ByCode', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_Check_Ressources_ByCode;  
GO  
CREATE PROCEDURE Fred_ToolBox_Check_Ressources_ByCode   
		@GroupeCode varchar(500) =NULL,
		@Code varchar(500)
AS   
IF @GroupeCode IS NOT NULL

	BEGIN
	PRINT 'RECHERCHE Avec CODE Groupe'
		SELECT	FRED_CHAPITRE.ChapitreID AS R1_ID,
			FRED_CHAPITRE.Code AS R1_CODE, 
			FRED_CHAPITRE.Libelle AS R1_LIBELLE, 
			FRED_SOUS_CHAPITRE.SousChapitreId AS R2_ID,
			FRED_SOUS_CHAPITRE.Code AS R2_CODE, 
			FRED_SOUS_CHAPITRE.Libelle AS R2_LIBELLE, 
			FRED_RESSOURCE.RessourceId AS R3_ID,
			FRED_RESSOURCE.Code AS R3_CODE, 
			FRED_RESSOURCE.Libelle AS R3_LIBELLE
		FROM FRED_CHAPITRE, FRED_SOUS_CHAPITRE, FRED_RESSOURCE 
		WHERE FRED_RESSOURCE.Code = @Code
		AND FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
		AND FRED_SOUS_CHAPITRE.ChapitreId = FREd_CHAPITRE.ChapitreId
		AND FRED_CHAPITRE.GroupeId =  (SELECT GroupeId FROM FRED_GROUPE Where code = @GroupeCode)
	END
ELSE

	BEGIN
	PRINT 'RECHERCHE SANS CODE Groupe'
			SELECT	FRED_CHAPITRE.ChapitreID AS R1_ID,
			FRED_CHAPITRE.Code AS R1_CODE, 
			FRED_CHAPITRE.Libelle AS R1_LIBELLE, 
			FRED_SOUS_CHAPITRE.SousChapitreId AS R2_ID,
			FRED_SOUS_CHAPITRE.Code AS R2_CODE, 
			FRED_SOUS_CHAPITRE.Libelle AS R2_LIBELLE, 
			FRED_RESSOURCE.RessourceId AS R3_ID,
			FRED_RESSOURCE.Code AS R3_CODE, 
			FRED_RESSOURCE.Libelle AS R3_LIBELLE
		FROM FRED_CHAPITRE, FRED_SOUS_CHAPITRE, FRED_RESSOURCE 
		WHERE FRED_RESSOURCE.Code = @Code
		AND FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
		AND FRED_SOUS_CHAPITRE.ChapitreId = FREd_CHAPITRE.ChapitreId
		
	END


GO 

