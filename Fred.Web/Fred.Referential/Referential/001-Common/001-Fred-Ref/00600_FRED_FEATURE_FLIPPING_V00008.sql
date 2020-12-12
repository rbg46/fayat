UPDATE FRED_FEATURE_FLIPPING SET Name='budgetIntegrationFormules' WHERE Name ='budgetSousDetailFormule'
IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='budgetIntegrationFormules')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'budgetIntegrationFormules', 1, GetDate(), N'super_fred', 6)    
END