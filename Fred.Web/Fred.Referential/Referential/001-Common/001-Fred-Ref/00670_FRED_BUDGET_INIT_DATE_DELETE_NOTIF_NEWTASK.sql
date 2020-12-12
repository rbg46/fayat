-- =======================================================================================================================================
-- Author:		Yannick DEFAY  13/02/2019
--
-- Description:
--      - Initialise la date de suppression de la notification de nouvelle tâche avec la date de création du budget
--
-- =======================================================================================================================================

BEGIN TRAN

DECLARE @budgetId INT

DECLARE db_cursor CURSOR FOR
    SELECT BudgetId
    FROM FRED_BUDGET 
    WHERE DateDeleteNotificationNewTask IS NULL
OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @budgetId

WHILE @@FETCH_STATUS = 0  
BEGIN  
	DECLARE @dateCreation DATETIME = (SELECT TOP(1) [Date] FROM FRED_BUDGET_WORKFLOW WHERE BudgetId = @budgetId AND EtatInitialId IS NULL);

    IF (@dateCreation IS NOT NULL)
    BEGIN
        UPDATE FRED_BUDGET SET DateDeleteNotificationNewTask = @dateCreation WHERE BudgetId = @budgetId;
    END

	FETCH NEXT FROM db_cursor INTO @budgetId  
END 

CLOSE db_cursor  
DEALLOCATE db_cursor

COMMIT TRAN
