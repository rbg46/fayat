IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='budgetSousDetailFormule')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'budgetSousDetailFormule', 1, GetDate(), N'super_fred', 6)    
END