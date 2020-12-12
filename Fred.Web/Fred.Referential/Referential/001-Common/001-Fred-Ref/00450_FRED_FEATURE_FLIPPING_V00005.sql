IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='triTableauExplorateurDepenses')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'triTableauExplorateurDepenses', 1, GetDate(), N'super_fred', 5)    
END