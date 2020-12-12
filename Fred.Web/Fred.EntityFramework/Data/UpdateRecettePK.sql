BEGIN TRAN
    
    DECLARE @elements INT = 0;

    SET @elements  =
        (SELECT COUNT(*) 
	    FROM FRED_BUDGET B, FRED_BUDGET_RECETTE R 
	    WHERE B.BudgetRecetteId = R.BudgetRecetteId)

    IF (@elements > 0)
        BEGIN
            CREATE TABLE #TempBudgetRecetteUpatePK            
	        (            
		        Bugdet_id	int,             
		        Recette_id int,
		        MontantMarche decimal(18,2),
		        MontantAvenants decimal(18,2),
		        SommeAValoir decimal(18,2),
		        TravauxSup decimal(18,2),
		        Revision decimal(18,2),
		        AutresRecettes decimal(18,2),
		        PenalitesEtRetenues decimal(18,2)   
	        )  

	        INSERT INTO #TempBudgetRecetteUpatePK
		        SELECT B.BudgetId, R.*  
		        FROM FRED_BUDGET B, FRED_BUDGET_RECETTE R 
		        WHERE B.BudgetRecetteId = R.BudgetRecetteId

	        UPDATE FRED_BUDGET
	        SET BudgetRecetteId = NULL
	        WHERE BudgetRecetteId IS NOT NULL

	        DELETE FRED_BUDGET_RECETTE

	        DECLARE @budgetId INT
	        DECLARE @recetteId INT
	        DECLARE @montantMarche DECIMAL(18,2)
	        DECLARE @montantAvenants DECIMAL(18,2)
	        DECLARE @sommeAValoir DECIMAL(18,2)
	        DECLARE @travauxSup DECIMAL(18,2)
	        DECLARE @revision DECIMAL(18,2)
	        DECLARE @autresRecettes DECIMAL(18,2)
	        DECLARE @penalitesEtRetenues DECIMAL(18,2)

	        DECLARE budget_recette_link_cursor CURSOR FOR
		        SELECT *
		        FROM #TempBudgetRecetteUpatePK
	        OPEN budget_recette_link_cursor;
	        FETCH NEXT FROM budget_recette_link_cursor INTO @budgetId, @recetteId, @montantMarche, @montantAvenants, @sommeAValoir, @travauxSup, @revision, @autresRecettes, @penalitesEtRetenues;
	        SET IDENTITY_INSERT FRED_BUDGET_RECETTE ON
	        WHILE @@FETCH_STATUS = 0 BEGIN

		        INSERT INTO FRED_BUDGET_RECETTE 
			        (BudgetRecetteId, MontantMarche, MontantAvenants, SommeAValoir, TravauxSupplementaires, Revision, AutresRecettes, PenalitesEtRetenues) 
		        VALUES 
			        (@budgetId, @montantMarche, @montantAvenants, @sommeAValoir, @travauxSup, @revision, @autresRecettes, @penalitesEtRetenues)

		        FETCH NEXT FROM budget_recette_link_cursor INTO @budgetId, @recetteId, @montantMarche, @montantAvenants, @sommeAValoir, @travauxSup, @revision, @autresRecettes, @penalitesEtRetenues;
	        END
	        SET IDENTITY_INSERT FRED_BUDGET_RECETTE OFF
	        CLOSE budget_recette_link_cursor;
	        DEALLOCATE budget_recette_link_cursor;

	        IF(OBJECT_ID('tempdb.dbo.#TempBudgetRecetteUpatePK') Is Not Null)
	        BEGIN
		        DROP TABLE #TempBudgetRecetteUpatePK
	        END
        END

COMMIT TRAN